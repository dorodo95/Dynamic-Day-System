Shader "Fonts/Wave"
{
	Properties
	{
		[HideInInspector] _MainTex ("Texture", 2D) = "white" {}
		_OverlayTex ("Overlay Texture", 2D) = "white" {}
		[Toggle(GLOBAL_OVERLAY)]_useGlobalOverlay ("Use Global UV for Overlay", float) = 0
		_Fade("Fade", Range(0,1)) = 1.0
		_Speed("Speed", float) = 100
		_waveFrequency("Wave Frequency", float) = 10
		_waveStrengthX("Wave Strength X", float) = 5
		_waveStrengthY("Wave Strength Y", float) = 5
	}
	SubShader
	{
		Tags 
		{ 
			"Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True" 
		}

		Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature USE_OVERLAY
			#pragma shader_feature GLOBAL_OVERLAY

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _OverlayTex;
			float4 _OverlayTex_ST;
			float _Fade;
			float _Speed;
			float _waveFrequency;
			float _waveStrengthX;
			float _waveStrengthY;
			
			v2f vert (appdata v)
			{
				v2f o;
				
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = v.uv1;
				o.uv1.y = 0;
				o.uv2 = v.uv2;
				o.uv2 = TRANSFORM_TEX(v.uv2, _OverlayTex);
				o.uv2.x += _Time.x * 7;
				o.color = v.color;

				v.vertex.y += (sin((-_Time.x * _Speed) + v.uv1.x * _waveFrequency) * _waveStrengthY) * (1 -v.color.a);
				v.vertex.x += (cos((-_Time.x * _Speed) + v.uv1.y * _waveFrequency) * _waveStrengthX) * (1 -v.color.a);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col;
				fixed font = tex2D(_MainTex, i.uv).a;

				#ifdef GLOBAL_OVERLAY
					fixed3 overlayCol = tex2D(_OverlayTex, i.uv1 + (_Time * 14)).rgb;
					#else
					fixed3 overlayCol = tex2D(_OverlayTex, i.uv2).rgb;
				#endif

				col.rgb = i.color.rgb;
				//col.a = lerp( (1 - i.uv1) * col.a, col.a, _Fade);
				col.a = step(i.uv1.x,_Fade) * font;
				col.rgb = lerp(overlayCol, col.rgb, i.color.a);
				//fixed4 col = fixed4(sin( (_Time.x * 100) + i.uv1.x),0,0,1);
				return col;
			}
			ENDCG
		}
	}
}
