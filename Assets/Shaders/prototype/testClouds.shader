Shader "Unlit/testClouds"
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
        ZWrite On

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
                float4 localPos : TEXCOORD1;
                float3 yAxis : TEXCOORD2;
            };

            sampler2D _MainTex;
            float3 _xAxis;
            float3 _zAxis;
            float3 _moonPhase;

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
                fixed3 normPos = i.localPos * 0.5 + 0.5;
                fixed2 radUV = atan2(normPos.z,normPos.x);
                fixed lenUV = distance(fixed3(normPos.xz,0),fixed3(0,0,0));
                radUV.y = lenUV;
                fixed4 tex = tex2D(_MainTex, fixed2(radUV.x + frac(_Time.r), radUV.y) );
                return fixed4(tex);
                
            }
            ENDCG
        }
    }
}
