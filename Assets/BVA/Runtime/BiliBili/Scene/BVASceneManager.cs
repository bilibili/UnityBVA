using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BVA.Extensions;
using System.Threading.Tasks;
using BVA.Loader;
using System.IO;
using System.Linq;
using System;

namespace BVA
{
    public struct BVAScene
    {
        public Transform mainScene;
        public AssetType type;
        public string assetPath;
        public GLTFSceneImporter importer;
        /// <summary>
        /// Scenes root except default scene
        /// </summary>
        public Transform[] scenes;
        public bool IsValid() { return mainScene != null && type != AssetType.Unknown; }
        public string name { get { return mainScene.name; } set { mainScene.name = value; } }
        public void Destroy(float t = 0.0f)
        {
            if (mainScene != null)
            {
                GameObject.Destroy(mainScene.gameObject, t);
                mainScene = null;
            }
            if (scenes != null)
            {
                foreach (Transform scene in scenes)
                {
                    if (scene.gameObject != null)
                        GameObject.Destroy(scene.gameObject);
                }
            }
            scenes = null;
            importer = null;
        }
        public bool IsSceneLoaded(int index)
        {
            if (index == 0)
                return mainScene != null;
            else
                return index > 0 && index < scenes.Length && scenes[index] != null;
        }
        public int GetSceneCount()
        {
            if (!IsValid()) return 0;
            if (importer == null) return 1;
            return importer.Root.Scenes.Count;
        }
        public async void LoadSceneAsync(int index, bool showScene = true, IProgress<ImportProgress> progress = null)
        {
            if (importer == null) return;
            await importer.LoadSceneAsync(index, showScene, null, System.Threading.CancellationToken.None, progress);
            scenes[index] = importer.LastLoadedScene.transform;
        }
        public void SetActive(int index, bool active)
        {
            if (index < 0) return;
            if (index == 0) SetActive(active);
            if (scenes != null && index < scenes.Length && scenes[index] != null)
            {
                scenes[index].gameObject.SetActive(active);
            }
        }
        public void SetActive(bool active)
        {
            mainScene.gameObject.SetActive(active);
        }
    }
    /// <summary>
    /// the default scene will be load immediately, for other multi-scene exist in files, cached it here,then try to reuse it 
    /// </summary>
    public class BVASceneManager : MonoSingleton<BVASceneManager>
    {
        private ImporterFactory Factory;
        /// <summary>
        /// All loaded scenes
        /// </summary>
        public List<BVAScene> loadedScenes;
        /// <summary>
        /// All scenes in which exist in memories but not be loaded yet
        /// </summary>
        public List<GLTFSceneImporter> cachedSceneImporters;
        /// <summary>
        /// All loaded avatar's animator
        /// </summary>
        public List<BVAScene> loadedAvatars;
        /// <summary>
        /// Call after loaded a scene
        /// </summary>
        public delegate void OnSceneLoadedDelegate(AssetType assetType, BVAScene scene);
        public OnSceneLoadedDelegate onSceneLoaded;
        /// <summary>
        /// Last loaded scene root
        /// </summary>
        public BVAScene LastLoadedScene;
        /// <summary>
        /// All loaded scenes name
        /// </summary>
        private Queue<Action> onSceneLoadedDelegates;
        public string[] loadedSceneNames => loadedScenes.Select(x => x.name).ToArray();
        public BVASceneManager()
        {
            loadedScenes = new List<BVAScene>();
            onSceneLoadedDelegates = new Queue<Action>();
            cachedSceneImporters = new List<GLTFSceneImporter>();
            loadedAvatars = new List<BVAScene>();
        }
        public void Update()
        {
            if (onSceneLoadedDelegates.Count > 0)
            {
                var callback = onSceneLoadedDelegates.Dequeue();
                if (callback != null)
                    callback();
            }

        }
        private string GetUniqueSceneName(string sceneName)
        {
            RemoveInvalidScene();
            return sceneName.GetUniqueName(loadedSceneNames);
        }

        public void AddScene(BVAScene scene)
        {
            loadedScenes.Add(scene);
        }

        public void AddAvatar(BVAScene avatar)
        {
            loadedAvatars.Add(avatar);
        }

