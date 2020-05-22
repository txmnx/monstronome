// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// reference: https://blog.csdn.net/wolf96/article/details/43019719

Shader "Universal Render Pipeline/Custom/Vibrating_CelHatchingDitherShader" {
	Properties {
		
		_Outline("Thick of Outline",range(0,0.1)) = 0.02	
		// texture
		[NoScaleOffset] _MainTex("Albedo (RGB)", 2D) = "white" {}
		[Toggle] _UseTexture("Use Texture?", float) = 1
		_AlbedoColor ("Albedo Color", Color) = (0, 0, 0, 1)
		[Toggle] _UseNormalMap("Use Normal Map?", float) = 0
		_GrainNormal("Grain Normal Map", 2D) = "bump" {}
		
		//Vibration
		_Frequency("Vibration Frequency", int) = 220
		_Speed("Vibration Speed", int) = 10
		_Amplitude("Vibration Amplitude", int) = 1
		
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
			//Vibration
            float _Frequency;
            float _Amplitude;
            float _Speed;
			
			
			struct v2f {
				// SV_POSITION -> tell the engine how to move data through the graphics pipeline
				float4 pos:SV_POSITION;
			};
			
			fixed4 Vibration(float4 vert)
            {   vert.x *= cos(_Time * _Speed) * _Amplitude + cos(vert.y*_Frequency) +.5;
                return vert;
            }
            

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
				v.vertex = Vibration(v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
            
			// fragment shader
			float4 frag(v2f i) :COLOR
			{
				float4 c = 0;
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
			sampler2D _GrainNormal;
			float4 _AlbedoColor;
			bool _UseTexture;
			bool _UseNormalMap;
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
            
            //Vibration
            float _Frequency;
            float _Amplitude;
            float _Speed;
            
            fixed4 Vibration(float4 vert)
            {   vert.x *= cos(_Time * _Speed) * _Amplitude + cos(vert.y*_Frequency);
                return vert;
            }
            
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
            
			struct v2f {
				float4 pos:SV_POSITION;
				//  first UV coordinate
				float3 lightDir:TEXCOORD0;
				float3 viewDir:TEXCOORD1;
				float3 normal:TEXCOORD2;
				float2 uv:TEXCOORD3;
				
				float4 screenPosition : TEXCOORD4;
				
                float3 worldPos : TEXCOORD5;
                // these three vectors will hold a 3x3 rotation matrix
                // that transforms from tangent to world space
                half3 tspace0 : TEXCOORD6; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD7; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD8; // tangent.z, bitangent.z, normal.z				
			};
			
			float4 GetColor(v2f i) {
			    if (_UseTexture)
			        return tex2D(_MainTex, i.uv);
                else return _AlbedoColor;
			}

			// initialize variables like position, normal, light direction and view direction based on basic vertex information and shader api
			v2f vert(appdata_full v, float4 tangent : TANGENT) {
				v2f o;
				// world coordination
				v.vertex = Vibration(v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
				
				if(_UseNormalMap){
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    half3 wNormal = UnityObjectToWorldNormal(v.normal);
                    half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
                    // compute bitangent from cross product of normal and tangent
                    half tangentSign = tangent.w * unity_WorldTransformParams.w;
                    half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                    // output the tangent space matrix
                    o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                    o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                    o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
                }
                
				o.normal = v.normal;
				// ObjSpaceLightDir -> object space direction (not normalized) to light, given object space vertex position.
				o.lightDir = ObjSpaceLightDir(v.vertex);
				// ObjSpaceViewDir -> object space direction (not normalized) from given object space vertex position towards the camera.
				o.viewDir = ObjSpaceViewDir(v.vertex);
				// macro, scales and offsets texture coordinates.
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.screenPosition = ComputeScreenPos(o.pos);
				return o;
			}
			
			float4 frag(v2f i) :COLOR
			{
			    float3 N;
			    
			    if(_UseNormalMap){
                    // sample the normal map, and decode from the Unity encoding
                    half3 tnormal = UnpackNormal(tex2D(_GrainNormal, i.uv));
                    // transform normal from tangent to world space
                    half3 worldNormal;
                    worldNormal.x = dot(i.tspace0, tnormal);
                    worldNormal.y = dot(i.tspace1, tnormal);
                    worldNormal.z = dot(i.tspace2, tnormal);
                    N = normalize(worldNormal);
                } else {
                    	N = normalize(i.normal);
                }
                
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

				// rim light, saturate same as clamp
				float rim = 1.0 - saturate(dot(N, normalize(viewDir)));
				rim = rim + 1;
				// size of the edge of the tim
				rim = pow(rim, _RimPower);
				float toonRim = floor(rim * _ToonRimStep) / _ToonRimStep;
				rim = lerp(rim, toonRim, _ToonEffect);
				// mix the color
				c = c*_LightColor0*diffuse*rim;
				
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
			sampler2D _GrainNormal;
			float4 _AlbedoColor;
			bool _UseTexture;
			bool _UseNormalMap;
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
		
			struct v2f {
				float4 pos:SV_POSITION;
				float3 lightDir:TEXCOORD0;
				float3 viewDir:TEXCOORD1;
				float3 normal:TEXCOORD2;
				float2 uv:TEXCOORD3;
				
				float4 screenPosition : TEXCOORD4;
				            
                float3 worldPos : TEXCOORD5;
                // these three vectors will hold a 3x3 rotation matrix
                // that transforms from tangent to world space
                half3 tspace0 : TEXCOORD6; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD7; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD8; // tangent.z, bitangent.z, normal.z	
			};
			
			float4 GetColor(v2f i) {
			    if (_UseTexture)
			        return tex2D(_MainTex, i.uv);
                else return _AlbedoColor;
			}
			
			v2f vert(appdata_full v, float4 tangent : TANGENT) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				if(_UseNormalMap){
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    half3 wNormal = UnityObjectToWorldNormal(v.normal);
                    half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
                    // compute bitangent from cross product of normal and tangent
                    half tangentSign = tangent.w * unity_WorldTransformParams.w;
                    half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                    // output the tangent space matrix
                    o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                    o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                    o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
                }
				
				o.normal = v.normal;
				o.lightDir = ObjSpaceLightDir(v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
                o.screenPosition = ComputeScreenPos(o.pos);
				return o;
			}
			
			float4 frag(v2f i) :COLOR
			{
			    float3 N;
			    
			    if(_UseNormalMap){
                    // sample the normal map, and decode from the Unity encoding
                    half3 tnormal = UnpackNormal(tex2D(_GrainNormal, i.uv));
                    // transform normal from tangent to world space
                    half3 worldNormal;
                    worldNormal.x = dot(i.tspace0, tnormal);
                    worldNormal.y = dot(i.tspace1, tnormal);
                    worldNormal.z = dot(i.tspace2, tnormal);
                    N = normalize(worldNormal);
                } else {
                    	N = normalize(i.normal);
                }
                
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

				half3 h = normalize(lightDir + viewDir);
				// half vector
				float nh = max(0, dot(N, h));
				// specular light
				float specular = pow(nh, 32.0);
				float toonSpec = floor(specular*atten * 2) / 2;
				specular = lerp(specular,toonSpec,_ToonEffect);

				c = c*_LightColor0*(diffuse + specular);
				
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