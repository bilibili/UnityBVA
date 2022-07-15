// #pragma once is a safe guard best practice in almost every .hlsl (need Unity2020 or up), 
// doing this can make sure your .hlsl's user can include this .hlsl anywhere anytime without producing any multi include conflict
//#pragma once

struct a2v
{
	float4 positionOS: POSITION;
	float4 normalOS: NORMAL;
	float4 tangentOS: TANGENT; 
	float4 uv8     : TEXCOORD7;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
	float4 positionCS: SV_POSITION;

	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO  
};

TEXTURE2D(_BaseMap);			SAMPLER(sampler_BaseMap);
TEXTURE2D(_ToonMaskMap);		SAMPLER(sampler_ToonMaskMap);

v2f vert(a2v v)
{ 
	v2f o;

	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	/*
	float4 objPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
	float2 uv = TRANSFORM_TEX(v.texcoord, _BaseMap);
	float4 optMapColor = SAMPLE_TEXTURE2D_LOD(_ToonMaskMap, sampler_ToonMaskMap, uv, 0);

	float Set_Outline_Width = _OutLineThickness * 0.001
		* smoothstep(_OutLineDistance_Far, _OutLineDistance_Near, distance(objPos.rgb, _WorldSpaceCameraPos))
		* optMapColor.g;
	o.positionCS = TransformObjectToHClip(v.positionOS.xyz + v.normalOS.xyz * Set_Outline_Width);

	*/

	
	VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
	float4 pos = vertexInput.positionCS;
	VertexNormalInputs normalInput = GetVertexNormalInputs(v.normalOS.xyz, v.tangentOS.xyzw);
	
	real sign = v.tangentOS.w * GetOddNegativeScale();
	float3 bitangentOS = cross(v.normalOS.xyz, v.tangentOS.xyz) * sign;
	float3 uv8NormalOS = mul(v.uv8.xyz, float3x3(v.tangentOS.xyz, bitangentOS.xyz, v.normalOS.xyz)); //从切线空间变换到物体空间
	 

	float Set_OutlineWidth = pos.w * _OutLineThickness;
    //Set_OutlineWidth = min(Set_OutlineWidth, _OutLineThickness);
    //Set_OutlineWidth *= _OutLineThickness;
    //Set_OutlineWidth = min(Set_OutlineWidth, _OutLineThickness) * 0.001;

	float3 posOS = v.positionOS.xyz + uv8NormalOS * Set_OutlineWidth;
	o.positionCS = TransformObjectToHClip(posOS); 

	return o;
}

float4 frag(v2f i) : SV_Target
{
	float4 col = _OutLineColor;
	return col;
}