Shader "GAP/AlphaBlendedMobileDistortionScroll" {
	Properties {
		_TintColor ("Color", Vector) = (1,0.6235296,0.1470588,1)
		_ColorMultiplier ("Color Multiplier", Range(0, 10)) = 1
		_MainTextUSpeed ("MainText U Speed", Float) = 0
		_MainTextVSpeed ("MainText V Speed", Float) = 0
		_MainTex ("MainTex", 2D) = "white" {}
		[MaterialToggle] _DistortMainTexture ("Distort Main Texture", Float) = 0
		_GradientPower ("Gradient Power", Range(0, 50)) = 0
		_GradientUSpeed ("Gradient U Speed", Float) = 0.1
		_GradientVSpeed ("Gradient V Speed", Float) = 0.1
		_Gradient ("Gradient", 2D) = "white" {}
		_NoiseAmount ("Noise Amount", Range(-1, 1)) = 0.1
		_DistortionUSpeed ("Distortion U Speed", Float) = 0.1
		_DistortionVSpeed ("Distortion V Speed", Float) = 0.1
		_Distortion ("Distortion", 2D) = "white" {}
		_MainTexMask ("MainTexMask", 2D) = "white" {}
		_DoubleSided ("DoubleSided", Float) = 1
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	//CustomEditor "ShaderForgeMaterialInspector"
}