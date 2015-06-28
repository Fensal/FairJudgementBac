Shader "Custom/Merge" {
	Properties {
		_MainTex ("Texture", 2D) = "white" { }
		_NormalTex ("Texture", 2D) = "white" { }
		_CharTex ("Texture", 2D) = "white" { }
		_TunnelTex ("Texture", 2D) = "white" { }
		_Radius ("Radius", Int) = 2
		_Edgestrength ("Edgestrength", Int) = 1
		_Threshold ("Threshold", Range(0.0, 0.1)) = 0.025
		_Health ("Health", Range(0.0, 1.0)) = 1.0
	}
    SubShader {
		Tags { 
			"LightMode" = "ForwardBase"
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
		}
		
        Pass {
			
			CGPROGRAM

			#pragma target 4.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _CharTex;
			sampler2D _TunnelTex;
			float _Radius;
			float _Health;
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};
			
			float4 _MainTex_ST;
			float4 _CharTex_ST;
			float4 _TunnelTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				int radius = 3;
				half2 src_size = half2(1920.0, 1080.0);
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

				half4 CharacterColor = tex2D(_CharTex, float2(uv.x, 1.0f - uv.y));

				float character = CharacterColor.a;
				float rest = 1.0f - character;

				return (color*rest + tex2D(_MainTex, uv)*character) * 1.0f - (tex2D(_TunnelTex, uv).r * (1.0f - _Health));
			}
			ENDCG
        }
		
		Pass {
			Blend DstColor Zero
		
			CGPROGRAM

			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _NormalTex;
			sampler2D _CharTex;
			float _Edgestrength;
			float _Threshold;
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};
			
			float4 _MainTex_ST;
			float4 _NormalTex_ST;
			float4 _CharTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				const float3x3 g0 = float3x3(0.3535533845424652, 0, -0.3535533845424652, 0.5, 0, -0.5, 0.3535533845424652, 0, -0.3535533845424652 );
				const float3x3 g1 = float3x3(0.3535533845424652, 0.5, 0.3535533845424652, 0, 0, 0, -0.3535533845424652, -0.5, -0.3535533845424652 );
				const float3x3 g2 = float3x3(0, 0.3535533845424652, -0.5, -0.3535533845424652, 0, 0.3535533845424652, 0.5, -0.3535533845424652, 0 );
				const float3x3 g3 = float3x3(0.5, -0.3535533845424652, 0, -0.3535533845424652, 0, 0.3535533845424652, 0, 0.3535533845424652, -0.5 );
				const float3x3 g4 = float3x3(0, -0.5, 0, 0.5, 0, 0.5, 0, -0.5, 0 );
				const float3x3 g5 = float3x3(-0.5, 0, 0.5, 0, 0, 0, 0.5, 0, -0.5 );
				const float3x3 g6 = float3x3(0.1666666716337204, -0.3333333432674408, 0.1666666716337204, -0.3333333432674408, 0.6666666865348816, -0.3333333432674408, 0.1666666716337204, -0.3333333432674408, 0.1666666716337204 );
				const float3x3 g7 = float3x3(-0.3333333432674408, 0.1666666716337204, -0.3333333432674408, 0.1666666716337204, 0.6666666865348816, 0.1666666716337204, -0.3333333432674408, 0.1666666716337204, -0.3333333432674408 );
				const float3x3 g8 = float3x3(0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408 );
				
				const float texelX=float(1.0/1920.0);
				const float texelY=float(1.0/1080.0);
				
				float2 uv = i.uv;
				
				float3x3 G[9];
				G[0]=g0;
				G[1]=g1;
				G[2]=g2;
				G[3]=g3;
				G[4]=g4;
				G[5]=g5;
				G[6]=g6;
				G[7]=g7;
				G[8]=g8;
				
				float3x3 I;
				float cnv[9];
				half3 sample;
				
				for (float i=0.0; i<3.0; i++) {
					for (float j=0.0; j<3.0; j++) {
						sample = tex2D (_NormalTex, float2(uv.x, 1.0f - uv.y) + float2(texelX, texelY) * float2(i-1.0f, j-1.0f)).rgb;
						I[int(i)][int(j)] = length(sample);
					}
				}
				
				for (int i=0; i<9; i++) {
					float dp3 = dot(G[i][0], I[0]) + dot(G[i][1], I[1]) + dot(G[i][2], I[2]);
					cnv[i] = dp3 * dp3;
				}
				
				float M = (cnv[0] + cnv[1]) + (cnv[2] + cnv[3]);
				float S = (cnv[4] + cnv[5]) + (cnv[6] + cnv[7]) + (cnv[8] + M);
				
				
				
				half temp = sqrt(M/S);
				
				if(temp > _Threshold)
				{
					temp = temp*_Edgestrength;
				}
				else
				{
					temp = 0.0;
				}
				
				//float character = 1.0f-tex2D(_CharTex, float2(uv.x, 1.0f - uv.y)).a;

				half4 CharacterColor = tex2D(_CharTex, float2(uv.x, 1.0f - uv.y));
				half4 ScreenColor = tex2D(_MainTex, float2(uv.x, uv.y));
				float DifR = ScreenColor.r - CharacterColor.r;
				float DifG = ScreenColor.g - CharacterColor.g;
				float DifB = ScreenColor.b - CharacterColor.b;
				float DifAvg = (DifR+DifG+DifB)/3.0f;

				float character = 1.0f - step(DifAvg, 0.01f);
				
				return half4(1.0f-temp*character, 1.0f-temp*character, 1.0f-temp*character, tex2D(_MainTex, uv).a );
			}
			ENDCG

        }
		
		/*Pass {
		Blend DstColor Zero
		CGPROGRAM

		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _CharTex;
		
		struct v2f {
			float4 pos : SV_POSITION;
			float2  uv : TEXCOORD0;
		};
		
		float4 _MainTex_ST;
		float4 _NormalTex_ST;
		float4 _CharTex_ST;

		v2f vert (appdata_base v)
		{
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			return o;
		}
		
		half4 sobel(float2 uv)
		{
			float x=float(1.0/1024.0);
			float y=float(1.0/768.0);
			int edgestrength = 5;
			float2 newUV = float2(uv.x, 1.0f-uv.y);
			
			half4 p1= tex2D(_NormalTex, float2(newUV.x-x, newUV.y-y));
			half4 p2= tex2D(_NormalTex, float2(newUV.x, newUV.y-y));
			half4 p3= tex2D(_NormalTex, float2(newUV.x+x, newUV.y-y));
			
			half4 p4= tex2D(_NormalTex, float2(newUV.x-x, newUV.y));
			half4 p5= tex2D(_NormalTex, float2(newUV.x, newUV.y));
			half4 p6= tex2D(_NormalTex, float2(newUV.x+x, newUV.y));
			
			half4 p7= tex2D(_NormalTex, float2(newUV.x-x, newUV.y+y));
			half4 p8= tex2D(_NormalTex, float2(newUV.x, newUV.y+y));
			half4 p9= tex2D(_NormalTex, float2(newUV.x+x, newUV.y+y));
			
			return half4(half3(1.0, 1.0f, 1.0f).xyz-clamp((p1+(p2+p2)+p3-p7-(p8+p8)-p9)+(p3+(p6+p6)+p9-p1-(p4+p4)-p7)*edgestrength, 0, 1).xyz, 1.0);
		}

		half4 frag (v2f i) : COLOR
		{
			float2 uv = i.uv;
			
			//float character = 1.0f-tex2D(_CharTex, float2(uv.x, 1.0f - uv.y)).a;

			half4 CharacterColor = tex2D(_CharTex, float2(uv.x, 1.0f - uv.y));
			half4 ScreenColor = tex2D(_MainTex, float2(uv.x, uv.y));
			float DifR = ScreenColor.r - CharacterColor.r;
			float DifG = ScreenColor.g - CharacterColor.g;
			float DifB = ScreenColor.b - CharacterColor.b;
			float character = (DifR+DifG+DifB)/3.0f;

			half4 color = sobel(uv);
				
			return half4(character, character, character, 1.0f);

			return half4(color.x*character, color.y*character, color.z*character, tex2D(_MainTex, uv).a );
		
			return sobel(uv);
		}
		ENDCG

        }*/
		
		
    }
    Fallback "VertexLit"
}