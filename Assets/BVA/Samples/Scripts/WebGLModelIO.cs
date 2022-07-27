#if UNITY_STANDALONE || UNITY_WEBGL
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BVA;
using BVA.Extensions;
using System.Threading.Tasks;
using BVA.Loader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.Networking;
using VRM;
using UniGLTF;
public class WebGLModelIO : MonoBehaviour
{
    #region ExternFunction

    /// <summary>
    /// 打开文件夹选择文件
    /// </summary>
    /// <param name="gameObjectName"></param>
    /// <param name="methodName"></param>
    /// <param name="filter"></param>
    /// <param name="multiple"></param>
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);
    /// <summary>
    /// 下载指定数据到文件
    /// </summary>
    /// <param name="gameObjectName"></param>
    /// <param name="methodName"></param>
    /// <param name="filename"></param>
    /// <param name="byteArray"></param>
    /// <param name="byteArraySize"></param>
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    #endregion

    public enum ModelType
    {
        GLB,
        MMD,
        VRM,
    }

    [HideInInspector]
    public GameObject Model;

    [HideInInspector]
    public ModelType AvatarModelType;

    public Action<GameObject> LoadFinishCallbackFunction;

    private Vector3 _defaultPosition = Vector3.zero;
    private Quaternion _defaultRotation = Quaternion.Euler(0, 180, 0);

    #region Upload
    public void LoadGLB()
    {
#if UNITY_EDITOR
        string[] paths = SFB.StandaloneFileBrowser.OpenFilePanel("PMX", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("GLB Avatar Files", "glb") }, false);
        if (paths.Length == 0) return;
        LoadGLBCallBack(paths[0]);
#elif UNITY_WEBGL
        UploadFile(gameObject.name, "LoadGLBCallBack", ".glb", false);
#endif
    }
    public void LoadVRM()
    {
#if UNITY_EDITOR
        string[] path = SFB.StandaloneFileBrowser.OpenFilePanel("VRM File", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("VRM Files", "vrm") }, false);
        if (path.Length == 0) return;
        LoadVRMCallBack(path[0]);
#elif UNITY_WEBGL
        UploadFile(gameObject.name, "LoadVRMCallBack", ".vrm", false);
