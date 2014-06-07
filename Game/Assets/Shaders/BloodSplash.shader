Shader "Custom/BloodSplash" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Normal ("Normalmap", 2D) = "bump" {}
		_Color ("Color", Color) = (1, 0, 0)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_SpecularMultiplier ("Specular Multiplier", float) = 1.0
		_GlossMultiplier ("Gloss Multiplier", float) = 1.0
	}
	SubShader {
		Tags { "Queue"="Transparent-100" "RenderType"="Transparent" }
		LOD 200
		ZWrite Off
		CGPROGRAM
		//#pragma surface surf Lambert alpha
		#pragma surface surf BlinnPhong alpha

		sampler2D _MainTex;
		sampler2D _Normal;
		sampler2D _CameraDepthTexture;
		float4 _Color;
		half _GlossMultiplier;
		half _SpecularMultiplier;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float depth = DECODE_EYEDEPTH(tex2Dproj(_CameraDepthTexture, IN.screenPos).r);
			float sceneZ = IN.screenPos.z + _ProjectionParams.y;
			float transparency = saturate(1 - (depth - sceneZ) * 10);
			half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//o.Emission = c;
			o.Albedo = c;
			o.Alpha = c.a * transparency;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
			o.Gloss = _SpecularMultiplier;
			o.Specular = _GlossMultiplier;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
