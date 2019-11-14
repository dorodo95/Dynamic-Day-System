Shader "Environment/Stars (Skybox)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[HDR]_ColorTint ("Color Tint", color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Transparent" }
		ZWrite Off

		Blend One One

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
				float2 uv1 : TEXCOORD1;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _ColorTint;
			float _SunIntensity;
			float _CurrentTime;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = v.uv1;
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				i.uv1 = saturate(i.uv1);
				fixed col = tex2D(_MainTex, i.uv + _CurrentTime * 4).r;
				fixed mask = tex2D(_MainTex, i.uv - _CurrentTime * 4).g;
				col.r *= i.color.a * 10 *  mask * (mask + 0.5);
				fixed4 finalCol = col.r * _ColorTint;
				finalCol *= saturate(1 - (_SunIntensity * 1.5));
				return finalCol;
				//return fixed4(i.uv1.rg,0,1);
			}
			ENDCG
		}
	}
}
