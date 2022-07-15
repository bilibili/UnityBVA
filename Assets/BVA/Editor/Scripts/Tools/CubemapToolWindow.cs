using UnityEditor;
using BVA.Extensions;
using UnityEngine;
using System.IO;
using System.Linq;

public class CubemapToolWindow : EditorWindow
{
    int m_mode;
    static string[] EDIT_MODES = new[]{
            "Textures2Cubemap",
            "Cubemap2Texture",
            "Panorama2Cubemap"
        };
    readonly string[] sizesText = new string[] { "64", "128", "256", "512", "1024", "2048" };
    readonly int[] sizes = new int[] { 64, 128, 256, 512, 1024, 2048 };
    [MenuItem("BVA/Developer Tools/Cubemap Tool")]
    static void Init()
    {
        CubemapToolWindow window = (CubemapToolWindow)GetWindow(typeof(CubemapToolWindow), false, "Cubemap Tool");
        window.Show();
    }
    private void OnEnable()
    {
        if (packTextures == null)
            packTextures = new Texture2D[6];
    }
    private void OnGUI()
    {
        m_mode = GUILayout.Toolbar(m_mode, EDIT_MODES);
        switch (m_mode)
        {
            case 0:
                Textures2CubemapGUI();
                break;
            case 1:
                Cubemap2TextureGUI();
                break;
            case 2:
                Panorama2CubemapGUI();
                break;
        }
    }
    private void DelayImportAsCubemap(string fullPath, bool compress = false)
    {
        EditorApplication.delayCall += () =>
        {
            var unityPath = fullPath.UnityAssetPath();
            Debug.Log(unityPath);
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(unityPath);
            if (importer != null && importer.textureShape != TextureImporterShape.TextureCube)
            {
                importer.textureShape = TextureImporterShape.TextureCube;
                if (!compress)
                {
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                }
                importer.SaveAndReimport();
                cubemap = AssetDatabase.LoadAssetAtPath<Cubemap>(unityPath);
                EditorGUIUtility.PingObject(cubemap);
            }
        };
    }

    #region Unpack

