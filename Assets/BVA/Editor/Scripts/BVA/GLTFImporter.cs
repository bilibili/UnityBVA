using ADBRuntime;
using ADBRuntime.Mono;
using GLTF;
using GLTF.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using BVA.Extensions;
using BVA.Loader;
using BVA.Component;
using Object = UnityEngine.Object;

namespace BVA
{
    public enum GLTFImporterNormals
    {
        Import,
        Calculate,
        None
    }
    [UnityEditor.AssetImporters.ScriptedImporter(1, new[] { "bva", "glb", "gltf" })]
    public class GLTFImporter : UnityEditor.AssetImporters.ScriptedImporter
    {
        [SerializeField] private bool _removeEmptyRootObjects = true;
        [SerializeField] private float _scaleFactor = 1.0f;
        [SerializeField] private bool _readWriteEnabled = true;
        [SerializeField] private bool _swapUvs = false;
        [SerializeField] private GLTFImporterNormals _importNormals = GLTFImporterNormals.Import;
        [SerializeField] private bool _importMaterials = true;
        [SerializeField] private bool _importAnimations = true;
        [SerializeField] private bool _importAudio = true;
        [SerializeField] private bool _importAvatars = true;
        [SerializeField] private bool _importSkybox = true;
        [SerializeField] private bool _importPuretsScript = true;
        private static string folderName;
        private static string assetName;
        public static string GetImportDir(string catalog)
        {
            return string.Concat(folderName, "/", assetName, "/", $"{catalog}/");
        }
        public static string ImportTextureRoot => GetImportDir("Textures");
        public static string ImportMetaRoot => GetImportDir("Meta");
        public static string ImportAudioRoot => GetImportDir("Audios");
        public static string ImportMaterialRoot => GetImportDir("Materials");
        public static string ImportPostProcessRoot => GetImportDir("PostProcess");
        public static string ImportPostProcessLutRoot => GetImportDir("PostProcess/Lut");

