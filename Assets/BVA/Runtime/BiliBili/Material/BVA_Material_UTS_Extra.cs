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
public class BVA_Material_UTS_Extra : MaterialExtra
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
public MaterialParam<float> parameter_SimpleUI = new MaterialParam<float>(SIMPLEUI, 1.0f);
public MaterialParam<float> parameter_VersionX = new MaterialParam<float>(UTSVERSIONX, 1.0f);
public MaterialParam<float> parameter_VersionY = new MaterialParam<float>(UTSVERSIONY, 1.0f);
public MaterialParam<float> parameter_VersionZ = new MaterialParam<float>(UTSVERSIONZ, 1.0f);
public MaterialParam<float> parameter_Technique = new MaterialParam<float>(UTSTECHNIQUE, 1.0f);
public MaterialParam<float> parameter_AutomaticRenderQueue = new MaterialParam<float>(AUTORENDERQUEUE, 1.0f);
public MaterialParam<float> parameter_StencilMode = new MaterialParam<float>(STENCILMODE, 1.0f);
public MaterialParam<float> parameter_StencilComparison = new MaterialParam<float>(STENCILCOMP, 1.0f);
public MaterialParam<float> parameter_StencilNo = new MaterialParam<float>(STENCILNO, 1.0f);
public MaterialParam<float> parameter_StencilOperationPass = new MaterialParam<float>(STENCILOPPASS, 1.0f);
public MaterialParam<float> parameter_StencilOperationFail = new MaterialParam<float>(STENCILOPFAIL, 1.0f);
public MaterialParam<float> parameter_TransparentMode = new MaterialParam<float>(TRANSPARENTENABLED, 1.0f);
public MaterialParam<float> parameter__Metallic = new MaterialParam<float>(METALLIC, 1.0f);
public MaterialParam<float> parameter_Smoothness = new MaterialParam<float>(SMOOTHNESS, 1.0f);
public MaterialParam<Color> parameter__SpecColor = new MaterialParam<Color>(SPECCOLOR, Color.white);
public MaterialParam<float> parameter_CliippingMode = new MaterialParam<float>(CLIPPINGMODE, 1.0f);
public MaterialParam<float> parameter_CullMode = new MaterialParam<float>(CULLMODE, 1.0f);
public MaterialParam<float> parameter_ZWriteMode = new MaterialParam<float>(ZWRITEMODE, 1.0f);
public MaterialParam<float> parameter_ZOverDrawMode = new MaterialParam<float>(ZOVERDRAWMODE, 1.0f);
public MaterialParam<float> parameter_SPRDefaultUnlitPathColorMask = new MaterialParam<float>(SPRDEFAULTUNLITCOLORMASK, 1.0f);
public MaterialParam<float> parameter_SPRDefaultUnlitCullMode = new MaterialParam<float>(SRPDEFAULTUNLITCOLMODE, 1.0f);
public MaterialTextureParam parameter_ClippingMask = new MaterialTextureParam(CLIPPINGMASK);
public MaterialParam<float> parameter_IsBaseMapAlphaAsClippingMask = new MaterialParam<float>(ISBASEMAPALPHAASCLIPPINGMASK, 1.0f);
public MaterialParam<float> parameter_Inverse_Clipping = new MaterialParam<float>(INVERSECLIPPING, 1.0f);
public MaterialParam<float> parameter_Clipping_Level = new MaterialParam<float>(CLIPPINGLEVEL, 1.0f);
public MaterialParam<float> parameter_Tweak_transparency = new MaterialParam<float>(TWEAKTRANSPARENCY, 1.0f);
public MaterialTextureParam parameter_BaseMap = new MaterialTextureParam(MAINTEX);
public MaterialParam<Color> parameter_BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialParam<Color> parameter_Color = new MaterialParam<Color>(COLOR, Color.white);
public MaterialParam<float> parameter_Is_LightColor_Base = new MaterialParam<float>(ISLIGHTCOLORBASE, 1.0f);
public MaterialTextureParam parameter_1st_ShadeMap = new MaterialTextureParam(FIRSTSHADEMAP);
public MaterialParam<float> parameter_UseBaseMapas1st_ShadeMap = new MaterialParam<float>(USEBASEASFIRST, 1.0f);
public MaterialParam<Color> parameter_1st_ShadeColor = new MaterialParam<Color>(FIRSTSHADECOLOR, Color.white);
public MaterialParam<float> parameter_Is_LightColor_1st_Shade = new MaterialParam<float>(ISLIGHTCOLORFIRSTSHADE, 1.0f);
public MaterialTextureParam parameter_2nd_ShadeMap = new MaterialTextureParam(SECONDSHADEMAP);
public MaterialParam<float> parameter_Use1st_ShadeMapas2nd_ShadeMap = new MaterialParam<float>(USEFIRSTASSECOND, 1.0f);
public MaterialParam<Color> parameter_2nd_ShadeColor = new MaterialParam<Color>(SECONDSHADECOLOR, Color.white);
public MaterialParam<float> parameter_Is_LightColor_2nd_Shade = new MaterialParam<float>(ISLIGHTCOLORSECONDSHADE, 1.0f);
public MaterialTextureParam parameter_NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialParam<float> parameter_NormalScale = new MaterialParam<float>(BUMPSCALE, 1.0f);
public MaterialParam<float> parameter_Is_NormalMapToBase = new MaterialParam<float>(ISNORMALMAPTOBASE, 1.0f);
public MaterialParam<float> parameter_Set_SystemShadowsToBase = new MaterialParam<float>(SETSYSTEMSHADOWSTOBASE, 1.0f);
public MaterialParam<float> parameter_Tweak_SystemShadowsLevel = new MaterialParam<float>(TWEAKSYSTEMSHADOWSLEVEL, 1.0f);
public MaterialParam<float> parameter_BaseColor_Step = new MaterialParam<float>(BASECOLORSTEP, 1.0f);
public MaterialParam<float> parameter_BaseShade_Feather = new MaterialParam<float>(BASESHADEFEATHER, 1.0f);
public MaterialParam<float> parameter_ShadeColor_Step = new MaterialParam<float>(SHADECOLORSTEP, 1.0f);
public MaterialParam<float> parameter_1st2nd_Shades_Feather = new MaterialParam<float>(FIRSTSECONDSHADESFEATHER, 1.0f);
public MaterialParam<float> parameter_1st_ShadeColor_Step = new MaterialParam<float>(FIRSTSHADECOLORSTEP, 1.0f);
public MaterialParam<float> parameter_1st_ShadeColor_Feather = new MaterialParam<float>(FIRSTSHADECOLORFEATHER, 1.0f);
public MaterialParam<float> parameter_2nd_ShadeColor_Step = new MaterialParam<float>(SECONDSHADECOLORSTEP, 1.0f);
public MaterialParam<float> parameter_2nd_ShadeColor_Feather = new MaterialParam<float>(SECONDSHADECOLORFEATHER, 1.0f);
public MaterialParam<float> parameter_Step_OffsetForwardAddOnly = new MaterialParam<float>(STEPOFFSET, 1.0f);
public MaterialParam<float> parameter_PointLightsHiCut_FilterForwardAddOnly = new MaterialParam<float>(ISFILTERHICUTPOINTLIGHTCOLOR, 1.0f);
public MaterialTextureParam parameter_Set_1st_ShadePosition = new MaterialTextureParam(SETFIRSTSHADEPOSITION);
public MaterialTextureParam parameter_Set_2nd_ShadePosition = new MaterialTextureParam(SETSECONDSHADEPOSITION);
public MaterialTextureParam parameter_ShadingGradeMap = new MaterialTextureParam(SHADINGGRADEMAP);
public MaterialParam<float> parameter_Tweak_ShadingGradeMapLevel = new MaterialParam<float>(TWEAKSHADINGGRADEMAPLEVEL, 1.0f);
public MaterialParam<float> parameter_BlurLevelofShadingGradeMap = new MaterialParam<float>(BLURLEVELSGM, 1.0f);
public MaterialParam<Color> parameter_HighColor = new MaterialParam<Color>(HIGHCOLOR, Color.white);
public MaterialTextureParam parameter_HighColor_Tex = new MaterialTextureParam(HIGHCOLORTEX);
public MaterialParam<float> parameter_Is_LightColor_HighColor = new MaterialParam<float>(ISLIGHTCOLORHIGHCOLOR, 1.0f);
public MaterialParam<float> parameter_Is_NormalMapToHighColor = new MaterialParam<float>(ISNORMALMAPTOHIGHCOLOR, 1.0f);
public MaterialParam<float> parameter_HighColor_Power = new MaterialParam<float>(HIGHCOLORPOWER, 1.0f);
public MaterialParam<float> parameter_Is_SpecularToHighColor = new MaterialParam<float>(ISSPECULARTOHIGHCOLOR, 1.0f);
public MaterialParam<float> parameter_Is_BlendAddToHiColor = new MaterialParam<float>(ISBLENDADDTOHICOLOR, 1.0f);
public MaterialParam<float> parameter_Is_UseTweakHighColorOnShadow = new MaterialParam<float>(ISUSETWEAKHIGHCOLORONSHADOW, 1.0f);
public MaterialParam<float> parameter_TweakHighColorOnShadow = new MaterialParam<float>(TWEAKHIGHCOLORONSHADOW, 1.0f);
public MaterialTextureParam parameter_Set_HighColorMask = new MaterialTextureParam(SETHIGHCOLORMASK);
public MaterialParam<float> parameter_Tweak_HighColorMaskLevel = new MaterialParam<float>(TWEAKHIGHCOLORMASKLEVEL, 1.0f);
public MaterialParam<float> parameter_RimLight = new MaterialParam<float>(RIMLIGHT, 1.0f);
public MaterialParam<Color> parameter_RimLightColor = new MaterialParam<Color>(RIMLIGHTCOLOR, Color.white);
public MaterialParam<float> parameter_Is_LightColor_RimLight = new MaterialParam<float>(ISLIGHTCOLORRIMLIGHT, 1.0f);
public MaterialParam<float> parameter_Is_NormalMapToRimLight = new MaterialParam<float>(ISNORMALMAPTORIMLIGHT, 1.0f);
public MaterialParam<float> parameter_RimLight_Power = new MaterialParam<float>(RIMLIGHTPOWER, 1.0f);
public MaterialParam<float> parameter_RimLight_InsideMask = new MaterialParam<float>(RIMLIGHTINSIDEMASK, 1.0f);
public MaterialParam<float> parameter_RimLight_FeatherOff = new MaterialParam<float>(RIMLIGHTFEATHEROFF, 1.0f);
public MaterialParam<float> parameter_LightDirection_MaskOn = new MaterialParam<float>(LIGHTDIRECTIONMASKON, 1.0f);
public MaterialParam<float> parameter_Tweak_LightDirection_MaskLevel = new MaterialParam<float>(TWEAKLIGHTDIRECTIONMASKLEVEL, 1.0f);
public MaterialParam<float> parameter_Add_Antipodean_RimLight = new MaterialParam<float>(ADDANTIPODEANRIMLIGHT, 1.0f);
public MaterialParam<Color> parameter_Ap_RimLightColor = new MaterialParam<Color>(APRIMLIGHTCOLOR, Color.white);
public MaterialParam<float> parameter_Is_LightColor_Ap_RimLight = new MaterialParam<float>(ISLIGHTCOLORAPRIMLIGHT, 1.0f);
public MaterialParam<float> parameter_Ap_RimLight_Power = new MaterialParam<float>(APRIMLIGHTPOWER, 1.0f);
public MaterialParam<float> parameter_Ap_RimLight_FeatherOff = new MaterialParam<float>(APRIMLIGHTFEATHEROFF, 1.0f);
public MaterialTextureParam parameter_Set_RimLightMask = new MaterialTextureParam(SETRIMLIGHTMASK);
public MaterialParam<float> parameter_Tweak_RimLightMaskLevel = new MaterialParam<float>(TWEAKRIMLIGHTMASKLEVEL, 1.0f);
public MaterialParam<float> parameter_MatCap = new MaterialParam<float>(MATCAP, 1.0f);
public MaterialTextureParam parameter_MatCap_Sampler = new MaterialTextureParam(MATCAPSAMPLER);
public MaterialParam<float> parameter_BlurLevelofMatCap_Sampler = new MaterialParam<float>(BLURLEVELMATCAP, 1.0f);
public MaterialParam<Color> parameter_MatCapColor = new MaterialParam<Color>(MATCAPCOLOR, Color.white);
public MaterialParam<float> parameter_Is_LightColor_MatCap = new MaterialParam<float>(ISLIGHTCOLORMATCAP, 1.0f);
public MaterialParam<float> parameter_Is_BlendAddToMatCap = new MaterialParam<float>(ISBLENDADDTOMATCAP, 1.0f);
public MaterialParam<float> parameter_Tweak_MatCapUV = new MaterialParam<float>(TWEAKMATCAPUV, 1.0f);
public MaterialParam<float> parameter_Rotate_MatCapUV = new MaterialParam<float>(ROTATEMATCAPUV, 1.0f);
public MaterialParam<float> parameter_ActivateCameraRolling_Stabilizer = new MaterialParam<float>(CAMERAROLLINGSTABILIZER, 1.0f);
public MaterialParam<float> parameter_Is_NormalMapForMatCap = new MaterialParam<float>(ISNORMALMAPFORMATCAP, 1.0f);
public MaterialTextureParam parameter_NormalMapForMatCap = new MaterialTextureParam(NORMALMAPFORMATCAP);
public MaterialParam<float> parameter_ScaleforNormalMapforMatCap = new MaterialParam<float>(BUMPSCALEMATCAP, 1.0f);
public MaterialParam<float> parameter_Rotate_NormalMapForMatCapUV = new MaterialParam<float>(ROTATENORMALMAPFORMATCAPUV, 1.0f);
public MaterialParam<float> parameter_Is_UseTweakMatCapOnShadow = new MaterialParam<float>(ISUSETWEAKMATCAPONSHADOW, 1.0f);
public MaterialParam<float> parameter_TweakMatCapOnShadow = new MaterialParam<float>(TWEAKMATCAPONSHADOW, 1.0f);
public MaterialTextureParam parameter_Set_MatcapMask = new MaterialTextureParam(SETMATCAPMASK);
public MaterialParam<float> parameter_Tweak_MatcapMaskLevel = new MaterialParam<float>(TWEAKMATCAPMASKLEVEL, 1.0f);
public MaterialParam<float> parameter_Inverse_MatcapMask = new MaterialParam<float>(INVERSEMATCAPMASK, 1.0f);
public MaterialParam<float> parameter_OrthographicProjectionforMatCap = new MaterialParam<float>(ISORTHO, 1.0f);
public MaterialParam<float> parameter_AngelRing = new MaterialParam<float>(ANGELRING, 1.0f);
public MaterialTextureParam parameter_AngelRing_Sampler = new MaterialTextureParam(ANGELRINGSAMPLER);
public MaterialParam<Color> parameter_AngelRing_Color = new MaterialParam<Color>(ANGELRINGCOLOR, Color.white);
public MaterialParam<float> parameter_Is_LightColor_AR = new MaterialParam<float>(ISLIGHTCOLORAR, 1.0f);
public MaterialParam<float> parameter_AR_OffsetU = new MaterialParam<float>(AROFFSETU, 1.0f);
public MaterialParam<float> parameter_AR_OffsetV = new MaterialParam<float>(AROFFSETV, 1.0f);
public MaterialParam<float> parameter_ARSampler_AlphaOn = new MaterialParam<float>(ARSAMPLERALPHAON, 1.0f);
public MaterialParam<float> parameter_EMISSIVEMODE = new MaterialParam<float>(EMISSIVE, 1.0f);
public MaterialTextureParam parameter_Emissive_Tex = new MaterialTextureParam(EMISSIVETEX);
public MaterialParam<Color> parameter_Emissive_Color = new MaterialParam<Color>(EMISSIVECOLOR, Color.white);
public MaterialParam<float> parameter_Base_Speed = new MaterialParam<float>(BASESPEED, 1.0f);
public MaterialParam<float> parameter_Scroll_EmissiveU = new MaterialParam<float>(SCROLLEMISSIVEU, 1.0f);
public MaterialParam<float> parameter_Scroll_EmissiveV = new MaterialParam<float>(SCROLLEMISSIVEV, 1.0f);
public MaterialParam<float> parameter_Rotate_EmissiveUV = new MaterialParam<float>(ROTATEEMISSIVEUV, 1.0f);
public MaterialParam<float> parameter_Is_PingPong_Base = new MaterialParam<float>(ISPINGPONGBASE, 1.0f);
public MaterialParam<float> parameter_ActivateColorShift = new MaterialParam<float>(ISCOLORSHIFT, 1.0f);
public MaterialParam<Color> parameter_ColorSift = new MaterialParam<Color>(COLORSHIFT, Color.white);
public MaterialParam<float> parameter_ColorShift_Speed = new MaterialParam<float>(COLORSHIFTSPEED, 1.0f);
public MaterialParam<float> parameter_ActivateViewShift = new MaterialParam<float>(ISVIEWSHIFT, 1.0f);
public MaterialParam<Color> parameter_ViewSift = new MaterialParam<Color>(VIEWSHIFT, Color.white);
public MaterialParam<float> parameter_Is_ViewCoord_Scroll = new MaterialParam<float>(ISVIEWCOORDSCROLL, 1.0f);
public MaterialParam<float> parameter_OUTLINEMODE = new MaterialParam<float>(OUTLINE, 1.0f);
public MaterialParam<float> parameter_Outline_Width = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
public MaterialParam<float> parameter_Farthest_Distance = new MaterialParam<float>(FARTHESTDISTANCE, 1.0f);
public MaterialParam<float> parameter_Nearest_Distance = new MaterialParam<float>(NEARESTDISTANCE, 1.0f);
public MaterialTextureParam parameter_Outline_Sampler = new MaterialTextureParam(OUTLINESAMPLER);
public MaterialParam<Color> parameter_Outline_Color = new MaterialParam<Color>(OUTLINECOLOR, Color.white);
public MaterialParam<float> parameter_Is_BlendBaseColor = new MaterialParam<float>(ISBLENDBASECOLOR, 1.0f);
public MaterialParam<float> parameter_Is_LightColor_Outline = new MaterialParam<float>(ISLIGHTCOLOROUTLINE, 1.0f);
public MaterialParam<float> parameter_Alphacutoff = new MaterialParam<float>(CUTOFF, 1.0f);
public MaterialParam<float> parameter_Is_OutlineTex = new MaterialParam<float>(ISOUTLINETEX, 1.0f);
public MaterialTextureParam parameter_OutlineTex = new MaterialTextureParam(OUTLINETEX);
public MaterialParam<float> parameter_Offset_Camera_Z = new MaterialParam<float>(OFFSETZ, 1.0f);
public MaterialParam<float> parameter_Is_BakedNormal = new MaterialParam<float>(ISBAKEDNORMAL, 1.0f);
public MaterialTextureParam parameter_BakedNormalforOutline = new MaterialTextureParam(BAKEDNORMAL);
public MaterialParam<float> parameter_GI_Intensity = new MaterialParam<float>(GIINTENSITY, 1.0f);
public MaterialParam<float> parameter_Unlit_Intensity = new MaterialParam<float>(UNLITINTENSITY, 1.0f);
public MaterialParam<float> parameter_VRChatSceneLightsHiCut_Filter = new MaterialParam<float>(ISFILTERLIGHTCOLOR, 1.0f);
public MaterialParam<float> parameter_AdvancedActivateBuiltinLightDirection = new MaterialParam<float>(ISBLD, 1.0f);
public MaterialParam<float> parameter_OffsetXAxisBuiltinLightDirection = new MaterialParam<float>(OFFSETXAXISBLD, 1.0f);
public MaterialParam<float> parameter_OffsetYAxisBuiltinLightDirection = new MaterialParam<float>(OFFSETYAXISBLD, 1.0f);
public MaterialParam<float> parameter_InverseZAxisBuiltinLightDirection = new MaterialParam<float>(INVERSEZAXISBLD, 1.0f);
public BVA_Material_UTS_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_SimpleUI.Value = material.GetFloat(parameter_SimpleUI.ParamName);
parameter_VersionX.Value = material.GetFloat(parameter_VersionX.ParamName);
parameter_VersionY.Value = material.GetFloat(parameter_VersionY.ParamName);
parameter_VersionZ.Value = material.GetFloat(parameter_VersionZ.ParamName);
parameter_Technique.Value = material.GetFloat(parameter_Technique.ParamName);
parameter_AutomaticRenderQueue.Value = material.GetFloat(parameter_AutomaticRenderQueue.ParamName);
parameter_StencilMode.Value = material.GetFloat(parameter_StencilMode.ParamName);
parameter_StencilComparison.Value = material.GetFloat(parameter_StencilComparison.ParamName);
parameter_StencilNo.Value = material.GetFloat(parameter_StencilNo.ParamName);
parameter_StencilOperationPass.Value = material.GetFloat(parameter_StencilOperationPass.ParamName);
parameter_StencilOperationFail.Value = material.GetFloat(parameter_StencilOperationFail.ParamName);
parameter_TransparentMode.Value = material.GetFloat(parameter_TransparentMode.ParamName);
parameter__Metallic.Value = material.GetFloat(parameter__Metallic.ParamName);
parameter_Smoothness.Value = material.GetFloat(parameter_Smoothness.ParamName);
parameter__SpecColor.Value = material.GetColor(parameter__SpecColor.ParamName);
parameter_CliippingMode.Value = material.GetFloat(parameter_CliippingMode.ParamName);
parameter_CullMode.Value = material.GetFloat(parameter_CullMode.ParamName);
parameter_ZWriteMode.Value = material.GetFloat(parameter_ZWriteMode.ParamName);
parameter_ZOverDrawMode.Value = material.GetFloat(parameter_ZOverDrawMode.ParamName);
parameter_SPRDefaultUnlitPathColorMask.Value = material.GetFloat(parameter_SPRDefaultUnlitPathColorMask.ParamName);
parameter_SPRDefaultUnlitCullMode.Value = material.GetFloat(parameter_SPRDefaultUnlitCullMode.ParamName);
var parameter_clippingmask_temp = material.GetTexture(parameter_ClippingMask.ParamName);
if (parameter_clippingmask_temp != null) parameter_ClippingMask.Value = exportTextureInfo(parameter_clippingmask_temp);
parameter_IsBaseMapAlphaAsClippingMask.Value = material.GetFloat(parameter_IsBaseMapAlphaAsClippingMask.ParamName);
parameter_Inverse_Clipping.Value = material.GetFloat(parameter_Inverse_Clipping.ParamName);
parameter_Clipping_Level.Value = material.GetFloat(parameter_Clipping_Level.ParamName);
parameter_Tweak_transparency.Value = material.GetFloat(parameter_Tweak_transparency.ParamName);
var parameter_basemap_temp = material.GetTexture(parameter_BaseMap.ParamName);
if (parameter_basemap_temp != null) parameter_BaseMap.Value = exportTextureInfo(parameter_basemap_temp);
parameter_BaseColor.Value = material.GetColor(parameter_BaseColor.ParamName);
parameter_Color.Value = material.GetColor(parameter_Color.ParamName);
parameter_Is_LightColor_Base.Value = material.GetFloat(parameter_Is_LightColor_Base.ParamName);
var parameter_1st_shademap_temp = material.GetTexture(parameter_1st_ShadeMap.ParamName);
if (parameter_1st_shademap_temp != null) parameter_1st_ShadeMap.Value = exportTextureInfo(parameter_1st_shademap_temp);
parameter_UseBaseMapas1st_ShadeMap.Value = material.GetFloat(parameter_UseBaseMapas1st_ShadeMap.ParamName);
parameter_1st_ShadeColor.Value = material.GetColor(parameter_1st_ShadeColor.ParamName);
parameter_Is_LightColor_1st_Shade.Value = material.GetFloat(parameter_Is_LightColor_1st_Shade.ParamName);
var parameter_2nd_shademap_temp = material.GetTexture(parameter_2nd_ShadeMap.ParamName);
if (parameter_2nd_shademap_temp != null) parameter_2nd_ShadeMap.Value = exportTextureInfo(parameter_2nd_shademap_temp);
parameter_Use1st_ShadeMapas2nd_ShadeMap.Value = material.GetFloat(parameter_Use1st_ShadeMapas2nd_ShadeMap.ParamName);
parameter_2nd_ShadeColor.Value = material.GetColor(parameter_2nd_ShadeColor.ParamName);
parameter_Is_LightColor_2nd_Shade.Value = material.GetFloat(parameter_Is_LightColor_2nd_Shade.ParamName);
var parameter_normalmap_temp = material.GetTexture(parameter_NormalMap.ParamName);
if (parameter_normalmap_temp != null) parameter_NormalMap.Value = exportTextureInfo(parameter_normalmap_temp);
parameter_NormalScale.Value = material.GetFloat(parameter_NormalScale.ParamName);
parameter_Is_NormalMapToBase.Value = material.GetFloat(parameter_Is_NormalMapToBase.ParamName);
parameter_Set_SystemShadowsToBase.Value = material.GetFloat(parameter_Set_SystemShadowsToBase.ParamName);
parameter_Tweak_SystemShadowsLevel.Value = material.GetFloat(parameter_Tweak_SystemShadowsLevel.ParamName);
parameter_BaseColor_Step.Value = material.GetFloat(parameter_BaseColor_Step.ParamName);
parameter_BaseShade_Feather.Value = material.GetFloat(parameter_BaseShade_Feather.ParamName);
parameter_ShadeColor_Step.Value = material.GetFloat(parameter_ShadeColor_Step.ParamName);
parameter_1st2nd_Shades_Feather.Value = material.GetFloat(parameter_1st2nd_Shades_Feather.ParamName);
parameter_1st_ShadeColor_Step.Value = material.GetFloat(parameter_1st_ShadeColor_Step.ParamName);
parameter_1st_ShadeColor_Feather.Value = material.GetFloat(parameter_1st_ShadeColor_Feather.ParamName);
parameter_2nd_ShadeColor_Step.Value = material.GetFloat(parameter_2nd_ShadeColor_Step.ParamName);
parameter_2nd_ShadeColor_Feather.Value = material.GetFloat(parameter_2nd_ShadeColor_Feather.ParamName);
parameter_Step_OffsetForwardAddOnly.Value = material.GetFloat(parameter_Step_OffsetForwardAddOnly.ParamName);
parameter_PointLightsHiCut_FilterForwardAddOnly.Value = material.GetFloat(parameter_PointLightsHiCut_FilterForwardAddOnly.ParamName);
var parameter_set_1st_shadeposition_temp = material.GetTexture(parameter_Set_1st_ShadePosition.ParamName);
if (parameter_set_1st_shadeposition_temp != null) parameter_Set_1st_ShadePosition.Value = exportTextureInfo(parameter_set_1st_shadeposition_temp);
var parameter_set_2nd_shadeposition_temp = material.GetTexture(parameter_Set_2nd_ShadePosition.ParamName);
if (parameter_set_2nd_shadeposition_temp != null) parameter_Set_2nd_ShadePosition.Value = exportTextureInfo(parameter_set_2nd_shadeposition_temp);
var parameter_shadinggrademap_temp = material.GetTexture(parameter_ShadingGradeMap.ParamName);
if (parameter_shadinggrademap_temp != null) parameter_ShadingGradeMap.Value = exportTextureInfo(parameter_shadinggrademap_temp);
parameter_Tweak_ShadingGradeMapLevel.Value = material.GetFloat(parameter_Tweak_ShadingGradeMapLevel.ParamName);
parameter_BlurLevelofShadingGradeMap.Value = material.GetFloat(parameter_BlurLevelofShadingGradeMap.ParamName);
parameter_HighColor.Value = material.GetColor(parameter_HighColor.ParamName);
var parameter_highcolor_tex_temp = material.GetTexture(parameter_HighColor_Tex.ParamName);
if (parameter_highcolor_tex_temp != null) parameter_HighColor_Tex.Value = exportTextureInfo(parameter_highcolor_tex_temp);
parameter_Is_LightColor_HighColor.Value = material.GetFloat(parameter_Is_LightColor_HighColor.ParamName);
parameter_Is_NormalMapToHighColor.Value = material.GetFloat(parameter_Is_NormalMapToHighColor.ParamName);
parameter_HighColor_Power.Value = material.GetFloat(parameter_HighColor_Power.ParamName);
parameter_Is_SpecularToHighColor.Value = material.GetFloat(parameter_Is_SpecularToHighColor.ParamName);
parameter_Is_BlendAddToHiColor.Value = material.GetFloat(parameter_Is_BlendAddToHiColor.ParamName);
parameter_Is_UseTweakHighColorOnShadow.Value = material.GetFloat(parameter_Is_UseTweakHighColorOnShadow.ParamName);
parameter_TweakHighColorOnShadow.Value = material.GetFloat(parameter_TweakHighColorOnShadow.ParamName);
var parameter_set_highcolormask_temp = material.GetTexture(parameter_Set_HighColorMask.ParamName);
if (parameter_set_highcolormask_temp != null) parameter_Set_HighColorMask.Value = exportTextureInfo(parameter_set_highcolormask_temp);
parameter_Tweak_HighColorMaskLevel.Value = material.GetFloat(parameter_Tweak_HighColorMaskLevel.ParamName);
parameter_RimLight.Value = material.GetFloat(parameter_RimLight.ParamName);
parameter_RimLightColor.Value = material.GetColor(parameter_RimLightColor.ParamName);
parameter_Is_LightColor_RimLight.Value = material.GetFloat(parameter_Is_LightColor_RimLight.ParamName);
parameter_Is_NormalMapToRimLight.Value = material.GetFloat(parameter_Is_NormalMapToRimLight.ParamName);
parameter_RimLight_Power.Value = material.GetFloat(parameter_RimLight_Power.ParamName);
parameter_RimLight_InsideMask.Value = material.GetFloat(parameter_RimLight_InsideMask.ParamName);
parameter_RimLight_FeatherOff.Value = material.GetFloat(parameter_RimLight_FeatherOff.ParamName);
parameter_LightDirection_MaskOn.Value = material.GetFloat(parameter_LightDirection_MaskOn.ParamName);
parameter_Tweak_LightDirection_MaskLevel.Value = material.GetFloat(parameter_Tweak_LightDirection_MaskLevel.ParamName);
parameter_Add_Antipodean_RimLight.Value = material.GetFloat(parameter_Add_Antipodean_RimLight.ParamName);
parameter_Ap_RimLightColor.Value = material.GetColor(parameter_Ap_RimLightColor.ParamName);
parameter_Is_LightColor_Ap_RimLight.Value = material.GetFloat(parameter_Is_LightColor_Ap_RimLight.ParamName);
parameter_Ap_RimLight_Power.Value = material.GetFloat(parameter_Ap_RimLight_Power.ParamName);
parameter_Ap_RimLight_FeatherOff.Value = material.GetFloat(parameter_Ap_RimLight_FeatherOff.ParamName);
var parameter_set_rimlightmask_temp = material.GetTexture(parameter_Set_RimLightMask.ParamName);
if (parameter_set_rimlightmask_temp != null) parameter_Set_RimLightMask.Value = exportTextureInfo(parameter_set_rimlightmask_temp);
parameter_Tweak_RimLightMaskLevel.Value = material.GetFloat(parameter_Tweak_RimLightMaskLevel.ParamName);
parameter_MatCap.Value = material.GetFloat(parameter_MatCap.ParamName);
var parameter_matcap_sampler_temp = material.GetTexture(parameter_MatCap_Sampler.ParamName);
if (parameter_matcap_sampler_temp != null) parameter_MatCap_Sampler.Value = exportTextureInfo(parameter_matcap_sampler_temp);
parameter_BlurLevelofMatCap_Sampler.Value = material.GetFloat(parameter_BlurLevelofMatCap_Sampler.ParamName);
parameter_MatCapColor.Value = material.GetColor(parameter_MatCapColor.ParamName);
parameter_Is_LightColor_MatCap.Value = material.GetFloat(parameter_Is_LightColor_MatCap.ParamName);
parameter_Is_BlendAddToMatCap.Value = material.GetFloat(parameter_Is_BlendAddToMatCap.ParamName);
parameter_Tweak_MatCapUV.Value = material.GetFloat(parameter_Tweak_MatCapUV.ParamName);
parameter_Rotate_MatCapUV.Value = material.GetFloat(parameter_Rotate_MatCapUV.ParamName);
parameter_ActivateCameraRolling_Stabilizer.Value = material.GetFloat(parameter_ActivateCameraRolling_Stabilizer.ParamName);
parameter_Is_NormalMapForMatCap.Value = material.GetFloat(parameter_Is_NormalMapForMatCap.ParamName);
var parameter_normalmapformatcap_temp = material.GetTexture(parameter_NormalMapForMatCap.ParamName);
if (parameter_normalmapformatcap_temp != null) parameter_NormalMapForMatCap.Value = exportTextureInfo(parameter_normalmapformatcap_temp);
parameter_ScaleforNormalMapforMatCap.Value = material.GetFloat(parameter_ScaleforNormalMapforMatCap.ParamName);
parameter_Rotate_NormalMapForMatCapUV.Value = material.GetFloat(parameter_Rotate_NormalMapForMatCapUV.ParamName);
parameter_Is_UseTweakMatCapOnShadow.Value = material.GetFloat(parameter_Is_UseTweakMatCapOnShadow.ParamName);
parameter_TweakMatCapOnShadow.Value = material.GetFloat(parameter_TweakMatCapOnShadow.ParamName);
var parameter_set_matcapmask_temp = material.GetTexture(parameter_Set_MatcapMask.ParamName);
if (parameter_set_matcapmask_temp != null) parameter_Set_MatcapMask.Value = exportTextureInfo(parameter_set_matcapmask_temp);
parameter_Tweak_MatcapMaskLevel.Value = material.GetFloat(parameter_Tweak_MatcapMaskLevel.ParamName);
parameter_Inverse_MatcapMask.Value = material.GetFloat(parameter_Inverse_MatcapMask.ParamName);
parameter_OrthographicProjectionforMatCap.Value = material.GetFloat(parameter_OrthographicProjectionforMatCap.ParamName);
parameter_AngelRing.Value = material.GetFloat(parameter_AngelRing.ParamName);
var parameter_angelring_sampler_temp = material.GetTexture(parameter_AngelRing_Sampler.ParamName);
if (parameter_angelring_sampler_temp != null) parameter_AngelRing_Sampler.Value = exportTextureInfo(parameter_angelring_sampler_temp);
parameter_AngelRing_Color.Value = material.GetColor(parameter_AngelRing_Color.ParamName);
parameter_Is_LightColor_AR.Value = material.GetFloat(parameter_Is_LightColor_AR.ParamName);
parameter_AR_OffsetU.Value = material.GetFloat(parameter_AR_OffsetU.ParamName);
parameter_AR_OffsetV.Value = material.GetFloat(parameter_AR_OffsetV.ParamName);
parameter_ARSampler_AlphaOn.Value = material.GetFloat(parameter_ARSampler_AlphaOn.ParamName);
parameter_EMISSIVEMODE.Value = material.GetFloat(parameter_EMISSIVEMODE.ParamName);
var parameter_emissive_tex_temp = material.GetTexture(parameter_Emissive_Tex.ParamName);
if (parameter_emissive_tex_temp != null) parameter_Emissive_Tex.Value = exportTextureInfo(parameter_emissive_tex_temp);
parameter_Emissive_Color.Value = material.GetColor(parameter_Emissive_Color.ParamName);
parameter_Base_Speed.Value = material.GetFloat(parameter_Base_Speed.ParamName);
parameter_Scroll_EmissiveU.Value = material.GetFloat(parameter_Scroll_EmissiveU.ParamName);
parameter_Scroll_EmissiveV.Value = material.GetFloat(parameter_Scroll_EmissiveV.ParamName);
parameter_Rotate_EmissiveUV.Value = material.GetFloat(parameter_Rotate_EmissiveUV.ParamName);
parameter_Is_PingPong_Base.Value = material.GetFloat(parameter_Is_PingPong_Base.ParamName);
parameter_ActivateColorShift.Value = material.GetFloat(parameter_ActivateColorShift.ParamName);
parameter_ColorSift.Value = material.GetColor(parameter_ColorSift.ParamName);
parameter_ColorShift_Speed.Value = material.GetFloat(parameter_ColorShift_Speed.ParamName);
parameter_ActivateViewShift.Value = material.GetFloat(parameter_ActivateViewShift.ParamName);
parameter_ViewSift.Value = material.GetColor(parameter_ViewSift.ParamName);
parameter_Is_ViewCoord_Scroll.Value = material.GetFloat(parameter_Is_ViewCoord_Scroll.ParamName);
parameter_OUTLINEMODE.Value = material.GetFloat(parameter_OUTLINEMODE.ParamName);
parameter_Outline_Width.Value = material.GetFloat(parameter_Outline_Width.ParamName);
parameter_Farthest_Distance.Value = material.GetFloat(parameter_Farthest_Distance.ParamName);
parameter_Nearest_Distance.Value = material.GetFloat(parameter_Nearest_Distance.ParamName);
var parameter_outline_sampler_temp = material.GetTexture(parameter_Outline_Sampler.ParamName);
if (parameter_outline_sampler_temp != null) parameter_Outline_Sampler.Value = exportTextureInfo(parameter_outline_sampler_temp);
parameter_Outline_Color.Value = material.GetColor(parameter_Outline_Color.ParamName);
parameter_Is_BlendBaseColor.Value = material.GetFloat(parameter_Is_BlendBaseColor.ParamName);
parameter_Is_LightColor_Outline.Value = material.GetFloat(parameter_Is_LightColor_Outline.ParamName);
parameter_Alphacutoff.Value = material.GetFloat(parameter_Alphacutoff.ParamName);
parameter_Is_OutlineTex.Value = material.GetFloat(parameter_Is_OutlineTex.ParamName);
var parameter_outlinetex_temp = material.GetTexture(parameter_OutlineTex.ParamName);
if (parameter_outlinetex_temp != null) parameter_OutlineTex.Value = exportTextureInfo(parameter_outlinetex_temp);
parameter_Offset_Camera_Z.Value = material.GetFloat(parameter_Offset_Camera_Z.ParamName);
parameter_Is_BakedNormal.Value = material.GetFloat(parameter_Is_BakedNormal.ParamName);
var parameter_bakednormalforoutline_temp = material.GetTexture(parameter_BakedNormalforOutline.ParamName);
if (parameter_bakednormalforoutline_temp != null) parameter_BakedNormalforOutline.Value = exportTextureInfo(parameter_bakednormalforoutline_temp);
parameter_GI_Intensity.Value = material.GetFloat(parameter_GI_Intensity.ParamName);
parameter_Unlit_Intensity.Value = material.GetFloat(parameter_Unlit_Intensity.ParamName);
parameter_VRChatSceneLightsHiCut_Filter.Value = material.GetFloat(parameter_VRChatSceneLightsHiCut_Filter.ParamName);
parameter_AdvancedActivateBuiltinLightDirection.Value = material.GetFloat(parameter_AdvancedActivateBuiltinLightDirection.ParamName);
parameter_OffsetXAxisBuiltinLightDirection.Value = material.GetFloat(parameter_OffsetXAxisBuiltinLightDirection.ParamName);
parameter_OffsetYAxisBuiltinLightDirection.Value = material.GetFloat(parameter_OffsetYAxisBuiltinLightDirection.ParamName);
parameter_InverseZAxisBuiltinLightDirection.Value = material.GetFloat(parameter_InverseZAxisBuiltinLightDirection.ParamName);
}
public static async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache,AsyncLoadTexture loadTexture,AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
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
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(parameter_SimpleUI.ParamName, parameter_SimpleUI.Value);
jo.Add(parameter_VersionX.ParamName, parameter_VersionX.Value);
jo.Add(parameter_VersionY.ParamName, parameter_VersionY.Value);
jo.Add(parameter_VersionZ.ParamName, parameter_VersionZ.Value);
jo.Add(parameter_Technique.ParamName, parameter_Technique.Value);
jo.Add(parameter_AutomaticRenderQueue.ParamName, parameter_AutomaticRenderQueue.Value);
jo.Add(parameter_StencilMode.ParamName, parameter_StencilMode.Value);
jo.Add(parameter_StencilComparison.ParamName, parameter_StencilComparison.Value);
jo.Add(parameter_StencilNo.ParamName, parameter_StencilNo.Value);
jo.Add(parameter_StencilOperationPass.ParamName, parameter_StencilOperationPass.Value);
jo.Add(parameter_StencilOperationFail.ParamName, parameter_StencilOperationFail.Value);
jo.Add(parameter_TransparentMode.ParamName, parameter_TransparentMode.Value);
jo.Add(parameter__Metallic.ParamName, parameter__Metallic.Value);
jo.Add(parameter_Smoothness.ParamName, parameter_Smoothness.Value);
jo.Add(parameter__SpecColor.ParamName, parameter__SpecColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_CliippingMode.ParamName, parameter_CliippingMode.Value);
jo.Add(parameter_CullMode.ParamName, parameter_CullMode.Value);
jo.Add(parameter_ZWriteMode.ParamName, parameter_ZWriteMode.Value);
jo.Add(parameter_ZOverDrawMode.ParamName, parameter_ZOverDrawMode.Value);
jo.Add(parameter_SPRDefaultUnlitPathColorMask.ParamName, parameter_SPRDefaultUnlitPathColorMask.Value);
jo.Add(parameter_SPRDefaultUnlitCullMode.ParamName, parameter_SPRDefaultUnlitCullMode.Value);
if (parameter_ClippingMask != null && parameter_ClippingMask.Value != null) jo.Add(parameter_ClippingMask.ParamName, parameter_ClippingMask.Serialize());
jo.Add(parameter_IsBaseMapAlphaAsClippingMask.ParamName, parameter_IsBaseMapAlphaAsClippingMask.Value);
jo.Add(parameter_Inverse_Clipping.ParamName, parameter_Inverse_Clipping.Value);
jo.Add(parameter_Clipping_Level.ParamName, parameter_Clipping_Level.Value);
jo.Add(parameter_Tweak_transparency.ParamName, parameter_Tweak_transparency.Value);
if (parameter_BaseMap != null && parameter_BaseMap.Value != null) jo.Add(parameter_BaseMap.ParamName, parameter_BaseMap.Serialize());
jo.Add(parameter_BaseColor.ParamName, parameter_BaseColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Color.ParamName, parameter_Color.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_LightColor_Base.ParamName, parameter_Is_LightColor_Base.Value);
if (parameter_1st_ShadeMap != null && parameter_1st_ShadeMap.Value != null) jo.Add(parameter_1st_ShadeMap.ParamName, parameter_1st_ShadeMap.Serialize());
jo.Add(parameter_UseBaseMapas1st_ShadeMap.ParamName, parameter_UseBaseMapas1st_ShadeMap.Value);
jo.Add(parameter_1st_ShadeColor.ParamName, parameter_1st_ShadeColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_LightColor_1st_Shade.ParamName, parameter_Is_LightColor_1st_Shade.Value);
if (parameter_2nd_ShadeMap != null && parameter_2nd_ShadeMap.Value != null) jo.Add(parameter_2nd_ShadeMap.ParamName, parameter_2nd_ShadeMap.Serialize());
jo.Add(parameter_Use1st_ShadeMapas2nd_ShadeMap.ParamName, parameter_Use1st_ShadeMapas2nd_ShadeMap.Value);
jo.Add(parameter_2nd_ShadeColor.ParamName, parameter_2nd_ShadeColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_LightColor_2nd_Shade.ParamName, parameter_Is_LightColor_2nd_Shade.Value);
if (parameter_NormalMap != null && parameter_NormalMap.Value != null) jo.Add(parameter_NormalMap.ParamName, parameter_NormalMap.Serialize());
jo.Add(parameter_NormalScale.ParamName, parameter_NormalScale.Value);
jo.Add(parameter_Is_NormalMapToBase.ParamName, parameter_Is_NormalMapToBase.Value);
jo.Add(parameter_Set_SystemShadowsToBase.ParamName, parameter_Set_SystemShadowsToBase.Value);
jo.Add(parameter_Tweak_SystemShadowsLevel.ParamName, parameter_Tweak_SystemShadowsLevel.Value);
jo.Add(parameter_BaseColor_Step.ParamName, parameter_BaseColor_Step.Value);
jo.Add(parameter_BaseShade_Feather.ParamName, parameter_BaseShade_Feather.Value);
jo.Add(parameter_ShadeColor_Step.ParamName, parameter_ShadeColor_Step.Value);
jo.Add(parameter_1st2nd_Shades_Feather.ParamName, parameter_1st2nd_Shades_Feather.Value);
jo.Add(parameter_1st_ShadeColor_Step.ParamName, parameter_1st_ShadeColor_Step.Value);
jo.Add(parameter_1st_ShadeColor_Feather.ParamName, parameter_1st_ShadeColor_Feather.Value);
jo.Add(parameter_2nd_ShadeColor_Step.ParamName, parameter_2nd_ShadeColor_Step.Value);
jo.Add(parameter_2nd_ShadeColor_Feather.ParamName, parameter_2nd_ShadeColor_Feather.Value);
jo.Add(parameter_Step_OffsetForwardAddOnly.ParamName, parameter_Step_OffsetForwardAddOnly.Value);
jo.Add(parameter_PointLightsHiCut_FilterForwardAddOnly.ParamName, parameter_PointLightsHiCut_FilterForwardAddOnly.Value);
if (parameter_Set_1st_ShadePosition != null && parameter_Set_1st_ShadePosition.Value != null) jo.Add(parameter_Set_1st_ShadePosition.ParamName, parameter_Set_1st_ShadePosition.Serialize());
if (parameter_Set_2nd_ShadePosition != null && parameter_Set_2nd_ShadePosition.Value != null) jo.Add(parameter_Set_2nd_ShadePosition.ParamName, parameter_Set_2nd_ShadePosition.Serialize());
if (parameter_ShadingGradeMap != null && parameter_ShadingGradeMap.Value != null) jo.Add(parameter_ShadingGradeMap.ParamName, parameter_ShadingGradeMap.Serialize());
jo.Add(parameter_Tweak_ShadingGradeMapLevel.ParamName, parameter_Tweak_ShadingGradeMapLevel.Value);
jo.Add(parameter_BlurLevelofShadingGradeMap.ParamName, parameter_BlurLevelofShadingGradeMap.Value);
jo.Add(parameter_HighColor.ParamName, parameter_HighColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_HighColor_Tex != null && parameter_HighColor_Tex.Value != null) jo.Add(parameter_HighColor_Tex.ParamName, parameter_HighColor_Tex.Serialize());
jo.Add(parameter_Is_LightColor_HighColor.ParamName, parameter_Is_LightColor_HighColor.Value);
jo.Add(parameter_Is_NormalMapToHighColor.ParamName, parameter_Is_NormalMapToHighColor.Value);
jo.Add(parameter_HighColor_Power.ParamName, parameter_HighColor_Power.Value);
jo.Add(parameter_Is_SpecularToHighColor.ParamName, parameter_Is_SpecularToHighColor.Value);
jo.Add(parameter_Is_BlendAddToHiColor.ParamName, parameter_Is_BlendAddToHiColor.Value);
jo.Add(parameter_Is_UseTweakHighColorOnShadow.ParamName, parameter_Is_UseTweakHighColorOnShadow.Value);
jo.Add(parameter_TweakHighColorOnShadow.ParamName, parameter_TweakHighColorOnShadow.Value);
if (parameter_Set_HighColorMask != null && parameter_Set_HighColorMask.Value != null) jo.Add(parameter_Set_HighColorMask.ParamName, parameter_Set_HighColorMask.Serialize());
jo.Add(parameter_Tweak_HighColorMaskLevel.ParamName, parameter_Tweak_HighColorMaskLevel.Value);
jo.Add(parameter_RimLight.ParamName, parameter_RimLight.Value);
jo.Add(parameter_RimLightColor.ParamName, parameter_RimLightColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_LightColor_RimLight.ParamName, parameter_Is_LightColor_RimLight.Value);
jo.Add(parameter_Is_NormalMapToRimLight.ParamName, parameter_Is_NormalMapToRimLight.Value);
jo.Add(parameter_RimLight_Power.ParamName, parameter_RimLight_Power.Value);
jo.Add(parameter_RimLight_InsideMask.ParamName, parameter_RimLight_InsideMask.Value);
jo.Add(parameter_RimLight_FeatherOff.ParamName, parameter_RimLight_FeatherOff.Value);
jo.Add(parameter_LightDirection_MaskOn.ParamName, parameter_LightDirection_MaskOn.Value);
jo.Add(parameter_Tweak_LightDirection_MaskLevel.ParamName, parameter_Tweak_LightDirection_MaskLevel.Value);
jo.Add(parameter_Add_Antipodean_RimLight.ParamName, parameter_Add_Antipodean_RimLight.Value);
jo.Add(parameter_Ap_RimLightColor.ParamName, parameter_Ap_RimLightColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_LightColor_Ap_RimLight.ParamName, parameter_Is_LightColor_Ap_RimLight.Value);
jo.Add(parameter_Ap_RimLight_Power.ParamName, parameter_Ap_RimLight_Power.Value);
jo.Add(parameter_Ap_RimLight_FeatherOff.ParamName, parameter_Ap_RimLight_FeatherOff.Value);
if (parameter_Set_RimLightMask != null && parameter_Set_RimLightMask.Value != null) jo.Add(parameter_Set_RimLightMask.ParamName, parameter_Set_RimLightMask.Serialize());
jo.Add(parameter_Tweak_RimLightMaskLevel.ParamName, parameter_Tweak_RimLightMaskLevel.Value);
jo.Add(parameter_MatCap.ParamName, parameter_MatCap.Value);
if (parameter_MatCap_Sampler != null && parameter_MatCap_Sampler.Value != null) jo.Add(parameter_MatCap_Sampler.ParamName, parameter_MatCap_Sampler.Serialize());
jo.Add(parameter_BlurLevelofMatCap_Sampler.ParamName, parameter_BlurLevelofMatCap_Sampler.Value);
jo.Add(parameter_MatCapColor.ParamName, parameter_MatCapColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_LightColor_MatCap.ParamName, parameter_Is_LightColor_MatCap.Value);
jo.Add(parameter_Is_BlendAddToMatCap.ParamName, parameter_Is_BlendAddToMatCap.Value);
jo.Add(parameter_Tweak_MatCapUV.ParamName, parameter_Tweak_MatCapUV.Value);
jo.Add(parameter_Rotate_MatCapUV.ParamName, parameter_Rotate_MatCapUV.Value);
jo.Add(parameter_ActivateCameraRolling_Stabilizer.ParamName, parameter_ActivateCameraRolling_Stabilizer.Value);
jo.Add(parameter_Is_NormalMapForMatCap.ParamName, parameter_Is_NormalMapForMatCap.Value);
if (parameter_NormalMapForMatCap != null && parameter_NormalMapForMatCap.Value != null) jo.Add(parameter_NormalMapForMatCap.ParamName, parameter_NormalMapForMatCap.Serialize());
jo.Add(parameter_ScaleforNormalMapforMatCap.ParamName, parameter_ScaleforNormalMapforMatCap.Value);
jo.Add(parameter_Rotate_NormalMapForMatCapUV.ParamName, parameter_Rotate_NormalMapForMatCapUV.Value);
jo.Add(parameter_Is_UseTweakMatCapOnShadow.ParamName, parameter_Is_UseTweakMatCapOnShadow.Value);
jo.Add(parameter_TweakMatCapOnShadow.ParamName, parameter_TweakMatCapOnShadow.Value);
if (parameter_Set_MatcapMask != null && parameter_Set_MatcapMask.Value != null) jo.Add(parameter_Set_MatcapMask.ParamName, parameter_Set_MatcapMask.Serialize());
jo.Add(parameter_Tweak_MatcapMaskLevel.ParamName, parameter_Tweak_MatcapMaskLevel.Value);
jo.Add(parameter_Inverse_MatcapMask.ParamName, parameter_Inverse_MatcapMask.Value);
jo.Add(parameter_OrthographicProjectionforMatCap.ParamName, parameter_OrthographicProjectionforMatCap.Value);
jo.Add(parameter_AngelRing.ParamName, parameter_AngelRing.Value);
if (parameter_AngelRing_Sampler != null && parameter_AngelRing_Sampler.Value != null) jo.Add(parameter_AngelRing_Sampler.ParamName, parameter_AngelRing_Sampler.Serialize());
jo.Add(parameter_AngelRing_Color.ParamName, parameter_AngelRing_Color.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_LightColor_AR.ParamName, parameter_Is_LightColor_AR.Value);
jo.Add(parameter_AR_OffsetU.ParamName, parameter_AR_OffsetU.Value);
jo.Add(parameter_AR_OffsetV.ParamName, parameter_AR_OffsetV.Value);
jo.Add(parameter_ARSampler_AlphaOn.ParamName, parameter_ARSampler_AlphaOn.Value);
jo.Add(parameter_EMISSIVEMODE.ParamName, parameter_EMISSIVEMODE.Value);
if (parameter_Emissive_Tex != null && parameter_Emissive_Tex.Value != null) jo.Add(parameter_Emissive_Tex.ParamName, parameter_Emissive_Tex.Serialize());
jo.Add(parameter_Emissive_Color.ParamName, parameter_Emissive_Color.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Base_Speed.ParamName, parameter_Base_Speed.Value);
jo.Add(parameter_Scroll_EmissiveU.ParamName, parameter_Scroll_EmissiveU.Value);
jo.Add(parameter_Scroll_EmissiveV.ParamName, parameter_Scroll_EmissiveV.Value);
jo.Add(parameter_Rotate_EmissiveUV.ParamName, parameter_Rotate_EmissiveUV.Value);
jo.Add(parameter_Is_PingPong_Base.ParamName, parameter_Is_PingPong_Base.Value);
jo.Add(parameter_ActivateColorShift.ParamName, parameter_ActivateColorShift.Value);
jo.Add(parameter_ColorSift.ParamName, parameter_ColorSift.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_ColorShift_Speed.ParamName, parameter_ColorShift_Speed.Value);
jo.Add(parameter_ActivateViewShift.ParamName, parameter_ActivateViewShift.Value);
jo.Add(parameter_ViewSift.ParamName, parameter_ViewSift.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_ViewCoord_Scroll.ParamName, parameter_Is_ViewCoord_Scroll.Value);
jo.Add(parameter_OUTLINEMODE.ParamName, parameter_OUTLINEMODE.Value);
jo.Add(parameter_Outline_Width.ParamName, parameter_Outline_Width.Value);
jo.Add(parameter_Farthest_Distance.ParamName, parameter_Farthest_Distance.Value);
jo.Add(parameter_Nearest_Distance.ParamName, parameter_Nearest_Distance.Value);
if (parameter_Outline_Sampler != null && parameter_Outline_Sampler.Value != null) jo.Add(parameter_Outline_Sampler.ParamName, parameter_Outline_Sampler.Serialize());
jo.Add(parameter_Outline_Color.ParamName, parameter_Outline_Color.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Is_BlendBaseColor.ParamName, parameter_Is_BlendBaseColor.Value);
jo.Add(parameter_Is_LightColor_Outline.ParamName, parameter_Is_LightColor_Outline.Value);
jo.Add(parameter_Alphacutoff.ParamName, parameter_Alphacutoff.Value);
jo.Add(parameter_Is_OutlineTex.ParamName, parameter_Is_OutlineTex.Value);
if (parameter_OutlineTex != null && parameter_OutlineTex.Value != null) jo.Add(parameter_OutlineTex.ParamName, parameter_OutlineTex.Serialize());
jo.Add(parameter_Offset_Camera_Z.ParamName, parameter_Offset_Camera_Z.Value);
jo.Add(parameter_Is_BakedNormal.ParamName, parameter_Is_BakedNormal.Value);
if (parameter_BakedNormalforOutline != null && parameter_BakedNormalforOutline.Value != null) jo.Add(parameter_BakedNormalforOutline.ParamName, parameter_BakedNormalforOutline.Serialize());
jo.Add(parameter_GI_Intensity.ParamName, parameter_GI_Intensity.Value);
jo.Add(parameter_Unlit_Intensity.ParamName, parameter_Unlit_Intensity.Value);
jo.Add(parameter_VRChatSceneLightsHiCut_Filter.ParamName, parameter_VRChatSceneLightsHiCut_Filter.Value);
jo.Add(parameter_AdvancedActivateBuiltinLightDirection.ParamName, parameter_AdvancedActivateBuiltinLightDirection.Value);
jo.Add(parameter_OffsetXAxisBuiltinLightDirection.ParamName, parameter_OffsetXAxisBuiltinLightDirection.Value);
jo.Add(parameter_OffsetYAxisBuiltinLightDirection.ParamName, parameter_OffsetYAxisBuiltinLightDirection.Value);
jo.Add(parameter_InverseZAxisBuiltinLightDirection.ParamName, parameter_InverseZAxisBuiltinLightDirection.Value);
return new JProperty(BVA_Material_UTS_Extra.SHADER_NAME, jo);
}
}
}
