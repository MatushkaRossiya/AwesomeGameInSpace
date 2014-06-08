Shader "Custom/Goo" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Visibility ("Visibility", range(0, 1)) = 1
	}
	SubShader {
		Cull Off Fog { Mode Off } //Rendering settings
 
		Pass{
			//BlendOp Add
			Blend DstColor Zero
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" 
			//we include "UnityCG.cginc" to use the appdata_img struct
    
			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
   
			//Our Vertex Shader 
			v2f vert (appdata_img v){
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
				return o; 
			}
    
			sampler2D _MainTex;
			float _Visibility;
    
			//Our Fragment Shader
			float4 frag (v2f i) : COLOR{
				return 1 - _Visibility + tex2D(_MainTex, i.uv) * _Visibility;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
