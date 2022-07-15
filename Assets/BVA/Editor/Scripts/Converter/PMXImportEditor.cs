using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace LibMMD.Unity3D
{
    public class PMXImportEditor
    {
        public static async Task LoadPMX(MaterialLoader.MaterialType materialType)
        {
            string[] path = SFB.StandaloneFileBrowser.OpenFilePanel("PMX", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("MMD Files", "pmx") }, false);
            if (path.Length == 0) return;
            string humanPath = path[0];
            MaterialLoader.UseMaterialType = materialType;
            Transform alicia = await PMXModelLoader.LoadPMXModel(humanPath, null);
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
        [MenuItem("BVA/Runtime Load/Load PMX (Zelda)")]
        public static async void LoadPMXMZelda()
        {
            await LoadPMX(MaterialLoader.MaterialType.Zelda);
        }
    }
}