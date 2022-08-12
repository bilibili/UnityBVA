using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace LibMMD.Unity3D
{
    public class PMXImportEditor
    {
        public static async Task LoadPMX(MaterialLoader.MaterialType materialType)
        {
            string path = EditorUtility.OpenFilePanelWithFilters("PMX", "", new string[] { "MMD Files", "pmx" });
            if (string.IsNullOrEmpty(path)) return;
            MaterialLoader.UseMaterialType = materialType;
            Transform alicia = await PMXModelLoader.LoadPMXModel(path, null);
        }

        [MenuItem("BVA/Runtime Load/Load PMX (MToon)")]
        public static async void LoadPMXMToon()
        {
            await LoadPMX(MaterialLoader.MaterialType.MToon);
        }
        [MenuItem("BVA/Runtime Load/Load PMX (Unlit)")]
        public static async void LoadPMXMDefault()
        {
            await LoadPMX(MaterialLoader.MaterialType.Default);
        }
    }
}