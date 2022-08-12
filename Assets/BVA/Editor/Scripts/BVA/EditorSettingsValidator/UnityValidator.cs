using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using UnityEditor.Rendering;

namespace BVA
{
    public sealed class URPSettingsValidator : ISettingsValidator
    {
        public bool IsValid => GetHeaderDescription()==null;
        public string HeaderDescription => GetHeaderDescriptionFull();
        public string CurrentValueDescription => "";
        public string RecommendedValueDescription => IsValid ? ExportCommon.Localization("已修复", "OK") : ExportCommon.Localization("修复", "Fix it");

        public bool CanFix => true;

        string GetHeaderDescriptionFull()
        {
            var result = GetHeaderDescription();
            if (result == null)
            {
                result = ExportCommon.Localization("URPSetting设置无冲突", "Color Space Settings is valid");
            }
            return result;
        }

        string GetHeaderDescription()
        {
            if (QualitySettings.renderPipeline == null || GraphicsSettings.renderPipelineAsset == null)
            {
                return ExportCommon.Localization("构建使用 BVA 的应用需要有效的通用渲染管线资源", "A valid Universal Render Pipeline Asset is required for build the App that using BVA");
            }

            return null;
        }

        public void Validate()
        {
            UniversalRenderPipelineAsset pipeLineAsset = AssetDatabase.LoadAssetAtPath("Packages/com.bilibili.bva/URP/BVAUniversalRenderPipelineAsset.asset", typeof(UniversalRenderPipelineAsset)) as UniversalRenderPipelineAsset;
            if (pipeLineAsset == null)
            {
                pipeLineAsset = AssetDatabase.LoadAssetAtPath("Assets/BVA/URP/BVAUniversalRenderPipelineAsset.asset", typeof(UniversalRenderPipelineAsset)) as UniversalRenderPipelineAsset;
            }
            Debug.Assert(pipeLineAsset != null);

            QualitySettings.renderPipeline = GraphicsSettings.renderPipelineAsset = pipeLineAsset;
        }
    }

    public sealed class ColorSpaceSettingsValidator : ISettingsValidator
    {
        public bool IsValid => GetHeaderDescription() == null;
        public string HeaderDescription => GetHeaderDescriptionFull();
        public string CurrentValueDescription => "";
        public string RecommendedValueDescription => IsValid ? ExportCommon.Localization("已修复", "OK") : ExportCommon.Localization("修复", "Fix it");

        public bool CanFix => true;

        string GetHeaderDescriptionFull()
        {
            var result = GetHeaderDescription();
            if (result == null)
            {
                result = ExportCommon.Localization("色彩空间设置无冲突", "Color Space Settings is valid");
            }
            return result;
        }
        string GetHeaderDescription()
        {
            if (PlayerSettings.colorSpace != UnityEngine.ColorSpace.Linear)
            {
                return ExportCommon.Localization("色彩空间：应在“玩家设置”面板上启用线性色彩空间以获得最佳效果。当前设置为", "COLORSPACE: Linear color space should be enabled on Player Settings Panel for best results. Currently set to "
                ) + PlayerSettings.colorSpace.ToString();
            }
            
            return null;
        }
        public void Validate()
        {
            PlayerSettings.colorSpace = UnityEngine.ColorSpace.Linear;
        }
    }

    public sealed class ReflectionProbeSettingsValidator : ISettingsValidator
    {
        public bool IsValid => GetHeaderDescription() == null;
        public string HeaderDescription => GetHeaderDescriptionFull();
        public string CurrentValueDescription => "";
        public string RecommendedValueDescription => IsValid ? ExportCommon.Localization("已修复", "OK") : ExportCommon.Localization("修复", "Fix it");

        public bool CanFix => true;

        string GetHeaderDescriptionFull()
        {
            var result = GetHeaderDescription();
            if (result == null)
            {
                result = ExportCommon.Localization("反射探针设置无冲突", "Reflection Probe Settings is valid");
            }
            return result;
        }

