Shader "Custom/CutoutShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _TargetCut ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        LOD 200
        Cull Off
        ZWrite On
        ZTest LEqual

        CGPROGRAM
        #pragma surface surf Lambert alphatest:_TargetCut

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float cutoff;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            clip(c.a - IN.cutoff);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
