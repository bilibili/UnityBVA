using UnityEngine.UI;
using UnityEngine;
using UniGLTF;
using VRM;
using BVA;
using UniHumanoid;
using System.IO;
using OggVorbis;
using NAudio.Wave;
using System.Runtime.InteropServices;

public class SampleScript : MonoBehaviour
{
    public enum ModelType
    {
        GLB,
        MMD,
        VRM,
    }
    public ModelType modelType;
    public Text logText;
    public Toggle bvhSkeletonToggle;
    public Camera mainCamera;
    public GameObject cameraCenter;
    public AudioSource audioSource;
    public RuntimeAnimatorController SampleAnimatorController;
    Vector3 defaultPosition = Vector3.zero;
    Quaternion defaultRotation = Quaternion.Euler(0, 180, 0);
    GameObject model;
    UnityVMDPlayer vmdPlayer;
    string motionPath;
    UnityVMDCameraPlayer cameraPlayer;
    string musicPath;

    BvhImporterContext m_context;
    HumanPoseTransfer m_dst;
    private void Awake()
    {
        Screen.fullScreen = false;
        Application.logMessageReceived += Application_logMessageReceived;
        bvhSkeletonToggle.onValueChanged.AddListener(OnSwitchSkeleton);
        bvhSkeletonToggle.gameObject.SetActive(false);
    }

