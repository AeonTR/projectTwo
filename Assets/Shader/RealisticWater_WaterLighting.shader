Shader "RealisticWater/WaterLighting" {
	Properties {
		_Color ("Color", Vector) = (0.5,0.5,0.5,0.5)
		_SpecColor ("Specular Material Color", Vector) = (1,1,1,1)
		_Shininess ("Shininess", Float) = 10
		_Wave1 ("Wave1 Texture", 2D) = "white" {}
		_Wave2 ("Wave2 Texture", 2D) = "white" {}
		_Direction ("Waves Direction 1 & 2", Vector) = (1,1,-1,-1)
		_FPOW ("FPOW Fresnel", Float) = 5
		_R0 ("R0 Fresnel", Float) = 0.05
		_OffsetFresnel ("Offset Fresnel", Float) = 0.1
		_WorldLightDir ("Specular light direction", Vector) = (0,0.1,-0.5,0)
		_TexturesScale ("Textures Scale", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
}