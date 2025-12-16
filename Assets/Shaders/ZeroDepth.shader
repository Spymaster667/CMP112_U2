Shader "Custom/ZeroDepth"
{
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}
		LOD 200
		ZWrite On

		Pass
		{
		HLSLPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct FOut {
				half4 target : SV_Target;
				float depth : SV_Depth;
			};

			sampler2D _MainTex;

			FOut frag(v2f_img i)
			{
				FOut o;
				o.target = tex2D(_MainTex, i.uv);
				o.depth = 1;
				return o;
			}
		ENDHLSL
		}
	}
	FallBack "Diffuse"
}