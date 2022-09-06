using UnityEngine;
using System;
using UnityEngine.Rendering;

public static class BVAMaterialExtension
{
    #region MToon
    private static class MToon
    {
        private const string PropVersion = "_MToonVersion";
        private const string PropDebugMode = "_DebugMode";
        private const string PropOutlineWidthMode = "_OutlineWidthMode";
        private const string PropOutlineColorMode = "_OutlineColorMode";
        private const string PropBlendMode = "_BlendMode";
        private const string PropCullMode = "_CullMode";
        private const string PropOutlineCullMode = "_OutlineCullMode";
        private const string PropCutoff = "_Cutoff";
        private const string PropColor = "_Color";
        private const string PropShadeColor = "_ShadeColor";
        private const string PropMainTex = "_MainTex";
        private const string PropShadeTexture = "_ShadeTexture";
        private const string PropBumpScale = "_BumpScale";
        private const string PropBumpMap = "_BumpMap";
        private const string PropReceiveShadowRate = "_ReceiveShadowRate";
        private const string PropReceiveShadowTexture = "_ReceiveShadowTexture";
        private const string PropShadingGradeRate = "_ShadingGradeRate";
        private const string PropShadingGradeTexture = "_ShadingGradeTexture";
        private const string PropShadeShift = "_ShadeShift";
        private const string PropShadeToony = "_ShadeToony";
        private const string PropLightColorAttenuation = "_LightColorAttenuation";
        private const string PropIndirectLightIntensity = "_IndirectLightIntensity";
        private const string PropRimColor = "_RimColor";
        private const string PropRimTexture = "_RimTexture";
        private const string PropRimLightingMix = "_RimLightingMix";
        private const string PropRimFresnelPower = "_RimFresnelPower";
        private const string PropRimLift = "_RimLift";
        private const string PropSphereAdd = "_SphereAdd";
        private const string PropEmissionColor = "_EmissionColor";
        private const string PropEmissionMap = "_EmissionMap";
        private const string PropOutlineWidthTexture = "_OutlineWidthTexture";
        private const string PropOutlineWidth = "_OutlineWidth";
        private const string PropOutlineScaledMaxDistance = "_OutlineScaledMaxDistance";
        private const string PropOutlineColor = "_OutlineColor";
        private const string PropOutlineLightingMix = "_OutlineLightingMix";
        private const string PropUvAnimMaskTexture = "_UvAnimMaskTexture";
        private const string PropUvAnimScrollX = "_UvAnimScrollX";
        private const string PropUvAnimScrollY = "_UvAnimScrollY";
        private const string PropUvAnimRotation = "_UvAnimRotation";
        private const string PropSrcBlend = "_SrcBlend";
        private const string PropDstBlend = "_DstBlend";
        private const string PropZWrite = "_ZWrite";
        private const string PropAlphaToMask = "_AlphaToMask";

        private const string KeyNormalMap = "_NORMALMAP";
        private const string KeyAlphaTestOn = "_ALPHATEST_ON";
        private const string KeyAlphaBlendOn = "_ALPHABLEND_ON";
        private const string KeyAlphaPremultiplyOn = "_ALPHAPREMULTIPLY_ON";
        private const string KeyOutlineWidthWorld = "MTOON_OUTLINE_WIDTH_WORLD";
        private const string KeyOutlineWidthScreen = "MTOON_OUTLINE_WIDTH_SCREEN";
        private const string KeyOutlineColorFixed = "MTOON_OUTLINE_COLOR_FIXED";
        private const string KeyOutlineColorMixed = "MTOON_OUTLINE_COLOR_MIXED";
        private const string KeyDebugNormal = "MTOON_DEBUG_NORMAL";
        private const string KeyDebugLitShadeRate = "MTOON_DEBUG_LITSHADERATE";

        private const string TagRenderTypeKey = "RenderType";
        private const string TagRenderTypeValueOpaque = "Opaque";
        private const string TagRenderTypeValueTransparentCutout = "TransparentCutout";
        private const string TagRenderTypeValueTransparent = "Transparent";

        private const int DisabledIntValue = 0;
        private const int EnabledIntValue = 1;
        private enum DebugMode
        {
            None = 0,
            Normal = 1,
            LitShadeRate = 2,
        }

        private enum OutlineColorMode
        {
            FixedColor = 0,
            MixedLighting = 1,
        }

        private enum OutlineWidthMode
        {
            None = 0,
            WorldCoordinates = 1,
            ScreenCoordinates = 2,
        }

        private enum RenderMode
        {
            Opaque = 0,
            Cutout = 1,
            Transparent = 2,
            TransparentWithZWrite = 3,
        }

        private enum CullMode
        {
            Off = 0,
            Front = 1,
            Back = 2,
        }

