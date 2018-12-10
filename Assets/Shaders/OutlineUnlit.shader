Shader "AOB/OutlineUnlit"
{
	Properties
	{
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
		_OutlineSize ("Outline Size", float) = 0
		_Color ("Model Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		// Unlit colour pass - give the object an unlit colour.
		Pass
		{
			// Write to the stencil buffer - the outline pass reads this.
			Stencil
			{
				Ref 4
				Comp always
				Pass replace
				ZFail keep
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color: COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = _Color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}

			ENDCG
		}

		// Outline pass - draw a thick outline around the object.
		Pass
		{
			Cull OFF
			ZWrite OFF
			ZTest ON

			Stencil
			{
				Ref 4
				Comp notequal
				Fail keep
				Pass replace
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float4 _OutlineColor;
			uniform float  _OutlineSize;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal: NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color: COLOR;
			};
			
			v2f vert (appdata v)
			{
				v2f o;

				// Add an extrustion along the normal vector.
				float3 normal = normalize(v.normal);
				o.vertex = v.vertex + float4(normal, 0.0) * _OutlineSize;

				o.vertex = UnityObjectToClipPos(o.vertex);
				o.color = _OutlineColor;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}

			ENDCG
		}
	}
}