        string GetHeaderDescription()
        {
            if (Graphics.activeTier == GraphicsTier.Tier1)
            {
                if (EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, GraphicsTier.Tier1).reflectionProbeBlending)
                {
                    return ExportCommon.Localization("图形第 1 层：不支持反射探头混合。在 Tier 1 设置面板上禁用反射探头混合以获得最佳效果"
               , "GRAPHICS TIER 1: Reflection probe blending not supported. Disable reflection probe blending on Tier 1 Settings Panel for best results.");
                }
            }
            else if (Graphics.activeTier == GraphicsTier.Tier2)
            {
                if (EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, GraphicsTier.Tier2).reflectionProbeBlending)
                {
                    return ExportCommon.Localization("图形第 2 层：不支持反射探头混合。在 Tier2 设置面板上禁用反射探头混合以获得最佳效果"
               , "GRAPHICS TIER 1: Reflection probe blending not supported. Disable reflection probe blending on Tier 1 Settings Panel for best results.");
                }
            }
            else if (Graphics.activeTier == GraphicsTier.Tier3 && EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, GraphicsTier.Tier3).reflectionProbeBlending)
            {
                return ExportCommon.Localization("图形第 3 层：不支持反射探头混合。在 Tier 3 设置面板上禁用反射探头混合以获得最佳效果"
               , "GRAPHICS TIER 1: Reflection probe blending not supported. Disable reflection probe blending on Tier 1 Settings Panel for best results.");
            }
            return null;
        }

        public void Validate()
        {
            var setting = EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, Graphics.activeTier);
            setting.reflectionProbeBlending = false;
            EditorGraphicsSettings.SetTierSettings(BuildTargetGroup.Standalone, Graphics.activeTier, setting);
        }

    }

    public sealed class LightmapSettingsValidator : ISettingsValidator
    {
        public bool IsValid => GetHeaderDescription() == null;
        public string HeaderDescription => GetHeaderDescriptionFull();
        public string CurrentValueDescription => "";
        public string RecommendedValueDescription => IsValid ? ExportCommon.Localization("已修复", "OK") : ExportCommon.Localization("修复", "Fix it");

        public bool CanFix => true;

        string GetHeaderDescriptionFull()
        {
            var result = GetHeaderDescription();
            if (result == null)
            {
                result = ExportCommon.Localization("光照设置无冲突", "Lightmap Settings is valid");
            }
            return result;
        }

        string GetHeaderDescription()
        {
            if (Lightmapping.realtimeGI)
            {
                return ExportCommon.Localization("光照贴图：应在“光照”面板上禁用实时全局照明以获得最佳效果", "LIGHTMAPS: Realtime global illumination should be disabled on Lighting Panel for best results.");
            }
            if (!Lightmapping.bakedGI)
            {
                return ExportCommon.Localization("光照贴图：应在“光照”面板上启用烘焙的全局照明以获得最佳效果", "LIGHTMAPS: Baked global illumination should be enabled on Lighting Panel for best results.");
            }

            if (Lightmapping.bakedGI)
            {
                if (GetLightmapBakeMode() != MixedLightingMode.Subtractive)
                {
                    return ExportCommon.Localization("光照贴图：建议使用减法光照模式，不应在“光照面板”上选择该模式以获得最佳效果", "LIGHTMAPS: Subtractive lighting mode is recommended and should not be selected on Lighting Panel for best results.");
                }
                if (GetLightmapDirectionMode() != LightmapsMode.NonDirectional)
                {
                    //Debug.LogWarning("LIGHTMAPS: Non directional lighting mode should be selected on Lighting Panel for best results.");
                }
                if (GetLightmapCompressionEnabled())
                {
                    return ExportCommon.Localization("光照贴图：应在“光照”面板上禁用纹理压缩模式以获得最佳效果。", "LIGHTMAPS: Texture compression mode should be disabled on Lighting Panel for best results.");
                }
            }
            return null;
        }
        public static LightmapsMode GetLightmapDirectionMode()
        {
            LightmapsMode result = LightmapsMode.CombinedDirectional;
            LightingSettings lightingSettings = GetLightingSettings();
            if (lightingSettings != null)
            {
                result = lightingSettings.directionalityMode;
            }
            else
            {
                //Debug.LogWarning("LIGHTMAPS: Failed to get lighting editor settings");
            }
            return result;
        }

        public static bool GetLightmapCompressionEnabled()
        {
            bool result = true;
            LightingSettings lightingSettings = GetLightingSettings();
            if (lightingSettings != null)
            {
#if UNITY_2021_1_OR_NEWER
                result = lightingSettings.lightmapCompression != LightmapCompression.None;
#endif
            }
            else
            {
                //Debug.LogWarning("LIGHTMAPS: Failed to get lighting editor settings");
            }
            return result;
        }
        public static LightingSettings GetLightingSettings()
        {
            try
            {
                return Lightmapping.lightingSettings;
            }
            catch (Exception)
            {

                return null;
            }

        }
        public static MixedLightingMode GetLightmapBakeMode()
        {
            MixedLightingMode result = 0;
            LightingSettings lightingSettings = GetLightingSettings();
            if (lightingSettings != null)
            {
                result = lightingSettings.mixedBakeMode;
            }
            else
            {
                //Debug.LogWarning("LIGHTMAPS: Failed to get lighting editor settings");
            }
            return result;
        }
        public void Validate()
        {
            if (Lightmapping.bakedGI)
            {
                LightingSettings setting = GetLightingSettings();
                if (setting != null)
                {
#if UNITY_2021_1_OR_NEWER
                    setting.lightmapCompression = LightmapCompression.None;
#endif
                    setting.mixedBakeMode = MixedLightingMode.Subtractive;
                    setting.directionalityMode = LightmapsMode.NonDirectional;
                }
            }
            Lightmapping.realtimeGI = false;

        }

    }

    public sealed class PlayerSettingsValidator : ISettingsValidator
    {
        public bool IsValid => GetHeaderDescription() == null;
        public string HeaderDescription => GetHeaderDescriptionFull();
        public string CurrentValueDescription => "";
        public string RecommendedValueDescription => IsValid ? ExportCommon.Localization("已修复", "OK") : ExportCommon.Localization("修复", "Fix it");

        public bool CanFix => true;

        string GetHeaderDescriptionFull()
        {
            var result = GetHeaderDescription();
            if (result == null)
            {
                result = ExportCommon.Localization("PlayerSetting无冲突", "Player Settings is valid");
            }
            return result;
        }
        string GetHeaderDescription()
        {
            if (
                PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.Standalone) != ManagedStrippingLevel.Disabled ||
                 PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.Android) != ManagedStrippingLevel.Disabled ||
                  PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.iOS) != ManagedStrippingLevel.Disabled
            )
            {
                return ExportCommon.Localization("Managed Stripping Level 没有关闭", "Managed Stripping Level is not Disabled");
            }
            if (PlayerSettings.assemblyVersionValidation)
            {
                return ExportCommon.Localization("Assembly Version Validation 应该不勾选", "Assembly Version Validation should be false");
            }
            if (!PlayerSettings.allowUnsafeCode)
            {
                return ExportCommon.Localization("Allow Unsafe Code 应该不勾选", "Allow Unsafe Code should be true");
            }
            return null;
        }

        public void Validate()
        {
            PlayerSettings.assemblyVersionValidation = false;
            PlayerSettings.allowUnsafeCode = true;
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Standalone, ManagedStrippingLevel.Disabled);
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Disabled);
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Disabled);
        }

    }

    public sealed class NormalMapsValidator : ISettingsValidator
    {
        public bool IsValid => GetHeaderDescription() == null;
        public string HeaderDescription => GetHeaderDescription();
        public string CurrentValueDescription => "";
        public string RecommendedValueDescription => IsValid ? ExportCommon.Localization("已修复", "OK"): ExportCommon.Localization("修复", "Fix it");
        public bool CanFix => false;



        string GetHeaderDescription()
        {
            if (
                 PlayerSettings.GetNormalMapEncoding(BuildTargetGroup.Standalone) != NormalMapEncoding.XYZ ||
                  PlayerSettings.GetNormalMapEncoding(BuildTargetGroup.Android) != NormalMapEncoding.XYZ ||
                   PlayerSettings.GetNormalMapEncoding(BuildTargetGroup.iOS) != NormalMapEncoding.XYZ
                )
            {
                return ExportCommon.Localization("Normal Map Encoding 不是XYZ", "Normal Map Encoding is not XYZ");
            }
            return null;
        }

        public void Validate()
        {
            PlayerSettings.SetNormalMapEncoding(BuildTargetGroup.Standalone, NormalMapEncoding.XYZ);
            PlayerSettings.SetNormalMapEncoding(BuildTargetGroup.Android, NormalMapEncoding.XYZ);
            PlayerSettings.SetNormalMapEncoding(BuildTargetGroup.iOS, NormalMapEncoding.XYZ);

            if (GetHeaderDescription() != null)
            {
            }
        }

    }
}
