// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TestShader"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_Color("Color",Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent"  "RenderType" = "Transparent" }
		LOD 100

		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha

		Cull Back
		ZWrite Off

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;

		float2 screenuv: TEXCOORD1;
		float depth : DEPTH;

	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed4 _Color;
	fixed4 _RColor;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;

		o.screenuv = ((o.vertex.xy / o.vertex.w) + 1) / 2;
		//o.screenuv.y = 1 - o.screenuv.y;
		o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z *_ProjectionParams.w;

		return o;
	}
	float4 horizontalBars(float y) {
		y = y - (_Time.x%0.05);
		return 1 - saturate(round(abs(frac(y * 20)*((y + 0.5) * 2))));
	}

	float4 frag(v2f i) : SV_Target
	{
		// Basic color fade+ bars
		float4 col = float4(_Color.x, _Color.y, _Color.z, pow( i.uv.y,4)*((_SinTime.w + 1) / 2 + 0.25));
		col = col + float4(_Color.x, _Color.y, _Color.z, pow(( i.uv.y), 2)*horizontalBars((i.uv.y)).w) / 2;

		return col;
	}
		ENDCG
	}
	}
}