#endif
    }


    public async void LoadGLBCallBack(string path)
    {
        Debug.Log("模型路径（url）:" + path);
        BVASceneManager.Instance.onSceneLoaded += (type, scene) =>
        {
            Transform alicia = scene.mainScene;
            alicia.position = _defaultPosition;
            alicia.rotation = _defaultRotation;
            Model = alicia.gameObject;
        };
        //UnityWebRequest uwr = UnityWebRequest.Get(path);
        //await uwr.SendWebRequest();
        //var sd  = new MemoryStream(uwr.downloadHandler.data);
        
        await BVASceneManager.Instance.LoadAvatar(path);
        AvatarModelType = ModelType.GLB;
        LoadFinishCallbackFunction?.Invoke(Model);
    }

    public async void LoadVRMCallBack(string path)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(path);
        await uwr.SendWebRequest();
        // load into scene
        var data = new GlbLowLevelParser(path, uwr.downloadHandler.data).Parse();
        // VRM extension を parse します
        var vrm = new VRMData(data);
        var context = new VRMImporterContext(vrm);
        var loaded = context.Load();
        loaded.EnableUpdateWhenOffscreen();
        loaded.ShowMeshes();
        loaded.gameObject.name = loaded.name;

        Transform alicia = loaded.gameObject.transform;
        alicia.position = _defaultPosition;
        alicia.rotation = _defaultRotation;
        Model = alicia.gameObject;

        //modelType = ModelType.VRM;

        //Remove VRM Scripts to BVA
        VRMMetaObject meta = Model.GetComponent<VRMMeta>().Meta;
        BVA.Component.BVAMetaInfoScriptableObject bvaMetaInfo = ScriptableObject.CreateInstance<BVA.Component.BVAMetaInfoScriptableObject>();
        bvaMetaInfo.formatVersion = meta.ExporterVersion;
        bvaMetaInfo.title = meta.Title;
        bvaMetaInfo.version = meta.Version;
        bvaMetaInfo.author = meta.Author;
        bvaMetaInfo.contact = meta.ContactInformation;
        bvaMetaInfo.reference = meta.Reference;
        bvaMetaInfo.thumbnail = meta.Thumbnail;
        bvaMetaInfo.contentType = BVA.Component.ContentType.Avatar;
        bvaMetaInfo.legalUser = (BVA.Component.LegalUser)Enum.GetNames(typeof(AllowedUser)).ToList().IndexOf(meta.AllowedUser.ToString());
        bvaMetaInfo.violentUsage = (BVA.Component.UsageLicense)Enum.GetNames(typeof(UssageLicense)).ToList().IndexOf(meta.ViolentUssage.ToString());
        bvaMetaInfo.sexualUsage = (BVA.Component.UsageLicense)Enum.GetNames(typeof(UssageLicense)).ToList().IndexOf(meta.SexualUssage.ToString());
        bvaMetaInfo.commercialUsage = (BVA.Component.UsageLicense)Enum.GetNames(typeof(UssageLicense)).ToList().IndexOf(meta.CommercialUssage.ToString());
        bvaMetaInfo.licenseType = (BVA.Component.LicenseType)Enum.GetNames(typeof(LicenseType)).ToList().IndexOf(meta.LicenseType.ToString());
        bvaMetaInfo.customLicenseUrl = meta.OtherPermissionUrl;

        Model.AddComponent<BVA.Component.BVAMetaInfo>().metaInfo = bvaMetaInfo;
        Destroy(Model.GetComponent<VRMMeta>());
        Destroy(Model.GetComponent<VRMHumanoidDescription>());
        Destroy(Model.GetComponent<VRMBlendShapeProxy>());
        Destroy(Model.GetComponent<VRMFirstPerson>());
        Destroy(Model.GetComponent<VRMLookAtHead>());
        Destroy(Model.GetComponent<VRMLookAtBoneApplyer>());

        Shader replaceShader = Shader.Find("VRM/URP/MToon");
        var skinnedMeshRenderers = loaded.gameObject.GetComponentsInChildren<Renderer>();
        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {
            foreach (var v in skinnedMeshRenderer.materials)
            {
                v.shader = replaceShader;
            }
        }

        AvatarModelType = ModelType.VRM;
        LoadFinishCallbackFunction?.Invoke(Model);
    }

#endregion

#region Download
    public void DownloadGLB()
    {
        if (Model == null)
        {
            Debug.LogWarning("未加载模型");
            return;
        }
        if (AvatarModelType == ModelType.MMD)
        {
            GLTFSceneExporter.SeparateMMDMeshProcessing(Model);
        }
        var animator = Model.GetComponent<Animator>();
        animator.StopPlayback();
        Humanoid.SetTPose(animator.avatar, Model.transform);
        GLTFSceneExporter.SetDefaultAvatarExportSetting();
        var exportOptions = new ExportOptions { TexturePathRetriever = null /*(texture) => { return null; }*/ };
        var exporter = new GLTFSceneExporter(new Transform[] { Model.transform }, exportOptions);

        var bytes = exporter.SaveGLBToByteArray(Model.name ,true);
#if UNITY_WEBGL && !UNITY_EDITOR
        DownloadFile(gameObject.name, "DownloadGLBCallBack", Model.name + ".glb", bytes, bytes.Length);
#else
        var path = SFB.StandaloneFileBrowser.SaveFilePanel("Runtime Export", "", Model.name, "avatar.glb");
        File.WriteAllBytes(path, bytes);
        Debug.Log("文件已保存:" + path);
#endif
        Model.transform.position = _defaultPosition;
        Model.transform.rotation = _defaultRotation;
    }

    public void DownloadGLBCallBack()
    {
        Debug.Log("glb文件导出成功！");
    }
#endregion
}
#endif