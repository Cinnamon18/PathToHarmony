Shader "Custom/Water"
{
	Properties
	{
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		_SurfaceColorDark("Surface Color Dark", Color) = (1,1,1,1)
		_SurfaceColorLight("Surface Color Light", Color) = (1,1,1,1)
		_SideColor("Side Color", Color) = (1,1,1,1)
		_NoiseTex("Noise Tex", 2D) = "white" {}
		_NormalTex("Normal Tex", 2D) = "bump" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
		#pragma target 3.0

		sampler2D _NoiseTex;
		sampler2D _NormalTex;

		float4 noise(sampler2D tex, float3 worldPos)
		{
			float t = _Time.x;
			float2 coords1 = worldPos.xz * +0.20 + float2(t * -4, t * -3);
			float2 coords2 = worldPos.zx * -0.08 + float2(t * 0.8, t * 1.2);
			float4 result1 = tex2Dlod(tex, float4(coords1, 0, 0));
			float4 result2 = tex2Dlod(tex, float4(coords2 + result1.r * 0.1, 0, 0));

			return lerp(result1, result2, 0.75);
		}

		void vert(inout appdata_full v)
		{
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float disp = noise(_NoiseTex, worldPos).r;
        	v.vertex.z += disp * v.color.r;
    	}

		struct Input
		{
			float3 worldPos;
			float4 color : COLOR;
		};
		
		fixed4 _FoamColor;
		fixed4 _SurfaceColorDark;
		fixed4 _SurfaceColorLight;
		fixed4 _SideColor;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float noi = noise(_NoiseTex, IN.worldPos).r;
			float4 nor = noise(_NormalTex, IN.worldPos);

			float foamNoise = noise(_NoiseTex, IN.worldPos * -3 + _Time.z * 2).r;
			foamNoise = saturate((foamNoise - 0.3) * 10);
			float foam = foamNoise * (1 - saturate(abs(noi - 0.3) * 20));
			foam *= IN.color.r;

			float c = saturate(abs(noi - 0.2) * 10);
			o.Albedo = lerp(_SurfaceColorDark, _SurfaceColorLight, c);
			o.Albedo = lerp(o.Albedo, _FoamColor * 2, foam);
			o.Albedo = lerp(_SideColor, o.Albedo, pow(IN.color.r, 8));

			o.Normal = UnpackScaleNormal(nor, IN.color.r * 0.25);
			o.Smoothness = lerp(0.9, 0.1, foam);
			o.Metallic = 0;
		}

		ENDCG
	}

	FallBack "Diffuse"
}