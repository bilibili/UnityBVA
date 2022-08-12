using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

namespace BVA.Sample
{
    public class LoadFromWeb : MonoBehaviour
    {
        public InputField url;
        public Text textLog;
        public Text textError;
        public new Light light;
        public RuntimeAnimatorController controller;
        public Transform content;
        public GameObject fileButton;
        public Toggle fastLoad;
        [Tooltip("Enable Default Directional Light")]
        public Toggle enableLight;
        [Tooltip("Disable loaded cameras")]
        public Toggle useMainCamera;
        string localUrl;
        public float posShift = 0.3f;
        public int count = 0;
        public Camera[] cameras;

        void OnLightChange(bool isOn)
        {
            light.enabled = isOn;
        }

        void OnUseMainCamera(bool isOn)
        {
            if (cameras == null) return;
            foreach (Camera cam in cameras)
                cam.gameObject.SetActive(!isOn);
        }

        void Start()
        {
            Application.logMessageReceived += OnLog;
            enableLight.onValueChanged.AddListener(OnLightChange);
            useMainCamera.onValueChanged.AddListener(OnUseMainCamera);

            BVASceneManager.Instance.onSceneLoaded = (type, scene) =>
            {
                Transform sceneRootTransform = scene.mainScene;
                Animator animator = sceneRootTransform.GetComponent<Animator>() ?? sceneRootTransform.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    animator.runtimeAnimatorController = controller;
                    float pos = (count % 2 == 0 ? 1 : -1) * posShift * ((count + 1) / 2);
                    sceneRootTransform.transform.position = new Vector3(pos, sceneRootTransform.transform.position.y, sceneRootTransform.transform.position.z);
                    count++;
                }

                cameras = scene.mainScene.GetComponentsInChildren<Camera>();
                foreach (var cam in cameras)
                {
                    var cameraController = cam.gameObject.AddComponent<OrbitCameraController>();
                    cameraController.ResetCamera(sceneRootTransform);
                }
            };

            string[] files = Directory.GetFiles(Application.persistentDataPath);
            var glbs = files.Where(f => f.EndsWith(".glb") || f.EndsWith(".bva"));
            foreach (string glb in glbs)
            {
                var button = GameObject.Instantiate(fileButton, content, false);
                button.SetActive(true);
                button.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(glb);
                button.GetComponent<Button>().onClick.AddListener(async () =>
                {
                    await (fastLoad.isOn ? BVASceneManager.Instance.LoadAvatar(glb) : BVASceneManager.Instance.LoadSceneAsync(glb));
                });
            }
        }

        private void OnLog(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Log)
                textLog.text += condition + "\n";
            if (type == LogType.Error)
                textError.text += condition + "\n";
        }

        public void Download()
        {
            StartCoroutine(DownloadFile());
        }

        IEnumerator DownloadFile()
        {
            var uwr = new UnityWebRequest(url.text, UnityWebRequest.kHttpVerbGET);
            localUrl = Path.Combine(Application.persistentDataPath, Path.GetFileName(url.text));
            uwr.downloadHandler = new DownloadHandlerFile(localUrl);
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(uwr.error);
            else
                Debug.Log("File successfully downloaded and saved to " + localUrl);
        }

        public async void Load()
        {
            if (localUrl != null)
                await (fastLoad.isOn ? BVASceneManager.Instance.LoadAvatar(localUrl) : BVASceneManager.Instance.LoadSceneAsync(localUrl));
            else
                Debug.Log("没有下载文件");
        }

        public void DestroyScene()
        {
            BVASceneManager.Instance.UnloadAllScenes();
        }
    }
}