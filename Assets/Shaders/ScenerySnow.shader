Shader "_Scenery/Snow"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _Roughness ("Smoothness", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _RTTexture ("Render Texture", 2D) = "black"
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Roughness;
        sampler2D _BumpMap;
        sampler2D _RTTexture;
        float3 _camPosition;
        float _orthoCamSize;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos : TEXCOORD1;
        };

        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v, out Input o) 
        {
          UNITY_INITIALIZE_OUTPUT(Input,o);
          o.worldPos = mul(unity_ObjectToWorld, v.vertex);
            float2 OffsetUV = (o.worldPos.xz - _camPosition.xz) / (_orthoCamSize * 2) + 0.5;
          fixed3 rtMap = tex2Dlod(_RTTexture, OffsetUV.xyxy);
          v.vertex.y += 0.5 * (1 - rtMap);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = _Color;
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            fixed rmap = tex2D (_Roughness, IN.uv_MainTex);
            o.Smoothness = 1 - rmap;
            //o.Smoothness = 1 - rmap * ( 1- saturate(rtMap));
            o.Metallic = _Metallic;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
