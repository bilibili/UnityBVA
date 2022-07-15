using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using BVA.Extensions;

public class UnityTools
{
    #region GUI
    public static void DrawGuiBox(Rect rect, Color color)
    {
        Color color2 = GUI.color;
        GUI.color = color;
        GUI.Box(rect, "");
        GUI.color = color2;
    }
    #endregion
    #region UNITY PATH
    public static string GetAssetPath()
    {
        return Application.dataPath;
    }

    public static string GetRootPath()
    {
        return Application.dataPath.Replace("/Assets", "");
    }

    public static string GetTempPath()
    {
        return Application.dataPath.Replace("/Assets", "/Temp");
    }

    public static string GetNativePath(string assetPath)
    {
        return Path.Combine(GetRootPath(), assetPath);
    }

    public static string GetAssetsRootPath()
    {
#if UNITY_2021_1_OR_NEWER
        return Application.dataPath[0..^6];
#else
        return Application.dataPath.Substring(0, Application.dataPath.Length - 6);
#endif
    }

    public static string GetTempFileName(string ext = null)
    {
        string text = "tmp-" + Path.GetRandomFileName().ToLower();
        if (!string.IsNullOrWhiteSpace(ext))
        {
            text = text + "." + ext;
        }
        string tempPath = GetTempPath();
        if (!Directory.Exists(tempPath))
        {
            Directory.CreateDirectory(tempPath);
        }
        return Path.Combine(tempPath, text);
    }
    public static string GetAssetPath(UnityEngine.Object asset)
    {
        return AssetDatabase.GetAssetPath(asset);
    }

    public static string GetAssetFilename(UnityEngine.Object asset)
    {
        string text = GetAssetPath(asset);
        if (!string.IsNullOrWhiteSpace(text))
        {
            text = Path.GetFileName(text);
        }
        return text;
    }

    public static string GetAssetExtension(UnityEngine.Object asset)
    {
        string result = string.Empty;
        string assetFilename = GetAssetFilename(asset);
        if (!string.IsNullOrWhiteSpace(assetFilename))
        {
            result = Path.GetExtension(assetFilename);
        }
        return result;
    }

    public static string GetDefaultExportFolder()
    {
        string text = Application.dataPath.Replace("/Assets", "/Export");
        if (!Directory.Exists(text))
        {
            Directory.CreateDirectory(text);
        }
        return text;
    }
#endregion
#region TRANSFORM
    public static bool IsNoInstanceNode(ref Transform transform)
    {
        return transform.gameObject.layer == 29;
    }

    public static bool IsPrefabTransform(ref Transform transform)
    {
        return transform.gameObject.layer == 31;
    }

    public static bool IsIgnoreTransform(ref Transform transform)
    {
        return transform.gameObject.layer == 30;
    }

    public static bool IsActiveInHierarchy(Transform transform)
    {
        bool result = transform.gameObject.activeInHierarchy || transform.gameObject.activeSelf;
        Transform transform2 = transform;
        if (IsIgnoreTransform(ref transform2))
        {
            result = false;
        }
        return result;
    }

#endregion
#region RENDER SETTING
    public static float GetAmbientIntensity()
    {
        return RenderSettings.ambientIntensity;
    }

