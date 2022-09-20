#include "MToonCore.hlsl"

v2f vert_forward_base(appdata_full v)
{
    UNITY_SETUP_INSTANCE_ID(v);
    v.normal = normalize(v.normal);
    return InitializeV2F(v, TransformWorldToHClip(TransformObjectToWorld(v.vertex)), 0);
}

v2f vert_forward_base_outline(appdata_full v)
{
    UNITY_SETUP_INSTANCE_ID(v);
    v.normal = normalize(v.normal);
    return InitializeV2F(v, CalculateOutlineVertexClipPosition(v), 1);
}

v2f vert_forward_add(appdata_full v)
{
    UNITY_SETUP_INSTANCE_ID(v);
    v.normal = normalize(v.normal);
    return InitializeV2F(v, TransformWorldToHClip(TransformObjectToWorld(v.vertex)), 0);
}