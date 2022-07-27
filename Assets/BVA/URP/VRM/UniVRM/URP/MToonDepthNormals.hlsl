#include "MToonCore.hlsl"

v2f DepthOnlyVertex(appdata_full v)
{
    v2f o = (v2f)0;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = TransformObjectToHClip(v.vertex);
    o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
    return o;
}

half4 DepthOnlyFragment(v2f i) : SV_TARGET
{
    Alpha(SampleAlbedoAlpha(i.uv0, TEXTURE2D_ARGS(_MainTex, sampler_MainTex)).a, _Color, _Cutoff);
    return 0;
}

v2f DepthNormalsVertex(appdata_full v)
{
    v2f o = (v2f)0;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = TransformObjectToHClip(v.vertex);
    o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
    VertexNormalInputs normalInput = GetVertexNormalInputs(v.normal, v.tangent);
    half3 worldNormal = normalInput.normalWS;
    half3 worldTangent = normalInput.tangentWS;
    half3 worldBitangent = normalInput.bitangentWS;
    o.tspace0 = half3(worldTangent.x, worldBitangent.x, worldNormal.x);
    o.tspace1 = half3(worldTangent.y, worldBitangent.y, worldNormal.y);
    o.tspace2 = half3(worldTangent.z, worldBitangent.z, worldNormal.z);
    return o;
}

half4 DepthNormalsFragment(v2f i) : SV_TARGET
{
    Alpha(SampleAlbedoAlpha(i.uv0, TEXTURE2D_ARGS(_MainTex, sampler_MainTex)).a, _Color, _Cutoff);
    
#ifdef _NORMALMAP
    half3 tangentNormal = SampleNormal(i.uv0, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
    half3 worldNormal;
    worldNormal.x = dot(i.tspace0, tangentNormal);
    worldNormal.y = dot(i.tspace1, tangentNormal);
    worldNormal.z = dot(i.tspace2, tangentNormal);
#else
    half3 worldNormal = half3(i.tspace0.z, i.tspace1.z, i.tspace2.z);
#endif

    return half4(NormalizeNormalPerPixel(worldNormal), 0.0);
}