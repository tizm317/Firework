Shader "Custom/Water2"
{

    Properties{
      _TintColor("Test Color", color) = (1, 1, 1, 1)
      _Intensity("Range Sample", Range(0, 1)) = 0.5
      _MainTex("Main Texture", 2D) = "white" {}
    [NoScaleOffset] _Flowmap("Flowmap", 2D) = "white" {}
     _FlowIntensity("flow Intensity", Range(0,1)) = 1
     _FlowTime("flow time", Range(0,10)) = 1
    
    }

    SubShader
    {
        Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "RenderType" = "Opaque"
                "Queue" = "Geometry"
            }
        Pass
            {
                 Name "Universal Forward"
                 Tags {"LightMode" = "UniversalForward"}

                HLSLPROGRAM
                    #pragma prefer_hlslcc gles
                    #pragma exclude_renderers d3d11_9x

                    #pragma vertex vert
                    #pragma fragment frag		

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

                half4 _TintColor;
                float _Intensity;
                float _FlowIntensity, _FlowTime;

                float4 _MainTex_ST;
                Texture2D _MainTex, _Flowmap;
                SamplerState sampler_MainTex;

                struct VertexInput
                {
                    float4 vertex : POSITION;
                    float2 uv 	  : TEXCOORD0;
                };

                struct VertexOutput
                {
                    float4 vertex  	: SV_POSITION;
                    float2 uv      	: TEXCOORD0;
                };

                VertexOutput vert(VertexInput v)
                {
                    VertexOutput o;

                    o.vertex = TransformObjectToHClip(v.vertex.xyz);
                    o.vertex.y += sin(v.vertex.x + v.vertex.z + _Time.y);
                    o.uv = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                    o.vertex.y += sin(_Time.y + v.vertex.x + v.vertex.z) * _FlowIntensity;
                    //o.uv.x += _Time.x;
                    return o;
                }

                half4 frag(VertexOutput i) : SV_Target
                {
                    float4 flow = _Flowmap.Sample(sampler_MainTex, i.uv);
                    i.uv += frac(_Time.x * _FlowTime) + flow.rg * _FlowIntensity;
                    float4 color = _MainTex.Sample(sampler_MainTex, i.uv);
                    color.rgb *= _TintColor * _Intensity;
                    return color;
                }
            ENDHLSL
            }
    }

}
