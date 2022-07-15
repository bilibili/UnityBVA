using UnityEditor;
using UnityEngine;
using UniGLTF;
using System.IO;
using System.Text.RegularExpressions;
using VRMShaders;
using System.Collections.Generic;
using ADBRuntime.Mono;

namespace VRM
{
    public class VRMImportEditor : AssetPostprocessor
    {
        [MenuItem("BVA/Runtime Load/Load VRM (Universal RP)")]
        static void ImportMenu()
        {
            var path = EditorUtility.OpenFilePanel("open vrm", "", "vrm");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (Application.isPlaying)
            {
                Selection.activeGameObject = ImportRuntime(path);
                SwitchToURPMToon(Selection.activeGameObject);
                return;
            }
        }

        static GameObject ImportRuntime(string path)
        {
            // load into scene
            var data = new GlbFileParser(path).Parse();
            // VRM extension を parse します
            var vrm = new VRMData(data);
            using (var context = new VRMImporterContext(vrm))
            {
                var loaded = context.Load();
                loaded.EnableUpdateWhenOffscreen();
                loaded.ShowMeshes();
                loaded.gameObject.name = loaded.name;
                return loaded.gameObject;
            }
        }

        static void SwitchToURPMToon(GameObject go)
        {
            Shader replaceShader = Shader.Find("Shader Graphs/MToon");
            var skinnedMeshRenderers = go.GetComponentsInChildren<Renderer>();
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                foreach (var v in skinnedMeshRenderer.sharedMaterials)
                {
                    v.shader = replaceShader;
                }
            }
        }

        const string AssetFolderName = "Assets";
        const string MeshFolderName = "Mesh/";
        const string AvatarFolderName = "Avatar/";
        const string TexturesFolderName = "Textures/";
        const string MaterialsFolderName = "Materials/";
        const string PhysicsFolderName = "Physics/";
        const string VRMExtension = ".vrm";
        const string PNGExtension = ".png";
        const string MeshExtension = ".mesh";
        const string AvatarExtension = ".asset";
        const string PhysicsExtension = ".asset";
        const string MaterialExtension = ".mat";
        const string PrefabExtension = ".prefab";

