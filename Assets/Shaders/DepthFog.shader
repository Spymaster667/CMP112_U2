Shader "Custom/DepthFog"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		fogStart ("Fog Start", Float) = 0
		fogEnd ("Fog End", Float) = 1
		fogCol ("Fog Colour", Color) = (.25,.5,1)

		debugFog ("Debug Fog Start and End", Integer) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
		HLSLPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			// Uniforms
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			float fogStart;
			float fogEnd;
			float3 fogCol;

			bool debugFog;
			
			half4 frag(v2f_img i) : SV_Target
			{
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
				return half4(col, 1);
			}
		ENDHLSL
		}
	}
}