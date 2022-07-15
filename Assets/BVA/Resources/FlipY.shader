Shader "Hidden/FlipY"
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
				o.vertex = GetVertexPositionInputs(v.vertex).positionCS;
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			half4 frag(v2f i) : SV_Target
			{
				i.uv.y = 1.0 - i.uv.y;
				// Graphics.CopyTexture sometimes will flip the uv.y, so we need Blit it to Flip the Y
				float4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDHLSL
		}
	}
}