        public void AddScenePayload(GLTFSceneImporter importer)
        {
            LogPool.ImportLogger.Log(LogPart.Scene, $"multiple scene added, scene count : {importer.Root.Scenes.Count}, Main Scene : {importer.Root.Scenes[importer.Root.Scene.Id]}");
            cachedSceneImporters.Add(importer);
        }

        public Transform CreateScene(string sceneName)
        {
            sceneName = GetUniqueSceneName(sceneName);
            GameObject sceneRoot = new GameObject(sceneName);
            BVAScene scene = new BVAScene()
            {
                type = AssetType.Created,
                mainScene = sceneRoot.transform
            };
            loadedScenes.Add(scene);
            return sceneRoot.transform;
        }

        public BVAScene GetSceneAt(int index)
        {
            if (loadedScenes.Count <= index)
                return new BVAScene();
            return loadedScenes[index];
        }

        public BVAScene GetSceneByName(string sceneName)
        {
            return loadedScenes.Find(x => x.name == sceneName);
        }

        /// <summary>
        /// Get scenes with same prefix name
        /// eg: Cube, Cube(0), Cube(1) will all be returned when search Cube
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public BVAScene[] GetScenesByName(string sceneName)
        {
            return loadedScenes.FindAll(x =>
            {
                if (x.name == sceneName)
                    return true;
                if (x.name.EndsWith(")"))
                {
                    int startIndex = x.name.LastIndexOf('(');
                    int endIndex = x.name.LastIndexOf(')');
                    string prefixName = x.name.Substring(0, startIndex);
                    if (startIndex > 0 && endIndex > 0 && endIndex > startIndex && prefixName == sceneName)
                    {

                        string numberStr = x.name.Substring(startIndex + 1, endIndex - startIndex - 2);
                        if (int.TryParse(numberStr, out int number)) return true;
                    }
                }
                return false;
            }).ToArray();
        }

        internal void AddLoadedScene(BVAScene scene)
        {
            scene.name = GetUniqueSceneName(scene.name);
            if (scene.type == AssetType.Avatar)
            {
                loadedAvatars.Add(scene);
            }
            else
            {
                loadedScenes.Add(scene);
            }
        }
#if UNITY_WEBGL //&& !UNITY_EDITOR

        public async Task LoadAvatar(string glbPath)
        {
            var importOptions = new ImportOptions
            {
                RuntimeImport = true,
            };
            var Factory = ScriptableObject.CreateInstance<DefaultImporterFactory>();
            var sceneImporter = Factory.CreateSceneImporter(glbPath, importOptions);
            bool isSuccess = true;
            try
            {
                await sceneImporter.LoadAvatar();

            }
            catch (Exception ex)
            {
                isSuccess = false;
                Debug.LogError
                    (ex.Message);
                Debug.LogError
                    (ex.StackTrace);
            }
            if (isSuccess)
            {
                LastLoadedScene = new BVAScene()
                {
                    mainScene = sceneImporter.LastLoadedScene.transform,
                    assetPath = glbPath,
                    type = AssetType.Avatar,
                    importer = sceneImporter
                };
                AddLoadedScene(LastLoadedScene);
                onSceneLoaded?.Invoke(AssetType.Avatar, LastLoadedScene);
            }
            else
            {
                Debug.LogError("Error occured!");
            }
        }
#else
        public async Task LoadAvatar(string glbPath)
        {
            var importOptions = new ImportOptions { RuntimeImport = true };
            string directoryPath = URIHelper.GetDirectoryName(glbPath);
            if (glbPath.IsUrl())
            {
                importOptions.DataLoader = new WebRequestLoader(glbPath);
            }
            else
            {
                importOptions.DataLoader = new FileLoader(directoryPath);
            }
            var Factory = ScriptableObject.CreateInstance<DefaultImporterFactory>();
            var sceneImporter = Factory.CreateSceneImporter(glbPath, importOptions);
            bool isSuccess = true;
            try
            {
                await sceneImporter.LoadAvatar();
            }
            catch
            {
                isSuccess = false;
            }
            if (isSuccess)
            {
                LastLoadedScene = new BVAScene()
                {
                    mainScene = sceneImporter.LastLoadedScene.transform,
                    assetPath = glbPath,
                    type = AssetType.Avatar,
                    importer = sceneImporter
                };
                AddLoadedScene(LastLoadedScene);
                onSceneLoaded?.Invoke(AssetType.Avatar, LastLoadedScene);
            }
            else
            {
                LogPool.RuntimeLogger.LogError(LogPart.Avatar, "Load avatar error occurred!");
            }
        }

#endif
        public void LoadScene(string gltfUri)
        {
            Load(gltfUri).RunSynchronously();
        }

