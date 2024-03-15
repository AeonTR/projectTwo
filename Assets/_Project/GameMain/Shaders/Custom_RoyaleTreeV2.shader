Shader "Custom/RoyaleTreeV2" {
	Properties {
		[NoScaleOffset] _BaseMap ("Branch Texture", 2D) = "white" {}
		[NoScaleOffset] _CompositeDiffuse ("Composite Map", 2D) = "white" {}
		[NoScaleOffset] _CompositeShadow ("Composite Shadow Map", 2D) = "white" {}
		[HDR] _EmissiveColor ("EmissiveColor(bakery fix)", Vector) = (0,0,0,0)
		_EmissionColor ("EmissionColor(bakery fix)", Vector) = (0,0,0,0)
		[HideInInspector] _Cull ("__cull", Float) = 2
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
}