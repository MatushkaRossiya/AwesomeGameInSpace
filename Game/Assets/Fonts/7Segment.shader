Shader "GUI/7Segment" { 
	Properties { 
	   _MainTex ("Font Texture", 2D) = "white" {} 
	   _Color ("Text Color", Color) = (1,1,1,1) 
	} 

	SubShader { 
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" } 
		Lighting Off Cull Off ZWrite Off Fog { Mode Off } 
		Blend SrcAlpha OneMinusSrcAlpha 
		Pass { 
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert_img
			#pragma fragment frag

			sampler2D _MainTex;
			half3 _Color;

			float4 frag(v2f_img i) : COLOR {
				float4 c;
				c.a = tex2D(_MainTex, i.uv).a;
				c.rgb = _Color * 2;
				return c;
			}
			ENDCG
		} 
	} 
}