using UnityEngine;
using BVA;
using System.Linq;
using BVA.Extensions;
using UnityEngine.UI;
namespace BVA.Sampler
{
    public class MultipleScenePayload : SceneViewer
    {
        public Transform gltfFile;
        public Transform gltfScene;
        public Transform fileUI;
        public Transform sceneUI;

        public GameObject NewGltfFilePrefab(BVAScene scene)
        {
            var gameObject = Instantiate(fileUI.gameObject);
            gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text = scene.name;
            gameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => RebuildSceneSelectGUI(scene));
            gameObject.transform.GetChild(1).GetComponentInChildren<Text>().text = "Unload";
            gameObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            { 
                BVASceneManager.Instance.UnloadScene(scene);
                Destroy(gameObject);
                gltfScene.DestroyAllChild();
            });
            gameObject.SetActive(true);
            return gameObject;
        }
        private void RebuildSceneSelectGUI(BVAScene scene)
        {
            gltfScene.DestroyAllChild();

            if (scene.importer != null)
            {
                for (int i = 0; i < scene.GetSceneCount(); i++)
                {
                    int index = i;
                    var gameObject = Instantiate(sceneUI.gameObject);
                    gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text = scene.importer.Root.Scenes[index].Name;
                    gameObject.transform.GetChild(1).GetComponent<Toggle>().SetIsOnWithoutNotify(index == 0);
                    gameObject.transform.GetChild(1).GetComponent<Toggle>().onValueChanged.AddListener((isOn) =>
                    {
                        if (isOn)
                        {
                            if (!scene.IsSceneLoaded(index))
                            {
                                scene.LoadSceneAsync(index);
                            }
                            else
                            {
                                scene.SetActive(index,true);
                            }
                        }
                        else
                        {
                            scene.SetActive(index,false);
                        }
                    });
                    gameObject.SetActive(true);
                    gameObject.transform.SetParent(gltfScene, false);
                }
            }
            else
            {
                var gameObject = Instantiate(fileUI.gameObject);
                gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text = scene.name;
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                gameObject.SetActive(true);
                gameObject.transform.SetParent(gltfScene.transform, false);
            }
        }
        public void RebuildGUI()
        {
            var all = BVASceneManager.Instance.GetAllScenes().Concat(BVASceneManager.Instance.GetAllAvatars());
            gltfFile.DestroyAllChild();
            foreach (var scene in all)
            {
                var file = NewGltfFilePrefab(scene);
                file.transform.SetParent(gltfFile, false);
            }
        }
        public override void OnLoaded(AssetType assetType, BVAScene scene)
        {
            base.OnLoaded(assetType, scene);
            Debug.Log(assetType.ToString() + scene.name);
            RebuildGUI();
        }

        public void OpenSceneFile()
        {
            OpenFile(AssetType.Scene);
        }

        public void ClearScene()
        {
            var assets = FindObjectsOfType<AssetManager>();
            foreach (var asset in assets)
                Destroy(asset);
        }

        public void ListAllScenes(GLTFSceneImporter importer)
        {

        }

        public void ListAllFiles()
        {

        }
    }
}