Shader "Custom/AmmoCounter" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0.01,0.97)) = 0.5
		_Color ("Color", Color) = (1, 0, 0)
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Simple alphatest:_Cutoff

		half4 LightingSimple (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half4 c;
			c.rgb = s.Albedo;
			c.a = 1;
			return c;
		}

		struct Input {
			float2 uv_MainTex;
		};
    
		sampler2D _MainTex;
		float _Threshold;
		half3 _Color;
    
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color;
			o.Alpha = tex2D (_MainTex, IN.uv_MainTex).r;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
