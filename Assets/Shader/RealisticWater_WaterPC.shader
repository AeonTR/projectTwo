Shader "RealisticWater/WaterPC" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,1)
		_GlareColor ("Glare Color", Vector) = (1,1,1,1)
		_FadeColor ("Fade Color", Vector) = (0.9,0.9,1,1)
		_ReflectionColor ("Reflection Color", Vector) = (1,1,1,1)
		_ReflectionBrightness ("Reflection Brightness", Range(0, 1)) = 0.5
		_Wave1 ("Wave1 Distortion Texture", 2D) = "bump" {}
		_Wave2 ("Wave2 Distortion Texture", 2D) = "bump" {}
		_Foam ("Foam Texture", 2D) = "white" {}
		_Direction ("Waves Direction 1 & 2", Vector) = (1,1,-1,-1)
		_FoamDirection ("Foam Direction R & G Chanell", Vector) = (1,1,-1,-1)
		_FPOW ("FPOW Fresnel", Float) = 5
		_R0 ("R0 Fresnel", Float) = 0.05
		_OffsetFresnel ("Offset Fresnel", Float) = 0.1
		_FoamIntensity ("Foam Intensity", Float) = 1
		_FadeBlend ("Fade Blend Foam", Float) = 1
		_FadeBlend2 ("Fade Blend Transparency", Float) = 1
		_FadeDepth ("Fade Depth", Float) = 1
		_DepthTransperent ("Depth Transperent", Range(0, 1)) = 0
		_Distortion ("Distortion Normal", Float) = 400
		_DistortionVert ("Per Vertex Distortion", Float) = 1
		_EdgeDistortion ("EdgesDistortion", Float) = 1
		_Bias ("Bias Glare", Float) = -1
		_Scale ("Scale Glare", Float) = 10
		_Power ("Power Glare", Float) = 2
		_GAmplitude ("Wave Amplitude", Vector) = (0.1,0.3,0.2,0.15)
		_GFrequency ("Wave Frequency", Vector) = (0.6,0.5,0.5,1.8)
		_GSteepness ("Wave Steepness", Vector) = (1,2,1.5,1)
		_GSpeed ("Wave Speed", Vector) = (-0.23,-1.25,-3,1.5)
		_GDirectionAB ("Wave Direction", Vector) = (0.3,0.5,0.85,0.25)
		_GDirectionCD ("Wave Direction", Vector) = (0.1,0.9,0.5,0.5)
		_WaveScale ("Waves Scale", Float) = 1
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