        public void LoadAllScenes(string gltfUri)
        {
            Load(gltfUri, true, false).RunSynchronously();
        }

        public async Task LoadSceneAsync(string gltfUri)
        {
            await Load(gltfUri);
        }
        public async Task LoadAllScenesAsync(string gltfUri)
        {
            await Load(gltfUri, true, false);
        }

        public void MergeScenes(Transform sourceScene, Transform destScene)
        {
            if (sourceScene == null || destScene == null || sourceScene == destScene) return;
            while (destScene.childCount > 0)
            {
                destScene.GetChild(0).SetParent(sourceScene);
            }
        }

        public void UnloadScene(string sceneName, float t = 0.0f)
        {
            BVAScene scene = GetSceneByName(sceneName);
            if (scene.IsValid())
            {
                loadedScenes.Remove(scene);
                scene.Destroy(t);
            }
        }

        public void UnloadScene(BVAScene scene, float t = 0.0f)
        {
            if (!scene.IsValid())
            {
                return;
            }
            if (loadedScenes.Contains(scene))
            {
                loadedScenes.Remove(scene);
                scene.Destroy(t);
            }
            else if (loadedAvatars.Contains(scene))
            {
                loadedAvatars.Remove(scene);
                scene.Destroy(t);
            }
            else
            {
                LogPool.RuntimeLogger.LogError(LogPart.Scene, "The scene root you try to destroy is not exist in loaded scenes");
            }
        }

        public void UnloadAvatar(BVAScene avatar, float t = 0.0f)
        {
            if (loadedAvatars.Contains(avatar))
            {
                loadedAvatars.Remove(avatar);
                avatar.Destroy(t);
            }
            else
            {
                LogPool.RuntimeLogger.LogError(LogPart.Scene, "The avatar root you try to destroy is not exist in loaded scenes");
            }
        }

        public void UnloadScene(IEnumerable<string> sceneNames, float t = 0.0f)
        {
            foreach (string sceneName in sceneNames)
            {
                UnloadScene(sceneName, t);
            }
        }

        public void UnloadScene(IEnumerable<BVAScene> scenes, float t = 0.0f)
        {
            while (scenes.Count() > 0)
                UnloadScene(scenes.First(), t);
        }

        public void UnloadAllScenes()
        {
            UnloadScene(loadedAvatars);
            UnloadScene(loadedScenes);
        }

        public void RemoveInvalidScene()
        {
            loadedScenes.RemoveAll(x => x.mainScene == null);
        }

        public void MoveGameObjectToScene(GameObject go, BVAScene scene)
        {
            if (scene.IsValid())
            {
                go.transform.SetParent(scene.mainScene, true);
            }
            else
            {
                LogPool.RuntimeLogger.LogError(LogPart.Scene, "The scene root is not valid in loaded scenes");
            }
        }

        public void MoveGameObjectToScene(GameObject go, string sceneName)
        {
            BVAScene scene = GetSceneByName(sceneName);
            if (scene.IsValid())
            {
                go.transform.SetParent(scene.mainScene, true);
            }
            else
            {
                LogPool.RuntimeLogger.LogError(LogPart.Scene, "The scene root you try to destroy is not exist in loaded scenes");
            }
        }

        public BVAScene[] GetAllScenes()
        {
            return loadedScenes.ToArray();
        }

        public BVAScene[] GetAllAvatars()
        {
            return loadedAvatars.ToArray();
        }

