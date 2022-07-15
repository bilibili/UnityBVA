using System.Collections.Generic;
using UnityEditor;
using BVA;
using UnityEngine;
using BVA.Component;
using System;
using System.Linq;
using Object = UnityEngine.Object;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace BVA
{

    public class ModelImportFixedMenu : EditorWindow
    {

        public class ConditionSolusionPair<T>
        {
            public Func<T, bool> condition;
            public Func<T, bool> solution;
            public Action helpBox;
        }

        static List<ConditionSolusionPair<TextureImporter>> textureSolutions = new List<ConditionSolusionPair<TextureImporter>>()
       {
           new ConditionSolusionPair<TextureImporter>()
           {
               condition = (x=>!x.isReadable),
               solution=((x)=>{
                    x.isReadable=true;
                    x.SaveAndReimport();
                    return true;
                }),
               helpBox=(()=> EditorGUILayout.HelpBox($"Texture's read/write should be enabled", MessageType.Error))
            },
       };

        static List<ConditionSolusionPair<ModelImporter>> modelSolutions = new List<ConditionSolusionPair<ModelImporter>>()
       {
           new ConditionSolusionPair<ModelImporter>()
           {
               condition = (x=>!x.isReadable),
               solution=(
               (x)=>{
                    x.isReadable=true;
                    x.SaveAndReimport();
                    return true;
                }
               ),
               helpBox=(
               ()=> EditorGUILayout.HelpBox($"Mesh's read/write should be enabled", MessageType.Error)
        )
            }
       };

        static List<ConditionSolusionPair<TextureImporter>> textureSolutionReverts = new List<ConditionSolusionPair<TextureImporter>>()
       {
           new ConditionSolusionPair<TextureImporter>()
           {
               condition = (x=>x.isReadable),
               solution=((x)=>{
                    x.isReadable=false;
                    x.SaveAndReimport();
                    return true;
                }),
               helpBox=(()=> EditorGUILayout.HelpBox($"Revert Texture's read/write", MessageType.Info))
            },
       };

        static List<ConditionSolusionPair<ModelImporter>> modelSolutionReverts = new List<ConditionSolusionPair<ModelImporter>>()
       {
           new ConditionSolusionPair<ModelImporter>()
           {
               condition = (x=>x.isReadable),
               solution=(
               (x)=>{
                    x.isReadable=false;
                    x.SaveAndReimport();
                    return true;
                }
               ),
               helpBox=(
               ()=> EditorGUILayout.HelpBox($"Revert Mesh's read/write", MessageType.Info)
        )
            }
       };


        static string[] EDIT_MODES = new[]{
            "Fixed GameObject",
            "Fixed Scene",
            "Fixed All"
        };

        static ModelImportFixedMenu modelImportWindow;
        private List<TextureImporter> textureImporterList;
        private List<ModelImporter> modelImporterList;
        private bool isShowAllTexture;
        private bool isShowAllModel;
        private SceneAsset mainScene;
        private GameObject mainSelect;
        private int mode;
        private Vector2 _scrollPos;
        private int textureImporterErrorCount;
        private int modelImporterErrorCount;
        private bool isRevert;

        [MenuItem("BVA/Developer Tools/Modify ReadWrite Tool (Mesh&Texture)")]
        public static void Init()
        {
            if (modelImportWindow == null)
            {
                modelImportWindow = (ModelImportFixedMenu)EditorWindow.GetWindow(typeof(ModelImportFixedMenu), false, "Fixed Import Model");
            }
            modelImportWindow.Show();
        }
        private void OnEnable()
        {
            mainScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(UnityEngine.SceneManagement.SceneManager.GetActiveScene().path);
            mainSelect = Selection.activeGameObject;
            textureImporterList = new List<TextureImporter>();
            modelImporterList = new List<ModelImporter>();
            mode = 0;
        }


        void CheckValidity()
        {
            if (modelImporterList == null)
            {
                return;
            }
            bool isFixedAll = false;

            isRevert = EditorGUILayout.Toggle("Is Revert", isRevert);

            if (GUILayout.Button("Fixed all"))
            {
                isFixedAll = true;
            }

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            if (!isRevert)
            {
                textureImporterErrorCount = CollectCount(textureImporterList, textureSolutions);
            }
            else
            {
                textureImporterErrorCount = CollectCount(textureImporterList, textureSolutionReverts);
            }

            isShowAllTexture = EditorGUILayout.BeginFoldoutHeaderGroup(isShowAllTexture, "Show All Textrues :" + textureImporterErrorCount);

            bool isFixedAllTextrue = isFixedAll;
            if (GUILayout.Button("Fixed allTextrue"))
            {
                isFixedAllTextrue = true;
                Undo.RecordObjects(textureImporterList.ToArray(), "Redo Texture");
            }
            if (isShowAllTexture|| isFixedAllTextrue)
            {
                if (isRevert)
                {
                    ShowList(textureImporterList, textureSolutionReverts, isFixedAllTextrue);
                }
                else
                {
                    ShowList(textureImporterList, textureSolutions, isFixedAllTextrue);
                }

            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (!isRevert)
            {
                modelImporterErrorCount = CollectCount(modelImporterList, modelSolutions);
            }
            else
            {
                modelImporterErrorCount = CollectCount(modelImporterList, modelSolutionReverts);
            }

            isShowAllModel = EditorGUILayout.BeginFoldoutHeaderGroup(isShowAllModel, "Show All Model :" + modelImporterErrorCount);

            bool isFixedAllModel = isFixedAll;
            if (GUILayout.Button("Fixed All Model"))
            {
                isFixedAllModel = true;
                Undo.RecordObjects(modelImporterList.ToArray(), "Redo Model");
            }
            if (isShowAllModel|| isFixedAllModel)
            {
                if (isRevert)
                {
                    ShowList(modelImporterList, modelSolutionReverts, isFixedAllModel);
                }
                else
                {
                    ShowList(modelImporterList, modelSolutions, isFixedAllModel);
                }

            }
            EditorGUILayout.EndFoldoutHeaderGroup();
           EditorGUILayout.EndScrollView();
        }
        int CollectCount<T>(List<T> targetImporters, List<ConditionSolusionPair<T>> conditionSolusionPairs)
        {
            int count = 0;
            for (int i = 0; i < conditionSolusionPairs.Count; i++)
            {
                for (int ii = 0; ii < targetImporters.Count; ii++)
                {
                    if (conditionSolusionPairs[i].condition(targetImporters[ii]))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        void ShowList<T>(List<T> targetImporters, List<ConditionSolusionPair<T>> conditionSolusionPairs,bool isFixedAll) where T: AssetImporter
        {
            for (int i = 0; i < conditionSolusionPairs.Count; i++)
            {
                var conditionSolution = conditionSolusionPairs[i];
                var Condition = conditionSolution.condition;
                var Solution = conditionSolution.solution;
                var ShowHelpBox = conditionSolution.helpBox;
                bool isShowMessageBox = false;
                for (int ii = 0; ii < targetImporters.Count; ii++)
                {
                    var importer = targetImporters[ii];

                    if (Condition(importer))
                    {
                        if (!isShowMessageBox)
                        {
                            isShowMessageBox = true;
                            ShowHelpBox();
                        }

                        Object target = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Object));

                        GUILayout.BeginHorizontal();

                        EditorGUILayout.ObjectField("TargetObject", target, typeof(Object), true);

                        if (GUILayout.Button("Select it"))
                        {
                            Selection.SetActiveObjectWithContext(target, null);
                        }
                        if (GUILayout.Button("Fixed it") || isFixedAll)
                        {
                            bool isSuccess = Solution(importer);
                            if (!isSuccess)
                            {
                                Debug.LogError(importer.name + " Cannot be fixed !");
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                }
            }
        }
        void OnGUI()
        {
            int exportMode = GUILayout.Toolbar(mode, EDIT_MODES);
            bool needRefresh = false;
            if (GUILayout.Button("Load")) needRefresh = true;


            switch (mode)
            {
                case 0:
                    mainSelect = EditorGUILayout.ObjectField("TargetObject", mainSelect, typeof(Object), true) as GameObject;
                    break;
                case 1:
                    mainScene = EditorGUILayout.ObjectField("TargetScene", mainScene, typeof(Object), true) as SceneAsset;
                    if (mainScene == null)
                    {
                        EditorGUILayout.HelpBox($"Try save scene and restart this menu", MessageType.Info);
                    }
                    break;
                case 2:
                    break;

                default: break;
            }

            if (exportMode != mode || needRefresh)
            {
                mode = exportMode;
                switch (mode)
                {
                    case 0:
                        GetFromGameObject();
                        break;
                    case 1:
                        GetFromScene();
                        break;
                    case 2:
                        GetFromProject();
                        break;

                    default: break;
                }
            }


            CheckValidity();
            EditorGUILayout.HelpBox($"BVA version {BVAConst.FORMAT_VERSION}", MessageType.Info);
        }

        void GetFromGameObject()
        {
            textureImporterList.Clear();
            modelImporterList.Clear();
            HashSet<string> textureImporterHash = new HashSet<string>();
            if (mainSelect == null)
            {
                return;
            }
            Renderer[] allRenderer = mainSelect.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < allRenderer.Length; i++)
            {
                Renderer renderer = allRenderer[i];
                Material[] materials = renderer.sharedMaterials;
                for (int ii = 0; ii < materials.Length; ii++)
                {
                    Material material = materials[ii];
                    Shader shader = material.shader;
                    for (int iii = 0; iii < ShaderUtil.GetPropertyCount(shader); iii++)
                    {
                        if (ShaderUtil.GetPropertyType(shader, iii) == ShaderUtil.ShaderPropertyType.TexEnv)
                        {
                            Texture texture = renderer.sharedMaterial.GetTexture(ShaderUtil.GetPropertyName(shader, iii));
                            string path = AssetDatabase.GetAssetPath(texture);
                            if (!string.IsNullOrEmpty(path) && !textureImporterHash.Contains(path))
                            {
                                textureImporterHash.Add(path);
                            }
                        }
                    }
                }
            }
            IEnumerable<TextureImporter> textureImporters = textureImporterHash.Select(x => AssetImporter.GetAtPath(x) as TextureImporter);
            textureImporterList.AddRange(textureImporters);


            HashSet<string> meshPathHash = new HashSet<string>();

            SkinnedMeshRenderer[] allMeshRenderer = mainSelect.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < allMeshRenderer.Length; i++)
            {
                SkinnedMeshRenderer meshRenderer = allMeshRenderer[i];

                string path = AssetDatabase.GetAssetPath(meshRenderer.sharedMesh);
                if (path != null)
                {
                    meshPathHash.Add(path);
                }
            }

            MeshFilter[] allMeshFilter = mainSelect.GetComponentsInChildren<MeshFilter>();
            for (int i = 0; i < allMeshFilter.Length; i++)
            {
                MeshFilter meshFliter = allMeshFilter[i];

                string path = AssetDatabase.GetAssetPath(meshFliter.sharedMesh);
                if (path != null)
                {
                    meshPathHash.Add(path);
                }
            }
            IEnumerable<ModelImporter> modelImporters = meshPathHash.Select(x => AssetImporter.GetAtPath(x) as ModelImporter);
            modelImporterList.AddRange(modelImporters);
        }

        void GetFromScene()
        {

            textureImporterList.Clear();
            modelImporterList.Clear();
            HashSet<string> textureImporterHash = new HashSet<string>();
            if (mainScene == null)
            {
                return;
            }
            Scene scene = EditorSceneManager.GetSceneByPath(AssetDatabase.GetAssetPath(mainScene));
            GameObject[] roots = scene.GetRootGameObjects();
            Renderer[] allRenderer = roots.SelectMany(x => x.GetComponentsInChildren<Renderer>()).ToArray();
            for (int i = 0; i < allRenderer.Length; i++)
            {
                Renderer renderer = allRenderer[i];
                Material[] materials = renderer.sharedMaterials;
                for (int ii = 0; ii < materials.Length; ii++)
                {
                    Material material = materials[ii];
                    Shader shader = material.shader;
                    for (int iii = 0; iii < ShaderUtil.GetPropertyCount(shader); iii++)
                    {
                        if (ShaderUtil.GetPropertyType(shader, iii) == ShaderUtil.ShaderPropertyType.TexEnv)
                        {
                            Texture texture = renderer.sharedMaterial.GetTexture(ShaderUtil.GetPropertyName(shader, iii));
                            string path = AssetDatabase.GetAssetPath(texture);
                            if (!string.IsNullOrEmpty(path) && !textureImporterHash.Contains(path))
                            {
                                textureImporterHash.Add(path);
                            }
                        }
                    }
                }
            }
            IEnumerable<TextureImporter> textureImporters = textureImporterHash.Select(x => AssetImporter.GetAtPath(x) as TextureImporter);
            textureImporterList.AddRange(textureImporters);

            HashSet<string> meshPathHash = new HashSet<string>();


            SkinnedMeshRenderer[] allMeshRenderer = roots.SelectMany(x => x.GetComponentsInChildren<SkinnedMeshRenderer>()).ToArray();
            for (int i = 0; i < allMeshRenderer.Length; i++)
            {
                SkinnedMeshRenderer meshRenderer = allMeshRenderer[i];

                string path = AssetDatabase.GetAssetPath(meshRenderer.sharedMesh);
                if (!string.IsNullOrEmpty(path) && path != null)
                {
                    meshPathHash.Add(path);
                }
            }

            MeshFilter[] allMeshFilter = roots.SelectMany(x => x.GetComponentsInChildren<MeshFilter>()).ToArray();
            for (int i = 0; i < allMeshFilter.Length; i++)
            {
                MeshFilter meshFliter = allMeshFilter[i];

                string path = AssetDatabase.GetAssetPath(meshFliter.sharedMesh);
                if (!string.IsNullOrEmpty(path) && path != null)
                {
                    meshPathHash.Add(path);
                }
            }
            IEnumerable<ModelImporter> modelImporters = meshPathHash.Select(x => AssetImporter.GetAtPath(x) as ModelImporter).Where(x => x != null);
            modelImporterList.AddRange(modelImporters);
        }

        void GetFromProject()
        {

            textureImporterList.Clear();
            modelImporterList.Clear();
            string[] allTextruePath = AssetDatabase.FindAssets("t:texture", new string[] { "Assets" });
            for (int i = 0; i < allTextruePath.Length; i++)
            {
                TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(allTextruePath[i])) as TextureImporter;
                if (textureImporter != null)
                {
                    textureImporterList.Add(textureImporter);
                }
            }


            string[] allModelPath = AssetDatabase.FindAssets("t:model", new string[] { "Assets" });
            for (int i = 0; i < allModelPath.Length; i++)
            {
                ModelImporter modelImporter = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(allModelPath[i])) as ModelImporter;
                if (modelImporter != null)
                {
                    modelImporterList.Add(modelImporter);
                }
            }
        }


    }
}