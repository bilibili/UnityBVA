void LightweightPBR_float
(float3	Albedo,
	float	Metallic,
	float3	Specular,
	float	Smoothness,
	float	Occlusion,
	float3	Emission,
	float	Alpha,
	float3  PositionWS,
	float3  NormalWS,
	float3  ViewDirectionWS,
	float   FogCoord,
	float3  VertexLighting,
	float3  BakedGI,
	out float4	fragOut) {

	fragOut = float4 (1,1,1,1);

	#ifdef	LIGHTWEIGHT_INPUT_INCLUDED
	InputData inputData;
	inputData.positionWS = PositionWS;
	inputData.normalWS = NormalWS;
	inputData.viewDirectionWS = ViewDirectionWS;
	inputData.shadowCoord = GetShadowCoord(GetVertexPositionInputs(PositionWS));
	inputData.fogCoord = FogCoord;
	inputData.vertexLighting = VertexLighting;
	inputData.bakedGI = BakedGI;
	fragOut = LightweightFragmentPBR(inputData, Albedo, Metallic, Specular, Smoothness, Occlusion, Emission, Alpha);
	# endif
}