        public AssetType GetAssetType(string URL)
        {
            if (URL.EndsWith(BVAConst.EXTENSION_BVA_AVATAR))
                return AssetType.Avatar;
            else if (URL.EndsWith(BVAConst.EXTENSION_BVA))
                return AssetType.Scene;
            else if (URL.EndsWith(BVAConst.EXTENSION_GLB) || URL.EndsWith(BVAConst.EXTENSION_GLTF))
                return AssetType.StandardGLTF;
            else return AssetType.Unknown;
        }

        /// <summary>
        /// Load a glb file async
        /// </summary>
        /// <param name="gltfURL">The url to the file</param>
        /// <param name="multithreaded">Multithreaded loading scene</param>
        /// <param name="loadDefaultSceneOnly">If true, destroy SceneImporter after scene loaded ,cause we won't use it in the future</param>
        /// <returns></returns>
        private async Task Load(string gltfURL, bool multithreaded = true, bool loadDefaultSceneOnly = true)
        {
            var importOptions = new ImportOptions
            {
                AsyncCoroutineHelper = gameObject.GetComponent<AsyncCoroutineHelper>() ?? gameObject.AddComponent<AsyncCoroutineHelper>(),
                RuntimeImport = true
            };

            AssetType assetType = GetAssetType(gltfURL);
            GLTFSceneImporter sceneImporter;
            //try
            {
                Factory ??= ScriptableObject.CreateInstance<DefaultImporterFactory>();


                if (gltfURL.IsUrl())
                {
                    string directoryPath = URIHelper.GetDirectoryName(gltfURL);
                    importOptions.DataLoader = new WebRequestLoader(directoryPath);
                    sceneImporter = Factory.CreateSceneImporter(URIHelper.GetFileFromUri(new Uri(gltfURL)), importOptions);
                }
                else
                {
                    var fileName = Path.GetFileName(gltfURL);
                    var ext = Path.GetExtension(gltfURL).ToLower();
                    if (ext == ".zip")
                    {
                        importOptions.DataLoader = new ZipFileLoader(gltfURL);
                        fileName = fileName.Replace("zip", "gltf");
                    }
                    else
                    {
                        string directoryPath = URIHelper.GetDirectoryName(gltfURL);
                        importOptions.DataLoader = new FileLoader(directoryPath);
                    }
                    sceneImporter = Factory.CreateSceneImporter(fileName, importOptions);
                }
                sceneImporter.SceneParent = null;
                sceneImporter.IsMultithreaded = multithreaded;
                await sceneImporter.LoadAsync();

                LogPool.RuntimeLogger.Log(LogPart.Scene, "model loaded with vertices: " + sceneImporter.Statistics.VertexCount.ToString() + ", triangles: " + sceneImporter.Statistics.TriangleCount.ToString());

                LastLoadedScene = new BVAScene()
                {
                    mainScene = sceneImporter.LastLoadedScene.transform,
                    assetPath = gltfURL,
                    type = assetType,
                    importer = sceneImporter
                };

                if (sceneImporter.Root.Scenes.Count > 1)
                {
                    LastLoadedScene.scenes = new Transform[sceneImporter.Root.Scenes.Count];
                    LastLoadedScene.scenes[0] = sceneImporter.LastLoadedScene.transform;
                    if (!loadDefaultSceneOnly)
                    {
                        for (int i = 1; i < sceneImporter.Root.Scenes.Count; i++)
                        {
                            await sceneImporter.LoadSceneAsync(i);
                            LastLoadedScene.scenes[i] = sceneImporter.LastLoadedScene.transform;
                        }
                    }
                }
                AddLoadedScene(LastLoadedScene);
                onSceneLoadedDelegates.Enqueue(() => onSceneLoaded(assetType, LastLoadedScene));
            }
            //finally
            {
                // Destroy GLTFSceneImporter immediately if nothing is needed anymore
                if (!loadDefaultSceneOnly && sceneImporter.Root.Scenes.Count <= 1)
                {
                    if (importOptions.DataLoader != null)
                    {
                        sceneImporter?.Dispose();
                        importOptions.DataLoader = null;
                    }
                }
                else
                {
                    cachedSceneImporters.Add(sceneImporter);
                }
            }
        }
    }
}