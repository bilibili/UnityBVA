using UnityEditor;
using UnityEngine;
using BVA;
using System.Linq;
using UnityEngine.Rendering;

public class MiscEditorTools
{
    public static void RecursiveDeleteChildWithMissingScript(GameObject gameObject)
    {
        int number = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
        if (gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                RecursiveDeleteChildWithMissingScript(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }

    [MenuItem("GameObject/Remove Missing Component")]
    public static void DestoryMissScript()
    {
        if (Selection.activeGameObject == null)
            return;
        var objs = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = objs.Sum(GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
        Debug.Log($"Removed {count} missing scripts");
        RecursiveDeleteChildWithMissingScript(Selection.activeGameObject);
        AssetDatabase.Refresh();
    }

    public static void RecursiveResetTRS(Transform transform)
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        if (transform.transform.childCount > 0)
        {
            for (int i = 0; i < transform.transform.childCount; i++)
            {
                RecursiveResetTRS(transform.transform.GetChild(i));
            }
        }
    }

    [MenuItem("GameObject/Reset Transform")]
    public static void ResetTRS()
    {
        if (Selection.activeGameObject == null)
            return;
        RecursiveResetTRS(Selection.activeGameObject.transform);
        Debug.Log("reset all transforms!");
    }

    [MenuItem("BVA/Developer Tools/Enforce Avatar T-Pose")]
    public static void EnforceTPose()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("Please select an Avatar and make sure it has Animator component");
            return;
        }
        Animator animator = Selection.activeGameObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator founded on the gameObject");
            return;
        }
        Humanoid.EnforceTPose2(animator);
        Debug.Log("set T-Pose success!");
    }

    [MenuItem("BVA/Manual", priority = 99)]
    public static void OpenManual()
    {
        Application.OpenURL("https://github.com/bilibili/UnityBVA");
    }

    [MenuItem("BVA/Developer Tools/Set Material GlobalIllumination-Baked(Static GameObject Only)", priority = 100)]
    public static void SetMaterialGlobalIllumination()
    {
        GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in gameObjects)
        {
            var renders = obj.GetComponentsInChildren<Renderer>();
            foreach (var render in renders)
            {
                if (!render.gameObject.isStatic) continue;
                foreach (var material in render.sharedMaterials)
                {
                    var _material = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GetAssetPath(material));
                    _material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
                }
            }
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("BVA/Developer Tools/Disable Material Environment Reflection", priority = 100)]
    public static void DisableMaterialEnvironmentReflection()
    {
        GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in gameObjects)
        {
            var renders = obj.GetComponentsInChildren<Renderer>();
            foreach (var render in renders)
            {
                foreach (var material in render.sharedMaterials)
                {
                    var _material = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GetAssetPath(material));
                    if (_material.HasFloat("_EnvironmentReflections"))
                    {
                        _material.SetFloat("_EnvironmentReflections", 0.0f);
                        CoreUtils.SetKeyword(_material, "_ENVIRONMENTREFLECTIONS_OFF", true);
                    }
                }
            }
        }
        AssetDatabase.Refresh();
    }
}
