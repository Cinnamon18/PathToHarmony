Shader "Grayscale" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

         CGPROGRAM
         #pragma surface surf Lambert
         sampler2D _MainTex;
         struct Input {
             float2 uv_MainTex;
         };
         void surf (Input IN, inout SurfaceOutput o) {
             half4 c = tex2D (_MainTex, IN.uv_MainTex);
             o.Albedo = (c.r + c.g + c.b)/3;
			 
			 //I think this should act as a damper, but idk, i don't write shaders
			 float damper = (o.Albedo - 0.5) / 5;

			//It looks better lighter imo
			 o.Albedo = o.Albedo - damper + 0.1;
             o.Alpha = c.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"


}

