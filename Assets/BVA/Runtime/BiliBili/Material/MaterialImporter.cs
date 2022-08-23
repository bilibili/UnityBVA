using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BVA;
using UnityEngine.Rendering;
using System.Linq;

namespace GLTF.Schema.BVA
{
    public enum SurfaceType
    {
        Opaque,
        Transparent
    }

    public enum WorkflowMode
    {
        Specular = 0,
        Metallic
    }

    public interface IMaterialExtra : IExtra,ICloneable
    {
        Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache, AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap);
        void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo);
        string ShaderName { get; }
        string ExtraName { get; }
    }
    public class MaterialExtraAttribute : Attribute {}

    public static class MaterialImporter
    {
        private static Dictionary<string, IMaterialExtra> customShaders;
        public static Dictionary<string, IMaterialExtra> CustomShaders
        {
            get
            {
                if (customShaders == null)
                {
                    customShaders = new Dictionary<string, IMaterialExtra>();
                    System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(typeof(MaterialExtraAttribute));
                    Type[] types = asm.GetExportedTypes();

                    Func<Attribute[], bool> IsAttribute = o =>
                    {
                        foreach (Attribute a in o)
                        {
                            if (a is MaterialExtraAttribute)
                                return true;
                        }
                        return false;
                    };

                    var cosType = types.Where(o =>
                    {
                        return IsAttribute(Attribute.GetCustomAttributes(o, true));
                    });
                    foreach (var extraType in cosType)
                    {
                        IMaterialExtra extraExtra = (IMaterialExtra)Activator.CreateInstance(extraType);
                        customShaders.Add(extraExtra.ShaderName, extraExtra);
                    }
                }
                return customShaders;
            }
        }

        /// <summary>
        /// for export material extra
        /// </summary>
        /// <param name="materialObj"></param>
        /// <param name="material"></param>
        /// <param name="exportTextureInfo"></param>
        /// <returns></returns>
        public static bool ExportMaterialExtra(Material materialObj, GLTFMaterial material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemap exportCubemapInfo)
        {
            string shader = materialObj.shader.name;
            foreach (var kvp in CustomShaders)
            {
                if (kvp.Key == shader)
                {
                    //Type t = kvp.Value.Item1;
                    //MaterialExtra extra = (MaterialExtra)Activator.CreateInstance(t, new object[] { materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo });
                    if (kvp.Value != null)
                    {
                        IMaterialExtra me = kvp.Value.Clone() as IMaterialExtra;
                        me.SetData(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                        material.AddExtra(me.ExtraName, me);
                    }
                    return true;
                }
            }
            return false;
        }

        public static void SetCommonMaterialKeywords(Material material)
        {
            // Receive Shadows
            if (material.HasProperty("_ReceiveShadows"))
                CoreUtils.SetKeyword(material, "_RECEIVE_SHADOWS_OFF", material.GetFloat("_ReceiveShadows") == 0.0f);
            // Emission
            bool shouldEmissionBeEnabled = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
            if (material.HasProperty("_EmissionEnabled") && !shouldEmissionBeEnabled)
                shouldEmissionBeEnabled = material.GetFloat("_EmissionEnabled") >= 0.5f;
            CoreUtils.SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);
            // Normal Map
            if (material.HasProperty("_BumpMap"))
            {
                CoreUtils.SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap"));
            }
            if (material.HasProperty("_NormalMap"))
            {
                CoreUtils.SetKeyword(material, "_NORMALMAP", material.GetTexture("_NormalMap"));
            }
        }

        #region ToonLit
        public static class ToonLit
        {
            public enum BlendMode
            {
                Alpha,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
                Premultiply, // Physically plausible transparency mode, implemented as alpha pre-multiply
                Additive,
                Multiply
            }
            public enum RoughnessMapChannel
            {
                SpecularMetallicAlpha,
                AlbedoAlpha,
            }
            public static RoughnessMapChannel GetRoughnessMapChannel(Material material)
            {
                int ch = (int)material.GetFloat("_RoughnessTextureChannel");
                if (ch == (int)RoughnessMapChannel.AlbedoAlpha)
                    return RoughnessMapChannel.AlbedoAlpha;

                return RoughnessMapChannel.SpecularMetallicAlpha;
            }
            public static void SetMaterialKeywords(Material material)
            {
                bool isSpecularWorkFlow = false;
                bool opaque = ((SurfaceType)material.GetFloat("_Surface") ==
                              SurfaceType.Opaque);
                // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
                // (MaterialProperty value might come from renderer material property block)
                bool hasGlossMap;
                if (material.HasProperty("_WorkflowMode"))
                {
                    isSpecularWorkFlow = (WorkflowMode)material.GetFloat("_WorkflowMode") == WorkflowMode.Specular;
                    if (isSpecularWorkFlow)
                        hasGlossMap = material.GetTexture("_SpecGlossMap") != null;
                    else
                        hasGlossMap = material.GetTexture("_MetallicGlossMap") != null;
                }
                else
                {
                    hasGlossMap = material.GetTexture("_MetallicGlossMap") != null;
                }

                CoreUtils.SetKeyword(material, "_SPECULAR_SETUP", isSpecularWorkFlow);

                CoreUtils.SetKeyword(material, "_METALLICSPECGLOSSMAP", hasGlossMap);

                if (material.HasProperty("_SpecularHighlights"))
                    CoreUtils.SetKeyword(material, "_SPECULARHIGHLIGHTS_OFF", material.GetFloat("_SpecularHighlights") == 0.0f);
                if (material.HasProperty("_EnvironmentReflections"))
                    CoreUtils.SetKeyword(material, "_ENVIRONMENTREFLECTIONS_OFF", material.GetFloat("_EnvironmentReflections") == 0.0f);
                if (material.HasProperty("_OcclusionMap"))
                    CoreUtils.SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));

                if (material.HasProperty("_RoughnessTextureChannel"))
                {
                    CoreUtils.SetKeyword(material, "_ROUGHNESS_TEXTURE_ALBEDO_CHANNEL_A",
                        GetRoughnessMapChannel(material) == RoughnessMapChannel.AlbedoAlpha && opaque);
                }
            }
            public static void SetupMaterialBlendMode(Material material)
            {
                if (material == null)
                    throw new ArgumentNullException("material");

                bool alphaClip = false;
                if (material.HasProperty("_AlphaClip"))
                    alphaClip = material.GetFloat("_AlphaClip") >= 0.5;


                if (alphaClip)
                {
                    material.EnableKeyword("_ALPHATEST_ON");
                }
                else
                {
                    material.DisableKeyword("_ALPHATEST_ON");
                }

                if (material.HasProperty("_Surface"))
                {
                    SurfaceType surfaceType = (SurfaceType)material.GetFloat("_Surface");
                    if (surfaceType == SurfaceType.Opaque)
                    {
                        if (alphaClip)
                        {
                            material.renderQueue = (int)RenderQueue.AlphaTest;
                            material.SetOverrideTag("RenderType", "TransparentCutout");

                            material.SetShaderPassEnabled("OutLine", false);//alpha clip的时候禁用外描边
                        }
                        else
                        {
                            material.renderQueue = (int)RenderQueue.Geometry;
                            material.SetOverrideTag("RenderType", "Opaque");

                            material.SetShaderPassEnabled("OutLine", true);
                        }

                        material.renderQueue += material.HasProperty("_QueueOffset") ? (int)material.GetFloat("_QueueOffset") : 0;
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        material.SetInt("_ZWrite", 1);
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.SetShaderPassEnabled("ShadowCaster", true);
                    }
                    else
                    {
                        BlendMode blendMode = (BlendMode)material.GetFloat("_Blend");

                        // Specific Transparent Mode Settings
                        switch (blendMode)
                        {
                            case BlendMode.Alpha:
                                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                                break;
                            case BlendMode.Premultiply:
                                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                                break;
                            case BlendMode.Additive:
                                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                                break;
                            case BlendMode.Multiply:
                                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.DstColor);
                                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                                material.EnableKeyword("_ALPHAMODULATE_ON");
                                break;
                        }

                        // General Transparent Material Settings
                        material.SetOverrideTag("RenderType", "Transparent");
                        material.SetInt("_ZWrite", 0);
                        material.renderQueue = (int)RenderQueue.Transparent;
                        material.renderQueue += material.HasProperty("_QueueOffset") ? (int)material.GetFloat("_QueueOffset") : 0;
                        material.SetShaderPassEnabled("OutLine", false);
                        material.SetShaderPassEnabled("ShadowCaster", false);
                    }
                }
            }
        }
        #endregion
        #region UTS Toon
        public static class UTS
        {
            public enum _UTS_Technique
            {
                DoubleShadeWithFeather, ShadingGradeMap
            }

            public enum _UTS_ClippingMode
            {
                Off, On, TransClippingMode
            }

            public enum _UTS_TransClippingMode
            {
                Off, On,
            }
            public enum _UTS_Transparent
            {
                Off, On,
            }
            public enum _UTS_StencilMode
            {
                Off, StencilOut, StencilMask
            }

            public enum _StencilOperation
            {
                //https://docs.unity3d.com/Manual/SL-Stencil.html
                Keep, //    Keep the current contents of the buffer.
                Zero, //    Write 0 into the buffer.
                Replace, // Write the reference value into the buffer.
                IncrSat, // Increment the current value in the buffer. If the value is 255 already, it stays at 255.
                DecrSat, // Decrement the current value in the buffer. If the value is 0 already, it stays at 0.
                Invert, //  Negate all the bits.
                IncrWrap, //    Increment the current value in the buffer. If the value is 255 already, it becomes 0.
                DecrWrap, //    Decrement the current value in the buffer. If the value is 0 already, it becomes 255.
            }

            public enum _StencilCompFunction
            {

                Disabled,//    Depth or stencil test is disabled.
                Never,   //   Never pass depth or stencil test.
                Less,   //   Pass depth or stencil test when new value is less than old one.
                Equal,  //  Pass depth or stencil test when values are equal.
                LessEqual, // Pass depth or stencil test when new value is less or equal than old one.
                Greater, // Pass depth or stencil test when new value is greater than old one.
                NotEqual, //    Pass depth or stencil test when values are different.
                GreaterEqual, // Pass depth or stencil test when new value is greater or equal than old one.
                Always,//  Always pass depth or stencil test.
            }

            public enum _OutlineMode
            {
                NormalDirection, PositionScaling
            }

            public enum _CullingMode
            {
                CullingOff, FrontCulling, BackCulling
            }

            public enum _EmissiveMode
            {
                SimpleEmissive, EmissiveAnimation
            }
            const string ShaderDefineSHADINGGRADEMAP = "_SHADINGGRADEMAP";
            const string ShaderDefineANGELRING_ON = "_IS_ANGELRING_ON";
            const string ShaderDefineANGELRING_OFF = "_IS_ANGELRING_OFF";
            const string ShaderDefineUTS_USE_RAYTRACING_SHADOW = "UTS_USE_RAYTRACING_SHADOW";
            const string ShaderPropAngelRing = "_AngelRing";
            const string ShaderPropRTHS = "_RTHS";
            const string ShaderPropMatCap = "_MatCap";
            const string ShaderPropClippingMode = "_ClippingMode";
            const string ShaderPropClippingMask = "_ClippingMask";
            const string ShaderPropSimpleUI = "_simpleUI";
            const string ShaderPropUtsTechniqe = "_utsTechnique";
            const string ShaderPropAutoRenderQueue = "_AutoRenderQueue";
            const string ShaderPropStencilMode = "_StencilMode";
            const string ShaderPropStencilNo = "_StencilNo";
            const string ShaderPropTransparentEnabled = "_TransparentEnabled";
            const string ShaderPropStencilComp = "_StencilComp";
            const string ShaderPropStencilOpPass = "_StencilOpPass";
            const string ShaderPropStencilOpFail = "_StencilOpFail";

            const string ShaderDefineIS_OUTLINE_CLIPPING_NO = "_IS_OUTLINE_CLIPPING_NO";
            const string ShaderDefineIS_OUTLINE_CLIPPING_YES = "_IS_OUTLINE_CLIPPING_YES";

            const string ShaderDefineIS_CLIPPING_OFF = "_IS_CLIPPING_OFF";
            const string ShaderDefineIS_CLIPPING_MODE = "_IS_CLIPPING_MODE";
            const string ShaderDefineIS_CLIPPING_TRANSMODE = "_IS_CLIPPING_TRANSMODE";

            const string ShaderDefineIS_TRANSCLIPPING_OFF = "_IS_TRANSCLIPPING_OFF";
            const string ShaderDefineIS_TRANSCLIPPING_ON = "_IS_TRANSCLIPPING_ON";
            public static void ApplyQueueAndRenderType(Material material)
            {
                _UTS_Transparent _Transparent_Setting = (_UTS_Transparent)material.GetInt(ShaderPropTransparentEnabled);

                const string OPAQUE = "Opaque";
                const string TRANSPARENTCUTOUT = "TransparentCutOut";
                const string TRANSPARENT = "Transparent";
                const string RENDERTYPE = "RenderType";
                const string IGNOREPROJECTION = "IgnoreProjection";
                const string DO_IGNOREPROJECTION = "True";
                const string DONT_IGNOREPROJECTION = "False";
                var renderType = OPAQUE;
                var ignoreProjection = DONT_IGNOREPROJECTION;
                _UTS_Technique technique = (_UTS_Technique)material.GetInt(ShaderPropUtsTechniqe);
                switch (technique)
                {
                    case _UTS_Technique.DoubleShadeWithFeather:
                        material.DisableKeyword(ShaderDefineSHADINGGRADEMAP);
                        break;
                    case _UTS_Technique.ShadingGradeMap:
                        material.EnableKeyword(ShaderDefineSHADINGGRADEMAP);
                        break;
                }
                if (_Transparent_Setting == _UTS_Transparent.On)
                {
                    renderType = TRANSPARENT;
                    ignoreProjection = DO_IGNOREPROJECTION;
                }
                else
                {
                    switch (technique)
                    {
                        case _UTS_Technique.DoubleShadeWithFeather:
                            {
                                _UTS_ClippingMode clippingMode = (_UTS_ClippingMode)material.GetInt(ShaderPropClippingMode);
                                if (clippingMode == _UTS_ClippingMode.Off)
                                {

                                }
                                else
                                {
                                    renderType = TRANSPARENTCUTOUT;

                                }

                                break;
                            }
                        case _UTS_Technique.ShadingGradeMap:
                            {
                                _UTS_TransClippingMode transClippingMode = (_UTS_TransClippingMode)material.GetInt(ShaderPropClippingMode);
                                if (transClippingMode == _UTS_TransClippingMode.Off)
                                {
                                }
                                else
                                {
                                    renderType = TRANSPARENTCUTOUT;

                                }

                                break;
                            }
                    }
                }
                material.SetOverrideTag(RENDERTYPE, renderType);
                material.SetOverrideTag(IGNOREPROJECTION, ignoreProjection);
            }
            public static void ApplyMatCapMode(Material material)
            {
                if (material.GetInt(ShaderPropClippingMode) == 0)
                {
                    if (material.GetFloat(ShaderPropMatCap) == 1)
                        material.EnableKeyword(ShaderPropMatCap);
                    else
                        material.DisableKeyword(ShaderPropMatCap);
                }
                else
                {
                    material.DisableKeyword(ShaderPropMatCap);
                }
            }
            public static void ApplyAngelRing(Material material)
            {
                int angelRingEnabled = material.GetInt(ShaderPropAngelRing);
                if (angelRingEnabled == 0)
                {
                    material.DisableKeyword(ShaderDefineANGELRING_ON);
                    material.EnableKeyword(ShaderDefineANGELRING_OFF);
                }
                else
                {
                    material.EnableKeyword(ShaderDefineANGELRING_ON);
                    material.DisableKeyword(ShaderDefineANGELRING_OFF);

                }
            }

            public static void ApplyStencilMode(Material material)
            {
                _UTS_StencilMode mode = (_UTS_StencilMode)(material.GetInt(ShaderPropStencilMode));
                switch (mode)
                {
                    case _UTS_StencilMode.Off:
                        //    material.SetInt(ShaderPropStencilNo,0);
                        material.SetInt(ShaderPropStencilComp, (int)_StencilCompFunction.Disabled);
                        material.SetInt(ShaderPropStencilOpPass, (int)_StencilOperation.Keep);
                        material.SetInt(ShaderPropStencilOpFail, (int)_StencilOperation.Keep);
                        break;
                    case _UTS_StencilMode.StencilMask:
                        //    material.SetInt(ShaderPropStencilNo,0);
                        material.SetInt(ShaderPropStencilComp, (int)_StencilCompFunction.Always);
                        material.SetInt(ShaderPropStencilOpPass, (int)_StencilOperation.Replace);
                        material.SetInt(ShaderPropStencilOpFail, (int)_StencilOperation.Replace);
                        break;
                    case _UTS_StencilMode.StencilOut:
                        //    material.SetInt(ShaderPropStencilNo,0);
                        material.SetInt(ShaderPropStencilComp, (int)_StencilCompFunction.NotEqual);
                        material.SetInt(ShaderPropStencilOpPass, (int)_StencilOperation.Keep);
                        material.SetInt(ShaderPropStencilOpFail, (int)_StencilOperation.Keep);

                        break;
                }
            }
            public static void ApplyClippingMode(Material material)
            {
                bool IsShadingGrademap = material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.ShadingGradeMap;
                if (!IsShadingGrademap)
                {
                    material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_OFF);
                    material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_ON);

                    switch (material.GetInt(ShaderPropClippingMode))
                    {
                        case 0:
                            material.EnableKeyword(ShaderDefineIS_CLIPPING_OFF);
                            material.DisableKeyword(ShaderDefineIS_CLIPPING_MODE);
                            material.DisableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
                            material.EnableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_NO);
                            material.DisableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_YES);
                            break;
                        case 1:
                            material.DisableKeyword(ShaderDefineIS_CLIPPING_OFF);
                            material.EnableKeyword(ShaderDefineIS_CLIPPING_MODE);
                            material.DisableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
                            material.DisableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_NO);
                            material.EnableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_YES);
                            break;
                        default:
                            material.DisableKeyword(ShaderDefineIS_CLIPPING_OFF);
                            material.DisableKeyword(ShaderDefineIS_CLIPPING_MODE);
                            material.EnableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
                            material.DisableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_NO);
                            material.EnableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_YES);
                            break;
                    }
                }
                else
                {
                    material.DisableKeyword(ShaderDefineIS_CLIPPING_OFF);
                    material.DisableKeyword(ShaderDefineIS_CLIPPING_MODE);
                    material.DisableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
                    switch (material.GetInt(ShaderPropClippingMode))
                    {
                        case 0:
                            material.EnableKeyword(ShaderDefineIS_TRANSCLIPPING_OFF);
                            material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_ON);
                            break;
                        default:
                            material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_OFF);
                            material.EnableKeyword(ShaderDefineIS_TRANSCLIPPING_ON);
                            break;

                    }
                }
            }

            const string srpDefaultLightModeName = "SRPDefaultUnlit";
            const string ShaderPropOutline = "_OUTLINE";
            public static void EnableOutline(Material material, bool isOutlineEnabled)
            {
                var srpDefaultLightModeTag = material.GetTag("LightMode", false, srpDefaultLightModeName);

                material.SetShaderPassEnabled(srpDefaultLightModeName, isOutlineEnabled);
                if (!isOutlineEnabled)
                    return;

                int _OutlineMode_Setting = material.GetInt(ShaderPropOutline);
                if ((int)_OutlineMode.NormalDirection == _OutlineMode_Setting)
                {
                    material.SetFloat(ShaderPropOutline, 0);
                    material.EnableKeyword("_OUTLINE_NML");
                    material.DisableKeyword("_OUTLINE_POS");
                }
                else if ((int)_OutlineMode.PositionScaling == _OutlineMode_Setting)
                {
                    material.SetFloat(ShaderPropOutline, 1);
                    material.EnableKeyword("_OUTLINE_POS");
                    material.DisableKeyword("_OUTLINE_NML");
                }
            }
        }
        #endregion

        #region MToon
        public static class MToon
        {
            public const string PropVersion = "_MToonVersion";
            public const string PropDebugMode = "_DebugMode";
            public const string PropOutlineWidthMode = "_OutlineWidthMode";
            public const string PropOutlineColorMode = "_OutlineColorMode";
            public const string PropBlendMode = "_BlendMode";
            public const string PropCullMode = "_CullMode";
            public const string PropOutlineCullMode = "_OutlineCullMode";
            public const string PropCutoff = "_Cutoff";
            public const string PropColor = "_Color";
            public const string PropShadeColor = "_ShadeColor";
            public const string PropMainTex = "_MainTex";
            public const string PropShadeTexture = "_ShadeTexture";
            public const string PropBumpScale = "_BumpScale";
            public const string PropBumpMap = "_BumpMap";
            public const string PropReceiveShadowRate = "_ReceiveShadowRate";
            public const string PropReceiveShadowTexture = "_ReceiveShadowTexture";
            public const string PropShadingGradeRate = "_ShadingGradeRate";
            public const string PropShadingGradeTexture = "_ShadingGradeTexture";
            public const string PropShadeShift = "_ShadeShift";
            public const string PropShadeToony = "_ShadeToony";
            public const string PropLightColorAttenuation = "_LightColorAttenuation";
            public const string PropIndirectLightIntensity = "_IndirectLightIntensity";
            public const string PropRimColor = "_RimColor";
            public const string PropRimTexture = "_RimTexture";
            public const string PropRimLightingMix = "_RimLightingMix";
            public const string PropRimFresnelPower = "_RimFresnelPower";
            public const string PropRimLift = "_RimLift";
            public const string PropSphereAdd = "_SphereAdd";
            public const string PropEmissionColor = "_EmissionColor";
            public const string PropEmissionMap = "_EmissionMap";
            public const string PropOutlineWidthTexture = "_OutlineWidthTexture";
            public const string PropOutlineWidth = "_OutlineWidth";
            public const string PropOutlineScaledMaxDistance = "_OutlineScaledMaxDistance";
            public const string PropOutlineColor = "_OutlineColor";
            public const string PropOutlineLightingMix = "_OutlineLightingMix";
            public const string PropUvAnimMaskTexture = "_UvAnimMaskTexture";
            public const string PropUvAnimScrollX = "_UvAnimScrollX";
            public const string PropUvAnimScrollY = "_UvAnimScrollY";
            public const string PropUvAnimRotation = "_UvAnimRotation";
            public const string PropSrcBlend = "_SrcBlend";
            public const string PropDstBlend = "_DstBlend";
            public const string PropZWrite = "_ZWrite";
            public const string PropAlphaToMask = "_AlphaToMask";

            public const string KeyNormalMap = "_NORMALMAP";
            public const string KeyAlphaTestOn = "_ALPHATEST_ON";
            public const string KeyAlphaBlendOn = "_ALPHABLEND_ON";
            public const string KeyAlphaPremultiplyOn = "_ALPHAPREMULTIPLY_ON";
            public const string KeyOutlineWidthWorld = "MTOON_OUTLINE_WIDTH_WORLD";
            public const string KeyOutlineWidthScreen = "MTOON_OUTLINE_WIDTH_SCREEN";
            public const string KeyOutlineColorFixed = "MTOON_OUTLINE_COLOR_FIXED";
            public const string KeyOutlineColorMixed = "MTOON_OUTLINE_COLOR_MIXED";
            public const string KeyDebugNormal = "MTOON_DEBUG_NORMAL";
            public const string KeyDebugLitShadeRate = "MTOON_DEBUG_LITSHADERATE";

            public const string TagRenderTypeKey = "RenderType";
            public const string TagRenderTypeValueOpaque = "Opaque";
            public const string TagRenderTypeValueTransparentCutout = "TransparentCutout";
            public const string TagRenderTypeValueTransparent = "Transparent";

            public const int DisabledIntValue = 0;
            public const int EnabledIntValue = 1;
            public enum DebugMode
            {
                None = 0,
                Normal = 1,
                LitShadeRate = 2,
            }

            public enum OutlineColorMode
            {
                FixedColor = 0,
                MixedLighting = 1,
            }

            public enum OutlineWidthMode
            {
                None = 0,
                WorldCoordinates = 1,
                ScreenCoordinates = 2,
            }

            public enum RenderMode
            {
                Opaque = 0,
                Cutout = 1,
                Transparent = 2,
                TransparentWithZWrite = 3,
            }

            public enum CullMode
            {
                Off = 0,
                Front = 1,
                Back = 2,
            }

            public struct RenderQueueRequirement
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
            public static RenderQueueRequirement GetRenderQueueRequirement(RenderMode renderMode)
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
        public static async Task<Material> ImportMaterial(string shaderName, GLTFMaterial materialDef, GLTFRoot root, JsonReader reader, AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
        {
            if (!CustomShaders.ContainsKey(shaderName)) return null;

            //if (!CUSTOM_SHADER_LIST.ContainsKey(shaderName)){ return null;  }

            Shader shader = Shader.Find(shaderName);

            if (shader == null)
                throw new ShaderNotFoundException(shaderName + " not found. Did you forget to add it to the build?");

            Material matCache = new Material(shader);
            matCache.name = materialDef.Name;

            //await CUSTOM_SHADER_LIST[shaderName].Item2(root, reader, matCache, loadTexture, loadNormalMap, loadCubemap);
            await CustomShaders[shaderName].Deserialize(root, reader, matCache, loadTexture, loadNormalMap, loadCubemap);

            SetCommonMaterialKeywords(matCache);

            /*if (shaderName == BVA_Material_ToonLit_Extra.SHADER_NAME)
            {
                ToonLit.SetupMaterialBlendMode(matCache);
                ToonLit.SetMaterialKeywords(matCache);
            }
            else if (shaderName == BVA_Material_UTS_Extra.SHADER_NAME)
            {
                UTS.EnableOutline(matCache, true);
                UTS.ApplyAngelRing(matCache);
                UTS.ApplyClippingMode(matCache);
                UTS.ApplyMatCapMode(matCache);
                UTS.ApplyQueueAndRenderType(matCache);
                UTS.ApplyStencilMode(matCache);
            }
            else */if (shaderName == BVA_Material_MToon_Extra.SHADER_NAME)
            {
                MToon.ValidateProperties(matCache, false);
            }
            return matCache;
        }
    }
}