    private void OnSwitchSkeleton(bool show)
    {
        m_context.Root.GetComponent<Renderer>().enabled = show;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error)
            logText.text += $"<color=#ff0000ff>{condition}</color>\n";
    }
    public async void LoadGLB()
    {
        string[] paths = SFB.StandaloneFileBrowser.OpenFilePanel("PMX", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("GLB Avatar Files", "glb") }, false);
        if (paths.Length == 0) return;
        BVASceneManager.Instance.onSceneLoaded += (type, scene) =>
        {
            Transform alicia = scene.mainScene;
            alicia.position = defaultPosition;
            alicia.rotation = defaultRotation;
            model = alicia.gameObject;
        };
        CpuWatch.Start();
        await BVASceneManager.Instance.LoadAvatar(paths[0]);
        CpuWatch.Stop();
        modelType = ModelType.GLB;
        logText.text += $"成功加载GLB模型 : {paths[0]} Load Time :{CpuWatch.Ellapsed}s\n";
    }


    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void LoadGLBV2()
    {
        UploadFile(gameObject.name, "LOADGLBV2WithPath", ".glb", false);
    }

    public async void LOADGLBV2WithPath(string path)
    {
        BVASceneManager.Instance.onSceneLoaded += (type, scene) =>
        {
            Transform alicia = scene.mainScene;
            alicia.position = defaultPosition;
            alicia.rotation = defaultRotation;
            model = alicia.gameObject;
        };
        await BVASceneManager.Instance.LoadAvatar(path);
        modelType = ModelType.GLB;
        logText.text += $"成功加载GLB模型 : {path} Load Time :{CpuWatch.Ellapsed}s\n";
    }


    public async void LoadPMX()
    {
        string[] path = SFB.StandaloneFileBrowser.OpenFilePanel("PMX", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("MMD Files", "pmx") }, false);
        if (path.Length == 0) return;
        string humanPath = path[0];
        CpuWatch.Start();
        Transform alicia = await PMXModelLoader.LoadPMXModel(humanPath, SampleAnimatorController);
        alicia.position = defaultPosition;
        alicia.rotation = defaultRotation;
        model = alicia.gameObject;
        alicia.GetComponentInChildren<ADBRuntime.Mono.ADBRuntimeController>()?.RestoreRuntimePoint();
        CpuWatch.Stop();

        modelType = ModelType.MMD;
        if (model.GetComponent<Animator>() != null)
            logText.text += $"成功加载MMD模型 : {humanPath} Load Time :{CpuWatch.Ellapsed}s\n";
        else
            logText.text += $"<color=#ff0000ff>无法加载MMD模型，骨骼出错 : {humanPath}</color>\n";
    }
    public void LoadVRM()
    {
        string[] path = SFB.StandaloneFileBrowser.OpenFilePanel("VRM File", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("VRM Files", "vrm") }, false);
        if (path.Length == 0) return;
        string vrmPath = path[0];
        CpuWatch.Start();
        // load into scene
        var data = new GlbFileParser(vrmPath).Parse();
        // VRM extension を parse します
        var vrm = new VRMData(data);
        var context = new VRMImporterContext(vrm);
        var loaded = context.Load();
        loaded.EnableUpdateWhenOffscreen();
        loaded.ShowMeshes();
        loaded.gameObject.name = loaded.name;

        Transform alicia = loaded.gameObject.transform;
        alicia.position = defaultPosition;
        alicia.rotation = defaultRotation;
        model = alicia.gameObject;
        CpuWatch.Stop();

        modelType = ModelType.VRM;
        logText.text += $"成功加载VRM模型 : {vrmPath}  Load Time :{CpuWatch.Ellapsed}s\n";

        Shader replaceShader = Shader.Find("Shader Graphs/MToon");
        var skinnedMeshRenderers = loaded.gameObject.GetComponentsInChildren<Renderer>();
        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {
            foreach (var v in skinnedMeshRenderer.materials)
            {
                v.shader = replaceShader;
            }
        }
    }
    public void LoadVMDMotion()
    {
        string[] path = SFB.StandaloneFileBrowser.OpenFilePanel("VMD Character Motion", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("MMD Files", "vmd") }, false);
        if (path.Length == 0) return;
        motionPath = path[0];
        vmdPlayer = model.AddComponent<UnityVMDPlayer>();
        vmdPlayer.LeftUpperArmTwist = model.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftUpperArm);
        vmdPlayer.RightUpperArmTwist = model.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightUpperArm);
        logText.text += $"成功加载MMD动作 : {motionPath}\n";
    }

    public void LoadBVHMotion()
    {
        string[] path = SFB.StandaloneFileBrowser.OpenFilePanel("BVH Character Motion", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("BVH Files", "bvh") }, false);
        if (path.Length == 0) return;
        motionPath = path[0];
        if (m_context != null)
        {
            m_context.Destroy(true);
            m_context = null;
        }

        m_context = new BvhImporterContext();
        m_context.Parse(motionPath);
        m_context.Load();
        bvhSkeletonToggle.gameObject.SetActive(true);
        var src = m_context.Root.GetComponent<HumanPoseTransfer>();

        m_dst = model.GetOrAddComponent<HumanPoseTransfer>();
        m_dst.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;
        m_dst.Source = src;
        logText.text += $"成功加载BVH动作 : {motionPath}\n";
    }

    public void LoadVMDCamera()
    {
        string[] path = SFB.StandaloneFileBrowser.OpenFilePanel("VMD Camera Motion", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("MMD Files", "vmd") }, false);
        if (path.Length == 0) { return; }
        cameraPlayer = model.AddComponent<UnityVMDCameraPlayer>();
        cameraPlayer.FrameFromAnimator = false;
        cameraPlayer.Select_VMD = path[0];
        cameraPlayer.CameraCenter = cameraCenter;
        cameraPlayer.MainCamera = mainCamera;
        cameraPlayer.MMD_model = model;
        logText.text += $"成功加载镜头 : {path[0]}\n";
    }
    public async void LoadMusic()
    {
        string[] path = SFB.StandaloneFileBrowser.OpenFilePanel("VMD Camera Motion", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("Music Files", "mp3", "wav", "wma", "ogg", "flac") }, false);
        if (path.Length == 0) { return; }
        musicPath = path[0];
        logText.text += "开始加载音乐，请稍后...\n";
        if (musicPath != null)
        {
            string musicName = Path.GetFileName(musicPath);
            if (musicPath.EndsWith(".mp3"))
            {
                var bytes = File.ReadAllBytes(musicPath);
                var (waveHeader, wavData) = await WaveUtil.ToWaveData(bytes);
                var audioClip = AudioClip.Create(musicName, waveHeader.DataChunkSize / waveHeader.BlockSize, waveHeader.Channel, waveHeader.SampleRate, false);
                audioClip.SetData(wavData, 0);

                GameObject audioObj = new GameObject(audioClip.name);
                audioSource = audioObj.AddComponent<AudioSource>();
                audioSource.clip = audioClip;
            }
            else if (musicPath.EndsWith(".ogg"))
            {
                var bytes = File.ReadAllBytes(musicPath);
                AudioClip audioClip = VorbisPlugin.ToAudioClip(bytes, musicName);

                GameObject audioObj = new GameObject(audioClip.name);
                audioSource = audioObj.AddComponent<AudioSource>();
                audioSource.clip = audioClip;
            }
        }
        logText.text += $"成功加载音乐 : {musicPath}\n";
    }
    public async void Play()
    {
        if (vmdPlayer != null && motionPath != null)
        {
            await vmdPlayer.PlayAsync(motionPath);
            if (audioSource != null)
                audioSource.Play();
        }
        if (cameraPlayer != null)
            cameraPlayer.isPlaying = true;


        if (musicPath != null)
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
            else
            {
                // Open with external application
                Application.OpenURL(musicPath);
            }
        }
    }

    public void DestroyModel()
    {
        Destroy(model);
        if (audioSource != null)
            audioSource.Stop();
        logText.text = "";
    }

    public void ExportAvatar()
    {
        string name = model.name;

        if (model == null)
        {
            logText.text += $"<color=#ff0000ff>模型不存在</color>\n";
            return;
        }
        if (modelType == ModelType.MMD)
        {
            logText.text += $"MMD模型导出进行BlendShape分离\n";
            GLTFSceneExporter.SeparateMMDMeshProcessing(model);
            logText.text += "处理完成Mesh的分离\n";
        }
        GLTFSceneExporter.SetDefaultAvatarExportSetting();
        var exportOptions = new ExportOptions { TexturePathRetriever = null };
        var exporter = new GLTFSceneExporter(new Transform[] { model.transform }, exportOptions);
        var path = SFB.StandaloneFileBrowser.SaveFilePanel("Runtime Export", "", name, "avatar.glb");
        if (!string.IsNullOrEmpty(path))
        {
            exporter.SaveGLB(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path), "avatar");
        }
        model.transform.position = defaultPosition;
        model.transform.rotation = defaultRotation;
    }
}
