Shader "Custom/Grass"
{
	Properties
	{
		_BottomColor ("Bottom Color", Color) = (1,1,1,1)
		_TopColor ("Top Color", Color) = (1,1,1,1)
		_NoiseTex ("Albedo (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Cull Off

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows addshadow
		#pragma target 3.0

		sampler2D _NoiseTex;

		struct Input {
			float3 worldPos;
			float4 color : COLOR;
		};

		fixed4 _BottomColor;
		fixed4 _TopColor;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float h = 1 - IN.color.r;

			float2 uv = IN.worldPos.xz * 0.08;
			float noiseRoll = tex2D(_NoiseTex, uv * 0.01 - _Time.x * 0.05).r;
			float noiseBase = tex2D(_NoiseTex, uv - noiseRoll * 0.05 * h).r;
			
			float color = lerp(noiseBase, noiseRoll, 0.6);
			float alpha = noiseBase;

			o.Albedo = lerp(_BottomColor, _TopColor, color) * 1.25;
			clip(alpha - h);

			o.Smoothness = lerp(0.1, 0.5, h);
		}

		ENDCG
	}

	FallBack "Diffuse"
}