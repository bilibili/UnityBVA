using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using GLTF.Schema.BVA;
using GLTF.Schema;
using System.Linq;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async Task LoadSkybox(GameObject sceneObj)
        {
            if (_gltfRoot.Extensions.Skyboxs.Count == 0) return;
            _assetManager.AddContainer(sceneObj.AddComponent<SkyboxContainer>());
            foreach (var skybox in _gltfRoot.Extensions.Skyboxs)
            {
                var materialIndex = skybox.material.Id;
                if (_assetCache.MaterialCache[materialIndex] == null)
                {
                    var def = _gltfRoot.Materials[materialIndex];
                    await ConstructMaterialImageBuffers(def);
                    await ConstructMaterial(def, materialIndex);
                }
                AssetManager.skyboxContainer.Add(_assetCache.MaterialCache[materialIndex].UnityMaterial);
            }
            RenderSettings.skybox = AssetManager.skyboxContainer.Get(0);
            DynamicGI.UpdateEnvironment();
        }
    }

    public partial class GLTFSceneExporter
    {
        private bool IsValidSkyboxMaterial(Material material)
        {
            return SkyboxContainer.IsValidMaterial(material);
        }
        private bool ShouldExportSceneSkybox()
        {
            return ExportSkybox && RenderSettings.skybox != null && IsValidSkyboxMaterial(RenderSettings.skybox);
        }
        /// <summary>
        /// Export audioClip in AudioClipContainer
        /// </summary>
        /// <param name="container"></param>
        private void ExportContainer(SkyboxContainer container)
        {
            if (!container.isActiveAndEnabled) return;
            container.RemoveInvalidOrNull();
            if (container.materials != null && container.materials.Count > 0)
            {
                foreach (var material in container.materials)
                {
                    ExportSkyboxMaterial(material);
                }
            }
        }
        private void ExportSkyboxMaterial(Material material)
        {
            var materialId = ExportMaterial(material);
            _root.AddExtension(_root, BVA_skybox_sixSidedExtensionFactory.EXTENSION_NAME, null, RequireExtensions);
            _root.Extensions.AddSkybox(new BVA_skybox_sixSidedExtension(materialId));
        }
    }
}