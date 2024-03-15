Shader "GAP/AdditiveMobileDistortionScroll" {
	Properties {
		_TintColor ("Color", Vector) = (1,0.5342799,0.1764706,1)
		_ColorRamp ("Color Ramp", 2D) = "white" {}
		_ColorMultiplier ("Color Multiplier", Range(0, 10)) = 1.32872
		_MainTextureUSpeed ("Main Texture U Speed", Float) = 0
		_MainTextureVSpeed ("Main Texture V Speed", Float) = 0
		_MainTexutre ("Main Texutre", 2D) = "white" {}
		[MaterialToggle] _DistortMainTexture ("Distort Main Texture", Float) = 0
		_GradientPower ("Gradient Power", Range(0, 50)) = 2.214298
		_GradientUSpeed ("Gradient U Speed", Float) = -0.2
		_GradientVSpeed ("Gradient V Speed", Float) = -0.2
		_Gradient ("Gradient", 2D) = "white" {}
		_NoiseAmount ("Noise Amount", Range(-1, 1)) = 0.1144851
		_DistortionUSpeed ("Distortion U Speed", Float) = 0.2
		_DistortionVSpeed ("Distortion V Speed", Float) = 0
		_Distortion ("Distortion", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_DoubleSided ("DoubleSided", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	//CustomEditor "ShaderForgeMaterialInspector"
}