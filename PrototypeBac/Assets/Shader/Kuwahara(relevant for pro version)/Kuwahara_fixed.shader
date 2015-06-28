Shader "Custom/KuwaharaFixed" {
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
			half2 src_size = half2(1024.0, 768.0);
			float2 uv = i.uv;
			int n = int((radius + 1) * (radius + 1));
			half4 color = half4(1.0, 1.0, 1.0, 1.0);

			half3 m[4];
			half3 s[4];

			for (int k = 0; k < 4; ++k) {
				m[k] = half3(0.0, 0.0, 0.0);
				s[k] = half3(0.0, 0.0, 0.0);
			}

			for (int j = -radius; j <= 0; ++j)  {
				for (int i = -radius; i <= 0; ++i)  {
					half3 c = tex2D (_MainTex, uv + float2(i,j) / src_size).rgb;
					m[0] += c;
					s[0] += c * c;
				}
			}

			for (int j = -radius; j <= 0; ++j)  {
				for (int i = 0; i <= radius; ++i)  {
					half3 c = tex2D (_MainTex, uv + float2(i,j) / src_size).rgb;
					m[1] += c;
					s[1] += c * c;
				}
			}

			for (int j = 0; j <= radius; ++j)  {
				for (int i = 0; i <= radius; ++i)  {
					half3 c = tex2D (_MainTex, uv + float2(i,j) / src_size).rgb;
					m[2] += c;
					s[2] += c * c;
				}
			}

			for (int j = 0; j <= radius; ++j)  {
				for (int i = -radius; i <= 0; ++i)  {
					half3 c = tex2D (_MainTex, uv + float2(i,j) / src_size).rgb;
					m[3] += c;
					s[3] += c * c;
				}
			}

			float min_sigma2 = 1e+2;
			for (int k = 0; k < 4; ++k) {
				m[k] /= n;
				s[k] = abs(s[k] / n - m[k] * m[k]);

				float sigma2 = s[k].r + s[k].g + s[k].b;
				if (sigma2 < min_sigma2) {
					min_sigma2 = sigma2;
					color = half4(m[k], 1.0);
				}
			}
			
			
			return color;
		}
		ENDCG

        }
    }
    Fallback "VertexLit"
}