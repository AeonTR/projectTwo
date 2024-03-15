Shader "Mobile/MobileWaterShader_EKV" {
	Properties {
		[Toggle(LIGHTING)] _SECTIONLIGHTING ("---- LIGHTING ------------------------", Float) = 1
		[PowerSlider(5.0)] _Shininess ("Shininess", Range(0.01, 2)) = 0
		[PowerSlider(5.0)] _Brightness ("Brightness", Range(0.01, 100)) = 0
		[PowerSlider(5.0)] _Attenuation ("Attenuation", Range(0.001, 1)) = 0
		_Color ("Color Tint", Vector) = (0.5,0.5,0.5,1)
		[Toggle(TEXTURES)] _SECTIONTEXTURES ("---- TEXTURES (if disabled, waves will disable too)---------------", Float) = 1
		_MainTex ("Texture A", 2D) = "black" {}
		_MainTexRot ("Texture A Rotation", Range(0, 360)) = 0
		_DiffTex ("Texture B", 2D) = "black" {}
		_DiffTexRot ("Texture B Rotation", Range(0, 360)) = 0
		[Toggle(WAVES)] _SECTIONWAVES ("---- WAVES AND FLOW ------------------------", Float) = 1
		[NoScaleOffset] _DerivHeightMap ("Wave Height Map", 2D) = "black" {}
		_Tiling ("Tiling", Float) = 3
		[PowerSlider(1.0)] _Speed ("Speed", Range(0.001, 1)) = 0.1
		[PowerSlider(1.0)] _FlowStrength ("Flow Strength", Range(-1, 1)) = 0.1
		[PowerSlider(1.0)] _FlowOffset ("Flow Offset", Range(-1, 1)) = 0.25
		[PowerSlider(1.0)] _HeightScale ("Height Scale, Constant", Range(-1, 1)) = 0.5
		[PowerSlider(1.0)] _HeightScaleModulated ("Height Scale, Modulated", Range(-5, 5)) = 4
		[Toggle(REFLECTION)] _REFLECTION ("---- CUBEMAP REFLECTION ------------------------", Float) = 1
		[Slider] _RefStrength ("Reflection Strength", Range(0, 1)) = 1
		_Cube ("Cubemap", Cube) = "" {}
		[Toggle(BLENDING)] _BLENDING ("---- BLENDING MODE (always on) ------------------------", Float) = 1
		SrcMode ("Src Mode - recommended: 4 (opaque), 3 or 5 (transparent)", Float) = 4
		DstMode ("Dst Mode - recommended: 3", Float) = 3
		[Toggle(COMMTRANSP)] _COMMTRANSP ("(Remember setting render queue to 3000 for transparency)", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Mobile/VertexLit"
}