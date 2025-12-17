Shader "Custom/PassThrough"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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

			sampler2D _MainTex;

			half4 frag(v2f_img i) : SV_Target
			{
				return tex2D(_MainTex, i.uv);
			}
		ENDHLSL
		}
	}
}