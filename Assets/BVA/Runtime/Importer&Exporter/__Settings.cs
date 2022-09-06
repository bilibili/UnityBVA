using GLTF.Schema;
using UnityEngine;
using BVA.Extensions;
using System.Collections.Generic;
using GLTF.Schema.BVA;
using System.Threading.Tasks;
using System;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async Task LoadSceneRenderSetting(GLTFScene scene)
        {
            if (!hasValidExtension(scene, BVA_setting_renderSettingExtensionFactory.EXTENSION_NAME))
                return;

            IExtension ext = scene.Extensions[BVA_setting_renderSettingExtensionFactory.EXTENSION_NAME];
            var impl = (BVA_setting_renderSettingExtensionFactory)ext;
            if (impl == null) throw new InvalidCastException($"cast {nameof(BVA_setting_renderSettingExtensionFactory)} failed");
            await ImportLightmap(_gltfRoot.Extensions.RenderSettings[impl.id.Id]);
        }
        public async Task ImportLightmap(BVA_setting_renderSettingExtension ext)
        {
            ext = ext.Deserialize(_gltfRoot);
            if (ext.skybox != null) RenderSettings.skybox = await LoadMaterial(ext.skybox);
            if (ext.sun != null && ext.sun.IsValid) RenderSettings.sun = _assetCache.NodeCache[ext.sun.Id].GetComponent<Light>();
            if (ext.customReflection != null) RenderSettings.customReflection = await LoadCubemap(ext.customReflection);
        }
    }

    public partial class GLTFSceneExporter
    {
        private bool ShouldExportRenderSettings()
        {
            return ExportRenderSetting;
        }
        public RenderSettingId ExportSceneRenderSetting()
        {
            BVA_setting_renderSettingExtension ext = new BVA_setting_renderSettingExtension();

            if (RenderSettings.skybox != null) ext.skybox = ExportMaterial(RenderSettings.skybox);
            if (RenderSettings.sun != null && RenderSettings.sun.gameObject.activeSelf) ext.sun = new NodeId() { Id = _nodeCache.GetId(RenderSettings.sun.gameObject), Root = _root };
            if (RenderSettings.customReflection != null && RenderSettings.customReflection is Cubemap)
                ext.customReflection = ExportCubemap(RenderSettings.customReflection as Cubemap);

            var id = new RenderSettingId
            {
                Id = _root.Extensions.RenderSettings.Count,
                Root = _root
            };
            _root.Extensions.AddRenderSetting(ext);
            return id;
        }
    }
}