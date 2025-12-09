Shader "Unlit/URPTest1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScrollSpeed ("Scroll Speed (vec2)", Vector) = (0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);

			float2 _ScrollSpeed;

			Varyings vert(Attributes IN)
			{
				Varyings OUT;
				OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
				OUT.uv = IN.uv;
				return OUT;
			}

			half4 frag(Varyings IN) : SV_Target
			{
				float2 scrolledUV = IN.uv - _Time.x * _ScrollSpeed;
				return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, scrolledUV);
			}
			ENDHLSL
		}
	}
}