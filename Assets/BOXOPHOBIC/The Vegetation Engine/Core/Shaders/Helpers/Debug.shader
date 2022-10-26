// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/BOXOPHOBIC/The Vegetation Engine/Helpers/Debug"
{
	Properties
	{
		[StyledBanner(Debug)]_Banner("Banner", Float) = 0
		_IsSimpleShader("_IsSimpleShader", Float) = 0
		[HideInInspector]_IsTVEShader("_IsTVEShader", Float) = 0
		_IsStandardShader("_IsStandardShader", Float) = 0
		_IsSubsurfaceShader("_IsSubsurfaceShader", Float) = 0
		_IsPropShader("_IsPropShader", Float) = 0
		_IsBarkShader("_IsBarkShader", Float) = 0
		_IsImpostorShader("_IsImpostorShader", Float) = 0
		_IsVegetationShader("_IsVegetationShader", Float) = 0
		_IsGrassShader("_IsGrassShader", Float) = 0
		_IsLeafShader("_IsLeafShader", Float) = 0
		_IsCrossShader("_IsCrossShader", Float) = 0
		[NoScaleOffset]_MainNormalTex("_MainNormalTex", 2D) = "black" {}
		[NoScaleOffset]_EmissiveTex("_EmissiveTex", 2D) = "black" {}
		[NoScaleOffset]_SecondMaskTex("_SecondMaskTex", 2D) = "black" {}
		[NoScaleOffset]_SecondNormalTex("_SecondNormalTex", 2D) = "black" {}
		[NoScaleOffset]_SecondAlbedoTex("_SecondAlbedoTex", 2D) = "black" {}
		[NoScaleOffset]_MainAlbedoTex("_MainAlbedoTex", 2D) = "black" {}
		[NoScaleOffset]_MainMaskTex("_MainMaskTex", 2D) = "black" {}
		_RenderClip("_RenderClip", Float) = 0
		_IsElementShader("_IsElementShader", Float) = 0
		_IsHelperShader("_IsHelperShader", Float) = 0
		_Cutoff("_Cutoff", Float) = 0
		_DetailMode("_DetailMode", Float) = 0
		[StyledEnum(Default _Layer 1 _Layer 2 _Layer 3 _Layer 4 _Layer 5 _Layer 6 _Layer 7 _Layer 8)]_LayerMotionValue("Layer Motion", Float) = 0
		_EmissiveCat("_EmissiveCat", Float) = 0
		[HDR]_EmissiveColor("_EmissiveColor", Color) = (0,0,0,0)
		[HDR][Space(10)]_MainColor("Main Color", Color) = (1,1,1,1)
		[Space(10)][StyledToggle]_VertexColorMode("Use Vertex Colors as Albedo", Range( 0 , 1)) = 0
		[StyledToggle]_LeavesFilterMode("Use Color Filter for Leaves", Float) = 0
		[Space(10)]_LeavesFilterColor("Leaves Color Filter", Color) = (0,0,0,1)
		_LeavesFilterRange("Leaves Color Range", Range( 0 , 1)) = 0
		_IsPolygonalShader("_IsPolygonalShader", Float) = 0
		[IntRange]_MotionSpeed_10("Primary Speed", Range( 0 , 40)) = 40
		_MotionScale_10("Primary Scale", Range( 0 , 20)) = 0
		[ASEEnd][StyledMessage(Info, Use this shader to debug the original mesh or the converted mesh attributes., 0,0)]_Message("Message", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
		//[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		//[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}
	
	SubShader
	{
		
		Tags { "RenderType"="Opaque" "Queue"="Geometry" "DisableBatching"="True" }
	LOD 0

		Cull Off
		AlphaToMask Off
		ZWrite On
		ZTest LEqual
		ColorMask RGBA
		
		Blend Off
		

		CGINCLUDE
		#pragma target 5.0

		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		ENDCG

		
		Pass
		{
			
			Name "ForwardBase"
			Tags { "LightMode"="ForwardBase" }
			
			Blend One Zero

			CGPROGRAM
			#define ASE_NO_AMBIENT 1
			#if defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES) || defined(SHADER_API_GLES3) || defined(SHADER_API_D3D9)
			#define FRONT_FACE_SEMANTIC VFACE
			#define FRONT_FACE_TYPE float
			#else
			#define FRONT_FACE_SEMANTIC SV_IsFrontFace
			#define FRONT_FACE_TYPE bool
			#endif

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#ifndef UNITY_PASS_FORWARDBASE
				#define UNITY_PASS_FORWARDBASE
			#endif
			#include "HLSLSupport.cginc"
			#ifndef UNITY_INSTANCED_LOD_FADE
				#define UNITY_INSTANCED_LOD_FADE
			#endif
			#ifndef UNITY_INSTANCED_SH
				#define UNITY_INSTANCED_SH
			#endif
			#ifndef UNITY_INSTANCED_LIGHTMAPSTS
				#define UNITY_INSTANCED_LIGHTMAPSTS
			#endif
			#include "UnityShaderVariables.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			#include "AutoLight.cginc"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
			#define SAMPLE_TEXTURE2D_ARRAY(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
			#else//ASE Sampling Macros
			#define SAMPLE_TEXTURE2D_ARRAY(tex,samplertex,coord) tex2DArray(tex,coord)
			#endif//ASE Sampling Macros
			

			struct appdata {
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f {
				#if UNITY_VERSION >= 201810
					UNITY_POSITION(pos);
				#else
					float4 pos : SV_POSITION;
				#endif
				#if defined(LIGHTMAP_ON) || (!defined(LIGHTMAP_ON) && SHADER_TARGET >= 30)
					float4 lmap : TEXCOORD0;
				#endif
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
					half3 sh : TEXCOORD1;
				#endif
				#if defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS) && UNITY_VERSION >= 201810 && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					UNITY_LIGHTING_COORDS(2,3)
				#elif defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if UNITY_VERSION >= 201710
						UNITY_SHADOW_COORDS(2)
					#else
						SHADOW_COORDS(2)
					#endif
				#endif
				#ifdef ASE_FOG
					UNITY_FOG_COORDS(4)
				#endif
				float4 tSpace0 : TEXCOORD5;
				float4 tSpace1 : TEXCOORD6;
				float4 tSpace2 : TEXCOORD7;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD8;
				#endif
				float4 ase_texcoord9 : TEXCOORD9;
				float4 ase_texcoord10 : TEXCOORD10;
				float4 ase_texcoord11 : TEXCOORD11;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			uniform half _Message;
			uniform half _Banner;
			uniform float _IsSimpleShader;
			uniform half _IsTVEShader;
			uniform half TVE_DEBUG_Type;
			uniform float _IsBarkShader;
			uniform float _IsCrossShader;
			uniform float _IsGrassShader;
			uniform float _IsLeafShader;
			uniform float _IsPropShader;
			uniform float _IsImpostorShader;
			uniform float _IsPolygonalShader;
			uniform float _IsStandardShader;
			uniform float _IsSubsurfaceShader;
			uniform half TVE_DEBUG_Index;
			uniform sampler2D _MainAlbedoTex;
			uniform float4 _MainAlbedoTex_ST;
			uniform sampler2D _MainNormalTex;
			uniform float4 _MainNormalTex_ST;
			uniform sampler2D _MainMaskTex;
			uniform float4 _MainMaskTex_ST;
			uniform sampler2D _SecondAlbedoTex;
			uniform float4 _SecondAlbedoTex_ST;
			uniform sampler2D _SecondNormalTex;
			uniform float4 _SecondNormalTex_ST;
			uniform sampler2D _SecondMaskTex;
			uniform float4 _SecondMaskTex_ST;
			uniform float _DetailMode;
			uniform sampler2D _EmissiveTex;
			uniform float4 _EmissiveTex_ST;
			uniform float4 _EmissiveColor;
			uniform float _EmissiveCat;
			uniform half TVE_DEBUG_Min;
			uniform half TVE_DEBUG_Max;
			float4 _MainAlbedoTex_TexelSize;
			float4 _MainNormalTex_TexelSize;
			float4 _MainMaskTex_TexelSize;
			float4 _SecondAlbedoTex_TexelSize;
			float4 _SecondMaskTex_TexelSize;
			float4 _EmissiveTex_TexelSize;
			uniform half4 TVE_MotionParams;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(TVE_MotionTex);
			uniform half4 TVE_MotionCoords;
			uniform half _LayerMotionValue;
			SamplerState samplerTVE_MotionTex;
			uniform float TVE_MotionUsage[10];
			uniform float _MotionScale_10;
			uniform half4 TVE_NoiseParams;
			uniform float _MotionSpeed_10;
			uniform half4 TVE_ColorsParams;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(TVE_ColorsTex);
			uniform half4 TVE_ColorsCoords;
			uniform half TVE_DEBUG_Layer;
			SamplerState samplerTVE_ColorsTex;
			uniform float TVE_ColorsUsage[10];
			uniform half4 TVE_ExtrasParams;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(TVE_ExtrasTex);
			uniform half4 TVE_ExtrasCoords;
			SamplerState samplerTVE_ExtrasTex;
			uniform float TVE_ExtrasUsage[10];
			uniform half4 TVE_VertexParams;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(TVE_VertexTex);
			uniform half4 TVE_VertexCoords;
			SamplerState samplerTVE_VertexTex;
			uniform float TVE_VertexUsage[10];
			uniform float _IsVegetationShader;
			uniform half4 _LeavesFilterColor;
			uniform half4 _MainColor;
			uniform half _VertexColorMode;
			uniform float _LeavesFilterRange;
			uniform half _LeavesFilterMode;
			uniform half TVE_DEBUG_Filter;
			uniform half TVE_DEBUG_Clip;
			uniform float _RenderClip;
			uniform float _Cutoff;
			uniform float _IsElementShader;
			uniform float _IsHelperShader;

	
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			

			v2f VertexFunction (appdata v  ) {
				UNITY_SETUP_INSTANCE_ID(v);
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				UNITY_TRANSFER_INSTANCE_ID(v,o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 customSurfaceDepth676_g62695 = v.vertex.xyz;
				float customEye676_g62695 = -UnityObjectToViewPos( customSurfaceDepth676_g62695 ).z;
				o.ase_texcoord9.x = customEye676_g62695;
				float Debug_Index464_g62695 = TVE_DEBUG_Index;
				float3 ifLocalVar40_g63014 = 0;
				if( Debug_Index464_g62695 == 0.0 )
				ifLocalVar40_g63014 = saturate( v.vertex.xyz );
				float3 ifLocalVar40_g62989 = 0;
				if( Debug_Index464_g62695 == 1.0 )
				ifLocalVar40_g62989 = v.normal;
				float3 ifLocalVar40_g62995 = 0;
				if( Debug_Index464_g62695 == 2.0 )
				ifLocalVar40_g62995 = v.tangent.xyz;
				float ifLocalVar40_g63008 = 0;
				if( Debug_Index464_g62695 == 3.0 )
				ifLocalVar40_g63008 = saturate( v.tangent.w );
				float3 temp_cast_0 = (v.ase_color.r).xxx;
				float3 hsvTorgb260_g62695 = HSVToRGB( float3(v.ase_color.r,1.0,1.0) );
				float3 gammaToLinear266_g62695 = GammaToLinearSpace( hsvTorgb260_g62695 );
				float _IsBarkShader347_g62695 = _IsBarkShader;
				float _IsLeafShader360_g62695 = _IsLeafShader;
				float _IsCrossShader342_g62695 = _IsCrossShader;
				float _IsGrassShader341_g62695 = _IsGrassShader;
				float _IsVegetationShader1101_g62695 = _IsVegetationShader;
				float _IsAnyVegetationShader362_g62695 = saturate( ( _IsBarkShader347_g62695 + _IsLeafShader360_g62695 + _IsCrossShader342_g62695 + _IsGrassShader341_g62695 + _IsVegetationShader1101_g62695 ) );
				float3 lerpResult290_g62695 = lerp( temp_cast_0 , gammaToLinear266_g62695 , _IsAnyVegetationShader362_g62695);
				float3 ifLocalVar40_g62998 = 0;
				if( Debug_Index464_g62695 == 5.0 )
				ifLocalVar40_g62998 = lerpResult290_g62695;
				float ifLocalVar40_g62994 = 0;
				if( Debug_Index464_g62695 == 6.0 )
				ifLocalVar40_g62994 = v.ase_color.g;
				float ifLocalVar40_g62999 = 0;
				if( Debug_Index464_g62695 == 7.0 )
				ifLocalVar40_g62999 = v.ase_color.b;
				float ifLocalVar40_g63003 = 0;
				if( Debug_Index464_g62695 == 8.0 )
				ifLocalVar40_g63003 = v.ase_color.a;
				float ifLocalVar40_g63007 = 0;
				if( Debug_Index464_g62695 == 9.0 )
				ifLocalVar40_g63007 = v.ase_texcoord3.x;
				float ifLocalVar40_g63035 = 0;
				if( Debug_Index464_g62695 == 10.0 )
				ifLocalVar40_g63035 = v.ase_texcoord3.y;
				float ifLocalVar40_g62986 = 0;
				if( Debug_Index464_g62695 == 11.0 )
				ifLocalVar40_g62986 = v.ase_texcoord3.z;
				float3 vertexToFrag328_g62695 = ( ( ifLocalVar40_g63014 + ifLocalVar40_g62989 + ifLocalVar40_g62995 + ifLocalVar40_g63008 ) + ( ifLocalVar40_g62998 + ifLocalVar40_g62994 + ifLocalVar40_g62999 + ifLocalVar40_g63003 ) + ( ifLocalVar40_g63007 + ifLocalVar40_g63035 + ifLocalVar40_g62986 ) );
				o.ase_texcoord10.xyz = vertexToFrag328_g62695;
				float4 color1097_g62695 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 color1096_g62695 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float2 uv_MainMaskTex1077_g62695 = v.ase_texcoord.xy;
				float3 linearToGamma1066_g62695 = LinearToGammaSpace( _LeavesFilterColor.rgb );
				#ifdef UNITY_COLORSPACE_GAMMA
				float3 staticSwitch1060_g62695 = (_LeavesFilterColor).rgb;
				#else
				float3 staticSwitch1060_g62695 = linearToGamma1066_g62695;
				#endif
				float2 uv_MainAlbedoTex1045_g62695 = v.ase_texcoord.xy;
				float4 tex2DNode1045_g62695 = tex2Dlod( _MainAlbedoTex, float4( uv_MainAlbedoTex1045_g62695, 0, 0.0) );
				float3 lerpResult1043_g62695 = lerp( (tex2DNode1045_g62695).rgb , (v.ase_color).rgb , _VertexColorMode);
				half3 Main_Albedo1078_g62695 = ( (_MainColor).rgb * lerpResult1043_g62695 );
				float3 linearToGamma1058_g62695 = LinearToGammaSpace( Main_Albedo1078_g62695 );
				#ifdef UNITY_COLORSPACE_GAMMA
				float3 staticSwitch1068_g62695 = (Main_Albedo1078_g62695).xyz;
				#else
				float3 staticSwitch1068_g62695 = linearToGamma1058_g62695;
				#endif
				float lerpResult1071_g62695 = lerp( 1.0 , saturate( ( 1.0 - ceil( ( distance( staticSwitch1060_g62695 , staticSwitch1068_g62695 ) - _LeavesFilterRange ) ) ) ) , _LeavesFilterMode);
				half Main_ColorFilter1061_g62695 = lerpResult1071_g62695;
				float4 lerpResult1095_g62695 = lerp( color1097_g62695 , color1096_g62695 , ( tex2Dlod( _MainMaskTex, float4( uv_MainMaskTex1077_g62695, 0, 0.0) ).b * Main_ColorFilter1061_g62695 ));
				float4 vertexToFrag11_g63037 = lerpResult1095_g62695;
				o.ase_texcoord11 = vertexToFrag11_g63037;
				
				o.ase_texcoord9.yz = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord9.w = 0;
				o.ase_texcoord10.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.vertex.w = 1;
				v.normal = v.normal;
				v.tangent = v.tangent;

				o.pos = UnityObjectToClipPos(v.vertex);
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

				#ifdef DYNAMICLIGHTMAP_ON
				o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif
				#ifdef LIGHTMAP_ON
				o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				#ifndef LIGHTMAP_ON
					#if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
						o.sh = 0;
						#ifdef VERTEXLIGHT_ON
						o.sh += Shade4PointLights (
							unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
							unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
							unity_4LightAtten0, worldPos, worldNormal);
						#endif
						o.sh = ShadeSHPerVertex (worldNormal, o.sh);
					#endif
				#endif

				#if UNITY_VERSION >= 201810 && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					UNITY_TRANSFER_LIGHTING(o, v.texcoord1.xy);
				#elif defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if UNITY_VERSION >= 201710
						UNITY_TRANSFER_SHADOW(o, v.texcoord1.xy);
					#else
						TRANSFER_SHADOW(o);
					#endif
				#endif

				#ifdef ASE_FOG
					UNITY_TRANSFER_FOG(o,o.pos);
				#endif
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
					o.screenPos = ComputeScreenPos(o.pos);
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( appdata v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.tangent = v.tangent;
				o.normal = v.normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				o.ase_texcoord3 = v.ase_texcoord3;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, UNITY_MATRIX_M, _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, UNITY_MATRIX_M, _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, UNITY_MATRIX_M, _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			v2f DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				appdata o = (appdata) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.tangent = patch[0].tangent * bary.x + patch[1].tangent * bary.y + patch[2].tangent * bary.z;
				o.normal = patch[0].normal * bary.x + patch[1].normal * bary.y + patch[2].normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].normal * (dot(o.vertex.xyz, patch[i].normal) - dot(patch[i].vertex.xyz, patch[i].normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			v2f vert ( appdata v )
			{
				return VertexFunction( v );
			}
			#endif
			
			fixed4 frag (v2f IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC
				#ifdef _DEPTHOFFSET_ON
				, out float outputDepth : SV_Depth
				#endif
				) : SV_Target 
			{
				UNITY_SETUP_INSTANCE_ID(IN);

				#ifdef LOD_FADE_CROSSFADE
					UNITY_APPLY_DITHER_CROSSFADE(IN.pos.xy);
				#endif

				#if defined(_SPECULAR_SETUP)
					SurfaceOutputStandardSpecular o = (SurfaceOutputStandardSpecular)0;
				#else
					SurfaceOutputStandard o = (SurfaceOutputStandard)0;
				#endif
				float3 WorldTangent = float3(IN.tSpace0.x,IN.tSpace1.x,IN.tSpace2.x);
				float3 WorldBiTangent = float3(IN.tSpace0.y,IN.tSpace1.y,IN.tSpace2.y);
				float3 WorldNormal = float3(IN.tSpace0.z,IN.tSpace1.z,IN.tSpace2.z);
				float3 worldPos = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
				#else
					half atten = 1;
				#endif
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				float Debug_Type367_g62695 = TVE_DEBUG_Type;
				float4 color646_g62695 = IsGammaSpace() ? float4(0.9245283,0.7969696,0.4142933,1) : float4(0.8368256,0.5987038,0.1431069,1);
				float customEye676_g62695 = IN.ase_texcoord9.x;
				float saferPower688_g62695 = abs( (0.0 + (customEye676_g62695 - 300.0) * (1.0 - 0.0) / (0.0 - 300.0)) );
				float clampResult702_g62695 = clamp( pow( saferPower688_g62695 , 1.25 ) , 0.75 , 1.0 );
				float Shading655_g62695 = clampResult702_g62695;
				float4 Output_Converted717_g62695 = ( color646_g62695 * Shading655_g62695 );
				float4 ifLocalVar40_g63004 = 0;
				if( Debug_Type367_g62695 == 0.0 )
				ifLocalVar40_g63004 = Output_Converted717_g62695;
				float4 color466_g62695 = IsGammaSpace() ? float4(0.8113208,0.4952317,0.264062,0) : float4(0.6231937,0.2096542,0.05668841,0);
				float _IsBarkShader347_g62695 = _IsBarkShader;
				float4 color469_g62695 = IsGammaSpace() ? float4(0.6566009,0.3404236,0.8490566,0) : float4(0.3886527,0.09487338,0.6903409,0);
				float _IsCrossShader342_g62695 = _IsCrossShader;
				float4 color472_g62695 = IsGammaSpace() ? float4(0.7100264,0.8018868,0.2231666,0) : float4(0.4623997,0.6070304,0.0407874,0);
				float _IsGrassShader341_g62695 = _IsGrassShader;
				float4 color475_g62695 = IsGammaSpace() ? float4(0.3267961,0.7264151,0.3118103,0) : float4(0.08721471,0.4865309,0.07922345,0);
				float _IsLeafShader360_g62695 = _IsLeafShader;
				float4 color478_g62695 = IsGammaSpace() ? float4(0.3252937,0.6122813,0.8113208,0) : float4(0.08639329,0.3330702,0.6231937,0);
				float _IsPropShader346_g62695 = _IsPropShader;
				float4 color1114_g62695 = IsGammaSpace() ? float4(0.9716981,0.3162602,0.4816265,0) : float4(0.9368213,0.08154967,0.1974273,0);
				float _IsImpostorShader1110_g62695 = _IsImpostorShader;
				float4 color1117_g62695 = IsGammaSpace() ? float4(0.257921,0.8679245,0.8361252,0) : float4(0.05410501,0.7254258,0.6668791,0);
				float _IsPolygonalShader1112_g62695 = _IsPolygonalShader;
				float4 Output_Shader445_g62695 = ( ( ( color466_g62695 * _IsBarkShader347_g62695 ) + ( color469_g62695 * _IsCrossShader342_g62695 ) + ( color472_g62695 * _IsGrassShader341_g62695 ) + ( color475_g62695 * _IsLeafShader360_g62695 ) + ( color478_g62695 * _IsPropShader346_g62695 ) + ( color1114_g62695 * _IsImpostorShader1110_g62695 ) + ( color1117_g62695 * _IsPolygonalShader1112_g62695 ) ) * Shading655_g62695 );
				float4 ifLocalVar40_g62984 = 0;
				if( Debug_Type367_g62695 == 1.0 )
				ifLocalVar40_g62984 = Output_Shader445_g62695;
				float4 color544_g62695 = IsGammaSpace() ? float4(0.3252937,0.6122813,0.8113208,0) : float4(0.08639329,0.3330702,0.6231937,0);
				float _IsSimpleShader359_g62695 = _IsSimpleShader;
				float4 color521_g62695 = IsGammaSpace() ? float4(0.6566009,0.3404236,0.8490566,0) : float4(0.3886527,0.09487338,0.6903409,0);
				float _IsStandardShader344_g62695 = _IsStandardShader;
				float4 color1121_g62695 = IsGammaSpace() ? float4(0.9245283,0.8421515,0.1788003,0) : float4(0.8368256,0.677754,0.02687956,0);
				float _IsSubsurfaceShader548_g62695 = _IsSubsurfaceShader;
				float4 Output_Lighting525_g62695 = ( ( ( color544_g62695 * _IsSimpleShader359_g62695 ) + ( color521_g62695 * _IsStandardShader344_g62695 ) + ( color1121_g62695 * _IsSubsurfaceShader548_g62695 ) ) * Shading655_g62695 );
				float4 ifLocalVar40_g62978 = 0;
				if( Debug_Type367_g62695 == 2.0 )
				ifLocalVar40_g62978 = Output_Lighting525_g62695;
				float Debug_Index464_g62695 = TVE_DEBUG_Index;
				float2 uv_MainAlbedoTex = IN.ase_texcoord9.yz * _MainAlbedoTex_ST.xy + _MainAlbedoTex_ST.zw;
				float4 tex2DNode586_g62695 = tex2D( _MainAlbedoTex, uv_MainAlbedoTex );
				float3 appendResult637_g62695 = (float3(tex2DNode586_g62695.r , tex2DNode586_g62695.g , tex2DNode586_g62695.b));
				float3 ifLocalVar40_g63010 = 0;
				if( Debug_Index464_g62695 == 0.0 )
				ifLocalVar40_g63010 = appendResult637_g62695;
				float ifLocalVar40_g63032 = 0;
				if( Debug_Index464_g62695 == 1.0 )
				ifLocalVar40_g63032 = tex2D( _MainAlbedoTex, uv_MainAlbedoTex ).a;
				float2 uv_MainNormalTex = IN.ase_texcoord9.yz * _MainNormalTex_ST.xy + _MainNormalTex_ST.zw;
				float4 tex2DNode604_g62695 = tex2D( _MainNormalTex, uv_MainNormalTex );
				float3 appendResult876_g62695 = (float3(tex2DNode604_g62695.a , tex2DNode604_g62695.g , 1.0));
				float3 gammaToLinear878_g62695 = GammaToLinearSpace( appendResult876_g62695 );
				float3 ifLocalVar40_g62951 = 0;
				if( Debug_Index464_g62695 == 2.0 )
				ifLocalVar40_g62951 = gammaToLinear878_g62695;
				float2 uv_MainMaskTex = IN.ase_texcoord9.yz * _MainMaskTex_ST.xy + _MainMaskTex_ST.zw;
				float ifLocalVar40_g63024 = 0;
				if( Debug_Index464_g62695 == 3.0 )
				ifLocalVar40_g63024 = tex2D( _MainMaskTex, uv_MainMaskTex ).r;
				float ifLocalVar40_g62976 = 0;
				if( Debug_Index464_g62695 == 4.0 )
				ifLocalVar40_g62976 = tex2D( _MainMaskTex, uv_MainMaskTex ).g;
				float ifLocalVar40_g62997 = 0;
				if( Debug_Index464_g62695 == 5.0 )
				ifLocalVar40_g62997 = tex2D( _MainMaskTex, uv_MainMaskTex ).b;
				float ifLocalVar40_g63013 = 0;
				if( Debug_Index464_g62695 == 6.0 )
				ifLocalVar40_g63013 = tex2D( _MainMaskTex, uv_MainMaskTex ).a;
				float2 uv_SecondAlbedoTex = IN.ase_texcoord9.yz * _SecondAlbedoTex_ST.xy + _SecondAlbedoTex_ST.zw;
				float4 tex2DNode854_g62695 = tex2D( _SecondAlbedoTex, uv_SecondAlbedoTex );
				float3 appendResult839_g62695 = (float3(tex2DNode854_g62695.r , tex2DNode854_g62695.g , tex2DNode854_g62695.b));
				float3 ifLocalVar40_g63020 = 0;
				if( Debug_Index464_g62695 == 7.0 )
				ifLocalVar40_g63020 = appendResult839_g62695;
				float ifLocalVar40_g62934 = 0;
				if( Debug_Index464_g62695 == 8.0 )
				ifLocalVar40_g62934 = tex2D( _SecondAlbedoTex, uv_SecondAlbedoTex ).a;
				float2 uv_SecondNormalTex = IN.ase_texcoord9.yz * _SecondNormalTex_ST.xy + _SecondNormalTex_ST.zw;
				float4 tex2DNode841_g62695 = tex2D( _SecondNormalTex, uv_SecondNormalTex );
				float3 appendResult880_g62695 = (float3(tex2DNode841_g62695.a , tex2DNode841_g62695.g , 1.0));
				float3 gammaToLinear879_g62695 = GammaToLinearSpace( appendResult880_g62695 );
				float3 ifLocalVar40_g62985 = 0;
				if( Debug_Index464_g62695 == 8.0 )
				ifLocalVar40_g62985 = gammaToLinear879_g62695;
				float2 uv_SecondMaskTex = IN.ase_texcoord9.yz * _SecondMaskTex_ST.xy + _SecondMaskTex_ST.zw;
				float ifLocalVar40_g62950 = 0;
				if( Debug_Index464_g62695 == 10.0 )
				ifLocalVar40_g62950 = tex2D( _SecondMaskTex, uv_SecondMaskTex ).r;
				float ifLocalVar40_g62955 = 0;
				if( Debug_Index464_g62695 == 11.0 )
				ifLocalVar40_g62955 = tex2D( _SecondMaskTex, uv_SecondMaskTex ).g;
				float ifLocalVar40_g62983 = 0;
				if( Debug_Index464_g62695 == 12.0 )
				ifLocalVar40_g62983 = tex2D( _SecondMaskTex, uv_SecondMaskTex ).b;
				float ifLocalVar40_g62988 = 0;
				if( Debug_Index464_g62695 == 13.0 )
				ifLocalVar40_g62988 = tex2D( _SecondMaskTex, uv_SecondMaskTex ).a;
				float2 uv_EmissiveTex = IN.ase_texcoord9.yz * _EmissiveTex_ST.xy + _EmissiveTex_ST.zw;
				float4 tex2DNode858_g62695 = tex2D( _EmissiveTex, uv_EmissiveTex );
				float3 appendResult867_g62695 = (float3(tex2DNode858_g62695.r , tex2DNode858_g62695.g , tex2DNode858_g62695.b));
				float3 ifLocalVar40_g62956 = 0;
				if( Debug_Index464_g62695 == 14.0 )
				ifLocalVar40_g62956 = appendResult867_g62695;
				float Debug_Min721_g62695 = TVE_DEBUG_Min;
				float temp_output_7_0_g62962 = Debug_Min721_g62695;
				float4 temp_cast_3 = (temp_output_7_0_g62962).xxxx;
				float Debug_Max723_g62695 = TVE_DEBUG_Max;
				float4 Output_Maps561_g62695 = ( ( ( float4( ( ( ifLocalVar40_g63010 + ifLocalVar40_g63032 + ifLocalVar40_g62951 ) + ( ifLocalVar40_g63024 + ifLocalVar40_g62976 + ifLocalVar40_g62997 + ifLocalVar40_g63013 ) ) , 0.0 ) + float4( ( ( ( ifLocalVar40_g63020 + ifLocalVar40_g62934 + ifLocalVar40_g62985 ) + ( ifLocalVar40_g62950 + ifLocalVar40_g62955 + ifLocalVar40_g62983 + ifLocalVar40_g62988 ) ) * _DetailMode ) , 0.0 ) + ( ( float4( ifLocalVar40_g62956 , 0.0 ) * _EmissiveColor ) * _EmissiveCat ) ) - temp_cast_3 ) / ( Debug_Max723_g62695 - temp_output_7_0_g62962 ) );
				float4 ifLocalVar40_g62936 = 0;
				if( Debug_Type367_g62695 == 3.0 )
				ifLocalVar40_g62936 = Output_Maps561_g62695;
				float Resolution44_g62953 = max( _MainAlbedoTex_TexelSize.z , _MainAlbedoTex_TexelSize.w );
				float4 color62_g62953 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g62953 = 0;
				if( Resolution44_g62953 <= 256.0 )
				ifLocalVar61_g62953 = color62_g62953;
				float4 color55_g62953 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g62953 = 0;
				if( Resolution44_g62953 == 512.0 )
				ifLocalVar56_g62953 = color55_g62953;
				float4 color42_g62953 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g62953 = 0;
				if( Resolution44_g62953 == 1024.0 )
				ifLocalVar40_g62953 = color42_g62953;
				float4 color48_g62953 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g62953 = 0;
				if( Resolution44_g62953 == 2048.0 )
				ifLocalVar47_g62953 = color48_g62953;
				float4 color51_g62953 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g62953 = 0;
				if( Resolution44_g62953 >= 4096.0 )
				ifLocalVar52_g62953 = color51_g62953;
				float4 ifLocalVar40_g62987 = 0;
				if( Debug_Index464_g62695 == 0.0 )
				ifLocalVar40_g62987 = ( ifLocalVar61_g62953 + ifLocalVar56_g62953 + ifLocalVar40_g62953 + ifLocalVar47_g62953 + ifLocalVar52_g62953 );
				float Resolution44_g63000 = max( _MainNormalTex_TexelSize.z , _MainNormalTex_TexelSize.w );
				float4 color62_g63000 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g63000 = 0;
				if( Resolution44_g63000 <= 256.0 )
				ifLocalVar61_g63000 = color62_g63000;
				float4 color55_g63000 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g63000 = 0;
				if( Resolution44_g63000 == 512.0 )
				ifLocalVar56_g63000 = color55_g63000;
				float4 color42_g63000 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g63000 = 0;
				if( Resolution44_g63000 == 1024.0 )
				ifLocalVar40_g63000 = color42_g63000;
				float4 color48_g63000 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g63000 = 0;
				if( Resolution44_g63000 == 2048.0 )
				ifLocalVar47_g63000 = color48_g63000;
				float4 color51_g63000 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g63000 = 0;
				if( Resolution44_g63000 >= 4096.0 )
				ifLocalVar52_g63000 = color51_g63000;
				float4 ifLocalVar40_g63025 = 0;
				if( Debug_Index464_g62695 == 1.0 )
				ifLocalVar40_g63025 = ( ifLocalVar61_g63000 + ifLocalVar56_g63000 + ifLocalVar40_g63000 + ifLocalVar47_g63000 + ifLocalVar52_g63000 );
				float Resolution44_g62954 = max( _MainMaskTex_TexelSize.z , _MainMaskTex_TexelSize.w );
				float4 color62_g62954 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g62954 = 0;
				if( Resolution44_g62954 <= 256.0 )
				ifLocalVar61_g62954 = color62_g62954;
				float4 color55_g62954 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g62954 = 0;
				if( Resolution44_g62954 == 512.0 )
				ifLocalVar56_g62954 = color55_g62954;
				float4 color42_g62954 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g62954 = 0;
				if( Resolution44_g62954 == 1024.0 )
				ifLocalVar40_g62954 = color42_g62954;
				float4 color48_g62954 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g62954 = 0;
				if( Resolution44_g62954 == 2048.0 )
				ifLocalVar47_g62954 = color48_g62954;
				float4 color51_g62954 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g62954 = 0;
				if( Resolution44_g62954 >= 4096.0 )
				ifLocalVar52_g62954 = color51_g62954;
				float4 ifLocalVar40_g63001 = 0;
				if( Debug_Index464_g62695 == 2.0 )
				ifLocalVar40_g63001 = ( ifLocalVar61_g62954 + ifLocalVar56_g62954 + ifLocalVar40_g62954 + ifLocalVar47_g62954 + ifLocalVar52_g62954 );
				float Resolution44_g62963 = max( _SecondAlbedoTex_TexelSize.z , _SecondAlbedoTex_TexelSize.w );
				float4 color62_g62963 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g62963 = 0;
				if( Resolution44_g62963 <= 256.0 )
				ifLocalVar61_g62963 = color62_g62963;
				float4 color55_g62963 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g62963 = 0;
				if( Resolution44_g62963 == 512.0 )
				ifLocalVar56_g62963 = color55_g62963;
				float4 color42_g62963 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g62963 = 0;
				if( Resolution44_g62963 == 1024.0 )
				ifLocalVar40_g62963 = color42_g62963;
				float4 color48_g62963 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g62963 = 0;
				if( Resolution44_g62963 == 2048.0 )
				ifLocalVar47_g62963 = color48_g62963;
				float4 color51_g62963 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g62963 = 0;
				if( Resolution44_g62963 >= 4096.0 )
				ifLocalVar52_g62963 = color51_g62963;
				float4 ifLocalVar40_g62996 = 0;
				if( Debug_Index464_g62695 == 3.0 )
				ifLocalVar40_g62996 = ( ifLocalVar61_g62963 + ifLocalVar56_g62963 + ifLocalVar40_g62963 + ifLocalVar47_g62963 + ifLocalVar52_g62963 );
				float Resolution44_g63036 = max( _SecondMaskTex_TexelSize.z , _SecondMaskTex_TexelSize.w );
				float4 color62_g63036 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g63036 = 0;
				if( Resolution44_g63036 <= 256.0 )
				ifLocalVar61_g63036 = color62_g63036;
				float4 color55_g63036 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g63036 = 0;
				if( Resolution44_g63036 == 512.0 )
				ifLocalVar56_g63036 = color55_g63036;
				float4 color42_g63036 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g63036 = 0;
				if( Resolution44_g63036 == 1024.0 )
				ifLocalVar40_g63036 = color42_g63036;
				float4 color48_g63036 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g63036 = 0;
				if( Resolution44_g63036 == 2048.0 )
				ifLocalVar47_g63036 = color48_g63036;
				float4 color51_g63036 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g63036 = 0;
				if( Resolution44_g63036 >= 4096.0 )
				ifLocalVar52_g63036 = color51_g63036;
				float4 ifLocalVar40_g63009 = 0;
				if( Debug_Index464_g62695 == 4.0 )
				ifLocalVar40_g63009 = ( ifLocalVar61_g63036 + ifLocalVar56_g63036 + ifLocalVar40_g63036 + ifLocalVar47_g63036 + ifLocalVar52_g63036 );
				float Resolution44_g62964 = max( _SecondAlbedoTex_TexelSize.z , _SecondAlbedoTex_TexelSize.w );
				float4 color62_g62964 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g62964 = 0;
				if( Resolution44_g62964 <= 256.0 )
				ifLocalVar61_g62964 = color62_g62964;
				float4 color55_g62964 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g62964 = 0;
				if( Resolution44_g62964 == 512.0 )
				ifLocalVar56_g62964 = color55_g62964;
				float4 color42_g62964 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g62964 = 0;
				if( Resolution44_g62964 == 1024.0 )
				ifLocalVar40_g62964 = color42_g62964;
				float4 color48_g62964 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g62964 = 0;
				if( Resolution44_g62964 == 2048.0 )
				ifLocalVar47_g62964 = color48_g62964;
				float4 color51_g62964 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g62964 = 0;
				if( Resolution44_g62964 >= 4096.0 )
				ifLocalVar52_g62964 = color51_g62964;
				float4 ifLocalVar40_g63033 = 0;
				if( Debug_Index464_g62695 == 5.0 )
				ifLocalVar40_g63033 = ( ifLocalVar61_g62964 + ifLocalVar56_g62964 + ifLocalVar40_g62964 + ifLocalVar47_g62964 + ifLocalVar52_g62964 );
				float Resolution44_g63038 = max( _EmissiveTex_TexelSize.z , _EmissiveTex_TexelSize.w );
				float4 color62_g63038 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g63038 = 0;
				if( Resolution44_g63038 <= 256.0 )
				ifLocalVar61_g63038 = color62_g63038;
				float4 color55_g63038 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g63038 = 0;
				if( Resolution44_g63038 == 512.0 )
				ifLocalVar56_g63038 = color55_g63038;
				float4 color42_g63038 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g63038 = 0;
				if( Resolution44_g63038 == 1024.0 )
				ifLocalVar40_g63038 = color42_g63038;
				float4 color48_g63038 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g63038 = 0;
				if( Resolution44_g63038 == 2048.0 )
				ifLocalVar47_g63038 = color48_g63038;
				float4 color51_g63038 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g63038 = 0;
				if( Resolution44_g63038 >= 4096.0 )
				ifLocalVar52_g63038 = color51_g63038;
				float4 ifLocalVar40_g62961 = 0;
				if( Debug_Index464_g62695 == 6.0 )
				ifLocalVar40_g62961 = ( ifLocalVar61_g63038 + ifLocalVar56_g63038 + ifLocalVar40_g63038 + ifLocalVar47_g63038 + ifLocalVar52_g63038 );
				float4 Output_Resolution737_g62695 = ( ( ifLocalVar40_g62987 + ifLocalVar40_g63025 + ifLocalVar40_g63001 ) + ( ifLocalVar40_g62996 + ifLocalVar40_g63009 + ifLocalVar40_g63033 ) + ifLocalVar40_g62961 );
				float4 ifLocalVar40_g62941 = 0;
				if( Debug_Type367_g62695 == 4.0 )
				ifLocalVar40_g62941 = Output_Resolution737_g62695;
				float3 WorldPosition893_g62695 = worldPos;
				half3 Input_Position419_g62965 = -WorldPosition893_g62695;
				float4 temp_output_91_19_g62942 = TVE_MotionCoords;
				float3 Position83_g62942 = WorldPosition893_g62695;
				float temp_output_84_0_g62942 = _LayerMotionValue;
				float4 lerpResult87_g62942 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY( TVE_MotionTex, samplerTVE_MotionTex, float3(( (temp_output_91_19_g62942).zw + ( (temp_output_91_19_g62942).xy * (Position83_g62942).xz ) ),temp_output_84_0_g62942) ) , TVE_MotionUsage[(int)temp_output_84_0_g62942]);
				float4 break322_g62966 = lerpResult87_g62942;
				float3 appendResult397_g62966 = (float3(break322_g62966.x , 0.0 , break322_g62966.y));
				float3 temp_output_398_0_g62966 = (appendResult397_g62966*2.0 + -1.0);
				half2 Wind_DirectionWS1031_g62695 = (temp_output_398_0_g62966).xz;
				half2 Input_DirectionWS423_g62965 = Wind_DirectionWS1031_g62695;
				float Motion_Scale287_g62965 = ( _MotionScale_10 + 1.0 );
				half Global_Scale448_g62965 = TVE_NoiseParams.y;
				half Input_Speed62_g62965 = _MotionSpeed_10;
				half Global_Speed449_g62965 = TVE_NoiseParams.x;
				float mulTime426_g62965 = _Time.y * ( Input_Speed62_g62965 * Global_Speed449_g62965 );
				half Global_DistortionScale453_g62965 = TVE_NoiseParams.w;
				float3 break461_g62965 = ( Input_Position419_g62965 * Global_DistortionScale453_g62965 );
				half Global_Distortion452_g62965 = TVE_NoiseParams.z;
				float Noise_Distortion469_g62965 = ( sin( ( break461_g62965.x + break461_g62965.z ) ) * Global_Distortion452_g62965 );
				half Motion_Variation284_g62965 = 0.0;
				float2 temp_output_425_0_g62965 = ( ( (Input_Position419_g62965).xz * Input_DirectionWS423_g62965 * Motion_Scale287_g62965 * Global_Scale448_g62965 ) + ( mulTime426_g62965 + Noise_Distortion469_g62965 + Motion_Variation284_g62965 ) );
				float2 break500_g62965 = ( temp_output_425_0_g62965 * 0.1178 );
				float2 break494_g62965 = ( temp_output_425_0_g62965 * 0.1742 );
				half Wind_Power369_g62966 = break322_g62966.z;
				half Wind_Pow1128_g62695 = Wind_Power369_g62966;
				half Input_WindPower327_g62965 = Wind_Pow1128_g62695;
				float lerpResult430_g62965 = lerp( 1.0 , 0.4 , Input_WindPower327_g62965);
				half Motion_Noise915_g62695 = pow( (( sin( ( break500_g62965.x + break500_g62965.y ) ) * sin( ( break494_g62965.x + break494_g62965.y ) ) )*0.5 + 0.5) , lerpResult430_g62965 );
				float ifLocalVar40_g63023 = 0;
				if( Debug_Index464_g62695 == 0.0 )
				ifLocalVar40_g63023 = Motion_Noise915_g62695;
				float4 temp_output_91_19_g62930 = TVE_ColorsCoords;
				float3 Position58_g62930 = WorldPosition893_g62695;
				float Debug_Layer885_g62695 = TVE_DEBUG_Layer;
				float temp_output_82_0_g62930 = Debug_Layer885_g62695;
				float4 lerpResult88_g62930 = lerp( TVE_ColorsParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ColorsTex, samplerTVE_ColorsTex, float3(( (temp_output_91_19_g62930).zw + ( (temp_output_91_19_g62930).xy * (Position58_g62930).xz ) ),temp_output_82_0_g62930) ) , TVE_ColorsUsage[(int)temp_output_82_0_g62930]);
				float3 ifLocalVar40_g63040 = 0;
				if( Debug_Index464_g62695 == 1.0 )
				ifLocalVar40_g63040 = (lerpResult88_g62930).rgb;
				float4 temp_output_91_19_g62926 = TVE_ColorsCoords;
				float3 Position58_g62926 = WorldPosition893_g62695;
				float temp_output_82_0_g62926 = Debug_Layer885_g62695;
				float4 lerpResult88_g62926 = lerp( TVE_ColorsParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ColorsTex, samplerTVE_ColorsTex, float3(( (temp_output_91_19_g62926).zw + ( (temp_output_91_19_g62926).xy * (Position58_g62926).xz ) ),temp_output_82_0_g62926) ) , TVE_ColorsUsage[(int)temp_output_82_0_g62926]);
				float ifLocalVar40_g63031 = 0;
				if( Debug_Index464_g62695 == 2.0 )
				ifLocalVar40_g63031 = (lerpResult88_g62926).a;
				float4 temp_output_93_19_g62922 = TVE_ExtrasCoords;
				float3 Position82_g62922 = WorldPosition893_g62695;
				float temp_output_84_0_g62922 = Debug_Layer885_g62695;
				float4 lerpResult88_g62922 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, float3(( (temp_output_93_19_g62922).zw + ( (temp_output_93_19_g62922).xy * (Position82_g62922).xz ) ),temp_output_84_0_g62922) ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62922]);
				float4 break89_g62922 = lerpResult88_g62922;
				float ifLocalVar40_g63021 = 0;
				if( Debug_Index464_g62695 == 3.0 )
				ifLocalVar40_g63021 = break89_g62922.r;
				float4 temp_output_93_19_g63016 = TVE_ExtrasCoords;
				float3 Position82_g63016 = WorldPosition893_g62695;
				float temp_output_84_0_g63016 = Debug_Layer885_g62695;
				float4 lerpResult88_g63016 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, float3(( (temp_output_93_19_g63016).zw + ( (temp_output_93_19_g63016).xy * (Position82_g63016).xz ) ),temp_output_84_0_g63016) ) , TVE_ExtrasUsage[(int)temp_output_84_0_g63016]);
				float4 break89_g63016 = lerpResult88_g63016;
				float ifLocalVar40_g63022 = 0;
				if( Debug_Index464_g62695 == 4.0 )
				ifLocalVar40_g63022 = break89_g63016.g;
				float4 temp_output_93_19_g62946 = TVE_ExtrasCoords;
				float3 Position82_g62946 = WorldPosition893_g62695;
				float temp_output_84_0_g62946 = Debug_Layer885_g62695;
				float4 lerpResult88_g62946 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, float3(( (temp_output_93_19_g62946).zw + ( (temp_output_93_19_g62946).xy * (Position82_g62946).xz ) ),temp_output_84_0_g62946) ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62946]);
				float4 break89_g62946 = lerpResult88_g62946;
				float ifLocalVar40_g63015 = 0;
				if( Debug_Index464_g62695 == 5.0 )
				ifLocalVar40_g63015 = break89_g62946.b;
				float4 temp_output_93_19_g62957 = TVE_ExtrasCoords;
				float3 Position82_g62957 = WorldPosition893_g62695;
				float temp_output_84_0_g62957 = Debug_Layer885_g62695;
				float4 lerpResult88_g62957 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, float3(( (temp_output_93_19_g62957).zw + ( (temp_output_93_19_g62957).xy * (Position82_g62957).xz ) ),temp_output_84_0_g62957) ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62957]);
				float4 break89_g62957 = lerpResult88_g62957;
				float ifLocalVar40_g63002 = 0;
				if( Debug_Index464_g62695 == 6.0 )
				ifLocalVar40_g63002 = break89_g62957.a;
				float4 temp_output_91_19_g62937 = TVE_MotionCoords;
				float3 Position83_g62937 = WorldPosition893_g62695;
				float temp_output_84_0_g62937 = Debug_Layer885_g62695;
				float4 lerpResult87_g62937 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY( TVE_MotionTex, samplerTVE_MotionTex, float3(( (temp_output_91_19_g62937).zw + ( (temp_output_91_19_g62937).xy * (Position83_g62937).xz ) ),temp_output_84_0_g62937) ) , TVE_MotionUsage[(int)temp_output_84_0_g62937]);
				float3 appendResult1012_g62695 = (float3((lerpResult87_g62937).rg , 0.0));
				float3 ifLocalVar40_g63005 = 0;
				if( Debug_Index464_g62695 == 7.0 )
				ifLocalVar40_g63005 = appendResult1012_g62695;
				float4 temp_output_91_19_g62979 = TVE_MotionCoords;
				float3 Position83_g62979 = WorldPosition893_g62695;
				float temp_output_84_0_g62979 = Debug_Layer885_g62695;
				float4 lerpResult87_g62979 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY( TVE_MotionTex, samplerTVE_MotionTex, float3(( (temp_output_91_19_g62979).zw + ( (temp_output_91_19_g62979).xy * (Position83_g62979).xz ) ),temp_output_84_0_g62979) ) , TVE_MotionUsage[(int)temp_output_84_0_g62979]);
				float ifLocalVar40_g63006 = 0;
				if( Debug_Index464_g62695 == 8.0 )
				ifLocalVar40_g63006 = (lerpResult87_g62979).b;
				float4 temp_output_91_19_g62990 = TVE_MotionCoords;
				float3 Position83_g62990 = WorldPosition893_g62695;
				float temp_output_84_0_g62990 = Debug_Layer885_g62695;
				float4 lerpResult87_g62990 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY( TVE_MotionTex, samplerTVE_MotionTex, float3(( (temp_output_91_19_g62990).zw + ( (temp_output_91_19_g62990).xy * (Position83_g62990).xz ) ),temp_output_84_0_g62990) ) , TVE_MotionUsage[(int)temp_output_84_0_g62990]);
				float ifLocalVar40_g62977 = 0;
				if( Debug_Index464_g62695 == 9.0 )
				ifLocalVar40_g62977 = (lerpResult87_g62990).a;
				float4 temp_output_94_19_g62967 = TVE_VertexCoords;
				float3 Position83_g62967 = WorldPosition893_g62695;
				float temp_output_84_0_g62967 = Debug_Layer885_g62695;
				float4 lerpResult87_g62967 = lerp( TVE_VertexParams , SAMPLE_TEXTURE2D_ARRAY( TVE_VertexTex, samplerTVE_VertexTex, float3(( (temp_output_94_19_g62967).zw + ( (temp_output_94_19_g62967).xy * (Position83_g62967).xz ) ),temp_output_84_0_g62967) ) , TVE_VertexUsage[(int)temp_output_84_0_g62967]);
				float ifLocalVar40_g62952 = 0;
				if( Debug_Index464_g62695 == 10.0 )
				ifLocalVar40_g62952 = (lerpResult87_g62967).a;
				float3 Output_Globals888_g62695 = ( ifLocalVar40_g63023 + ( ifLocalVar40_g63040 + ifLocalVar40_g63031 ) + ( ifLocalVar40_g63021 + ifLocalVar40_g63022 + ifLocalVar40_g63015 + ifLocalVar40_g63002 ) + ( ifLocalVar40_g63005 + ifLocalVar40_g63006 + ifLocalVar40_g62977 ) + ( ifLocalVar40_g62952 + 0.0 ) );
				float3 ifLocalVar40_g63034 = 0;
				if( Debug_Type367_g62695 == 8.0 )
				ifLocalVar40_g63034 = Output_Globals888_g62695;
				float3 vertexToFrag328_g62695 = IN.ase_texcoord10.xyz;
				float4 color1016_g62695 = IsGammaSpace() ? float4(0.5831653,0.6037736,0.2135992,0) : float4(0.2992498,0.3229691,0.03750122,0);
				float4 color1017_g62695 = IsGammaSpace() ? float4(0.8117647,0.3488252,0.2627451,0) : float4(0.6239604,0.0997834,0.05612849,0);
				float4 switchResult1015_g62695 = (((ase_vface>0)?(color1016_g62695):(color1017_g62695)));
				float3 ifLocalVar40_g63012 = 0;
				if( Debug_Index464_g62695 == 4.0 )
				ifLocalVar40_g63012 = (switchResult1015_g62695).rgb;
				float temp_output_7_0_g63011 = Debug_Min721_g62695;
				float3 temp_cast_28 = (temp_output_7_0_g63011).xxx;
				float3 Output_Mesh316_g62695 = saturate( ( ( ( vertexToFrag328_g62695 + ifLocalVar40_g63012 ) - temp_cast_28 ) / ( Debug_Max723_g62695 - temp_output_7_0_g63011 ) ) );
				float3 ifLocalVar40_g63039 = 0;
				if( Debug_Type367_g62695 == 9.0 )
				ifLocalVar40_g63039 = Output_Mesh316_g62695;
				float4 color1086_g62695 = IsGammaSpace() ? float4(0.1226415,0.1226415,0.1226415,0) : float4(0.01390275,0.01390275,0.01390275,0);
				float4 vertexToFrag11_g63037 = IN.ase_texcoord11;
				float _IsVegetationShader1101_g62695 = _IsVegetationShader;
				float4 lerpResult1089_g62695 = lerp( color1086_g62695 , vertexToFrag11_g63037 , ( _IsPolygonalShader1112_g62695 * _IsVegetationShader1101_g62695 ));
				float3 Output_Misc1080_g62695 = (lerpResult1089_g62695).rgb;
				float3 ifLocalVar40_g62935 = 0;
				if( Debug_Type367_g62695 == 10.0 )
				ifLocalVar40_g62935 = Output_Misc1080_g62695;
				float4 temp_output_459_0_g62695 = ( ifLocalVar40_g63004 + ifLocalVar40_g62984 + ifLocalVar40_g62978 + ifLocalVar40_g62936 + ifLocalVar40_g62941 + float4( ifLocalVar40_g63034 , 0.0 ) + float4( ifLocalVar40_g63039 , 0.0 ) + float4( ifLocalVar40_g62935 , 0.0 ) );
				float4 color690_g62695 = IsGammaSpace() ? float4(0.1226415,0.1226415,0.1226415,0) : float4(0.01390275,0.01390275,0.01390275,0);
				float _IsTVEShader647_g62695 = _IsTVEShader;
				float4 lerpResult689_g62695 = lerp( color690_g62695 , temp_output_459_0_g62695 , _IsTVEShader647_g62695);
				float Debug_Filter322_g62695 = TVE_DEBUG_Filter;
				float4 lerpResult326_g62695 = lerp( temp_output_459_0_g62695 , lerpResult689_g62695 , Debug_Filter322_g62695);
				float Debug_Clip623_g62695 = TVE_DEBUG_Clip;
				float lerpResult622_g62695 = lerp( 1.0 , tex2D( _MainAlbedoTex, uv_MainAlbedoTex ).a , ( Debug_Clip623_g62695 * _RenderClip ));
				clip( lerpResult622_g62695 - _Cutoff);
				clip( ( 1.0 - saturate( ( _IsElementShader + _IsHelperShader ) ) ) - 1.0);
				
				o.Albedo = fixed3( 0.5, 0.5, 0.5 );
				o.Normal = fixed3( 0, 0, 1 );
				o.Emission = lerpResult326_g62695.rgb;
				#if defined(_SPECULAR_SETUP)
					o.Specular = fixed3( 0, 0, 0 );
				#else
					o.Metallic = 0;
				#endif
				o.Smoothness = 0;
				o.Occlusion = 1;
				o.Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;				

				#ifdef _ALPHATEST_ON
					clip( o.Alpha - AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
					outputDepth = IN.pos.z;
				#endif

				#ifndef USING_DIRECTIONAL_LIGHT
					fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				#else
					fixed3 lightDir = _WorldSpaceLightPos0.xyz;
				#endif

				fixed4 c = 0;
				float3 worldN;
				worldN.x = dot(IN.tSpace0.xyz, o.Normal);
				worldN.y = dot(IN.tSpace1.xyz, o.Normal);
				worldN.z = dot(IN.tSpace2.xyz, o.Normal);
				worldN = normalize(worldN);
				o.Normal = worldN;

				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
				gi.indirect.diffuse = 0;
				gi.indirect.specular = 0;
				gi.light.color = _LightColor0.rgb;
				gi.light.dir = lightDir;

				UnityGIInput giInput;
				UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
				giInput.light = gi.light;
				giInput.worldPos = worldPos;
				giInput.worldViewDir = worldViewDir;
				giInput.atten = atten;
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
					giInput.lightmapUV = IN.lmap;
				#else
					giInput.lightmapUV = 0.0;
				#endif
				#if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
					giInput.ambient = IN.sh;
				#else
					giInput.ambient.rgb = 0.0;
				#endif
				giInput.probeHDR[0] = unity_SpecCube0_HDR;
				giInput.probeHDR[1] = unity_SpecCube1_HDR;
				#if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
					giInput.boxMin[0] = unity_SpecCube0_BoxMin;
				#endif
				#ifdef UNITY_SPECCUBE_BOX_PROJECTION
					giInput.boxMax[0] = unity_SpecCube0_BoxMax;
					giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
					giInput.boxMax[1] = unity_SpecCube1_BoxMax;
					giInput.boxMin[1] = unity_SpecCube1_BoxMin;
					giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
				#endif
				
				#if defined(_SPECULAR_SETUP)
					LightingStandardSpecular_GI(o, giInput, gi);
				#else
					LightingStandard_GI( o, giInput, gi );
				#endif

				#ifdef ASE_BAKEDGI
					gi.indirect.diffuse = BakedGI;
				#endif

				#if UNITY_SHOULD_SAMPLE_SH && !defined(LIGHTMAP_ON) && defined(ASE_NO_AMBIENT)
					gi.indirect.diffuse = 0;
				#endif

				#if defined(_SPECULAR_SETUP)
					c += LightingStandardSpecular (o, worldViewDir, gi);
				#else
					c += LightingStandard( o, worldViewDir, gi );
				#endif
				
				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;
					#ifdef DIRECTIONAL
						float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, shadow );
					#else
						float3 lightAtten = gi.light.color;
					#endif
					half3 transmission = max(0 , -dot(o.Normal, gi.light.dir)) * lightAtten * Transmission;
					c.rgb += o.Albedo * transmission;
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					#ifdef DIRECTIONAL
						float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, shadow );
					#else
						float3 lightAtten = gi.light.color;
					#endif
					half3 lightDir = gi.light.dir + o.Normal * normal;
					half transVdotL = pow( saturate( dot( worldViewDir, -lightDir ) ), scattering );
					half3 translucency = lightAtten * (transVdotL * direct + gi.indirect.diffuse * ambient) * Translucency;
					c.rgb += o.Albedo * translucency * strength;
				}
				#endif

				//#ifdef _REFRACTION_ASE
				//	float4 projScreenPos = ScreenPos / ScreenPos.w;
				//	float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, WorldNormal ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
				//	projScreenPos.xy += refractionOffset.xy;
				//	float3 refraction = UNITY_SAMPLE_SCREENSPACE_TEXTURE( _GrabTexture, projScreenPos ) * RefractionColor;
				//	color.rgb = lerp( refraction, color.rgb, color.a );
				//	color.a = 1;
				//#endif

				c.rgb += o.Emission;

				#ifdef ASE_FOG
					UNITY_APPLY_FOG(IN.fogCoord, c);
				#endif
				return c;
			}
			ENDCG
		}

		
		Pass
		{
			
			Name "Deferred"
			Tags { "LightMode"="Deferred" }

			AlphaToMask Off

			CGPROGRAM
			#define ASE_NO_AMBIENT 1
			#if defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES) || defined(SHADER_API_GLES3) || defined(SHADER_API_D3D9)
			#define FRONT_FACE_SEMANTIC VFACE
			#define FRONT_FACE_TYPE float
			#else
			#define FRONT_FACE_SEMANTIC SV_IsFrontFace
			#define FRONT_FACE_TYPE bool
			#endif

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma exclude_renderers nomrt 
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#pragma multi_compile_prepassfinal
			#ifndef UNITY_PASS_DEFERRED
				#define UNITY_PASS_DEFERRED
			#endif
			#include "HLSLSupport.cginc"
			#if !defined( UNITY_INSTANCED_LOD_FADE )
				#define UNITY_INSTANCED_LOD_FADE
			#endif
			#if !defined( UNITY_INSTANCED_SH )
				#define UNITY_INSTANCED_SH
			#endif
			#if !defined( UNITY_INSTANCED_LIGHTMAPSTS )
				#define UNITY_INSTANCED_LIGHTMAPSTS
			#endif
			#include "UnityShaderVariables.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
			#define SAMPLE_TEXTURE2D_ARRAY(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
			#else//ASE Sampling Macros
			#define SAMPLE_TEXTURE2D_ARRAY(tex,samplertex,coord) tex2DArray(tex,coord)
			#endif//ASE Sampling Macros
			

			struct appdata {
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				#if UNITY_VERSION >= 201810
					UNITY_POSITION(pos);
				#else
					float4 pos : SV_POSITION;
				#endif
				float4 lmap : TEXCOORD2;
				#ifndef LIGHTMAP_ON
					#if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
						half3 sh : TEXCOORD3;
					#endif
				#else
					#ifdef DIRLIGHTMAP_OFF
						float4 lmapFadePos : TEXCOORD4;
					#endif
				#endif
				float4 tSpace0 : TEXCOORD5;
				float4 tSpace1 : TEXCOORD6;
				float4 tSpace2 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				float4 ase_texcoord10 : TEXCOORD10;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#ifdef LIGHTMAP_ON
			float4 unity_LightmapFade;
			#endif
			fixed4 unity_Ambient;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			uniform half _Message;
			uniform half _Banner;
			uniform float _IsSimpleShader;
			uniform half _IsTVEShader;
			uniform half TVE_DEBUG_Type;
			uniform float _IsBarkShader;
			uniform float _IsCrossShader;
			uniform float _IsGrassShader;
			uniform float _IsLeafShader;
			uniform float _IsPropShader;
			uniform float _IsImpostorShader;
			uniform float _IsPolygonalShader;
			uniform float _IsStandardShader;
			uniform float _IsSubsurfaceShader;
			uniform half TVE_DEBUG_Index;
			uniform sampler2D _MainAlbedoTex;
			uniform float4 _MainAlbedoTex_ST;
			uniform sampler2D _MainNormalTex;
			uniform float4 _MainNormalTex_ST;
			uniform sampler2D _MainMaskTex;
			uniform float4 _MainMaskTex_ST;
			uniform sampler2D _SecondAlbedoTex;
			uniform float4 _SecondAlbedoTex_ST;
			uniform sampler2D _SecondNormalTex;
			uniform float4 _SecondNormalTex_ST;
			uniform sampler2D _SecondMaskTex;
			uniform float4 _SecondMaskTex_ST;
			uniform float _DetailMode;
			uniform sampler2D _EmissiveTex;
			uniform float4 _EmissiveTex_ST;
			uniform float4 _EmissiveColor;
			uniform float _EmissiveCat;
			uniform half TVE_DEBUG_Min;
			uniform half TVE_DEBUG_Max;
			float4 _MainAlbedoTex_TexelSize;
			float4 _MainNormalTex_TexelSize;
			float4 _MainMaskTex_TexelSize;
			float4 _SecondAlbedoTex_TexelSize;
			float4 _SecondMaskTex_TexelSize;
			float4 _EmissiveTex_TexelSize;
			uniform half4 TVE_MotionParams;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(TVE_MotionTex);
			uniform half4 TVE_MotionCoords;
			uniform half _LayerMotionValue;
			SamplerState samplerTVE_MotionTex;
			uniform float TVE_MotionUsage[10];
			uniform float _MotionScale_10;
			uniform half4 TVE_NoiseParams;
			uniform float _MotionSpeed_10;
			uniform half4 TVE_ColorsParams;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(TVE_ColorsTex);
			uniform half4 TVE_ColorsCoords;
			uniform half TVE_DEBUG_Layer;
			SamplerState samplerTVE_ColorsTex;
			uniform float TVE_ColorsUsage[10];
			uniform half4 TVE_ExtrasParams;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(TVE_ExtrasTex);
			uniform half4 TVE_ExtrasCoords;
			SamplerState samplerTVE_ExtrasTex;
			uniform float TVE_ExtrasUsage[10];
			uniform half4 TVE_VertexParams;
			UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(TVE_VertexTex);
			uniform half4 TVE_VertexCoords;
			SamplerState samplerTVE_VertexTex;
			uniform float TVE_VertexUsage[10];
			uniform float _IsVegetationShader;
			uniform half4 _LeavesFilterColor;
			uniform half4 _MainColor;
			uniform half _VertexColorMode;
			uniform float _LeavesFilterRange;
			uniform half _LeavesFilterMode;
			uniform half TVE_DEBUG_Filter;
			uniform half TVE_DEBUG_Clip;
			uniform float _RenderClip;
			uniform float _Cutoff;
			uniform float _IsElementShader;
			uniform float _IsHelperShader;

	
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			

			v2f VertexFunction (appdata v  ) {
				UNITY_SETUP_INSTANCE_ID(v);
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
				UNITY_TRANSFER_INSTANCE_ID(v,o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 customSurfaceDepth676_g62695 = v.vertex.xyz;
				float customEye676_g62695 = -UnityObjectToViewPos( customSurfaceDepth676_g62695 ).z;
				o.ase_texcoord8.x = customEye676_g62695;
				float Debug_Index464_g62695 = TVE_DEBUG_Index;
				float3 ifLocalVar40_g63014 = 0;
				if( Debug_Index464_g62695 == 0.0 )
				ifLocalVar40_g63014 = saturate( v.vertex.xyz );
				float3 ifLocalVar40_g62989 = 0;
				if( Debug_Index464_g62695 == 1.0 )
				ifLocalVar40_g62989 = v.normal;
				float3 ifLocalVar40_g62995 = 0;
				if( Debug_Index464_g62695 == 2.0 )
				ifLocalVar40_g62995 = v.tangent.xyz;
				float ifLocalVar40_g63008 = 0;
				if( Debug_Index464_g62695 == 3.0 )
				ifLocalVar40_g63008 = saturate( v.tangent.w );
				float3 temp_cast_0 = (v.ase_color.r).xxx;
				float3 hsvTorgb260_g62695 = HSVToRGB( float3(v.ase_color.r,1.0,1.0) );
				float3 gammaToLinear266_g62695 = GammaToLinearSpace( hsvTorgb260_g62695 );
				float _IsBarkShader347_g62695 = _IsBarkShader;
				float _IsLeafShader360_g62695 = _IsLeafShader;
				float _IsCrossShader342_g62695 = _IsCrossShader;
				float _IsGrassShader341_g62695 = _IsGrassShader;
				float _IsVegetationShader1101_g62695 = _IsVegetationShader;
				float _IsAnyVegetationShader362_g62695 = saturate( ( _IsBarkShader347_g62695 + _IsLeafShader360_g62695 + _IsCrossShader342_g62695 + _IsGrassShader341_g62695 + _IsVegetationShader1101_g62695 ) );
				float3 lerpResult290_g62695 = lerp( temp_cast_0 , gammaToLinear266_g62695 , _IsAnyVegetationShader362_g62695);
				float3 ifLocalVar40_g62998 = 0;
				if( Debug_Index464_g62695 == 5.0 )
				ifLocalVar40_g62998 = lerpResult290_g62695;
				float ifLocalVar40_g62994 = 0;
				if( Debug_Index464_g62695 == 6.0 )
				ifLocalVar40_g62994 = v.ase_color.g;
				float ifLocalVar40_g62999 = 0;
				if( Debug_Index464_g62695 == 7.0 )
				ifLocalVar40_g62999 = v.ase_color.b;
				float ifLocalVar40_g63003 = 0;
				if( Debug_Index464_g62695 == 8.0 )
				ifLocalVar40_g63003 = v.ase_color.a;
				float ifLocalVar40_g63007 = 0;
				if( Debug_Index464_g62695 == 9.0 )
				ifLocalVar40_g63007 = v.ase_texcoord3.x;
				float ifLocalVar40_g63035 = 0;
				if( Debug_Index464_g62695 == 10.0 )
				ifLocalVar40_g63035 = v.ase_texcoord3.y;
				float ifLocalVar40_g62986 = 0;
				if( Debug_Index464_g62695 == 11.0 )
				ifLocalVar40_g62986 = v.ase_texcoord3.z;
				float3 vertexToFrag328_g62695 = ( ( ifLocalVar40_g63014 + ifLocalVar40_g62989 + ifLocalVar40_g62995 + ifLocalVar40_g63008 ) + ( ifLocalVar40_g62998 + ifLocalVar40_g62994 + ifLocalVar40_g62999 + ifLocalVar40_g63003 ) + ( ifLocalVar40_g63007 + ifLocalVar40_g63035 + ifLocalVar40_g62986 ) );
				o.ase_texcoord9.xyz = vertexToFrag328_g62695;
				float4 color1097_g62695 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 color1096_g62695 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float2 uv_MainMaskTex1077_g62695 = v.ase_texcoord.xy;
				float3 linearToGamma1066_g62695 = LinearToGammaSpace( _LeavesFilterColor.rgb );
				#ifdef UNITY_COLORSPACE_GAMMA
				float3 staticSwitch1060_g62695 = (_LeavesFilterColor).rgb;
				#else
				float3 staticSwitch1060_g62695 = linearToGamma1066_g62695;
				#endif
				float2 uv_MainAlbedoTex1045_g62695 = v.ase_texcoord.xy;
				float4 tex2DNode1045_g62695 = tex2Dlod( _MainAlbedoTex, float4( uv_MainAlbedoTex1045_g62695, 0, 0.0) );
				float3 lerpResult1043_g62695 = lerp( (tex2DNode1045_g62695).rgb , (v.ase_color).rgb , _VertexColorMode);
				half3 Main_Albedo1078_g62695 = ( (_MainColor).rgb * lerpResult1043_g62695 );
				float3 linearToGamma1058_g62695 = LinearToGammaSpace( Main_Albedo1078_g62695 );
				#ifdef UNITY_COLORSPACE_GAMMA
				float3 staticSwitch1068_g62695 = (Main_Albedo1078_g62695).xyz;
				#else
				float3 staticSwitch1068_g62695 = linearToGamma1058_g62695;
				#endif
				float lerpResult1071_g62695 = lerp( 1.0 , saturate( ( 1.0 - ceil( ( distance( staticSwitch1060_g62695 , staticSwitch1068_g62695 ) - _LeavesFilterRange ) ) ) ) , _LeavesFilterMode);
				half Main_ColorFilter1061_g62695 = lerpResult1071_g62695;
				float4 lerpResult1095_g62695 = lerp( color1097_g62695 , color1096_g62695 , ( tex2Dlod( _MainMaskTex, float4( uv_MainMaskTex1077_g62695, 0, 0.0) ).b * Main_ColorFilter1061_g62695 ));
				float4 vertexToFrag11_g63037 = lerpResult1095_g62695;
				o.ase_texcoord10 = vertexToFrag11_g63037;
				
				o.ase_texcoord8.yz = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.w = 0;
				o.ase_texcoord9.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.vertex.w = 1;
				v.normal = v.normal;
				v.tangent = v.tangent;

				o.pos = UnityObjectToClipPos(v.vertex);
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

				#ifdef DYNAMICLIGHTMAP_ON
					o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#else
					o.lmap.zw = 0;
				#endif
				#ifdef LIGHTMAP_ON
					o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					#ifdef DIRLIGHTMAP_OFF
						o.lmapFadePos.xyz = (mul(unity_ObjectToWorld, v.vertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w;
						o.lmapFadePos.w = (-UnityObjectToViewPos(v.vertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w);
					#endif
				#else
					o.lmap.xy = 0;
					#if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
						o.sh = 0;
						o.sh = ShadeSHPerVertex (worldNormal, o.sh);
					#endif
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( appdata v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.tangent = v.tangent;
				o.normal = v.normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				o.ase_texcoord3 = v.ase_texcoord3;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, UNITY_MATRIX_M, _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, UNITY_MATRIX_M, _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, UNITY_MATRIX_M, _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			v2f DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				appdata o = (appdata) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.tangent = patch[0].tangent * bary.x + patch[1].tangent * bary.y + patch[2].tangent * bary.z;
				o.normal = patch[0].normal * bary.x + patch[1].normal * bary.y + patch[2].normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].normal * (dot(o.vertex.xyz, patch[i].normal) - dot(patch[i].vertex.xyz, patch[i].normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			v2f vert ( appdata v )
			{
				return VertexFunction( v );
			}
			#endif

			void frag (v2f IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC
				, out half4 outGBuffer0 : SV_Target0
				, out half4 outGBuffer1 : SV_Target1
				, out half4 outGBuffer2 : SV_Target2
				, out half4 outEmission : SV_Target3
				#if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
				, out half4 outShadowMask : SV_Target4
				#endif
				#ifdef _DEPTHOFFSET_ON
				, out float outputDepth : SV_Depth
				#endif
			) 
			{
				UNITY_SETUP_INSTANCE_ID(IN);

				#ifdef LOD_FADE_CROSSFADE
					UNITY_APPLY_DITHER_CROSSFADE(IN.pos.xy);
				#endif

				#if defined(_SPECULAR_SETUP)
					SurfaceOutputStandardSpecular o = (SurfaceOutputStandardSpecular)0;
				#else
					SurfaceOutputStandard o = (SurfaceOutputStandard)0;
				#endif
				float3 WorldTangent = float3(IN.tSpace0.x,IN.tSpace1.x,IN.tSpace2.x);
				float3 WorldBiTangent = float3(IN.tSpace0.y,IN.tSpace1.y,IN.tSpace2.y);
				float3 WorldNormal = float3(IN.tSpace0.z,IN.tSpace1.z,IN.tSpace2.z);
				float3 worldPos = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				half atten = 1;

				float Debug_Type367_g62695 = TVE_DEBUG_Type;
				float4 color646_g62695 = IsGammaSpace() ? float4(0.9245283,0.7969696,0.4142933,1) : float4(0.8368256,0.5987038,0.1431069,1);
				float customEye676_g62695 = IN.ase_texcoord8.x;
				float saferPower688_g62695 = abs( (0.0 + (customEye676_g62695 - 300.0) * (1.0 - 0.0) / (0.0 - 300.0)) );
				float clampResult702_g62695 = clamp( pow( saferPower688_g62695 , 1.25 ) , 0.75 , 1.0 );
				float Shading655_g62695 = clampResult702_g62695;
				float4 Output_Converted717_g62695 = ( color646_g62695 * Shading655_g62695 );
				float4 ifLocalVar40_g63004 = 0;
				if( Debug_Type367_g62695 == 0.0 )
				ifLocalVar40_g63004 = Output_Converted717_g62695;
				float4 color466_g62695 = IsGammaSpace() ? float4(0.8113208,0.4952317,0.264062,0) : float4(0.6231937,0.2096542,0.05668841,0);
				float _IsBarkShader347_g62695 = _IsBarkShader;
				float4 color469_g62695 = IsGammaSpace() ? float4(0.6566009,0.3404236,0.8490566,0) : float4(0.3886527,0.09487338,0.6903409,0);
				float _IsCrossShader342_g62695 = _IsCrossShader;
				float4 color472_g62695 = IsGammaSpace() ? float4(0.7100264,0.8018868,0.2231666,0) : float4(0.4623997,0.6070304,0.0407874,0);
				float _IsGrassShader341_g62695 = _IsGrassShader;
				float4 color475_g62695 = IsGammaSpace() ? float4(0.3267961,0.7264151,0.3118103,0) : float4(0.08721471,0.4865309,0.07922345,0);
				float _IsLeafShader360_g62695 = _IsLeafShader;
				float4 color478_g62695 = IsGammaSpace() ? float4(0.3252937,0.6122813,0.8113208,0) : float4(0.08639329,0.3330702,0.6231937,0);
				float _IsPropShader346_g62695 = _IsPropShader;
				float4 color1114_g62695 = IsGammaSpace() ? float4(0.9716981,0.3162602,0.4816265,0) : float4(0.9368213,0.08154967,0.1974273,0);
				float _IsImpostorShader1110_g62695 = _IsImpostorShader;
				float4 color1117_g62695 = IsGammaSpace() ? float4(0.257921,0.8679245,0.8361252,0) : float4(0.05410501,0.7254258,0.6668791,0);
				float _IsPolygonalShader1112_g62695 = _IsPolygonalShader;
				float4 Output_Shader445_g62695 = ( ( ( color466_g62695 * _IsBarkShader347_g62695 ) + ( color469_g62695 * _IsCrossShader342_g62695 ) + ( color472_g62695 * _IsGrassShader341_g62695 ) + ( color475_g62695 * _IsLeafShader360_g62695 ) + ( color478_g62695 * _IsPropShader346_g62695 ) + ( color1114_g62695 * _IsImpostorShader1110_g62695 ) + ( color1117_g62695 * _IsPolygonalShader1112_g62695 ) ) * Shading655_g62695 );
				float4 ifLocalVar40_g62984 = 0;
				if( Debug_Type367_g62695 == 1.0 )
				ifLocalVar40_g62984 = Output_Shader445_g62695;
				float4 color544_g62695 = IsGammaSpace() ? float4(0.3252937,0.6122813,0.8113208,0) : float4(0.08639329,0.3330702,0.6231937,0);
				float _IsSimpleShader359_g62695 = _IsSimpleShader;
				float4 color521_g62695 = IsGammaSpace() ? float4(0.6566009,0.3404236,0.8490566,0) : float4(0.3886527,0.09487338,0.6903409,0);
				float _IsStandardShader344_g62695 = _IsStandardShader;
				float4 color1121_g62695 = IsGammaSpace() ? float4(0.9245283,0.8421515,0.1788003,0) : float4(0.8368256,0.677754,0.02687956,0);
				float _IsSubsurfaceShader548_g62695 = _IsSubsurfaceShader;
				float4 Output_Lighting525_g62695 = ( ( ( color544_g62695 * _IsSimpleShader359_g62695 ) + ( color521_g62695 * _IsStandardShader344_g62695 ) + ( color1121_g62695 * _IsSubsurfaceShader548_g62695 ) ) * Shading655_g62695 );
				float4 ifLocalVar40_g62978 = 0;
				if( Debug_Type367_g62695 == 2.0 )
				ifLocalVar40_g62978 = Output_Lighting525_g62695;
				float Debug_Index464_g62695 = TVE_DEBUG_Index;
				float2 uv_MainAlbedoTex = IN.ase_texcoord8.yz * _MainAlbedoTex_ST.xy + _MainAlbedoTex_ST.zw;
				float4 tex2DNode586_g62695 = tex2D( _MainAlbedoTex, uv_MainAlbedoTex );
				float3 appendResult637_g62695 = (float3(tex2DNode586_g62695.r , tex2DNode586_g62695.g , tex2DNode586_g62695.b));
				float3 ifLocalVar40_g63010 = 0;
				if( Debug_Index464_g62695 == 0.0 )
				ifLocalVar40_g63010 = appendResult637_g62695;
				float ifLocalVar40_g63032 = 0;
				if( Debug_Index464_g62695 == 1.0 )
				ifLocalVar40_g63032 = tex2D( _MainAlbedoTex, uv_MainAlbedoTex ).a;
				float2 uv_MainNormalTex = IN.ase_texcoord8.yz * _MainNormalTex_ST.xy + _MainNormalTex_ST.zw;
				float4 tex2DNode604_g62695 = tex2D( _MainNormalTex, uv_MainNormalTex );
				float3 appendResult876_g62695 = (float3(tex2DNode604_g62695.a , tex2DNode604_g62695.g , 1.0));
				float3 gammaToLinear878_g62695 = GammaToLinearSpace( appendResult876_g62695 );
				float3 ifLocalVar40_g62951 = 0;
				if( Debug_Index464_g62695 == 2.0 )
				ifLocalVar40_g62951 = gammaToLinear878_g62695;
				float2 uv_MainMaskTex = IN.ase_texcoord8.yz * _MainMaskTex_ST.xy + _MainMaskTex_ST.zw;
				float ifLocalVar40_g63024 = 0;
				if( Debug_Index464_g62695 == 3.0 )
				ifLocalVar40_g63024 = tex2D( _MainMaskTex, uv_MainMaskTex ).r;
				float ifLocalVar40_g62976 = 0;
				if( Debug_Index464_g62695 == 4.0 )
				ifLocalVar40_g62976 = tex2D( _MainMaskTex, uv_MainMaskTex ).g;
				float ifLocalVar40_g62997 = 0;
				if( Debug_Index464_g62695 == 5.0 )
				ifLocalVar40_g62997 = tex2D( _MainMaskTex, uv_MainMaskTex ).b;
				float ifLocalVar40_g63013 = 0;
				if( Debug_Index464_g62695 == 6.0 )
				ifLocalVar40_g63013 = tex2D( _MainMaskTex, uv_MainMaskTex ).a;
				float2 uv_SecondAlbedoTex = IN.ase_texcoord8.yz * _SecondAlbedoTex_ST.xy + _SecondAlbedoTex_ST.zw;
				float4 tex2DNode854_g62695 = tex2D( _SecondAlbedoTex, uv_SecondAlbedoTex );
				float3 appendResult839_g62695 = (float3(tex2DNode854_g62695.r , tex2DNode854_g62695.g , tex2DNode854_g62695.b));
				float3 ifLocalVar40_g63020 = 0;
				if( Debug_Index464_g62695 == 7.0 )
				ifLocalVar40_g63020 = appendResult839_g62695;
				float ifLocalVar40_g62934 = 0;
				if( Debug_Index464_g62695 == 8.0 )
				ifLocalVar40_g62934 = tex2D( _SecondAlbedoTex, uv_SecondAlbedoTex ).a;
				float2 uv_SecondNormalTex = IN.ase_texcoord8.yz * _SecondNormalTex_ST.xy + _SecondNormalTex_ST.zw;
				float4 tex2DNode841_g62695 = tex2D( _SecondNormalTex, uv_SecondNormalTex );
				float3 appendResult880_g62695 = (float3(tex2DNode841_g62695.a , tex2DNode841_g62695.g , 1.0));
				float3 gammaToLinear879_g62695 = GammaToLinearSpace( appendResult880_g62695 );
				float3 ifLocalVar40_g62985 = 0;
				if( Debug_Index464_g62695 == 8.0 )
				ifLocalVar40_g62985 = gammaToLinear879_g62695;
				float2 uv_SecondMaskTex = IN.ase_texcoord8.yz * _SecondMaskTex_ST.xy + _SecondMaskTex_ST.zw;
				float ifLocalVar40_g62950 = 0;
				if( Debug_Index464_g62695 == 10.0 )
				ifLocalVar40_g62950 = tex2D( _SecondMaskTex, uv_SecondMaskTex ).r;
				float ifLocalVar40_g62955 = 0;
				if( Debug_Index464_g62695 == 11.0 )
				ifLocalVar40_g62955 = tex2D( _SecondMaskTex, uv_SecondMaskTex ).g;
				float ifLocalVar40_g62983 = 0;
				if( Debug_Index464_g62695 == 12.0 )
				ifLocalVar40_g62983 = tex2D( _SecondMaskTex, uv_SecondMaskTex ).b;
				float ifLocalVar40_g62988 = 0;
				if( Debug_Index464_g62695 == 13.0 )
				ifLocalVar40_g62988 = tex2D( _SecondMaskTex, uv_SecondMaskTex ).a;
				float2 uv_EmissiveTex = IN.ase_texcoord8.yz * _EmissiveTex_ST.xy + _EmissiveTex_ST.zw;
				float4 tex2DNode858_g62695 = tex2D( _EmissiveTex, uv_EmissiveTex );
				float3 appendResult867_g62695 = (float3(tex2DNode858_g62695.r , tex2DNode858_g62695.g , tex2DNode858_g62695.b));
				float3 ifLocalVar40_g62956 = 0;
				if( Debug_Index464_g62695 == 14.0 )
				ifLocalVar40_g62956 = appendResult867_g62695;
				float Debug_Min721_g62695 = TVE_DEBUG_Min;
				float temp_output_7_0_g62962 = Debug_Min721_g62695;
				float4 temp_cast_3 = (temp_output_7_0_g62962).xxxx;
				float Debug_Max723_g62695 = TVE_DEBUG_Max;
				float4 Output_Maps561_g62695 = ( ( ( float4( ( ( ifLocalVar40_g63010 + ifLocalVar40_g63032 + ifLocalVar40_g62951 ) + ( ifLocalVar40_g63024 + ifLocalVar40_g62976 + ifLocalVar40_g62997 + ifLocalVar40_g63013 ) ) , 0.0 ) + float4( ( ( ( ifLocalVar40_g63020 + ifLocalVar40_g62934 + ifLocalVar40_g62985 ) + ( ifLocalVar40_g62950 + ifLocalVar40_g62955 + ifLocalVar40_g62983 + ifLocalVar40_g62988 ) ) * _DetailMode ) , 0.0 ) + ( ( float4( ifLocalVar40_g62956 , 0.0 ) * _EmissiveColor ) * _EmissiveCat ) ) - temp_cast_3 ) / ( Debug_Max723_g62695 - temp_output_7_0_g62962 ) );
				float4 ifLocalVar40_g62936 = 0;
				if( Debug_Type367_g62695 == 3.0 )
				ifLocalVar40_g62936 = Output_Maps561_g62695;
				float Resolution44_g62953 = max( _MainAlbedoTex_TexelSize.z , _MainAlbedoTex_TexelSize.w );
				float4 color62_g62953 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g62953 = 0;
				if( Resolution44_g62953 <= 256.0 )
				ifLocalVar61_g62953 = color62_g62953;
				float4 color55_g62953 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g62953 = 0;
				if( Resolution44_g62953 == 512.0 )
				ifLocalVar56_g62953 = color55_g62953;
				float4 color42_g62953 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g62953 = 0;
				if( Resolution44_g62953 == 1024.0 )
				ifLocalVar40_g62953 = color42_g62953;
				float4 color48_g62953 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g62953 = 0;
				if( Resolution44_g62953 == 2048.0 )
				ifLocalVar47_g62953 = color48_g62953;
				float4 color51_g62953 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g62953 = 0;
				if( Resolution44_g62953 >= 4096.0 )
				ifLocalVar52_g62953 = color51_g62953;
				float4 ifLocalVar40_g62987 = 0;
				if( Debug_Index464_g62695 == 0.0 )
				ifLocalVar40_g62987 = ( ifLocalVar61_g62953 + ifLocalVar56_g62953 + ifLocalVar40_g62953 + ifLocalVar47_g62953 + ifLocalVar52_g62953 );
				float Resolution44_g63000 = max( _MainNormalTex_TexelSize.z , _MainNormalTex_TexelSize.w );
				float4 color62_g63000 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g63000 = 0;
				if( Resolution44_g63000 <= 256.0 )
				ifLocalVar61_g63000 = color62_g63000;
				float4 color55_g63000 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g63000 = 0;
				if( Resolution44_g63000 == 512.0 )
				ifLocalVar56_g63000 = color55_g63000;
				float4 color42_g63000 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g63000 = 0;
				if( Resolution44_g63000 == 1024.0 )
				ifLocalVar40_g63000 = color42_g63000;
				float4 color48_g63000 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g63000 = 0;
				if( Resolution44_g63000 == 2048.0 )
				ifLocalVar47_g63000 = color48_g63000;
				float4 color51_g63000 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g63000 = 0;
				if( Resolution44_g63000 >= 4096.0 )
				ifLocalVar52_g63000 = color51_g63000;
				float4 ifLocalVar40_g63025 = 0;
				if( Debug_Index464_g62695 == 1.0 )
				ifLocalVar40_g63025 = ( ifLocalVar61_g63000 + ifLocalVar56_g63000 + ifLocalVar40_g63000 + ifLocalVar47_g63000 + ifLocalVar52_g63000 );
				float Resolution44_g62954 = max( _MainMaskTex_TexelSize.z , _MainMaskTex_TexelSize.w );
				float4 color62_g62954 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g62954 = 0;
				if( Resolution44_g62954 <= 256.0 )
				ifLocalVar61_g62954 = color62_g62954;
				float4 color55_g62954 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g62954 = 0;
				if( Resolution44_g62954 == 512.0 )
				ifLocalVar56_g62954 = color55_g62954;
				float4 color42_g62954 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g62954 = 0;
				if( Resolution44_g62954 == 1024.0 )
				ifLocalVar40_g62954 = color42_g62954;
				float4 color48_g62954 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g62954 = 0;
				if( Resolution44_g62954 == 2048.0 )
				ifLocalVar47_g62954 = color48_g62954;
				float4 color51_g62954 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g62954 = 0;
				if( Resolution44_g62954 >= 4096.0 )
				ifLocalVar52_g62954 = color51_g62954;
				float4 ifLocalVar40_g63001 = 0;
				if( Debug_Index464_g62695 == 2.0 )
				ifLocalVar40_g63001 = ( ifLocalVar61_g62954 + ifLocalVar56_g62954 + ifLocalVar40_g62954 + ifLocalVar47_g62954 + ifLocalVar52_g62954 );
				float Resolution44_g62963 = max( _SecondAlbedoTex_TexelSize.z , _SecondAlbedoTex_TexelSize.w );
				float4 color62_g62963 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g62963 = 0;
				if( Resolution44_g62963 <= 256.0 )
				ifLocalVar61_g62963 = color62_g62963;
				float4 color55_g62963 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g62963 = 0;
				if( Resolution44_g62963 == 512.0 )
				ifLocalVar56_g62963 = color55_g62963;
				float4 color42_g62963 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g62963 = 0;
				if( Resolution44_g62963 == 1024.0 )
				ifLocalVar40_g62963 = color42_g62963;
				float4 color48_g62963 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g62963 = 0;
				if( Resolution44_g62963 == 2048.0 )
				ifLocalVar47_g62963 = color48_g62963;
				float4 color51_g62963 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g62963 = 0;
				if( Resolution44_g62963 >= 4096.0 )
				ifLocalVar52_g62963 = color51_g62963;
				float4 ifLocalVar40_g62996 = 0;
				if( Debug_Index464_g62695 == 3.0 )
				ifLocalVar40_g62996 = ( ifLocalVar61_g62963 + ifLocalVar56_g62963 + ifLocalVar40_g62963 + ifLocalVar47_g62963 + ifLocalVar52_g62963 );
				float Resolution44_g63036 = max( _SecondMaskTex_TexelSize.z , _SecondMaskTex_TexelSize.w );
				float4 color62_g63036 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g63036 = 0;
				if( Resolution44_g63036 <= 256.0 )
				ifLocalVar61_g63036 = color62_g63036;
				float4 color55_g63036 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g63036 = 0;
				if( Resolution44_g63036 == 512.0 )
				ifLocalVar56_g63036 = color55_g63036;
				float4 color42_g63036 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g63036 = 0;
				if( Resolution44_g63036 == 1024.0 )
				ifLocalVar40_g63036 = color42_g63036;
				float4 color48_g63036 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g63036 = 0;
				if( Resolution44_g63036 == 2048.0 )
				ifLocalVar47_g63036 = color48_g63036;
				float4 color51_g63036 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g63036 = 0;
				if( Resolution44_g63036 >= 4096.0 )
				ifLocalVar52_g63036 = color51_g63036;
				float4 ifLocalVar40_g63009 = 0;
				if( Debug_Index464_g62695 == 4.0 )
				ifLocalVar40_g63009 = ( ifLocalVar61_g63036 + ifLocalVar56_g63036 + ifLocalVar40_g63036 + ifLocalVar47_g63036 + ifLocalVar52_g63036 );
				float Resolution44_g62964 = max( _SecondAlbedoTex_TexelSize.z , _SecondAlbedoTex_TexelSize.w );
				float4 color62_g62964 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g62964 = 0;
				if( Resolution44_g62964 <= 256.0 )
				ifLocalVar61_g62964 = color62_g62964;
				float4 color55_g62964 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g62964 = 0;
				if( Resolution44_g62964 == 512.0 )
				ifLocalVar56_g62964 = color55_g62964;
				float4 color42_g62964 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g62964 = 0;
				if( Resolution44_g62964 == 1024.0 )
				ifLocalVar40_g62964 = color42_g62964;
				float4 color48_g62964 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g62964 = 0;
				if( Resolution44_g62964 == 2048.0 )
				ifLocalVar47_g62964 = color48_g62964;
				float4 color51_g62964 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g62964 = 0;
				if( Resolution44_g62964 >= 4096.0 )
				ifLocalVar52_g62964 = color51_g62964;
				float4 ifLocalVar40_g63033 = 0;
				if( Debug_Index464_g62695 == 5.0 )
				ifLocalVar40_g63033 = ( ifLocalVar61_g62964 + ifLocalVar56_g62964 + ifLocalVar40_g62964 + ifLocalVar47_g62964 + ifLocalVar52_g62964 );
				float Resolution44_g63038 = max( _EmissiveTex_TexelSize.z , _EmissiveTex_TexelSize.w );
				float4 color62_g63038 = IsGammaSpace() ? float4(0.484069,0.862666,0.9245283,0) : float4(0.1995908,0.7155456,0.8368256,0);
				float4 ifLocalVar61_g63038 = 0;
				if( Resolution44_g63038 <= 256.0 )
				ifLocalVar61_g63038 = color62_g63038;
				float4 color55_g63038 = IsGammaSpace() ? float4(0.1933962,0.7383016,1,0) : float4(0.03108436,0.5044825,1,0);
				float4 ifLocalVar56_g63038 = 0;
				if( Resolution44_g63038 == 512.0 )
				ifLocalVar56_g63038 = color55_g63038;
				float4 color42_g63038 = IsGammaSpace() ? float4(0.4431373,0.7921569,0.1764706,0) : float4(0.1651322,0.5906189,0.02624122,0);
				float4 ifLocalVar40_g63038 = 0;
				if( Resolution44_g63038 == 1024.0 )
				ifLocalVar40_g63038 = color42_g63038;
				float4 color48_g63038 = IsGammaSpace() ? float4(1,0.6889491,0.07075471,0) : float4(1,0.4324122,0.006068094,0);
				float4 ifLocalVar47_g63038 = 0;
				if( Resolution44_g63038 == 2048.0 )
				ifLocalVar47_g63038 = color48_g63038;
				float4 color51_g63038 = IsGammaSpace() ? float4(1,0.2066492,0.0990566,0) : float4(1,0.03521443,0.009877041,0);
				float4 ifLocalVar52_g63038 = 0;
				if( Resolution44_g63038 >= 4096.0 )
				ifLocalVar52_g63038 = color51_g63038;
				float4 ifLocalVar40_g62961 = 0;
				if( Debug_Index464_g62695 == 6.0 )
				ifLocalVar40_g62961 = ( ifLocalVar61_g63038 + ifLocalVar56_g63038 + ifLocalVar40_g63038 + ifLocalVar47_g63038 + ifLocalVar52_g63038 );
				float4 Output_Resolution737_g62695 = ( ( ifLocalVar40_g62987 + ifLocalVar40_g63025 + ifLocalVar40_g63001 ) + ( ifLocalVar40_g62996 + ifLocalVar40_g63009 + ifLocalVar40_g63033 ) + ifLocalVar40_g62961 );
				float4 ifLocalVar40_g62941 = 0;
				if( Debug_Type367_g62695 == 4.0 )
				ifLocalVar40_g62941 = Output_Resolution737_g62695;
				float3 WorldPosition893_g62695 = worldPos;
				half3 Input_Position419_g62965 = -WorldPosition893_g62695;
				float4 temp_output_91_19_g62942 = TVE_MotionCoords;
				float3 Position83_g62942 = WorldPosition893_g62695;
				float temp_output_84_0_g62942 = _LayerMotionValue;
				float4 lerpResult87_g62942 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY( TVE_MotionTex, samplerTVE_MotionTex, float3(( (temp_output_91_19_g62942).zw + ( (temp_output_91_19_g62942).xy * (Position83_g62942).xz ) ),temp_output_84_0_g62942) ) , TVE_MotionUsage[(int)temp_output_84_0_g62942]);
				float4 break322_g62966 = lerpResult87_g62942;
				float3 appendResult397_g62966 = (float3(break322_g62966.x , 0.0 , break322_g62966.y));
				float3 temp_output_398_0_g62966 = (appendResult397_g62966*2.0 + -1.0);
				half2 Wind_DirectionWS1031_g62695 = (temp_output_398_0_g62966).xz;
				half2 Input_DirectionWS423_g62965 = Wind_DirectionWS1031_g62695;
				float Motion_Scale287_g62965 = ( _MotionScale_10 + 1.0 );
				half Global_Scale448_g62965 = TVE_NoiseParams.y;
				half Input_Speed62_g62965 = _MotionSpeed_10;
				half Global_Speed449_g62965 = TVE_NoiseParams.x;
				float mulTime426_g62965 = _Time.y * ( Input_Speed62_g62965 * Global_Speed449_g62965 );
				half Global_DistortionScale453_g62965 = TVE_NoiseParams.w;
				float3 break461_g62965 = ( Input_Position419_g62965 * Global_DistortionScale453_g62965 );
				half Global_Distortion452_g62965 = TVE_NoiseParams.z;
				float Noise_Distortion469_g62965 = ( sin( ( break461_g62965.x + break461_g62965.z ) ) * Global_Distortion452_g62965 );
				half Motion_Variation284_g62965 = 0.0;
				float2 temp_output_425_0_g62965 = ( ( (Input_Position419_g62965).xz * Input_DirectionWS423_g62965 * Motion_Scale287_g62965 * Global_Scale448_g62965 ) + ( mulTime426_g62965 + Noise_Distortion469_g62965 + Motion_Variation284_g62965 ) );
				float2 break500_g62965 = ( temp_output_425_0_g62965 * 0.1178 );
				float2 break494_g62965 = ( temp_output_425_0_g62965 * 0.1742 );
				half Wind_Power369_g62966 = break322_g62966.z;
				half Wind_Pow1128_g62695 = Wind_Power369_g62966;
				half Input_WindPower327_g62965 = Wind_Pow1128_g62695;
				float lerpResult430_g62965 = lerp( 1.0 , 0.4 , Input_WindPower327_g62965);
				half Motion_Noise915_g62695 = pow( (( sin( ( break500_g62965.x + break500_g62965.y ) ) * sin( ( break494_g62965.x + break494_g62965.y ) ) )*0.5 + 0.5) , lerpResult430_g62965 );
				float ifLocalVar40_g63023 = 0;
				if( Debug_Index464_g62695 == 0.0 )
				ifLocalVar40_g63023 = Motion_Noise915_g62695;
				float4 temp_output_91_19_g62930 = TVE_ColorsCoords;
				float3 Position58_g62930 = WorldPosition893_g62695;
				float Debug_Layer885_g62695 = TVE_DEBUG_Layer;
				float temp_output_82_0_g62930 = Debug_Layer885_g62695;
				float4 lerpResult88_g62930 = lerp( TVE_ColorsParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ColorsTex, samplerTVE_ColorsTex, float3(( (temp_output_91_19_g62930).zw + ( (temp_output_91_19_g62930).xy * (Position58_g62930).xz ) ),temp_output_82_0_g62930) ) , TVE_ColorsUsage[(int)temp_output_82_0_g62930]);
				float3 ifLocalVar40_g63040 = 0;
				if( Debug_Index464_g62695 == 1.0 )
				ifLocalVar40_g63040 = (lerpResult88_g62930).rgb;
				float4 temp_output_91_19_g62926 = TVE_ColorsCoords;
				float3 Position58_g62926 = WorldPosition893_g62695;
				float temp_output_82_0_g62926 = Debug_Layer885_g62695;
				float4 lerpResult88_g62926 = lerp( TVE_ColorsParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ColorsTex, samplerTVE_ColorsTex, float3(( (temp_output_91_19_g62926).zw + ( (temp_output_91_19_g62926).xy * (Position58_g62926).xz ) ),temp_output_82_0_g62926) ) , TVE_ColorsUsage[(int)temp_output_82_0_g62926]);
				float ifLocalVar40_g63031 = 0;
				if( Debug_Index464_g62695 == 2.0 )
				ifLocalVar40_g63031 = (lerpResult88_g62926).a;
				float4 temp_output_93_19_g62922 = TVE_ExtrasCoords;
				float3 Position82_g62922 = WorldPosition893_g62695;
				float temp_output_84_0_g62922 = Debug_Layer885_g62695;
				float4 lerpResult88_g62922 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, float3(( (temp_output_93_19_g62922).zw + ( (temp_output_93_19_g62922).xy * (Position82_g62922).xz ) ),temp_output_84_0_g62922) ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62922]);
				float4 break89_g62922 = lerpResult88_g62922;
				float ifLocalVar40_g63021 = 0;
				if( Debug_Index464_g62695 == 3.0 )
				ifLocalVar40_g63021 = break89_g62922.r;
				float4 temp_output_93_19_g63016 = TVE_ExtrasCoords;
				float3 Position82_g63016 = WorldPosition893_g62695;
				float temp_output_84_0_g63016 = Debug_Layer885_g62695;
				float4 lerpResult88_g63016 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, float3(( (temp_output_93_19_g63016).zw + ( (temp_output_93_19_g63016).xy * (Position82_g63016).xz ) ),temp_output_84_0_g63016) ) , TVE_ExtrasUsage[(int)temp_output_84_0_g63016]);
				float4 break89_g63016 = lerpResult88_g63016;
				float ifLocalVar40_g63022 = 0;
				if( Debug_Index464_g62695 == 4.0 )
				ifLocalVar40_g63022 = break89_g63016.g;
				float4 temp_output_93_19_g62946 = TVE_ExtrasCoords;
				float3 Position82_g62946 = WorldPosition893_g62695;
				float temp_output_84_0_g62946 = Debug_Layer885_g62695;
				float4 lerpResult88_g62946 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, float3(( (temp_output_93_19_g62946).zw + ( (temp_output_93_19_g62946).xy * (Position82_g62946).xz ) ),temp_output_84_0_g62946) ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62946]);
				float4 break89_g62946 = lerpResult88_g62946;
				float ifLocalVar40_g63015 = 0;
				if( Debug_Index464_g62695 == 5.0 )
				ifLocalVar40_g63015 = break89_g62946.b;
				float4 temp_output_93_19_g62957 = TVE_ExtrasCoords;
				float3 Position82_g62957 = WorldPosition893_g62695;
				float temp_output_84_0_g62957 = Debug_Layer885_g62695;
				float4 lerpResult88_g62957 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, float3(( (temp_output_93_19_g62957).zw + ( (temp_output_93_19_g62957).xy * (Position82_g62957).xz ) ),temp_output_84_0_g62957) ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62957]);
				float4 break89_g62957 = lerpResult88_g62957;
				float ifLocalVar40_g63002 = 0;
				if( Debug_Index464_g62695 == 6.0 )
				ifLocalVar40_g63002 = break89_g62957.a;
				float4 temp_output_91_19_g62937 = TVE_MotionCoords;
				float3 Position83_g62937 = WorldPosition893_g62695;
				float temp_output_84_0_g62937 = Debug_Layer885_g62695;
				float4 lerpResult87_g62937 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY( TVE_MotionTex, samplerTVE_MotionTex, float3(( (temp_output_91_19_g62937).zw + ( (temp_output_91_19_g62937).xy * (Position83_g62937).xz ) ),temp_output_84_0_g62937) ) , TVE_MotionUsage[(int)temp_output_84_0_g62937]);
				float3 appendResult1012_g62695 = (float3((lerpResult87_g62937).rg , 0.0));
				float3 ifLocalVar40_g63005 = 0;
				if( Debug_Index464_g62695 == 7.0 )
				ifLocalVar40_g63005 = appendResult1012_g62695;
				float4 temp_output_91_19_g62979 = TVE_MotionCoords;
				float3 Position83_g62979 = WorldPosition893_g62695;
				float temp_output_84_0_g62979 = Debug_Layer885_g62695;
				float4 lerpResult87_g62979 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY( TVE_MotionTex, samplerTVE_MotionTex, float3(( (temp_output_91_19_g62979).zw + ( (temp_output_91_19_g62979).xy * (Position83_g62979).xz ) ),temp_output_84_0_g62979) ) , TVE_MotionUsage[(int)temp_output_84_0_g62979]);
				float ifLocalVar40_g63006 = 0;
				if( Debug_Index464_g62695 == 8.0 )
				ifLocalVar40_g63006 = (lerpResult87_g62979).b;
				float4 temp_output_91_19_g62990 = TVE_MotionCoords;
				float3 Position83_g62990 = WorldPosition893_g62695;
				float temp_output_84_0_g62990 = Debug_Layer885_g62695;
				float4 lerpResult87_g62990 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY( TVE_MotionTex, samplerTVE_MotionTex, float3(( (temp_output_91_19_g62990).zw + ( (temp_output_91_19_g62990).xy * (Position83_g62990).xz ) ),temp_output_84_0_g62990) ) , TVE_MotionUsage[(int)temp_output_84_0_g62990]);
				float ifLocalVar40_g62977 = 0;
				if( Debug_Index464_g62695 == 9.0 )
				ifLocalVar40_g62977 = (lerpResult87_g62990).a;
				float4 temp_output_94_19_g62967 = TVE_VertexCoords;
				float3 Position83_g62967 = WorldPosition893_g62695;
				float temp_output_84_0_g62967 = Debug_Layer885_g62695;
				float4 lerpResult87_g62967 = lerp( TVE_VertexParams , SAMPLE_TEXTURE2D_ARRAY( TVE_VertexTex, samplerTVE_VertexTex, float3(( (temp_output_94_19_g62967).zw + ( (temp_output_94_19_g62967).xy * (Position83_g62967).xz ) ),temp_output_84_0_g62967) ) , TVE_VertexUsage[(int)temp_output_84_0_g62967]);
				float ifLocalVar40_g62952 = 0;
				if( Debug_Index464_g62695 == 10.0 )
				ifLocalVar40_g62952 = (lerpResult87_g62967).a;
				float3 Output_Globals888_g62695 = ( ifLocalVar40_g63023 + ( ifLocalVar40_g63040 + ifLocalVar40_g63031 ) + ( ifLocalVar40_g63021 + ifLocalVar40_g63022 + ifLocalVar40_g63015 + ifLocalVar40_g63002 ) + ( ifLocalVar40_g63005 + ifLocalVar40_g63006 + ifLocalVar40_g62977 ) + ( ifLocalVar40_g62952 + 0.0 ) );
				float3 ifLocalVar40_g63034 = 0;
				if( Debug_Type367_g62695 == 8.0 )
				ifLocalVar40_g63034 = Output_Globals888_g62695;
				float3 vertexToFrag328_g62695 = IN.ase_texcoord9.xyz;
				float4 color1016_g62695 = IsGammaSpace() ? float4(0.5831653,0.6037736,0.2135992,0) : float4(0.2992498,0.3229691,0.03750122,0);
				float4 color1017_g62695 = IsGammaSpace() ? float4(0.8117647,0.3488252,0.2627451,0) : float4(0.6239604,0.0997834,0.05612849,0);
				float4 switchResult1015_g62695 = (((ase_vface>0)?(color1016_g62695):(color1017_g62695)));
				float3 ifLocalVar40_g63012 = 0;
				if( Debug_Index464_g62695 == 4.0 )
				ifLocalVar40_g63012 = (switchResult1015_g62695).rgb;
				float temp_output_7_0_g63011 = Debug_Min721_g62695;
				float3 temp_cast_28 = (temp_output_7_0_g63011).xxx;
				float3 Output_Mesh316_g62695 = saturate( ( ( ( vertexToFrag328_g62695 + ifLocalVar40_g63012 ) - temp_cast_28 ) / ( Debug_Max723_g62695 - temp_output_7_0_g63011 ) ) );
				float3 ifLocalVar40_g63039 = 0;
				if( Debug_Type367_g62695 == 9.0 )
				ifLocalVar40_g63039 = Output_Mesh316_g62695;
				float4 color1086_g62695 = IsGammaSpace() ? float4(0.1226415,0.1226415,0.1226415,0) : float4(0.01390275,0.01390275,0.01390275,0);
				float4 vertexToFrag11_g63037 = IN.ase_texcoord10;
				float _IsVegetationShader1101_g62695 = _IsVegetationShader;
				float4 lerpResult1089_g62695 = lerp( color1086_g62695 , vertexToFrag11_g63037 , ( _IsPolygonalShader1112_g62695 * _IsVegetationShader1101_g62695 ));
				float3 Output_Misc1080_g62695 = (lerpResult1089_g62695).rgb;
				float3 ifLocalVar40_g62935 = 0;
				if( Debug_Type367_g62695 == 10.0 )
				ifLocalVar40_g62935 = Output_Misc1080_g62695;
				float4 temp_output_459_0_g62695 = ( ifLocalVar40_g63004 + ifLocalVar40_g62984 + ifLocalVar40_g62978 + ifLocalVar40_g62936 + ifLocalVar40_g62941 + float4( ifLocalVar40_g63034 , 0.0 ) + float4( ifLocalVar40_g63039 , 0.0 ) + float4( ifLocalVar40_g62935 , 0.0 ) );
				float4 color690_g62695 = IsGammaSpace() ? float4(0.1226415,0.1226415,0.1226415,0) : float4(0.01390275,0.01390275,0.01390275,0);
				float _IsTVEShader647_g62695 = _IsTVEShader;
				float4 lerpResult689_g62695 = lerp( color690_g62695 , temp_output_459_0_g62695 , _IsTVEShader647_g62695);
				float Debug_Filter322_g62695 = TVE_DEBUG_Filter;
				float4 lerpResult326_g62695 = lerp( temp_output_459_0_g62695 , lerpResult689_g62695 , Debug_Filter322_g62695);
				float Debug_Clip623_g62695 = TVE_DEBUG_Clip;
				float lerpResult622_g62695 = lerp( 1.0 , tex2D( _MainAlbedoTex, uv_MainAlbedoTex ).a , ( Debug_Clip623_g62695 * _RenderClip ));
				clip( lerpResult622_g62695 - _Cutoff);
				clip( ( 1.0 - saturate( ( _IsElementShader + _IsHelperShader ) ) ) - 1.0);
				
				o.Albedo = fixed3( 0.5, 0.5, 0.5 );
				o.Normal = fixed3( 0, 0, 1 );
				o.Emission = lerpResult326_g62695.rgb;
				#if defined(_SPECULAR_SETUP)
					o.Specular = fixed3( 0, 0, 0 );
				#else
					o.Metallic = 0;
				#endif
				o.Smoothness = 0;
				o.Occlusion = 1;
				o.Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float3 BakedGI = 0;

				#ifdef _ALPHATEST_ON
					clip( o.Alpha - AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
					outputDepth = IN.pos.z;
				#endif

				#ifndef USING_DIRECTIONAL_LIGHT
					fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				#else
					fixed3 lightDir = _WorldSpaceLightPos0.xyz;
				#endif

				float3 worldN;
				worldN.x = dot(IN.tSpace0.xyz, o.Normal);
				worldN.y = dot(IN.tSpace1.xyz, o.Normal);
				worldN.z = dot(IN.tSpace2.xyz, o.Normal);
				worldN = normalize(worldN);
				o.Normal = worldN;

				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
				gi.indirect.diffuse = 0;
				gi.indirect.specular = 0;
				gi.light.color = 0;
				gi.light.dir = half3(0,1,0);

				UnityGIInput giInput;
				UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
				giInput.light = gi.light;
				giInput.worldPos = worldPos;
				giInput.worldViewDir = worldViewDir;
				giInput.atten = atten;
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
					giInput.lightmapUV = IN.lmap;
				#else
					giInput.lightmapUV = 0.0;
				#endif
				#if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
					giInput.ambient = IN.sh;
				#else
					giInput.ambient.rgb = 0.0;
				#endif
				giInput.probeHDR[0] = unity_SpecCube0_HDR;
				giInput.probeHDR[1] = unity_SpecCube1_HDR;
				#if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
					giInput.boxMin[0] = unity_SpecCube0_BoxMin;
				#endif
				#ifdef UNITY_SPECCUBE_BOX_PROJECTION
					giInput.boxMax[0] = unity_SpecCube0_BoxMax;
					giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
					giInput.boxMax[1] = unity_SpecCube1_BoxMax;
					giInput.boxMin[1] = unity_SpecCube1_BoxMin;
					giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
				#endif

				#if defined(_SPECULAR_SETUP)
					LightingStandardSpecular_GI( o, giInput, gi );
				#else
					LightingStandard_GI( o, giInput, gi );
				#endif

				#ifdef ASE_BAKEDGI
					gi.indirect.diffuse = BakedGI;
				#endif

				#if UNITY_SHOULD_SAMPLE_SH && !defined(LIGHTMAP_ON) && defined(ASE_NO_AMBIENT)
					gi.indirect.diffuse = 0;
				#endif

				#if defined(_SPECULAR_SETUP)
					outEmission = LightingStandardSpecular_Deferred( o, worldViewDir, gi, outGBuffer0, outGBuffer1, outGBuffer2 );
				#else
					outEmission = LightingStandard_Deferred( o, worldViewDir, gi, outGBuffer0, outGBuffer1, outGBuffer2 );
				#endif

				#if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
					outShadowMask = UnityGetRawBakedOcclusions (IN.lmap.xy, float3(0, 0, 0));
				#endif
				#ifndef UNITY_HDR_ON
					outEmission.rgb = exp2(-outEmission.rgb);
				#endif
			}
			ENDCG
		}

	
	}
	
	
	Dependency "LightMode"="ForwardBase"

	
}
/*ASEBEGIN
Version=18935
1920;0;1920;1029;2288.421;5752.535;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;2069;-1792,-4992;Half;False;Global;TVE_DEBUG_Min;TVE_DEBUG_Min;4;0;Create;True;0;5;Vertex Colors;100;Texture Coords;200;Vertex Postion;300;Vertex Normals;301;Vertex Tangents;302;0;True;2;Space(10);StyledEnum (Vertex Position _Vertex Normals _VertexTangents _Vertex Sign _Vertex Red (Variation) _Vertex Green (Occlusion) _Vertex Blue (Blend) _Vertex Alpha (Height) _Motion Bending _Motion Rolling _Motion Flutter);False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2155;-1792,-5248;Half;False;Global;TVE_DEBUG_Layer;TVE_DEBUG_Layer;4;0;Create;True;0;5;Vertex Colors;100;Texture Coords;200;Vertex Postion;300;Vertex Normals;301;Vertex Tangents;302;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2013;-1792,-5312;Half;False;Global;TVE_DEBUG_Index;TVE_DEBUG_Index;4;0;Create;True;0;5;Vertex Colors;100;Texture Coords;200;Vertex Postion;300;Vertex Normals;301;Vertex Tangents;302;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1908;-1792,-5376;Half;False;Global;TVE_DEBUG_Type;TVE_DEBUG_Type;4;0;Create;True;0;5;Vertex Colors;100;Texture Coords;200;Vertex Postion;300;Vertex Normals;301;Vertex Tangents;302;0;True;2;Space(10);StyledEnum (Vertex Position _Vertex Normals _VertexTangents _Vertex Sign _Vertex Red (Variation) _Vertex Green (Occlusion) _Vertex Blue (Blend) _Vertex Alpha (Height) _Motion Bending _Motion Rolling _Motion Flutter);False;0;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1953;-1792,-5120;Half;False;Global;TVE_DEBUG_Filter;TVE_DEBUG_Filter;4;0;Create;True;0;5;Vertex Colors;100;Texture Coords;200;Vertex Postion;300;Vertex Normals;301;Vertex Tangents;302;0;True;2;Space(10);StyledEnum (Vertex Position _Vertex Normals _VertexTangents _Vertex Sign _Vertex Red (Variation) _Vertex Green (Occlusion) _Vertex Blue (Blend) _Vertex Alpha (Height) _Motion Bending _Motion Rolling _Motion Flutter);False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2032;-1792,-5056;Half;False;Global;TVE_DEBUG_Clip;TVE_DEBUG_Clip;4;0;Create;True;0;5;Vertex Colors;100;Texture Coords;200;Vertex Postion;300;Vertex Normals;301;Vertex Tangents;302;0;True;2;Space(10);StyledEnum (Vertex Position _Vertex Normals _VertexTangents _Vertex Sign _Vertex Red (Variation) _Vertex Green (Occlusion) _Vertex Blue (Blend) _Vertex Alpha (Height) _Motion Bending _Motion Rolling _Motion Flutter);False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2070;-1792,-4928;Half;False;Global;TVE_DEBUG_Max;TVE_DEBUG_Max;4;0;Create;True;0;5;Vertex Colors;100;Texture Coords;200;Vertex Postion;300;Vertex Normals;301;Vertex Tangents;302;0;True;2;Space(10);StyledEnum (Vertex Position _Vertex Normals _VertexTangents _Vertex Sign _Vertex Red (Variation) _Vertex Green (Occlusion) _Vertex Blue (Blend) _Vertex Alpha (Height) _Motion Bending _Motion Rolling _Motion Flutter);False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;1803;-1344,2944;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.3;False;4;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;2191;-1408,-5376;Inherit;False;Tool Debug;1;;62695;d48cde928c5068141abea1713047719b;0;7;336;FLOAT;0;False;465;FLOAT;0;False;884;FLOAT;0;False;337;FLOAT;0;False;624;FLOAT;0;False;720;FLOAT;0;False;722;FLOAT;0;False;1;COLOR;338
Node;AmplifyShaderEditor.RangedFloatNode;1878;-1792,-5632;Half;False;Property;_Banner;Banner;0;0;Create;True;0;0;0;True;1;StyledBanner(Debug);False;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1772;-1088,3072;Float;False;Constant;_Float3;Float 3;31;0;Create;True;0;0;0;False;0;False;24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1881;-1600,-5632;Half;False;Property;_Message;Message;74;0;Create;True;0;0;0;True;1;StyledMessage(Info, Use this shader to debug the original mesh or the converted mesh attributes., 0,0);False;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1804;-1792,2944;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;1774;-880,2944;Inherit;False;True;5;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1931;-1408,-5632;Half;False;Property;_DebugCategory;[ Debug Category ];73;0;Create;True;0;0;0;False;1;StyledCategory(Debug Settings, 5, 10);False;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;1843;-1632,2944;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1771;-1088,2944;Inherit;False;-1;;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;1800;-1472,2944;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2110;-896,-5376;Float;False;False;-1;2;ASEMaterialInspector;0;9;New Amplify Shader;ed95fe726fd7b4644bb42f4d1ddd2bcd;True;ForwardAdd;0;2;ForwardAdd;0;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;DisableBatching=False=DisableBatching;True;2;False;0;False;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;True;1;LightMode=ForwardAdd;False;False;0;True;1;LightMode=ForwardAdd;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2108;-896,-5376;Float;False;False;-1;2;ASEMaterialInspector;0;9;New Amplify Shader;ed95fe726fd7b4644bb42f4d1ddd2bcd;True;ExtraPrePass;0;0;ExtraPrePass;6;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;DisableBatching=False=DisableBatching;True;2;False;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;True;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=ForwardBase;False;False;0;-1;59;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;=;LightMode=ForwardBase;=;=;=;=;=;=;=;=;=;=;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2109;-896,-5376;Float;False;True;-1;2;;0;9;Hidden/BOXOPHOBIC/The Vegetation Engine/Helpers/Debug;ed95fe726fd7b4644bb42f4d1ddd2bcd;True;ForwardBase;0;1;ForwardBase;18;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;2;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;1;False;-1;True;3;False;-1;False;True;3;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;DisableBatching=True=DisableBatching;True;7;False;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;1;LightMode=ForwardBase;0;Standard;40;Workflow,InvertActionOnDeselection;1;0;Surface;0;0;  Blend;0;0;  Refraction Model;0;0;  Dither Shadows;1;0;Two Sided;0;0;Deferred Pass;1;0;Transmission;0;0;  Transmission Shadow;0.5,False,-1;0;Translucency;0;0;  Translucency Strength;1,False,-1;0;  Normal Distortion;0.5,False,-1;0;  Scattering;2,False,-1;0;  Direct;0.9,False,-1;0;  Ambient;0.1,False,-1;0;  Shadow;0.5,False,-1;0;Cast Shadows;0;0;  Use Shadow Threshold;0;0;Receive Shadows;0;0;GPU Instancing;0;0;LOD CrossFade;0;0;Built-in Fog;0;0;Ambient Light;0;0;Meta Pass;0;0;Add Pass;0;0;Override Baked GI;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Fwd Specular Highlights Toggle;0;0;Fwd Reflections Toggle;0;0;Disable Batching;1;0;Vertex Position,InvertActionOnDeselection;1;0;0;6;False;True;False;True;False;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2112;-896,-5376;Float;False;False;-1;2;ASEMaterialInspector;0;9;New Amplify Shader;ed95fe726fd7b4644bb42f4d1ddd2bcd;True;Meta;0;4;Meta;0;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;DisableBatching=False=DisableBatching;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;True;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;False;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2113;-896,-5376;Float;False;False;-1;2;ASEMaterialInspector;0;9;New Amplify Shader;ed95fe726fd7b4644bb42f4d1ddd2bcd;True;ShadowCaster;0;5;ShadowCaster;0;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;DisableBatching=False=DisableBatching;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;True;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;True;1;=;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2111;-896,-5376;Float;False;False;-1;2;ASEMaterialInspector;0;9;New Amplify Shader;ed95fe726fd7b4644bb42f4d1ddd2bcd;True;Deferred;0;3;Deferred;0;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;DisableBatching=False=DisableBatching;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;True;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Deferred;True;2;True;17;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;0;False;0;0;Standard;0;False;0
WireConnection;1803;0;1800;0
WireConnection;2191;336;1908;0
WireConnection;2191;465;2013;0
WireConnection;2191;884;2155;0
WireConnection;2191;337;1953;0
WireConnection;2191;624;2032;0
WireConnection;2191;720;2069;0
WireConnection;2191;722;2070;0
WireConnection;1774;0;1771;0
WireConnection;1774;1;1772;0
WireConnection;1774;3;1803;0
WireConnection;1843;0;1804;0
WireConnection;1800;0;1843;0
WireConnection;2109;2;2191;338
ASEEND*/
//CHKSM=DC46DEF7DEBD2F61A6F24206B627F5F83E1C81BD