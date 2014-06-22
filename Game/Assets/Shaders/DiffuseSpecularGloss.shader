Shader "Custom/DiffuseSpecularGloss" {
	Properties {
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Normal ("Normalmap", 2D) = "bump" {}
		_Specular ("Specular", 2D) = "white" {}
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_SpecularMultiplier ("Specular Multiplier", float) = 1.0
		_GlossMultiplier ("Gloss Multiplier", float) = 1.0
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
		half _GlossMultiplier;
		half _SpecularMultiplier;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			float2 specular_gloss = tex2D(_Specular, IN.uv_MainTex).rg;
			o.Gloss = specular_gloss.r * _SpecularMultiplier;			// Really it is Specular
			o.Alpha = 1;
			o.Specular = specular_gloss.g * _GlossMultiplier + 0.01;	// Really it is Gloss
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
		}
		ENDCG
	}

	FallBack "Bumped Specular"
}
