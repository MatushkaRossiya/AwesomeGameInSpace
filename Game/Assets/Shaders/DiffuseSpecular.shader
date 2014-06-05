Shader "Custom/DiffuseSpecular" {
	Properties {
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Normal ("Normalmap", 2D) = "bump" {}
		_Specular ("Specular", 2D) = "bump" {}
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	}
	SubShader { 
		Tags { "RenderType"="Opaque" }
		LOD 600
	
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Normal;
		sampler2D _Specular;
		half _Gloss;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Normal;
			float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			float2 specular_gloss = tex2D(_Specular, IN.uv_MainTex).rg;
			o.Gloss = specular_gloss.r;
			o.Alpha = 1;
			o.Specular = specular_gloss.g;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));
		}
		ENDCG
	}

	FallBack "Bumped Specular"
}
