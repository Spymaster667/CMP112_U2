Shader "Custom/URPTest1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScrollSpeed ("Scroll Speed (vec2)", Vector) = (1,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		LOD 200

		Pass
		{
		HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			// Uniforms
			sampler2D _MainTex;
			float2 _ScrollSpeed;

			Varyings vert(Attributes IN)
			{
				Varyings OUT;
				OUT.positionHCS = mul(UNITY_MATRIX_MVP, IN.positionOS);
				OUT.uv = IN.uv;
				return OUT;
			}

			half4 frag(Varyings IN) : SV_Target
			{
				float2 scrolledUV = IN.uv - _Time.x * _ScrollSpeed;
				return tex2D(_MainTex, scrolledUV);
			}
		ENDHLSL
		}
	}
}