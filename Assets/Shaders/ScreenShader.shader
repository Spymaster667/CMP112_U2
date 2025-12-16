Shader "Custom/ScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[NoScaleOffset] ditherPattern ("Dither Pattern", 2D) = "white" {}
		ditherIntensity ("Dither Intensity", Float) = .75
		colourDepth ("Colour Depth", Integer) = 24
		colourSlope ("Colour Slope", Float) = 1 // Less important with a high colour depth
		
		fogStart ("Fog Start", Float) = 0
		fogEnd ("Fog End", Float) = 1
		fogCol ("Fog Colour", Color) = (.25,.5,1)
		
		debugFog ("Debug Fog Start and End", Integer) = 0
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


			float fogStart;
			float fogEnd;
			float3 fogCol;

			bool debugFog;

			half4 frag(v2f_img i) : SV_Target
			{
				// Sample
				half3 col = tex2D(_MainTex, i.uv).rgb;
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);

				// convert to linear depth
				depth = Linear01Depth(depth);
				float fogStartLinear = fogStart / 1000;
				float fogEndLinear = fogEnd / 1000;

				// remap depth
				depth = (depth - fogStartLinear) / abs(fogEndLinear - fogStartLinear);
				// Debug
				if (debugFog)
				{
					col.r = depth > 1;
					col.g = depth < 0;
					col.b = depth;
					if (depth > 1 || depth < 0)
					{
						col.b = 0;
					}
					return half4(col, 1);
				}

				// Apply depth
				depth = clamp(depth, 0., 1.);
				col = lerp(col, fogCol, depth);
				return depth;

				// ---------
				// Dithering
				// ---------
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