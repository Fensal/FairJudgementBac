Shader "Custom/Kuwahara" {
Properties {
    _MainTex ("Texture", 2D) = "white" { }
	//_SobelTex ("Texture", 2D) = "white" { }
}
SubShader {
    Pass {
		Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members d)
			#pragma exclude_renderers d3d11 xbox360
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			//sampler2D _SobelTex;

			struct v2f {
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				//float d;
			};

			float4 _MainTex_ST;
			//float4 _SobelTex_ST;

			v2f vert (appdata_base v)
			{
				//float4 lightPosition = float4(unity_4LightPosX0[1], unity_4LightPosY0[1], unity_4LightPosZ0[1], 1.0);
				//float3 vertexToLight = float3(lightPosition-v.vertex);


				v2f o;
				//o.d = dot(v.normal, normalize(vertexToLight));
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				int radius = 2;
				half2 src_size = half2(512.0, 384.0);
				float2 uv = i.uv;
				int n = int((radius + 1) * (radius + 1));

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
				half4 color = half4(1.0, 1.0, 1.0, 1.0);
				for (int k = 0; k < 4; ++k) {
					m[k] /= n;
					s[k] = abs(s[k] / n - m[k] * m[k]);

					float sigma2 = s[k].r + s[k].g + s[k].b;
					if (sigma2 < min_sigma2) {
						min_sigma2 = sigma2;
						color = half4(m[k], 1.0);
					}
				}
				//half4 sobel = tex2D(_SobelTex, uv)*1;
				
				//float value = clamp((sobel.x+sobel.y+sobel.z)/3, 0, 1);
				
				return half4(0.0f, 1.0f, 1.0f, 1.0f);//color;//*value;
			}
			ENDCG

			}
		}
	Fallback "VertexLit"
}