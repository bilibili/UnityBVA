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
        public void ImportLight(KHR_lights_punctualExtension ext, Node node, GameObject nodeObj)
        {
            var lightObj = nodeObj.AddComponent<Light>();
            lightObj.color = ext.color;
            lightObj.intensity = (float)ext.intensity;
            lightObj.type = (UnityEngine.LightType)ext.type;
            lightObj.range = (float)ext.range;
            lightObj.innerSpotAngle = (float)ext.innerConeAngle * Mathf.Rad2Deg * 2;
            lightObj.spotAngle = (float)ext.outerConeAngle * Mathf.Rad2Deg * 2;

            if (lightObj.type != UnityEngine.LightType.Directional)
            {
                lightObj.bounceIntensity = 0;
            }

            if (node.Extras != null && node.Extras.Count > 0)
            {
                foreach (var extra in node.Extras)
                {
                    var (propertyName, reader) = GetExtraProperty(extra);
                    if (propertyName == BVA_Light_URP_Extra.PROPERTY)
                    {
                        BVA_Light_URP_Extra.Deserialize(_gltfRoot, reader, lightObj);
                    }
                }
            }
        }
        private bool hasValidExtension(GLTFScene scene, string extenionName)
        {
            return _gltfRoot.ExtensionsUsed != null
             && _gltfRoot.ExtensionsUsed.Contains(extenionName)
             && scene.Extensions != null
             && scene.Extensions.ContainsKey(extenionName);
        }
        public async Task LoadSceneLightmap(GLTFScene scene)
        {
            if (!hasValidExtension(scene, BVA_light_lightmapExtensionFactory.EXTENSION_NAME))
                return;

            IExtension ext = scene.Extensions[BVA_light_lightmapExtensionFactory.EXTENSION_NAME];
            var impl = (BVA_light_lightmapExtensionFactory)ext;
            if (impl == null) throw new InvalidCastException($"cast {nameof(BVA_light_lightmapExtensionFactory)} failed");
            await ImportLightmap(_gltfRoot.Extensions.Lightmaps[impl.id.Id]);
        }

        public async Task ImportLightmap(BVA_light_lightmapExtension ext)
        {
            LightmapSettings.lightmapsMode = ext.lightmapsMode;
            var newLightmaps = new LightmapData[ext.lightmaps.Length];

            for (int i = 0; i < newLightmaps.Length; i++)
            {
                newLightmaps[i] = new LightmapData();
                newLightmaps[i].lightmapColor = (Texture2D)await GetTextureAsLinear(ext.lightmaps[i].lightmapColor);

                if (ext.lightmapsMode != LightmapsMode.NonDirectional)
                {
                    newLightmaps[i].lightmapDir = (Texture2D)await GetLightmap(ext.lightmaps[i].lightmapDir);
                    if (newLightmaps[i].shadowMask != null)
                    {
                        // If the textuer existed and was set in the data file.
                        newLightmaps[i].shadowMask = (Texture2D)await GetLightmap(ext.lightmaps[i].shadowMask);
                    }
                }
            }
            LightmapSettings.lightmaps = newLightmaps;
        }
    }

    public partial class GLTFSceneExporter
    {
        private bool ShouldExportLight(Light unityLight)
        {
            return unityLight != null &&
#if UNITY_EDITOR
            (unityLight.lightmapBakeType == LightmapBakeType.Realtime || unityLight.lightmapBakeType == LightmapBakeType.Mixed) &&
#endif
            (unityLight.type == UnityEngine.LightType.Directional || unityLight.type == UnityEngine.LightType.Point || unityLight.type == UnityEngine.LightType.Spot);
        }
        private LightId ExportLight(Light unityLight)
        {
            KHR_lights_punctualExtension ext = new KHR_lights_punctualExtension((GLTF.Schema.LightType)unityLight.type, unityLight.name, unityLight.color, unityLight.intensity, unityLight.range, Mathf.Deg2Rad * unityLight.innerSpotAngle / 2, Mathf.Deg2Rad * unityLight.spotAngle / 2);

            var id = new LightId
            {
                Id = _root.Extensions.Lights.Count,
                Root = _root
            };
            _root.Extensions.AddLight(ext);
            return id;
        }

        private bool _exportLightmap;
        private bool ShouldExportLightmap()
        {
#if UNITY_EDITOR
            if (ExportLightmap && LightmapSettings.lightmaps.Length > 0 && LightmapSettings.lightmaps[0].lightmapColor != null)
                _exportLightmap = true;
            else
                _exportLightmap = false;
            Debug.Log(_exportLightmap ? "Export Lightmap" : "not export lightmap");
            return _exportLightmap;
#else
                _exportLightmap = false;
                return _exportLightmap;
#endif
        }

        public LightmapId ExportSceneLightmap()
        {
            List<LightmapTextureInfo> lightmapTextureInfos = new List<LightmapTextureInfo>();
            foreach (var lightmap in LightmapSettings.lightmaps)
            {
                TextureInfo colorInfo = lightmap.lightmapColor == null ? null : ExportTextureInfo(lightmap.lightmapColor, TextureMapType.LightMapColor);//Color is already encoded as RGBM, export as Main, but decoded is needed when importing
                TextureInfo dirInfo = lightmap.lightmapDir == null ? null : ExportTextureInfo(lightmap.lightmapDir, TextureMapType.LightMapDir);
                TextureInfo shadowInfo = lightmap.shadowMask == null ? null : ExportTextureInfo(lightmap.shadowMask, TextureMapType.LightMapDir);
                lightmapTextureInfos.Add(new LightmapTextureInfo() { lightmapColor = colorInfo?.Index, lightmapDir = dirInfo?.Index, shadowMask = shadowInfo?.Index });
            }

            Debug.Log(LightmapSettings.lightProbes.bakedProbes.Length);
            BVA_light_lightmapExtension ext = new BVA_light_lightmapExtension(LightmapSettings.lightmapsMode, lightmapTextureInfos.ToArray(), LightmapsEncoding.RGBM);

            var id = new LightmapId
            {
                Id = _root.Extensions.Lightmaps.Count,
                Root = _root
            };
            _root.Extensions.AddLightmap(ext);
            return id;
        }
    }
}