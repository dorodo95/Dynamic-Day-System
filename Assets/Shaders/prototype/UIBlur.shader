Shader "Unlit/ghostBlur"
{
	Properties 
    { 
        [PerRendererData] _MainTex ("MainTex", 2D) = "white" {}
    }
	SubShader
	{
		Tags { "RenderType" = "Transparent" }
		ZWrite Off
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex: POSITION;
				float2 uv: TEXCOORD0;
				float4 color: COLOR;
			};

			struct v2f
			{
				float2 uv: TEXCOORD0;
				float4 vertex: SV_POSITION;
				float4 screenPos: TEXCOORD1;
				float4 color: COLOR;
			};

			sampler2D _TexID;
			float4 _TexID_TexelSize;
            sampler2D _MainTex;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}

			float4 boxBlur(sampler2D tex, float2 uv, float4 texelSize, float len)
			{
				int bias = ceil(len / 2.0);
				int start = bias - len;
				int end = -start;

				float4 col;

				for (int y = start; y <= end; y ++)
				{
					for (int x = start; x <= end; x ++)
					{
                      col +=  tex2D(tex, uv + float2(x * texelSize.x, y * texelSize.y));
					}
				}

				
				return col / (len * len);
			}

			fixed4 frag(v2f i): SV_Target
			{
                fixed4 sprite = tex2D(_MainTex, i.uv);
				fixed2 screenUV = i.screenPos.xy / i.screenPos.w;
				fixed4 col = boxBlur(_TexID, screenUV, _TexID_TexelSize, 6) * i.color * sprite;
				return col;
			}
			ENDCG
			
		}
	}
}
