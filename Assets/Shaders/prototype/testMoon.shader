Shader "Unlit/testMoon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _Tiling ("Tiling", Range(20,8)) = 20
        [HDR]_MoonColor ("Moon Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define sqr2 1.41421356237

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
                float4 localPos : TEXCOORD1;
                float3 yAxis : TEXCOORD2;
            };

            sampler2D _MainTex;
            float3 _xAxis;
            float3 _zAxis;
            float3 _moonPhase;
            float _Tiling;
            float4 _MoonColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.localPos = v.vertex;
                o.yAxis = cross(_zAxis,_xAxis);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                col.r = length( (i.localPos - _xAxis) / sqr2 );
                col.g = length( (i.localPos - i.yAxis) / sqr2 );
                col.b = length( (i.localPos - _zAxis) / sqr2 );
                col = 1 - col;
                col.b = step(col.b,0);
                col += col.b;
                float off = 0.5/_Tiling;
                col.rg += off;
                col *= _Tiling;
                float moon = tex2D(_MainTex, col.xy).a * 2;
                float3 moonNorm = tex2D(_MainTex, col.xy).rgb * 2 - 1;
                moon *= saturate(dot(_moonPhase, moonNorm) * 1.5);
                return float4(moon.r * _MoonColor);
            }
            ENDCG
        }
    }
}
