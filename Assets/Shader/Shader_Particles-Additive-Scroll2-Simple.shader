Shader "Shader/Particles-Additive-Scroll2-Simple" {
	Properties {
		_MainTex ("Base layer (RGB)", 2D) = "white" {}
		_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
		_ScrollX ("1nd layer Scroll speed X", Float) = 1
		_ScrollY ("1nd layer Scroll speed Y", Float) = 0
		_Scroll2X ("2nd layer Scroll speed X", Float) = 1
		_Scroll2Y ("2nd layer Scroll speed Y", Float) = 0
		_Color ("Color", Vector) = (1,1,1,1)
		_MMultiplier ("Layer Multiplier", Float) = 1
		[Enum(Off, 0, Additive, 1, Blend, 2, Custom, 3)] _BlendMode ("Blend Mode", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] [HideInInspector] _ColorSrc ("Color Blend Src Factor", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] [HideInInspector] _ColorDst ("Color Blend Dst Factor", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] [HideInInspector] _AlphaSrc ("Alpha Blend Src Factor", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] [HideInInspector] _AlphaDst ("Alpha Blend Dst Factor", Float) = 1
		[Enum(Off, 0, Front, 1, Back, 2)] _Cull ("Cull Mode", Float) = 0
		[HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector] _Stencil ("Stencil ID", Float) = 0
		[HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
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
	Fallback "Shader/Particles-UnlitTexture"
	//CustomEditor "GameEditor.ParticlesShaderGUI"
}