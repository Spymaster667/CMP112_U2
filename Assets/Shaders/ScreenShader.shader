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
			Name "OrderedDither"
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include <UnityCG.cginc>

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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(v.vertex, UNITY_MATRIX_MVP);
				o.uv = v.uv;
				return o;
			}

			// Uniforms
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			
			sampler2D ditherPattern;
			float4 ditherPattern_TexelSize;
			
			float ditherIntensity;
			int colourDepth;

			half4 frag(v2f i) : SV_Target
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