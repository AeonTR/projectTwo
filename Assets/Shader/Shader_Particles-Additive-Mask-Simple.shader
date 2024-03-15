Shader "Shader/Particles-Additive-Mask-Simple" {
	Properties {
		_MainTex ("Particle Texture", 2D) = "white" {}
		_Mask ("Mask ( R Channel )", 2D) = "white" {}
		_TintColor ("Tint Color", Vector) = (0.5,0.5,0.5,0.5)
		[HideInInspector] _Center ("Center", Vector) = (0,0,0,1)
		[HideInInspector] _Scale ("Scale", Vector) = (1,1,1,1)
		[HideInInspector] _Normal ("Normal", Vector) = (0,0,1,0)
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
	Fallback "Shader/Particles-UnlitTexture"
	//CustomEditor "GameEditor.ParticlesShaderGUI"
}