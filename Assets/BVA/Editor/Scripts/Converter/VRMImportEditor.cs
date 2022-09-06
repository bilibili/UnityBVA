using UnityEditor;
using UnityEngine;
using UniGLTF;
using System.IO;
using System.Text.RegularExpressions;
using VRMShaders;
using System.Collections.Generic;
using ADBRuntime.Mono;
using System;
using System.Linq;
using VRM;
using BVA;
using BVA.Component;
using GLTF.Schema.BVA;

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
                BVAMaterialExtension.ChangeMaterial(Selection.activeGameObject);
                BVASpringBoneExtension.TranslateVRMPhysicToBVAPhysics(Selection.activeGameObject);
                SwitchComponent(Selection.activeGameObject, null);
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
                data.Dispose();
                return loaded.gameObject;
            }
        }

        static void SwitchComponent(GameObject go, string thumbnailPath)
        {
            //Meta Info
            VRMMetaObject meta = go.GetComponent<VRMMeta>().Meta;
            BVAMetaInfoScriptableObject bvaMetaInfo = ScriptableObject.CreateInstance<BVA.Component.BVAMetaInfoScriptableObject>();
            bvaMetaInfo.formatVersion = meta.ExporterVersion;
            bvaMetaInfo.title = meta.Title;
            bvaMetaInfo.version = meta.Version;
            bvaMetaInfo.author = meta.Author;
            bvaMetaInfo.contact = meta.ContactInformation;
            bvaMetaInfo.reference = meta.Reference;
            if (thumbnailPath != null)
            {
                if (meta.Thumbnail != null)
                {
                    File.WriteAllBytes(thumbnailPath, meta.Thumbnail.EncodeToPNG());
                    AssetDatabase.Refresh();
                    var thumbnail = AssetDatabase.LoadAssetAtPath<Texture2D>(thumbnailPath);
                    bvaMetaInfo.thumbnail = thumbnail;
                }
            }
            else
            {
                bvaMetaInfo.thumbnail = meta.Thumbnail;
            }
            bvaMetaInfo.contentType = ContentType.Avatar;
            bvaMetaInfo.legalUser = (LegalUser)Enum.GetNames(typeof(AllowedUser)).ToList().IndexOf(meta.AllowedUser.ToString());
            bvaMetaInfo.violentUsage = (UsageLicense)Enum.GetNames(typeof(UssageLicense)).ToList().IndexOf(meta.ViolentUssage.ToString());
            bvaMetaInfo.sexualUsage = (UsageLicense)Enum.GetNames(typeof(UssageLicense)).ToList().IndexOf(meta.SexualUssage.ToString());
            bvaMetaInfo.commercialUsage = (UsageLicense)Enum.GetNames(typeof(UssageLicense)).ToList().IndexOf(meta.CommercialUssage.ToString());
            bvaMetaInfo.licenseType = (BVA.Component.LicenseType)Enum.GetNames(typeof(LicenseType)).ToList().IndexOf(meta.LicenseType.ToString());
            bvaMetaInfo.customLicenseUrl = meta.OtherPermissionUrl;
            go.AddComponent<BVAMetaInfo>().metaInfo = bvaMetaInfo;

            //BlendshapeMixer
            BlendShapeMixer mixer = go.AddComponent<BlendShapeMixer>();
            mixer.CreateDefaultPreset();
            BlendShapeAvatar bsa = go.GetComponent<VRMBlendShapeProxy>().BlendShapeAvatar;
            foreach (BlendShapeClip clip in bsa.Clips)
            {
                switch (clip.Preset)
                {
                    case BlendShapePreset.Unknown:
                        mixer.keys.Add(new BVA.BlendShapeKey()
                        {
                            keyName = clip.BlendShapeName,
                            preset = BlendShapeMixerPreset.Custom
                        });

                        foreach (var value in clip.Values)
                        {
                            mixer.keys[mixer.keys.Count - 1].blendShapeValues.Add(new BlendShapeValueBinding()
                            {
                                node = go.transform.Find(value.RelativePath).GetComponent<SkinnedMeshRenderer>(),
                                index = value.Index,
                                weight = value.Weight
                            });
                        }
                        break;
                    case BlendShapePreset.Neutral:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.Neutral, go);
                        break;
                    case BlendShapePreset.A:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.A, go);
                        break;
                    case BlendShapePreset.I:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.I, go);
                        break;
                    case BlendShapePreset.U:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.U, go);
                        break;
                    case BlendShapePreset.E:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.E, go);
                        break;
                    case BlendShapePreset.O:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.O, go);
                        break;
                    case BlendShapePreset.Blink:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.Blink, go);
                        break;
                    case BlendShapePreset.Joy:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.Joy, go);
                        break;
                    case BlendShapePreset.Angry:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.Angry, go);
                        break;
                    case BlendShapePreset.Sorrow:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.Sorrow, go);
                        break;
                    case BlendShapePreset.Fun:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.Fun, go);
                        break;
                    case BlendShapePreset.LookUp:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.LookUp, go);
                        break;
                    case BlendShapePreset.LookDown:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.LookDown, go);
                        break;
                    case BlendShapePreset.LookLeft:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.LookLeft, go);
                        break;
                    case BlendShapePreset.LookRight:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.LookRight, go);
                        break;
                    case BlendShapePreset.Blink_L:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.Blink_L, go);
                        break;
                    case BlendShapePreset.Blink_R:
                        SetPresetValue(clip, mixer, (int)BlendShapeMixerPreset.Blink_R, go);
                        break;
                }
            }

            go.AddComponent<LookAt>();
        }

        static void SetPresetValue(BlendShapeClip clip, BlendShapeMixer mixer, int preset, GameObject go)
        {
            foreach (var value in clip.Values)
            {
                mixer.keys[preset].blendShapeValues.Add(new BlendShapeValueBinding()
                {
                    node = go.transform.Find(value.RelativePath).GetComponent<SkinnedMeshRenderer>(),
                    index = value.Index,
                    weight = value.Weight
                });
            }

            foreach (var value in clip.MaterialValues)
            {
                (SkinnedMeshRenderer targetNode, int targetIndex) = FindMaterial(go, value.MaterialName);

                mixer.keys[preset].materialVector4Values.Add(new MaterialVector4ValueBinding()
                {
                    node = targetNode,
                    index = targetIndex,
                    propertyName = value.ValueName,
                    targetValue = value.TargetValue,
                    baseValue = value.BaseValue
                });

                // VRM only provide Vector4 parameter
                //mixer.keys[preset].materialColorValues.Add(new MaterialColorValueBinding()
                //{
                //    node = targetNode,
                //    index = targetIndex,
                //    propertyName = value.ValueName,
                //    targetValue = value.TargetValue,
                //    baseValue = value.BaseValue
                //});
                //mixer.keys[preset].materialFloatValues.Add(new MaterialFloatValueBinding()
                //{
                //    node = targetNode,
                //    index = targetIndex,
                //    propertyName = value.ValueName,
                //    targetValue = value.TargetValue,
                //    baseValue = value.BaseValue
                //});
            }
        }

        static (SkinnedMeshRenderer, int) FindMaterial(GameObject go, string materialName)
        {
            SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                for (int i = 0; 1 < renderer.materials.Length; i++)
                {
                    if (renderer.materials[i].name == materialName)
                    {
                        return (renderer, i);
                    }
                }
            }

            return (null, -1);
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

                    BVAMaterialExtension.ChangeMaterial(model);
                    BVASpringBoneExtension.TranslateVRMPhysicToBVAPhysics(model);

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
                            if (!File.Exists(materialPath))
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
                            string path = outputFolderRelativePath + PhysicsFolderName + settingMeta.name + PhysicsExtension;
                            if (!File.Exists(path))
                            {
                                AssetDatabase.CreateAsset(settingMeta, path);
                            }
                            dynamicBone.SetADBSetting(AssetDatabase.LoadAssetAtPath<ADBRuntime.ADBPhysicsSetting>(path));
                        }
                    }

                    SwitchComponent(model, outputFolderRelativePath + "Thumbnail.png");

                    if (model.TryGetComponent<BVAMetaInfo>(out BVAMetaInfo meta))
                    {
                        AssetDatabase.CreateAsset(meta.metaInfo, outputFolderRelativePath + "MetaInfo.asset");
                        AssetDatabase.Refresh();
                        BVAMetaInfoScriptableObject asset = AssetDatabase.LoadAssetAtPath<BVAMetaInfoScriptableObject>(outputFolderRelativePath + "MetaInfo.asset");
                        asset.thumbnail = AssetDatabase.LoadAssetAtPath<Texture2D>(outputFolderRelativePath + "Thumbnail.png");
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
