Shader "AOB/InvertBehind"
{
	Properties
	{
		_Color ("Tint Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
	}

		SubShader
	{
		Tags { "Queue" = "Transparent" }

		Pass
		{
			ZWrite On
			ColorMask 0
		}

		// Blend such that the colour inverts.
		Blend OneMinusDstColor OneMinusSrcAlpha
		BlendOp Add

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _Color;

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR0;
				float2 uv : TEXCOORD0;
			};

			// Transform vertices as normal.
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = _Color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			// Mix the inverted colour with the source color.
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				return i.color * col;
			}

			ENDCG
		}
	}
}
