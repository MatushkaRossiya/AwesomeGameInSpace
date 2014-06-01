Shader "Custom/AmmoCounter" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0.01,0.97)) = 0.5
		_Color ("Color", Color) = (1, 0, 0)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" }
		ZWrite Off
		Pass{
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag 

			sampler2D _MainTex;
			float _Cutoff;
			half4 _Color;

			struct vertexInput {
				float4 vertex : POSITION;
				float2 tex : TEXCOORD0;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
			};

			vertexOutput vert(vertexInput input) 
			{
				vertexOutput output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.tex;
				return output;
			}
    
			float4 frag(vertexOutput input) : COLOR
			{
				float4 color = _Color * 2;   
				if(tex2D(_MainTex, input.tex).r < _Cutoff) discard;
				return color;
			}
			ENDCG
		}
	}
}