        static void OnPostprocessAllAssets(string[] relativePaths, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string relativePath in relativePaths)
            {
                if (relativePath.EndsWith(VRMExtension))
                {
                    string inputFileAbsolutePath = Application.dataPath + relativePath.TrimStart(AssetFolderName.ToCharArray());
                    string inputFileName = Path.GetFileName(inputFileAbsolutePath);
                    string inputFileNameWithoutExtension = inputFileName.TrimEnd(VRMExtension.ToCharArray());
                    string inputFolderAbsolutePath = Path.GetDirectoryName(inputFileAbsolutePath);
                    string inputFolderRelativePath = Path.GetDirectoryName(relativePath);
                    string outputFolderAbsolutePath = inputFolderAbsolutePath + "/" + SanitizeFolderName(inputFileNameWithoutExtension) + "/";
                    string outputFolderRelativePath = inputFolderRelativePath + "/" + SanitizeFolderName(inputFileNameWithoutExtension) + "/";
                    string outputFileAbsolutePath = outputFolderAbsolutePath + inputFileNameWithoutExtension + PrefabExtension;

                    GameObject model = ImportRuntime(inputFileAbsolutePath);
                    SwitchToURPMToon(model);
                    if (model == null)
                    {
                        Debug.Log("Error on loading : " + inputFileAbsolutePath);
                        continue;
                    }

                    if (!Directory.Exists(outputFolderAbsolutePath))
                    {
                        Directory.CreateDirectory(outputFolderAbsolutePath);
                    }

                    string meshFolderAbsolutePath = outputFolderAbsolutePath + MeshFolderName;
                    if (!Directory.Exists(meshFolderAbsolutePath))
                    {
                        Directory.CreateDirectory(meshFolderAbsolutePath);
                    }

                    string avatarFolderAbsolutePath = outputFolderAbsolutePath + AvatarFolderName;
                    if (!Directory.Exists(avatarFolderAbsolutePath))
                    {
                        Directory.CreateDirectory(avatarFolderAbsolutePath);
                    }

                    string texturesFolderPath = outputFolderAbsolutePath + TexturesFolderName;
                    if (!Directory.Exists(texturesFolderPath))
                    {
                        Directory.CreateDirectory(texturesFolderPath);
                    }

                    string materialsFolderPath = outputFolderAbsolutePath + MaterialsFolderName;
                    if (!Directory.Exists(materialsFolderPath))
                    {
                        Directory.CreateDirectory(materialsFolderPath);
                    }

                    string physicsFolderPath = outputFolderAbsolutePath + PhysicsFolderName;
                    if (!Directory.Exists(physicsFolderPath))
                    {
                        Directory.CreateDirectory(physicsFolderPath);
                    }

                    var renderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (var renderer in renderers)
                    {
                        var mesh = renderer.sharedMesh;
                        if (mesh != null)
                        {
                            AssetDatabase.CreateAsset(mesh, outputFolderRelativePath + MeshFolderName + mesh.name + MeshExtension);
                        }
                    }

                    Animator animator = model.GetComponent<Animator>();
                    if (animator != null)
                    {
                        Avatar avatar = animator.avatar;
                        if (avatar != null)
                        {
                            AssetDatabase.CreateAsset(avatar, outputFolderRelativePath + AvatarFolderName + avatar.name + AvatarExtension);
                        }
                    }
                    List<string> textureNames = new List<string>();
                    foreach (var renderer in renderers)
                    {
                        foreach (Material material in renderer.sharedMaterials)
                        {
                            if (material == null) continue;
                            var texProperties = material.GetTexturePropertyNames();
                            foreach (var texProperty in texProperties)
                            {
                                Texture2D tex = material.GetTexture(texProperty) as Texture2D;
                                if (tex == null) continue;
                                string textureAbsolutePath = texturesFolderPath + tex.name + PNGExtension;
                                string textureRelativePath = outputFolderRelativePath + TexturesFolderName + tex.name + PNGExtension;
                                if (textureNames.Contains(tex.name))
                                {
                                    material.SetTexture(texProperty, AssetDatabase.LoadAssetAtPath<Texture>(textureRelativePath));
                                    continue;
                                }
                                using (FileStream fileStream = new FileStream(textureAbsolutePath, FileMode.Create))
                                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                                {

                                    if (!tex.isReadable)
                                    {
                                        var dst = TextureConverter.CopyTexture(tex, VRMShaders.ColorSpace.sRGB, true, null);
                                        binaryWriter.Write(dst.EncodeToPNG());
                                    }
                                    else
                                        binaryWriter.Write(tex.EncodeToPNG());
                                }
                                textureNames.Add(tex.name);
                                AssetDatabase.Refresh();
                                var importer = (TextureImporter)AssetImporter.GetAtPath(textureRelativePath);
                                importer.mipmapEnabled = false;
                                importer.SaveAndReimport();
                                material.SetTexture(texProperty, AssetDatabase.LoadAssetAtPath<Texture>(textureRelativePath));
                            }

                            var materialPath = outputFolderRelativePath + MaterialsFolderName + material.name + MaterialExtension;
                            if(!File.Exists(materialPath))
                                AssetDatabase.CreateAsset(material, materialPath);
                        }
                    }
                    var dynamicBones = model.GetComponentsInChildren<ADBChainProcessor>();
                    for (int i = 0; i < dynamicBones.Length; i++)
                    {
                        var dynamicBone = dynamicBones[i];
                        
                        ADBRuntime.ADBPhysicsSetting settingMeta = dynamicBone.GetADBSetting();
                        if (settingMeta != null)
                        {
                            string path = outputFolderRelativePath + PhysicsFolderName +settingMeta.name + PhysicsExtension;
                            if (!File.Exists(path))
                            {
                                AssetDatabase.CreateAsset(settingMeta, path);
                            }
                            dynamicBone.SetADBSetting(AssetDatabase.LoadAssetAtPath<ADBRuntime.ADBPhysicsSetting>(path));
                        }
                    }

                    PrefabUtility.SaveAsPrefabAsset(model, outputFolderRelativePath + model.name + PrefabExtension);
#if UNITY_EDITOR
                    GameObject.DestroyImmediate(model);
#else
                GameObject.Destroy(model);
#endif

                }
            }
        }

        public static string SanitizeFolderName(string name)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(name, "");
        }
    }
}
