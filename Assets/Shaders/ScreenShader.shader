Shader "UI/ScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[NoScaleOffset] ditherPattern ("Dither Pattern", 2D) = "white" {}
		ditherIntensity ("Dither Intensity", Float) = .5
		colourDepth ("Colour Depth", Integer) = 4
	}
	SubShader
	{
		// No culling or depth
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Always

		Pass
		{
			Name "Fog"
		HLSLPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			
			half4 frag(v2f_img i)
			{
				return tex2D(_MainTex, i.uv);
			}
		ENDHLSL
		}

		Pass
		{
			Name "OrderedDither"
		HLSLPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include <UnityCG.cginc>

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// Uniforms
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			sampler2D ditherPattern;
			float4 ditherPattern_TexelSize;

			float ditherIntensity;
			int colourDepth;

			half4 frag(v2f_img i) : SV_Target
			{
				half3 col = tex2D(_MainTex, i.uv).rgb;

				// Sample DitherPattern
				float2 ditherUV = i.uv * (_MainTex_TexelSize.zw / ditherPattern_TexelSize.zw);

				float ditherValue = tex2D(ditherPattern, ditherUV).r;
				ditherValue *= ditherIntensity / 10;

				// Quantization
				col = round((col + ditherValue) * colourDepth) / colourDepth;

				return half4(col, 1);
			}
		ENDHLSL
		}
	}
}