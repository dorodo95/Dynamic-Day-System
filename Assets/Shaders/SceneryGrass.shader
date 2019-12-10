Shader "_Scenery/Default"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Roughness ("Smoothness", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Roughness;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
            float4 vertexColor : COLOR;
            float3 worldNormal;
            float3 viewDir;
            float3 tangent_input;
        };

        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //c.rgb = lerp(fixed3(0,0,0), c.rgb, IN.vertexColor.r);
            //fixed4 c = lerp(fixed4(0,0,0,1), fixed4(1,1,1,1), IN.COLOR.r);
            fixed tangentMask = abs(dot(_WorldSpaceLightPos0, IN.tangent_input));
            tangentMask = pow(tangentMask,1.5);
            o.Emission = 1 - saturate(dot(IN.worldNormal, IN.viewDir));
            o.Emission = saturate(pow(o.Emission, 30) + pow(o.Emission, 5) * 0.1) * (_LightColor0 * 7) * c.rgb * tangentMask;
            o.Emission *= tangentMask;
            o.Albedo = c.rgb;
            fixed rmap = tex2D (_Roughness, IN.uv_MainTex);
            o.Smoothness = 1 - rmap;
            o.Metallic = _Metallic;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
