Shader "Custom/Sobel" {
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
		
		half4 sobel(float2 uv)
		{
			float x=float(1.0/1024.0);
			float y=float(1.0/768.0);
			int edgestrength = 5;
			
			half4 p1= tex2D(_MainTex, float2(uv.x-x, uv.y-y));
			half4 p2= tex2D(_MainTex, float2(uv.x, uv.y-y));
			half4 p3= tex2D(_MainTex, float2(uv.x+x, uv.y-y));
			
			half4 p4= tex2D(_MainTex, float2(uv.x-x, uv.y));
			half4 p5= tex2D(_MainTex, float2(uv.x, uv.y));
			half4 p6= tex2D(_MainTex, float2(uv.x+x, uv.y));
			
			half4 p7= tex2D(_MainTex, float2(uv.x-x, uv.y+y));
			half4 p8= tex2D(_MainTex, float2(uv.x, uv.y+y));
			half4 p9= tex2D(_MainTex, float2(uv.x+x, uv.y+y));
			
			return half4(half3(1.0, 1.0f, 1.0f).xyz-clamp((p1+(p2+p2)+p3-p7-(p8+p8)-p9)+(p3+(p6+p6)+p9-p1-(p4+p4)-p7)*edgestrength, 0, 1).xyz, 1.0);
		}

		half4 frag (v2f i) : COLOR
		{
			float2 uv = i.uv;
			
			
		
			return sobel(uv);
			//return half4 (1);
		}
		ENDCG

        }
    }
    Fallback "VertexLit"
}