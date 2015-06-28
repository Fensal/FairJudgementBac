Shader "Custom/GaussianBlur" {
	Properties {
		_MainTex ("Texture", 2D) = "white" { }
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
		
			float kernel[3][3];
			
			kernel[0][0] = 0.0625f;
			kernel[0][1] = 0.125f;
			kernel[0][2] = 0.0625f;
			
			kernel[1][0] = 0.125f;
			kernel[1][1] = 0.25f;
			kernel[1][2] = 0.125f;
			
			kernel[2][0] = 0.0625f;
			kernel[2][1] = 0.125f;
			kernel[2][2] = 0.0625f;
			
			half3 result = half3(0.0f, 0.0f, 0.0f);
			
			for(int x = 0; x<3; x++)
			{
				for(int y = 0; y<3; y++)
				{
					half3 c = tex2D (_MainTex, uv + float2(x/src_size.x, y/src_size.y)).rgb;
					
					result += c * kernel[x][y];
				}
			}
			
			//return tex2D (_MainTex, uv);
			return half4(result.x, result.y, result.z, 1.0f);
		}
		ENDCG

        }
    }
    Fallback "VertexLit"
}