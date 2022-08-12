using UnityEngine;
using UnityEngine.UI;
using BVA.Component;
using BVA.Extensions;

namespace BVA.Sample
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
        public Light mainLight;
        public Toggle toggleLight;
        public void OpenAvatarFile() { OpenFile(AssetType.Avatar); }
        public void OpenSceneFile() { OpenFile(AssetType.Scene); }
        public void OpenCommonFile() { OpenFile(AssetType.Common); }
        bool activeWindow = false;
        private void Start()
        {
            SetActive(false);
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
            else
            {
                LoadScenePanel(LastLoadedScene.gameObject);
            }
            var cameras = scene.mainScene.GetComponentsInChildren<Camera>();
            foreach (var cam in cameras)
            {
                cam.gameObject.AddComponent<OrbitCameraController>();
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
            if (root.TryGetComponent<BlendShapeMixer> (out var blendshape))
            {
                blendshapeMixerPanel.gameObject.SetActive(true);
                blendshapeMixerPanel.SetBlendShapeMixer(blendshape);
            }
           
            cameraPanel.gameObject.SetActive(false);
            postprocessPanel.gameObject.SetActive(false);

            if (root.TryGetComponent<Animation>(out var animation))
            {
                animationPanel.gameObject.SetActive(true);
                animationPanel.Set(root.GetComponent<AssetManager>(), animation);
            }
        }
        public void LoadScenePanel(GameObject root)
        {
            var audio = root.GetComponent<AudioClipContainer>();
            animationPanel.gameObject.SetActive(true);
            if (audio != null)
            {
                audioPanel.gameObject.SetActive(true);
                audioPanel.SetAudioContainer(audio);
            }

            var skybox = root.GetComponent<SkyboxContainer>();
            if (skybox != null)
            {
                skyboxPanel.gameObject.SetActive(true);
                skyboxPanel.SetSkybox(skybox);
            }

            if (root.TryGetComponent<Animation>(out var animation))
            {
                animationPanel.gameObject.SetActive(true);
                animationPanel.Set(root.GetComponent<AssetManager>(), animation);
            }
            cameraPanel.gameObject.SetActive(true);
            cameraPanel.SetCameras();
            //timelinePanel.gameObject.SetActive(true);
            //timelinePanel.Set(root.GetComponentInChildren<PlayableController>());
        }
    }
}