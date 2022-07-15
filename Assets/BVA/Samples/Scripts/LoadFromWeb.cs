using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using BVA;

public class LoadFromWeb : MonoBehaviour
{
    public InputField url;
    public Text textLog;
    public Text textError;
    public RuntimeAnimatorController controller;
    public Transform content;
    public GameObject fileButton;
    string localUrl;
    public float posShift = 0.3f;
    public int count = 0;

    void Start()
    {
        Application.logMessageReceived += OnLog;

        BVASceneManager.Instance.onSceneLoaded = (type, scene) =>
        {
            Transform alicia = scene.mainScene;
            Animator animator = alicia.GetComponent<Animator>() ?? alicia.GetComponentInChildren<Animator>();
            if(animator != null)
                animator.runtimeAnimatorController = controller;
            float pos = (count % 2 == 0 ? 1 : -1) * posShift * ((count+1) / 2);
            alicia.transform.position = new Vector3(pos, alicia.transform.position.y, alicia.transform.position.z);
            count++;

        };

        string[] files = Directory.GetFiles(Application.persistentDataPath);
        var glbs = files.Where(f => f.EndsWith(".glb"));
        foreach (string glb in glbs)
        {
            var button = GameObject.Instantiate(fileButton, content, false);
            button.SetActive(true);
            button.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(glb);
            button.GetComponent<Button>().onClick.AddListener(async () =>
            {
                await BVASceneManager.Instance.LoadAvatar(glb);
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
            await BVASceneManager.Instance.LoadAvatar(localUrl);
        else
            Debug.Log("没有下载文件");
    }
}
