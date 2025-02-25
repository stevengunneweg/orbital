﻿Shader "Custom/EarthShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_LowColor("LowColor", Color) = (1,1,1,1)
		_MidColor("MidColor", Color) = (1,1,1,1)
		_HighColor("HighColor", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_CentrePoint("Centre", Vector) = (0, 0, 0, 0)
		_Fade("FadeStrenght", Range(0,100)) = 10.0
		_MinDepth("MinDepth", Range(1,3)) = 1.96
		_MidDepth("_MidDepth", Range(1,3)) = 2
		_MaxDepth("_MaxDepth", Range(1,3)) = 2.2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv2_MainTex : TEXCOORD0;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		half _Fade;
		half _MinDepth;
		half _MidDepth;
		half _MaxDepth;
		fixed4 _Color;
		fixed4 _LowColor;
		fixed4 _MidColor;
		fixed4 _HighColor;
		float4 _CentrePoint;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) {

			half min = _MinDepth;
			half mid = _MidDepth;
			half max = _MaxDepth;
			float t = (distance(_CentrePoint.xyz, IN.worldPos) - min) / (max - min);
			float t2 = (distance(_CentrePoint.xyz, IN.worldPos) - min) / (mid - min);
			float t3 = (distance(_CentrePoint.xyz, IN.worldPos) - mid) / (max - mid);

			fixed4 lowColor = lerp(_LowColor, _MidColor, t2*_Fade);
			fixed4 highColor = lerp(_MidColor, _HighColor, t3*_Fade);

			_Color = lerp(lowColor, highColor, round(t));
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv2_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
