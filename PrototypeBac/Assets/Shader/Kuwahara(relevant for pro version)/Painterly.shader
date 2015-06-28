Shader "Custom/Painterly" {
	Properties {
		_MainTex ("Texture", 2D) = "white" { }
		_GaussianTex ("Texture", 2D) = "white" { }
	}
    SubShader {
        Pass {
		Tags { "LightMode" = "ForwardBase" }
		CGPROGRAM

		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _GaussianTex;
		
		struct v2f {
			float4 pos : SV_POSITION;
			float2  uv : TEXCOORD0;
		};
		
		float4 _MainTex_ST;

		v2f vert (appdata_base v)
		{
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			return o;
		}

		half4 frag (v2f i) : COLOR
		{
			int radius = 5;
			half2 src_size = half2(1920.0, 1080.0);
			float2 uv = i.uv;
			
			//difference between source and gaussian image
			half3 D = tex2D (_MainTex, uv).rgb - tex2D (_GaussianTex, uv).rgb;
			
			//grid
			int BrushSize = 1;
			float grid = 0.01f * BrushSize;
			
			
			return half4(1.0f, 1.0f, 1.0f, 1.0f);
		}
		ENDCG

        }
    }
    Fallback "VertexLit"
}