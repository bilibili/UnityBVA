using System.IO;
using UnityEditor;
using UnityEngine;
using LibMMD.Unity3D;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class AutoPMXImporterEditor : AssetPostprocessor
{
    const string AssetFolderName = "Assets";
    const string outputFolderName = "/Prefab/";
    const string MeshFolderName = "Mesh/";
    const string AvatarFolderName = "Avatar/";
    const string TexturesFolderName = "Textures/";
    const string MaterialsFolderName = "Materials/";
    const string PMXExtension = ".pmx";
    const string PNGExtension = ".png";
    const string MeshExtension = ".mesh";
    const string AvatarExtension = ".asset";
    const string MaterialExtension = ".mat";
    const string PrefabExtension = ".prefab";

    async static void OnPostprocessAllAssets(string[] relativePaths, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string relativePath in relativePaths)
        {
            if (relativePath.EndsWith(PMXExtension))
            {
                string inputFileAbsolutePath = Application.dataPath + relativePath.TrimStart(AssetFolderName.ToCharArray());
                string inputFileName = Path.GetFileName(inputFileAbsolutePath);
                string inputFileNameWithoutExtension = inputFileName.TrimEnd(PMXExtension.ToCharArray());
                string inputFolderAbsolutePath = Path.GetDirectoryName(inputFileAbsolutePath);
                string inputFolderRelativePath = Path.GetDirectoryName(relativePath);
                string outputFolderAbsolutePath = inputFolderAbsolutePath + outputFolderName + SanitizeFolderName(inputFileNameWithoutExtension) + "/";
                string outputFolderRelativePath = inputFolderRelativePath + outputFolderName + SanitizeFolderName(inputFileNameWithoutExtension) + "/";
                string outputFileAbsolutePath = outputFolderAbsolutePath + inputFileNameWithoutExtension + PrefabExtension;

                Transform model = await PMXModelLoader.LoadPMXModel(inputFileAbsolutePath, false);
                if (model == null)
                {
                    Debug.Log("読み込みに問題がありました");
                    Debug.Log(inputFileAbsolutePath);
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

                Mesh mesh = model.GetComponent<MMDModel>().Mesh;
                if (mesh != null)
                {
                    AssetDatabase.CreateAsset(mesh, outputFolderRelativePath + MeshFolderName + mesh.name + MeshExtension);
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

                foreach (Material material in model.GetComponent<MMDModel>().SkinnedMeshRenderer.sharedMaterials)
                {
                    List<(string, Texture)> allTexture = new List<(string, Texture)>();
                    Shader shader = material.shader;
                    for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
                    {
                        if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                        {
                            string propertyName = ShaderUtil.GetPropertyName(shader, i);
                            Texture texture = material.GetTexture(ShaderUtil.GetPropertyName(shader, i));
                            if (texture != null)
                            {
                                allTexture.Add((propertyName, texture));
                            }

                        }
                    }
                    if (material != null && allTexture.Count != 0)
                    {
                        foreach (var property in allTexture)
                        {
                            string textureAbsolutePath = texturesFolderPath + property.Item2.name + PNGExtension;
                            string textureRelativePath = outputFolderRelativePath + TexturesFolderName + property.Item2.name + PNGExtension;
                            if (!File.Exists(textureAbsolutePath))
                            {
                                using (FileStream fileStream = new FileStream(textureAbsolutePath, FileMode.Create))
                                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                                {
                                    binaryWriter.Write((property.Item2 as Texture2D).EncodeToPNG());
                                }
                                AssetDatabase.Refresh();
                            }
                            var savedTexture = AssetDatabase.LoadAssetAtPath<Texture>(textureRelativePath);
#if UNITY_2021_1_OR_NEWER
                            if (material.HasTexture(property.Item1))
                                material.SetTexture(property.Item1, savedTexture);
#else
                            if (material.GetTexture(property.Item1) != null)
                                material.SetTexture(property.Item1, savedTexture);
#endif
                        }

                    }

                    if (material != null)
                    {
                        var path = outputFolderRelativePath + MaterialsFolderName + material.name + MaterialExtension;
                        string dir = Path.GetDirectoryName(path);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        AssetDatabase.CreateAsset(material, path);
                    }
                }

                MMDModel mmdModel = model.GetComponent<MMDModel>();
                mmdModel.ShowModel();

#if UNITY_EDITOR
                GameObject.DestroyImmediate(mmdModel);
#else
                GameObject.Destroy(mmdModel);
#endif

                PrefabUtility.SaveAsPrefabAsset(model.gameObject, outputFolderRelativePath + model.name + PrefabExtension);

#if UNITY_EDITOR
                GameObject.DestroyImmediate(model.gameObject);
#else
                GameObject.Destroy(model.gameObject);
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
