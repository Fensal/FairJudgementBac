Shader "Custom/FreiChen" {
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
			const float3x3 g0 = float3x3(0.3535533845424652, 0, -0.3535533845424652, 0.5, 0, -0.5, 0.3535533845424652, 0, -0.3535533845424652 );
			const float3x3 g1 = float3x3(0.3535533845424652, 0.5, 0.3535533845424652, 0, 0, 0, -0.3535533845424652, -0.5, -0.3535533845424652 );
			const float3x3 g2 = float3x3(0, 0.3535533845424652, -0.5, -0.3535533845424652, 0, 0.3535533845424652, 0.5, -0.3535533845424652, 0 );
			const float3x3 g3 = float3x3(0.5, -0.3535533845424652, 0, -0.3535533845424652, 0, 0.3535533845424652, 0, 0.3535533845424652, -0.5 );
			const float3x3 g4 = float3x3(0, -0.5, 0, 0.5, 0, 0.5, 0, -0.5, 0 );
			const float3x3 g5 = float3x3(-0.5, 0, 0.5, 0, 0, 0, 0.5, 0, -0.5 );
			const float3x3 g6 = float3x3(0.1666666716337204, -0.3333333432674408, 0.1666666716337204, -0.3333333432674408, 0.6666666865348816, -0.3333333432674408, 0.1666666716337204, -0.3333333432674408, 0.1666666716337204 );
			const float3x3 g7 = float3x3(-0.3333333432674408, 0.1666666716337204, -0.3333333432674408, 0.1666666716337204, 0.6666666865348816, 0.1666666716337204, -0.3333333432674408, 0.1666666716337204, -0.3333333432674408 );
			const float3x3 g8 = float3x3(0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408, 0.3333333432674408 );
			
			const float texelX=float(1.0/1024.0);
			const float texelY=float(1.0/768.0);
			
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
					sample = tex2D (_MainTex, uv + float2(texelX, texelY) * float2(i-1.0f, j-1.0f)).rgb;
					I[int(i)][int(j)] = length(sample);
				}
			}
			
			for (int i=0; i<9; i++) {
				float dp3 = dot(G[i][0], I[0]) + dot(G[i][1], I[1]) + dot(G[i][2], I[2]);
				cnv[i] = dp3 * dp3;
			}
			
			float M = (cnv[0] + cnv[1]) + (cnv[2] + cnv[3]);
			float S = (cnv[4] + cnv[5]) + (cnv[6] + cnv[7]) + (cnv[8] + M);
			
			
			half3 temp = sqrt(M/S);
			return half4(1.0f-temp.x, 1.0f-temp.y, 1.0f-temp.z, tex2D(_MainTex, uv).a );
		}
		ENDCG

        }
    }
    Fallback "VertexLit"
}