        private struct RenderQueueRequirement
        {
            public int DefaultValue;
            public int MinValue;
            public int MaxValue;
        }
        public static void ValidateProperties(Material material, bool isBlendModeChangedByUser = false)
        {
            SetRenderMode(material,
                (RenderMode)material.GetFloat(PropBlendMode),
                material.renderQueue - GetRenderQueueRequirement((RenderMode)material.GetFloat(PropBlendMode)).DefaultValue,
                useDefaultRenderQueue: isBlendModeChangedByUser);
            SetNormalMapping(material, material.GetTexture(PropBumpMap), material.GetFloat(PropBumpScale));
            SetOutlineMode(material,
                (OutlineWidthMode)material.GetFloat(PropOutlineWidthMode),
                (OutlineColorMode)material.GetFloat(PropOutlineColorMode));
            //SetDebugMode(material, (DebugMode)material.GetFloat(PropDebugMode));
            SetCullMode(material, (CullMode)material.GetFloat(PropCullMode));

            var mainTex = material.GetTexture(PropMainTex);
            var shadeTex = material.GetTexture(PropShadeTexture);
            if (mainTex != null && shadeTex == null)
            {
                material.SetTexture(PropShadeTexture, mainTex);
            }
        }
        private static void SetRenderMode(Material material, RenderMode renderMode, int renderQueueOffset,
            bool useDefaultRenderQueue)
        {
            SetValue(material, PropBlendMode, (int)renderMode);

            switch (renderMode)
            {
                case RenderMode.Opaque:
                    material.SetOverrideTag(TagRenderTypeKey, TagRenderTypeValueOpaque);
                    material.SetInt(PropSrcBlend, (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt(PropDstBlend, (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt(PropZWrite, EnabledIntValue);
                    material.SetInt(PropAlphaToMask, DisabledIntValue);
                    SetKeyword(material, KeyAlphaTestOn, false);
                    SetKeyword(material, KeyAlphaBlendOn, false);
                    SetKeyword(material, KeyAlphaPremultiplyOn, false);
                    break;
                case RenderMode.Cutout:
                    material.SetOverrideTag(TagRenderTypeKey, TagRenderTypeValueTransparentCutout);
                    material.SetInt(PropSrcBlend, (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt(PropDstBlend, (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt(PropZWrite, EnabledIntValue);
                    material.SetInt(PropAlphaToMask, EnabledIntValue);
                    SetKeyword(material, KeyAlphaTestOn, true);
                    SetKeyword(material, KeyAlphaBlendOn, false);
                    SetKeyword(material, KeyAlphaPremultiplyOn, false);
                    break;
                case RenderMode.Transparent:
                    material.SetOverrideTag(TagRenderTypeKey, TagRenderTypeValueTransparent);
                    material.SetInt(PropSrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt(PropDstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt(PropZWrite, DisabledIntValue);
                    material.SetInt(PropAlphaToMask, DisabledIntValue);
                    SetKeyword(material, KeyAlphaTestOn, false);
                    SetKeyword(material, KeyAlphaBlendOn, true);
                    SetKeyword(material, KeyAlphaPremultiplyOn, false);
                    break;
                case RenderMode.TransparentWithZWrite:
                    material.SetOverrideTag(TagRenderTypeKey, TagRenderTypeValueTransparent);
                    material.SetInt(PropSrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt(PropDstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt(PropZWrite, EnabledIntValue);
                    material.SetInt(PropAlphaToMask, DisabledIntValue);
                    SetKeyword(material, KeyAlphaTestOn, false);
                    SetKeyword(material, KeyAlphaBlendOn, true);
                    SetKeyword(material, KeyAlphaPremultiplyOn, false);
                    break;
            }

            if (useDefaultRenderQueue)
            {
                var requirement = GetRenderQueueRequirement(renderMode);
                material.renderQueue = requirement.DefaultValue;
            }
            else
            {
                var requirement = GetRenderQueueRequirement(renderMode);
                material.renderQueue = Mathf.Clamp(
                    requirement.DefaultValue + renderQueueOffset, requirement.MinValue, requirement.MaxValue);
            }
        }
        private static void SetValue(Material material, string propertyName, float val)
        {
            material.SetFloat(propertyName, val);
        }

        private static void SetKeyword(Material mat, string keyword, bool required)
        {
            if (required)
                mat.EnableKeyword(keyword);
            else
                mat.DisableKeyword(keyword);
        }
        private static void SetTexture(Material material, string propertyName, Texture texture)
        {
            material.SetTexture(propertyName, texture);
        }
        private static void SetNormalMapping(Material material, Texture bumpMap, float bumpScale)
        {
            SetTexture(material, PropBumpMap, bumpMap);
            SetValue(material, PropBumpScale, bumpScale);

            SetKeyword(material, KeyNormalMap, bumpMap != null);
        }
        private static void SetOutlineMode(Material material, OutlineWidthMode outlineWidthMode,
            OutlineColorMode outlineColorMode)
        {
            SetValue(material, PropOutlineWidthMode, (int)outlineWidthMode);
            SetValue(material, PropOutlineColorMode, (int)outlineColorMode);

            var isFixed = outlineColorMode == OutlineColorMode.FixedColor;
            var isMixed = outlineColorMode == OutlineColorMode.MixedLighting;

            switch (outlineWidthMode)
            {
                case OutlineWidthMode.None:
                    SetKeyword(material, KeyOutlineWidthWorld, false);
                    SetKeyword(material, KeyOutlineWidthScreen, false);
                    SetKeyword(material, KeyOutlineColorFixed, false);
                    SetKeyword(material, KeyOutlineColorMixed, false);
                    break;
                case OutlineWidthMode.WorldCoordinates:
                    SetKeyword(material, KeyOutlineWidthWorld, true);
                    SetKeyword(material, KeyOutlineWidthScreen, false);
                    SetKeyword(material, KeyOutlineColorFixed, isFixed);
                    SetKeyword(material, KeyOutlineColorMixed, isMixed);
                    break;
                case OutlineWidthMode.ScreenCoordinates:
                    SetKeyword(material, KeyOutlineWidthWorld, false);
                    SetKeyword(material, KeyOutlineWidthScreen, true);
                    SetKeyword(material, KeyOutlineColorFixed, isFixed);
                    SetKeyword(material, KeyOutlineColorMixed, isMixed);
                    break;
            }
        }
        private static void SetCullMode(Material material, CullMode cullMode)
        {
            SetValue(material, PropCullMode, (int)cullMode);

            switch (cullMode)
            {
                case CullMode.Back:
                    material.SetInt(PropCullMode, (int)CullMode.Back);
                    material.SetInt(PropOutlineCullMode, (int)CullMode.Front);
                    break;
                case CullMode.Front:
                    material.SetInt(PropCullMode, (int)CullMode.Front);
                    material.SetInt(PropOutlineCullMode, (int)CullMode.Back);
                    break;
                case CullMode.Off:
                    material.SetInt(PropCullMode, (int)CullMode.Off);
                    material.SetInt(PropOutlineCullMode, (int)CullMode.Front);
                    break;
            }
        }
        private static RenderQueueRequirement GetRenderQueueRequirement(RenderMode renderMode)
        {
            const int shaderDefaultQueue = -1;
            const int firstTransparentQueue = 2501;
            const int spanOfQueue = 50;

            switch (renderMode)
            {
                case RenderMode.Opaque:
                    return new RenderQueueRequirement()
                    {
                        DefaultValue = shaderDefaultQueue,
                        MinValue = shaderDefaultQueue,
                        MaxValue = shaderDefaultQueue,
                    };
                case RenderMode.Cutout:
                    return new RenderQueueRequirement()
                    {
                        DefaultValue = (int)RenderQueue.AlphaTest,
                        MinValue = (int)RenderQueue.AlphaTest,
                        MaxValue = (int)RenderQueue.AlphaTest,
                    };
                case RenderMode.Transparent:
                    return new RenderQueueRequirement()
                    {
                        DefaultValue = (int)RenderQueue.Transparent,
                        MinValue = (int)RenderQueue.Transparent - spanOfQueue + 1,
                        MaxValue = (int)RenderQueue.Transparent,
                    };
                case RenderMode.TransparentWithZWrite:
                    return new RenderQueueRequirement()
                    {
                        DefaultValue = firstTransparentQueue,
                        MinValue = firstTransparentQueue,
                        MaxValue = firstTransparentQueue + spanOfQueue - 1,
                    };
                default:
                    throw new ArgumentOutOfRangeException("renderMode", renderMode, null);
            }
        }
    }

    #endregion
    public static void ChangeMaterial(GameObject gameObject)
    {
        Shader urpMToonShader = Shader.Find("VRM/URP/MToon");
        Shader urpUnlitShader = Shader.Find("Universal Render Pipeline/Unlit");
        Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
        var skinnedMeshRenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {
            foreach (var v in skinnedMeshRenderer.materials)
            {
                if (v.shader.name.ToLower().Contains("mtoon"))
                {
                    v.shader = urpMToonShader;
                    MToon.ValidateProperties(v);
                }
                if (v.shader.name.ToLower().Contains("unlit"))
                {
                    Texture baseMap = v.GetTexture("_MainTex");
                    v.shader = urpUnlitShader;
                    v.SetTexture("_BaseMap", baseMap);
                }
                if (v.shader.name.ToLower().Contains("standard"))
                {
                    Texture baseMap = v.GetTexture("_MainTex");
                    Color baseColor = v.GetColor("_Color");
                    float smoothness = v.GetFloat("_Glossiness");

                    v.shader = urpLitShader;
                    v.SetTexture("_BaseMap", baseMap);
                    v.SetColor("_BaseColor", baseColor);
                    v.SetFloat("_Smoothness", smoothness);
                }
            }
        }
    }
}