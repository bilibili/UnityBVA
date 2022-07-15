using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BVA;
using UnityEngine.Rendering;

namespace GLTF.Schema.BVA
{
    public enum SurfaceType
    {
        Opaque,
        Transparent
    }

    public enum BlendMode
    {
        Alpha,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
        Premultiply, // Physically plausible transparency mode, implemented as alpha pre-multiply
        Additive,
        Multiply
    }

    public enum RoughnessSource
    {
        BaseAlpha,
        SpecularAlpha
    }

    public enum RenderFace
    {
        Front = 2,
        Back = 1,
        Both = 0
    }
    public abstract class MaterialDescriptor : IExtra
    {
        public abstract JProperty Serialize();
    }
    public static class MaterialFactory
    {
        private static bool IsCustomShader(Material material)
        {
            return CUSTOM_SHADER_LIST.ContainsKey(material.shader.name);
        }

        private static readonly Dictionary<string, AsyncDeserializeCustomMaterial> CUSTOM_SHADER_LIST = new Dictionary<string, AsyncDeserializeCustomMaterial>
        {
            { BVA_Material_ClothLED_Extra.SHADER_NAME,BVA_Material_ClothLED_Extra.Deserialize },
            { BVA_Material_UTS_Extra.SHADER_NAME,BVA_Material_UTS_Extra.Deserialize },
            { BVA_Material_MToon_Extra.SHADER_NAME,BVA_Material_MToon_Extra.Deserialize },
            { BVA_Material_ToonLit_Extra.SHADER_NAME,BVA_Material_ToonLit_Extra.Deserialize },
            { BVA_Material_ToonSimple_Extra.SHADER_NAME,BVA_Material_ToonSimple_Extra.Deserialize },
            { BVA_Material_ToonGGX_Extra.SHADER_NAME,BVA_Material_ToonGGX_Extra.Deserialize },
            { BVA_Material_ToonDisolve_Extra.SHADER_NAME,BVA_Material_ToonDisolve_Extra.Deserialize },
            { BVA_Material_ToonHair_Extra.SHADER_NAME,BVA_Material_ToonHair_Extra.Deserialize },
            { BVA_Material_ToonRamp_Extra.SHADER_NAME,BVA_Material_ToonRamp_Extra.Deserialize },
            { BVA_Material_ToonStylized_Extra.SHADER_NAME,BVA_Material_ToonStylized_Extra.Deserialize },
            { BVA_Material_ToonTransparent_Extra.SHADER_NAME,BVA_Material_ToonTransparent_Extra.Deserialize },
            { BVA_Material_ToonTransparentCutout_Extra.SHADER_NAME,BVA_Material_ToonTransparentCutout_Extra.Deserialize },
            { BVA_Material_Toon_Extra.SHADER_NAME,BVA_Material_Toon_Extra.Deserialize },
            { BVA_Material_ZeldaToon_Extra.SHADER_NAME,BVA_Material_ZeldaToon_Extra.Deserialize },
            { BVA_Material_SkyBox6Sided_Extra.SHADER_NAME,BVA_Material_SkyBox6Sided_Extra.Deserialize },
            { BVA_Material_SkyboxCubemap_Extra.SHADER_NAME,BVA_Material_SkyboxCubemap_Extra.Deserialize },
            { BVA_Material_Decal_Extra.SHADER_NAME,BVA_Material_Decal_Extra.Deserialize },
            { BVA_Material_UnlitTransparentZwrite_Extra.SHADER_NAME,BVA_Material_UnlitTransparentZwrite_Extra.Deserialize },
            { BVA_Material_MirrorFloor_Extra.SHADER_NAME,BVA_Material_MirrorFloor_Extra.Deserialize },
            { BVA_Material_MirrorPlane_Extra.SHADER_NAME,BVA_Material_MirrorPlane_Extra.Deserialize },
        };

        /// <summary>
        /// for export material extra
        /// </summary>
        /// <param name="materialObj"></param>
        /// <param name="material"></param>
        /// <param name="exportTextureInfo"></param>
        /// <returns></returns>
        public static bool ExportMaterialExtra(Material materialObj, GLTFMaterial material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
        {
            string shader = materialObj.shader.name;
            if (shader == BVA_Material_ClothLED_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ClothLED_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ClothLED_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_UTS_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_UTS_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_UTS_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_MToon_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_MToon_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_MToon_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ToonLit_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ToonLit_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ToonLit_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ToonSimple_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ToonSimple_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ToonSimple_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_Toon_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_Toon_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_Toon_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ToonGGX_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ToonGGX_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ToonGGX_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ToonHair_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ToonHair_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ToonHair_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ToonRamp_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ToonRamp_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ToonRamp_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ToonStylized_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ToonStylized_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ToonStylized_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ToonTransparent_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ToonTransparent_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ToonTransparent_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ToonTransparentCutout_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ToonTransparentCutout_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ToonTransparentCutout_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_ZeldaToon_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_ZeldaToon_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_ZeldaToon_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_SkyBox6Sided_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_SkyBox6Sided_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_SkyBox6Sided_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_SkyboxCubemap_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_SkyboxCubemap_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_SkyboxCubemap_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_Decal_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_Decal_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_Decal_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_UnlitTransparentZwrite_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_UnlitTransparentZwrite_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_UnlitTransparentZwrite_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_MirrorFloor_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_MirrorFloor_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_MirrorFloor_Extra.PROPERTY, extra);
                return true;
            }
            if (shader == BVA_Material_MirrorPlane_Extra.SHADER_NAME)
            {
                var extra = new BVA_Material_MirrorPlane_Extra(materialObj, exportTextureInfo, exportNormalMapInfo, exportCubemapInfo);
                material.AddExtra(BVA_Material_MirrorPlane_Extra.PROPERTY, extra);
                return true;
            }
            return false;
        }
        #region ToonLit
        public static void SetupMaterialBlendMode(Material material)
        {
            if (material == null)
                throw new System.ArgumentNullException("material");

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
        public static void SetMaterialKeywords(Material material)
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
        }
        #endregion
        #region UTS Toon
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
        static void ApplyQueueAndRenderType(Material material)
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
        static void ApplyMatCapMode(Material material)
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



        static void ApplyAngelRing(Material material)
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

        static void ApplyStencilMode(Material material)
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
        static void ApplyClippingMode(Material material)
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
        static void EnableOutline(Material material, bool isOutlineEnabled)
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
        #endregion
        public static async Task<Material> ImportMaterial(string shaderName, GLTFMaterial materialDef, GLTFRoot root, JsonReader reader, AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
        {
            if (!CUSTOM_SHADER_LIST.ContainsKey(shaderName))
            {
                return null;
            }
            Shader shader = Shader.Find(shaderName);

            if (shader == null)
                throw new ShaderNotFoundException(shaderName + " not found. Did you forget to add it to the build?");

            Material matCache = new Material(shader);
            matCache.name = materialDef.Name;

            await CUSTOM_SHADER_LIST[shaderName](root, reader, matCache, loadTexture, loadNormalMap, loadCubemap);
            if (shaderName == BVA_Material_ToonLit_Extra.SHADER_NAME)
            {
                SetupMaterialBlendMode(matCache);
                SetMaterialKeywords(matCache);
            }
            else if (shaderName == BVA_Material_UTS_Extra.SHADER_NAME)
            {
                EnableOutline(matCache, true);
                ApplyAngelRing(matCache);
                ApplyClippingMode(matCache);
                ApplyMatCapMode(matCache);
                ApplyQueueAndRenderType(matCache);
                ApplyStencilMode(matCache);
            }
            return matCache;
        }
    }
}