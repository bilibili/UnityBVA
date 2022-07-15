Shader "Hidden/NormalChannel"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = GetVertexPositionInputs(v.vertex.xyz).positionCS;
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			half4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				// If a texture is marked as a normal map
				// the values are stored in the A and G channel.
				//return float4(col.a, col.g, 1, 1);
				// 
				// Convert from compressed normal value to usual normal value.
				col.xyz = (UnpackNormal(col) + 1) * 0.5;
				col.w = 1;

				return col;
			}
			ENDHLSL
		}
	}
}
