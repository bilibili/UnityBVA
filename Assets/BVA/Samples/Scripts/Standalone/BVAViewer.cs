using UnityEngine;
using UnityEngine.UI;
using BVA.Component;

namespace BVA.Sampler
{
    public class BVAViewer : SceneViewer
    {
        public AnimationPanel animationPanel;
        public AudioPanel audioPanel;
        public BlendshapeMixerPanel blendshapeMixerPanel;
        public CameraPanel cameraPanel;
        public SkyboxPanel skyboxPanel;
        public PostProcessPanel postprocessPanel;
        public TimelinePanel timelinePanel;
        public Text logText;
        public Light mainLight;
        public Toggle toggleLight;
        public void OpenAvatarFile() { OpenFile(AssetType.Avatar); }
        public void OpenSceneFile() { OpenFile(AssetType.Scene); }
        public void OpenCommonFile() { OpenFile(AssetType.Common); }
        bool activeWindow = false;
        public void LogCallback(string condition, string stackTrace, LogType type)
        {
            //if(type == LogType.Error)
            {
                logText.text += condition;
                logText.text = stackTrace;
            }
        }
        private void Start()
        {
            SetActive(false);
            logText.text = "";
            Application.logMessageReceived += LogCallback;
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SetActive(!activeWindow);
            }
        }
        private void SetActive(bool active)
        {
            activeWindow = active;
            animationPanel.gameObject.SetActive(active);
            audioPanel.gameObject.SetActive(active);
            blendshapeMixerPanel.gameObject.SetActive(active);
            skyboxPanel.gameObject.SetActive(active);
            cameraPanel.gameObject.SetActive(active);
            postprocessPanel.gameObject.SetActive(active);
            timelinePanel.gameObject.SetActive(active);
        }

        public void ToggleLight(bool enable)
        {
            mainLight.gameObject.SetActive(toggleLight.isOn);
        }
        public override void OnLoaded(AssetType assetType, BVAScene scene)
        {
            base.OnLoaded(assetType, scene);

            if (assetType == AssetType.Avatar)
            {
                LoadAvatarPanel(LastLoadedScene.gameObject);
            }
        }
        public void LoadAvatarPanel(GameObject root)
        {
            var audio = root.GetComponent<AudioClipContainer>();
            animationPanel.gameObject.SetActive(true);
            if (audio != null)
            {
                audioPanel.gameObject.SetActive(true);
                audioPanel.SetAudioContainer(audio);
            }
            var blendshape = root.GetComponentInChildren<BlendShapeMixer>();
            if (blendshape != null)
            {
                blendshapeMixerPanel.gameObject.SetActive(true);
                blendshapeMixerPanel.SetBlendShapeMixer(blendshape);
            }
            cameraPanel.gameObject.SetActive(true);
            cameraPanel.SetCameras();
            postprocessPanel.gameObject.SetActive(false);

            timelinePanel.gameObject.SetActive(true);
            timelinePanel.Set(root.GetComponentInChildren<PlayableController>());

            animationPanel.Set(root.GetComponent<AssetManager>(), root.transform.GetChild(0).GetComponent<Animator>());

            var skybox = root.GetComponent<SkyboxContainer>();
            if (skybox != null)
            {
                skyboxPanel.gameObject.SetActive(true);
                skyboxPanel.SetSkybox(skybox);
            }
        }
    }
}