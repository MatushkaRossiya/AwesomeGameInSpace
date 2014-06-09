Shader "Hidden/SimpleGaussian" {
	Properties {
		_MainTex ("Screen", 2D) = "white" {}
		_Size  ("Blur Size", float) = 0.001
		_Steam ("Steam", 2D) = "white" {}
		_Steaminess ("Steaminess", range(0, 1)) = 0
	}


	CGINCLUDE
	#include "UnityCG.cginc" 
	//we include "UnityCG.cginc" to use the appdata_img struct
    
	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	float _Size;
	sampler2D _Steam;
	float _Steaminess;
   
	//Our Vertex Shader 
	v2f vert (appdata_img v){
		v2f o;
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o; 
	}
	ENDCG


	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings
		
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
	
			float4 frag (v2f i) : COLOR{
				float4 sum = float4(0, 0, 0, 0);

				float size = _Size * saturate(_Steaminess * 4 - tex2D(_Steam, i.uv).r * 3);
				sum += tex2D(_MainTex, i.uv + float2(size * -2, 0)) * 0.06136;
				sum += tex2D(_MainTex, i.uv + float2(size * -1, 0)) * 0.24477;
				sum += tex2D(_MainTex, i.uv) * 0.38774;
				sum += tex2D(_MainTex, i.uv + float2(size * 1, 0)) * 0.24477;
				sum += tex2D(_MainTex, i.uv + float2(size * 2, 0)) * 0.06136;

				return sum;
			}
			ENDCG
		}

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			    
			float4 frag (v2f i) : COLOR{
				float4 sum = float4(0, 0, 0, 0);
				
				float size = _Size * saturate(_Steaminess * 4 - tex2D(_Steam, i.uv).r * 3);

				sum += tex2D(_MainTex, i.uv + float2(0, size * -2)) * 0.06136;
				sum += tex2D(_MainTex, i.uv + float2(0, size * -1)) * 0.24477;
				sum += tex2D(_MainTex, i.uv) * 0.38774;
				sum += tex2D(_MainTex, i.uv + float2(0, size * 1)) * 0.24477;
				sum += tex2D(_MainTex, i.uv + float2(0, size * 2)) * 0.06136;

				return sum;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}