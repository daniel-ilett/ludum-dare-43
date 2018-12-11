Shader "Hidden/ChromaticAberration"
{
	// Thanks to http://halisavakis.com/my-take-on-shaders-chromatic-aberration-introduction-to-image-effects-part-iv/
	// for much of the detail on this shader!
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Strength ("Aberration Strength", Range(0.0, 1.0)) = 0.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			uniform sampler2D _MainTex;
			uniform float _Strength;

			// Shift the red and blue channel UV coordinates.
			fixed4 frag (v2f i) : SV_Target
			{
				float r = tex2D(_MainTex, i.uv - float2(_Strength, _Strength)).r;
				float g = tex2D(_MainTex, i.uv).g;
				float b = tex2D(_MainTex, i.uv + float2(_Strength, _Strength)).b;
				return fixed4(r, g, b, 1.0);
			}
			ENDCG
		}
	}
}
