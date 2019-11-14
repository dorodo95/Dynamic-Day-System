Shader "Environment/Skybox"
{
	Properties
	{
		[HideInInspector]_ZenithColor("Zennith Color", Color) = (0.93,0.77,0.68,1)
		[HideInInspector]_HorizonColor("Horizon Color", Color) = (0.34,0.55,0.81,1)
		_starTex("Stars Texture", 2D) = "white" {}
		_SunSize("Sun Size", float) = 1.0
		_SunSoftness("Sun Softness", float) = 1.0
		[HDR]_SunTint1("Sun Color Zenith", color) = (1,1,1,1)
		[HDR]_SunTint2("Sun Color Horizon", color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 uv : TEXCOORD0;
			};
			half4 _ZenithColor;
			half4 _HorizonColor;
			half3 _SunDirection;
			half4 _SunTint1;
			half4 _SunTint2;
			half4 _DirectionalColor;
			half _SunSize;
			half _SunSoftness;
			half _SunIntensity;
			sampler2D _starTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.uv = v.uv;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				_SunDirection *= -1;
				float2 uvnew = normalize(i.uv);
				float dist = distance(_SunDirection, i.uv);
				fixed sunBloom = saturate(1 - dist) * 0.07;
				sunBloom += saturate(1 - dist * 5) * 0.03;
				dist = smoothstep(0, _SunSize, dist);
				dist = 1 - saturate(dist);
				float4 skyCol = lerp(_HorizonColor,_ZenithColor, saturate(uvnew.y));
				dist = (smoothstep(0,_SunSoftness, dist)) * saturate(i.uv.y);
				fixed4 _SunColor = lerp(_SunTint2, _SunTint1, i.uv.y) * _DirectionalColor;
				dist += (sunBloom * saturate( i.uv.y + 0.4) * saturate(_DirectionalColor * _SunIntensity * 10 + 0.1) );
				return skyCol + (dist * _SunColor);
				//return fixed4(sunBloom.rrr,1);
				//return fixed4(distance(_SunDirection,i.uv).rrr,1);
			}
			ENDCG
		}
	}
}