    Cubemap cubemap;
    ReflectionProbe probe;
    Texture2D[] textures;
    private void Cubemap2TextureGUI()
    {
        EditorGUILayout.Space();
        cubemap = EditorGUILayout.ObjectField("Cubemap :", cubemap, typeof(Cubemap), false) as Cubemap;

        EditorGUILayout.BeginHorizontal();
        probe = EditorGUILayout.ObjectField("Camera Position:", probe, typeof(ReflectionProbe), true) as ReflectionProbe;
        if (GUILayout.Button("Render2Cube"))
        {
            cubemap = new Cubemap(probe.resolution, TextureFormat.ARGB32,false);
            GameObject gameObject = new GameObject("CubemapCamera");
            var cam = gameObject.AddComponent<Camera>();
            gameObject.transform.position = probe.transform.position;
            gameObject.transform.rotation = Quaternion.identity;
            cam.RenderToCubemap(cubemap);
            DestroyImmediate(gameObject);
        }
        EditorGUILayout.EndHorizontal();
        if (textures != null)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = EditorGUILayout.ObjectField("Texture :", textures[i], typeof(Texture2D), false) as Texture2D;
            }
        }

        cubemapName = EditorGUILayout.TextField("Output name :", cubemapName);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("6 Textures"))
        {
            Unpack(WriteCubemapTo6Texture);
        }
        if (GUILayout.Button("Row"))
        {
            Unpack((x) => WriteCubemapToRow(x, false));
        }
        if (GUILayout.Button("Column"))
        {
            Unpack((x) => WriteCubemapToRow(x, true));
        }
        EditorGUILayout.EndHorizontal();
    }
    private void WriteCubemapTo6Texture(string savePath)
    {
        int i = 0;
        string[] filePaths = new string[6];
        foreach (Texture2D texture in cubemap.UnpackTextures())
        {
            filePaths[i] = savePath + "/" + cubemapName + ".png";
            File.WriteAllBytes(filePaths[i], texture.EncodeToPNG());
            i++;
        }
        AssetDatabase.Refresh();

        textures = new Texture2D[filePaths.Length];
        for (i = 0; i < filePaths.Length; i++)
        {
            string path = filePaths[i].UnityAssetPath();
            textures[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
    }

    private void WriteCubemapToRow(string savePath, bool isColumn)
    {
        Texture2D flatedTexture = cubemap.FlatTexture(isColumn);
        var filePath = savePath + "/" + cubemapName + ".png";
        File.WriteAllBytes(filePath, flatedTexture.EncodeToPNG());
        AssetDatabase.Refresh();
    }
    private void Unpack(System.Action<string> writeTextureFunc)
    {
        if (cubemap == null) return;
        if (string.IsNullOrWhiteSpace(cubemapName))
        {
            EditorUtility.DisplayDialog("wrong name", "use a valid name for cubemap", "OK");
            return;
        }
        textures = null;
        var assetPath = AssetDatabase.GetAssetPath(cubemap);
        string savePath = null;
        if(!string.IsNullOrEmpty(assetPath))
        {
            var importer = TextureImporter.GetAtPath(assetPath) as TextureImporter;
            if (!importer.isReadable || importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                importer.isReadable = true;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }
            savePath = EditorUtility.SaveFolderPanel("save path", Path.GetDirectoryName(assetPath), "");
        }
        else
        {
            savePath = Path.GetDirectoryName(EditorUtility.SaveFilePanelInProject("Save Cubemap", cubemapName, "png", ""));
        }
        if (string.IsNullOrEmpty(savePath)) return;
        if (Directory.Exists(savePath))
        {
            writeTextureFunc(savePath);
        }
        DelayImportAsCubemap($"{savePath}/{cubemapName}.png");
    }
    #endregion

    #region Pack
    Texture2D[] packTextures;
    string errorLog;
    string cubemapName;
    bool changeSize;
    int selectedSizeIndex = 3;
    private void Textures2CubemapGUI()
    {
        EditorGUILayout.Space();
        if (packTextures == null) packTextures = new Texture2D[6];
        for (int i = 0; i < 6; i++)
        {
            packTextures[i] = EditorGUILayout.ObjectField(((CubemapFace)i).ToString(), packTextures[i], typeof(Texture2D), false) as Texture2D;
        }
        cubemapName = EditorGUILayout.TextField("Output name :", cubemapName);
        errorLog = null;

        string currentTextureSize = "";
        if (packTextures[0] != null)
        {
            currentTextureSize = $"now {packTextures[0].width}";
            changeSize = EditorGUILayout.Toggle("set size:" + currentTextureSize, changeSize);
            if (changeSize)
                selectedSizeIndex = EditorGUILayout.Popup("Size :", selectedSizeIndex, sizesText);
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Panorama"))
        {
            Pack(SaveCubemapAsEquirect, true);
        }
        if (GUILayout.Button("Row"))
        {
            Pack((a, b) => SaveCubemapAsFlat(a, b, false), true);
        }
        if (GUILayout.Button("Column"))
        {
            Pack((a, b) => SaveCubemapAsFlat(a, b, true), true);
        }
        EditorGUILayout.EndHorizontal();
        if (errorLog != null)
        {
            EditorGUILayout.HelpBox(errorLog, MessageType.Error);
        }
    }
    private void Pack(System.Action<Texture2D[], string> saveFunction, bool modifyReadable)
    {
        if (string.IsNullOrWhiteSpace(cubemapName))
        {
            EditorUtility.DisplayDialog("wrong name", "use a valid name for cubemap", "OK");
            return;
        }
        if (packTextures.Any(x => x == null))
        {
            errorLog = "not all face's texture are filled";
        }
        if (packTextures.Any(x => x.width != packTextures[0].width || x.height != packTextures[0].width))
        {
            errorLog = "not all face's texture are same size";
        }
        if (errorLog == null)
        {
            if (modifyReadable)
            {
                foreach (var texture in packTextures)
                {
                    var importer = TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(texture)) as TextureImporter;
                    if (importer != null && importer.isReadable == true && importer.textureCompression == TextureImporterCompression.Uncompressed)
                        continue;
                    importer.isReadable = true;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.SaveAndReimport();
                }
            }
            EditorApplication.delayCall += () =>
            {
                string dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(packTextures[0]));
                var savePath = EditorUtility.SaveFolderPanel("save path", dir, "");
                savePath = $"{savePath}/{cubemapName}.png";
                saveFunction(packTextures, savePath);
            };
        }
    }
    private void SaveCubemapAsEquirect(Texture2D[] aMap, string aPath)
    {
        int width = changeSize ? aMap[0].width : sizes[selectedSizeIndex];
        byte[] pngData = CubemapExtensions.ConvertToEquirectangular(aMap, width * 2, width);
        File.WriteAllBytes(aPath, pngData);
        AssetDatabase.Refresh();
        DelayImportAsCubemap(aPath);
    }
    private void SaveCubemapAsFlat(Texture2D[] aMap, string aPath, bool isVertical)
    {
        int width = changeSize ? aMap[0].width : sizes[selectedSizeIndex];
        byte[] pngData = CubemapExtensions.ConvertToFlat(aMap, width, isVertical);
        File.WriteAllBytes(aPath, pngData);
        AssetDatabase.Refresh();
        DelayImportAsCubemap(aPath);
    }
    #endregion

    #region Panorama2Cubemap
    Texture2D panoramaTexture;
    float direction;
    int textureSizeIndex = 3;
    bool isColumn;
    private void Panorama2CubemapGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        panoramaTexture = EditorGUILayout.ObjectField(panoramaTexture, typeof(Texture2D), true, GUILayout.MinWidth(200), GUILayout.MaxWidth(200), GUILayout.MinHeight(100), GUILayout.MaxHeight(100)) as Texture2D;
        direction = EditorGUILayout.Slider("Direction", direction, -180.0f, 180.0f);
        textureSizeIndex = EditorGUILayout.Popup("Texture Size", textureSizeIndex, sizesText);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Cubemap"))
        {
            CreateCubemap();
        }
        if (!string.IsNullOrEmpty(errorLog))
        {
            EditorGUILayout.HelpBox(errorLog, MessageType.Error);
        }
    }
    private void CreateCubemap()
    {
        errorLog = null;
        if (panoramaTexture == null)
            errorLog = "Texture can't be null";
        if (panoramaTexture.width != panoramaTexture.height * 2)
            errorLog = "Texture size is not valid, width = height * 2";

        if (!string.IsNullOrEmpty(errorLog))
            return;

        var importer = TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(panoramaTexture)) as TextureImporter;
        if (!importer.isReadable || importer.textureCompression != TextureImporterCompression.Uncompressed)
        {
            importer.isReadable = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }
        int width = sizes[selectedSizeIndex];
        byte[] pngData = CubemapExtensions.ConvertToFlat(panoramaTexture, width, isColumn);
        var savePath = EditorUtility.SaveFilePanelInProject("save path", "", "png", "");
        File.WriteAllBytes(savePath, pngData);
        AssetDatabase.Refresh();
        DelayImportAsCubemap(savePath);
    }
    #endregion
}