        public static string ImportDynamicBoneRoot => GetImportDir("DynamicBoneSettings");
        public static string ImportPureScriptRoot => GetImportDir("PureScripts");
        public static void ImportTexture(List<Texture2D> textures, string texturesRoot)
        {
            if (textures.Count > 0)
            {
                Directory.CreateDirectory(texturesRoot);

                foreach (var tex in textures)
                {
                    if (tex == null)
                        continue;
                    if (tex.isReadable == false)
                    {
                        Debug.LogWarning($"Texture {tex.name} is not readable!");
                        continue;
                    }
                    var ext = ".png";
                    var texPath = string.Concat(texturesRoot, tex.name, ext);
                    File.WriteAllBytes(texPath, tex.EncodeToPNG());
                    AssetDatabase.ImportAsset(texPath);
                }
            }
        }
        public static void ImportCubemap(List<Texture2D> textures, string texturesRoot)
        {
            if (textures.Count > 0)
            {
                Directory.CreateDirectory(texturesRoot);

                foreach (var tex in textures)
                {
                    if (tex == null)
                        continue;
                    var ext = ".png";
                    var texPath = string.Concat(texturesRoot, tex.name, ext);
                    File.WriteAllBytes(texPath, tex.EncodeToPNG());
                    AssetDatabase.ImportAsset(texPath);
                }
            }
        }
        public static void ImportCubemap(List<Cubemap> cubemaps, string texturesRoot)
        {
            if (cubemaps.Count > 0)
            {
                Directory.CreateDirectory(texturesRoot);

                foreach (var cubemap in cubemaps)
                {
                    if (cubemap == null)
                        continue;
                    Texture2D tex = cubemap.FlatTexture();
                    var ext = ".png";
                    var texPath = string.Concat(texturesRoot, tex.name, ext);
                    File.WriteAllBytes(texPath, tex.EncodeToPNG());
                    AssetDatabase.ImportAsset(texPath);
                }
            }
        }
        private string GetImagePath(string path)
        {
            return string.Concat(path, ".png");
        }
        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
        {
            string assetPath = ctx.assetPath;
            string sceneName = null;
            GameObject gltfScene = null;
            List<Mesh> meshes = null;
            try
            {
                sceneName = Path.GetFileNameWithoutExtension(ctx.assetPath);
                GLTFSceneImporter sceneImporter = CreateGLTFScene(ctx.assetPath);
                gltfScene = sceneImporter.LastLoadedScene;
                assetName = Path.GetFileNameWithoutExtension(ctx.assetPath);
                folderName = Path.GetDirectoryName(ctx.assetPath);

                // Remove empty roots
                if (_removeEmptyRootObjects)
                {
                    var t = gltfScene.transform;
                    while (gltfScene.transform.childCount == 1 && gltfScene.GetComponents<UnityEngine.Component>().Length == 1)
                    {
                        var parent = gltfScene;
                        gltfScene = gltfScene.transform.GetChild(0).gameObject;
                        t = gltfScene.transform;
                        t.parent = null; // To keep transform information in the new parent
                        DestroyImmediate(parent); // Get rid of the parent
                    }
                }

                // Ensure there are no hide flags present (will cause problems when saving)
                gltfScene.hideFlags &= ~(HideFlags.HideAndDontSave);
                foreach (Transform child in gltfScene.transform)
                {
                    child.gameObject.hideFlags &= ~(HideFlags.HideAndDontSave);
                }

                // Zero position
                gltfScene.transform.position = Vector3.zero;

                // Get meshes
                var meshNames = new List<string>();
                var meshHash = new HashSet<Mesh>();
                var meshFilters = gltfScene.GetComponentsInChildren<MeshFilter>();
                var vertexBuffer = new List<Vector3>();

                meshes = meshFilters.Select(mf =>
                {
                    var mesh = mf.sharedMesh;
                    vertexBuffer.Clear();
                    mesh.GetVertices(vertexBuffer);
                    for (var i = 0; i < vertexBuffer.Count; ++i)
                    {
                        vertexBuffer[i] *= _scaleFactor;
                    }
                    mesh.SetVertices(vertexBuffer);
                    if (_swapUvs)
                    {
                        var uv = mesh.uv;
                        var uv2 = mesh.uv2;
                        mesh.uv = uv2;
                        mesh.uv2 = uv2;
                    }
                    if (_importNormals == GLTFImporterNormals.None)
                    {
                        mesh.normals = new Vector3[0];
                    }
                    if (_importNormals == GLTFImporterNormals.Calculate && mesh.GetTopology(0) == MeshTopology.Triangles)
                    {
                        mesh.RecalculateNormals();
                    }
                    mesh.UploadMeshData(!_readWriteEnabled);

                    if (meshHash.Add(mesh))
                    {
                        var meshName = string.IsNullOrEmpty(mesh.name) ? mf.gameObject.name : mesh.name;
                        mesh.name = ObjectNames.GetUniqueName(meshNames.ToArray(), meshName);
                        meshNames.Add(mesh.name);
                    }

                    return mesh;
                }).ToList();


                var skinMeshRenderers = gltfScene.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var smr in skinMeshRenderers)
                {
                    var mesh = smr.sharedMesh;
                    if (meshHash.Add(mesh))
                    {
                        var meshName = string.IsNullOrEmpty(mesh.name) ? smr.gameObject.name : mesh.name;
                        mesh.name = ObjectNames.GetUniqueName(meshNames.ToArray(), meshName);
                        meshNames.Add(mesh.name);
                    }
                    meshes.Add(mesh);
                }

                if (_importAvatars)
                {
                    var animators = gltfScene.GetComponentsInChildren<Animator>();
                    var avatars = new List<Avatar>();
                    foreach (var animator in animators)
                    {
                        if (animator.avatar != null && animator.avatar.isValid && animator.avatar.isHuman)
                        {
                            if (avatars.Contains(animator.avatar)) continue;
                            avatars.Add(animator.avatar);
                        }
                    }
                    foreach (var avatar in avatars)
                    {
                        ctx.AddObjectToAsset("Avatars " + avatar.name, avatar);
                    }
                }

                if (_importAnimations)
                {
                    var animations = gltfScene.GetComponentsInChildren<Animation>();
                    // Get animationClips
                    var animationClipNames = new List<string>();
                    var animationClipHash = new HashSet<AnimationClip>();
                    var clips = animations.SelectMany(r =>
                    {
                        return r.GetAnimationClips().Where(clip =>
                        {
                            if (animationClipHash.Add(clip))
                            {
                                var clipName = clip.name;
                                clipName = clipName.Substring(Mathf.Min(clipName.LastIndexOf("/") + 1, clipName.Length - 1));
                                // Ensure name is unique
                                clipName = string.Format("{0}_{1}", sceneName, ObjectNames.NicifyVariableName(clipName));
                                clipName = ObjectNames.GetUniqueName(animationClipNames.ToArray(), clipName);

                                clip.name = clipName;
                                animationClipNames.Add(clipName);
                            }

                            return clip;
                        });
                    }).ToList();
                    foreach (var clip in clips)
                    {
                        ctx.AddObjectToAsset("animationClips " + clip.name, clip);
                    }
                    if (sceneImporter.AssetManager.motionLoader.animatorInfo != null)
                    {
                        List<AnimationClip> uniqueList = new List<AnimationClip>();
                        foreach (var clip in sceneImporter.AssetManager.motionLoader.animatorInfo)
                        {
                            foreach (var v in clip.Value.animationClips)
                            {
                                if (!uniqueList.Contains(v.motion))
                                {
                                    ctx.AddObjectToAsset("animationClips " + v.name, v.motion);
                                    uniqueList.Add(v.motion);
                                }
                            }
                        }
                    }
                }

                var reflectionProbes = gltfScene.GetComponentsInChildren<ReflectionProbe>();
                if (reflectionProbes.Length > 0)
                {
                    var cubemapRoot = ImportTextureRoot;
                    Directory.CreateDirectory(cubemapRoot);
                    List<Texture2D> textures = new List<Texture2D>();
                    List<string> textureNames = new List<string>();
                    for (int i = 0; i < reflectionProbes.Length; i++)
                    {
                        ReflectionProbe reflectionProbe = reflectionProbes[i];
                        Cubemap cubemap = reflectionProbe.customBakedTexture as Cubemap;

                        if (cubemap != null)
                        {
                            string texName = cubemap.name;
                            texName = ObjectNames.NicifyVariableName(texName);
                            texName = ObjectNames.GetUniqueName(textureNames.ToArray(), texName);
                            cubemap.name = texName;
                            textureNames.Add(texName);

                            textures.Add(CubemapExtensions.FlatTexture(cubemap));
                        }
                    }

                    ImportCubemap(textures, cubemapRoot);

                    EditorApplication.delayCall += () =>
                    {
                        gltfScene = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                        var reflectionProbes = gltfScene.GetComponentsInChildren<ReflectionProbe>();

                        var texturesRoot = ImportTextureRoot;
                        for (int i = 0; i < textureNames.Count; ++i)
                        {
                            var path = string.Concat(texturesRoot, textureNames[i]);
                            path = GetImagePath(path);
                            var importer = (TextureImporter)TextureImporter.GetAtPath(path);
                            if (importer == null) continue;
                            importer.textureShape = TextureImporterShape.TextureCube;
                            importer.mipmapEnabled = false;
                            importer.sRGBTexture = true;
                            importer.SaveAndReimport();
                            reflectionProbes[i].customBakedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                        }
                    };
                }

                var audio = gltfScene.GetComponent<AudioClipContainer>();
                if (_importAudio && audio != null && audio.hasAudio)
                {
                    var audiosRoot = ImportAudioRoot;
                    Directory.CreateDirectory(audiosRoot);
                    var audioNames = new List<string>();
                    var audioHash = new HashSet<AudioClip>();
                    var audioClips = audio.audioClips.Select(clip =>
                        {
                            if (audioHash.Add(clip))
                            {
                                var matName = clip.name;
                                if (string.IsNullOrEmpty(matName))
                                {
                                    matName = gltfScene.name;
                                }

                                // Ensure name is unique
                                matName = ObjectNames.NicifyVariableName(matName);
                                matName = ObjectNames.GetUniqueName(audioNames.ToArray(), matName);

                                clip.name = matName;
                                audioNames.Add(matName);
                            }

                            return clip;
                        }).ToArray();

                    for (int i = 0; i < audioClips.Length; ++i)
                    {
                        var clip = audioClips[i];
                        var audioPathNoExt = string.Concat(audiosRoot, clip.name);

                        string audioPath = string.Concat(audioPathNoExt, ".wav");
                        if (clip.length < GLTFSceneExporter.ExportOggAudioClipLength)
                        {
                            WaveAudio.Save(audioPath, clip);
                        }
                        else
                        {
                            try
                            {
                                audioPath = string.Concat(audioPathNoExt, ".ogg");
                                OggVorbis.VorbisPlugin.Save(audioPath, clip);
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning($"Can't save as ogg format, exception happened : {e}");
                                audioPath = string.Concat(audioPathNoExt, ".wav");
                                WaveAudio.Save(audioPath, clip);
                            }
                        }
                        AssetDatabase.ImportAsset(audioPath);
                    }
                    EditorApplication.delayCall += () =>
                    {
                        gltfScene = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                        var audioSourceContainer = gltfScene.GetComponent<AudioClipContainer>();
                        var playables = gltfScene.GetComponentsInChildren<PlayableController>();

                        var audiosRoot = GetImportDir("Audios");
                        for (int i = 0; i < audioNames.Count; ++i)
                        {
                            var audioPathNoExt = string.Concat(audiosRoot, audioNames[i]);
                            var audioPath = string.Concat(audioPathNoExt, ".ogg");
                            if (!File.Exists(audioPath))
                                audioPath = string.Concat(audioPathNoExt, ".wav");
                            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
                            audioSourceContainer.audioClips[i] = clip;
                        }
                        sceneImporter.ReimportPlayableMedia(playables, audioSourceContainer);
                    };
                }

                var playables = gltfScene.GetComponentsInChildren<PlayableController>();
                if (playables.Length > 0)
                {
                    int trackIndex = 0;
                    var textureNames = new List<string>();
                    //place texture in a sepreate place, then record the index to track.textureId ,reimport texture then retarget the reference by new textureId 
                    List<Texture2D> textures = playables.SelectMany(
                        r =>
                        {
                            return r.trackAsset.materialTextureTrackGroup.tracks.Select(track =>
                            {
                                track.SetTextureId(new TextureId() { Id = trackIndex++ });
                                string texName = track.value.name;
                                texName = string.Format("{0}_{1}", sceneName, ObjectNames.NicifyVariableName(texName));
                                texName = ObjectNames.GetUniqueName(textureNames.ToArray(), texName);
                                track.value.name = texName;
                                textureNames.Add(texName);
                                return track.value;
                            });
                        }).ToList();

                    ImportTexture(textures, ImportTextureRoot);

                    EditorApplication.delayCall += () =>
                    {
                        gltfScene = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                        var playables = gltfScene.GetComponentsInChildren<PlayableController>();

                        var texturesRoot = ImportTextureRoot;
                        List<Texture2D> newTextures = new List<Texture2D>();
                        for (int i = 0; i < textureNames.Count; ++i)
                        {
                            var path = string.Concat(texturesRoot, textureNames[i]);
                            path = GetImagePath(path);
                            var newTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                            newTextures.Add(newTexture);
                        }
                        sceneImporter.ReimportPlayableMedia(playables, newTextures);
                    };
                }
                var metas = gltfScene.GetComponentsInChildren<BVAMetaInfo>();
                if (metas.Length > 0)
                {
                    int index = 0;
                    var metaNames = new List<string>();
                    //place texture in a sepreate place, then record the index to track.textureId ,reimport texture then retarget the reference by new textureId 
                    List<Texture2D> textures = metas.Select(r =>
                    {
                        string titleName = r.metaInfo.title;
                        titleName = string.Format("{0}_{1}", sceneName, ObjectNames.NicifyVariableName(titleName));
                        titleName = ObjectNames.GetUniqueName(metaNames.ToArray(), titleName);

                        if (r.metaInfo.thumbnail != null)
                        {
                            r.metaInfo.SetTextureId(new TextureId() { Id = index++ });
                            r.metaInfo.thumbnail.name = titleName;
                        }
                        string metaPath = ImportMetaRoot;
                        string path = string.Concat(metaPath, $"{titleName}.asset");
                        if (!Directory.Exists(metaPath))
                            Directory.CreateDirectory(metaPath);
                        AssetDatabase.CreateAsset(r.metaInfo, path);
                        AssetDatabase.ImportAsset(path);
                        metaNames.Add(titleName);
                        return r.metaInfo.thumbnail;
                    }).ToList();

                    ImportTexture(textures, ImportMetaRoot);

                    EditorApplication.delayCall += () =>
                    {
                        gltfScene = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                        var metas = gltfScene.GetComponentsInChildren<BVAMetaInfo>();

                        var metaRoot = ImportMetaRoot;
                        for (int i = 0; i < metaNames.Count; ++i)
                        {
                            var path = string.Concat(metaRoot, metaNames[i]);
                            var texPath = GetImagePath(path);
                            var assetPath = string.Concat(path, ".asset");
                            metas[i].metaInfo = AssetDatabase.LoadAssetAtPath<BVAMetaInfoScriptableObject>(assetPath);
                            metas[i].metaInfo.thumbnail = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
                        }

                    };
                }

                var volumes = gltfScene.GetComponentsInChildren<Volume>();
                if (volumes.Length > 0)
                {
                    var postProcessRootPath = ImportPostProcessRoot;
                    if (!Directory.Exists(postProcessRootPath)) Directory.CreateDirectory(postProcessRootPath);
                    var volumeNames = new List<string>();
                    List<Texture2D> textures = new List<Texture2D>();
                    var lutTextureNames = new List<string>();
                    //place texture in a sepreate place, then record the index to track.textureId ,reimport texture then retarget the reference by new textureId 
                    string lutTexturePath = ImportPostProcessLutRoot;
                    List<VolumeProfile> volumeProfiles = volumes.Select(x => x.sharedProfile).ToList();

                    foreach (var profile in volumeProfiles)
                    {
                        string titleName = profile.name;
                        titleName = string.Format("{0}_{1}", sceneName, ObjectNames.NicifyVariableName(titleName));
                        titleName = ObjectNames.GetUniqueName(volumeNames.ToArray(), titleName);

                        string path = string.Concat(postProcessRootPath, $"{titleName}.asset");
                        AssetDatabase.CreateAsset(profile, path);
                        foreach (var v in profile.components)
                            AssetDatabase.AddObjectToAsset(v, path);
                        AssetDatabase.SaveAssets();
                        volumeNames.Add(titleName);

                        foreach (var v in profile.components)
                        {
                            ColorLookup lookup = v as ColorLookup;
                            if (lookup != null)
                            {
                                if (lookup.texture != null && lookup.texture.value != null)
                                {
                                    textures.Add(lookup.texture.value as Texture2D);
                                    var texPath = GetImagePath(string.Concat(lutTexturePath, lookup.texture.value.name));
                                    lutTextureNames.Add(texPath);
                                }
                                else
                                {
                                    textures.Add(null);
                                    lutTextureNames.Add(null);
                                }
                            }
                        }
                    };
                    ImportTexture(textures, lutTexturePath);

                    EditorApplication.delayCall += () =>
                    {
                        gltfScene = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                        volumes = gltfScene.GetComponentsInChildren<Volume>();
                        volumeProfiles = volumes.Select(x => x.sharedProfile).ToList();

                        for (int i = 0; i < volumeProfiles.Count; ++i)
                        {
                            var path = string.Concat(ImportPostProcessRoot, volumeNames[i]);
                            var assetPath = string.Concat(path, ".asset");
                            volumes[i].sharedProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(assetPath);
                            if (textures[i] == null || lutTextureNames[i] == null)
                                continue;
                            foreach (var v in volumes[i].sharedProfile.components)
                            {
                                ColorLookup lookup = v as ColorLookup;
                                if (lookup != null)
                                {
                                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(lutTextureNames[i]);
                                    lookup.texture.value = texture;
                                    break;
                                }
                            }
                        }
                    };
                }
                var renderers = gltfScene.GetComponentsInChildren<Renderer>();
#if UNITY_2021_1_OR_NEWER
                var decals = gltfScene.GetComponentsInChildren<DecalProjector>();
#endif
                var skyboxContainer = gltfScene.GetComponent<SkyboxContainer>();

                if (_importMaterials)
                {
                    // Get materials
                    var materialNames = new List<string>();
                    var materialHash = new HashSet<Material>();
                    void MaterialNameHandle(Material mat)
                    {
                        if (materialHash.Add(mat))
                        {
                            var matName = string.IsNullOrEmpty(mat.name) ? mat.shader.name : mat.name;
                            if (matName == mat.shader.name)
                            {
                                matName = matName.Substring(Mathf.Min(matName.LastIndexOf("/") + 1, matName.Length - 1));
                            }

                            // Ensure name is unique
                            matName = string.Format("{0}_{1}", sceneName, ObjectNames.NicifyVariableName(matName));
                            matName = ObjectNames.GetUniqueName(materialNames.ToArray(), matName);

                            mat.name = matName;
                            materialNames.Add(matName);
                        }
                    }
                    var materials = renderers.SelectMany(r =>
                    {
                        if (r.GetComponent<TextMesh>() != null)
                        {
                            return new Material[0];
                        }
                        return r.sharedMaterials.Select(mat =>
                        {
                            MaterialNameHandle(mat);
                            return mat;
                        });
                    }).ToList();

                    var skybox = gltfScene.GetComponent<SkyboxContainer>();
                    if (_importSkybox && skybox != null)
                    {
                        materials.AddRange(skybox.materials);
                    }
#if UNITY_2021_1_OR_NEWER
                    var decalMaterials = decals.Select(r => { MaterialNameHandle(r.material); return r.material; });
                    materials.AddRange(decalMaterials);
#endif
                    // Get textures
                    var textureNames = new List<string>();
                    var textureHash = new HashSet<Texture2D>();
                    var cubemapHash = new HashSet<Cubemap>();
                    var texMaterialMap = new Dictionary<Texture2D, List<TexMaterialMap>>();
                    var cubemapMaterialMap = new Dictionary<Cubemap, List<TexMaterialMap>>();
                    var textures = materials.SelectMany(mat =>
                    {
                        var shader = mat.shader;
                        if (!shader) return Enumerable.Empty<Texture2D>();

                        var matTextures = new List<Texture2D>();
                        for (var i = 0; i < ShaderUtil.GetPropertyCount(shader); ++i)
                        {
                            if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                            {
                                var propertyName = ShaderUtil.GetPropertyName(shader, i);
                                var tex = mat.GetTexture(propertyName) as Texture2D;
                                if (tex)
                                {
                                    if (textureHash.Add(tex))
                                    {
                                        var texName = tex.name;
                                        if (string.IsNullOrEmpty(texName))
                                        {
                                            if (propertyName.StartsWith("_")) texName = propertyName.Substring(Mathf.Min(1, propertyName.Length - 1));
                                        }

                                        // Ensure name is unique
                                        texName = string.Format("{0}_{1}", sceneName, ObjectNames.NicifyVariableName(texName));
                                        texName = ObjectNames.GetUniqueName(textureNames.ToArray(), texName);

                                        tex.name = texName;
                                        textureNames.Add(texName);
                                        matTextures.Add(tex);
                                    }

                                    List<TexMaterialMap> materialMaps;
                                    if (!texMaterialMap.TryGetValue(tex, out materialMaps))
                                    {
                                        materialMaps = new List<TexMaterialMap>();
                                        texMaterialMap.Add(tex, materialMaps);
                                    }

                                    materialMaps.Add(new TexMaterialMap(mat, propertyName, UnityTools.IsNormalMap(propertyName), false, tex.mipmapCount > 1));
                                }
                            }
                        }
                        return matTextures;
                    }).ToList();
                    var cubemaps = materials.SelectMany(mat =>
                    {
                        var shader = mat.shader;
                        if (!shader) return Enumerable.Empty<Cubemap>();

                        var matCubemaps = new List<Cubemap>();
                        for (var i = 0; i < ShaderUtil.GetPropertyCount(shader); ++i)
                        {
                            if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                            {
                                var propertyName = ShaderUtil.GetPropertyName(shader, i);
                                var cubemap = mat.GetTexture(propertyName) as Cubemap;
                                if (cubemap)
                                {
                                    if (cubemapHash.Add(cubemap))
                                    {
                                        var cubemapName = cubemap.name;
                                        if (string.IsNullOrEmpty(cubemapName))
                                        {
                                            if (propertyName.StartsWith("_")) cubemapName = propertyName.Substring(Mathf.Min(1, propertyName.Length - 1));
                                        }

                                        // Ensure name is unique
                                        cubemapName = string.Format("{0}_{1}", sceneName, ObjectNames.NicifyVariableName(cubemapName));
                                        cubemapName = ObjectNames.GetUniqueName(textureNames.ToArray(), cubemapName);

                                        cubemap.name = cubemapName;
                                        textureNames.Add(cubemapName);
                                        matCubemaps.Add(cubemap);
                                    }

                                    if (!cubemapMaterialMap.TryGetValue(cubemap, out List<TexMaterialMap> materialMaps))
                                    {
                                        materialMaps = new List<TexMaterialMap>();
                                        cubemapMaterialMap.Add(cubemap, materialMaps);
                                    }

                                    materialMaps.Add(new TexMaterialMap(mat, propertyName, false, true, true));
                                }
                            }
                        }
                        return matCubemaps;
                    }).ToList();

                    ImportTexture(textures, ImportTextureRoot);
                    ImportCubemap(cubemaps, ImportTextureRoot);

                    // Save materials as separate assets and rewrite refs
                    if (materials.Count > 0)
                    {
                        var materialRoot = ImportMaterialRoot;
                        Directory.CreateDirectory(materialRoot);

                        foreach (var mat in materials)
                        {
                            var materialPath = string.Concat(materialRoot, mat.name, ".mat");
                            var newMat = mat;
                            CopyOrNew(mat, materialPath, m =>
                            {
                                // Fix references
                                newMat = m;
                                foreach (var r in renderers)
                                {
                                    var sharedMaterials = r.sharedMaterials;
                                    for (var i = 0; i < sharedMaterials.Length; ++i)
                                    {
                                        var sharedMaterial = sharedMaterials[i];
                                        if (sharedMaterial.name == mat.name) sharedMaterials[i] = m;
                                    }
                                    //sharedMaterials = sharedMaterials.Where(sm => sm).ToArray();
                                    r.sharedMaterials = sharedMaterials;
                                }
#if UNITY_2021_1_OR_NEWER
                                foreach (var d in decals)
                                {
                                    d.material = m;
                                }
#endif
                                if (skyboxContainer)
                                {
                                    for (var i = 0; i < skyboxContainer.materials.Count; ++i)
                                    {
                                        var sharedMaterial = skyboxContainer.materials[i];
                                        if (sharedMaterial.name == mat.name) skyboxContainer.materials[i] = m;
                                    }
                                }
                            });

                            // Unity needs a frame to kick off the texture import so we can rewrite the ref
                            if (textures.Count > 0)
                            {
                                EditorApplication.delayCall += () =>
                                {
                                    var texturesRoot = ImportTextureRoot;
                                    void ReimportTextures(List<TexMaterialMap> materialMaps, string texPath)
                                    {
                                        var importer = (TextureImporter)GetAtPath(texPath);
                                        var importedTex = AssetDatabase.LoadAssetAtPath<Texture>(texPath);
                                        if (importer != null)
                                        {
                                            var isNormalMap = false;
                                            var isCubemap = false;
                                            var useMipmap = false;
                                            foreach (var materialMap in materialMaps)
                                            {
                                                if (materialMap.Material == mat)
                                                {
                                                    isNormalMap = materialMap.IsNormalMap;
                                                    isCubemap = materialMap.IsCubemap;
                                                    useMipmap = materialMap.UseMipmap;
                                                    newMat.SetTexture(materialMap.Property, importedTex);
                                                }
                                            };
                                            bool changed = false;
                                            if (importer.mipmapEnabled != useMipmap)
                                            {
                                                importer.mipmapEnabled = useMipmap;
                                                changed = true;
                                            }
                                            if (isNormalMap && importer.textureType != TextureImporterType.NormalMap)
                                            {
                                                changed = true;
                                                importer.sRGBTexture = false;
                                                importer.textureType = TextureImporterType.NormalMap;
                                            }
                                            else if (isCubemap)
                                            {
                                                changed = true;
                                                importer.sRGBTexture = false;
                                                importer.textureShape = TextureImporterShape.TextureCube;
                                            }
                                            else if (importer.textureType == TextureImporterType.Sprite)
                                            {
                                                changed = true;
                                                importer.textureType = TextureImporterType.Sprite;
                                            }
                                            if (changed)
                                                importer.SaveAndReimport();
                                        }
                                        else
                                        {
                                            Debug.LogWarning("GLTFImporter: Unable to import texture from path reference");
                                        }
                                    }
                                    for (var i = 0; i < textures.Count; ++i)
                                    {
                                        var tex = textures[i];
                                        if (tex == null)
                                            continue;
                                        var texPath = GetImagePath(string.Concat(texturesRoot, tex.name));

                                        // Grab new imported texture
                                        var materialMaps = texMaterialMap[tex];
                                        ReimportTextures(materialMaps, texPath);
                                    }
                                    for (var i = 0; i < cubemaps.Count; ++i)
                                    {
                                        var cubemap = cubemaps[i];
                                        if (cubemap == null)
                                            continue;
                                        var texPath = GetImagePath(string.Concat(texturesRoot, cubemap.name));

                                        // Grab new imported texture
                                        var materialMaps = cubemapMaterialMap[cubemap];
                                        ReimportTextures(materialMaps, texPath);
                                    }
                                };
                            }
                        }
                    }
                }
                else
                {
                    var temp = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    temp.SetActive(false);
                    var defaultMat = new[] { temp.GetComponent<Renderer>().sharedMaterial };
                    DestroyImmediate(temp);

                    foreach (var rend in renderers)
                    {
                        rend.sharedMaterials = defaultMat;
                    }
                }
                #region DynamicBone

                var dynamicBones = gltfScene.GetComponentsInChildren<ADBChainProcessor>();
                if (_importPuretsScript)
                {
                    if (dynamicBones.Length > 0)
                    {
                        Directory.CreateDirectory(ImportDynamicBoneRoot);

                        List<string> ADBSettingMetaNames = new List<string>();
                        HashSet<string> metaNameTable = new HashSet<string>();
                        for (int i = 0; i < dynamicBones.Length; i++)
                        {
                            var importer = dynamicBones[i];

                            var settingMeta = importer.GetADBSetting();
                            ADBSettingMetaNames.Add(settingMeta.name);
                            if (metaNameTable.Add(settingMeta.name))//OYM:Does the same setting?
                            {
                                string path = string.Concat(ImportDynamicBoneRoot, $"{settingMeta.name}.asset");

                                AssetDatabase.CreateAsset(settingMeta, path);
                            }
                        }
                        EditorApplication.delayCall += () =>
                        {
                            gltfScene = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                            var chainImporters = gltfScene.GetComponentsInChildren<ADBChainProcessor>();
                            for (int i = 0; i < chainImporters.Length; i++)
                            {
                                string path = string.Concat(ImportDynamicBoneRoot, $"{ADBSettingMetaNames[i]}.asset");
                                chainImporters[i].SetADBSetting(AssetDatabase.LoadAssetAtPath<ADBPhysicsSetting>(path));
                            }
                        };
                    }

                }
                else
                {
                    for (int i = 0; i < dynamicBones.Length; i++)
                    {
                        DestroyImmediate(dynamicBones[i].Target);
                    }
                }

#if PUERTS_INTEGRATION
                var puertsMonos = gltfScene.GetComponentsInChildren<MonoBehaviour>()
    .Where(x => x is PuertsEvent || x is PuertsCall).ToArray();
                if (_importPuretsScript)
                {
                    if (puertsMonos.Length > 0)
                    {
                        Directory.CreateDirectory(ImportPureScriptRoot);
                        string[][] tsScriptNameLists = new string[puertsMonos.Length][];

                        for (int i = 0; i < puertsMonos.Length; i++)
                        {
                            var puertsMono = puertsMonos[i];

                            if (puertsMono is PuertsEvent)
                            {
                                var puertsEvent = puertsMono as PuertsEvent;

                                var scriptList = puertsEvent.scripts;
                                tsScriptNameLists[i] = new string[puertsEvent.scripts.Count];
                                for (int j0 = 0; j0 < scriptList.Count; j0++)
                                {

                                    var script = scriptList[j0].script;
                                    if (script==null)
                                    {
                                        continue;
                                    }
                                    string name = script.name;
                                    if (!tsScriptNameLists.Any(x=>x.Contains(name)))
                                    {
                                        string path = string.Concat(ImportPureScriptRoot, $"{name}.asset");
                                        AssetDatabase.CreateAsset(script, path);
                                    }
                                    tsScriptNameLists[i][j0]=name;
                                }
                            }
                            else if (puertsMono is PuertsCall)
                            {
                                var puertsEvent = puertsMono as PuertsCall;

                                var scriptList = puertsEvent.scripts;
                                tsScriptNameLists[i] = new string[puertsEvent.scripts.Count];
                                for (int j0 = 0; j0 < scriptList.Count; j0++)
                                {
                                    var script = scriptList[j0].script;
                                    if (script == null)
                                    {
                                        continue;
                                    }

                                    string name = script.name;
                                    if (!tsScriptNameLists.Any(x => x!=null&& x.Any(y=>y==name)))
                                    {
                                        string path = string.Concat(ImportPureScriptRoot, $"{name}.asset");
                                        AssetDatabase.CreateAsset(script, path);
                                    }
                                    tsScriptNameLists[i][j0] = name;
                                }
                            }
                        }


                        EditorApplication.delayCall += () =>
                        {
                            gltfScene = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                            var _puertsMonos = gltfScene.GetComponentsInChildren<MonoBehaviour>()
    .Where(x => x is PuertsEvent || x is PuertsCall).ToArray();
                            gltfScene = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                            for (int i = 0; i < _puertsMonos.Length; i++)
                            {
                                var puertsMono = _puertsMonos[i];
                                if (puertsMono is PuertsEvent)
                                {
                                    var puertsEvent = puertsMono as PuertsEvent;

                                    var scriptList = puertsEvent.scripts;
                                    for (int j0 = 0; j0 < scriptList.Count; j0++)
                                    {
                                        string name = tsScriptNameLists[i][j0];
                                        if (name==null)
                                        {
                                            continue;
                                        }
                                        string path = string.Concat(ImportPureScriptRoot, $"{name}.asset");
                                        var scriptStruct = scriptList[j0];
                                        scriptStruct.script = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                                        scriptList[j0] = scriptStruct;
                                    }
                                }
                                else if (puertsMono is PuertsCall)
                                {
                                    var puertsEvent = puertsMono as PuertsCall;

                                    var scriptList = puertsEvent.scripts;
                                    for (int j0 = 0; j0 < scriptList.Count; j0++)
                                    {
                                        string name = tsScriptNameLists[i][j0];
                                        if (name == null)
                                        {
                                            continue;
                                        }
                                        string path = string.Concat(ImportPureScriptRoot, $"{name}.asset");
                                        var scriptStruct = scriptList[j0];
                                        scriptStruct.script = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                                        scriptList[j0] = scriptStruct;
                                    }
                                }
                            }
                        };
                    }

                }
                else
                {
                    for (int i = 0; i < puertsMonos.Length; i++)
                    {
                        DestroyImmediate(puertsMonos[i]);
                    }
                }
#endif
            }
            #endregion
            catch
            {
                if (gltfScene) DestroyImmediate(gltfScene);
                throw;
            }
            // Set main asset
            ctx.AddObjectToAsset("main asset", gltfScene);

