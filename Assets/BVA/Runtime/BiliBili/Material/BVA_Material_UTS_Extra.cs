using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector4 = UnityEngine.Vector4;

namespace GLTF.Schema.BVA
{
    [MaterialExtra]
    public class BVA_Material_UTS_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_UTS_Extra";
        public const string SHADER_NAME = "Universal Render Pipeline/Toon";
        public const string SIMPLEUI = "_simpleUI";
        public const string UTSVERSIONX = "_utsVersionX";
        public const string UTSVERSIONY = "_utsVersionY";
        public const string UTSVERSIONZ = "_utsVersionZ";
        public const string UTSTECHNIQUE = "_utsTechnique";
        public const string AUTORENDERQUEUE = "_AutoRenderQueue";
        public const string STENCILMODE = "_StencilMode";
        public const string STENCILCOMP = "_StencilComp";
        public const string STENCILNO = "_StencilNo";
        public const string STENCILOPPASS = "_StencilOpPass";
        public const string STENCILOPFAIL = "_StencilOpFail";
        public const string TRANSPARENTENABLED = "_TransparentEnabled";
        public const string METALLIC = "_Metallic";
        public const string SMOOTHNESS = "_Smoothness";
        public const string SPECCOLOR = "_SpecColor";
        public const string CLIPPINGMODE = "_ClippingMode";
        public const string CULLMODE = "_CullMode";
        public const string ZWRITEMODE = "_ZWriteMode";
        public const string ZOVERDRAWMODE = "_ZOverDrawMode";
        public const string SPRDEFAULTUNLITCOLORMASK = "_SPRDefaultUnlitColorMask";
        public const string SRPDEFAULTUNLITCOLMODE = "_SRPDefaultUnlitColMode";
        public const string CLIPPINGMASK = "_ClippingMask";
        public const string ISBASEMAPALPHAASCLIPPINGMASK = "_IsBaseMapAlphaAsClippingMask";
        public const string INVERSECLIPPING = "_Inverse_Clipping";
        public const string CLIPPINGLEVEL = "_Clipping_Level";
        public const string TWEAKTRANSPARENCY = "_Tweak_transparency";
        public const string MAINTEX = "_MainTex";
        public const string BASECOLOR = "_BaseColor";
        public const string COLOR = "_Color";
        public const string ISLIGHTCOLORBASE = "_Is_LightColor_Base";
        public const string FIRSTSHADEMAP = "_1st_ShadeMap";
        public const string USEBASEASFIRST = "_Use_BaseAs1st";
        public const string FIRSTSHADECOLOR = "_1st_ShadeColor";
        public const string ISLIGHTCOLORFIRSTSHADE = "_Is_LightColor_1st_Shade";
        public const string SECONDSHADEMAP = "_2nd_ShadeMap";
        public const string USEFIRSTASSECOND = "_Use_1stAs2nd";
        public const string SECONDSHADECOLOR = "_2nd_ShadeColor";
        public const string ISLIGHTCOLORSECONDSHADE = "_Is_LightColor_2nd_Shade";
        public const string NORMALMAP = "_NormalMap";
        public const string BUMPSCALE = "_BumpScale";
        public const string ISNORMALMAPTOBASE = "_Is_NormalMapToBase";
        public const string SETSYSTEMSHADOWSTOBASE = "_Set_SystemShadowsToBase";
        public const string TWEAKSYSTEMSHADOWSLEVEL = "_Tweak_SystemShadowsLevel";
        public const string BASECOLORSTEP = "_BaseColor_Step";
        public const string BASESHADEFEATHER = "_BaseShade_Feather";
        public const string SHADECOLORSTEP = "_ShadeColor_Step";
        public const string FIRSTSECONDSHADESFEATHER = "_1st2nd_Shades_Feather";
        public const string FIRSTSHADECOLORSTEP = "_1st_ShadeColor_Step";
        public const string FIRSTSHADECOLORFEATHER = "_1st_ShadeColor_Feather";
        public const string SECONDSHADECOLORSTEP = "_2nd_ShadeColor_Step";
        public const string SECONDSHADECOLORFEATHER = "_2nd_ShadeColor_Feather";
        public const string STEPOFFSET = "_StepOffset";
        public const string ISFILTERHICUTPOINTLIGHTCOLOR = "_Is_Filter_HiCutPointLightColor";
        public const string SETFIRSTSHADEPOSITION = "_Set_1st_ShadePosition";
        public const string SETSECONDSHADEPOSITION = "_Set_2nd_ShadePosition";
        public const string SHADINGGRADEMAP = "_ShadingGradeMap";
        public const string TWEAKSHADINGGRADEMAPLEVEL = "_Tweak_ShadingGradeMapLevel";
        public const string BLURLEVELSGM = "_BlurLevelSGM";
        public const string HIGHCOLOR = "_HighColor";
        public const string HIGHCOLORTEX = "_HighColor_Tex";
        public const string ISLIGHTCOLORHIGHCOLOR = "_Is_LightColor_HighColor";
        public const string ISNORMALMAPTOHIGHCOLOR = "_Is_NormalMapToHighColor";
        public const string HIGHCOLORPOWER = "_HighColor_Power";
        public const string ISSPECULARTOHIGHCOLOR = "_Is_SpecularToHighColor";
        public const string ISBLENDADDTOHICOLOR = "_Is_BlendAddToHiColor";
        public const string ISUSETWEAKHIGHCOLORONSHADOW = "_Is_UseTweakHighColorOnShadow";
        public const string TWEAKHIGHCOLORONSHADOW = "_TweakHighColorOnShadow";
        public const string SETHIGHCOLORMASK = "_Set_HighColorMask";
        public const string TWEAKHIGHCOLORMASKLEVEL = "_Tweak_HighColorMaskLevel";
        public const string RIMLIGHT = "_RimLight";
        public const string RIMLIGHTCOLOR = "_RimLightColor";
        public const string ISLIGHTCOLORRIMLIGHT = "_Is_LightColor_RimLight";
        public const string ISNORMALMAPTORIMLIGHT = "_Is_NormalMapToRimLight";
        public const string RIMLIGHTPOWER = "_RimLight_Power";
        public const string RIMLIGHTINSIDEMASK = "_RimLight_InsideMask";
        public const string RIMLIGHTFEATHEROFF = "_RimLight_FeatherOff";
        public const string LIGHTDIRECTIONMASKON = "_LightDirection_MaskOn";
        public const string TWEAKLIGHTDIRECTIONMASKLEVEL = "_Tweak_LightDirection_MaskLevel";
        public const string ADDANTIPODEANRIMLIGHT = "_Add_Antipodean_RimLight";
        public const string APRIMLIGHTCOLOR = "_Ap_RimLightColor";
        public const string ISLIGHTCOLORAPRIMLIGHT = "_Is_LightColor_Ap_RimLight";
        public const string APRIMLIGHTPOWER = "_Ap_RimLight_Power";
        public const string APRIMLIGHTFEATHEROFF = "_Ap_RimLight_FeatherOff";
        public const string SETRIMLIGHTMASK = "_Set_RimLightMask";
        public const string TWEAKRIMLIGHTMASKLEVEL = "_Tweak_RimLightMaskLevel";
        public const string MATCAP = "_MatCap";
        public const string MATCAPSAMPLER = "_MatCap_Sampler";
        public const string BLURLEVELMATCAP = "_BlurLevelMatcap";
        public const string MATCAPCOLOR = "_MatCapColor";
        public const string ISLIGHTCOLORMATCAP = "_Is_LightColor_MatCap";
        public const string ISBLENDADDTOMATCAP = "_Is_BlendAddToMatCap";
        public const string TWEAKMATCAPUV = "_Tweak_MatCapUV";
        public const string ROTATEMATCAPUV = "_Rotate_MatCapUV";
        public const string CAMERAROLLINGSTABILIZER = "_CameraRolling_Stabilizer";
        public const string ISNORMALMAPFORMATCAP = "_Is_NormalMapForMatCap";
        public const string NORMALMAPFORMATCAP = "_NormalMapForMatCap";
        public const string BUMPSCALEMATCAP = "_BumpScaleMatcap";
        public const string ROTATENORMALMAPFORMATCAPUV = "_Rotate_NormalMapForMatCapUV";
        public const string ISUSETWEAKMATCAPONSHADOW = "_Is_UseTweakMatCapOnShadow";
        public const string TWEAKMATCAPONSHADOW = "_TweakMatCapOnShadow";
        public const string SETMATCAPMASK = "_Set_MatcapMask";
        public const string TWEAKMATCAPMASKLEVEL = "_Tweak_MatcapMaskLevel";
        public const string INVERSEMATCAPMASK = "_Inverse_MatcapMask";
        public const string ISORTHO = "_Is_Ortho";
        public const string ANGELRING = "_AngelRing";
        public const string ANGELRINGSAMPLER = "_AngelRing_Sampler";
        public const string ANGELRINGCOLOR = "_AngelRing_Color";
        public const string ISLIGHTCOLORAR = "_Is_LightColor_AR";
        public const string AROFFSETU = "_AR_OffsetU";
        public const string AROFFSETV = "_AR_OffsetV";
        public const string ARSAMPLERALPHAON = "_ARSampler_AlphaOn";
        public const string EMISSIVE = "_EMISSIVE";
        public const string EMISSIVETEX = "_Emissive_Tex";
        public const string EMISSIVECOLOR = "_Emissive_Color";
        public const string BASESPEED = "_Base_Speed";
        public const string SCROLLEMISSIVEU = "_Scroll_EmissiveU";
        public const string SCROLLEMISSIVEV = "_Scroll_EmissiveV";
        public const string ROTATEEMISSIVEUV = "_Rotate_EmissiveUV";
        public const string ISPINGPONGBASE = "_Is_PingPong_Base";
        public const string ISCOLORSHIFT = "_Is_ColorShift";
        public const string COLORSHIFT = "_ColorShift";
        public const string COLORSHIFTSPEED = "_ColorShift_Speed";
        public const string ISVIEWSHIFT = "_Is_ViewShift";
        public const string VIEWSHIFT = "_ViewShift";
        public const string ISVIEWCOORDSCROLL = "_Is_ViewCoord_Scroll";
        public const string OUTLINE = "_OUTLINE";
        public const string OUTLINEWIDTH = "_Outline_Width";
        public const string FARTHESTDISTANCE = "_Farthest_Distance";
        public const string NEARESTDISTANCE = "_Nearest_Distance";
        public const string OUTLINESAMPLER = "_Outline_Sampler";
        public const string OUTLINECOLOR = "_Outline_Color";
        public const string ISBLENDBASECOLOR = "_Is_BlendBaseColor";
        public const string ISLIGHTCOLOROUTLINE = "_Is_LightColor_Outline";
        public const string CUTOFF = "_Cutoff";
        public const string ISOUTLINETEX = "_Is_OutlineTex";
        public const string OUTLINETEX = "_OutlineTex";
        public const string OFFSETZ = "_Offset_Z";
        public const string ISBAKEDNORMAL = "_Is_BakedNormal";
        public const string BAKEDNORMAL = "_BakedNormal";
        public const string GIINTENSITY = "_GI_Intensity";
        public const string UNLITINTENSITY = "_Unlit_Intensity";
        public const string ISFILTERLIGHTCOLOR = "_Is_Filter_LightColor";
        public const string ISBLD = "_Is_BLD";
        public const string OFFSETXAXISBLD = "_Offset_X_Axis_BLD";
        public const string OFFSETYAXISBLD = "_Offset_Y_Axis_BLD";
        public const string INVERSEZAXISBLD = "_Inverse_Z_Axis_BLD";
        public MaterialParam<float> parameter__simpleUI = new MaterialParam<float>(SIMPLEUI, 1.0f);
        public MaterialParam<float> parameter__utsVersionX = new MaterialParam<float>(UTSVERSIONX, 1.0f);
        public MaterialParam<float> parameter__utsVersionY = new MaterialParam<float>(UTSVERSIONY, 1.0f);
        public MaterialParam<float> parameter__utsVersionZ = new MaterialParam<float>(UTSVERSIONZ, 1.0f);
        public MaterialParam<float> parameter__utsTechnique = new MaterialParam<float>(UTSTECHNIQUE, 1.0f);
        public MaterialParam<float> parameter__AutoRenderQueue = new MaterialParam<float>(AUTORENDERQUEUE, 1.0f);
        public MaterialParam<float> parameter__StencilMode = new MaterialParam<float>(STENCILMODE, 1.0f);
        public MaterialParam<float> parameter__StencilComp = new MaterialParam<float>(STENCILCOMP, 1.0f);
        public MaterialParam<float> parameter__StencilNo = new MaterialParam<float>(STENCILNO, 1.0f);
        public MaterialParam<float> parameter__StencilOpPass = new MaterialParam<float>(STENCILOPPASS, 1.0f);
        public MaterialParam<float> parameter__StencilOpFail = new MaterialParam<float>(STENCILOPFAIL, 1.0f);
        public MaterialParam<float> parameter__TransparentEnabled = new MaterialParam<float>(TRANSPARENTENABLED, 1.0f);
        public MaterialParam<float> parameter__Metallic = new MaterialParam<float>(METALLIC, 1.0f);
        public MaterialParam<float> parameter__Smoothness = new MaterialParam<float>(SMOOTHNESS, 1.0f);
        public MaterialParam<Color> parameter__SpecColor = new MaterialParam<Color>(SPECCOLOR, Color.white);
        public MaterialParam<float> parameter__ClippingMode = new MaterialParam<float>(CLIPPINGMODE, 1.0f);
        public MaterialParam<float> parameter__CullMode = new MaterialParam<float>(CULLMODE, 1.0f);
        public MaterialParam<float> parameter__ZWriteMode = new MaterialParam<float>(ZWRITEMODE, 1.0f);
        public MaterialParam<float> parameter__ZOverDrawMode = new MaterialParam<float>(ZOVERDRAWMODE, 1.0f);
        public MaterialParam<float> parameter__SPRDefaultUnlitColorMask = new MaterialParam<float>(SPRDEFAULTUNLITCOLORMASK, 1.0f);
        public MaterialParam<float> parameter__SRPDefaultUnlitColMode = new MaterialParam<float>(SRPDEFAULTUNLITCOLMODE, 1.0f);
        public MaterialTextureParam parameter__ClippingMask = new MaterialTextureParam(CLIPPINGMASK);
        public MaterialParam<float> parameter__IsBaseMapAlphaAsClippingMask = new MaterialParam<float>(ISBASEMAPALPHAASCLIPPINGMASK, 1.0f);
        public MaterialParam<float> parameter__Inverse_Clipping = new MaterialParam<float>(INVERSECLIPPING, 1.0f);
        public MaterialParam<float> parameter__Clipping_Level = new MaterialParam<float>(CLIPPINGLEVEL, 1.0f);
        public MaterialParam<float> parameter__Tweak_transparency = new MaterialParam<float>(TWEAKTRANSPARENCY, 1.0f);
        public MaterialTextureParam parameter__MainTex = new MaterialTextureParam(MAINTEX);
        public MaterialParam<Color> parameter__BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
        public MaterialParam<Color> parameter__Color = new MaterialParam<Color>(COLOR, Color.white);
        public MaterialParam<float> parameter__Is_LightColor_Base = new MaterialParam<float>(ISLIGHTCOLORBASE, 1.0f);
        public MaterialTextureParam parameter__1st_ShadeMap = new MaterialTextureParam(FIRSTSHADEMAP);
        public MaterialParam<float> parameter__Use_BaseAs1st = new MaterialParam<float>(USEBASEASFIRST, 1.0f);
        public MaterialParam<Color> parameter__1st_ShadeColor = new MaterialParam<Color>(FIRSTSHADECOLOR, Color.white);
        public MaterialParam<float> parameter__Is_LightColor_1st_Shade = new MaterialParam<float>(ISLIGHTCOLORFIRSTSHADE, 1.0f);
        public MaterialTextureParam parameter__2nd_ShadeMap = new MaterialTextureParam(SECONDSHADEMAP);
        public MaterialParam<float> parameter__Use_1stAs2nd = new MaterialParam<float>(USEFIRSTASSECOND, 1.0f);
        public MaterialParam<Color> parameter__2nd_ShadeColor = new MaterialParam<Color>(SECONDSHADECOLOR, Color.white);
        public MaterialParam<float> parameter__Is_LightColor_2nd_Shade = new MaterialParam<float>(ISLIGHTCOLORSECONDSHADE, 1.0f);
        public MaterialTextureParam parameter__NormalMap = new MaterialTextureParam(NORMALMAP);
        public MaterialParam<float> parameter__BumpScale = new MaterialParam<float>(BUMPSCALE, 1.0f);
        public MaterialParam<float> parameter__Is_NormalMapToBase = new MaterialParam<float>(ISNORMALMAPTOBASE, 1.0f);
        public MaterialParam<float> parameter__Set_SystemShadowsToBase = new MaterialParam<float>(SETSYSTEMSHADOWSTOBASE, 1.0f);
        public MaterialParam<float> parameter__Tweak_SystemShadowsLevel = new MaterialParam<float>(TWEAKSYSTEMSHADOWSLEVEL, 1.0f);
        public MaterialParam<float> parameter__BaseColor_Step = new MaterialParam<float>(BASECOLORSTEP, 1.0f);
        public MaterialParam<float> parameter__BaseShade_Feather = new MaterialParam<float>(BASESHADEFEATHER, 1.0f);
        public MaterialParam<float> parameter__ShadeColor_Step = new MaterialParam<float>(SHADECOLORSTEP, 1.0f);
        public MaterialParam<float> parameter__1st2nd_Shades_Feather = new MaterialParam<float>(FIRSTSECONDSHADESFEATHER, 1.0f);
        public MaterialParam<float> parameter__1st_ShadeColor_Step = new MaterialParam<float>(FIRSTSHADECOLORSTEP, 1.0f);
        public MaterialParam<float> parameter__1st_ShadeColor_Feather = new MaterialParam<float>(FIRSTSHADECOLORFEATHER, 1.0f);
        public MaterialParam<float> parameter__2nd_ShadeColor_Step = new MaterialParam<float>(SECONDSHADECOLORSTEP, 1.0f);
        public MaterialParam<float> parameter__2nd_ShadeColor_Feather = new MaterialParam<float>(SECONDSHADECOLORFEATHER, 1.0f);
        public MaterialParam<float> parameter__StepOffset = new MaterialParam<float>(STEPOFFSET, 1.0f);
        public MaterialParam<float> parameter__Is_Filter_HiCutPointLightColor = new MaterialParam<float>(ISFILTERHICUTPOINTLIGHTCOLOR, 1.0f);
        public MaterialTextureParam parameter__Set_1st_ShadePosition = new MaterialTextureParam(SETFIRSTSHADEPOSITION);
        public MaterialTextureParam parameter__Set_2nd_ShadePosition = new MaterialTextureParam(SETSECONDSHADEPOSITION);
        public MaterialTextureParam parameter__ShadingGradeMap = new MaterialTextureParam(SHADINGGRADEMAP);
        public MaterialParam<float> parameter__Tweak_ShadingGradeMapLevel = new MaterialParam<float>(TWEAKSHADINGGRADEMAPLEVEL, 1.0f);
        public MaterialParam<float> parameter__BlurLevelSGM = new MaterialParam<float>(BLURLEVELSGM, 1.0f);
        public MaterialParam<Color> parameter__HighColor = new MaterialParam<Color>(HIGHCOLOR, Color.white);
        public MaterialTextureParam parameter__HighColor_Tex = new MaterialTextureParam(HIGHCOLORTEX);
        public MaterialParam<float> parameter__Is_LightColor_HighColor = new MaterialParam<float>(ISLIGHTCOLORHIGHCOLOR, 1.0f);
        public MaterialParam<float> parameter__Is_NormalMapToHighColor = new MaterialParam<float>(ISNORMALMAPTOHIGHCOLOR, 1.0f);
        public MaterialParam<float> parameter__HighColor_Power = new MaterialParam<float>(HIGHCOLORPOWER, 1.0f);
        public MaterialParam<float> parameter__Is_SpecularToHighColor = new MaterialParam<float>(ISSPECULARTOHIGHCOLOR, 1.0f);
        public MaterialParam<float> parameter__Is_BlendAddToHiColor = new MaterialParam<float>(ISBLENDADDTOHICOLOR, 1.0f);
        public MaterialParam<float> parameter__Is_UseTweakHighColorOnShadow = new MaterialParam<float>(ISUSETWEAKHIGHCOLORONSHADOW, 1.0f);
        public MaterialParam<float> parameter__TweakHighColorOnShadow = new MaterialParam<float>(TWEAKHIGHCOLORONSHADOW, 1.0f);
        public MaterialTextureParam parameter__Set_HighColorMask = new MaterialTextureParam(SETHIGHCOLORMASK);
        public MaterialParam<float> parameter__Tweak_HighColorMaskLevel = new MaterialParam<float>(TWEAKHIGHCOLORMASKLEVEL, 1.0f);
        public MaterialParam<float> parameter__RimLight = new MaterialParam<float>(RIMLIGHT, 1.0f);
        public MaterialParam<Color> parameter__RimLightColor = new MaterialParam<Color>(RIMLIGHTCOLOR, Color.white);
        public MaterialParam<float> parameter__Is_LightColor_RimLight = new MaterialParam<float>(ISLIGHTCOLORRIMLIGHT, 1.0f);
        public MaterialParam<float> parameter__Is_NormalMapToRimLight = new MaterialParam<float>(ISNORMALMAPTORIMLIGHT, 1.0f);
        public MaterialParam<float> parameter__RimLight_Power = new MaterialParam<float>(RIMLIGHTPOWER, 1.0f);
        public MaterialParam<float> parameter__RimLight_InsideMask = new MaterialParam<float>(RIMLIGHTINSIDEMASK, 1.0f);
        public MaterialParam<float> parameter__RimLight_FeatherOff = new MaterialParam<float>(RIMLIGHTFEATHEROFF, 1.0f);
        public MaterialParam<float> parameter__LightDirection_MaskOn = new MaterialParam<float>(LIGHTDIRECTIONMASKON, 1.0f);
        public MaterialParam<float> parameter__Tweak_LightDirection_MaskLevel = new MaterialParam<float>(TWEAKLIGHTDIRECTIONMASKLEVEL, 1.0f);
        public MaterialParam<float> parameter__Add_Antipodean_RimLight = new MaterialParam<float>(ADDANTIPODEANRIMLIGHT, 1.0f);
        public MaterialParam<Color> parameter__Ap_RimLightColor = new MaterialParam<Color>(APRIMLIGHTCOLOR, Color.white);
        public MaterialParam<float> parameter__Is_LightColor_Ap_RimLight = new MaterialParam<float>(ISLIGHTCOLORAPRIMLIGHT, 1.0f);
        public MaterialParam<float> parameter__Ap_RimLight_Power = new MaterialParam<float>(APRIMLIGHTPOWER, 1.0f);
        public MaterialParam<float> parameter__Ap_RimLight_FeatherOff = new MaterialParam<float>(APRIMLIGHTFEATHEROFF, 1.0f);
        public MaterialTextureParam parameter__Set_RimLightMask = new MaterialTextureParam(SETRIMLIGHTMASK);
        public MaterialParam<float> parameter__Tweak_RimLightMaskLevel = new MaterialParam<float>(TWEAKRIMLIGHTMASKLEVEL, 1.0f);
        public MaterialParam<float> parameter__MatCap = new MaterialParam<float>(MATCAP, 1.0f);
        public MaterialTextureParam parameter__MatCap_Sampler = new MaterialTextureParam(MATCAPSAMPLER);
        public MaterialParam<float> parameter__BlurLevelMatcap = new MaterialParam<float>(BLURLEVELMATCAP, 1.0f);
        public MaterialParam<Color> parameter__MatCapColor = new MaterialParam<Color>(MATCAPCOLOR, Color.white);
        public MaterialParam<float> parameter__Is_LightColor_MatCap = new MaterialParam<float>(ISLIGHTCOLORMATCAP, 1.0f);
        public MaterialParam<float> parameter__Is_BlendAddToMatCap = new MaterialParam<float>(ISBLENDADDTOMATCAP, 1.0f);
        public MaterialParam<float> parameter__Tweak_MatCapUV = new MaterialParam<float>(TWEAKMATCAPUV, 1.0f);
        public MaterialParam<float> parameter__Rotate_MatCapUV = new MaterialParam<float>(ROTATEMATCAPUV, 1.0f);
        public MaterialParam<float> parameter__CameraRolling_Stabilizer = new MaterialParam<float>(CAMERAROLLINGSTABILIZER, 1.0f);
        public MaterialParam<float> parameter__Is_NormalMapForMatCap = new MaterialParam<float>(ISNORMALMAPFORMATCAP, 1.0f);
        public MaterialTextureParam parameter__NormalMapForMatCap = new MaterialTextureParam(NORMALMAPFORMATCAP);
        public MaterialParam<float> parameter__BumpScaleMatcap = new MaterialParam<float>(BUMPSCALEMATCAP, 1.0f);
        public MaterialParam<float> parameter__Rotate_NormalMapForMatCapUV = new MaterialParam<float>(ROTATENORMALMAPFORMATCAPUV, 1.0f);
        public MaterialParam<float> parameter__Is_UseTweakMatCapOnShadow = new MaterialParam<float>(ISUSETWEAKMATCAPONSHADOW, 1.0f);
        public MaterialParam<float> parameter__TweakMatCapOnShadow = new MaterialParam<float>(TWEAKMATCAPONSHADOW, 1.0f);
        public MaterialTextureParam parameter__Set_MatcapMask = new MaterialTextureParam(SETMATCAPMASK);
        public MaterialParam<float> parameter__Tweak_MatcapMaskLevel = new MaterialParam<float>(TWEAKMATCAPMASKLEVEL, 1.0f);
        public MaterialParam<float> parameter__Inverse_MatcapMask = new MaterialParam<float>(INVERSEMATCAPMASK, 1.0f);
        public MaterialParam<float> parameter__Is_Ortho = new MaterialParam<float>(ISORTHO, 1.0f);
        public MaterialParam<float> parameter__AngelRing = new MaterialParam<float>(ANGELRING, 1.0f);
        public MaterialTextureParam parameter__AngelRing_Sampler = new MaterialTextureParam(ANGELRINGSAMPLER);
        public MaterialParam<Color> parameter__AngelRing_Color = new MaterialParam<Color>(ANGELRINGCOLOR, Color.white);
        public MaterialParam<float> parameter__Is_LightColor_AR = new MaterialParam<float>(ISLIGHTCOLORAR, 1.0f);
        public MaterialParam<float> parameter__AR_OffsetU = new MaterialParam<float>(AROFFSETU, 1.0f);
        public MaterialParam<float> parameter__AR_OffsetV = new MaterialParam<float>(AROFFSETV, 1.0f);
        public MaterialParam<float> parameter__ARSampler_AlphaOn = new MaterialParam<float>(ARSAMPLERALPHAON, 1.0f);
        public MaterialParam<float> parameter__EMISSIVE = new MaterialParam<float>(EMISSIVE, 1.0f);
        public MaterialTextureParam parameter__Emissive_Tex = new MaterialTextureParam(EMISSIVETEX);
        public MaterialParam<Color> parameter__Emissive_Color = new MaterialParam<Color>(EMISSIVECOLOR, Color.white);
        public MaterialParam<float> parameter__Base_Speed = new MaterialParam<float>(BASESPEED, 1.0f);
        public MaterialParam<float> parameter__Scroll_EmissiveU = new MaterialParam<float>(SCROLLEMISSIVEU, 1.0f);
        public MaterialParam<float> parameter__Scroll_EmissiveV = new MaterialParam<float>(SCROLLEMISSIVEV, 1.0f);
        public MaterialParam<float> parameter__Rotate_EmissiveUV = new MaterialParam<float>(ROTATEEMISSIVEUV, 1.0f);
        public MaterialParam<float> parameter__Is_PingPong_Base = new MaterialParam<float>(ISPINGPONGBASE, 1.0f);
        public MaterialParam<float> parameter__Is_ColorShift = new MaterialParam<float>(ISCOLORSHIFT, 1.0f);
        public MaterialParam<Color> parameter__ColorShift = new MaterialParam<Color>(COLORSHIFT, Color.white);
        public MaterialParam<float> parameter__ColorShift_Speed = new MaterialParam<float>(COLORSHIFTSPEED, 1.0f);
        public MaterialParam<float> parameter__Is_ViewShift = new MaterialParam<float>(ISVIEWSHIFT, 1.0f);
        public MaterialParam<Color> parameter__ViewShift = new MaterialParam<Color>(VIEWSHIFT, Color.white);
        public MaterialParam<float> parameter__Is_ViewCoord_Scroll = new MaterialParam<float>(ISVIEWCOORDSCROLL, 1.0f);
        public MaterialParam<float> parameter__OUTLINE = new MaterialParam<float>(OUTLINE, 1.0f);
        public MaterialParam<float> parameter__Outline_Width = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
        public MaterialParam<float> parameter__Farthest_Distance = new MaterialParam<float>(FARTHESTDISTANCE, 1.0f);
        public MaterialParam<float> parameter__Nearest_Distance = new MaterialParam<float>(NEARESTDISTANCE, 1.0f);
        public MaterialTextureParam parameter__Outline_Sampler = new MaterialTextureParam(OUTLINESAMPLER);
        public MaterialParam<Color> parameter__Outline_Color = new MaterialParam<Color>(OUTLINECOLOR, Color.white);
        public MaterialParam<float> parameter__Is_BlendBaseColor = new MaterialParam<float>(ISBLENDBASECOLOR, 1.0f);
        public MaterialParam<float> parameter__Is_LightColor_Outline = new MaterialParam<float>(ISLIGHTCOLOROUTLINE, 1.0f);
        public MaterialParam<float> parameter__Cutoff = new MaterialParam<float>(CUTOFF, 1.0f);
        public MaterialParam<float> parameter__Is_OutlineTex = new MaterialParam<float>(ISOUTLINETEX, 1.0f);
        public MaterialTextureParam parameter__OutlineTex = new MaterialTextureParam(OUTLINETEX);
        public MaterialParam<float> parameter__Offset_Z = new MaterialParam<float>(OFFSETZ, 1.0f);
        public MaterialParam<float> parameter__Is_BakedNormal = new MaterialParam<float>(ISBAKEDNORMAL, 1.0f);
        public MaterialTextureParam parameter__BakedNormal = new MaterialTextureParam(BAKEDNORMAL);
        public MaterialParam<float> parameter__GI_Intensity = new MaterialParam<float>(GIINTENSITY, 1.0f);
        public MaterialParam<float> parameter__Unlit_Intensity = new MaterialParam<float>(UNLITINTENSITY, 1.0f);
        public MaterialParam<float> parameter__Is_Filter_LightColor = new MaterialParam<float>(ISFILTERLIGHTCOLOR, 1.0f);
        public MaterialParam<float> parameter__Is_BLD = new MaterialParam<float>(ISBLD, 1.0f);
        public MaterialParam<float> parameter__Offset_X_Axis_BLD = new MaterialParam<float>(OFFSETXAXISBLD, 1.0f);
        public MaterialParam<float> parameter__Offset_Y_Axis_BLD = new MaterialParam<float>(OFFSETYAXISBLD, 1.0f);
        public MaterialParam<float> parameter__Inverse_Z_Axis_BLD = new MaterialParam<float>(INVERSEZAXISBLD, 1.0f);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            parameter__simpleUI.Value = material.GetFloat(parameter__simpleUI.ParamName);
            parameter__utsVersionX.Value = material.GetFloat(parameter__utsVersionX.ParamName);
            parameter__utsVersionY.Value = material.GetFloat(parameter__utsVersionY.ParamName);
            parameter__utsVersionZ.Value = material.GetFloat(parameter__utsVersionZ.ParamName);
            parameter__utsTechnique.Value = material.GetFloat(parameter__utsTechnique.ParamName);
            parameter__AutoRenderQueue.Value = material.GetFloat(parameter__AutoRenderQueue.ParamName);
            parameter__StencilMode.Value = material.GetFloat(parameter__StencilMode.ParamName);
            parameter__StencilComp.Value = material.GetFloat(parameter__StencilComp.ParamName);
            parameter__StencilNo.Value = material.GetFloat(parameter__StencilNo.ParamName);
            parameter__StencilOpPass.Value = material.GetFloat(parameter__StencilOpPass.ParamName);
            parameter__StencilOpFail.Value = material.GetFloat(parameter__StencilOpFail.ParamName);
            parameter__TransparentEnabled.Value = material.GetFloat(parameter__TransparentEnabled.ParamName);
            parameter__Metallic.Value = material.GetFloat(parameter__Metallic.ParamName);
            parameter__Smoothness.Value = material.GetFloat(parameter__Smoothness.ParamName);
            parameter__SpecColor.Value = material.GetColor(parameter__SpecColor.ParamName);
            parameter__ClippingMode.Value = material.GetFloat(parameter__ClippingMode.ParamName);
            parameter__CullMode.Value = material.GetFloat(parameter__CullMode.ParamName);
            parameter__ZWriteMode.Value = material.GetFloat(parameter__ZWriteMode.ParamName);
            parameter__ZOverDrawMode.Value = material.GetFloat(parameter__ZOverDrawMode.ParamName);
            parameter__SPRDefaultUnlitColorMask.Value = material.GetFloat(parameter__SPRDefaultUnlitColorMask.ParamName);
            parameter__SRPDefaultUnlitColMode.Value = material.GetFloat(parameter__SRPDefaultUnlitColMode.ParamName);
            var parameter__clippingmask_temp = material.GetTexture(parameter__ClippingMask.ParamName);
            if (parameter__clippingmask_temp != null) parameter__ClippingMask.Value = exportTextureInfo(parameter__clippingmask_temp);
            parameter__IsBaseMapAlphaAsClippingMask.Value = material.GetFloat(parameter__IsBaseMapAlphaAsClippingMask.ParamName);
            parameter__Inverse_Clipping.Value = material.GetFloat(parameter__Inverse_Clipping.ParamName);
            parameter__Clipping_Level.Value = material.GetFloat(parameter__Clipping_Level.ParamName);
            parameter__Tweak_transparency.Value = material.GetFloat(parameter__Tweak_transparency.ParamName);
            var parameter__maintex_temp = material.GetTexture(parameter__MainTex.ParamName);
            if (parameter__maintex_temp != null) parameter__MainTex.Value = exportTextureInfo(parameter__maintex_temp);
            parameter__BaseColor.Value = material.GetColor(parameter__BaseColor.ParamName);
            parameter__Color.Value = material.GetColor(parameter__Color.ParamName);
            parameter__Is_LightColor_Base.Value = material.GetFloat(parameter__Is_LightColor_Base.ParamName);
            var parameter__1st_shademap_temp = material.GetTexture(parameter__1st_ShadeMap.ParamName);
            if (parameter__1st_shademap_temp != null) parameter__1st_ShadeMap.Value = exportTextureInfo(parameter__1st_shademap_temp);
            parameter__Use_BaseAs1st.Value = material.GetFloat(parameter__Use_BaseAs1st.ParamName);
            parameter__1st_ShadeColor.Value = material.GetColor(parameter__1st_ShadeColor.ParamName);
            parameter__Is_LightColor_1st_Shade.Value = material.GetFloat(parameter__Is_LightColor_1st_Shade.ParamName);
            var parameter__2nd_shademap_temp = material.GetTexture(parameter__2nd_ShadeMap.ParamName);
            if (parameter__2nd_shademap_temp != null) parameter__2nd_ShadeMap.Value = exportTextureInfo(parameter__2nd_shademap_temp);
            parameter__Use_1stAs2nd.Value = material.GetFloat(parameter__Use_1stAs2nd.ParamName);
            parameter__2nd_ShadeColor.Value = material.GetColor(parameter__2nd_ShadeColor.ParamName);
            parameter__Is_LightColor_2nd_Shade.Value = material.GetFloat(parameter__Is_LightColor_2nd_Shade.ParamName);
            var parameter__normalmap_temp = material.GetTexture(parameter__NormalMap.ParamName);
            if (parameter__normalmap_temp != null) parameter__NormalMap.Value = exportNormalTextureInfo(parameter__normalmap_temp);
            parameter__BumpScale.Value = material.GetFloat(parameter__BumpScale.ParamName);
            parameter__Is_NormalMapToBase.Value = material.GetFloat(parameter__Is_NormalMapToBase.ParamName);
            parameter__Set_SystemShadowsToBase.Value = material.GetFloat(parameter__Set_SystemShadowsToBase.ParamName);
            parameter__Tweak_SystemShadowsLevel.Value = material.GetFloat(parameter__Tweak_SystemShadowsLevel.ParamName);
            parameter__BaseColor_Step.Value = material.GetFloat(parameter__BaseColor_Step.ParamName);
            parameter__BaseShade_Feather.Value = material.GetFloat(parameter__BaseShade_Feather.ParamName);
            parameter__ShadeColor_Step.Value = material.GetFloat(parameter__ShadeColor_Step.ParamName);
            parameter__1st2nd_Shades_Feather.Value = material.GetFloat(parameter__1st2nd_Shades_Feather.ParamName);
            parameter__1st_ShadeColor_Step.Value = material.GetFloat(parameter__1st_ShadeColor_Step.ParamName);
            parameter__1st_ShadeColor_Feather.Value = material.GetFloat(parameter__1st_ShadeColor_Feather.ParamName);
            parameter__2nd_ShadeColor_Step.Value = material.GetFloat(parameter__2nd_ShadeColor_Step.ParamName);
            parameter__2nd_ShadeColor_Feather.Value = material.GetFloat(parameter__2nd_ShadeColor_Feather.ParamName);
            parameter__StepOffset.Value = material.GetFloat(parameter__StepOffset.ParamName);
            parameter__Is_Filter_HiCutPointLightColor.Value = material.GetFloat(parameter__Is_Filter_HiCutPointLightColor.ParamName);
            var parameter__set_1st_shadeposition_temp = material.GetTexture(parameter__Set_1st_ShadePosition.ParamName);
            if (parameter__set_1st_shadeposition_temp != null) parameter__Set_1st_ShadePosition.Value = exportTextureInfo(parameter__set_1st_shadeposition_temp);
            var parameter__set_2nd_shadeposition_temp = material.GetTexture(parameter__Set_2nd_ShadePosition.ParamName);
            if (parameter__set_2nd_shadeposition_temp != null) parameter__Set_2nd_ShadePosition.Value = exportTextureInfo(parameter__set_2nd_shadeposition_temp);
            var parameter__shadinggrademap_temp = material.GetTexture(parameter__ShadingGradeMap.ParamName);
            if (parameter__shadinggrademap_temp != null) parameter__ShadingGradeMap.Value = exportTextureInfo(parameter__shadinggrademap_temp);
            parameter__Tweak_ShadingGradeMapLevel.Value = material.GetFloat(parameter__Tweak_ShadingGradeMapLevel.ParamName);
            parameter__BlurLevelSGM.Value = material.GetFloat(parameter__BlurLevelSGM.ParamName);
            parameter__HighColor.Value = material.GetColor(parameter__HighColor.ParamName);
            var parameter__highcolor_tex_temp = material.GetTexture(parameter__HighColor_Tex.ParamName);
            if (parameter__highcolor_tex_temp != null) parameter__HighColor_Tex.Value = exportTextureInfo(parameter__highcolor_tex_temp);
            parameter__Is_LightColor_HighColor.Value = material.GetFloat(parameter__Is_LightColor_HighColor.ParamName);
            parameter__Is_NormalMapToHighColor.Value = material.GetFloat(parameter__Is_NormalMapToHighColor.ParamName);
            parameter__HighColor_Power.Value = material.GetFloat(parameter__HighColor_Power.ParamName);
            parameter__Is_SpecularToHighColor.Value = material.GetFloat(parameter__Is_SpecularToHighColor.ParamName);
            parameter__Is_BlendAddToHiColor.Value = material.GetFloat(parameter__Is_BlendAddToHiColor.ParamName);
            parameter__Is_UseTweakHighColorOnShadow.Value = material.GetFloat(parameter__Is_UseTweakHighColorOnShadow.ParamName);
            parameter__TweakHighColorOnShadow.Value = material.GetFloat(parameter__TweakHighColorOnShadow.ParamName);
            var parameter__set_highcolormask_temp = material.GetTexture(parameter__Set_HighColorMask.ParamName);
            if (parameter__set_highcolormask_temp != null) parameter__Set_HighColorMask.Value = exportTextureInfo(parameter__set_highcolormask_temp);
            parameter__Tweak_HighColorMaskLevel.Value = material.GetFloat(parameter__Tweak_HighColorMaskLevel.ParamName);
            parameter__RimLight.Value = material.GetFloat(parameter__RimLight.ParamName);
            parameter__RimLightColor.Value = material.GetColor(parameter__RimLightColor.ParamName);
            parameter__Is_LightColor_RimLight.Value = material.GetFloat(parameter__Is_LightColor_RimLight.ParamName);
            parameter__Is_NormalMapToRimLight.Value = material.GetFloat(parameter__Is_NormalMapToRimLight.ParamName);
            parameter__RimLight_Power.Value = material.GetFloat(parameter__RimLight_Power.ParamName);
            parameter__RimLight_InsideMask.Value = material.GetFloat(parameter__RimLight_InsideMask.ParamName);
            parameter__RimLight_FeatherOff.Value = material.GetFloat(parameter__RimLight_FeatherOff.ParamName);
            parameter__LightDirection_MaskOn.Value = material.GetFloat(parameter__LightDirection_MaskOn.ParamName);
            parameter__Tweak_LightDirection_MaskLevel.Value = material.GetFloat(parameter__Tweak_LightDirection_MaskLevel.ParamName);
            parameter__Add_Antipodean_RimLight.Value = material.GetFloat(parameter__Add_Antipodean_RimLight.ParamName);
            parameter__Ap_RimLightColor.Value = material.GetColor(parameter__Ap_RimLightColor.ParamName);
            parameter__Is_LightColor_Ap_RimLight.Value = material.GetFloat(parameter__Is_LightColor_Ap_RimLight.ParamName);
            parameter__Ap_RimLight_Power.Value = material.GetFloat(parameter__Ap_RimLight_Power.ParamName);
            parameter__Ap_RimLight_FeatherOff.Value = material.GetFloat(parameter__Ap_RimLight_FeatherOff.ParamName);
            var parameter__set_rimlightmask_temp = material.GetTexture(parameter__Set_RimLightMask.ParamName);
            if (parameter__set_rimlightmask_temp != null) parameter__Set_RimLightMask.Value = exportTextureInfo(parameter__set_rimlightmask_temp);
            parameter__Tweak_RimLightMaskLevel.Value = material.GetFloat(parameter__Tweak_RimLightMaskLevel.ParamName);
            parameter__MatCap.Value = material.GetFloat(parameter__MatCap.ParamName);
            var parameter__matcap_sampler_temp = material.GetTexture(parameter__MatCap_Sampler.ParamName);
            if (parameter__matcap_sampler_temp != null) parameter__MatCap_Sampler.Value = exportTextureInfo(parameter__matcap_sampler_temp);
            parameter__BlurLevelMatcap.Value = material.GetFloat(parameter__BlurLevelMatcap.ParamName);
            parameter__MatCapColor.Value = material.GetColor(parameter__MatCapColor.ParamName);
            parameter__Is_LightColor_MatCap.Value = material.GetFloat(parameter__Is_LightColor_MatCap.ParamName);
            parameter__Is_BlendAddToMatCap.Value = material.GetFloat(parameter__Is_BlendAddToMatCap.ParamName);
            parameter__Tweak_MatCapUV.Value = material.GetFloat(parameter__Tweak_MatCapUV.ParamName);
            parameter__Rotate_MatCapUV.Value = material.GetFloat(parameter__Rotate_MatCapUV.ParamName);
            parameter__CameraRolling_Stabilizer.Value = material.GetFloat(parameter__CameraRolling_Stabilizer.ParamName);
            parameter__Is_NormalMapForMatCap.Value = material.GetFloat(parameter__Is_NormalMapForMatCap.ParamName);
            var parameter__normalmapformatcap_temp = material.GetTexture(parameter__NormalMapForMatCap.ParamName);
            if (parameter__normalmapformatcap_temp != null) parameter__NormalMapForMatCap.Value = exportTextureInfo(parameter__normalmapformatcap_temp);
            parameter__BumpScaleMatcap.Value = material.GetFloat(parameter__BumpScaleMatcap.ParamName);
            parameter__Rotate_NormalMapForMatCapUV.Value = material.GetFloat(parameter__Rotate_NormalMapForMatCapUV.ParamName);
            parameter__Is_UseTweakMatCapOnShadow.Value = material.GetFloat(parameter__Is_UseTweakMatCapOnShadow.ParamName);
            parameter__TweakMatCapOnShadow.Value = material.GetFloat(parameter__TweakMatCapOnShadow.ParamName);
            var parameter__set_matcapmask_temp = material.GetTexture(parameter__Set_MatcapMask.ParamName);
            if (parameter__set_matcapmask_temp != null) parameter__Set_MatcapMask.Value = exportTextureInfo(parameter__set_matcapmask_temp);
            parameter__Tweak_MatcapMaskLevel.Value = material.GetFloat(parameter__Tweak_MatcapMaskLevel.ParamName);
            parameter__Inverse_MatcapMask.Value = material.GetFloat(parameter__Inverse_MatcapMask.ParamName);
            parameter__Is_Ortho.Value = material.GetFloat(parameter__Is_Ortho.ParamName);
            parameter__AngelRing.Value = material.GetFloat(parameter__AngelRing.ParamName);
            var parameter__angelring_sampler_temp = material.GetTexture(parameter__AngelRing_Sampler.ParamName);
            if (parameter__angelring_sampler_temp != null) parameter__AngelRing_Sampler.Value = exportTextureInfo(parameter__angelring_sampler_temp);
            parameter__AngelRing_Color.Value = material.GetColor(parameter__AngelRing_Color.ParamName);
            parameter__Is_LightColor_AR.Value = material.GetFloat(parameter__Is_LightColor_AR.ParamName);
            parameter__AR_OffsetU.Value = material.GetFloat(parameter__AR_OffsetU.ParamName);
            parameter__AR_OffsetV.Value = material.GetFloat(parameter__AR_OffsetV.ParamName);
            parameter__ARSampler_AlphaOn.Value = material.GetFloat(parameter__ARSampler_AlphaOn.ParamName);
            parameter__EMISSIVE.Value = material.GetFloat(parameter__EMISSIVE.ParamName);
            var parameter__emissive_tex_temp = material.GetTexture(parameter__Emissive_Tex.ParamName);
            if (parameter__emissive_tex_temp != null) parameter__Emissive_Tex.Value = exportTextureInfo(parameter__emissive_tex_temp);
            parameter__Emissive_Color.Value = material.GetColor(parameter__Emissive_Color.ParamName);
            parameter__Base_Speed.Value = material.GetFloat(parameter__Base_Speed.ParamName);
            parameter__Scroll_EmissiveU.Value = material.GetFloat(parameter__Scroll_EmissiveU.ParamName);
            parameter__Scroll_EmissiveV.Value = material.GetFloat(parameter__Scroll_EmissiveV.ParamName);
            parameter__Rotate_EmissiveUV.Value = material.GetFloat(parameter__Rotate_EmissiveUV.ParamName);
            parameter__Is_PingPong_Base.Value = material.GetFloat(parameter__Is_PingPong_Base.ParamName);
            parameter__Is_ColorShift.Value = material.GetFloat(parameter__Is_ColorShift.ParamName);
            parameter__ColorShift.Value = material.GetColor(parameter__ColorShift.ParamName);
            parameter__ColorShift_Speed.Value = material.GetFloat(parameter__ColorShift_Speed.ParamName);
            parameter__Is_ViewShift.Value = material.GetFloat(parameter__Is_ViewShift.ParamName);
            parameter__ViewShift.Value = material.GetColor(parameter__ViewShift.ParamName);
            parameter__Is_ViewCoord_Scroll.Value = material.GetFloat(parameter__Is_ViewCoord_Scroll.ParamName);
            parameter__OUTLINE.Value = material.GetFloat(parameter__OUTLINE.ParamName);
            parameter__Outline_Width.Value = material.GetFloat(parameter__Outline_Width.ParamName);
            parameter__Farthest_Distance.Value = material.GetFloat(parameter__Farthest_Distance.ParamName);
            parameter__Nearest_Distance.Value = material.GetFloat(parameter__Nearest_Distance.ParamName);
            var parameter__outline_sampler_temp = material.GetTexture(parameter__Outline_Sampler.ParamName);
            if (parameter__outline_sampler_temp != null) parameter__Outline_Sampler.Value = exportTextureInfo(parameter__outline_sampler_temp);
            parameter__Outline_Color.Value = material.GetColor(parameter__Outline_Color.ParamName);
            parameter__Is_BlendBaseColor.Value = material.GetFloat(parameter__Is_BlendBaseColor.ParamName);
            parameter__Is_LightColor_Outline.Value = material.GetFloat(parameter__Is_LightColor_Outline.ParamName);
            parameter__Cutoff.Value = material.GetFloat(parameter__Cutoff.ParamName);
            parameter__Is_OutlineTex.Value = material.GetFloat(parameter__Is_OutlineTex.ParamName);
            var parameter__outlinetex_temp = material.GetTexture(parameter__OutlineTex.ParamName);
            if (parameter__outlinetex_temp != null) parameter__OutlineTex.Value = exportTextureInfo(parameter__outlinetex_temp);
            parameter__Offset_Z.Value = material.GetFloat(parameter__Offset_Z.ParamName);
            parameter__Is_BakedNormal.Value = material.GetFloat(parameter__Is_BakedNormal.ParamName);
            var parameter__bakednormal_temp = material.GetTexture(parameter__BakedNormal.ParamName);
            if (parameter__bakednormal_temp != null) parameter__BakedNormal.Value = exportTextureInfo(parameter__bakednormal_temp);
            parameter__GI_Intensity.Value = material.GetFloat(parameter__GI_Intensity.ParamName);
            parameter__Unlit_Intensity.Value = material.GetFloat(parameter__Unlit_Intensity.ParamName);
            parameter__Is_Filter_LightColor.Value = material.GetFloat(parameter__Is_Filter_LightColor.ParamName);
            parameter__Is_BLD.Value = material.GetFloat(parameter__Is_BLD.ParamName);
            parameter__Offset_X_Axis_BLD.Value = material.GetFloat(parameter__Offset_X_Axis_BLD.ParamName);
            parameter__Offset_Y_Axis_BLD.Value = material.GetFloat(parameter__Offset_Y_Axis_BLD.ParamName);
            parameter__Inverse_Z_Axis_BLD.Value = material.GetFloat(parameter__Inverse_Z_Axis_BLD.ParamName);
        }
        public async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache, AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case BVA_Material_UTS_Extra.SIMPLEUI:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SIMPLEUI, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.UTSVERSIONX:
                            matCache.SetFloat(BVA_Material_UTS_Extra.UTSVERSIONX, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.UTSVERSIONY:
                            matCache.SetFloat(BVA_Material_UTS_Extra.UTSVERSIONY, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.UTSVERSIONZ:
                            matCache.SetFloat(BVA_Material_UTS_Extra.UTSVERSIONZ, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.UTSTECHNIQUE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.UTSTECHNIQUE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.AUTORENDERQUEUE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.AUTORENDERQUEUE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.STENCILMODE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.STENCILMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.STENCILCOMP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.STENCILCOMP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.STENCILNO:
                            matCache.SetFloat(BVA_Material_UTS_Extra.STENCILNO, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.STENCILOPPASS:
                            matCache.SetFloat(BVA_Material_UTS_Extra.STENCILOPPASS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.STENCILOPFAIL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.STENCILOPFAIL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.TRANSPARENTENABLED:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TRANSPARENTENABLED, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.METALLIC:
                            matCache.SetFloat(BVA_Material_UTS_Extra.METALLIC, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SMOOTHNESS:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SMOOTHNESS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SPECCOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.SPECCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.CLIPPINGMODE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.CLIPPINGMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.CULLMODE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.CULLMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ZWRITEMODE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ZWRITEMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ZOVERDRAWMODE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ZOVERDRAWMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SPRDEFAULTUNLITCOLORMASK:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SPRDEFAULTUNLITCOLORMASK, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SRPDEFAULTUNLITCOLMODE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SRPDEFAULTUNLITCOLMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.CLIPPINGMASK:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.CLIPPINGMASK, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.ISBASEMAPALPHAASCLIPPINGMASK:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISBASEMAPALPHAASCLIPPINGMASK, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.INVERSECLIPPING:
                            matCache.SetFloat(BVA_Material_UTS_Extra.INVERSECLIPPING, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.CLIPPINGLEVEL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.CLIPPINGLEVEL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.TWEAKTRANSPARENCY:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKTRANSPARENCY, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.MAINTEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.MAINTEX, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.BASECOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.COLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.COLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLORBASE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLORBASE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.FIRSTSHADEMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.FIRSTSHADEMAP, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.USEBASEASFIRST:
                            matCache.SetFloat(BVA_Material_UTS_Extra.USEBASEASFIRST, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.FIRSTSHADECOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.FIRSTSHADECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLORFIRSTSHADE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLORFIRSTSHADE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SECONDSHADEMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.SECONDSHADEMAP, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.USEFIRSTASSECOND:
                            matCache.SetFloat(BVA_Material_UTS_Extra.USEFIRSTASSECOND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SECONDSHADECOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.SECONDSHADECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLORSECONDSHADE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLORSECONDSHADE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.NORMALMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.NORMALMAP, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.BUMPSCALE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.BUMPSCALE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISNORMALMAPTOBASE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISNORMALMAPTOBASE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SETSYSTEMSHADOWSTOBASE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SETSYSTEMSHADOWSTOBASE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.TWEAKSYSTEMSHADOWSLEVEL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKSYSTEMSHADOWSLEVEL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.BASECOLORSTEP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.BASECOLORSTEP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.BASESHADEFEATHER:
                            matCache.SetFloat(BVA_Material_UTS_Extra.BASESHADEFEATHER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SHADECOLORSTEP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SHADECOLORSTEP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.FIRSTSECONDSHADESFEATHER:
                            matCache.SetFloat(BVA_Material_UTS_Extra.FIRSTSECONDSHADESFEATHER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.FIRSTSHADECOLORSTEP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.FIRSTSHADECOLORSTEP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.FIRSTSHADECOLORFEATHER:
                            matCache.SetFloat(BVA_Material_UTS_Extra.FIRSTSHADECOLORFEATHER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SECONDSHADECOLORSTEP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SECONDSHADECOLORSTEP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SECONDSHADECOLORFEATHER:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SECONDSHADECOLORFEATHER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.STEPOFFSET:
                            matCache.SetFloat(BVA_Material_UTS_Extra.STEPOFFSET, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISFILTERHICUTPOINTLIGHTCOLOR:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISFILTERHICUTPOINTLIGHTCOLOR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SETFIRSTSHADEPOSITION:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.SETFIRSTSHADEPOSITION, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.SETSECONDSHADEPOSITION:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.SETSECONDSHADEPOSITION, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.SHADINGGRADEMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.SHADINGGRADEMAP, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.TWEAKSHADINGGRADEMAPLEVEL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKSHADINGGRADEMAPLEVEL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.BLURLEVELSGM:
                            matCache.SetFloat(BVA_Material_UTS_Extra.BLURLEVELSGM, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.HIGHCOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.HIGHCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.HIGHCOLORTEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.HIGHCOLORTEX, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLORHIGHCOLOR:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLORHIGHCOLOR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISNORMALMAPTOHIGHCOLOR:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISNORMALMAPTOHIGHCOLOR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.HIGHCOLORPOWER:
                            matCache.SetFloat(BVA_Material_UTS_Extra.HIGHCOLORPOWER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISSPECULARTOHIGHCOLOR:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISSPECULARTOHIGHCOLOR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISBLENDADDTOHICOLOR:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISBLENDADDTOHICOLOR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISUSETWEAKHIGHCOLORONSHADOW:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISUSETWEAKHIGHCOLORONSHADOW, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.TWEAKHIGHCOLORONSHADOW:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKHIGHCOLORONSHADOW, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SETHIGHCOLORMASK:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.SETHIGHCOLORMASK, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.TWEAKHIGHCOLORMASKLEVEL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKHIGHCOLORMASKLEVEL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.RIMLIGHT:
                            matCache.SetFloat(BVA_Material_UTS_Extra.RIMLIGHT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.RIMLIGHTCOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.RIMLIGHTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLORRIMLIGHT:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLORRIMLIGHT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISNORMALMAPTORIMLIGHT:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISNORMALMAPTORIMLIGHT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.RIMLIGHTPOWER:
                            matCache.SetFloat(BVA_Material_UTS_Extra.RIMLIGHTPOWER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.RIMLIGHTINSIDEMASK:
                            matCache.SetFloat(BVA_Material_UTS_Extra.RIMLIGHTINSIDEMASK, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.RIMLIGHTFEATHEROFF:
                            matCache.SetFloat(BVA_Material_UTS_Extra.RIMLIGHTFEATHEROFF, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.LIGHTDIRECTIONMASKON:
                            matCache.SetFloat(BVA_Material_UTS_Extra.LIGHTDIRECTIONMASKON, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.TWEAKLIGHTDIRECTIONMASKLEVEL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKLIGHTDIRECTIONMASKLEVEL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ADDANTIPODEANRIMLIGHT:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ADDANTIPODEANRIMLIGHT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.APRIMLIGHTCOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.APRIMLIGHTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLORAPRIMLIGHT:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLORAPRIMLIGHT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.APRIMLIGHTPOWER:
                            matCache.SetFloat(BVA_Material_UTS_Extra.APRIMLIGHTPOWER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.APRIMLIGHTFEATHEROFF:
                            matCache.SetFloat(BVA_Material_UTS_Extra.APRIMLIGHTFEATHEROFF, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SETRIMLIGHTMASK:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.SETRIMLIGHTMASK, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.TWEAKRIMLIGHTMASKLEVEL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKRIMLIGHTMASKLEVEL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.MATCAP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.MATCAP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.MATCAPSAMPLER:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.MATCAPSAMPLER, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.BLURLEVELMATCAP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.BLURLEVELMATCAP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.MATCAPCOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.MATCAPCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLORMATCAP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLORMATCAP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISBLENDADDTOMATCAP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISBLENDADDTOMATCAP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.TWEAKMATCAPUV:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKMATCAPUV, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ROTATEMATCAPUV:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ROTATEMATCAPUV, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.CAMERAROLLINGSTABILIZER:
                            matCache.SetFloat(BVA_Material_UTS_Extra.CAMERAROLLINGSTABILIZER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISNORMALMAPFORMATCAP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISNORMALMAPFORMATCAP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.NORMALMAPFORMATCAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.NORMALMAPFORMATCAP, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.BUMPSCALEMATCAP:
                            matCache.SetFloat(BVA_Material_UTS_Extra.BUMPSCALEMATCAP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ROTATENORMALMAPFORMATCAPUV:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ROTATENORMALMAPFORMATCAPUV, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISUSETWEAKMATCAPONSHADOW:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISUSETWEAKMATCAPONSHADOW, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.TWEAKMATCAPONSHADOW:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKMATCAPONSHADOW, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SETMATCAPMASK:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.SETMATCAPMASK, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.TWEAKMATCAPMASKLEVEL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.TWEAKMATCAPMASKLEVEL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.INVERSEMATCAPMASK:
                            matCache.SetFloat(BVA_Material_UTS_Extra.INVERSEMATCAPMASK, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISORTHO:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISORTHO, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ANGELRING:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ANGELRING, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ANGELRINGSAMPLER:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.ANGELRINGSAMPLER, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.ANGELRINGCOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.ANGELRINGCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLORAR:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLORAR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.AROFFSETU:
                            matCache.SetFloat(BVA_Material_UTS_Extra.AROFFSETU, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.AROFFSETV:
                            matCache.SetFloat(BVA_Material_UTS_Extra.AROFFSETV, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ARSAMPLERALPHAON:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ARSAMPLERALPHAON, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.EMISSIVE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.EMISSIVE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.EMISSIVETEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.EMISSIVETEX, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.EMISSIVECOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.EMISSIVECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.BASESPEED:
                            matCache.SetFloat(BVA_Material_UTS_Extra.BASESPEED, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SCROLLEMISSIVEU:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SCROLLEMISSIVEU, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.SCROLLEMISSIVEV:
                            matCache.SetFloat(BVA_Material_UTS_Extra.SCROLLEMISSIVEV, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ROTATEEMISSIVEUV:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ROTATEEMISSIVEUV, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISPINGPONGBASE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISPINGPONGBASE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISCOLORSHIFT:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISCOLORSHIFT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.COLORSHIFT:
                            matCache.SetColor(BVA_Material_UTS_Extra.COLORSHIFT, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.COLORSHIFTSPEED:
                            matCache.SetFloat(BVA_Material_UTS_Extra.COLORSHIFTSPEED, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISVIEWSHIFT:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISVIEWSHIFT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.VIEWSHIFT:
                            matCache.SetColor(BVA_Material_UTS_Extra.VIEWSHIFT, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISVIEWCOORDSCROLL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISVIEWCOORDSCROLL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.OUTLINE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.OUTLINE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.OUTLINEWIDTH:
                            matCache.SetFloat(BVA_Material_UTS_Extra.OUTLINEWIDTH, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.FARTHESTDISTANCE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.FARTHESTDISTANCE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.NEARESTDISTANCE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.NEARESTDISTANCE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.OUTLINESAMPLER:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.OUTLINESAMPLER, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.OUTLINECOLOR:
                            matCache.SetColor(BVA_Material_UTS_Extra.OUTLINECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_UTS_Extra.ISBLENDBASECOLOR:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISBLENDBASECOLOR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISLIGHTCOLOROUTLINE:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISLIGHTCOLOROUTLINE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.CUTOFF:
                            matCache.SetFloat(BVA_Material_UTS_Extra.CUTOFF, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISOUTLINETEX:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISOUTLINETEX, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.OUTLINETEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.OUTLINETEX, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.OFFSETZ:
                            matCache.SetFloat(BVA_Material_UTS_Extra.OFFSETZ, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISBAKEDNORMAL:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISBAKEDNORMAL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.BAKEDNORMAL:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_UTS_Extra.BAKEDNORMAL, tex);
                            }
                            break;
                        case BVA_Material_UTS_Extra.GIINTENSITY:
                            matCache.SetFloat(BVA_Material_UTS_Extra.GIINTENSITY, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.UNLITINTENSITY:
                            matCache.SetFloat(BVA_Material_UTS_Extra.UNLITINTENSITY, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISFILTERLIGHTCOLOR:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISFILTERLIGHTCOLOR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.ISBLD:
                            matCache.SetFloat(BVA_Material_UTS_Extra.ISBLD, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.OFFSETXAXISBLD:
                            matCache.SetFloat(BVA_Material_UTS_Extra.OFFSETXAXISBLD, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.OFFSETYAXISBLD:
                            matCache.SetFloat(BVA_Material_UTS_Extra.OFFSETYAXISBLD, reader.ReadAsFloat());
                            break;
                        case BVA_Material_UTS_Extra.INVERSEZAXISBLD:
                            matCache.SetFloat(BVA_Material_UTS_Extra.INVERSEZAXISBLD, reader.ReadAsFloat());
                            break;
                        case nameof(keywords):
                            {
                                var keywords = reader.ReadStringList();
                                foreach (var keyword in keywords)
                                    matCache.EnableKeyword(keyword);
                            }
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(parameter__simpleUI.ParamName, parameter__simpleUI.Value);
            jo.Add(parameter__utsVersionX.ParamName, parameter__utsVersionX.Value);
            jo.Add(parameter__utsVersionY.ParamName, parameter__utsVersionY.Value);
            jo.Add(parameter__utsVersionZ.ParamName, parameter__utsVersionZ.Value);
            jo.Add(parameter__utsTechnique.ParamName, parameter__utsTechnique.Value);
            jo.Add(parameter__AutoRenderQueue.ParamName, parameter__AutoRenderQueue.Value);
            jo.Add(parameter__StencilMode.ParamName, parameter__StencilMode.Value);
            jo.Add(parameter__StencilComp.ParamName, parameter__StencilComp.Value);
            jo.Add(parameter__StencilNo.ParamName, parameter__StencilNo.Value);
            jo.Add(parameter__StencilOpPass.ParamName, parameter__StencilOpPass.Value);
            jo.Add(parameter__StencilOpFail.ParamName, parameter__StencilOpFail.Value);
            jo.Add(parameter__TransparentEnabled.ParamName, parameter__TransparentEnabled.Value);
            jo.Add(parameter__Metallic.ParamName, parameter__Metallic.Value);
            jo.Add(parameter__Smoothness.ParamName, parameter__Smoothness.Value);
            jo.Add(parameter__SpecColor.ParamName, parameter__SpecColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__ClippingMode.ParamName, parameter__ClippingMode.Value);
            jo.Add(parameter__CullMode.ParamName, parameter__CullMode.Value);
            jo.Add(parameter__ZWriteMode.ParamName, parameter__ZWriteMode.Value);
            jo.Add(parameter__ZOverDrawMode.ParamName, parameter__ZOverDrawMode.Value);
            jo.Add(parameter__SPRDefaultUnlitColorMask.ParamName, parameter__SPRDefaultUnlitColorMask.Value);
            jo.Add(parameter__SRPDefaultUnlitColMode.ParamName, parameter__SRPDefaultUnlitColMode.Value);
            if (parameter__ClippingMask != null && parameter__ClippingMask.Value != null) jo.Add(parameter__ClippingMask.ParamName, parameter__ClippingMask.Serialize());
            jo.Add(parameter__IsBaseMapAlphaAsClippingMask.ParamName, parameter__IsBaseMapAlphaAsClippingMask.Value);
            jo.Add(parameter__Inverse_Clipping.ParamName, parameter__Inverse_Clipping.Value);
            jo.Add(parameter__Clipping_Level.ParamName, parameter__Clipping_Level.Value);
            jo.Add(parameter__Tweak_transparency.ParamName, parameter__Tweak_transparency.Value);
            if (parameter__MainTex != null && parameter__MainTex.Value != null) jo.Add(parameter__MainTex.ParamName, parameter__MainTex.Serialize());
            jo.Add(parameter__BaseColor.ParamName, parameter__BaseColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Color.ParamName, parameter__Color.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_LightColor_Base.ParamName, parameter__Is_LightColor_Base.Value);
            if (parameter__1st_ShadeMap != null && parameter__1st_ShadeMap.Value != null) jo.Add(parameter__1st_ShadeMap.ParamName, parameter__1st_ShadeMap.Serialize());
            jo.Add(parameter__Use_BaseAs1st.ParamName, parameter__Use_BaseAs1st.Value);
            jo.Add(parameter__1st_ShadeColor.ParamName, parameter__1st_ShadeColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_LightColor_1st_Shade.ParamName, parameter__Is_LightColor_1st_Shade.Value);
            if (parameter__2nd_ShadeMap != null && parameter__2nd_ShadeMap.Value != null) jo.Add(parameter__2nd_ShadeMap.ParamName, parameter__2nd_ShadeMap.Serialize());
            jo.Add(parameter__Use_1stAs2nd.ParamName, parameter__Use_1stAs2nd.Value);
            jo.Add(parameter__2nd_ShadeColor.ParamName, parameter__2nd_ShadeColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_LightColor_2nd_Shade.ParamName, parameter__Is_LightColor_2nd_Shade.Value);
            if (parameter__NormalMap != null && parameter__NormalMap.Value != null) jo.Add(parameter__NormalMap.ParamName, parameter__NormalMap.Serialize());
            jo.Add(parameter__BumpScale.ParamName, parameter__BumpScale.Value);
            jo.Add(parameter__Is_NormalMapToBase.ParamName, parameter__Is_NormalMapToBase.Value);
            jo.Add(parameter__Set_SystemShadowsToBase.ParamName, parameter__Set_SystemShadowsToBase.Value);
            jo.Add(parameter__Tweak_SystemShadowsLevel.ParamName, parameter__Tweak_SystemShadowsLevel.Value);
            jo.Add(parameter__BaseColor_Step.ParamName, parameter__BaseColor_Step.Value);
            jo.Add(parameter__BaseShade_Feather.ParamName, parameter__BaseShade_Feather.Value);
            jo.Add(parameter__ShadeColor_Step.ParamName, parameter__ShadeColor_Step.Value);
            jo.Add(parameter__1st2nd_Shades_Feather.ParamName, parameter__1st2nd_Shades_Feather.Value);
            jo.Add(parameter__1st_ShadeColor_Step.ParamName, parameter__1st_ShadeColor_Step.Value);
            jo.Add(parameter__1st_ShadeColor_Feather.ParamName, parameter__1st_ShadeColor_Feather.Value);
            jo.Add(parameter__2nd_ShadeColor_Step.ParamName, parameter__2nd_ShadeColor_Step.Value);
            jo.Add(parameter__2nd_ShadeColor_Feather.ParamName, parameter__2nd_ShadeColor_Feather.Value);
            jo.Add(parameter__StepOffset.ParamName, parameter__StepOffset.Value);
            jo.Add(parameter__Is_Filter_HiCutPointLightColor.ParamName, parameter__Is_Filter_HiCutPointLightColor.Value);
            if (parameter__Set_1st_ShadePosition != null && parameter__Set_1st_ShadePosition.Value != null) jo.Add(parameter__Set_1st_ShadePosition.ParamName, parameter__Set_1st_ShadePosition.Serialize());
            if (parameter__Set_2nd_ShadePosition != null && parameter__Set_2nd_ShadePosition.Value != null) jo.Add(parameter__Set_2nd_ShadePosition.ParamName, parameter__Set_2nd_ShadePosition.Serialize());
            if (parameter__ShadingGradeMap != null && parameter__ShadingGradeMap.Value != null) jo.Add(parameter__ShadingGradeMap.ParamName, parameter__ShadingGradeMap.Serialize());
            jo.Add(parameter__Tweak_ShadingGradeMapLevel.ParamName, parameter__Tweak_ShadingGradeMapLevel.Value);
            jo.Add(parameter__BlurLevelSGM.ParamName, parameter__BlurLevelSGM.Value);
            jo.Add(parameter__HighColor.ParamName, parameter__HighColor.Value.ToNumericsColorRaw().ToJArray());
            if (parameter__HighColor_Tex != null && parameter__HighColor_Tex.Value != null) jo.Add(parameter__HighColor_Tex.ParamName, parameter__HighColor_Tex.Serialize());
            jo.Add(parameter__Is_LightColor_HighColor.ParamName, parameter__Is_LightColor_HighColor.Value);
            jo.Add(parameter__Is_NormalMapToHighColor.ParamName, parameter__Is_NormalMapToHighColor.Value);
            jo.Add(parameter__HighColor_Power.ParamName, parameter__HighColor_Power.Value);
            jo.Add(parameter__Is_SpecularToHighColor.ParamName, parameter__Is_SpecularToHighColor.Value);
            jo.Add(parameter__Is_BlendAddToHiColor.ParamName, parameter__Is_BlendAddToHiColor.Value);
            jo.Add(parameter__Is_UseTweakHighColorOnShadow.ParamName, parameter__Is_UseTweakHighColorOnShadow.Value);
            jo.Add(parameter__TweakHighColorOnShadow.ParamName, parameter__TweakHighColorOnShadow.Value);
            if (parameter__Set_HighColorMask != null && parameter__Set_HighColorMask.Value != null) jo.Add(parameter__Set_HighColorMask.ParamName, parameter__Set_HighColorMask.Serialize());
            jo.Add(parameter__Tweak_HighColorMaskLevel.ParamName, parameter__Tweak_HighColorMaskLevel.Value);
            jo.Add(parameter__RimLight.ParamName, parameter__RimLight.Value);
            jo.Add(parameter__RimLightColor.ParamName, parameter__RimLightColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_LightColor_RimLight.ParamName, parameter__Is_LightColor_RimLight.Value);
            jo.Add(parameter__Is_NormalMapToRimLight.ParamName, parameter__Is_NormalMapToRimLight.Value);
            jo.Add(parameter__RimLight_Power.ParamName, parameter__RimLight_Power.Value);
            jo.Add(parameter__RimLight_InsideMask.ParamName, parameter__RimLight_InsideMask.Value);
            jo.Add(parameter__RimLight_FeatherOff.ParamName, parameter__RimLight_FeatherOff.Value);
            jo.Add(parameter__LightDirection_MaskOn.ParamName, parameter__LightDirection_MaskOn.Value);
            jo.Add(parameter__Tweak_LightDirection_MaskLevel.ParamName, parameter__Tweak_LightDirection_MaskLevel.Value);
            jo.Add(parameter__Add_Antipodean_RimLight.ParamName, parameter__Add_Antipodean_RimLight.Value);
            jo.Add(parameter__Ap_RimLightColor.ParamName, parameter__Ap_RimLightColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_LightColor_Ap_RimLight.ParamName, parameter__Is_LightColor_Ap_RimLight.Value);
            jo.Add(parameter__Ap_RimLight_Power.ParamName, parameter__Ap_RimLight_Power.Value);
            jo.Add(parameter__Ap_RimLight_FeatherOff.ParamName, parameter__Ap_RimLight_FeatherOff.Value);
            if (parameter__Set_RimLightMask != null && parameter__Set_RimLightMask.Value != null) jo.Add(parameter__Set_RimLightMask.ParamName, parameter__Set_RimLightMask.Serialize());
            jo.Add(parameter__Tweak_RimLightMaskLevel.ParamName, parameter__Tweak_RimLightMaskLevel.Value);
            jo.Add(parameter__MatCap.ParamName, parameter__MatCap.Value);
            if (parameter__MatCap_Sampler != null && parameter__MatCap_Sampler.Value != null) jo.Add(parameter__MatCap_Sampler.ParamName, parameter__MatCap_Sampler.Serialize());
            jo.Add(parameter__BlurLevelMatcap.ParamName, parameter__BlurLevelMatcap.Value);
            jo.Add(parameter__MatCapColor.ParamName, parameter__MatCapColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_LightColor_MatCap.ParamName, parameter__Is_LightColor_MatCap.Value);
            jo.Add(parameter__Is_BlendAddToMatCap.ParamName, parameter__Is_BlendAddToMatCap.Value);
            jo.Add(parameter__Tweak_MatCapUV.ParamName, parameter__Tweak_MatCapUV.Value);
            jo.Add(parameter__Rotate_MatCapUV.ParamName, parameter__Rotate_MatCapUV.Value);
            jo.Add(parameter__CameraRolling_Stabilizer.ParamName, parameter__CameraRolling_Stabilizer.Value);
            jo.Add(parameter__Is_NormalMapForMatCap.ParamName, parameter__Is_NormalMapForMatCap.Value);
            if (parameter__NormalMapForMatCap != null && parameter__NormalMapForMatCap.Value != null) jo.Add(parameter__NormalMapForMatCap.ParamName, parameter__NormalMapForMatCap.Serialize());
            jo.Add(parameter__BumpScaleMatcap.ParamName, parameter__BumpScaleMatcap.Value);
            jo.Add(parameter__Rotate_NormalMapForMatCapUV.ParamName, parameter__Rotate_NormalMapForMatCapUV.Value);
            jo.Add(parameter__Is_UseTweakMatCapOnShadow.ParamName, parameter__Is_UseTweakMatCapOnShadow.Value);
            jo.Add(parameter__TweakMatCapOnShadow.ParamName, parameter__TweakMatCapOnShadow.Value);
            if (parameter__Set_MatcapMask != null && parameter__Set_MatcapMask.Value != null) jo.Add(parameter__Set_MatcapMask.ParamName, parameter__Set_MatcapMask.Serialize());
            jo.Add(parameter__Tweak_MatcapMaskLevel.ParamName, parameter__Tweak_MatcapMaskLevel.Value);
            jo.Add(parameter__Inverse_MatcapMask.ParamName, parameter__Inverse_MatcapMask.Value);
            jo.Add(parameter__Is_Ortho.ParamName, parameter__Is_Ortho.Value);
            jo.Add(parameter__AngelRing.ParamName, parameter__AngelRing.Value);
            if (parameter__AngelRing_Sampler != null && parameter__AngelRing_Sampler.Value != null) jo.Add(parameter__AngelRing_Sampler.ParamName, parameter__AngelRing_Sampler.Serialize());
            jo.Add(parameter__AngelRing_Color.ParamName, parameter__AngelRing_Color.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_LightColor_AR.ParamName, parameter__Is_LightColor_AR.Value);
            jo.Add(parameter__AR_OffsetU.ParamName, parameter__AR_OffsetU.Value);
            jo.Add(parameter__AR_OffsetV.ParamName, parameter__AR_OffsetV.Value);
            jo.Add(parameter__ARSampler_AlphaOn.ParamName, parameter__ARSampler_AlphaOn.Value);
            jo.Add(parameter__EMISSIVE.ParamName, parameter__EMISSIVE.Value);
            if (parameter__Emissive_Tex != null && parameter__Emissive_Tex.Value != null) jo.Add(parameter__Emissive_Tex.ParamName, parameter__Emissive_Tex.Serialize());
            jo.Add(parameter__Emissive_Color.ParamName, parameter__Emissive_Color.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Base_Speed.ParamName, parameter__Base_Speed.Value);
            jo.Add(parameter__Scroll_EmissiveU.ParamName, parameter__Scroll_EmissiveU.Value);
            jo.Add(parameter__Scroll_EmissiveV.ParamName, parameter__Scroll_EmissiveV.Value);
            jo.Add(parameter__Rotate_EmissiveUV.ParamName, parameter__Rotate_EmissiveUV.Value);
            jo.Add(parameter__Is_PingPong_Base.ParamName, parameter__Is_PingPong_Base.Value);
            jo.Add(parameter__Is_ColorShift.ParamName, parameter__Is_ColorShift.Value);
            jo.Add(parameter__ColorShift.ParamName, parameter__ColorShift.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__ColorShift_Speed.ParamName, parameter__ColorShift_Speed.Value);
            jo.Add(parameter__Is_ViewShift.ParamName, parameter__Is_ViewShift.Value);
            jo.Add(parameter__ViewShift.ParamName, parameter__ViewShift.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_ViewCoord_Scroll.ParamName, parameter__Is_ViewCoord_Scroll.Value);
            jo.Add(parameter__OUTLINE.ParamName, parameter__OUTLINE.Value);
            jo.Add(parameter__Outline_Width.ParamName, parameter__Outline_Width.Value);
            jo.Add(parameter__Farthest_Distance.ParamName, parameter__Farthest_Distance.Value);
            jo.Add(parameter__Nearest_Distance.ParamName, parameter__Nearest_Distance.Value);
            if (parameter__Outline_Sampler != null && parameter__Outline_Sampler.Value != null) jo.Add(parameter__Outline_Sampler.ParamName, parameter__Outline_Sampler.Serialize());
            jo.Add(parameter__Outline_Color.ParamName, parameter__Outline_Color.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Is_BlendBaseColor.ParamName, parameter__Is_BlendBaseColor.Value);
            jo.Add(parameter__Is_LightColor_Outline.ParamName, parameter__Is_LightColor_Outline.Value);
            jo.Add(parameter__Cutoff.ParamName, parameter__Cutoff.Value);
            jo.Add(parameter__Is_OutlineTex.ParamName, parameter__Is_OutlineTex.Value);
            if (parameter__OutlineTex != null && parameter__OutlineTex.Value != null) jo.Add(parameter__OutlineTex.ParamName, parameter__OutlineTex.Serialize());
            jo.Add(parameter__Offset_Z.ParamName, parameter__Offset_Z.Value);
            jo.Add(parameter__Is_BakedNormal.ParamName, parameter__Is_BakedNormal.Value);
            if (parameter__BakedNormal != null && parameter__BakedNormal.Value != null) jo.Add(parameter__BakedNormal.ParamName, parameter__BakedNormal.Serialize());
            jo.Add(parameter__GI_Intensity.ParamName, parameter__GI_Intensity.Value);
            jo.Add(parameter__Unlit_Intensity.ParamName, parameter__Unlit_Intensity.Value);
            jo.Add(parameter__Is_Filter_LightColor.ParamName, parameter__Is_Filter_LightColor.Value);
            jo.Add(parameter__Is_BLD.ParamName, parameter__Is_BLD.Value);
            jo.Add(parameter__Offset_X_Axis_BLD.ParamName, parameter__Offset_X_Axis_BLD.Value);
            jo.Add(parameter__Offset_Y_Axis_BLD.ParamName, parameter__Offset_Y_Axis_BLD.Value);
            jo.Add(parameter__Inverse_Z_Axis_BLD.ParamName, parameter__Inverse_Z_Axis_BLD.Value);
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_UTS_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_UTS_Extra();
        }
    }
}
