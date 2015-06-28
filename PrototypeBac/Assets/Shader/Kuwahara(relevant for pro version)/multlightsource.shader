Shader "Cg per-pixel lighting with vertex lights" {
	Properties {
		_MainTex ("Texture", 2D) = "white" { }
		_Color ("Diffuse Material Color", Color) = (1,1,1,1) 
		_SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
		_Shininess ("Shininess", Float) = 0
	}
	SubShader {
		Pass {      
			Tags { "LightMode" = "ForwardBase" } // pass for 
            // 4 vertex lights, ambient light & first pixel light
 
			CGPROGRAM
			#pragma target 3.0
			#pragma multi_compile_fwdbase 
			#pragma vertex vert
			#pragma fragment frag
 
			#include "UnityCG.cginc" 
			uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
			// User-specified properties
			uniform float4 _Color; 
			uniform float4 _SpecColor; 
			uniform float _Shininess;
			uniform sampler2D _MainTex;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posWorld : TEXCOORD0;
            float3 normalDir : TEXCOORD1;
            float3 vertexLighting : TEXCOORD2;
			float2 uv : TEXCOORD3;
         };
 
		float4 _MainTex_ST;
 
         vertexOutput vert(appdata_base v)
         {          
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // unity_Scale.w is unnecessary here
 
            output.posWorld = mul(modelMatrix, v.vertex);
            output.normalDir = normalize(float3(
               mul(float4(v.normal, 0.0), modelMatrixInverse)));
            output.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			output.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
 
            // Diffuse reflection by four "vertex lights"            
            output.vertexLighting = float3(0.0, 0.0, 0.0);
            #ifdef VERTEXLIGHT_ON
            for (int index = 0; index < 4; index++)
            {    
               float4 lightPosition = float4(unity_4LightPosX0[index], 
                  unity_4LightPosY0[index], 
                  unity_4LightPosZ0[index], 1.0);
 
               float3 vertexToLightSource = 
                  float3(lightPosition - output.posWorld);        
               float3 lightDirection = normalize(vertexToLightSource);
               float squaredDistance = 
                  dot(vertexToLightSource, vertexToLightSource);
               float attenuation = 1.0 / (1.0 + 
                  unity_4LightAtten0[index] * squaredDistance);
               float3 diffuseReflection =  
                  attenuation * float3(unity_LightColor[index]) 
                  * float3(_Color) * max(0.0, 
                  dot(output.normalDir, lightDirection));         
 
               output.vertexLighting = 
                  output.vertexLighting + diffuseReflection;
            }
            #endif
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normalDir); 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos - float3(input.posWorld));
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = 
                  normalize(float3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  float3(_WorldSpaceLightPos0 - input.posWorld);
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            float3 ambientLighting = 
                float3(UNITY_LIGHTMODEL_AMBIENT) * float3(_Color);
 
            float3 diffuseReflection = 
               attenuation * float3(_LightColor0) * float3(_Color) 
               * max(0.0, dot(normalDirection, lightDirection));
 
            float3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = float3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               specularReflection = attenuation * float3(_LightColor0) 
                  * float3(_SpecColor) * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
			
			//KUWAHARA
			float radius = 2.0f;
			half2 src_size = half2(1024.0, 1024.0);
			float2 uv = input.uv;
			float n = float((radius + 1) * (radius + 1));

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
			//KUWAHARA
 
            return color * float4(input.vertexLighting + ambientLighting 
               + diffuseReflection + specularReflection, 1.0);
         }
		 
		 
		 
         ENDCG
      }
 
      Pass {    
         Tags { "LightMode" = "ForwardAdd" } 
            // pass for additional light sources
         Blend One One // additive blending 
 
          CGPROGRAM
		  #pragma target 3.0
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc" 
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform float4 _Color; 
         uniform float4 _SpecColor; 
         uniform float _Shininess;
		 uniform sampler2D _MainTex;
 
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posWorld : TEXCOORD0;
            float3 normalDir : TEXCOORD1;
			float2 uv : TEXCOORD3;
         };
 
		float4 _MainTex_ST;

		
         vertexOutput vert(appdata_base v) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.posWorld = mul(modelMatrix, v.vertex);
            output.normalDir = normalize(float3(
               mul(float4(v.normal, 0.0), modelMatrixInverse)));
            output.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			output.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normalDir);
 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos - float3(input.posWorld));
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = 
                  normalize(float3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  float3(_WorldSpaceLightPos0 - input.posWorld);
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            float3 diffuseReflection = 
               attenuation * float3(_LightColor0) * float3(_Color)
               * max(0.0, dot(normalDirection, lightDirection));
 
            float3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = float3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               specularReflection = attenuation * float3(_LightColor0) 
                  * float3(_SpecColor) * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
 
            return tex2D(_MainTex, input.uv) * float4(diffuseReflection 
               + specularReflection, 1.0);
               // no ambient lighting in this pass
         }
 
 
         ENDCG
      }
	  
 
   } 
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Specular"
}