            // Add meshes
            foreach (var mesh in meshes)
            {
                ctx.AddObjectToAsset("mesh " + mesh.name, mesh);
            }

            ctx.SetMainObject(gltfScene);
        }

        private GLTFSceneImporter CreateGLTFScene(string projectFilePath)
        {

            var importOptions = new ImportOptions
            {
                DataLoader = (Application.isPlaying) ? new FileLoader(Path.GetDirectoryName(projectFilePath)) : new EditorFileLoader(Path.GetDirectoryName(projectFilePath)),
                RuntimeImport = false
            };
            using (var stream = File.OpenRead(projectFilePath))
            {
                GLTFParser.ParseJson(stream, out var gLTFRoot);
                stream.Position = 0; // Make sure the read position is changed back to the beginning of the file
                var loader = new GLTFSceneImporter(gLTFRoot, stream, importOptions);
                loader.IsMultithreaded = true;

                loader.LoadAsync().Wait();
                return loader;
            }
        }

        private void CopyOrNew<T>(T asset, string assetPath, Action<T> replaceReferences) where T : Object
        {
            var existingAsset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (existingAsset)
            {
                EditorUtility.CopySerialized(asset, existingAsset);
                replaceReferences(existingAsset);
                return;
            }

            AssetDatabase.CreateAsset(asset, assetPath);
        }

        private class TexMaterialMap
        {
            public Material Material { get; set; }
            public string Property { get; set; }
            public bool IsNormalMap { get; set; }
            public bool IsCubemap { get; set; }
            public bool UseMipmap { get; set; }

            public TexMaterialMap(Material material, string property, bool isNormalMap, bool isCubemap, bool mipmap)
            {
                Material = material;
                Property = property;
                IsNormalMap = isNormalMap;
                IsCubemap = isCubemap;
                UseMipmap = mipmap;
            }
        }

    }
}
