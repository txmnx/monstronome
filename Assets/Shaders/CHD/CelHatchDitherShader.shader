// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// reference: https://blog.csdn.net/wolf96/article/details/43019719

Shader "Universal Render Pipeline/Custom/CelHatchingDitherShader" {
	Properties {
		_DirLightMultiplier("Directional Light Multiplier",range(0,50)) = 10
		//_DirLightAngleOffset("Directional Light Angle Offset",Vector) = (0,0,0,0)
		
		_Outline("Thick of Outline",range(0,0.1)) = 0.02	
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
		// texture
		[NoScaleOffset] _MainTex("Albedo (RGB)", 2D) = "white" {}
		[Toggle] _UseTexture("Use Texture?", float) = 1
		_AlbedoColor ("Albedo Color", Color) = (0, 0, 0, 1)
		
		/*[Toggle] _UseNormalMap("Use Normal Map?", float) = 0
		_GrainNormal("Grain Normal Map", 2D) = "bump" {}*/
		
		_ToonEffect("Toon Effect",range(0,1)) = 0.5	
		// num of layers
		_Steps("Steps of toon",range(0,9)) = 3
		// rim is the highlight on the edge
		_RimPower("RimPower",range(0,2)) = 0.4
		// num of layers of the rim
		_ToonRimStep("Steps of ToonRim",range(0,9)) = 3
		
		// lerp between the two shaders
		_ShaderBlend("Blend of cel and hatch",range(0,1)) = 0.5
		
		//_MainTex ("Texture", 2D) = "white" {}
		[NoScaleOffset] _Hatch0("Hatch 0", 2D) = "white" {}
		[NoScaleOffset] _Hatch1("Hatch 1", 2D) = "white" {}
		
		// dithering
		[NoScaleOffset] _DitherPattern ("Dithering Pattern", 2D) = "white" {}
        _Color1 ("Dither Color 1", Color) = (0, 0, 0, 1)
        _Color2 ("Dither Color 2", Color) = (1, 1, 1, 1)
        _DitherStep("Dither Step Value", range(0,1)) = 0.5
     	}



	SubShader{
	
	    Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
        LOD 300
	 
		// Outline
		pass {
			//Tags{ "LightMode" = "Always" }
			// Don’t render polygons facing towards the viewer.
			Cull Front
			// Controls whether pixels from this object are written to the depth buffer
			ZWrite On
			
			CGPROGRAM
			// claim vertex shader name
			#pragma vertex vert
			// claim fragment shader name
			#pragma fragment frag
			// Built-in shader include files
			#include "UnityCG.cginc"
			float _Outline;
			float4 _OutlineColor;
			
			struct v2f {
				// SV_POSITION -> tell the engine how to move data through the graphics pipeline
				float4 pos:SV_POSITION;
			};

			// vertex shader for outline
			v2f vert(appdata_full v) {
				v2f o;
				float3 dir = normalize(v.vertex.xyz);
				float3 vnormal = v.normal;
				float D = dot(dir,vnormal);
				// find the positive or negative for the direction
				dir = dir*sign(D);
				dir = dir*0.5 + vnormal*(1 - 0.5);
				v.vertex.xyz += dir*_Outline;
				// UnityObjectToClipPos -> Transforms a point from object space to the camera's clip space in homogeneous coordinates
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
            
			// fragment shader
			float4 frag(v2f i) :COLOR
			{
				float4 c = _OutlineColor;
				return c;
			}
			ENDCG
		}
        
		// first pass, outline, diffuse and rim
		pass {
			Tags{ "LightMode" = "UniversalForward" }
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _LightColor0;
			float _Steps;
			float _ToonEffect;
			sampler2D _MainTex;
			//sampler2D _GrainNormal;
			float4 _AlbedoColor;
			bool _UseTexture;
			//bool _UseNormalMap;
			float4 _MainTex_ST;
			float _RimPower;
			float _ToonRimStep;
			
			//Hatching
			float _ShaderBlend;
			sampler2D _Hatch0;
			sampler2D _Hatch1;

            // Dithering
            float _DitherStep;
            //The dithering pattern
            sampler2D _DitherPattern;
            float4 _DitherPattern_TexelSize;
            //Dither colors
            float4 _Color1;
            float4 _Color2;
                  
            //Directional Lights Modifiers
            float _DirLightMultiplier;
            //float4 _DirLightAngleOffset;
            
            fixed3 Hatching(float2 _uv, half _intensity)
			{
				half3 hatch0 = tex2D(_Hatch0, _uv).rgb;
				half3 hatch1 = tex2D(_Hatch1, _uv).rgb;

				half3 overbright = max(0, _intensity - 1.0);

				half3 weightsA = saturate((_intensity * 6.0) + half3(-0, -1, -2));
				half3 weightsB = saturate((_intensity * 6.0) + half3(-3, -4, -5));

				weightsA.xy -= weightsA.yz;
				weightsA.z -= weightsB.x;
				weightsB.xy -= weightsB.zy;

				hatch0 = hatch0 * weightsA;
				hatch1 = hatch1 * weightsB;

				half3 hatching = overbright + hatch0.r +
					hatch0.g + hatch0.b +
					hatch1.r + hatch1.g +
					hatch1.b;

				return hatching;
			}
			
			struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : TEXCOORD1;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
			struct v2f {
				float4 pos:SV_POSITION;
				//  first UV coordinate
				float3 lightDir:TEXCOORD0;
				float3 viewDir:TEXCOORD1;
				float3 normal:TEXCOORD2;
				float2 uv:TEXCOORD3;
				
				float4 screenPosition : TEXCOORD4;              
                float3 vertexLighting : TEXCOORD5;
                //UNITY_VERTEX_OUTPUT_STEREO
			};
			
			float4 GetColor(v2f i) {
			    if (_UseTexture)
			        return tex2D(_MainTex, i.uv);
                else return _AlbedoColor;
			}

			// initialize variables like position, normal, light direction and view direction based on basic vertex information and shader api
			v2f vert(appdata_full v, float4 tangent : TANGENT) {
				v2f o;
				
				/*UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);*/
				
				// world coordination
				o.pos = UnityObjectToClipPos(v.vertex);
                
				o.normal = v.normal;
				// ObjSpaceLightDir -> object space direction (not normalized) to light, given object space vertex position.
				o.lightDir = ObjSpaceLightDir(v.vertex);
				// ObjSpaceViewDir -> object space direction (not normalized) from given object space vertex position towards the camera.
				o.viewDir = ObjSpaceViewDir(v.vertex);
				// macro, scales and offsets texture coordinates.
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.screenPosition = ComputeScreenPos(o.pos);
                
                // Diffuse reflection by four "vertex lights"      
                o.vertexLighting = float3(0.0, 0.0, 0.0);
                #ifdef VERTEXLIGHT_ON
                for (int index = 0; index < 4; index++)
                {  
                  float4 lightPosition = float4(unity_4LightPosX0[index], 
                   unity_4LightPosY0[index], 
                   unity_4LightPosZ0[index], 1.0);
             
                  float3 vertexToLightSource = 
                  lightPosition.xyz - o.pos.xyz;    
                  float3 lightDirection = normalize(vertexToLightSource);
                  
                  //angle offset
                  lightDirection += _DirLightAngleOffset;
                  
                  float squaredDistance = 
                   dot(vertexToLightSource, vertexToLightSource);
                  float attenuation = 1.0 / (1.0 + 
                  unity_4LightAtten0[index] * squaredDistance);                  
                  float3 diffuseReflection = attenuation 
                   * unity_LightColor[index].rgb * _AlbedoColor 
                   * max(0.0, dot(o.normal, lightDirection));     
                  o.vertexLighting =  o.vertexLighting + diffuseReflection;
                  }
                #endif

				return o;
			}
			
			float4 frag(v2f i) :COLOR
			{
			    float3 N = normalize(i.normal);
                
				//float4 c = tex2D(_MainTex, i.uv);
				float4 c = GetColor(i);
				
				float3 viewDir = normalize(i.viewDir);
				float3 lightDir = normalize(i.lightDir);
				// get the diffuse
				float diffuse = max(0,dot(N,i.lightDir));
				// give diffuse light a base, to provide ambient light effect
				diffuse = (diffuse + 1) / 2;
				// smooth it to range 0 -1
				diffuse = smoothstep(0,1,diffuse);
				// change the toon to x steps
				float toon = floor(diffuse*_Steps) / _Steps;
				// lerp bthe toon effect
				diffuse = lerp(diffuse,toon,_ToonEffect);
				diffuse *= _DirLightMultiplier;

				// rim light, saturate same as clamp
				float rim = 1.0 - saturate(dot(N, normalize(viewDir)));
				rim = rim + 1;
				// size of the edge of the tim
				rim = pow(rim, _RimPower);
				float toonRim = floor(rim * _ToonRimStep) / _ToonRimStep;
				rim = lerp(rim, toonRim, _ToonEffect);
				// mix the color
				c = c*_LightColor0*diffuse*rim;
				
				//_WorldSpaceLightPos0 += _DirLightAngleOffset;
		
			  float attenuation;
              if (0.0 == _WorldSpaceLightPos0.w) // directional light?
              {
                attenuation = 1.0; // no attenuation
                lightDir = normalize(_WorldSpaceLightPos0.xyz);
              } 
              else // point or spot light
              {
                float3 vertexToLightSource = 
                 _WorldSpaceLightPos0.xyz - i.pos.xyz;
                float distance = length(vertexToLightSource);
                attenuation = 1.0 / distance; // linear attenuation 
                lightDir = normalize(vertexToLightSource);
              }

         
              float3 ambientLighting = 
                UNITY_LIGHTMODEL_AMBIENT.rgb * _AlbedoColor.rgb;
         
              float3 diffuseReflection = 
                attenuation * _LightColor0.rgb * _AlbedoColor.rgb 
                * max(0.0, dot(N, lightDir));
         
              float3 specularReflection;
              if (dot(N, lightDir) < 0.0) 
                // light source on the wrong side?
              {
                specularReflection = float3(0.0, 0.0, 0.0); 
                 // no specular reflection
              }
              else // light source on the right side
              {
                specularReflection = attenuation * _LightColor0.rgb 
                 * /*_SpecColor.rgb * */pow(max(0.0, dot(
                 reflect(-lightDir, N), 
                 viewDir)), 1);
              }
                c.rgb += i.vertexLighting + ambientLighting + diffuseReflection;
                
            // Hatching
            fixed intensity = dot(c, fixed3(0.2326, 0.7152, 0.0722));
            c.rgb =  lerp(Hatching(i.uv * 8, intensity), c.rgb, _ShaderBlend);
            
            //Dithering
            if(diffuse < _DitherStep) {
                //texture value the dithering is based on
                float texColor = GetColor(i).r;

                //value from the dither pattern
                float2 screenPos = i.screenPosition.xy / i.screenPosition.w;
                float2 ditherCoordinate = screenPos * _ScreenParams.xy * _DitherPattern_TexelSize.xy;
                float ditherValue = tex2D(_DitherPattern, ditherCoordinate).r;

                //combine dither pattern with texture value to get final result
                float ditheredValue = step(ditherValue, texColor);
                float4 ditherCol = lerp(_Color1, _Color2, ditheredValue);
                c.rgb = lerp(ditherCol, c.rgb, 0.5);
			}

				return c;
			}
				ENDCG
		}
		
	
		pass {
			Tags{ "LightMode" = "UniversalForward" }
			Blend One One
			Cull Back
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _LightColor0;
			float _Steps;
			float _ToonEffect;
			sampler2D _MainTex;
			//sampler2D _GrainNormal;
			float4 _AlbedoColor;
			bool _UseTexture;
			//bool _UseNormalMap;
			float4 _MainTex_ST;
			
			//Hatching
			float _ShaderBlend;
			sampler2D _Hatch0;
			sampler2D _Hatch1;
			
			//The dithering pattern
            sampler2D _DitherPattern;
            float4 _DitherPattern_TexelSize;

            //Dither colors
            float4 _Color1;
            float4 _Color2;
            
            //Directional Lights Modifiers
            float _DirLightMultiplier;
            float3 _DirLightAngleOffset;
            
            fixed3 Hatching(float2 _uv, half _intensity)
			{
				half3 hatch0 = tex2D(_Hatch0, _uv).rgb;
				half3 hatch1 = tex2D(_Hatch1, _uv).rgb;

				half3 overbright = max(0, _intensity - 1.0);

				half3 weightsA = saturate((_intensity * 6.0) + half3(-0, -1, -2));
				half3 weightsB = saturate((_intensity * 6.0) + half3(-3, -4, -5));

				weightsA.xy -= weightsA.yz;
				weightsA.z -= weightsB.x;
				weightsB.xy -= weightsB.zy;

				hatch0 = hatch0 * weightsA;
				hatch1 = hatch1 * weightsB;

				half3 hatching = overbright + hatch0.r +
					hatch0.g + hatch0.b +
					hatch1.r + hatch1.g +
					hatch1.b;

				return hatching;
			}
			
			struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : TEXCOORD1;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };
		
			struct v2f {
				float4 pos:SV_POSITION;
				//  first UV coordinate
				float3 lightDir:TEXCOORD0;
				float3 viewDir:TEXCOORD1;
				float3 normal:TEXCOORD2;
				float2 uv:TEXCOORD3;
				
				float4 screenPosition : TEXCOORD4;              
                float3 vertexLighting : TEXCOORD5;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			float4 GetColor(v2f i) {
			    if (_UseTexture)
			        return tex2D(_MainTex, i.uv);
                else return _AlbedoColor;
			}
			
			v2f vert(appdata_full v, float4 tangent : TANGENT) {
				v2f o;
				
				/*UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);*/
				
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.normal = v.normal;
				o.lightDir = ObjSpaceLightDir(v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
                o.screenPosition = ComputeScreenPos(o.pos);
                
                //main light Modifiers
                o.lightDir += _DirLightAngleOffset;
                
                // Diffuse reflection by four "vertex lights"      
                o.vertexLighting = float3(0.0, 0.0, 0.0);
                #ifdef VERTEXLIGHT_ON
                for (int index = 0; index < 4; index++)
                {  
                  float4 lightPosition = float4(unity_4LightPosX0[index], 
                   unity_4LightPosY0[index], 
                   unity_4LightPosZ0[index], 1.0);
             
                  float3 vertexToLightSource = 
                  lightPosition.xyz - o.pos.xyz;    
                  float3 lightDirection = normalize(vertexToLightSource);
                  
                  //main light Modifiers
                  lightDirection += _DirLightAngleOffset;
                  
                  float squaredDistance = 
                   dot(vertexToLightSource, vertexToLightSource);
                  float attenuation = 1.0 / (1.0 + 
                  unity_4LightAtten0[index] * squaredDistance);                  
                  float3 diffuseReflection = attenuation 
                   * unity_LightColor[index].rgb * _AlbedoColor 
                   * max(0.0, dot(o.normal, lightDirection));     
                  o.vertexLighting =  o.vertexLighting + diffuseReflection;
                  }
                #endif
                
				return o;
			}
			
			float4 frag(v2f i) :COLOR
			{
                float3 N = normalize(i.normal);
                
				// load the texture
				float4 c = GetColor(i);
				float3 viewDir = normalize(i.viewDir);
				float dist = length(i.lightDir);
				float3 lightDir = normalize(i.lightDir);
				float diffuse = max(0,dot(N,i.lightDir));
				diffuse = (diffuse + 1) / 2;
				diffuse = smoothstep(0,1,diffuse);
				float atten = 1 / (dist);
				float toon = floor(diffuse*atten*_Steps) / _Steps;
				diffuse = lerp(diffuse,toon,_ToonEffect);
				diffuse *= _DirLightMultiplier;
				

				half3 h = normalize(lightDir + viewDir);
				// half vector
				float nh = max(0, dot(N, h));
				// specular light
				float specular = pow(nh, 32.0);
				float toonSpec = floor(specular*atten * 2) / 2;
				specular = lerp(specular,toonSpec,_ToonEffect);

				c = c*_LightColor0*(diffuse + specular);	
				
				
				
                float attenuation;
     
                if (0.0 == _WorldSpaceLightPos0.w) // directional light?
                {
                  attenuation = 1.0; // no attenuation
                  lightDir = 
                   normalize(_WorldSpaceLightPos0.xyz);
                } 
                else // point or spot light
                {
                  float3 vertexToLightSource = 
                   _WorldSpaceLightPos0.xyz - i.pos.xyz;
                  float distance = length(vertexToLightSource);
                  attenuation = 1.0 / distance; // linear attenuation 
                  lightDir = normalize(vertexToLightSource);
                }
             
                float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _AlbedoColor.rgb;
           
                float3 diffuseReflection = attenuation * _LightColor0.rgb * _AlbedoColor.rgb * max(0.0, dot(N, lightDir));
             
                float3 specularReflection;
                if (dot(N, lightDir) < 0.0) 
                  // light source on the wrong side?
                {
                  specularReflection = float3(0.0, 0.0, 0.0); 
                   // no specular reflection
                }
                else // light source on the right side
                {
                   specularReflection = attenuation * _LightColor0.rgb 
                   * /*_SpecColor.rgb * */pow(max(0.0, dot(
                   reflect(-lightDir, N), 
                   viewDir)), 1);
                }
                c.rgb += i.vertexLighting + ambientLighting + diffuseReflection;
                    
                    
                    
                //Hatching
                fixed intensity = dot(c, fixed3(0.2326, 0.7152, 0.0722));
                c.rgb =  lerp(Hatching(i.uv * 8, intensity), c.rgb, _ShaderBlend);
                
                //Dithering
                //texture value the dithering is based on
                float texColor = GetColor(i).r;
    
                //value from the dither pattern
                float2 screenPos = i.screenPosition.xy / i.screenPosition.w;
                float2 ditherCoordinate = screenPos * _ScreenParams.xy * _DitherPattern_TexelSize.xy;
                float ditherValue = tex2D(_DitherPattern, ditherCoordinate).r;
    
                //combine dither pattern with texture value to get final result
                float ditheredValue = step(ditherValue, texColor);
                float4 ditherCol = lerp(_Color1, _Color2, ditheredValue);
                c.rgb = lerp(ditherCol, c.rgb, 0.5);	
                    
                return c;
			}
				ENDCG
		}
       
        
		// shadow
		Pass{	
		Tags{ "LightMode" = "ShadowCaster" }
		CGPROGRAM
		#pragma vertex vert  
		#pragma fragment frag  
		#pragma multi_compile_shadowcaster  
		#include "UnityCG.cginc"  
		sampler2D _Shadow;

		struct v2f
		{
			V2F_SHADOW_CASTER;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
			return o;
		}

		float4 frag(v2f i) : SV_Target
		{ 
			SHADOW_CASTER_FRAGMENT(i)
		}
			ENDCG
		}
		
	}
}