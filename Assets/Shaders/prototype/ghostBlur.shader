Shader "Unlit/ghostBlur"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
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
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _TexID;
            float4 _TexID_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 boxBlur (sampler2D tex, float2 uv, float4 texelSize)
            {
                float4 c =  tex2D(tex, uv + float2(-texelSize.x , texelSize.y )) +
                            tex2D(tex, uv + float2(0            , texelSize.y )) +
                            tex2D(tex, uv + float2( texelSize.x , texelSize.y )) +
                            tex2D(tex, uv + float2(-texelSize.x , 0           )) +
                            tex2D(tex, uv + float2(0            , 0           )) +
                            tex2D(tex, uv + float2( texelSize.x , 0           )) +
                            tex2D(tex, uv + float2(-texelSize.x , -texelSize.y)) +
                            tex2D(tex, uv + float2(0            , -texelSize.y)) +
                            tex2D(tex, uv + float2( texelSize.x , -texelSize.y));

                return c/9;

            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = boxBlur(_TexID, i.uv, _TexID_TexelSize);
                col.a = 0.5;
                return col;
            }
            ENDCG
        }
    }
}