    public static float GetReflectionIntensity()
    {
        return RenderSettings.reflectionIntensity;
    }
    public static bool IsHighDynamicRangeSkybox()
    {
        bool result = false;
        Material skybox = RenderSettings.skybox;
        if (skybox != null && skybox.shader.name == "Skybox/Cubemap")
        {
            Cubemap cubemap = RenderSettings.skybox.GetTexture("_Tex") as Cubemap;
            if (cubemap != null)
            {
                string extension = Path.GetExtension(AssetDatabase.GetAssetPath(cubemap));
                if (extension.Equals(".hdr", StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                }
                else if (extension.Equals(".exr", StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                }
                else if (extension.Equals(".dds", StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                }
            }
        }
        return result;
    }

    public static Color GetSkyboxMaterialTintColor()
    {
        Color result = Color.gray;
        if (RenderSettings.skybox != null)
        {
            if (RenderSettings.skybox.HasProperty("_SkyTint"))
            {
                result = RenderSettings.skybox.GetColor("_SkyTint");
            }
            else if (RenderSettings.skybox.HasProperty("_Tint"))
            {
                result = RenderSettings.skybox.GetColor("_Tint");
            }
        }
        return result;
    }

    public static float GetSkyboxMaterialExposure()
    {
        float result = 0f;
        if (RenderSettings.skybox != null && RenderSettings.skybox.HasProperty("_Exposure"))
        {
            result = RenderSettings.skybox.GetFloat("_Exposure");
        }
        return result;
    }

    public static float GetSkyboxMaterialRotation()
    {
        float result = 0f;
        if (RenderSettings.skybox != null && RenderSettings.skybox.HasProperty("_Rotation"))
        {
            result = RenderSettings.skybox.GetFloat("_Rotation");
        }
        return result;
    }
    public static bool IsHighDynamicRangeReflection()
    {
        bool result = false;
        Cubemap customReflection = (Cubemap)RenderSettings.customReflection;
        if (customReflection != null)
        {
            string extension = Path.GetExtension(AssetDatabase.GetAssetPath(customReflection));
            if (extension.Equals(".hdr", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }
            else if (extension.Equals(".exr", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }
            else if (extension.Equals(".dds", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }
        }
        return result;
    }
    public static Color GetSkyboxColor(Vector3 direction, float gradient = 0f)
    {
        Color result = (direction.y < 0f) ? RenderSettings.ambientGroundColor : RenderSettings.ambientSkyColor;
        if (RenderSettings.ambientMode == AmbientMode.Skybox)
        {
            SphericalHarmonicsL2 ambientProbe = RenderSettings.ambientProbe;
            try
            {
                Vector3[] array = new Vector3[]
                {
                        direction
                };
                Color[] array2 = new Color[]
                {
                        new Color(1f, 1f, 1f, 1f)
                };
                RenderSettings.ambientProbe.Evaluate(array, array2);
                Color color = array2[0];
                if (gradient > 0f)
                {
                    result = Color.Lerp(color, Color.white, Mathf.Clamp(gradient, 0f, 1f));
                }
                else
                {
                    result = color;
                }
            }
            catch (Exception ex)
            {
                result = ((direction.y < 0f) ? RenderSettings.ambientGroundColor : RenderSettings.ambientSkyColor);
                Debug.LogException(ex);
                Debug.LogWarning("Using Skybox Fallback Material Color: " + result.ToString() + " --> For Direction: " + direction.ToString());
            }
        }
        return result;
    }
#endregion
#region COLOR CONVERT
    public static float ClampColorSpace(float f)
    {
        return Mathf.Clamp(f, 0f, 1f);
    }

    public static float GammaToLinearSpace(float value)
    {
        if (value <= 0.04045f)
        {
            return value / 12.92f;
        }
        if (value < 1f)
        {
            return Mathf.Pow((value + 0.055f) / 1.055f, 2.4f);
        }
        return Mathf.Pow(value, 2.2f);
    }

    public static float LinearToGammaSpace(float value)
    {
        if (value <= 0f)
        {
            return 0f;
        }
        if (value <= 0.0031308f)
        {
            return 12.92f * value;
        }
        if (value < 1f)
        {
            return 1.055f * Mathf.Pow(value, 0.4166667f) - 0.055f;
        }
        return Mathf.Pow(value, 0.45454544f);
    }

    public static float GammaToLinearSpaceRaw(float f)
    {
        return Mathf.Pow(f, 2.2f);
    }

    public static float LinearToGammaSpaceRaw(float f)
    {
        return Mathf.Pow(f, 0.45454544f);
    }

    public static double GammaToLinearSpaceRaw(double d)
    {
        return Math.Pow(d, 2.2);
    }

    public static double LinearToGammaSpaceRaw(double d)
    {
        return Math.Pow(d, 0.45454545454545453);
    }

    public static Color GammaColorize(Color color, int samples, bool raw = false)
    {
        Color color2 = color;
        for (int i = 0; i < samples; i++)
        {
            color2 = ConvertColorToGamma(color2, raw);
        }
        return color2;
    }

    public static Color LinearColorize(Color color, int samples, bool raw = false)
    {
        Color color2 = color;
        for (int i = 0; i < samples; i++)
        {
            color2 = ConvertColorToLinear(color2, raw);
        }
        return color2;
    }

    public static Color ConvertColorToGamma(Color color, bool raw = false)
    {
        if (raw)
        {
            return new Color(LinearToGammaSpaceRaw(color.r), LinearToGammaSpaceRaw(color.g), LinearToGammaSpaceRaw(color.b), color.a);
        }
        return new Color(LinearToGammaSpace(color.r), LinearToGammaSpace(color.g), LinearToGammaSpace(color.b), color.a);
    }

    public static Color ConvertColorToLinear(Color color, bool raw = false)
    {
        if (raw)
        {
            return new Color(GammaToLinearSpaceRaw(color.r), GammaToLinearSpaceRaw(color.g), GammaToLinearSpaceRaw(color.b), color.a);
        }
        return new Color(GammaToLinearSpace(color.r), GammaToLinearSpace(color.g), GammaToLinearSpace(color.b), color.a);
    }
#endregion
#region TEXTURE
    public static Texture2D MakeTexture(int width, int height, Color color, TextureFormat format = TextureFormat.RGBA32)
    {
        Color[] array = new Color[width * height];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = color;
        }
        Texture2D texture2D = new Texture2D(width, height, format, false);
        texture2D.SetPixels(array);
        texture2D.Apply();
        return texture2D;
    }
    public static Texture2D CreateBlankNormalMap(int width, int height)
    {
        return CreateBlankTextureMap(width, height, new Color(0.5f, 0.5f, 1f, 1f));
    }

    public static Texture2D CreateBlankTextureMap(int width, int height, Color color)
    {
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture2D.Clear(color);
        return texture2D;
    }

    public static Texture2D EncodeMetallicTextureMap(ref Texture2D metallicTexture, float metalness, float glossiness)
    {
        int width = metallicTexture.width;
        int height = metallicTexture.height;
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Color[] array = new Color[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                array[j * width + i].r = metalness;
                array[j * width + i].a = glossiness;
            }
        }
        texture2D.SetPixels(array);
        texture2D.Apply();
        return texture2D;
    }

    public static Texture2D RenderExportTexture(ref Texture2D rawTexture, bool gammaCorrection = false, bool flipFaces = false, bool rgbaFloat = false, bool encodeRgbd = false)
    {
        float num = gammaCorrection ? 0.45454544f : 1f;
        Material material = new Material(Shader.Find("Hidden/EncodeTexture"));
        material.SetTexture("_MainTex", rawTexture);
        material.SetFloat("_GammaOut", num);
        material.SetFloat("_EncodeHDR", (float)(encodeRgbd ? 1 : 0));
        material.SetInt("_FlipY", flipFaces ? 1 : 0);
        if (rgbaFloat)
        {
            return rawTexture.RenderToTexture(TextureFormat.RGBAFloat, material, 24, RenderTextureFormat.ARGBFloat, 0);
        }
        return rawTexture.RenderToTexture(TextureFormat.ARGB32, material, 24, 0, 0);
    }

    public static Texture2D RenderExportTextureSize(ref Texture2D rawTexture, int width, int height, bool gammaCorrection = false, bool flipFaces = false, bool rgbaFloat = false, bool encodeRgbd = false)
    {
        float num = gammaCorrection ? 0.45454544f : 1f;
        Material material = new Material(Shader.Find("Hidden/EncodeTexture"));
        material.SetTexture("_MainTex", rawTexture);
        material.SetFloat("_GammaOut", num);
        material.SetFloat("_EncodeHDR", (float)(encodeRgbd ? 1 : 0));
        material.SetInt("_FlipY", flipFaces ? 1 : 0);
        if (rgbaFloat)
        {
            return rawTexture.RenderToTextureSize(width, height, TextureFormat.RGBAFloat, material, 24, RenderTextureFormat.ARGBFloat, 0);
        }
        return rawTexture.RenderToTextureSize(width, height, TextureFormat.ARGB32, material, 24, 0, 0);
    }

    public static Texture2D RenderExportTextureColor(ref Texture2D rawTexture, bool gammaCorrection = false, bool flipFaces = false, bool rgbaFloat = false, Color? texColor = null, bool encodeRgbd = false)
    {
        float num = gammaCorrection ? 0.45454544f : 1f;
        Material material = new Material(Shader.Find("Hidden/EncodeTextureColor"));
        material.SetTexture("_MainTex", rawTexture);
        material.SetFloat("_GammaOut", num);
        material.SetFloat("_EncodeHDR", (float)(encodeRgbd ? 1 : 0));
        material.SetColor("_TexColor", (texColor != null) ? texColor.Value : Color.white);
        material.SetInt("_FlipY", flipFaces ? 1 : 0);
        if (rgbaFloat)
        {
            return rawTexture.RenderToTexture(TextureFormat.RGBAFloat, material, 24, RenderTextureFormat.ARGBFloat, 0);
        }
        return rawTexture.RenderToTexture(TextureFormat.ARGB32, material, 24, 0, 0);
    }

    public static Texture2D RenderExportTextureColorSize(ref Texture2D rawTexture, int width, int height, bool gammaCorrection = false, bool flipFaces = false, bool rgbaFloat = false, Color? texColor = null, bool encodeRgbd = false)
    {
        float num = gammaCorrection ? 0.45454544f : 1f;
        Material material = new Material(Shader.Find("Hidden/EncodeTextureColor"));
        material.SetTexture("_MainTex", rawTexture);
        material.SetFloat("_GammaOut", num);
        material.SetFloat("_EncodeHDR", (float)(encodeRgbd ? 1 : 0));
        material.SetColor("_TexColor", (texColor != null) ? texColor.Value : Color.white);
        material.SetInt("_FlipY", flipFaces ? 1 : 0);
        if (rgbaFloat)
        {
            return rawTexture.RenderToTextureSize(width, height, TextureFormat.RGBAFloat, material, 24, RenderTextureFormat.ARGBFloat, 0);
        }
        return rawTexture.RenderToTextureSize(width, height, TextureFormat.ARGB32, material, 24, 0, 0);
    }

    public static Texture2D RenderExportLightmap(Texture2D rawTexture, bool flipFaces = false)
    {
        Material material = new Material(Shader.Find("Hidden/EncodeLightmap"));
        material.SetTexture("_MainTex", rawTexture);
        material.SetInt("_FlipY", flipFaces ? 1 : 0);
        material.SetInt("_MODE", 0);
        return rawTexture.RenderToTexture(TextureFormat.ARGB32, material, 24, 0, 0);
    }
    public enum TextureFileFormat
    {
        png,
        jpg,
        tga,
        exr,
        unknown
    }
    public static byte[] EncodeTexture(Texture2D tex, TextureFileFormat format) =>
        format switch
        {
            TextureFileFormat.jpg => tex.EncodeToJPG(),
            TextureFileFormat.png => tex.EncodeToPNG(),
            TextureFileFormat.tga => tex.EncodeToTGA(),
            TextureFileFormat.exr => tex.EncodeToEXR(),
            _ => null
        };

    public static Texture2D GenerateTerrainHeights(Terrain terrain)
    {
        Texture2D texture2D = null;
        if (terrain != null)
        {
            int heightmapResolution = terrain.terrainData.heightmapResolution;
            int heightmapResolution2 = terrain.terrainData.heightmapResolution;
            float[,] heights = terrain.terrainData.GetHeights(0, 0, heightmapResolution, heightmapResolution2);
            texture2D = new Texture2D(heightmapResolution, heightmapResolution2, TextureFormat.RGBAFloat, false);
            List<Color> list = new List<Color>();
            for (int i = 0; i < heightmapResolution2; i++)
            {
                for (int j = 0; j < heightmapResolution; j++)
                {
                    float num = heights[i, j];
                    list.Add(new Color(num, num, num, 1f));
                }
            }
            texture2D.SetPixels(list.ToArray());
            texture2D.Apply();
        }
        return texture2D;
    }
#endregion
#region MESH
    public static (string[], int[]) GetBlendshapesInfo(Mesh mesh)
    {
        int blendShapeCount = mesh.blendShapeCount;
        string[] blendShapeNames = new string[blendShapeCount];
        int[] blendShapeIndexes = new int[blendShapeCount];
        for (int i = 0; i < blendShapeCount; i++)
        {
            blendShapeNames[i] = mesh.GetBlendShapeName(i);
            blendShapeIndexes[i] = i;
        }
        return (blendShapeNames,blendShapeIndexes);
    }
    public static Vector2[] RemapTextureAtlasCoordinates(Vector2[] source, int index, Rect[] uvs, bool lerp = false)
    {
        Vector2[] array = new Vector2[source.Length];
        for (int i = 0; i < source.Length; i++)
        {
            if (lerp)
            {
                float xMin = uvs[index].xMin;
                float xMax = uvs[index].xMax;
                float yMin = uvs[index].yMin;
                float yMax = uvs[index].yMax;
                array[i].x = Mathf.Lerp(xMin, xMax, source[i].x);
                array[i].y = Mathf.Lerp(yMin, yMax, source[i].y);
            }
            else
            {
                array[i].x = source[i].x * uvs[index].width + uvs[index].x;
                array[i].y = source[i].y * uvs[index].height + uvs[index].y;
            }
        }
        return array;
    }
    public static Mesh CreateBillboardMesh(BillboardAsset billboard, bool speedTree = false, bool recalculateMesh = false)
    {
        Mesh mesh = new Mesh();
        float width = billboard.width;
        float height = billboard.height;
        float bottom = billboard.bottom;
        Vector2[] vertices = billboard.GetVertices();
        mesh.SetVertices((from v in vertices
                          select new Vector3((v.x - 0.5f) * width, v.y * height + bottom, 0f)).ToList<Vector3>());
        mesh.SetNormals(Enumerable.Repeat<Vector3>(Vector3.forward, billboard.vertexCount).ToList<Vector3>());
        mesh.SetColors(Enumerable.Repeat<Color>(Color.white, billboard.vertexCount).ToList<Color>());
        mesh.SetUVs(0, vertices.ToList<Vector2>());
        mesh.SetUVs(1, vertices.ToList<Vector2>());
        mesh.SetTriangles((from v in billboard.GetIndices()
                           select (int)v).ToList<int>(), 0);
        if (speedTree)
        {
            int num = mesh.uv.Length;
            List<Vector2> list = new List<Vector2>();
            Vector2[] collection = RemapTextureAtlasCoordinates(mesh.uv, 0, new Rect[]
            {
                    new Rect(0.33f, 0f, 0.33f, 0.33f)
            }, false);
            list.AddRange(collection);
            mesh.SetUVs(0, list.ToList<Vector2>());
            mesh.SetUVs(1, list.ToList<Vector2>());
        }
        if (recalculateMesh)
        {
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
        return mesh;
    }

#endregion
#region BUILD
    public static void RebuildProjectSourceCode()
    {
        string text = "REBUILD";
        string text2 = text + ";";
        string text3 = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        if (text3.IndexOf(text2) >= 0)
        {
            text3 = text3.Replace(text2, "");
        }
        else if (text3.IndexOf(text) >= 0)
        {
            text3 = text3.Replace(text, "");
        }
        else if (!string.IsNullOrWhiteSpace(text3))
        {
            text3 = text2 + text3;
        }
        else
        {
            text3 = text + text3;
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, text3);
    }
#endregion
#region TYPE
    public static string GetClassRootNamespace(Type klass)
    {
        int num = klass.FullName.IndexOf(".");
        if (num != -1)
        {
#if UNITY_2021_1_OR_NEWER
            return klass.FullName[..num];
#else
            return klass.FullName.Substring(0, num);
#endif
        }
        return "PROJECT";
    }
    public static BindingFlags FullBinding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

    public static BindingFlags StaticBinding = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;
    public static object GetInstanceField(Type type, object instance, string fieldName)
    {
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty;
        return type.GetField(fieldName, bindingAttr).GetValue(instance);
    }
    public static T[] GetAssetsOfType<T>(string fileExtension) where T : UnityEngine.Object
    {
        List<T> list = new List<T>();
        FileInfo[] files = new DirectoryInfo(Application.dataPath).GetFiles("*" + fileExtension, SearchOption.AllDirectories);
        int i = 0;
        int num = files.Length;
        while (i < num)
        {
            FileInfo fileInfo = files[i];
            if (fileInfo != null)
            {
                T t = AssetDatabase.LoadAssetAtPath<T>(fileInfo.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets"));
                if (!(t == null) && t != null)
                {
                    list.Add(t);
                }
            }
            i++;
        }
        return list.ToArray();
    }

    public static T GetReflectionField<T>(object source, string name)
    {
        object obj = null;
        FieldInfo field = source.GetType().GetField(name, FullBinding);
        if (field != null)
        {
            obj = field.GetValue(source);
        }
        return (T)((object)obj);
    }

    public static T GetReflectionProperty<T>(object source, string name)
    {
        object obj = null;
        PropertyInfo property = source.GetType().GetProperty(name, FullBinding);
        if (property != null)
        {
            obj = property.GetValue(source, null);
        }
        return (T)((object)obj);
    }

    public static void SetReflectionField(object source, string name, object value)
    {
        FieldInfo field = source.GetType().GetField(name, FullBinding);
        if (field != null)
        {
            field.SetValue(source, value);
        }
    }

    public static void SetReflectionProperty(object source, string name, object value)
    {
        PropertyInfo property = source.GetType().GetProperty(name, FullBinding);
        if (property != null)
        {
            property.SetValue(source, value, null);
        }
    }

    public static void CallReflectionMethod(object source, string method, params object[] args)
    {
        CallReflectionMethod<object>(source, method, args);
    }

    public static T CallReflectionMethod<T>(object source, string method, params object[] args)
    {
        object obj = null;
        MethodInfo method2 = source.GetType().GetMethod(method, FullBinding);
        if (method2 != null)
        {
            obj = method2.Invoke(source, args);
        }
        return (T)((object)obj);
    }

    public static T GetStaticReflectionField<T>(Type type, string name)
    {
        object obj = null;
        FieldInfo field = type.GetField(name, StaticBinding);
        if (field != null)
        {
            obj = field.GetValue(null);
        }
        return (T)((object)obj);
    }

    public static T GetStaticReflectionProperty<T>(Type type, string name)
    {
        object obj = null;
        PropertyInfo property = type.GetProperty(name, StaticBinding);
        if (property != null)
        {
            obj = property.GetValue(null, null);
        }
        return (T)((object)obj);
    }

    public static void SetStaticReflectionField(Type type, string name, object value)
    {
        FieldInfo field = type.GetField(name, StaticBinding);
        if (field != null)
        {
            field.SetValue(null, value);
        }
    }

    public static void SetStaticReflectionProperty(Type type, string name, object value)
    {
        PropertyInfo property = type.GetProperty(name, StaticBinding);
        if (property != null)
        {
            property.SetValue(null, value, null);
        }
    }

    public static void CallStaticReflectionMethod(Assembly assembly, string type, string method, params object[] args)
    {
        CallStaticReflectionMethod<object>(assembly.GetType(type), method, args);
    }

    public static void CallStaticReflectionMethod(Type type, string method, params object[] args)
    {
        CallStaticReflectionMethod<object>(type, method, args);
    }

    public static T CallStaticReflectionMethod<T>(Assembly assembly, string type, string method, params object[] args)
    {
        return CallStaticReflectionMethod<T>(assembly.GetType(type), method, args);
    }

    public static T CallStaticReflectionMethod<T>(Type type, string method, params object[] args)
    {
        object obj = null;
        MethodInfo method2 = type.GetMethod(method, StaticBinding);
        if (method2 != null)
        {
            obj = method2.Invoke(null, args);
        }
        return (T)((object)obj);
    }

    public static Type GetTypeFromAllAssemblies(string typeName)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            foreach (Type type in assemblies[i].GetTypes())
            {
                if (type.Name.Equals(typeName, StringComparison.CurrentCultureIgnoreCase) || type.Name.Contains("+" + typeName))
                {
                    return type;
                }
            }
        }
        return null;
    }
    public static string GetUserAccountName()
    {
        string result = null;
        string name = "UnityEditor.Connect.UnityConnect";
        object staticReflectionProperty = GetStaticReflectionProperty<object>(Assembly.Load("UnityEditor.dll").GetType(name), "instance");
        if (staticReflectionProperty != null && staticReflectionProperty.GetType() != null && GetReflectionProperty<bool>(staticReflectionProperty, "loggedIn"))
        {
            string text = CallReflectionMethod<string>(staticReflectionProperty, "GetUserName", new object[0]);
            if (!string.IsNullOrWhiteSpace(text))
            {
                result = text;
            }
        }
        return result;
    }

    public static Component GetScriptComponent(Transform transform, string name)
    {
        Component result = null;
        foreach (Component component in transform.GetComponents(typeof(MonoBehaviour)))
        {
            if (component.GetType().FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = component;
                break;
            }
        }
        return result;
    }

#endregion
#region EDITOR_WINDOW
    public static void ReportProgress(float value, string message = "")
    {
        EditorUtility.DisplayProgressBar("Babylon Toolkit", message, value);
    }

    public static bool ShowMessage(string message, string title = "BVA Toolkit", string ok = "OK", string cancel = "")
    {
        return EditorUtility.DisplayDialog(title, message, ok, cancel);
    }
    public static EditorWindow GetAssetStoreWindow()
    {
        EditorWindow result = null;
        Type type = typeof(EditorWindow).Assembly.GetType("UnityEditor.AssetStoreWindow");
        if (type != null)
        {
            result = (EditorWindow)type.GetMethod("Init", StaticBinding).Invoke(null, null);
        }
        return result;
    }

    public static EditorWindow AttachToAssetStoreWindow(string title, string url)
    {
        EditorWindow result = null;
        EditorWindow assetStoreWindow = GetAssetStoreWindow();
        if (assetStoreWindow != null)
        {
            object instanceField = GetInstanceField(assetStoreWindow.GetType(), assetStoreWindow, "webView");
            if (instanceField != null && !string.IsNullOrWhiteSpace(url))
            {
                instanceField.GetType().GetMethod("LoadURL", FullBinding).Invoke(instanceField, new object[]
                {
                        url
                });
                result = assetStoreWindow;
                if (!string.IsNullOrWhiteSpace(title))
                {
                    assetStoreWindow.titleContent.text = title;
                }
            }
            assetStoreWindow.Show();
        }
        return result;
    }
    public static EditorWindow CreateWebViewWindow(string url, Rect rect)
    {
        string name = "UnityEditor.Web.WebViewEditorWindowTabs";
        Type type = Assembly.Load("UnityEditor.dll").GetType(name);
        BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        object obj = type.GetMethod("Create", bindingAttr).MakeGenericMethod(new Type[]
        {
                type
        }).Invoke(null, new object[]
        {
                "WebView",
                url,
                rect.x,
                rect.y,
                rect.width,
                rect.height
        });
        if (obj == null)
        {
            return null;
        }
        return (EditorWindow)obj;
    }
#endregion
#region VALIDATE
    public static void FixDefaultProjectSetttings()
    {
        BVAConfig.FixProjectEnvironment();
    }
    public static List<string> ValidateRenderPipelineSettings()
    {
        List<string> result = new List<string>();
        if (!BVAConfig.HasValidRenderPipelineAsset())
        {
            result.Add("A valid Universal Render Pipeline Asset is required for build the App that using BVA");
        }
        return result;
    }
    public static List<string> ValidateColorSpaceSettings()
    {
        List<string> result = new List<string>();
        if (PlayerSettings.colorSpace != ColorSpace.Linear)
        {
            result.Add("COLORSPACE: Linear color space should be enabled on Player Settings Panel for best results. Currently set to " + PlayerSettings.colorSpace.ToString());
        }
        return result;
    }

    public static List<string> ValidateReflectionProbeSettings()
    {
        List<string> result = new List<string>();
        if (Graphics.activeTier == GraphicsTier.Tier1)
        {
            if (EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, GraphicsTier.Tier1).reflectionProbeBlending)
            {
                result.Add("GRAPHICS TIER 1: Reflection probe blending not supported. Disable reflection probe blending on Tier 1 Settings Panel for best results.");
            }
        }
        else if (Graphics.activeTier == GraphicsTier.Tier2)
        {
            if (EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, GraphicsTier.Tier2).reflectionProbeBlending)
            {
                result.Add("GRAPHICS TIER 2: Reflection probe blending not supported. Disable reflection probe blending on Tier 2 Settings Panel for best results.");
            }
        }
        else if (Graphics.activeTier == GraphicsTier.Tier3 && EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, GraphicsTier.Tier3).reflectionProbeBlending)
        {
            Debug.LogWarning("GRAPHICS TIER 3: Reflection probe blending not supported. Disable reflection probe blending on Tier 3 Settings Panel for best results.");
        }
        return result;
    }
    public static bool ValidateTextureCoordinates(Vector2[] uvs)
    {
        bool result = true;
        if (uvs != null && uvs.Length != 0)
        {
            for (int i = 0; i < uvs.Length; i++)
            {
                if (uvs[i].x < 0f || uvs[i].x > 1f || uvs[i].y < 0f || uvs[i].y > 1f)
                {
                    result = false;
                    break;
                }
            }
        }
        return result;
    }

    public static List<string> ValidatePlayerSettings()
    {
        List<string> result = new List<string>();
        if (PlayerSettings.assemblyVersionValidation)
        {
            result.Add("Assembly Version Validation may cause error when building Application");
        }
        return result;
    }
    public static List<string> ValidateLightmapSettings()
    {
        List<string> result = new List<string>();
        if (Lightmapping.realtimeGI)
        {
            result.Add("LIGHTMAPS: Realtime global illumination should be disabled on Lighting Panel for best results.");
        }
        if (!Lightmapping.bakedGI)
        {
            result.Add("LIGHTMAPS: Baked global illumination should be enabled on Lighting Panel for best results.");
        }

        if (Lightmapping.bakedGI)
        {
            if (GetLightmapBakeMode() != MixedLightingMode.Subtractive)
            {
                result.Add("LIGHTMAPS: Subtractive lighting mode is recommended and should not be selected on Lighting Panel for best results.");
            }
            if (GetLightmapDirectionMode() != LightmapsMode.NonDirectional)
            {
                //Debug.LogWarning("LIGHTMAPS: Non directional lighting mode should be selected on Lighting Panel for best results.");
            }
            if (GetLightmapCompressionEnabled())
            {
                result.Add("LIGHTMAPS: Texture compression mode should be disabled on Lighting Panel for best results.");
            }
        }
        return result;
    }
    public static List<string> ValidateTextureSize(Texture2D texture)
    {
        List<string> result = new List<string>();
        if (texture != null && (texture.width > 8192 || texture.height > 8192))
        {
            int num = (texture.height > texture.width) ? texture.height : texture.width;
            result.Add(string.Format("Texture exceeds the recommended max resolution: {0} - {1}", new object[]
            {
                    texture.name,
                    num
            }));
        }
        return result;
    }
#endregion
#region ANIMATION
    public static List<AnimationClip> GetAnimationClips(Animator animator)
    {
        List<AnimationClip> list = new List<AnimationClip>();
        if (animator != null && animator.runtimeAnimatorController != null && animator.runtimeAnimatorController.animationClips != null && animator.runtimeAnimatorController.animationClips.Length != 0)
        {
            AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
            if (animatorController != null && animatorController.layers.Length != 0 && animatorController.animationClips != null && animatorController.animationClips.Length != 0)
            {
                AnimationClip animationClip = null;
                for (int i = 0; i < animatorController.layers.Length; i++)
                {
                    AnimatorStateMachine stateMachine = animatorController.layers[i].stateMachine;
                    if (stateMachine.defaultState != null && stateMachine.defaultState.motion != null && stateMachine.defaultState.motion is AnimationClip)
                    {
                        animationClip = (stateMachine.defaultState.motion as AnimationClip);
                        break;
                    }
                }
                if (animationClip != null)
                {
                    animationClip.legacy = false;
                    list.Add(animationClip);
                }
                for (int j = 0; j < animatorController.animationClips.Length; j++)
                {
                    AnimationClip animationClip2 = animatorController.animationClips[j];
                    if (animationClip2 != null && !list.Contains(animationClip2))
                    {
                        animationClip2.legacy = false;
                        list.Add(animationClip2);
                    }
                }
            }
        }
        return list;
    }
    public static List<AnimationClip> GetAnimationClips(Animation legacy)
    {
        List<AnimationClip> list = new List<AnimationClip>();
        if (legacy != null)
        {
            AnimationClip animationClip = (legacy.clip != null) ? legacy.clip : null;
            if (animationClip != null)
            {
                animationClip.legacy = true;
                list.Add(animationClip);
            }
            foreach (AnimationClip animationClip2 in AnimationUtility.GetAnimationClips(legacy.gameObject))
            {
                if (animationClip2 != null && !list.Contains(animationClip2))
                {
                    animationClip2.legacy = true;
                    list.Add(animationClip2);
                }
            }
        }
        return list;
    }
    public static bool HasGenericCurves(AnimationClip clip)
    {
        bool flag = true;
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
        if (curveBindings != null)
        {
            foreach (EditorCurveBinding editorCurveBinding in curveBindings)
            {
                flag = (editorCurveBinding.propertyName.ToLower().Contains("m_localposition") || editorCurveBinding.propertyName.ToLower().Contains("m_localscale") || editorCurveBinding.propertyName.ToLower().Contains("localrotation") || editorCurveBinding.propertyName.ToLower().Contains("localeuler"));
                if (flag)
                {
                    break;
                }
            }
        }
        return flag;
    }

    public static bool HasHumanoidCurves(AnimationClip clip)
    {
        bool flag = clip.humanMotion;
        if (!flag)
        {
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
            if (curveBindings != null)
            {
                foreach (EditorCurveBinding editorCurveBinding in curveBindings)
                {
                    flag = (editorCurveBinding.type == typeof(Animator));
                    if (flag)
                    {
                        break;
                    }
                }
            }
        }
        return flag;
    }

    public static bool HasBlendshapeCurves(AnimationClip clip)
    {
        bool flag = true;
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
        if (curveBindings != null)
        {
            EditorCurveBinding[] array = curveBindings;
            for (int i = 0; i < array.Length; i++)
            {
                flag = array[i].propertyName.ToLower().Contains("blendshape");
                if (flag)
                {
                    break;
                }
            }
        }
        return flag;
    }

#endregion
#region SCENE
    public static Transform[] GetSceneTransforms()
    {
        return Array.ConvertAll<GameObject, Transform>(SceneManager.GetActiveScene().GetRootGameObjects(), (GameObject gameObject) => gameObject.transform);
    }
    public static Transform[] GetSelectedTransforms()
    {
        if (Selection.transforms.Length == 0)
        {
            throw new Exception("No objects selected, cannot export.");
        }
        return Selection.transforms;
    }
    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
#endregion
#region LIGHTMAP 
    public static bool IsBakingTransform(Transform transform)
    {
        bool result = false;
        Light component = transform.GetComponent<Light>();
        if (component != null)
        {
            result = (component.type == LightType.Area || component.lightmapBakeType == LightmapBakeType.Baked);
        }
        return result;
    }
    public static MixedLightingMode GetLightmapBakeMode()
    {
        MixedLightingMode result = 0;
        LightingSettings lightingSettings = GetLightingSettings();
        if (lightingSettings != null)
        {
            result = lightingSettings.mixedBakeMode;
        }
        else
        {
            Debug.LogWarning("LIGHTMAPS: Failed to get lighting editor settings");
        }
        return result;
    }

    public static LightmapsMode GetLightmapDirectionMode()
    {
        LightmapsMode result = LightmapsMode.CombinedDirectional;
        LightingSettings lightingSettings = GetLightingSettings();
        if (lightingSettings != null)
        {
            result = lightingSettings.directionalityMode;
        }
        else
        {
            Debug.LogWarning("LIGHTMAPS: Failed to get lighting editor settings");
        }
        return result;
    }

    public static bool GetLightmapCompressionEnabled()
    {
        bool result = true;
        LightingSettings lightingSettings = GetLightingSettings();
        if (lightingSettings != null)
        {
#if UNITY_2021_1_OR_NEWER
            result = lightingSettings.lightmapCompression != LightmapCompression.None;
#endif
        }
        else
        {
            Debug.LogWarning("LIGHTMAPS: Failed to get lighting editor settings");
        }
        return result;
    }
    public bool GetLightmapEncodingFullHDREnabled()
    {
        return Shader.IsKeywordEnabled("UNITY_LIGHTMAP_FULL_HDR");
    }
    public static LightingSettings GetLightingSettings()
    {
        return Lightmapping.lightingSettings;
    }

    public static float[] FormatLightmapUvFloats(Vector2[] uvs)
    {
        List<float> list = new List<float>();
        if (uvs != null && uvs.Length != 0)
        {
            foreach (Vector2 vector in uvs)
            {
                list.Add(vector.x);
                list.Add(vector.y);
            }
        }
        if (list.Count <= 0)
        {
            return null;
        }
        return list.ToArray();
    }

    public static float GetLightmapIntensity(Material material)
    {
        if (!(material != null) || !material.HasProperty("_LightmapIntensity"))
        {
            return 1f;
        }
        return material.GetFloat("_LightmapIntensity");
    }

    public static Vector2[] UpdateLightmapCoords(Mesh mesh, int lightmapIndex, Vector4 lightmapOffset)
    {
        Vector2[] result;
        if (lightmapIndex >= 0 && lightmapIndex <= 65000)
        {
            result = UnityTools.RemapTextureAtlasCoordinates(mesh.uv2, 0, new Rect[]
            {
                    new Rect(lightmapOffset.z, lightmapOffset.w, lightmapOffset.x, lightmapOffset.y)
            }, false);
        }
        else
        {
            result = mesh.uv2;
        }
        return result;
    }

    public static Vector2[] UpdateFallbackLightmapCoords(Mesh mesh, int lightmapIndex, Vector4 lightmapOffset)
    {
        Vector2[] result;
        if (lightmapIndex >= 0 && lightmapIndex <= 65000)
        {
            result = UnityTools.RemapTextureAtlasCoordinates(mesh.uv, 0, new Rect[]
            {
                    new Rect(lightmapOffset.z, lightmapOffset.w, lightmapOffset.x, lightmapOffset.y)
            }, false);
        }
        else
        {
            result = mesh.uv;
        }
        return result;
    }
#endregion
#region NORMALMAP
    public static bool IsNormalMap(string propertyName)
    {
        return propertyName.Contains("_BumpMap") || propertyName.Contains("NormalMap");
    }
#endregion
}
