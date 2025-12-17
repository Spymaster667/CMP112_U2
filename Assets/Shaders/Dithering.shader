Shader "Custom/Dithering"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[NoScaleOffset] ditherPattern ("Dither Pattern", 2D) = "white" {}
		ditherIntensity ("Dither Intensity", Float) = .75
		colourDepth ("Colour Depth", Integer) = 24
		colourSlope ("Colour Slope", Float) = 1 // Less important with a high colour depth
	}
	SubShader
	{
		// No culling or depth
		Cull Back
		Lighting Off
		ZWrite Off
		ZTest Always

		Pass
		{
			Name "OrderedDither"
		HLSLPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			// Uniforms
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			sampler2D _CameraDepthTexture;

			sampler2D ditherPattern;
			float4 ditherPattern_TexelSize;

			float ditherIntensity;
			int colourDepth;
			float colourSlope;

			half4 frag(v2f_img i) : SV_Target
			{
				// Sample
				half3 col = tex2D(_MainTex, i.uv).rgb;
				
				// Sample DitherPattern
				float2 ditherUV = i.uv * (_MainTex_TexelSize.zw / ditherPattern_TexelSize.zw);
				float ditherValue = tex2D(ditherPattern, ditherUV).r;
				
				ditherValue = ditherValue*2-1; // normalize
				ditherValue *= ditherIntensity / colourDepth;

				// Quantization
				col = pow(col, 1 / colourSlope);
				col = floor((col + ditherValue) * (colourDepth-1)) / (colourDepth-1);
				col = pow(col, colourSlope);

				// Desmos visualization:
				// \left(\frac{\operatorname{round}\left(x^{\frac{1}{s}}d\right)}{d}\right)^{s}
				// \left(\frac{\left(x^{\frac{1}{s}}d\right)}{d}\right)^{s}

				return half4(col, 1);
			}
		ENDHLSL
		}
	}
}