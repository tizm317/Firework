Shader "Custom/Water_transparant"
{
	// ��ȣ��
	// �� ���̴�
	// �ⷷ�� + ������

	Properties{
		_SPColor("Specular Color", color) = (1,1,1,1) // //
		_SPPower("Specular Power", Range(50, 300)) = 150 // //
		_SPMulti("Specular Multiply", Range(1,10)) = 3	// //
	_TintColor("Test Color", color) = (1, 1, 1, 1)
	_Intensity("Range Sample", Range(0, 1)) = 0.5
	_MainTex("Main Texture", 2D) = "white" {}
	[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
			[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
	[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 1

	[Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 0

	[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0

	_Factor("Factor", int) = 0
	_Units("Units", int) = 0

[Enum(Off, 0, On, 1)] _Mask("Alpha to Coverage", Float) = 0

	_Alpha("Alpha", Range(0,1)) = 0.5

		//
		[NoScaleOffset] _Flowmap("Flowmap", 2D) = "white" {}
	 _FlowIntensity("flow Intensity", Range(0,1)) = 1
	 _FlowTime("flow time", Range(0,10)) = 1
	}

		SubShader
	{
	Tags
	{
	 "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent"  "Queue" = "Transparent"	}
		Pass
		{
		 Blend[_SrcBlend][_DstBlend]
		 Cull[_Cull]
		 ZWrite[_ZWrite]
		 ZTest[_ZTest]

		 Offset[_Factor],[_Units]

		AlphaToMask[_Mask]

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
		float _Alpha;
		
		float _FlowIntensity, _FlowTime; //


		float4 _MainTex_ST;
		Texture2D _MainTex, _Flowmap; //
		SamplerState sampler_MainTex;

		//
		float4 _SPColor;
		float _SPPower;
		float _SPMulti;

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
			//Vertex shader�� Ȱ���� mesh animation ����
			VertexOutput o;
			o.vertex = TransformObjectToHClip(v.vertex.xyz);
			// sin �Լ��� �̿��� ���� ����
			// v.vertex.z���� �߰��ϸ� xz�� ���� ���� ������ ���ϰ�
			//���⿡ ������ ������ Time �Լ��� ���ϸ� ������ �����̴� �޽ø� ����� �ִ�.
			o.vertex.y += sin(v.vertex.x + v.vertex.z + _Time.y); // 
			o.uv = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			//o.vertex.y += sin(_Time.y + v.vertex.x + v.vertex.z) * _FlowIntensity; //
			return o;
		}

		half4 frag(VertexOutput i) : SV_Target
		{
			float4 flow = _Flowmap.Sample(sampler_MainTex, i.uv);

			////���� ��ũ�� �Ǵ� uv�� flowmap�� ������ uv���� �����غ���. 
			////�� ���̴����� ���� �߰� �ؽ�ó ����� �־��ش�.
			//Properties �޴��� Flow intensity�� Time�� �����ϴ� ��� �߰��ϱ�
			i.uv += frac(_Time.x * _FlowTime) + flow.rg * _FlowIntensity;
			
			float4 color = _MainTex.Sample(sampler_MainTex, i.uv);
			color.rgb *= _TintColor * _Intensity;
			color.a = color.a * _Alpha;
			return color;
		  }
		ENDHLSL
		}
	}
}
