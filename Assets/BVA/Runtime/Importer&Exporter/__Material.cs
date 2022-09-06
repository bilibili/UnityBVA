using GLTF.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using BVA.Cache;
using BVA.Extensions;
using GLTF.Schema.BVA;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        /// <summary>
        /// Load a Material from the glTF by index
        /// </summary>
        /// <param name="materialIndex"></param>
        /// <returns>Material at index</returns>
        public async Task<Material> LoadMaterialAsync(int materialIndex)
        {
            await ConstructMaterialAsync(materialIndex);
            return _assetCache.MaterialCache[materialIndex].UnityMaterialWithVertexColor;
        }
        public async Task ConstructMaterialAsync(int materialIndex)
        {
            if (materialIndex < 0 || materialIndex >= _gltfRoot.Materials.Count)
            {
                throw new System.ArgumentException($"There is no material for index {materialIndex}");
            }
            if (_assetCache.MaterialCache[materialIndex] == null)
            {
                var def = _gltfRoot.Materials[materialIndex];
                await ConstructMaterialImageBuffers(def);
                await ConstructMaterial(def, materialIndex);
            }
        }
        public async Task<Material> LoadMaterial(MaterialId materialId)
        {
            await ConstructMaterialAsync(materialId.Id);
            return _assetCache.MaterialCache[materialId.Id].UnityMaterialWithVertexColor;
        }
        protected Task ConstructMaterialImageBuffers(GLTFMaterial def)
        {
            var tasks = new List<Task>(8);
            if (def.PbrMetallicRoughness != null)
            {
                var pbr = def.PbrMetallicRoughness;

                if (pbr.BaseColorTexture != null)
                {
                    var textureId = pbr.BaseColorTexture.Index;
                    tasks.Add(ConstructImageBuffer(textureId.Value, textureId.Id));
                }
                if (pbr.MetallicRoughnessTexture != null)
                {
                    var textureId = pbr.MetallicRoughnessTexture.Index;

                    tasks.Add(ConstructImageBuffer(textureId.Value, textureId.Id));
                }
            }

            if (def.CommonConstant != null)
            {
                if (def.CommonConstant.LightmapTexture != null)
                {
                    var textureId = def.CommonConstant.LightmapTexture.Index;

                    tasks.Add(ConstructImageBuffer(textureId.Value, textureId.Id));
                }
            }

            if (def.NormalTexture != null)
            {
                var textureId = def.NormalTexture.Index;
                tasks.Add(ConstructImageBuffer(textureId.Value, textureId.Id));
            }

            if (def.OcclusionTexture != null)
            {
                var textureId = def.OcclusionTexture.Index;

                if (!(def.PbrMetallicRoughness != null
                        && def.PbrMetallicRoughness.MetallicRoughnessTexture != null
                        && def.PbrMetallicRoughness.MetallicRoughnessTexture.Index.Id == textureId.Id))
                {
                    tasks.Add(ConstructImageBuffer(textureId.Value, textureId.Id));
                }
            }

            if (def.EmissiveTexture != null)
            {
                var textureId = def.EmissiveTexture.Index;
                tasks.Add(ConstructImageBuffer(textureId.Value, textureId.Id));
            }

            // pbr_spec_gloss extension
            const string specGlossExtName = KHR_materials_pbrSpecularGlossinessExtensionFactory.EXTENSION_NAME;
            if (def.Extensions != null && def.Extensions.ContainsKey(specGlossExtName))
            {
                var specGlossDef = (KHR_materials_pbrSpecularGlossinessExtension)def.Extensions[specGlossExtName];
                if (specGlossDef.DiffuseTexture != null)
                {
                    var textureId = specGlossDef.DiffuseTexture.Index;
                    tasks.Add(ConstructImageBuffer(textureId.Value, textureId.Id));
                }

                if (specGlossDef.SpecularGlossinessTexture != null)
                {
                    var textureId = specGlossDef.SpecularGlossinessTexture.Index;
                    tasks.Add(ConstructImageBuffer(textureId.Value, textureId.Id));
                }
            }

            return Task.WhenAll(tasks);
        }
        protected async Task ConstructMaterial(GLTFMaterial def, int materialIndex)
        {
            if (def.Extras == null)
            {
                await ConstructPBRMaterial(def, materialIndex);
            }
            else
            {
                await ConstructCustomMaterial(def, materialIndex);
            }
        }

        protected async Task ConstructCustomMaterial(GLTFMaterial def, int materialIndex, bool preloadedTextures = false)
        {
            async Task<Texture> loadTexture(TextureId textureId)
            {
                if (!preloadedTextures)
                {
                    await ConstructImageBuffer(textureId.Value, textureId.Id);
                    await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
                }
                return _assetCache.TextureCache[textureId.Id].Texture;
            }
            if (def.Extras.Count == 0)
            {
                LogPool.ImportLogger.LogError(LogPart.Material, "Import Custom Material,but not find extra property!");
                return;
            }

            var (shaderName, reader) = GetExtraProperty(def.Extras[0]);

            // Currently, we don't preprocess NormalMap as it won't affect the actual render.
            Material matCache = await MaterialImporter.ImportMaterial(shaderName, def, _gltfRoot, reader, loadTexture, loadTexture, LoadCubemap);
            if (matCache == null) return;
            MaterialCacheData materialWrapper = new MaterialCacheData
            {
                UnityMaterial = matCache,
                UnityMaterialWithVertexColor = matCache,
                GLTFMaterial = def
            };

            if (materialIndex >= 0)
            {
                _assetCache.MaterialCache[materialIndex] = materialWrapper;
            }
            else
            {
                _defaultLoadedMaterial = materialWrapper;
            }
        }
        protected async Task ConstructPBRMaterial(GLTFMaterial def, int materialIndex)
        {
            IUniformMap mapper;
            const string specGlossExtName = KHR_materials_pbrSpecularGlossinessExtensionFactory.EXTENSION_NAME;
            //const string unlitExtName = KHR_materials_unlitExtensionFactory.EXTENSION_NAME;
            bool isClearcoat = _gltfRoot.ExtensionsUsed != null && _gltfRoot.ExtensionsUsed.Contains(KHR_materials_clearcoatExtensionFactory.EXTENSION_NAME)
                 && def.Extensions != null && def.Extensions.ContainsKey(KHR_materials_clearcoatExtensionFactory.EXTENSION_NAME);
            bool isUnlit = (def.Extras == null || def.Extras.Count == 0) && _gltfRoot.ExtensionsUsed != null && _gltfRoot.ExtensionsUsed.Contains(KHR_materials_unlitExtensionFactory.EXTENSION_NAME)
                     && def.Extensions != null && def.Extensions.ContainsKey(KHR_materials_unlitExtensionFactory.EXTENSION_NAME);
            if (_gltfRoot.ExtensionsUsed != null && _gltfRoot.ExtensionsUsed.Contains(specGlossExtName)
            && def.Extensions != null && def.Extensions.ContainsKey(specGlossExtName))
            {
                mapper = new UrpSpecGlossMap(isClearcoat, MaximumLod);
            }
            else
            {
                mapper = new UrpMetalRoughMap(isClearcoat, MaximumLod);
            }

            mapper.Material.name = def.Name;
            mapper.AlphaMode = def.AlphaMode;
            mapper.BlendMode = def.BlendMode;
            mapper.DoubleSided = def.DoubleSided;

            if (def.PbrMetallicRoughness != null && mapper is IMetalRoughUniformMap mrMapper)
            {
                mapper.Material.SetFloat("_WorkflowMode", 1.0f);
                var pbr = def.PbrMetallicRoughness;

                mrMapper.BaseColorFactor = pbr.BaseColorFactor;

                if (pbr.BaseColorTexture != null)
                {
                    TextureId textureId = pbr.BaseColorTexture.Index;
                    await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
                    mrMapper.BaseColorTexture = _assetCache.TextureCache[textureId.Id].Texture;
                    mrMapper.BaseColorTexCoord = pbr.BaseColorTexture.TexCoord;

                    var ext = GetTextureTransform(pbr.BaseColorTexture);
                    if (ext != null)
                    {
                        mrMapper.BaseColorXOffset = ext.Offset;
                        mrMapper.BaseColorXRotation = ext.Rotation;
                        mrMapper.BaseColorXScale = ext.Scale;
                        mrMapper.BaseColorXTexCoord = ext.TexCoord;
                    }
                }

                mrMapper.MetallicFactor = pbr.MetallicFactor;
                mrMapper.RoughnessFactor = pbr.RoughnessFactor;

                if (pbr.MetallicRoughnessTexture != null)
                {
                    TextureId textureId = pbr.MetallicRoughnessTexture.Index;
                    await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture, true);
                    mrMapper.MetallicRoughnessTexture = _assetCache.TextureCache[textureId.Id].Texture;
                    mrMapper.MetallicRoughnessTexCoord = pbr.MetallicRoughnessTexture.TexCoord;

                    var ext = GetTextureTransform(pbr.MetallicRoughnessTexture);
                    if (ext != null)
                    {
                        mrMapper.MetallicRoughnessXOffset = ext.Offset;
                        mrMapper.MetallicRoughnessXRotation = ext.Rotation;
                        mrMapper.MetallicRoughnessXScale = ext.Scale;
                        mrMapper.MetallicRoughnessXTexCoord = ext.TexCoord;
                    }
                }

            }

            if (mapper is ISpecGlossUniformMap sgMapper)
            {
                mapper.Material.SetFloat("_WorkflowMode", 0.0f);
                var specGloss = def.Extensions[specGlossExtName] as KHR_materials_pbrSpecularGlossinessExtension;
                sgMapper.DiffuseFactor = specGloss.DiffuseFactor;

                if (specGloss.DiffuseTexture != null)
                {
                    TextureId textureId = specGloss.DiffuseTexture.Index;
                    await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
                    sgMapper.DiffuseTexture = _assetCache.TextureCache[textureId.Id].Texture;
                    sgMapper.DiffuseTexCoord = specGloss.DiffuseTexture.TexCoord;

                    var ext = GetTextureTransform(specGloss.DiffuseTexture);
                    if (ext != null)
                    {
                        sgMapper.DiffuseXOffset = ext.Offset;
                        sgMapper.DiffuseXRotation = ext.Rotation;
                        sgMapper.DiffuseXScale = ext.Scale;
                        sgMapper.DiffuseXTexCoord = ext.TexCoord;
                    }
                }

                sgMapper.SpecularFactor = specGloss.SpecularFactor;
                sgMapper.GlossinessFactor = specGloss.GlossinessFactor;

                if (specGloss.SpecularGlossinessTexture != null)
                {
                    TextureId textureId = specGloss.SpecularGlossinessTexture.Index;
                    await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
                    sgMapper.SpecularGlossinessTexture = _assetCache.TextureCache[textureId.Id].Texture;

                    var ext = GetTextureTransform(specGloss.SpecularGlossinessTexture);
                    if (ext != null)
                    {
                        sgMapper.SpecularGlossinessXOffset = ext.Offset;
                        sgMapper.SpecularGlossinessXRotation = ext.Rotation;
                        sgMapper.SpecularGlossinessXScale = ext.Scale;
                        sgMapper.SpecularGlossinessXTexCoord = ext.TexCoord;
                    }
                }
            }

            if (def.NormalTexture != null)
            {
                TextureId textureId = def.NormalTexture.Index;
                await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture, true);
                mapper.NormalTexture = _assetCache.TextureCache[textureId.Id].Texture;
                mapper.NormalTexCoord = def.NormalTexture.TexCoord;
                mapper.NormalTexScale = def.NormalTexture.Scale;

                var ext = GetTextureTransform(def.NormalTexture);
                if (ext != null)
                {
                    mapper.NormalXOffset = ext.Offset;
                    mapper.NormalXRotation = ext.Rotation;
                    mapper.NormalXScale = ext.Scale;
                    mapper.NormalXTexCoord = ext.TexCoord;
                }
            }

            if (def.OcclusionTexture != null)
            {
                mapper.OcclusionTexStrength = def.OcclusionTexture.Strength;
                TextureId textureId = def.OcclusionTexture.Index;
                await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture, true);
                mapper.OcclusionTexture = _assetCache.TextureCache[textureId.Id].Texture;

                var ext = GetTextureTransform(def.OcclusionTexture);
                if (ext != null)
                {
                    mapper.OcclusionXOffset = ext.Offset;
                    mapper.OcclusionXRotation = ext.Rotation;
                    mapper.OcclusionXScale = ext.Scale;
                    mapper.OcclusionXTexCoord = ext.TexCoord;
                }
            }
            else
            {
                def.OcclusionTexture = null;
            }

            if (def.EmissiveTexture != null)
            {
                TextureId textureId = def.EmissiveTexture.Index;
                await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
                mapper.EmissiveTexture = _assetCache.TextureCache[textureId.Id].Texture;
                mapper.EmissiveTexCoord = def.EmissiveTexture.TexCoord;

                var ext = GetTextureTransform(def.EmissiveTexture);
                if (ext != null)
                {
                    mapper.EmissiveXOffset = ext.Offset;
                    mapper.EmissiveXRotation = ext.Rotation;
                    mapper.EmissiveXScale = ext.Scale;
                    mapper.EmissiveXTexCoord = ext.TexCoord;
                }
            }
            else
            {
                mapper.EmissiveTexture = null;
            }

            mapper.EmissiveFactor = def.EmissiveFactor;

            // Environment Reflection
            if (!_options.EnableEnvironmentReflection)
            {
                mapper.Material.SetInt("_EnvironmentReflections", 0);
                CoreUtils.SetKeyword(mapper.Material, "_ENVIRONMENTREFLECTIONS_OFF", true);
            }
            // Specular Highlight
            if (!_options.EnableSpecularHighlight)
            {
                mapper.Material.SetInt("_SpecularHighlights", 0);
                CoreUtils.SetKeyword(mapper.Material, "_SPECULARHIGHLIGHTS_OFF", true);
            }

            if (_gltfRoot.ExtensionsUsed != null && _gltfRoot.ExtensionsUsed.Contains(KHR_materials_emissive_strengthExtensionFactory.EXTENSION_NAME) && def.Extensions != null && def.Extensions.ContainsKey(KHR_materials_emissive_strengthExtensionFactory.EXTENSION_NAME))
            {
                var emissiveStrength = def.Extensions[KHR_materials_emissive_strengthExtensionFactory.EXTENSION_NAME] as KHR_materials_emissive_strengthExtension;
                mapper.EmissiveFactor *= emissiveStrength.emissiveStrength;
            }

            // import clearcoat
            if (isClearcoat)
            {
                var clearcoat = def.Extensions[KHR_materials_clearcoatExtensionFactory.EXTENSION_NAME] as KHR_materials_clearcoatExtension;

                mapper.ClearcoatFactor = clearcoat.clearcoatFactor;
                if (clearcoat.clearcoatTexture != null)
                {
                    TextureId textureId = clearcoat.clearcoatTexture.Index;
                    await ConstructImageBuffer(textureId.Value, textureId.Id);
                    await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
                    mapper.ClearcoatTexture = _assetCache.TextureCache[textureId.Id].Texture;
                }

                mapper.ClearcoatRoughnessFactor = clearcoat.clearcoatRoughnessFactor;
                if (clearcoat.clearcoatRoughnessTexture != null)
                {
                    TextureId textureId = clearcoat.clearcoatRoughnessTexture.Index;
                    await ConstructImageBuffer(textureId.Value, textureId.Id);
                    await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
                    mapper.ClearcoatRoughnessTexture = _assetCache.TextureCache[textureId.Id].Texture;
                }
            }

            if (isUnlit)
            {
                var unlitShader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Texture");
                mapper.Material.shader = unlitShader;
                mapper.AlphaCutoff = def.AlphaCutoff;
                mapper.AlphaMode = def.AlphaMode;
            }

            var vertColorMapper = mapper.Clone();
            vertColorMapper.VertexColorsEnabled = true;

            MaterialCacheData materialWrapper = new MaterialCacheData
            {
                UnityMaterial = mapper.Material,
                UnityMaterialWithVertexColor = vertColorMapper.Material,
                GLTFMaterial = def
            };
            if (materialIndex >= 0)
            {
                _assetCache.MaterialCache[materialIndex] = materialWrapper;
            }
            else
            {
                _defaultLoadedMaterial = materialWrapper;
            }
        }
    }
    public partial class GLTFSceneExporter
    {
        private bool IsPBRMetallicRoughness(Material material)
        {
            return material.HasProperty("_Metallic") && material.HasProperty("_MetallicGlossMap");
        }

        public bool IsClearcoat(Material material)
        {
            return (material.IsKeywordEnabled("_CLEARCOAT") || material.IsKeywordEnabled("_CLEARCOATMAP")) && (material.HasProperty("_ClearCoatMask") || material.HasProperty("_ClearCoatMap"));
        }
        private bool IsPBRSpecularGlossiness(Material material)
        {
            return /*material.HasProperty("_SpecColor") && material.HasProperty("_SpecGlossMap") &&*/ material.HasProperty("_WorkflowMode") && Mathf.Approximately(material.GetFloat("_WorkflowMode"), 0.0f);
        }
        /// <summary>
        /// shader name that contains Unlit will regard as unlit material
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        private bool IsUnlit(Material material)
        {
            return material.shader.name.Contains("Unlit");
        }
        private bool IsCommonConstant(Material material)
        {
            return material.HasProperty("_LightMap") && material.HasProperty("_LightFactor");
        }
        private MaterialId ExportMaterial(Material materialObj)
        {
            if (materialObj == null) return null;

            MaterialId id = GetMaterialId(_root, materialObj);
            if (id != null) return id;

            bool useCustomShader = false;
            var material = new GLTFMaterial();
            if (ExportNames)
            {
                material.Name = materialObj.name;
            }

            // Export Custom Shader Material
            useCustomShader = MaterialImporter.ExportMaterialExtra(materialObj, material, ExportTextureInfo, ExportNormalTextureInfo, ExportCubemap);

            //Export PBR Material
            if (ExportNames)
            {
                material.Name = materialObj.name;
            }

            if (materialObj.HasProperty("_Cutoff"))
            {
                material.AlphaCutoff = materialObj.GetFloat("_Cutoff");
            }

            switch (materialObj.GetTag("RenderType", false, ""))
            {
                case "TransparentCutout":
                    material.AlphaMode = AlphaMode.MASK;
                    break;
                case "Transparent":
                    material.AlphaMode = AlphaMode.BLEND;
                    break;
                default:
                    material.AlphaMode = AlphaMode.OPAQUE;
                    break;
            }

            if (materialObj.HasProperty("_Blend"))
            {
                var blend = (int)materialObj.GetFloat("_Blend");
                material.BlendMode = (GLTF.Schema.BlendMode)blend;
            }

            material.DoubleSided = materialObj.HasProperty("_Cull") && materialObj.GetInt("_Cull") == (float)CullMode.Off;

            if (materialObj.IsKeywordEnabled("_EMISSION"))
            {
                if (materialObj.HasProperty("_EmissionColor"))
                {
                    var emissiveColor = materialObj.GetColor("_EmissionColor");

                    // emissive factor should never greater than 1.0, if so, add KHR_materials_emissive_strength extension.
                    float maxFactor = emissiveColor.maxColorComponent;
                    if (maxFactor > 1.0f)
                    {
                        material.EmissiveFactor = (emissiveColor / maxFactor);
                        material.AddExtension(_root, KHR_materials_emissive_strengthExtensionFactory.EXTENSION_NAME, new KHR_materials_emissive_strengthExtension(maxFactor), RequireExtensions);
                    }
                    else
                    {
                        material.EmissiveFactor = emissiveColor;
                    }
                }

                if (GetOneMaterialProperty(materialObj, MATERIAL_PROPERTY_EmissionMap, out string emissionProperty))
                {
                    var emissionTex = materialObj.GetTexture(emissionProperty);

                    if (emissionTex != null)
                    {
                        if (emissionTex is Texture2D)
                        {
                            material.EmissiveTexture = ExportTextureInfo(emissionTex, TextureMapType.Emission);

                            ExportTextureTransform(material.EmissiveTexture, materialObj, emissionProperty);
                        }
                        else
                        {
                            LogPool.ExportLogger.LogError(LogPart.Material, $"Can't export a {emissionTex.GetType()} {emissionProperty} in material {materialObj.name}");
                        }

                    }
                }
            }

            if (GetOneMaterialProperty(materialObj, MATERIAL_PROPERTY_NormalMap, out string normalMapProperty))
            {
                var normalTex = materialObj.GetTexture(normalMapProperty);

                if (normalTex != null)
                {
                    if (normalTex is Texture2D)
                    {
                        material.NormalTexture = ExportNormalTextureInfo(normalTex, materialObj);
                        ExportTextureTransform(material.NormalTexture, materialObj, normalMapProperty);
                    }
                    else
                    {
                        LogPool.ExportLogger.LogError(LogPart.Material, $"Can't export a {normalTex.GetType()} {normalMapProperty} in material {materialObj.name}");
                    }
                }
            }

            if (GetOneMaterialProperty(materialObj, MATERIAL_PROPERTY_OcclusionMap, out string occlusionMapProperty))
            {
                var occTex = materialObj.GetTexture(occlusionMapProperty);
                if (occTex != null)
                {
                    if (occTex is Texture2D)
                    {
                        material.OcclusionTexture = ExportOcclusionTextureInfo(occTex, TextureMapType.Occlusion, materialObj);
                        ExportTextureTransform(material.OcclusionTexture, materialObj, occlusionMapProperty);
                    }
                    else
                    {
                        LogPool.ExportLogger.LogError(LogPart.Material, $"Can't export a {occTex.GetType()} {occlusionMapProperty} in material {materialObj.name}");
                    }
                }
            }

            material.PbrMetallicRoughness = ExportPBRMetallicRoughness(materialObj);

            if (IsUnlit(materialObj) || (useCustomShader && ExportUnlitWhenUsingCustomShader))
            {
                material.AddExtension(_root, KHR_materials_unlitExtensionFactory.EXTENSION_NAME, new KHR_materials_unlitExtension(), RequireExtensions);
            }
            else if (IsPBRSpecularGlossiness(materialObj))
            {
                ExportPBRSpecularGlossiness(material, materialObj);
            }
            if (IsCommonConstant(materialObj))
            {
                material.CommonConstant = ExportCommonConstant(materialObj);
            }
            if (IsClearcoat(materialObj))
            {
                ExportPBRClearcoat(material, materialObj);
            }

            _materials.Add(materialObj);

            id = new MaterialId
            {
                Id = _root.Materials.Count,
                Root = _root
            };

            _root.Materials.Add(material);

            return id;
        }
        public static readonly List<string> MATERIAL_PROPERTY_BaseColor = new List<string>() { "_BaseColor", "_Color", "_MainColor" };
        public static readonly List<string> MATERIAL_PROPERTY_BaseMap = new List<string>() { "_BaseMap", "_MainTex", "_BaseTexture", "_MainTexture" };
        public static readonly List<string> MATERIAL_PROPERTY_NormalMap = new List<string>() { "_BumpMap", "_NormalMap", "_DetailNormalMap", "_BumpTexture", "_NormalTexture", "_BumpTex", "_NormalTex" };
        public static readonly List<string> MATERIAL_PROPERTY_EmissionMap = new List<string>() { "_EmissionMap", "_EmissionTex", "_EmissionTexture" };
        public static readonly List<string> MATERIAL_PROPERTY_OcclusionMap = new List<string>() { "_OcclusionMap", "_OcclusionTex", "_OcclusionTexture" };
        public static readonly List<string> MATERIAL_PROPERTY_Metallic = new List<string>() { "_Metallic", "_MetallicFactor" };
        public static readonly List<string> MATERIAL_PROPERTY_Smoothness = new List<string>() { "_Smoothness", "_SmoothnessFactor" };
        public static readonly List<string> MATERIAL_PROPERTY_Roughness = new List<string>() { "_Roughness", "_RoughnessFactor" };
        public static readonly List<string> MATERIAL_PROPERTY_MetallicGlossMap = new List<string>() { "_MetallicGlossMap", "_SpecGlossMap", "_GlossMap", "_SpecularMap" };
        public bool GetOneMaterialProperty(Material material, List<string> materialProperties, out string property)
        {
            foreach (var prop in materialProperties)
            {
                if (material.HasProperty(prop))
                {
                    property = prop;
                    return true;
                }
            }
            property = null;
            return false;
        }
        private PbrMetallicRoughness ExportPBRMetallicRoughness(Material material)
        {
            var pbr = new PbrMetallicRoughness();

            if (GetOneMaterialProperty(material, MATERIAL_PROPERTY_BaseColor, out string baseColorProperty))
            {
                pbr.BaseColorFactor = material.GetColor(baseColorProperty);
            }

            if (GetOneMaterialProperty(material, MATERIAL_PROPERTY_BaseMap, out string baseMapProperty))
            {
                var mainTex = material.GetTexture(baseMapProperty);

                if (mainTex != null)
                {
                    if (mainTex is Texture2D)
                    {
                        pbr.BaseColorTexture = ExportTextureInfo(mainTex, TextureMapType.Main);
                        ExportTextureTransform(pbr.BaseColorTexture, material, baseMapProperty);
                    }
                    else
                    {
                        LogPool.ExportLogger.LogError(LogPart.Material, $"Can't export a {mainTex.GetType()} {baseMapProperty} in material {material.name}");
                    }
                }
            }

            bool hasMetallicGlossMap = false;
            if (GetOneMaterialProperty(material, MATERIAL_PROPERTY_MetallicGlossMap, out string metallicGlossMapProperty))
            {
                var mrTex = material.GetTexture(metallicGlossMapProperty);

                if (mrTex != null)
                {
                    if (mrTex is Texture2D)
                    {
                        hasMetallicGlossMap = true;
                        pbr.MetallicRoughnessTexture = ExportTextureInfo(mrTex, TextureMapType.MetallicGloss);
                        ExportTextureTransform(pbr.MetallicRoughnessTexture, material, metallicGlossMapProperty);
                    }
                    else
                    {
                        LogPool.ExportLogger.LogError(LogPart.Material, $"Can't export a {mrTex.GetType()} metallic smoothness texture in material {material.name}");
                    }
                }
            }

            if (GetOneMaterialProperty(material, MATERIAL_PROPERTY_Metallic, out string metallicProperty))
            {
                pbr.MetallicFactor = hasMetallicGlossMap ? 1.0f : material.GetFloat(metallicProperty);
            }

            if (GetOneMaterialProperty(material, MATERIAL_PROPERTY_Smoothness, out string smoothnessProperty))
            {
                pbr.RoughnessFactor = /*hasMetallicGlossMap ? 1.0 : */1.0f - material.GetFloat(smoothnessProperty);
            }
            else if (GetOneMaterialProperty(material, MATERIAL_PROPERTY_Roughness, out string roughnessProperty))
            {
                pbr.RoughnessFactor = /*hasMetallicGlossMap ? 1.0 : */material.GetFloat(roughnessProperty);
            }
            return pbr;
        }

        private MaterialCommonConstant ExportCommonConstant(Material material)
        {
            CheckExtension("KHR_materials_common");

            var constant = new MaterialCommonConstant();

            if (material.HasProperty("_AmbientFactor"))
            {
                constant.AmbientFactor = material.GetColor("_AmbientFactor");
            }

            if (material.HasProperty("_LightMap"))
            {
                var lmTex = material.GetTexture("_LightMap");

                if (lmTex != null)
                {
                    constant.LightmapTexture = ExportTextureInfo(lmTex, TextureMapType.LightMapDir);
                    ExportTextureTransform(constant.LightmapTexture, material, "_LightMap");
                }

            }

            if (material.HasProperty("_LightFactor"))
            {
                constant.LightmapFactor = material.GetColor("_LightFactor");
            }

            return constant;
        }

        private void ExportPBRSpecularGlossiness(GLTFMaterial gltfMaterial, Material material)
        {
            var specColor = material.GetColor("_BaseColor");

            var baseTex = material.GetTexture("_BaseMap");
            var baseTexInfo = baseTex == null ? null : ExportTextureInfo(baseTex, TextureMapType.SpecGloss);

            var specFactor = material.GetFloat("_GlossMapScale");

            var glossTex = material.GetTexture("_SpecGlossMap");
            var glossTexInfo = glossTex == null ? null : ExportTextureInfo(glossTex, TextureMapType.SpecGloss);

            var pbrSpec = new KHR_materials_pbrSpecularGlossinessExtension(specColor, baseTexInfo, new Vector3(specColor.r, specColor.g, specColor.b), specFactor, glossTexInfo);
            gltfMaterial.AddExtension(_root, KHR_materials_pbrSpecularGlossinessExtensionFactory.EXTENSION_NAME, pbrSpec, RequireExtensions);
        }

        /// <summary>
        /// Clearcoat map is not fully support yet, but still can be used inside Unity with this SDK
        /// </summary>
        /// <param name="gltfMaterial"></param>
        /// <param name="material"></param>
        private void ExportPBRClearcoat(GLTFMaterial gltfMaterial, Material material)
        {
            var clearcoatFactor = material.GetFloat("_ClearCoatMask");
            // Red: the Mask property.
            // Green: the Smoothness property.
            var clearcoatTexture = material.GetTexture("_ClearCoatMap");
            var clearcoatTextureInfo = clearcoatTexture == null ? null : ExportTextureInfo(clearcoatTexture, TextureMapType.Main);

            var clearcoatRoughnessFactor = 1.0f - material.GetFloat("_ClearCoatSmoothness");

            //var clearcoatRoughnessTexture = material.GetTexture("_ClearCoatMap");
            //var clearcoatRoughnessTextureInfo = clearcoatRoughnessTexture == null ? null : ExportTextureInfo(clearcoatRoughnessTexture, TextureMapType.OneMinusGreenChannel);

            var pbrClearcoat = new KHR_materials_clearcoatExtension(clearcoatFactor, clearcoatTextureInfo, clearcoatRoughnessFactor, null, null);
            gltfMaterial.AddExtension(_root, KHR_materials_clearcoatExtensionFactory.EXTENSION_NAME, pbrClearcoat, RequireExtensions);
        }

        public MaterialId GetMaterialId(GLTFRoot root, Material materialObj)
        {
            for (var i = 0; i < _materials.Count; i++)
            {
                if (_materials[i] == materialObj)
                {
                    return new MaterialId
                    {
                        Id = i,
                        Root = root
                    };
                }
            }

            return null;
        }
    }
}

