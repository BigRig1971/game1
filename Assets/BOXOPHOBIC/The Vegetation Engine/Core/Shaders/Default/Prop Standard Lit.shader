// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/The Vegetation Engine/Default/Prop Standard Lit"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[StyledCategory(Render Settings, 5, 10)]_RenderingCat("[ Rendering Cat ]", Float) = 0
		[Enum(Opaque,0,Transparent,1)]_RenderMode("Render Mode", Float) = 0
		[Enum(Off,0,On,1)]_RenderZWrite("Render ZWrite", Float) = 1
		[Enum(Both,0,Back,1,Front,2)]_RenderCull("Render Faces", Float) = 0
		[Enum(Flip,0,Mirror,1,Same,2)]_RenderNormals("Render Normals", Float) = 0
		[HideInInspector]_RenderQueue("Render Queue", Float) = 0
		[HideInInspector]_RenderPriority("Render Priority", Float) = 0
		[Enum(Off,0,On,1)]_RenderDecals("Render Decals", Float) = 0
		[Enum(Off,0,On,1)]_RenderSSR("Render SSR/SSGI", Float) = 0
		[Enum(Off,0,On,1)][Space(10)]_RenderClip("Alpha Clipping", Float) = 1
		[Enum(Off,0,On,1)]_RenderCoverage("Alpha To Mask", Float) = 0
		_AlphaCutoffValue("Alpha Treshold", Range( 0 , 1)) = 0.5
		[StyledSpace(10)]_SpaceRenderFade("# Space Render Fade", Float) = 0
		_FadeCameraValue("Fade by Camera Distance", Range( 0 , 1)) = 1
		[StyledCategory(Global Settings)]_GlobalCat("[ Global Cat ]", Float) = 0
		[StyledEnum(TVELayers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_LayerExtrasValue("Layer Extras", Float) = 0
		[StyledSpace(10)]_SpaceGlobalLayers("# Space Global Layers", Float) = 0
		[StyledMessage(Info, Procedural Variation in use. The Variation might not work as expected when switching from one LOD to another., _VertexVariationMode, 1 , 0, 10)]_MessageGlobalsVariation("# Message Globals Variation", Float) = 0
		_GlobalOverlay("Global Overlay", Range( 0 , 1)) = 1
		_GlobalWetness("Global Wetness", Range( 0 , 1)) = 1
		_GlobalEmissive("Global Emissive", Range( 0 , 1)) = 1
		[StyledRemapSlider(_ColorsMaskMinValue, _ColorsMaskMaxValue, 0, 1, 10, 0)]_ColorsMaskRemap("Color Mask", Vector) = (0,0,0,0)
		[StyledRemapSlider(_OverlayMaskMinValue, _OverlayMaskMaxValue, 0, 1, 10, 0)]_OverlayMaskRemap("Overlay Mask", Vector) = (0,0,0,0)
		[HideInInspector]_OverlayMaskMinValue("Overlay Mask Min Value", Range( 0 , 1)) = 0.45
		[HideInInspector]_OverlayMaskMaxValue("Overlay Mask Max Value", Range( 0 , 1)) = 0.55
		[StyledRemapSlider(_AlphaMaskMinValue, _AlphaMaskMaxValue, 0, 1, 10, 0)]_AlphaMaskRemap("Alpha Mask", Vector) = (0,0,0,0)
		[StyledSpace(10)]_SpaceGlobalPosition("# Space Global Position", Float) = 0
		[StyledToggle]_ExtrasPositionMode("Use Pivot Position for Extras", Float) = 0
		[StyledCategory(Main Settings)]_MainCat("[ Main Cat ]", Float) = 0
		[NoScaleOffset][StyledTextureSingleLine]_MainAlbedoTex("Main Albedo", 2D) = "white" {}
		[NoScaleOffset][StyledTextureSingleLine]_MainNormalTex("Main Normal", 2D) = "bump" {}
		[NoScaleOffset][StyledTextureSingleLine]_MainMaskTex("Main Mask", 2D) = "white" {}
		[Space(10)][StyledVector(9)]_MainUVs("Main UVs", Vector) = (1,1,0,0)
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
		_MainNormalValue("Main Normal", Range( -8 , 8)) = 1
		_MainMetallicValue("Main Metallic", Range( 0 , 1)) = 0
		_MainOcclusionValue("Main Occlusion", Range( 0 , 1)) = 0
		_MainSmoothnessValue("Main Smoothness", Range( 0 , 1)) = 0
		[StyledRemapSlider(_LeavesMaskMinValue, _LeavesMaskMaxValue, 0, 1)]_LeavesMaskRemap("Main Leaves Mask", Vector) = (0,0,0,2)
		[StyledCategory(Detail Settings)]_DetailCat("[ Detail Cat ]", Float) = 0
		[Enum(Off,0,On,1)]_DetailMode("Detail Mode", Float) = 0
		[Enum(Overlay,0,Replace,1)]_DetailBlendMode("Detail Blend", Float) = 1
		[Enum(Vertex Blue,0,Projection,1)]_DetailTypeMode("Detail Type", Float) = 0
		[Enum(UV 0,0,UV 2,1)]_DetailCoordMode("Detail Coord", Float) = 0
		[Enum(Top,0,Bottom,1)]_DetailProjectionMode("Detail Projection", Float) = 0
		[NoScaleOffset][Space(10)][StyledTextureSingleLine]_SecondAlbedoTex("Detail Albedo", 2D) = "white" {}
		[NoScaleOffset][StyledTextureSingleLine]_SecondNormalTex("Detail Normal", 2D) = "bump" {}
		[NoScaleOffset][StyledTextureSingleLine]_SecondMaskTex("Detail Mask", 2D) = "white" {}
		[Space(10)][StyledVector(9)]_SecondUVs("Detail UVs", Vector) = (1,1,0,0)
		[HDR]_SecondColor("Detail Color", Color) = (1,1,1,1)
		_SecondNormalValue("Detail Normal", Range( -8 , 8)) = 1
		_SecondMetallicValue("Detail Metallic", Range( 0 , 1)) = 0
		_SecondOcclusionValue("Detail Occlusion", Range( 0 , 1)) = 0
		_SecondSmoothnessValue("Detail Smoothness", Range( 0 , 1)) = 0
		[Space(10)]_DetailNormalValue("Detail Use Main Normal", Range( 0 , 1)) = 0.5
		[Enum(Main Mask,0,Detail Mask,1)][Space(10)]_DetailMaskMode("Detail Mask Source", Float) = 0
		[Enum(Off,0,On,1)]_DetailMaskInvertMode("Detail Mask Invert", Float) = 0
		_DetailMeshValue("Detail Mask Offset", Range( -1 , 1)) = 0
		[StyledRemapSlider(_DetailBlendMinValue, _DetailBlendMaxValue,0,1)]_DetailBlendRemap("Detail Blending", Vector) = (0,0,0,0)
		[HideInInspector]_DetailBlendMinValue("Detail Blend Min Value", Range( 0 , 1)) = 0.2
		[HideInInspector]_DetailBlendMaxValue("Detail Blend Max Value", Range( 0 , 1)) = 0.3
		[StyledCategory(Occlusion Settings)]_OcclusionCat("[ Occlusion Cat ]", Float) = 0
		[StyledRemapSlider(_VertexOcclusionMinValue, _VertexOcclusionMaxValue, 0, 1)]_VertexOcclusionRemap("Vertex Occlusion Mask", Vector) = (0,0,0,0)
		[StyledCategory(Subsurface Settings)]_SubsurfaceCat("[ Subsurface Cat ]", Float) = 0
		[StyledRemapSlider(_SubsurfaceMaskMinValue, _SubsurfaceMaskMaxValue,0,1)]_SubsurfaceMaskRemap("Subsurface Mask", Vector) = (0,0,0,0)
		[Space(10)][DiffusionProfile]_SubsurfaceDiffusion("Subsurface Diffusion", Float) = 0
		[HideInInspector]_SubsurfaceDiffusion_Asset("Subsurface Diffusion", Vector) = (0,0,0,0)
		[HideInInspector][Space(10)][ASEDiffusionProfile(_SubsurfaceDiffusion)]_SubsurfaceDiffusion_asset("Subsurface Diffusion", Vector) = (0,0,0,0)
		[Space(10)]_SubsurfaceScatteringValue("Subsurface Scattering", Range( 0 , 16)) = 2
		_SubsurfaceAngleValue("Subsurface Angle", Range( 1 , 16)) = 8
		_SubsurfaceNormalValue("Subsurface Normal", Range( 0 , 1)) = 0
		_SubsurfaceDirectValue("Subsurface Direct", Range( 0 , 1)) = 1
		_SubsurfaceAmbientValue("Subsurface Ambient", Range( 0 , 1)) = 0.2
		_SubsurfaceShadowValue("Subsurface Shadow", Range( 0 , 1)) = 1
		[StyledCategory(Gradient Settings)]_GradientCat("[ Gradient Cat ]", Float) = 0
		[StyledRemapSlider(_GradientMinValue, _GradientMaxValue, 0, 1)]_GradientMaskRemap("Gradient Mask", Vector) = (0,0,0,0)
		[StyledCategory(Noise Settings)]_NoiseCat("[ Noise Cat ]", Float) = 0
		[StyledRemapSlider(_NoiseMinValue, _NoiseMaxValue, 0, 1)]_NoiseMaskRemap("Noise Mask", Vector) = (0,0,0,0)
		[StyledCategory(Emissive Settings)]_EmissiveCat("[ Emissive Cat]", Float) = 0
		[NoScaleOffset][StyledTextureSingleLine]_EmissiveTex("Emissive Texture", 2D) = "white" {}
		[Space(10)][StyledVector(9)]_EmissiveUVs("Emissive UVs", Vector) = (1,1,0,0)
		[Enum(None,0,Any,10,Baked,20,Realtime,30)]_EmissiveFlagMode("Emissive Baking", Float) = 0
		[HDR]_EmissiveColor("Emissive Color", Color) = (0,0,0,0)
		[StyledEmissiveIntensity]_EmissiveIntensityParams("Emissive Intensity", Vector) = (1,1,1,0)
		_EmissiveExposureValue("Emissive Weight", Range( 0 , 1)) = 1
		[StyledCategory(Perspective Settings)]_PerspectiveCat("[ Perspective Cat ]", Float) = 0
		[StyledCategory(Size Fade Settings)]_SizeFadeCat("[ Size Fade Cat ]", Float) = 0
		[StyledMessage(Info, The Size Fade feature is recommended to be used to fade out vegetation at a distance in combination with the LOD Groups or with a 3rd party culling system., _SizeFadeMode, 1, 0, 10)]_MessageSizeFade("# Message Size Fade", Float) = 0
		[StyledCategory(Motion Settings)]_MotionCat("[ Motion Cat ]", Float) = 0
		[StyledMessage(Info, Procedural variation in use. Use the Scale settings if the Variation is breaking the bending and rolling animation., _VertexVariationMode, 1 , 0, 10)]_MessageMotionVariation("# Message Motion Variation", Float) = 0
		[StyledSpace(10)]_SpaceMotionHighlight("# SpaceMotionHighlight", Float) = 0
		[ASEEnd][StyledSpace(10)]_SpaceMotionLocals("# SpaceMotionLocals", Float) = 0
		[HideInInspector]_MaxBoundsInfo("_MaxBoundsInfo", Vector) = (1,1,1,1)
		[HideInInspector]_render_normals_options("_render_normals_options", Vector) = (1,1,1,0)
		[HideInInspector]_Cutoff("Legacy Cutoff", Float) = 0.5
		[HideInInspector]_Color("Legacy Color", Color) = (0,0,0,0)
		[HideInInspector]_MainTex("Legacy MainTex", 2D) = "white" {}
		[HideInInspector]_BumpMap("Legacy BumpMap", 2D) = "white" {}
		[HideInInspector]_LayerReactValue("Legacy Layer React", Float) = 0
		[HideInInspector]_VertexRollingMode("Legacy Vertex Rolling", Float) = 1
		[HideInInspector][StyledToggle]_VertexDynamicMode("_VertexDynamicMode", Float) = 0
		[HideInInspector]_VertexVariationMode("_VertexVariationMode", Float) = 0
		[HideInInspector]_VertexMasksMode("_VertexMasksMode", Float) = 0
		[HideInInspector]_IsTVEShader("_IsTVEShader", Float) = 1
		[HideInInspector]_IsVersion("_IsVersion", Float) = 640
		[HideInInspector]_HasEmissive("_HasEmissive", Float) = 0
		[HideInInspector]_HasGradient("_HasGradient", Float) = 0
		[HideInInspector]_HasOcclusion("_HasOcclusion", Float) = 0
		[HideInInspector]_IsPropShader("_IsPropShader", Float) = 1
		[HideInInspector]_IsStandardShader("_IsStandardShader", Float) = 1
		[HideInInspector]_render_cull("_render_cull", Float) = 0
		[HideInInspector]_render_src("_render_src", Float) = 1
		[HideInInspector]_render_dst("_render_dst", Float) = 0
		[HideInInspector]_render_zw("_render_zw", Float) = 1

		[HideInInspector] _RenderQueueType("Render Queue Type", Float) = 1
		//[HideInInspector] [ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		[HideInInspector] [ToggleUI]_SupportDecals("Boolean", Float) = 1
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 0
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 6
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 8
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 8
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 40
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 40
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 4
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 4
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 14
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 10
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector] [ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 0
		[HideInInspector] [ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1
		[HideInInspector] [ToggleUI] _ReceivesSSRTransparent("Boolean", Float) = 0
		[HideInInspector] _SurfaceType("Surface Type", Float) = 0
		[HideInInspector] _BlendMode("Blend Mode", Float) = 0
		[HideInInspector] _SrcBlend("Src Blend", Float) = 1
		[HideInInspector] _DstBlend("Dst Blend", Float) = 0
		[HideInInspector] _AlphaSrcBlend("Alpha Src Blend", Float) = 1
		[HideInInspector] _AlphaDstBlend("Alpha Dst Blend", Float) = 0
		[HideInInspector] [ToggleUI] _ZWrite("ZWrite", Float) = 1
		[HideInInspector] [ToggleUI] _TransparentZWrite("Transparent ZWrite", Float) = 1
		[HideInInspector] _CullMode("Cull Mode", Float) = 2
		[HideInInspector] _TransparentSortPriority("Transparent Sort Priority", Int) = 0
		[HideInInspector] [ToggleUI] _EnableFogOnTransparent("Enable Fog On Transparent", Float) = 1
		[HideInInspector] _CullModeForward("Cull Mode Forward", Float) = 2
		[HideInInspector] [Enum(Front, 1, Back, 2)] _TransparentCullMode("Transparent Cull Mode", Float) = 2
		[HideInInspector] _ZTestDepthEqualForOpaque("ZTest Depth Equal For Opaque", Int) = 4
		[HideInInspector] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("ZTest Transparent", Float) = 4
		[HideInInspector] [ToggleUI] _TransparentBackfaceEnable("Transparent Backface Enable", Float) = 0
		[HideInInspector] [ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0
		[HideInInspector] [ToggleUI] _UseShadowThreshold("Use Shadow Threshold", Float) = 0
		[HideInInspector] [ToggleUI] _DoubleSidedEnable("Double Sided Enable", Float) = 1
		[HideInInspector] [Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double Sided Normal Mode", Float) = 2
		[HideInInspector] _DoubleSidedConstants("DoubleSidedConstants", Vector) = (1,1,-1,0)
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" "Queue"="Geometry" }

		HLSLINCLUDE
		#pragma target 4.5
		#pragma only_renderers d3d11 metal vulkan xboxone xboxseries playstation switch 
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer

		struct GlobalSurfaceDescription // GBuffer Forward META TransparentBackface
		{
			float3 Albedo;
			float3 Normal;
			float3 BentNormal;
			float3 Specular;
			float CoatMask;
			float Metallic;
			float3 Emission;
			float Smoothness;
			float Occlusion;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float AlphaClipThresholdDepthPrepass;
			float AlphaClipThresholdDepthPostpass;
			float SpecularAAScreenSpaceVariance;
			float SpecularAAThreshold;
			float SpecularOcclusion;
			float DepthOffset;
			//Refraction
			float RefractionIndex;
			float3 RefractionColor;
			float RefractionDistance;
			//SSS/Translucent
			float Thickness;
			float SubsurfaceMask;
			float DiffusionProfile;
			//Anisotropy
			float Anisotropy;
			float3 Tangent;
			//Iridescent
			float IridescenceMask;
			float IridescenceThickness;
			//BakedGI
			float3 BakedGI;
			float3 BakedBackGI;
		};

		struct AlphaSurfaceDescription // ShadowCaster
		{
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float DepthOffset;
		};

		struct SceneSurfaceDescription // SceneSelection
		{
			float Alpha;
			float AlphaClipThreshold;
			float DepthOffset;
		};

		struct PrePassSurfaceDescription // DepthPrePass
		{
			float3 Normal;
			float Smoothness;
			float Alpha;
			float AlphaClipThresholdDepthPrepass;
			float DepthOffset;
		};

		struct PostPassSurfaceDescription //DepthPostPass
		{
			float Alpha;
			float AlphaClipThresholdDepthPostpass;
			float DepthOffset;
		};

		struct SmoothSurfaceDescription // MotionVectors DepthOnly
		{
			float3 Normal;
			float Smoothness;
			float Alpha;
			float AlphaClipThreshold;
			float DepthOffset;
		};

		struct DistortionSurfaceDescription //Distortion
		{
			float Alpha;
			float2 Distortion;
			float DistortionBlur;
			float AlphaClipThreshold;
		};
		
		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
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

		float DistanceFromPlaneASE (float3 pos, float4 plane)
		{
			return dot (float4(pos,1.0f), plane);
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlaneASE(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlaneASE(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlaneASE(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlaneASE(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
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
		#endif //ASE_TESS_FUNCS
		ENDHLSL
		
		Pass
		{
			
			Name "GBuffer"
			Tags { "LightMode"="GBuffer" }

			Cull [_CullMode]
			ZTest [_ZTestGBuffer]

			Stencil
			{
				Ref [_StencilRefGBuffer]
				WriteMask [_StencilWriteMaskGBuffer]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#define ASE_NEED_CULLFACE 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 100202
			#if !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif //ASE_NEED_CULLFACE


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT

			//#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			//#pragma shader_feature_local _ALPHATEST_ON

			#if !defined(DEBUG_DISPLAY) && defined(_ALPHATEST_ON)
			#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
			#endif

			#define SHADERPASS SHADERPASS_GBUFFER
			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile _ LIGHT_LAYERS

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaxBoundsInfo;
			half4 _VertexOcclusionRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _SecondColor;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _MainUVs;
			half4 _MainColor;
			half4 _DetailBlendRemap;
			float4 _NoiseMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			half4 _AlphaMaskRemap;
			half4 _ColorsMaskRemap;
			float4 _GradientMaskRemap;
			half4 _SecondUVs;
			half4 _OverlayMaskRemap;
			float4 _EmissiveIntensityParams;
			half4 _EmissiveUVs;
			float4 _Color;
			half4 _EmissiveColor;
			half4 _LeavesMaskRemap;
			half3 _render_normals_options;
			half _SpaceMotionHighlight;
			half _SizeFadeCat;
			half _DetailTypeMode;
			half _SpaceMotionLocals;
			half _render_src;
			half _render_zw;
			half _MessageGlobalsVariation;
			half _AlphaCutoffValue;
			half _DetailCoordMode;
			half _SubsurfaceNormalValue;
			half _IsStandardShader;
			half _SpaceGlobalLayers;
			half _DetailMaskInvertMode;
			half _MainNormalValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _SecondSmoothnessValue;
			half _MainSmoothnessValue;
			half _GlobalEmissive;
			half _EmissiveExposureValue;
			half _SecondMetallicValue;
			half _MainMetallicValue;
			half _SecondNormalValue;
			half _DetailNormalValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _LayerExtrasValue;
			half _ExtrasPositionMode;
			half _GlobalOverlay;
			half _VertexDynamicMode;
			half _DetailBlendMaxValue;
			half _DetailBlendMinValue;
			half _SubsurfaceScatteringValue;
			half _DetailMaskMode;
			half _DetailProjectionMode;
			half _DetailMeshValue;
			half _VertexRollingMode;
			half _render_cull;
			half _DetailCat;
			half _RenderClip;
			half _SubsurfaceShadowValue;
			float _SubsurfaceDiffusion;
			half _SpaceRenderFade;
			half _VertexVariationMode;
			half _MainCat;
			half _HasGradient;
			half _RenderPriority;
			half _RenderQueue;
			half _SubsurfaceAmbientValue;
			half _HasEmissive;
			half _RenderMode;
			half _RenderZWrite;
			half _DetailMode;
			half _DetailBlendMode;
			half _SubsurfaceCat;
			half _EmissiveCat;
			half _IsPropShader;
			half _render_dst;
			half _VertexMasksMode;
			half _GradientCat;
			half _MotionCat;
			half _RenderCull;
			half _IsVersion;
			half _NoiseCat;
			half _SecondOcclusionValue;
			half _RenderNormals;
			half _LayerReactValue;
			half _RenderDecals;
			half _SpaceGlobalPosition;
			half _EmissiveFlagMode;
			half _HasOcclusion;
			half _Cutoff;
			half _GlobalCat;
			half _MessageMotionVariation;
			half _MessageSizeFade;
			half _OcclusionCat;
			half _RenderSSR;
			half _PerspectiveCat;
			half _SubsurfaceDirectValue;
			half _SubsurfaceAngleValue;
			half _RenderingCat;
			half _IsTVEShader;
			half _RenderCoverage;
			half _FadeCameraValue;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half TVE_Enabled;
			half _DisableSRPBatcher;
			sampler2D _MainAlbedoTex;
			sampler2D _SecondAlbedoTex;
			sampler2D _MainNormalTex;
			sampler2D _MainMaskTex;
			sampler2D _SecondMaskTex;
			half4 TVE_OverlayColor;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];
			sampler2D _SecondNormalTex;
			sampler2D _EmissiveTex;
			half TVE_OverlaySmoothness;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_FRAG_VFACE
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_DETAIL_MODE_OFF TVE_DETAIL_MODE_ON
			#pragma shader_feature_local TVE_DETAIL_TYPE_VERTEX_BLUE TVE_DETAIL_TYPE_PROJECTION
			//TVE Shader Type Defines
			#define TVE_IS_OBJECT_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_HD_PIPELINE
			//TVE HD Pipeline Defines
			#pragma shader_feature_local _DISABLE_DECALS
			#pragma shader_feature_local _DISABLE_SSR


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 interp03 : TEXCOORD3;
				float4 interp04 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};


			float3 ObjectPosition_UNITY_MATRIX_M(  )
			{
				return float3(UNITY_MATRIX_M[0].w, UNITY_MATRIX_M[1].w, UNITY_MATRIX_M[2].w );
			}
			
			float3 ASEGetEmissionHDRColor(float3 ldrColor, float luminanceIntensity, float exposureWeight, float inverseCurrentExposureMultiplier)
			{
				float3 hdrColor = ldrColor * luminanceIntensity;
				hdrColor = lerp( hdrColor* inverseCurrentExposureMultiplier, hdrColor, exposureWeight);
				return hdrColor;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				bentNormalWS = surfaceData.normalWS;

				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );


				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				#ifdef _ASE_BAKEDGI
				builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if (SHADERPASS == SHADERPASS_DISTORTION)
				builtinData.distortion = surfaceDescription.Distortion;
				builtinData.distortionBlur = surfaceDescription.DistortionBlur;
				#else
				builtinData.distortion = float2(0.0, 0.0);
				builtinData.distortionBlur = 0.0;
				#endif

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				float3 VertexPosition3588_g60053 = inputMesh.positionOS;
				float3 Final_VertexPosition890_g60053 = ( VertexPosition3588_g60053 + _DisableSRPBatcher );
				
				float2 vertexToFrag11_g64879 = ( ( inputMesh.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord5.xy = vertexToFrag11_g64879;
				float2 lerpResult1545_g60053 = lerp( inputMesh.ase_texcoord.xy , inputMesh.uv1.xy , _DetailCoordMode);
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float2 staticSwitch3466_g60053 = lerpResult1545_g60053;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float2 staticSwitch3466_g60053 = (ase_worldPos).xz;
				#else
				float2 staticSwitch3466_g60053 = lerpResult1545_g60053;
				#endif
				float2 vertexToFrag11_g64813 = ( ( staticSwitch3466_g60053 * (_SecondUVs).xy ) + (_SecondUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord5.zw = vertexToFrag11_g64813;
				half Mesh_DetailMask90_g60053 = inputMesh.ase_color.b;
				float vertexToFrag11_g64796 = ( ( Mesh_DetailMask90_g60053 - 0.5 ) + _DetailMeshValue );
				outputPackedVaryingsMeshToPS.ase_texcoord6.x = vertexToFrag11_g64796;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				float ase_vertexTangentSign = inputMesh.tangentOS.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord6.yzw = ase_worldBitangent;
				float3 vertexToFrag3890_g60053 = ase_worldPos;
				outputPackedVaryingsMeshToPS.ase_texcoord7.xyz = vertexToFrag3890_g60053;
				float3 localObjectPosition_UNITY_MATRIX_M14_g64862 = ObjectPosition_UNITY_MATRIX_M();
				#ifdef SHADEROPTIONS_CAMERA_RELATIVE_RENDERING
				float3 staticSwitch13_g64862 = ( localObjectPosition_UNITY_MATRIX_M14_g64862 + _WorldSpaceCameraPos );
				#else
				float3 staticSwitch13_g64862 = localObjectPosition_UNITY_MATRIX_M14_g64862;
				#endif
				half3 ObjectData20_g64863 = staticSwitch13_g64862;
				half3 WorldData19_g64863 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g64863 = WorldData19_g64863;
				#else
				float3 staticSwitch14_g64863 = ObjectData20_g64863;
				#endif
				float3 temp_output_114_0_g64862 = staticSwitch14_g64863;
				float3 vertexToFrag4224_g60053 = temp_output_114_0_g64862;
				outputPackedVaryingsMeshToPS.ase_texcoord8.xyz = vertexToFrag4224_g60053;
				
				float2 vertexToFrag11_g64849 = ( ( inputMesh.ase_texcoord.xy * (_EmissiveUVs).xy ) + (_EmissiveUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord9.xy = vertexToFrag11_g64849;
				
				float temp_output_7_0_g64876 = TVE_CameraFadeStart;
				float lerpResult4755_g60053 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g64876 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g64876 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g64880 = lerpResult4755_g60053;
				outputPackedVaryingsMeshToPS.ase_texcoord7.w = vertexToFrag11_g64880;
				
				outputPackedVaryingsMeshToPS.ase_normal = inputMesh.normalOS;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord8.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord9.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = Final_VertexPosition890_g60053;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput,
						OUTPUT_GBUFFER(outGBuffer)
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
						)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.interp03.xyzw;
				input.texCoord2 = packedInput.interp04.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);
				SurfaceData surfaceData;
				BuiltinData builtinData;

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				half Leaves_Mask4511_g60053 = 1.0;
				float3 lerpResult4521_g60053 = lerp( float3( 1,1,1 ) , ( float3(1,1,1) * float3(1,1,1) * float3(1,1,1) ) , Leaves_Mask4511_g60053);
				float2 vertexToFrag11_g64879 = packedInput.ase_texcoord5.xy;
				half2 Main_UVs15_g60053 = vertexToFrag11_g64879;
				float4 tex2DNode29_g60053 = tex2D( _MainAlbedoTex, Main_UVs15_g60053 );
				half3 Main_Albedo99_g60053 = ( (_MainColor).rgb * (tex2DNode29_g60053).rgb );
				float2 vertexToFrag11_g64813 = packedInput.ase_texcoord5.zw;
				half2 Second_UVs17_g60053 = vertexToFrag11_g64813;
				float4 tex2DNode89_g60053 = tex2D( _SecondAlbedoTex, Second_UVs17_g60053 );
				half3 Second_Albedo153_g60053 = (( _SecondColor * tex2DNode89_g60053 )).rgb;
				float3 lerpResult4834_g60053 = lerp( ( Main_Albedo99_g60053 * Second_Albedo153_g60053 * 4.594794 ) , Second_Albedo153_g60053 , _DetailBlendMode);
				float vertexToFrag11_g64796 = packedInput.ase_texcoord6.x;
				float temp_output_3919_0_g60053 = vertexToFrag11_g64796;
				float3 unpack4112_g60053 = UnpackNormalScale( tex2D( _MainNormalTex, Main_UVs15_g60053 ), _MainNormalValue );
				unpack4112_g60053.z = lerp( 1, unpack4112_g60053.z, saturate(_MainNormalValue) );
				half3 Main_Normal137_g60053 = unpack4112_g60053;
				float3 ase_worldBitangent = packedInput.ase_texcoord6.yzw;
				float3 tanToWorld0 = float3( tangentWS.xyz.x, ase_worldBitangent.x, normalWS.x );
				float3 tanToWorld1 = float3( tangentWS.xyz.y, ase_worldBitangent.y, normalWS.y );
				float3 tanToWorld2 = float3( tangentWS.xyz.z, ase_worldBitangent.z, normalWS.z );
				float3 tanNormal4099_g60053 = Main_Normal137_g60053;
				float3 worldNormal4099_g60053 = float3(dot(tanToWorld0,tanNormal4099_g60053), dot(tanToWorld1,tanNormal4099_g60053), dot(tanToWorld2,tanNormal4099_g60053));
				float3 Main_Normal_WS4101_g60053 = worldNormal4099_g60053;
				float lerpResult1537_g60053 = lerp( Main_Normal_WS4101_g60053.y , -Main_Normal_WS4101_g60053.y , _DetailProjectionMode);
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float staticSwitch3467_g60053 = temp_output_3919_0_g60053;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float staticSwitch3467_g60053 = ( ( lerpResult1537_g60053 * 0.5 ) + _DetailMeshValue );
				#else
				float staticSwitch3467_g60053 = temp_output_3919_0_g60053;
				#endif
				half Blend_Source1540_g60053 = staticSwitch3467_g60053;
				float4 tex2DNode35_g60053 = tex2D( _MainMaskTex, Main_UVs15_g60053 );
				half Main_Mask57_g60053 = tex2DNode35_g60053.b;
				float4 tex2DNode33_g60053 = tex2D( _SecondMaskTex, Second_UVs17_g60053 );
				half Second_Mask81_g60053 = tex2DNode33_g60053.b;
				float lerpResult1327_g60053 = lerp( Main_Mask57_g60053 , Second_Mask81_g60053 , _DetailMaskMode);
				float lerpResult4058_g60053 = lerp( lerpResult1327_g60053 , ( 1.0 - lerpResult1327_g60053 ) , _DetailMaskInvertMode);
				float temp_output_7_0_g64830 = _DetailBlendMinValue;
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch4838_g60053 = 0.0;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch4838_g60053 = saturate( ( ( saturate( ( Blend_Source1540_g60053 + ( Blend_Source1540_g60053 * lerpResult4058_g60053 ) ) ) - temp_output_7_0_g64830 ) / ( _DetailBlendMaxValue - temp_output_7_0_g64830 ) ) );
				#else
				float staticSwitch4838_g60053 = 0.0;
				#endif
				half Mask_Detail147_g60053 = staticSwitch4838_g60053;
				float3 lerpResult235_g60053 = lerp( Main_Albedo99_g60053 , lerpResult4834_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch255_g60053 = Main_Albedo99_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch255_g60053 = lerpResult235_g60053;
				#else
				float3 staticSwitch255_g60053 = Main_Albedo99_g60053;
				#endif
				half3 Blend_Albedo265_g60053 = staticSwitch255_g60053;
				half3 Blend_AlbedoTinted2808_g60053 = ( lerpResult4521_g60053 * Blend_Albedo265_g60053 );
				half3 Blend_AlbedoColored863_g60053 = Blend_AlbedoTinted2808_g60053;
				half3 Blend_AlbedoAndSubsurface149_g60053 = Blend_AlbedoColored863_g60053;
				half3 Global_OverlayColor1758_g60053 = (TVE_OverlayColor).rgb;
				half VertexDynamicMode4798_g60053 = _VertexDynamicMode;
				float lerpResult4801_g60053 = lerp( Main_Normal_WS4101_g60053.y , packedInput.ase_normal.y , VertexDynamicMode4798_g60053);
				float lerpResult3567_g60053 = lerp( 0.5 , 1.0 , lerpResult4801_g60053);
				half Main_AlbedoTex_G3526_g60053 = tex2DNode29_g60053.g;
				half Second_AlbedoTex_G3581_g60053 = tex2DNode89_g60053.g;
				float lerpResult3579_g60053 = lerp( Main_AlbedoTex_G3526_g60053 , Second_AlbedoTex_G3581_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch3574_g60053 = Main_AlbedoTex_G3526_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch3574_g60053 = lerpResult3579_g60053;
				#else
				float staticSwitch3574_g60053 = Main_AlbedoTex_G3526_g60053;
				#endif
				float4 temp_output_93_19_g64789 = TVE_ExtrasCoords;
				float3 vertexToFrag3890_g60053 = packedInput.ase_texcoord7.xyz;
				float3 WorldPosition3905_g60053 = vertexToFrag3890_g60053;
				float3 vertexToFrag4224_g60053 = packedInput.ase_texcoord8.xyz;
				half3 ObjectData20_g64855 = vertexToFrag4224_g60053;
				half3 WorldData19_g64855 = vertexToFrag3890_g60053;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g64855 = WorldData19_g64855;
				#else
				float3 staticSwitch14_g64855 = ObjectData20_g64855;
				#endif
				float3 ObjectPosition4223_g60053 = staticSwitch14_g64855;
				float3 lerpResult4827_g60053 = lerp( WorldPosition3905_g60053 , ObjectPosition4223_g60053 , _ExtrasPositionMode);
				float3 Position82_g64789 = lerpResult4827_g60053;
				float temp_output_84_0_g64789 = _LayerExtrasValue;
				float4 lerpResult88_g64789 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g64789).zw + ( (temp_output_93_19_g64789).xy * (Position82_g64789).xz ) ),temp_output_84_0_g64789 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g64789]);
				float4 break89_g64789 = lerpResult88_g64789;
				half Global_Extras_Overlay156_g60053 = break89_g64789.b;
				half Overlay_Variation4560_g60053 = 1.0;
				half Overlay_Commons1365_g60053 = ( _GlobalOverlay * Global_Extras_Overlay156_g60053 * Overlay_Variation4560_g60053 );
				float temp_output_7_0_g64869 = _OverlayMaskMinValue;
				half Overlay_Mask269_g60053 = saturate( ( ( ( ( ( lerpResult3567_g60053 * 0.5 ) + staticSwitch3574_g60053 ) * Overlay_Commons1365_g60053 ) - temp_output_7_0_g64869 ) / ( _OverlayMaskMaxValue - temp_output_7_0_g64869 ) ) );
				float3 lerpResult336_g60053 = lerp( Blend_AlbedoAndSubsurface149_g60053 , Global_OverlayColor1758_g60053 , Overlay_Mask269_g60053);
				half3 Final_Albedo359_g60053 = lerpResult336_g60053;
				
				float3 lerpResult3372_g60053 = lerp( float3( 0,0,1 ) , Main_Normal137_g60053 , _DetailNormalValue);
				float3 unpack4114_g60053 = UnpackNormalScale( tex2D( _SecondNormalTex, Second_UVs17_g60053 ), _SecondNormalValue );
				unpack4114_g60053.z = lerp( 1, unpack4114_g60053.z, saturate(_SecondNormalValue) );
				half3 Second_Normal179_g60053 = unpack4114_g60053;
				float3 lerpResult3376_g60053 = lerp( Main_Normal137_g60053 , BlendNormal( lerpResult3372_g60053 , Second_Normal179_g60053 ) , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch267_g60053 = Main_Normal137_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch267_g60053 = lerpResult3376_g60053;
				#else
				float3 staticSwitch267_g60053 = Main_Normal137_g60053;
				#endif
				float3 temp_output_13_0_g64811 = staticSwitch267_g60053;
				float3 switchResult12_g64811 = (((isFrontFace>0)?(temp_output_13_0_g64811):(( temp_output_13_0_g64811 * _render_normals_options ))));
				half3 Blend_Normal312_g60053 = switchResult12_g64811;
				half3 Final_Normal366_g60053 = Blend_Normal312_g60053;
				
				half Main_Metallic237_g60053 = ( tex2DNode35_g60053.r * _MainMetallicValue );
				half Second_Metallic226_g60053 = ( tex2DNode33_g60053.r * _SecondMetallicValue );
				float lerpResult278_g60053 = lerp( Main_Metallic237_g60053 , Second_Metallic226_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch299_g60053 = Main_Metallic237_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch299_g60053 = lerpResult278_g60053;
				#else
				float staticSwitch299_g60053 = Main_Metallic237_g60053;
				#endif
				half Blend_Metallic306_g60053 = staticSwitch299_g60053;
				float lerpResult342_g60053 = lerp( Blend_Metallic306_g60053 , 0.0 , Overlay_Mask269_g60053);
				half Final_Metallic367_g60053 = lerpResult342_g60053;
				
				float3 hdEmission4189_g60053 = ASEGetEmissionHDRColor(_EmissiveColor.rgb,_EmissiveIntensityParams.x,_EmissiveExposureValue,GetInverseCurrentExposureMultiplier());
				float2 vertexToFrag11_g64849 = packedInput.ase_texcoord9.xy;
				half2 Emissive_UVs2468_g60053 = vertexToFrag11_g64849;
				half Global_Extras_Emissive4203_g60053 = break89_g64789.r;
				float lerpResult4206_g60053 = lerp( 1.0 , Global_Extras_Emissive4203_g60053 , _GlobalEmissive);
				half3 Final_Emissive2476_g60053 = ( (( hdEmission4189_g60053 * tex2D( _EmissiveTex, Emissive_UVs2468_g60053 ) )).rgb * lerpResult4206_g60053 );
				
				half Main_Smoothness227_g60053 = ( tex2DNode35_g60053.a * _MainSmoothnessValue );
				half Second_Smoothness236_g60053 = ( tex2DNode33_g60053.a * _SecondSmoothnessValue );
				float lerpResult266_g60053 = lerp( Main_Smoothness227_g60053 , Second_Smoothness236_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch297_g60053 = Main_Smoothness227_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch297_g60053 = lerpResult266_g60053;
				#else
				float staticSwitch297_g60053 = Main_Smoothness227_g60053;
				#endif
				half Blend_Smoothness314_g60053 = staticSwitch297_g60053;
				half Global_OverlaySmoothness311_g60053 = TVE_OverlaySmoothness;
				float lerpResult343_g60053 = lerp( Blend_Smoothness314_g60053 , Global_OverlaySmoothness311_g60053 , Overlay_Mask269_g60053);
				half Final_Smoothness371_g60053 = lerpResult343_g60053;
				half Global_Extras_Wetness305_g60053 = break89_g64789.g;
				float lerpResult3673_g60053 = lerp( 0.0 , Global_Extras_Wetness305_g60053 , _GlobalWetness);
				half Final_SmoothnessAndWetness4130_g60053 = saturate( ( Final_Smoothness371_g60053 + lerpResult3673_g60053 ) );
				
				float lerpResult240_g60053 = lerp( 1.0 , tex2DNode35_g60053.g , _MainOcclusionValue);
				half Main_Occlusion247_g60053 = lerpResult240_g60053;
				float lerpResult239_g60053 = lerp( 1.0 , tex2DNode33_g60053.g , _SecondOcclusionValue);
				half Second_Occlusion251_g60053 = lerpResult239_g60053;
				float lerpResult294_g60053 = lerp( Main_Occlusion247_g60053 , Second_Occlusion251_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch310_g60053 = Main_Occlusion247_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch310_g60053 = lerpResult294_g60053;
				#else
				float staticSwitch310_g60053 = Main_Occlusion247_g60053;
				#endif
				half Blend_Occlusion323_g60053 = staticSwitch310_g60053;
				
				float localCustomAlphaClip9_g64873 = ( 0.0 );
				float vertexToFrag11_g64880 = packedInput.ase_texcoord7.w;
				half Fade_Camera3743_g60053 = vertexToFrag11_g64880;
				float temp_output_41_0_g64856 = ( 1.0 * Fade_Camera3743_g60053 );
				half Final_AlphaFade3727_g60053 = saturate( ( temp_output_41_0_g64856 + ( temp_output_41_0_g64856 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g60053 ) ).r ) ) );
				float Main_Alpha316_g60053 = ( _MainColor.a * tex2DNode29_g60053.a );
				half AlphaTreshold2132_g60053 = _AlphaCutoffValue;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g60053 = ( Main_Alpha316_g60053 - ( AlphaTreshold2132_g60053 - 0.5 ) );
				#else
				float staticSwitch3792_g60053 = Main_Alpha316_g60053;
				#endif
				half Final_Alpha3754_g60053 = staticSwitch3792_g60053;
				float temp_output_661_0_g60053 = ( Final_AlphaFade3727_g60053 * Final_Alpha3754_g60053 );
				float temp_output_3_0_g64817 = temp_output_661_0_g60053;
				half Offest27_g64817 = 0.5;
				half Feather30_g64817 = 0.5;
				float temp_output_3_0_g64873 = ( ( ( temp_output_3_0_g64817 - Offest27_g64817 ) / ( max( fwidth( temp_output_3_0_g64817 ) , 0.001 ) + Feather30_g64817 ) ) + Offest27_g64817 );
				float Alpha9_g64873 = temp_output_3_0_g64873;
				float temp_output_15_0_g64873 = 0.5;
				float Treshold9_g64873 = temp_output_15_0_g64873;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha9_g64873 - Treshold9_g64873);
				#endif
				}
				half Final_Clip914_g60053 = Alpha9_g64873;
				
				surfaceDescription.Albedo = Final_Albedo359_g60053;
				surfaceDescription.Normal = Final_Normal366_g60053;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = Final_Metallic367_g60053;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = Final_Emissive2476_g60053;
				surfaceDescription.Smoothness = Final_SmoothnessAndWetness4130_g60053;
				surfaceDescription.Occlusion = Blend_Occlusion323_g60053;
				surfaceDescription.Alpha = Final_Clip914_g60053;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				surfaceDescription.AlphaClipThresholdDepthPrepass = 0.5;
				surfaceDescription.AlphaClipThresholdDepthPostpass = 0.5;

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef _ASE_DISTORTION
				surfaceDescription.Distortion = float2 ( 2, -1 );
				surfaceDescription.DistortionBlur = 1;
				#endif

				#ifdef _ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );
				ENCODE_INTO_GBUFFER( surfaceData, builtinData, posInput.positionSS, outGBuffer );
				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "META"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM

			#define ASE_NEED_CULLFACE 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 100202
			#if !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif //ASE_NEED_CULLFACE


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			//#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			//#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			
			CBUFFER_START( UnityPerMaterial )
			float4 _MaxBoundsInfo;
			half4 _VertexOcclusionRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _SecondColor;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _MainUVs;
			half4 _MainColor;
			half4 _DetailBlendRemap;
			float4 _NoiseMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			half4 _AlphaMaskRemap;
			half4 _ColorsMaskRemap;
			float4 _GradientMaskRemap;
			half4 _SecondUVs;
			half4 _OverlayMaskRemap;
			float4 _EmissiveIntensityParams;
			half4 _EmissiveUVs;
			float4 _Color;
			half4 _EmissiveColor;
			half4 _LeavesMaskRemap;
			half3 _render_normals_options;
			half _SpaceMotionHighlight;
			half _SizeFadeCat;
			half _DetailTypeMode;
			half _SpaceMotionLocals;
			half _render_src;
			half _render_zw;
			half _MessageGlobalsVariation;
			half _AlphaCutoffValue;
			half _DetailCoordMode;
			half _SubsurfaceNormalValue;
			half _IsStandardShader;
			half _SpaceGlobalLayers;
			half _DetailMaskInvertMode;
			half _MainNormalValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _SecondSmoothnessValue;
			half _MainSmoothnessValue;
			half _GlobalEmissive;
			half _EmissiveExposureValue;
			half _SecondMetallicValue;
			half _MainMetallicValue;
			half _SecondNormalValue;
			half _DetailNormalValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _LayerExtrasValue;
			half _ExtrasPositionMode;
			half _GlobalOverlay;
			half _VertexDynamicMode;
			half _DetailBlendMaxValue;
			half _DetailBlendMinValue;
			half _SubsurfaceScatteringValue;
			half _DetailMaskMode;
			half _DetailProjectionMode;
			half _DetailMeshValue;
			half _VertexRollingMode;
			half _render_cull;
			half _DetailCat;
			half _RenderClip;
			half _SubsurfaceShadowValue;
			float _SubsurfaceDiffusion;
			half _SpaceRenderFade;
			half _VertexVariationMode;
			half _MainCat;
			half _HasGradient;
			half _RenderPriority;
			half _RenderQueue;
			half _SubsurfaceAmbientValue;
			half _HasEmissive;
			half _RenderMode;
			half _RenderZWrite;
			half _DetailMode;
			half _DetailBlendMode;
			half _SubsurfaceCat;
			half _EmissiveCat;
			half _IsPropShader;
			half _render_dst;
			half _VertexMasksMode;
			half _GradientCat;
			half _MotionCat;
			half _RenderCull;
			half _IsVersion;
			half _NoiseCat;
			half _SecondOcclusionValue;
			half _RenderNormals;
			half _LayerReactValue;
			half _RenderDecals;
			half _SpaceGlobalPosition;
			half _EmissiveFlagMode;
			half _HasOcclusion;
			half _Cutoff;
			half _GlobalCat;
			half _MessageMotionVariation;
			half _MessageSizeFade;
			half _OcclusionCat;
			half _RenderSSR;
			half _PerspectiveCat;
			half _SubsurfaceDirectValue;
			half _SubsurfaceAngleValue;
			half _RenderingCat;
			half _IsTVEShader;
			half _RenderCoverage;
			half _FadeCameraValue;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half TVE_Enabled;
			half _DisableSRPBatcher;
			sampler2D _MainAlbedoTex;
			sampler2D _SecondAlbedoTex;
			sampler2D _MainNormalTex;
			sampler2D _MainMaskTex;
			sampler2D _SecondMaskTex;
			half4 TVE_OverlayColor;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];
			sampler2D _SecondNormalTex;
			sampler2D _EmissiveTex;
			half TVE_OverlaySmoothness;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_VFACE
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_DETAIL_MODE_OFF TVE_DETAIL_MODE_ON
			#pragma shader_feature_local TVE_DETAIL_TYPE_VERTEX_BLUE TVE_DETAIL_TYPE_PROJECTION
			//TVE Shader Type Defines
			#define TVE_IS_OBJECT_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_HD_PIPELINE
			//TVE HD Pipeline Defines
			#pragma shader_feature_local _DISABLE_DECALS
			#pragma shader_feature_local _DISABLE_SSR


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			float3 ObjectPosition_UNITY_MATRIX_M(  )
			{
				return float3(UNITY_MATRIX_M[0].w, UNITY_MATRIX_M[1].w, UNITY_MATRIX_M[2].w );
			}
			
			float3 ASEGetEmissionHDRColor(float3 ldrColor, float luminanceIntensity, float exposureWeight, float inverseCurrentExposureMultiplier)
			{
				float3 hdrColor = ldrColor * luminanceIntensity;
				hdrColor = lerp( hdrColor* inverseCurrentExposureMultiplier, hdrColor, exposureWeight);
				return hdrColor;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				bentNormalWS = surfaceData.normalWS;
				
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );


				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if (SHADERPASS == SHADERPASS_DISTORTION)
				builtinData.distortion = surfaceDescription.Distortion;
				builtinData.distortionBlur = surfaceDescription.DistortionBlur;
				#else
				builtinData.distortion = float2(0.0, 0.0);
				builtinData.distortionBlur = 0.0;
				#endif

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			CBUFFER_START(UnityMetaPass)
			bool4 unity_MetaVertexControl;
			bool4 unity_MetaFragmentControl;
			CBUFFER_END

			float unity_OneOverOutputBoost;
			float unity_MaxOutputValue;

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh  )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				float3 VertexPosition3588_g60053 = inputMesh.positionOS;
				float3 Final_VertexPosition890_g60053 = ( VertexPosition3588_g60053 + _DisableSRPBatcher );
				
				float2 vertexToFrag11_g64879 = ( ( inputMesh.uv0.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord.xy = vertexToFrag11_g64879;
				float2 lerpResult1545_g60053 = lerp( inputMesh.uv0.xy , inputMesh.uv1.xy , _DetailCoordMode);
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float2 staticSwitch3466_g60053 = lerpResult1545_g60053;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float2 staticSwitch3466_g60053 = (ase_worldPos).xz;
				#else
				float2 staticSwitch3466_g60053 = lerpResult1545_g60053;
				#endif
				float2 vertexToFrag11_g64813 = ( ( staticSwitch3466_g60053 * (_SecondUVs).xy ) + (_SecondUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord.zw = vertexToFrag11_g64813;
				half Mesh_DetailMask90_g60053 = inputMesh.ase_color.b;
				float vertexToFrag11_g64796 = ( ( Mesh_DetailMask90_g60053 - 0.5 ) + _DetailMeshValue );
				outputPackedVaryingsMeshToPS.ase_texcoord1.x = vertexToFrag11_g64796;
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				outputPackedVaryingsMeshToPS.ase_texcoord1.yzw = ase_worldTangent;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				outputPackedVaryingsMeshToPS.ase_texcoord2.xyz = ase_worldNormal;
				float ase_vertexTangentSign = inputMesh.tangentOS.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord3.xyz = ase_worldBitangent;
				float3 vertexToFrag3890_g60053 = ase_worldPos;
				outputPackedVaryingsMeshToPS.ase_texcoord4.xyz = vertexToFrag3890_g60053;
				float3 localObjectPosition_UNITY_MATRIX_M14_g64862 = ObjectPosition_UNITY_MATRIX_M();
				#ifdef SHADEROPTIONS_CAMERA_RELATIVE_RENDERING
				float3 staticSwitch13_g64862 = ( localObjectPosition_UNITY_MATRIX_M14_g64862 + _WorldSpaceCameraPos );
				#else
				float3 staticSwitch13_g64862 = localObjectPosition_UNITY_MATRIX_M14_g64862;
				#endif
				half3 ObjectData20_g64863 = staticSwitch13_g64862;
				half3 WorldData19_g64863 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g64863 = WorldData19_g64863;
				#else
				float3 staticSwitch14_g64863 = ObjectData20_g64863;
				#endif
				float3 temp_output_114_0_g64862 = staticSwitch14_g64863;
				float3 vertexToFrag4224_g60053 = temp_output_114_0_g64862;
				outputPackedVaryingsMeshToPS.ase_texcoord5.xyz = vertexToFrag4224_g60053;
				
				float2 vertexToFrag11_g64849 = ( ( inputMesh.uv0.xy * (_EmissiveUVs).xy ) + (_EmissiveUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord6.xy = vertexToFrag11_g64849;
				
				float temp_output_7_0_g64876 = TVE_CameraFadeStart;
				float lerpResult4755_g60053 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g64876 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g64876 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g64880 = lerpResult4755_g60053;
				outputPackedVaryingsMeshToPS.ase_texcoord2.w = vertexToFrag11_g64880;
				
				outputPackedVaryingsMeshToPS.ase_normal = inputMesh.normalOS;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord3.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord4.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord5.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord6.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = Final_VertexPosition890_g60053;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float2 uv = float2(0.0, 0.0);
				if (unity_MetaVertexControl.x)
				{
					uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				}
				else if (unity_MetaVertexControl.y)
				{
					uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				}

				outputPackedVaryingsMeshToPS.positionCS = float4(uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0);
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv0 = patch[0].uv0 * bary.x + patch[1].uv0 * bary.y + patch[2].uv0 * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag(PackedVaryingsMeshToPS packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				half Leaves_Mask4511_g60053 = 1.0;
				float3 lerpResult4521_g60053 = lerp( float3( 1,1,1 ) , ( float3(1,1,1) * float3(1,1,1) * float3(1,1,1) ) , Leaves_Mask4511_g60053);
				float2 vertexToFrag11_g64879 = packedInput.ase_texcoord.xy;
				half2 Main_UVs15_g60053 = vertexToFrag11_g64879;
				float4 tex2DNode29_g60053 = tex2D( _MainAlbedoTex, Main_UVs15_g60053 );
				half3 Main_Albedo99_g60053 = ( (_MainColor).rgb * (tex2DNode29_g60053).rgb );
				float2 vertexToFrag11_g64813 = packedInput.ase_texcoord.zw;
				half2 Second_UVs17_g60053 = vertexToFrag11_g64813;
				float4 tex2DNode89_g60053 = tex2D( _SecondAlbedoTex, Second_UVs17_g60053 );
				half3 Second_Albedo153_g60053 = (( _SecondColor * tex2DNode89_g60053 )).rgb;
				float3 lerpResult4834_g60053 = lerp( ( Main_Albedo99_g60053 * Second_Albedo153_g60053 * 4.594794 ) , Second_Albedo153_g60053 , _DetailBlendMode);
				float vertexToFrag11_g64796 = packedInput.ase_texcoord1.x;
				float temp_output_3919_0_g60053 = vertexToFrag11_g64796;
				float3 unpack4112_g60053 = UnpackNormalScale( tex2D( _MainNormalTex, Main_UVs15_g60053 ), _MainNormalValue );
				unpack4112_g60053.z = lerp( 1, unpack4112_g60053.z, saturate(_MainNormalValue) );
				half3 Main_Normal137_g60053 = unpack4112_g60053;
				float3 ase_worldTangent = packedInput.ase_texcoord1.yzw;
				float3 ase_worldNormal = packedInput.ase_texcoord2.xyz;
				float3 ase_worldBitangent = packedInput.ase_texcoord3.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal4099_g60053 = Main_Normal137_g60053;
				float3 worldNormal4099_g60053 = float3(dot(tanToWorld0,tanNormal4099_g60053), dot(tanToWorld1,tanNormal4099_g60053), dot(tanToWorld2,tanNormal4099_g60053));
				float3 Main_Normal_WS4101_g60053 = worldNormal4099_g60053;
				float lerpResult1537_g60053 = lerp( Main_Normal_WS4101_g60053.y , -Main_Normal_WS4101_g60053.y , _DetailProjectionMode);
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float staticSwitch3467_g60053 = temp_output_3919_0_g60053;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float staticSwitch3467_g60053 = ( ( lerpResult1537_g60053 * 0.5 ) + _DetailMeshValue );
				#else
				float staticSwitch3467_g60053 = temp_output_3919_0_g60053;
				#endif
				half Blend_Source1540_g60053 = staticSwitch3467_g60053;
				float4 tex2DNode35_g60053 = tex2D( _MainMaskTex, Main_UVs15_g60053 );
				half Main_Mask57_g60053 = tex2DNode35_g60053.b;
				float4 tex2DNode33_g60053 = tex2D( _SecondMaskTex, Second_UVs17_g60053 );
				half Second_Mask81_g60053 = tex2DNode33_g60053.b;
				float lerpResult1327_g60053 = lerp( Main_Mask57_g60053 , Second_Mask81_g60053 , _DetailMaskMode);
				float lerpResult4058_g60053 = lerp( lerpResult1327_g60053 , ( 1.0 - lerpResult1327_g60053 ) , _DetailMaskInvertMode);
				float temp_output_7_0_g64830 = _DetailBlendMinValue;
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch4838_g60053 = 0.0;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch4838_g60053 = saturate( ( ( saturate( ( Blend_Source1540_g60053 + ( Blend_Source1540_g60053 * lerpResult4058_g60053 ) ) ) - temp_output_7_0_g64830 ) / ( _DetailBlendMaxValue - temp_output_7_0_g64830 ) ) );
				#else
				float staticSwitch4838_g60053 = 0.0;
				#endif
				half Mask_Detail147_g60053 = staticSwitch4838_g60053;
				float3 lerpResult235_g60053 = lerp( Main_Albedo99_g60053 , lerpResult4834_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch255_g60053 = Main_Albedo99_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch255_g60053 = lerpResult235_g60053;
				#else
				float3 staticSwitch255_g60053 = Main_Albedo99_g60053;
				#endif
				half3 Blend_Albedo265_g60053 = staticSwitch255_g60053;
				half3 Blend_AlbedoTinted2808_g60053 = ( lerpResult4521_g60053 * Blend_Albedo265_g60053 );
				half3 Blend_AlbedoColored863_g60053 = Blend_AlbedoTinted2808_g60053;
				half3 Blend_AlbedoAndSubsurface149_g60053 = Blend_AlbedoColored863_g60053;
				half3 Global_OverlayColor1758_g60053 = (TVE_OverlayColor).rgb;
				half VertexDynamicMode4798_g60053 = _VertexDynamicMode;
				float lerpResult4801_g60053 = lerp( Main_Normal_WS4101_g60053.y , packedInput.ase_normal.y , VertexDynamicMode4798_g60053);
				float lerpResult3567_g60053 = lerp( 0.5 , 1.0 , lerpResult4801_g60053);
				half Main_AlbedoTex_G3526_g60053 = tex2DNode29_g60053.g;
				half Second_AlbedoTex_G3581_g60053 = tex2DNode89_g60053.g;
				float lerpResult3579_g60053 = lerp( Main_AlbedoTex_G3526_g60053 , Second_AlbedoTex_G3581_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch3574_g60053 = Main_AlbedoTex_G3526_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch3574_g60053 = lerpResult3579_g60053;
				#else
				float staticSwitch3574_g60053 = Main_AlbedoTex_G3526_g60053;
				#endif
				float4 temp_output_93_19_g64789 = TVE_ExtrasCoords;
				float3 vertexToFrag3890_g60053 = packedInput.ase_texcoord4.xyz;
				float3 WorldPosition3905_g60053 = vertexToFrag3890_g60053;
				float3 vertexToFrag4224_g60053 = packedInput.ase_texcoord5.xyz;
				half3 ObjectData20_g64855 = vertexToFrag4224_g60053;
				half3 WorldData19_g64855 = vertexToFrag3890_g60053;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g64855 = WorldData19_g64855;
				#else
				float3 staticSwitch14_g64855 = ObjectData20_g64855;
				#endif
				float3 ObjectPosition4223_g60053 = staticSwitch14_g64855;
				float3 lerpResult4827_g60053 = lerp( WorldPosition3905_g60053 , ObjectPosition4223_g60053 , _ExtrasPositionMode);
				float3 Position82_g64789 = lerpResult4827_g60053;
				float temp_output_84_0_g64789 = _LayerExtrasValue;
				float4 lerpResult88_g64789 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g64789).zw + ( (temp_output_93_19_g64789).xy * (Position82_g64789).xz ) ),temp_output_84_0_g64789 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g64789]);
				float4 break89_g64789 = lerpResult88_g64789;
				half Global_Extras_Overlay156_g60053 = break89_g64789.b;
				half Overlay_Variation4560_g60053 = 1.0;
				half Overlay_Commons1365_g60053 = ( _GlobalOverlay * Global_Extras_Overlay156_g60053 * Overlay_Variation4560_g60053 );
				float temp_output_7_0_g64869 = _OverlayMaskMinValue;
				half Overlay_Mask269_g60053 = saturate( ( ( ( ( ( lerpResult3567_g60053 * 0.5 ) + staticSwitch3574_g60053 ) * Overlay_Commons1365_g60053 ) - temp_output_7_0_g64869 ) / ( _OverlayMaskMaxValue - temp_output_7_0_g64869 ) ) );
				float3 lerpResult336_g60053 = lerp( Blend_AlbedoAndSubsurface149_g60053 , Global_OverlayColor1758_g60053 , Overlay_Mask269_g60053);
				half3 Final_Albedo359_g60053 = lerpResult336_g60053;
				
				float3 lerpResult3372_g60053 = lerp( float3( 0,0,1 ) , Main_Normal137_g60053 , _DetailNormalValue);
				float3 unpack4114_g60053 = UnpackNormalScale( tex2D( _SecondNormalTex, Second_UVs17_g60053 ), _SecondNormalValue );
				unpack4114_g60053.z = lerp( 1, unpack4114_g60053.z, saturate(_SecondNormalValue) );
				half3 Second_Normal179_g60053 = unpack4114_g60053;
				float3 lerpResult3376_g60053 = lerp( Main_Normal137_g60053 , BlendNormal( lerpResult3372_g60053 , Second_Normal179_g60053 ) , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch267_g60053 = Main_Normal137_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch267_g60053 = lerpResult3376_g60053;
				#else
				float3 staticSwitch267_g60053 = Main_Normal137_g60053;
				#endif
				float3 temp_output_13_0_g64811 = staticSwitch267_g60053;
				float3 switchResult12_g64811 = (((isFrontFace>0)?(temp_output_13_0_g64811):(( temp_output_13_0_g64811 * _render_normals_options ))));
				half3 Blend_Normal312_g60053 = switchResult12_g64811;
				half3 Final_Normal366_g60053 = Blend_Normal312_g60053;
				
				half Main_Metallic237_g60053 = ( tex2DNode35_g60053.r * _MainMetallicValue );
				half Second_Metallic226_g60053 = ( tex2DNode33_g60053.r * _SecondMetallicValue );
				float lerpResult278_g60053 = lerp( Main_Metallic237_g60053 , Second_Metallic226_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch299_g60053 = Main_Metallic237_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch299_g60053 = lerpResult278_g60053;
				#else
				float staticSwitch299_g60053 = Main_Metallic237_g60053;
				#endif
				half Blend_Metallic306_g60053 = staticSwitch299_g60053;
				float lerpResult342_g60053 = lerp( Blend_Metallic306_g60053 , 0.0 , Overlay_Mask269_g60053);
				half Final_Metallic367_g60053 = lerpResult342_g60053;
				
				float3 hdEmission4189_g60053 = ASEGetEmissionHDRColor(_EmissiveColor.rgb,_EmissiveIntensityParams.x,_EmissiveExposureValue,GetInverseCurrentExposureMultiplier());
				float2 vertexToFrag11_g64849 = packedInput.ase_texcoord6.xy;
				half2 Emissive_UVs2468_g60053 = vertexToFrag11_g64849;
				half Global_Extras_Emissive4203_g60053 = break89_g64789.r;
				float lerpResult4206_g60053 = lerp( 1.0 , Global_Extras_Emissive4203_g60053 , _GlobalEmissive);
				half3 Final_Emissive2476_g60053 = ( (( hdEmission4189_g60053 * tex2D( _EmissiveTex, Emissive_UVs2468_g60053 ) )).rgb * lerpResult4206_g60053 );
				
				half Main_Smoothness227_g60053 = ( tex2DNode35_g60053.a * _MainSmoothnessValue );
				half Second_Smoothness236_g60053 = ( tex2DNode33_g60053.a * _SecondSmoothnessValue );
				float lerpResult266_g60053 = lerp( Main_Smoothness227_g60053 , Second_Smoothness236_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch297_g60053 = Main_Smoothness227_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch297_g60053 = lerpResult266_g60053;
				#else
				float staticSwitch297_g60053 = Main_Smoothness227_g60053;
				#endif
				half Blend_Smoothness314_g60053 = staticSwitch297_g60053;
				half Global_OverlaySmoothness311_g60053 = TVE_OverlaySmoothness;
				float lerpResult343_g60053 = lerp( Blend_Smoothness314_g60053 , Global_OverlaySmoothness311_g60053 , Overlay_Mask269_g60053);
				half Final_Smoothness371_g60053 = lerpResult343_g60053;
				half Global_Extras_Wetness305_g60053 = break89_g64789.g;
				float lerpResult3673_g60053 = lerp( 0.0 , Global_Extras_Wetness305_g60053 , _GlobalWetness);
				half Final_SmoothnessAndWetness4130_g60053 = saturate( ( Final_Smoothness371_g60053 + lerpResult3673_g60053 ) );
				
				float lerpResult240_g60053 = lerp( 1.0 , tex2DNode35_g60053.g , _MainOcclusionValue);
				half Main_Occlusion247_g60053 = lerpResult240_g60053;
				float lerpResult239_g60053 = lerp( 1.0 , tex2DNode33_g60053.g , _SecondOcclusionValue);
				half Second_Occlusion251_g60053 = lerpResult239_g60053;
				float lerpResult294_g60053 = lerp( Main_Occlusion247_g60053 , Second_Occlusion251_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch310_g60053 = Main_Occlusion247_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch310_g60053 = lerpResult294_g60053;
				#else
				float staticSwitch310_g60053 = Main_Occlusion247_g60053;
				#endif
				half Blend_Occlusion323_g60053 = staticSwitch310_g60053;
				
				float localCustomAlphaClip9_g64873 = ( 0.0 );
				float vertexToFrag11_g64880 = packedInput.ase_texcoord2.w;
				half Fade_Camera3743_g60053 = vertexToFrag11_g64880;
				float temp_output_41_0_g64856 = ( 1.0 * Fade_Camera3743_g60053 );
				half Final_AlphaFade3727_g60053 = saturate( ( temp_output_41_0_g64856 + ( temp_output_41_0_g64856 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g60053 ) ).r ) ) );
				float Main_Alpha316_g60053 = ( _MainColor.a * tex2DNode29_g60053.a );
				half AlphaTreshold2132_g60053 = _AlphaCutoffValue;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g60053 = ( Main_Alpha316_g60053 - ( AlphaTreshold2132_g60053 - 0.5 ) );
				#else
				float staticSwitch3792_g60053 = Main_Alpha316_g60053;
				#endif
				half Final_Alpha3754_g60053 = staticSwitch3792_g60053;
				float temp_output_661_0_g60053 = ( Final_AlphaFade3727_g60053 * Final_Alpha3754_g60053 );
				float temp_output_3_0_g64817 = temp_output_661_0_g60053;
				half Offest27_g64817 = 0.5;
				half Feather30_g64817 = 0.5;
				float temp_output_3_0_g64873 = ( ( ( temp_output_3_0_g64817 - Offest27_g64817 ) / ( max( fwidth( temp_output_3_0_g64817 ) , 0.001 ) + Feather30_g64817 ) ) + Offest27_g64817 );
				float Alpha9_g64873 = temp_output_3_0_g64873;
				float temp_output_15_0_g64873 = 0.5;
				float Treshold9_g64873 = temp_output_15_0_g64873;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha9_g64873 - Treshold9_g64873);
				#endif
				}
				half Final_Clip914_g60053 = Alpha9_g64873;
				
				surfaceDescription.Albedo = Final_Albedo359_g60053;
				surfaceDescription.Normal = Final_Normal366_g60053;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = Final_Metallic367_g60053;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = Final_Emissive2476_g60053;
				surfaceDescription.Smoothness = Final_SmoothnessAndWetness4130_g60053;
				surfaceDescription.Occlusion = Blend_Occlusion323_g60053;
				surfaceDescription.Alpha = Final_Clip914_g60053;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);
				LightTransportData lightTransportData = GetLightTransportData(surfaceData, builtinData, bsdfData);

				float4 res = float4(0.0, 0.0, 0.0, 1.0);
				if (unity_MetaFragmentControl.x)
				{
					res.rgb = clamp(pow(abs(lightTransportData.diffuseColor), saturate(unity_OneOverOutputBoost)), 0, unity_MaxOutputValue);
				}

				if (unity_MetaFragmentControl.y)
				{
					res.rgb = lightTransportData.emissiveColor;
				}

				return res;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
			ZTest LEqual
			ColorMask 0

			HLSLPROGRAM

			#define ASE_NEED_CULLFACE 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			//#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			//#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_SHADOWS

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			//#define USE_LEGACY_UNITY_MATRIX_VARIABLES

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaxBoundsInfo;
			half4 _VertexOcclusionRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _SecondColor;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _MainUVs;
			half4 _MainColor;
			half4 _DetailBlendRemap;
			float4 _NoiseMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			half4 _AlphaMaskRemap;
			half4 _ColorsMaskRemap;
			float4 _GradientMaskRemap;
			half4 _SecondUVs;
			half4 _OverlayMaskRemap;
			float4 _EmissiveIntensityParams;
			half4 _EmissiveUVs;
			float4 _Color;
			half4 _EmissiveColor;
			half4 _LeavesMaskRemap;
			half3 _render_normals_options;
			half _SpaceMotionHighlight;
			half _SizeFadeCat;
			half _DetailTypeMode;
			half _SpaceMotionLocals;
			half _render_src;
			half _render_zw;
			half _MessageGlobalsVariation;
			half _AlphaCutoffValue;
			half _DetailCoordMode;
			half _SubsurfaceNormalValue;
			half _IsStandardShader;
			half _SpaceGlobalLayers;
			half _DetailMaskInvertMode;
			half _MainNormalValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _SecondSmoothnessValue;
			half _MainSmoothnessValue;
			half _GlobalEmissive;
			half _EmissiveExposureValue;
			half _SecondMetallicValue;
			half _MainMetallicValue;
			half _SecondNormalValue;
			half _DetailNormalValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _LayerExtrasValue;
			half _ExtrasPositionMode;
			half _GlobalOverlay;
			half _VertexDynamicMode;
			half _DetailBlendMaxValue;
			half _DetailBlendMinValue;
			half _SubsurfaceScatteringValue;
			half _DetailMaskMode;
			half _DetailProjectionMode;
			half _DetailMeshValue;
			half _VertexRollingMode;
			half _render_cull;
			half _DetailCat;
			half _RenderClip;
			half _SubsurfaceShadowValue;
			float _SubsurfaceDiffusion;
			half _SpaceRenderFade;
			half _VertexVariationMode;
			half _MainCat;
			half _HasGradient;
			half _RenderPriority;
			half _RenderQueue;
			half _SubsurfaceAmbientValue;
			half _HasEmissive;
			half _RenderMode;
			half _RenderZWrite;
			half _DetailMode;
			half _DetailBlendMode;
			half _SubsurfaceCat;
			half _EmissiveCat;
			half _IsPropShader;
			half _render_dst;
			half _VertexMasksMode;
			half _GradientCat;
			half _MotionCat;
			half _RenderCull;
			half _IsVersion;
			half _NoiseCat;
			half _SecondOcclusionValue;
			half _RenderNormals;
			half _LayerReactValue;
			half _RenderDecals;
			half _SpaceGlobalPosition;
			half _EmissiveFlagMode;
			half _HasOcclusion;
			half _Cutoff;
			half _GlobalCat;
			half _MessageMotionVariation;
			half _MessageSizeFade;
			half _OcclusionCat;
			half _RenderSSR;
			half _PerspectiveCat;
			half _SubsurfaceDirectValue;
			half _SubsurfaceAngleValue;
			half _RenderingCat;
			half _IsTVEShader;
			half _RenderCoverage;
			half _FadeCameraValue;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half TVE_Enabled;
			half _DisableSRPBatcher;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;
			sampler2D _MainAlbedoTex;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local TVE_ALPHA_CLIP
			//TVE Shader Type Defines
			#define TVE_IS_OBJECT_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_HD_PIPELINE
			//TVE HD Pipeline Defines
			#pragma shader_feature_local _DISABLE_DECALS
			#pragma shader_feature_local _DISABLE_SSR


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			
			void BuildSurfaceData(FragInputs fragInputs, inout AlphaSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				
				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				bentNormalWS = surfaceData.normalWS;
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(AlphaSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				#ifdef _ALPHATEST_SHADOW_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow );
				#else
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				float3 VertexPosition3588_g60053 = inputMesh.positionOS;
				float3 Final_VertexPosition890_g60053 = ( VertexPosition3588_g60053 + _DisableSRPBatcher );
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				float temp_output_7_0_g64876 = TVE_CameraFadeStart;
				float lerpResult4755_g60053 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g64876 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g64876 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g64880 = lerpResult4755_g60053;
				outputPackedVaryingsMeshToPS.ase_texcoord1.x = vertexToFrag11_g64880;
				float3 vertexToFrag3890_g60053 = ase_worldPos;
				outputPackedVaryingsMeshToPS.ase_texcoord1.yzw = vertexToFrag3890_g60053;
				float2 vertexToFrag11_g64879 = ( ( inputMesh.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord2.xy = vertexToFrag11_g64879;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = Final_VertexPosition890_g60053;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif
			
			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
						, out float4 outColor : SV_Target0
						#else
							#ifdef WRITE_MSAA_DEPTH
							// We need the depth color as SV_Target0 for alpha to coverage
							, out float4 depthColor : SV_Target0
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target1
								#endif
							#else
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target0
								#endif
							#endif

							// Decal buffer must be last as it is bind but we can optionally write into it (based on _DISABLE_DECALS)
							#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
							, out float4 outDecalBuffer : SV_TARGET_DECAL
							#endif
						#endif

						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				AlphaSurfaceDescription surfaceDescription = (AlphaSurfaceDescription)0;
				float localCustomAlphaClip9_g64873 = ( 0.0 );
				float vertexToFrag11_g64880 = packedInput.ase_texcoord1.x;
				half Fade_Camera3743_g60053 = vertexToFrag11_g64880;
				float temp_output_41_0_g64856 = ( 1.0 * Fade_Camera3743_g60053 );
				float3 vertexToFrag3890_g60053 = packedInput.ase_texcoord1.yzw;
				float3 WorldPosition3905_g60053 = vertexToFrag3890_g60053;
				half Final_AlphaFade3727_g60053 = saturate( ( temp_output_41_0_g64856 + ( temp_output_41_0_g64856 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g60053 ) ).r ) ) );
				float2 vertexToFrag11_g64879 = packedInput.ase_texcoord2.xy;
				half2 Main_UVs15_g60053 = vertexToFrag11_g64879;
				float4 tex2DNode29_g60053 = tex2D( _MainAlbedoTex, Main_UVs15_g60053 );
				float Main_Alpha316_g60053 = ( _MainColor.a * tex2DNode29_g60053.a );
				half AlphaTreshold2132_g60053 = _AlphaCutoffValue;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g60053 = ( Main_Alpha316_g60053 - ( AlphaTreshold2132_g60053 - 0.5 ) );
				#else
				float staticSwitch3792_g60053 = Main_Alpha316_g60053;
				#endif
				half Final_Alpha3754_g60053 = staticSwitch3792_g60053;
				float temp_output_661_0_g60053 = ( Final_AlphaFade3727_g60053 * Final_Alpha3754_g60053 );
				float temp_output_3_0_g64817 = temp_output_661_0_g60053;
				half Offest27_g64817 = 0.5;
				half Feather30_g64817 = 0.5;
				float temp_output_3_0_g64873 = ( ( ( temp_output_3_0_g64817 - Offest27_g64817 ) / ( max( fwidth( temp_output_3_0_g64817 ) , 0.001 ) + Feather30_g64817 ) ) + Offest27_g64817 );
				float Alpha9_g64873 = temp_output_3_0_g64873;
				float temp_output_15_0_g64873 = 0.5;
				float Treshold9_g64873 = temp_output_15_0_g64873;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha9_g64873 - Treshold9_g64873);
				#endif
				}
				half Final_Clip914_g60053 = Alpha9_g64873;
				
				surfaceDescription.Alpha = Final_Clip914_g60053;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				//outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }
			ColorMask 0

			HLSLPROGRAM

			#define ASE_NEED_CULLFACE 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			//#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			//#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENESELECTIONPASS
			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MaxBoundsInfo;
			half4 _VertexOcclusionRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _SecondColor;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _MainUVs;
			half4 _MainColor;
			half4 _DetailBlendRemap;
			float4 _NoiseMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			half4 _AlphaMaskRemap;
			half4 _ColorsMaskRemap;
			float4 _GradientMaskRemap;
			half4 _SecondUVs;
			half4 _OverlayMaskRemap;
			float4 _EmissiveIntensityParams;
			half4 _EmissiveUVs;
			float4 _Color;
			half4 _EmissiveColor;
			half4 _LeavesMaskRemap;
			half3 _render_normals_options;
			half _SpaceMotionHighlight;
			half _SizeFadeCat;
			half _DetailTypeMode;
			half _SpaceMotionLocals;
			half _render_src;
			half _render_zw;
			half _MessageGlobalsVariation;
			half _AlphaCutoffValue;
			half _DetailCoordMode;
			half _SubsurfaceNormalValue;
			half _IsStandardShader;
			half _SpaceGlobalLayers;
			half _DetailMaskInvertMode;
			half _MainNormalValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _SecondSmoothnessValue;
			half _MainSmoothnessValue;
			half _GlobalEmissive;
			half _EmissiveExposureValue;
			half _SecondMetallicValue;
			half _MainMetallicValue;
			half _SecondNormalValue;
			half _DetailNormalValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _LayerExtrasValue;
			half _ExtrasPositionMode;
			half _GlobalOverlay;
			half _VertexDynamicMode;
			half _DetailBlendMaxValue;
			half _DetailBlendMinValue;
			half _SubsurfaceScatteringValue;
			half _DetailMaskMode;
			half _DetailProjectionMode;
			half _DetailMeshValue;
			half _VertexRollingMode;
			half _render_cull;
			half _DetailCat;
			half _RenderClip;
			half _SubsurfaceShadowValue;
			float _SubsurfaceDiffusion;
			half _SpaceRenderFade;
			half _VertexVariationMode;
			half _MainCat;
			half _HasGradient;
			half _RenderPriority;
			half _RenderQueue;
			half _SubsurfaceAmbientValue;
			half _HasEmissive;
			half _RenderMode;
			half _RenderZWrite;
			half _DetailMode;
			half _DetailBlendMode;
			half _SubsurfaceCat;
			half _EmissiveCat;
			half _IsPropShader;
			half _render_dst;
			half _VertexMasksMode;
			half _GradientCat;
			half _MotionCat;
			half _RenderCull;
			half _IsVersion;
			half _NoiseCat;
			half _SecondOcclusionValue;
			half _RenderNormals;
			half _LayerReactValue;
			half _RenderDecals;
			half _SpaceGlobalPosition;
			half _EmissiveFlagMode;
			half _HasOcclusion;
			half _Cutoff;
			half _GlobalCat;
			half _MessageMotionVariation;
			half _MessageSizeFade;
			half _OcclusionCat;
			half _RenderSSR;
			half _PerspectiveCat;
			half _SubsurfaceDirectValue;
			half _SubsurfaceAngleValue;
			half _RenderingCat;
			half _IsTVEShader;
			half _RenderCoverage;
			half _FadeCameraValue;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _BumpMap;
			sampler2D _MainTex;
			half TVE_Enabled;
			half _DisableSRPBatcher;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;
			sampler2D _MainAlbedoTex;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local TVE_ALPHA_CLIP
			//TVE Shader Type Defines
			#define TVE_IS_OBJECT_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_HD_PIPELINE
			//TVE HD Pipeline Defines
			#pragma shader_feature_local _DISABLE_DECALS
			#pragma shader_feature_local _DISABLE_SSR


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			int _ObjectId;
			int _PassValue;

			
			void BuildSurfaceData(FragInputs fragInputs, inout SceneSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				bentNormalWS = surfaceData.normalWS;
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SceneSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				float3 VertexPosition3588_g60053 = inputMesh.positionOS;
				float3 Final_VertexPosition890_g60053 = ( VertexPosition3588_g60053 + _DisableSRPBatcher );
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				float temp_output_7_0_g64876 = TVE_CameraFadeStart;
				float lerpResult4755_g60053 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g64876 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g64876 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g64880 = lerpResult4755_g60053;
				outputPackedVaryingsMeshToPS.ase_texcoord1.x = vertexToFrag11_g64880;
				float3 vertexToFrag3890_g60053 = ase_worldPos;
				outputPackedVaryingsMeshToPS.ase_texcoord1.yzw = vertexToFrag3890_g60053;
				float2 vertexToFrag11_g64879 = ( ( inputMesh.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord2.xy = vertexToFrag11_g64879;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = Final_VertexPosition890_g60053;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif
			
			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
						, out float4 outColor : SV_Target0
						#else
							#ifdef WRITE_MSAA_DEPTH
							// We need the depth color as SV_Target0 for alpha to coverage
							, out float4 depthColor : SV_Target0
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target1
								#endif
							#else
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target0
								#endif
							#endif

							// Decal buffer must be last as it is bind but we can optionally write into it (based on _DISABLE_DECALS)
							#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
							, out float4 outDecalBuffer : SV_TARGET_DECAL
							#endif
						#endif

						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SceneSurfaceDescription surfaceDescription = (SceneSurfaceDescription)0;
				float localCustomAlphaClip9_g64873 = ( 0.0 );
				float vertexToFrag11_g64880 = packedInput.ase_texcoord1.x;
				half Fade_Camera3743_g60053 = vertexToFrag11_g64880;
				float temp_output_41_0_g64856 = ( 1.0 * Fade_Camera3743_g60053 );
				float3 vertexToFrag3890_g60053 = packedInput.ase_texcoord1.yzw;
				float3 WorldPosition3905_g60053 = vertexToFrag3890_g60053;
				half Final_AlphaFade3727_g60053 = saturate( ( temp_output_41_0_g64856 + ( temp_output_41_0_g64856 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g60053 ) ).r ) ) );
				float2 vertexToFrag11_g64879 = packedInput.ase_texcoord2.xy;
				half2 Main_UVs15_g60053 = vertexToFrag11_g64879;
				float4 tex2DNode29_g60053 = tex2D( _MainAlbedoTex, Main_UVs15_g60053 );
				float Main_Alpha316_g60053 = ( _MainColor.a * tex2DNode29_g60053.a );
				half AlphaTreshold2132_g60053 = _AlphaCutoffValue;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g60053 = ( Main_Alpha316_g60053 - ( AlphaTreshold2132_g60053 - 0.5 ) );
				#else
				float staticSwitch3792_g60053 = Main_Alpha316_g60053;
				#endif
				half Final_Alpha3754_g60053 = staticSwitch3792_g60053;
				float temp_output_661_0_g60053 = ( Final_AlphaFade3727_g60053 * Final_Alpha3754_g60053 );
				float temp_output_3_0_g64817 = temp_output_661_0_g60053;
				half Offest27_g64817 = 0.5;
				half Feather30_g64817 = 0.5;
				float temp_output_3_0_g64873 = ( ( ( temp_output_3_0_g64817 - Offest27_g64817 ) / ( max( fwidth( temp_output_3_0_g64817 ) , 0.001 ) + Feather30_g64817 ) ) + Offest27_g64817 );
				float Alpha9_g64873 = temp_output_3_0_g64873;
				float temp_output_15_0_g64873 = 0.5;
				float Treshold9_g64873 = temp_output_15_0_g64873;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha9_g64873 - Treshold9_g64873);
				#endif
				}
				half Final_Clip914_g60053 = Alpha9_g64873;
				
				surfaceDescription.Alpha = Final_Clip914_g60053;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				//outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#define ASE_NEED_CULLFACE 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 100202
			#if !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif //ASE_NEED_CULLFACE


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			//#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			//#pragma shader_feature_local _ALPHATEST_ON
			//#pragma shader_feature_local _ _DISABLE_DECALS

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH
			#pragma multi_compile _ WRITE_DECAL_BUFFER

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			
			CBUFFER_START( UnityPerMaterial )
			float4 _MaxBoundsInfo;
			half4 _VertexOcclusionRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _SecondColor;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _MainUVs;
			half4 _MainColor;
			half4 _DetailBlendRemap;
			float4 _NoiseMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			half4 _AlphaMaskRemap;
			half4 _ColorsMaskRemap;
			float4 _GradientMaskRemap;
			half4 _SecondUVs;
			half4 _OverlayMaskRemap;
			float4 _EmissiveIntensityParams;
			half4 _EmissiveUVs;
			float4 _Color;
			half4 _EmissiveColor;
			half4 _LeavesMaskRemap;
			half3 _render_normals_options;
			half _SpaceMotionHighlight;
			half _SizeFadeCat;
			half _DetailTypeMode;
			half _SpaceMotionLocals;
			half _render_src;
			half _render_zw;
			half _MessageGlobalsVariation;
			half _AlphaCutoffValue;
			half _DetailCoordMode;
			half _SubsurfaceNormalValue;
			half _IsStandardShader;
			half _SpaceGlobalLayers;
			half _DetailMaskInvertMode;
			half _MainNormalValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _SecondSmoothnessValue;
			half _MainSmoothnessValue;
			half _GlobalEmissive;
			half _EmissiveExposureValue;
			half _SecondMetallicValue;
			half _MainMetallicValue;
			half _SecondNormalValue;
			half _DetailNormalValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _LayerExtrasValue;
			half _ExtrasPositionMode;
			half _GlobalOverlay;
			half _VertexDynamicMode;
			half _DetailBlendMaxValue;
			half _DetailBlendMinValue;
			half _SubsurfaceScatteringValue;
			half _DetailMaskMode;
			half _DetailProjectionMode;
			half _DetailMeshValue;
			half _VertexRollingMode;
			half _render_cull;
			half _DetailCat;
			half _RenderClip;
			half _SubsurfaceShadowValue;
			float _SubsurfaceDiffusion;
			half _SpaceRenderFade;
			half _VertexVariationMode;
			half _MainCat;
			half _HasGradient;
			half _RenderPriority;
			half _RenderQueue;
			half _SubsurfaceAmbientValue;
			half _HasEmissive;
			half _RenderMode;
			half _RenderZWrite;
			half _DetailMode;
			half _DetailBlendMode;
			half _SubsurfaceCat;
			half _EmissiveCat;
			half _IsPropShader;
			half _render_dst;
			half _VertexMasksMode;
			half _GradientCat;
			half _MotionCat;
			half _RenderCull;
			half _IsVersion;
			half _NoiseCat;
			half _SecondOcclusionValue;
			half _RenderNormals;
			half _LayerReactValue;
			half _RenderDecals;
			half _SpaceGlobalPosition;
			half _EmissiveFlagMode;
			half _HasOcclusion;
			half _Cutoff;
			half _GlobalCat;
			half _MessageMotionVariation;
			half _MessageSizeFade;
			half _OcclusionCat;
			half _RenderSSR;
			half _PerspectiveCat;
			half _SubsurfaceDirectValue;
			half _SubsurfaceAngleValue;
			half _RenderingCat;
			half _IsTVEShader;
			half _RenderCoverage;
			half _FadeCameraValue;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half TVE_Enabled;
			half _DisableSRPBatcher;
			sampler2D _MainNormalTex;
			sampler2D _SecondNormalTex;
			sampler2D _MainMaskTex;
			sampler2D _SecondMaskTex;
			half TVE_OverlaySmoothness;
			sampler2D _MainAlbedoTex;
			sampler2D _SecondAlbedoTex;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_FRAG_VFACE
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_DETAIL_MODE_OFF TVE_DETAIL_MODE_ON
			#pragma shader_feature_local TVE_DETAIL_TYPE_VERTEX_BLUE TVE_DETAIL_TYPE_PROJECTION
			//TVE Shader Type Defines
			#define TVE_IS_OBJECT_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_HD_PIPELINE
			//TVE HD Pipeline Defines
			#pragma shader_feature_local _DISABLE_DECALS
			#pragma shader_feature_local _DISABLE_SSR


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			float3 ObjectPosition_UNITY_MATRIX_M(  )
			{
				return float3(UNITY_MATRIX_M[0].w, UNITY_MATRIX_M[1].w, UNITY_MATRIX_M[2].w );
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				
				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				bentNormalWS = surfaceData.normalWS;
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalPrepassBuffer.hlsl"
			#endif
			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				float3 VertexPosition3588_g60053 = inputMesh.positionOS;
				float3 Final_VertexPosition890_g60053 = ( VertexPosition3588_g60053 + _DisableSRPBatcher );
				
				float2 vertexToFrag11_g64879 = ( ( inputMesh.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = vertexToFrag11_g64879;
				float2 lerpResult1545_g60053 = lerp( inputMesh.ase_texcoord.xy , inputMesh.ase_texcoord1.xy , _DetailCoordMode);
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float2 staticSwitch3466_g60053 = lerpResult1545_g60053;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float2 staticSwitch3466_g60053 = (ase_worldPos).xz;
				#else
				float2 staticSwitch3466_g60053 = lerpResult1545_g60053;
				#endif
				float2 vertexToFrag11_g64813 = ( ( staticSwitch3466_g60053 * (_SecondUVs).xy ) + (_SecondUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = vertexToFrag11_g64813;
				half Mesh_DetailMask90_g60053 = inputMesh.ase_color.b;
				float vertexToFrag11_g64796 = ( ( Mesh_DetailMask90_g60053 - 0.5 ) + _DetailMeshValue );
				outputPackedVaryingsMeshToPS.ase_texcoord4.x = vertexToFrag11_g64796;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				float ase_vertexTangentSign = inputMesh.tangentOS.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord4.yzw = ase_worldBitangent;
				
				float3 vertexToFrag3890_g60053 = ase_worldPos;
				outputPackedVaryingsMeshToPS.ase_texcoord5.xyz = vertexToFrag3890_g60053;
				float3 localObjectPosition_UNITY_MATRIX_M14_g64862 = ObjectPosition_UNITY_MATRIX_M();
				#ifdef SHADEROPTIONS_CAMERA_RELATIVE_RENDERING
				float3 staticSwitch13_g64862 = ( localObjectPosition_UNITY_MATRIX_M14_g64862 + _WorldSpaceCameraPos );
				#else
				float3 staticSwitch13_g64862 = localObjectPosition_UNITY_MATRIX_M14_g64862;
				#endif
				half3 ObjectData20_g64863 = staticSwitch13_g64862;
				half3 WorldData19_g64863 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g64863 = WorldData19_g64863;
				#else
				float3 staticSwitch14_g64863 = ObjectData20_g64863;
				#endif
				float3 temp_output_114_0_g64862 = staticSwitch14_g64863;
				float3 vertexToFrag4224_g60053 = temp_output_114_0_g64862;
				outputPackedVaryingsMeshToPS.ase_texcoord6.xyz = vertexToFrag4224_g60053;
				
				float temp_output_7_0_g64876 = TVE_CameraFadeStart;
				float lerpResult4755_g60053 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g64876 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g64876 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g64880 = lerpResult4755_g60053;
				outputPackedVaryingsMeshToPS.ase_texcoord5.w = vertexToFrag11_g64880;
				
				outputPackedVaryingsMeshToPS.ase_normal = inputMesh.normalOS;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord6.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = Final_VertexPosition890_g60053;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				return outputPackedVaryingsMeshToPS;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
						, out float4 outColor : SV_Target0
						#else
							#ifdef WRITE_MSAA_DEPTH
							// We need the depth color as SV_Target0 for alpha to coverage
							, out float4 depthColor : SV_Target0
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target1
								#endif
							#else
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target0
								#endif
							#endif

							// Decal buffer must be last as it is bind but we can optionally write into it (based on _DISABLE_DECALS)
							#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
							, out float4 outDecalBuffer : SV_TARGET_DECAL
							#endif
						#endif

						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float2 vertexToFrag11_g64879 = packedInput.ase_texcoord3.xy;
				half2 Main_UVs15_g60053 = vertexToFrag11_g64879;
				float3 unpack4112_g60053 = UnpackNormalScale( tex2D( _MainNormalTex, Main_UVs15_g60053 ), _MainNormalValue );
				unpack4112_g60053.z = lerp( 1, unpack4112_g60053.z, saturate(_MainNormalValue) );
				half3 Main_Normal137_g60053 = unpack4112_g60053;
				float3 lerpResult3372_g60053 = lerp( float3( 0,0,1 ) , Main_Normal137_g60053 , _DetailNormalValue);
				float2 vertexToFrag11_g64813 = packedInput.ase_texcoord3.zw;
				half2 Second_UVs17_g60053 = vertexToFrag11_g64813;
				float3 unpack4114_g60053 = UnpackNormalScale( tex2D( _SecondNormalTex, Second_UVs17_g60053 ), _SecondNormalValue );
				unpack4114_g60053.z = lerp( 1, unpack4114_g60053.z, saturate(_SecondNormalValue) );
				half3 Second_Normal179_g60053 = unpack4114_g60053;
				float vertexToFrag11_g64796 = packedInput.ase_texcoord4.x;
				float temp_output_3919_0_g60053 = vertexToFrag11_g64796;
				float3 ase_worldBitangent = packedInput.ase_texcoord4.yzw;
				float3 tanToWorld0 = float3( tangentWS.xyz.x, ase_worldBitangent.x, normalWS.x );
				float3 tanToWorld1 = float3( tangentWS.xyz.y, ase_worldBitangent.y, normalWS.y );
				float3 tanToWorld2 = float3( tangentWS.xyz.z, ase_worldBitangent.z, normalWS.z );
				float3 tanNormal4099_g60053 = Main_Normal137_g60053;
				float3 worldNormal4099_g60053 = float3(dot(tanToWorld0,tanNormal4099_g60053), dot(tanToWorld1,tanNormal4099_g60053), dot(tanToWorld2,tanNormal4099_g60053));
				float3 Main_Normal_WS4101_g60053 = worldNormal4099_g60053;
				float lerpResult1537_g60053 = lerp( Main_Normal_WS4101_g60053.y , -Main_Normal_WS4101_g60053.y , _DetailProjectionMode);
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float staticSwitch3467_g60053 = temp_output_3919_0_g60053;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float staticSwitch3467_g60053 = ( ( lerpResult1537_g60053 * 0.5 ) + _DetailMeshValue );
				#else
				float staticSwitch3467_g60053 = temp_output_3919_0_g60053;
				#endif
				half Blend_Source1540_g60053 = staticSwitch3467_g60053;
				float4 tex2DNode35_g60053 = tex2D( _MainMaskTex, Main_UVs15_g60053 );
				half Main_Mask57_g60053 = tex2DNode35_g60053.b;
				float4 tex2DNode33_g60053 = tex2D( _SecondMaskTex, Second_UVs17_g60053 );
				half Second_Mask81_g60053 = tex2DNode33_g60053.b;
				float lerpResult1327_g60053 = lerp( Main_Mask57_g60053 , Second_Mask81_g60053 , _DetailMaskMode);
				float lerpResult4058_g60053 = lerp( lerpResult1327_g60053 , ( 1.0 - lerpResult1327_g60053 ) , _DetailMaskInvertMode);
				float temp_output_7_0_g64830 = _DetailBlendMinValue;
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch4838_g60053 = 0.0;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch4838_g60053 = saturate( ( ( saturate( ( Blend_Source1540_g60053 + ( Blend_Source1540_g60053 * lerpResult4058_g60053 ) ) ) - temp_output_7_0_g64830 ) / ( _DetailBlendMaxValue - temp_output_7_0_g64830 ) ) );
				#else
				float staticSwitch4838_g60053 = 0.0;
				#endif
				half Mask_Detail147_g60053 = staticSwitch4838_g60053;
				float3 lerpResult3376_g60053 = lerp( Main_Normal137_g60053 , BlendNormal( lerpResult3372_g60053 , Second_Normal179_g60053 ) , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch267_g60053 = Main_Normal137_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch267_g60053 = lerpResult3376_g60053;
				#else
				float3 staticSwitch267_g60053 = Main_Normal137_g60053;
				#endif
				float3 temp_output_13_0_g64811 = staticSwitch267_g60053;
				float3 switchResult12_g64811 = (((isFrontFace>0)?(temp_output_13_0_g64811):(( temp_output_13_0_g64811 * _render_normals_options ))));
				half3 Blend_Normal312_g60053 = switchResult12_g64811;
				half3 Final_Normal366_g60053 = Blend_Normal312_g60053;
				
				half Main_Smoothness227_g60053 = ( tex2DNode35_g60053.a * _MainSmoothnessValue );
				half Second_Smoothness236_g60053 = ( tex2DNode33_g60053.a * _SecondSmoothnessValue );
				float lerpResult266_g60053 = lerp( Main_Smoothness227_g60053 , Second_Smoothness236_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch297_g60053 = Main_Smoothness227_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch297_g60053 = lerpResult266_g60053;
				#else
				float staticSwitch297_g60053 = Main_Smoothness227_g60053;
				#endif
				half Blend_Smoothness314_g60053 = staticSwitch297_g60053;
				half Global_OverlaySmoothness311_g60053 = TVE_OverlaySmoothness;
				half VertexDynamicMode4798_g60053 = _VertexDynamicMode;
				float lerpResult4801_g60053 = lerp( Main_Normal_WS4101_g60053.y , packedInput.ase_normal.y , VertexDynamicMode4798_g60053);
				float lerpResult3567_g60053 = lerp( 0.5 , 1.0 , lerpResult4801_g60053);
				float4 tex2DNode29_g60053 = tex2D( _MainAlbedoTex, Main_UVs15_g60053 );
				half Main_AlbedoTex_G3526_g60053 = tex2DNode29_g60053.g;
				float4 tex2DNode89_g60053 = tex2D( _SecondAlbedoTex, Second_UVs17_g60053 );
				half Second_AlbedoTex_G3581_g60053 = tex2DNode89_g60053.g;
				float lerpResult3579_g60053 = lerp( Main_AlbedoTex_G3526_g60053 , Second_AlbedoTex_G3581_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch3574_g60053 = Main_AlbedoTex_G3526_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch3574_g60053 = lerpResult3579_g60053;
				#else
				float staticSwitch3574_g60053 = Main_AlbedoTex_G3526_g60053;
				#endif
				float4 temp_output_93_19_g64789 = TVE_ExtrasCoords;
				float3 vertexToFrag3890_g60053 = packedInput.ase_texcoord5.xyz;
				float3 WorldPosition3905_g60053 = vertexToFrag3890_g60053;
				float3 vertexToFrag4224_g60053 = packedInput.ase_texcoord6.xyz;
				half3 ObjectData20_g64855 = vertexToFrag4224_g60053;
				half3 WorldData19_g64855 = vertexToFrag3890_g60053;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g64855 = WorldData19_g64855;
				#else
				float3 staticSwitch14_g64855 = ObjectData20_g64855;
				#endif
				float3 ObjectPosition4223_g60053 = staticSwitch14_g64855;
				float3 lerpResult4827_g60053 = lerp( WorldPosition3905_g60053 , ObjectPosition4223_g60053 , _ExtrasPositionMode);
				float3 Position82_g64789 = lerpResult4827_g60053;
				float temp_output_84_0_g64789 = _LayerExtrasValue;
				float4 lerpResult88_g64789 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g64789).zw + ( (temp_output_93_19_g64789).xy * (Position82_g64789).xz ) ),temp_output_84_0_g64789 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g64789]);
				float4 break89_g64789 = lerpResult88_g64789;
				half Global_Extras_Overlay156_g60053 = break89_g64789.b;
				half Overlay_Variation4560_g60053 = 1.0;
				half Overlay_Commons1365_g60053 = ( _GlobalOverlay * Global_Extras_Overlay156_g60053 * Overlay_Variation4560_g60053 );
				float temp_output_7_0_g64869 = _OverlayMaskMinValue;
				half Overlay_Mask269_g60053 = saturate( ( ( ( ( ( lerpResult3567_g60053 * 0.5 ) + staticSwitch3574_g60053 ) * Overlay_Commons1365_g60053 ) - temp_output_7_0_g64869 ) / ( _OverlayMaskMaxValue - temp_output_7_0_g64869 ) ) );
				float lerpResult343_g60053 = lerp( Blend_Smoothness314_g60053 , Global_OverlaySmoothness311_g60053 , Overlay_Mask269_g60053);
				half Final_Smoothness371_g60053 = lerpResult343_g60053;
				half Global_Extras_Wetness305_g60053 = break89_g64789.g;
				float lerpResult3673_g60053 = lerp( 0.0 , Global_Extras_Wetness305_g60053 , _GlobalWetness);
				half Final_SmoothnessAndWetness4130_g60053 = saturate( ( Final_Smoothness371_g60053 + lerpResult3673_g60053 ) );
				
				float localCustomAlphaClip9_g64873 = ( 0.0 );
				float vertexToFrag11_g64880 = packedInput.ase_texcoord5.w;
				half Fade_Camera3743_g60053 = vertexToFrag11_g64880;
				float temp_output_41_0_g64856 = ( 1.0 * Fade_Camera3743_g60053 );
				half Final_AlphaFade3727_g60053 = saturate( ( temp_output_41_0_g64856 + ( temp_output_41_0_g64856 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g60053 ) ).r ) ) );
				float Main_Alpha316_g60053 = ( _MainColor.a * tex2DNode29_g60053.a );
				half AlphaTreshold2132_g60053 = _AlphaCutoffValue;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g60053 = ( Main_Alpha316_g60053 - ( AlphaTreshold2132_g60053 - 0.5 ) );
				#else
				float staticSwitch3792_g60053 = Main_Alpha316_g60053;
				#endif
				half Final_Alpha3754_g60053 = staticSwitch3792_g60053;
				float temp_output_661_0_g60053 = ( Final_AlphaFade3727_g60053 * Final_Alpha3754_g60053 );
				float temp_output_3_0_g64817 = temp_output_661_0_g60053;
				half Offest27_g64817 = 0.5;
				half Feather30_g64817 = 0.5;
				float temp_output_3_0_g64873 = ( ( ( temp_output_3_0_g64817 - Offest27_g64817 ) / ( max( fwidth( temp_output_3_0_g64817 ) , 0.001 ) + Feather30_g64817 ) ) + Offest27_g64817 );
				float Alpha9_g64873 = temp_output_3_0_g64873;
				float temp_output_15_0_g64873 = 0.5;
				float Treshold9_g64873 = temp_output_15_0_g64873;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha9_g64873 - Treshold9_g64873);
				#endif
				}
				half Final_Clip914_g60053 = Alpha9_g64873;
				
				surfaceDescription.Normal = Final_Normal366_g60053;
				surfaceDescription.Smoothness = Final_SmoothnessAndWetness4130_g60053;
				surfaceDescription.Alpha = Final_Clip914_g60053;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				//outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif

				#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
				DecalPrepassData decalPrepassData;
				decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
				decalPrepassData.decalLayerMask = GetMeshRenderingDecalLayer();
				EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="Forward" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
			Cull [_CullModeForward]
			ZTest [_ZTestDepthEqualForOpaque]
			ZWrite [_ZWrite]

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			ColorMask [_ColorMaskTransparentVel] 1

			HLSLPROGRAM

			#define ASE_NEED_CULLFACE 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _AMBIENT_OCCLUSION 1
			#define HAVE_MESH_MODIFICATION
			#define ASE_SRP_VERSION 100202
			#if !defined(ASE_NEED_CULLFACE)
			#define ASE_NEED_CULLFACE 1
			#endif //ASE_NEED_CULLFACE


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			//#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			//#pragma shader_feature_local _ALPHATEST_ON

			#if !defined(DEBUG_DISPLAY) && defined(_ALPHATEST_ON)
			#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
			#endif

			#define SHADERPASS SHADERPASS_FORWARD
			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
			#pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			// CBuffer must be declared before Material.hlsl since it internaly uses _BlendMode now
			CBUFFER_START( UnityPerMaterial )
			float4 _MaxBoundsInfo;
			half4 _VertexOcclusionRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _SecondColor;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _MainUVs;
			half4 _MainColor;
			half4 _DetailBlendRemap;
			float4 _NoiseMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			half4 _AlphaMaskRemap;
			half4 _ColorsMaskRemap;
			float4 _GradientMaskRemap;
			half4 _SecondUVs;
			half4 _OverlayMaskRemap;
			float4 _EmissiveIntensityParams;
			half4 _EmissiveUVs;
			float4 _Color;
			half4 _EmissiveColor;
			half4 _LeavesMaskRemap;
			half3 _render_normals_options;
			half _SpaceMotionHighlight;
			half _SizeFadeCat;
			half _DetailTypeMode;
			half _SpaceMotionLocals;
			half _render_src;
			half _render_zw;
			half _MessageGlobalsVariation;
			half _AlphaCutoffValue;
			half _DetailCoordMode;
			half _SubsurfaceNormalValue;
			half _IsStandardShader;
			half _SpaceGlobalLayers;
			half _DetailMaskInvertMode;
			half _MainNormalValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _SecondSmoothnessValue;
			half _MainSmoothnessValue;
			half _GlobalEmissive;
			half _EmissiveExposureValue;
			half _SecondMetallicValue;
			half _MainMetallicValue;
			half _SecondNormalValue;
			half _DetailNormalValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _LayerExtrasValue;
			half _ExtrasPositionMode;
			half _GlobalOverlay;
			half _VertexDynamicMode;
			half _DetailBlendMaxValue;
			half _DetailBlendMinValue;
			half _SubsurfaceScatteringValue;
			half _DetailMaskMode;
			half _DetailProjectionMode;
			half _DetailMeshValue;
			half _VertexRollingMode;
			half _render_cull;
			half _DetailCat;
			half _RenderClip;
			half _SubsurfaceShadowValue;
			float _SubsurfaceDiffusion;
			half _SpaceRenderFade;
			half _VertexVariationMode;
			half _MainCat;
			half _HasGradient;
			half _RenderPriority;
			half _RenderQueue;
			half _SubsurfaceAmbientValue;
			half _HasEmissive;
			half _RenderMode;
			half _RenderZWrite;
			half _DetailMode;
			half _DetailBlendMode;
			half _SubsurfaceCat;
			half _EmissiveCat;
			half _IsPropShader;
			half _render_dst;
			half _VertexMasksMode;
			half _GradientCat;
			half _MotionCat;
			half _RenderCull;
			half _IsVersion;
			half _NoiseCat;
			half _SecondOcclusionValue;
			half _RenderNormals;
			half _LayerReactValue;
			half _RenderDecals;
			half _SpaceGlobalPosition;
			half _EmissiveFlagMode;
			half _HasOcclusion;
			half _Cutoff;
			half _GlobalCat;
			half _MessageMotionVariation;
			half _MessageSizeFade;
			half _OcclusionCat;
			half _RenderSSR;
			half _PerspectiveCat;
			half _SubsurfaceDirectValue;
			half _SubsurfaceAngleValue;
			half _RenderingCat;
			half _IsTVEShader;
			half _RenderCoverage;
			half _FadeCameraValue;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half TVE_Enabled;
			half _DisableSRPBatcher;
			sampler2D _MainAlbedoTex;
			sampler2D _SecondAlbedoTex;
			sampler2D _MainNormalTex;
			sampler2D _MainMaskTex;
			sampler2D _SecondMaskTex;
			half4 TVE_OverlayColor;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];
			sampler2D _SecondNormalTex;
			sampler2D _EmissiveTex;
			half TVE_OverlaySmoothness;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
			#define HAS_LIGHTLOOP
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_FRAG_VFACE
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_DETAIL_MODE_OFF TVE_DETAIL_MODE_ON
			#pragma shader_feature_local TVE_DETAIL_TYPE_VERTEX_BLUE TVE_DETAIL_TYPE_PROJECTION
			//TVE Shader Type Defines
			#define TVE_IS_OBJECT_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_HD_PIPELINE
			//TVE HD Pipeline Defines
			#pragma shader_feature_local _DISABLE_DECALS
			#pragma shader_feature_local _DISABLE_SSR


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 previousPositionOS : TEXCOORD4;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						float3 precomputedVelocity : TEXCOORD5;
					#endif
				#endif
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 interp03 : TEXCOORD3;
				float4 interp04 : TEXCOORD4;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 vpassPositionCS : TEXCOORD5;
					float3 vpassPreviousPositionCS : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord9 : TEXCOORD9;
				float4 ase_texcoord10 : TEXCOORD10;
				float4 ase_texcoord11 : TEXCOORD11;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			float3 ObjectPosition_UNITY_MATRIX_M(  )
			{
				return float3(UNITY_MATRIX_M[0].w, UNITY_MATRIX_M[1].w, UNITY_MATRIX_M[2].w );
			}
			
			float3 ASEGetEmissionHDRColor(float3 ldrColor, float luminanceIntensity, float exposureWeight, float inverseCurrentExposureMultiplier)
			{
				float3 hdrColor = ldrColor * luminanceIntensity;
				hdrColor = lerp( hdrColor* inverseCurrentExposureMultiplier, hdrColor, exposureWeight);
				return hdrColor;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				bentNormalWS = surfaceData.normalWS;
				
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );


				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				#ifdef _ASE_BAKEDGI
				builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				float3 VertexPosition3588_g60053 = inputMesh.positionOS;
				float3 Final_VertexPosition890_g60053 = ( VertexPosition3588_g60053 + _DisableSRPBatcher );
				
				float2 vertexToFrag11_g64879 = ( ( inputMesh.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord7.xy = vertexToFrag11_g64879;
				float2 lerpResult1545_g60053 = lerp( inputMesh.ase_texcoord.xy , inputMesh.uv1.xy , _DetailCoordMode);
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float2 staticSwitch3466_g60053 = lerpResult1545_g60053;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float2 staticSwitch3466_g60053 = (ase_worldPos).xz;
				#else
				float2 staticSwitch3466_g60053 = lerpResult1545_g60053;
				#endif
				float2 vertexToFrag11_g64813 = ( ( staticSwitch3466_g60053 * (_SecondUVs).xy ) + (_SecondUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord7.zw = vertexToFrag11_g64813;
				half Mesh_DetailMask90_g60053 = inputMesh.ase_color.b;
				float vertexToFrag11_g64796 = ( ( Mesh_DetailMask90_g60053 - 0.5 ) + _DetailMeshValue );
				outputPackedVaryingsMeshToPS.ase_texcoord8.x = vertexToFrag11_g64796;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				float ase_vertexTangentSign = inputMesh.tangentOS.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord8.yzw = ase_worldBitangent;
				float3 vertexToFrag3890_g60053 = ase_worldPos;
				outputPackedVaryingsMeshToPS.ase_texcoord9.xyz = vertexToFrag3890_g60053;
				float3 localObjectPosition_UNITY_MATRIX_M14_g64862 = ObjectPosition_UNITY_MATRIX_M();
				#ifdef SHADEROPTIONS_CAMERA_RELATIVE_RENDERING
				float3 staticSwitch13_g64862 = ( localObjectPosition_UNITY_MATRIX_M14_g64862 + _WorldSpaceCameraPos );
				#else
				float3 staticSwitch13_g64862 = localObjectPosition_UNITY_MATRIX_M14_g64862;
				#endif
				half3 ObjectData20_g64863 = staticSwitch13_g64862;
				half3 WorldData19_g64863 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g64863 = WorldData19_g64863;
				#else
				float3 staticSwitch14_g64863 = ObjectData20_g64863;
				#endif
				float3 temp_output_114_0_g64862 = staticSwitch14_g64863;
				float3 vertexToFrag4224_g60053 = temp_output_114_0_g64862;
				outputPackedVaryingsMeshToPS.ase_texcoord10.xyz = vertexToFrag4224_g60053;
				
				float2 vertexToFrag11_g64849 = ( ( inputMesh.ase_texcoord.xy * (_EmissiveUVs).xy ) + (_EmissiveUVs).zw );
				outputPackedVaryingsMeshToPS.ase_texcoord11.xy = vertexToFrag11_g64849;
				
				float temp_output_7_0_g64876 = TVE_CameraFadeStart;
				float lerpResult4755_g60053 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g64876 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g64876 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g64880 = lerpResult4755_g60053;
				outputPackedVaryingsMeshToPS.ase_texcoord9.w = vertexToFrag11_g64880;
				
				outputPackedVaryingsMeshToPS.ase_normal = inputMesh.normalOS;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord10.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord11.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = Final_VertexPosition890_g60053;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = inputMesh.tangentOS;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(positionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						//ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}
				#endif

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outputPackedVaryingsMeshToPS.vpassPositionCS = float3(VPASSpositionCS.xyw);
					outputPackedVaryingsMeshToPS.vpassPreviousPositionCS = float3(VPASSpreviousPositionCS.xyw);
				#endif
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 previousPositionOS : TEXCOORD4;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						float3 precomputedVelocity : TEXCOORD5;
					#endif
				#endif
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					o.previousPositionOS = v.previousPositionOS;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						o.precomputedVelocity = v.precomputedVelocity;
					#endif
				#endif
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
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
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
					#endif
				#endif
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag(PackedVaryingsMeshToPS packedInput,
					#ifdef OUTPUT_SPLIT_LIGHTING
						out float4 outColor : SV_Target0,
						out float4 outDiffuseLighting : SV_Target1,
						OUTPUT_SSSBUFFER(outSSSBuffer)
					#else
						out float4 outColor : SV_Target0
					#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						, out float4 outMotionVec : SV_Target1
					#endif
					#endif
					#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
					#endif
					
						)
			{
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outMotionVec = float4(2.0, 0.0, 0.0, 0.0);
				#endif

				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.interp03.xyzw;
				input.texCoord2 = packedInput.interp04.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				input.positionSS.xy = _OffScreenRendering > 0 ? (input.positionSS.xy * _OffScreenDownsampleFactor) : input.positionSS.xy;
				uint2 tileIndex = uint2(input.positionSS.xy) / GetTileSize ();

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz, tileIndex );

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				half Leaves_Mask4511_g60053 = 1.0;
				float3 lerpResult4521_g60053 = lerp( float3( 1,1,1 ) , ( float3(1,1,1) * float3(1,1,1) * float3(1,1,1) ) , Leaves_Mask4511_g60053);
				float2 vertexToFrag11_g64879 = packedInput.ase_texcoord7.xy;
				half2 Main_UVs15_g60053 = vertexToFrag11_g64879;
				float4 tex2DNode29_g60053 = tex2D( _MainAlbedoTex, Main_UVs15_g60053 );
				half3 Main_Albedo99_g60053 = ( (_MainColor).rgb * (tex2DNode29_g60053).rgb );
				float2 vertexToFrag11_g64813 = packedInput.ase_texcoord7.zw;
				half2 Second_UVs17_g60053 = vertexToFrag11_g64813;
				float4 tex2DNode89_g60053 = tex2D( _SecondAlbedoTex, Second_UVs17_g60053 );
				half3 Second_Albedo153_g60053 = (( _SecondColor * tex2DNode89_g60053 )).rgb;
				float3 lerpResult4834_g60053 = lerp( ( Main_Albedo99_g60053 * Second_Albedo153_g60053 * 4.594794 ) , Second_Albedo153_g60053 , _DetailBlendMode);
				float vertexToFrag11_g64796 = packedInput.ase_texcoord8.x;
				float temp_output_3919_0_g60053 = vertexToFrag11_g64796;
				float3 unpack4112_g60053 = UnpackNormalScale( tex2D( _MainNormalTex, Main_UVs15_g60053 ), _MainNormalValue );
				unpack4112_g60053.z = lerp( 1, unpack4112_g60053.z, saturate(_MainNormalValue) );
				half3 Main_Normal137_g60053 = unpack4112_g60053;
				float3 ase_worldBitangent = packedInput.ase_texcoord8.yzw;
				float3 tanToWorld0 = float3( tangentWS.xyz.x, ase_worldBitangent.x, normalWS.x );
				float3 tanToWorld1 = float3( tangentWS.xyz.y, ase_worldBitangent.y, normalWS.y );
				float3 tanToWorld2 = float3( tangentWS.xyz.z, ase_worldBitangent.z, normalWS.z );
				float3 tanNormal4099_g60053 = Main_Normal137_g60053;
				float3 worldNormal4099_g60053 = float3(dot(tanToWorld0,tanNormal4099_g60053), dot(tanToWorld1,tanNormal4099_g60053), dot(tanToWorld2,tanNormal4099_g60053));
				float3 Main_Normal_WS4101_g60053 = worldNormal4099_g60053;
				float lerpResult1537_g60053 = lerp( Main_Normal_WS4101_g60053.y , -Main_Normal_WS4101_g60053.y , _DetailProjectionMode);
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float staticSwitch3467_g60053 = temp_output_3919_0_g60053;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float staticSwitch3467_g60053 = ( ( lerpResult1537_g60053 * 0.5 ) + _DetailMeshValue );
				#else
				float staticSwitch3467_g60053 = temp_output_3919_0_g60053;
				#endif
				half Blend_Source1540_g60053 = staticSwitch3467_g60053;
				float4 tex2DNode35_g60053 = tex2D( _MainMaskTex, Main_UVs15_g60053 );
				half Main_Mask57_g60053 = tex2DNode35_g60053.b;
				float4 tex2DNode33_g60053 = tex2D( _SecondMaskTex, Second_UVs17_g60053 );
				half Second_Mask81_g60053 = tex2DNode33_g60053.b;
				float lerpResult1327_g60053 = lerp( Main_Mask57_g60053 , Second_Mask81_g60053 , _DetailMaskMode);
				float lerpResult4058_g60053 = lerp( lerpResult1327_g60053 , ( 1.0 - lerpResult1327_g60053 ) , _DetailMaskInvertMode);
				float temp_output_7_0_g64830 = _DetailBlendMinValue;
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch4838_g60053 = 0.0;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch4838_g60053 = saturate( ( ( saturate( ( Blend_Source1540_g60053 + ( Blend_Source1540_g60053 * lerpResult4058_g60053 ) ) ) - temp_output_7_0_g64830 ) / ( _DetailBlendMaxValue - temp_output_7_0_g64830 ) ) );
				#else
				float staticSwitch4838_g60053 = 0.0;
				#endif
				half Mask_Detail147_g60053 = staticSwitch4838_g60053;
				float3 lerpResult235_g60053 = lerp( Main_Albedo99_g60053 , lerpResult4834_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch255_g60053 = Main_Albedo99_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch255_g60053 = lerpResult235_g60053;
				#else
				float3 staticSwitch255_g60053 = Main_Albedo99_g60053;
				#endif
				half3 Blend_Albedo265_g60053 = staticSwitch255_g60053;
				half3 Blend_AlbedoTinted2808_g60053 = ( lerpResult4521_g60053 * Blend_Albedo265_g60053 );
				half3 Blend_AlbedoColored863_g60053 = Blend_AlbedoTinted2808_g60053;
				half3 Blend_AlbedoAndSubsurface149_g60053 = Blend_AlbedoColored863_g60053;
				half3 Global_OverlayColor1758_g60053 = (TVE_OverlayColor).rgb;
				half VertexDynamicMode4798_g60053 = _VertexDynamicMode;
				float lerpResult4801_g60053 = lerp( Main_Normal_WS4101_g60053.y , packedInput.ase_normal.y , VertexDynamicMode4798_g60053);
				float lerpResult3567_g60053 = lerp( 0.5 , 1.0 , lerpResult4801_g60053);
				half Main_AlbedoTex_G3526_g60053 = tex2DNode29_g60053.g;
				half Second_AlbedoTex_G3581_g60053 = tex2DNode89_g60053.g;
				float lerpResult3579_g60053 = lerp( Main_AlbedoTex_G3526_g60053 , Second_AlbedoTex_G3581_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch3574_g60053 = Main_AlbedoTex_G3526_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch3574_g60053 = lerpResult3579_g60053;
				#else
				float staticSwitch3574_g60053 = Main_AlbedoTex_G3526_g60053;
				#endif
				float4 temp_output_93_19_g64789 = TVE_ExtrasCoords;
				float3 vertexToFrag3890_g60053 = packedInput.ase_texcoord9.xyz;
				float3 WorldPosition3905_g60053 = vertexToFrag3890_g60053;
				float3 vertexToFrag4224_g60053 = packedInput.ase_texcoord10.xyz;
				half3 ObjectData20_g64855 = vertexToFrag4224_g60053;
				half3 WorldData19_g64855 = vertexToFrag3890_g60053;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g64855 = WorldData19_g64855;
				#else
				float3 staticSwitch14_g64855 = ObjectData20_g64855;
				#endif
				float3 ObjectPosition4223_g60053 = staticSwitch14_g64855;
				float3 lerpResult4827_g60053 = lerp( WorldPosition3905_g60053 , ObjectPosition4223_g60053 , _ExtrasPositionMode);
				float3 Position82_g64789 = lerpResult4827_g60053;
				float temp_output_84_0_g64789 = _LayerExtrasValue;
				float4 lerpResult88_g64789 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g64789).zw + ( (temp_output_93_19_g64789).xy * (Position82_g64789).xz ) ),temp_output_84_0_g64789 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g64789]);
				float4 break89_g64789 = lerpResult88_g64789;
				half Global_Extras_Overlay156_g60053 = break89_g64789.b;
				half Overlay_Variation4560_g60053 = 1.0;
				half Overlay_Commons1365_g60053 = ( _GlobalOverlay * Global_Extras_Overlay156_g60053 * Overlay_Variation4560_g60053 );
				float temp_output_7_0_g64869 = _OverlayMaskMinValue;
				half Overlay_Mask269_g60053 = saturate( ( ( ( ( ( lerpResult3567_g60053 * 0.5 ) + staticSwitch3574_g60053 ) * Overlay_Commons1365_g60053 ) - temp_output_7_0_g64869 ) / ( _OverlayMaskMaxValue - temp_output_7_0_g64869 ) ) );
				float3 lerpResult336_g60053 = lerp( Blend_AlbedoAndSubsurface149_g60053 , Global_OverlayColor1758_g60053 , Overlay_Mask269_g60053);
				half3 Final_Albedo359_g60053 = lerpResult336_g60053;
				
				float3 lerpResult3372_g60053 = lerp( float3( 0,0,1 ) , Main_Normal137_g60053 , _DetailNormalValue);
				float3 unpack4114_g60053 = UnpackNormalScale( tex2D( _SecondNormalTex, Second_UVs17_g60053 ), _SecondNormalValue );
				unpack4114_g60053.z = lerp( 1, unpack4114_g60053.z, saturate(_SecondNormalValue) );
				half3 Second_Normal179_g60053 = unpack4114_g60053;
				float3 lerpResult3376_g60053 = lerp( Main_Normal137_g60053 , BlendNormal( lerpResult3372_g60053 , Second_Normal179_g60053 ) , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch267_g60053 = Main_Normal137_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch267_g60053 = lerpResult3376_g60053;
				#else
				float3 staticSwitch267_g60053 = Main_Normal137_g60053;
				#endif
				float3 temp_output_13_0_g64811 = staticSwitch267_g60053;
				float3 switchResult12_g64811 = (((isFrontFace>0)?(temp_output_13_0_g64811):(( temp_output_13_0_g64811 * _render_normals_options ))));
				half3 Blend_Normal312_g60053 = switchResult12_g64811;
				half3 Final_Normal366_g60053 = Blend_Normal312_g60053;
				
				half Main_Metallic237_g60053 = ( tex2DNode35_g60053.r * _MainMetallicValue );
				half Second_Metallic226_g60053 = ( tex2DNode33_g60053.r * _SecondMetallicValue );
				float lerpResult278_g60053 = lerp( Main_Metallic237_g60053 , Second_Metallic226_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch299_g60053 = Main_Metallic237_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch299_g60053 = lerpResult278_g60053;
				#else
				float staticSwitch299_g60053 = Main_Metallic237_g60053;
				#endif
				half Blend_Metallic306_g60053 = staticSwitch299_g60053;
				float lerpResult342_g60053 = lerp( Blend_Metallic306_g60053 , 0.0 , Overlay_Mask269_g60053);
				half Final_Metallic367_g60053 = lerpResult342_g60053;
				
				float3 hdEmission4189_g60053 = ASEGetEmissionHDRColor(_EmissiveColor.rgb,_EmissiveIntensityParams.x,_EmissiveExposureValue,GetInverseCurrentExposureMultiplier());
				float2 vertexToFrag11_g64849 = packedInput.ase_texcoord11.xy;
				half2 Emissive_UVs2468_g60053 = vertexToFrag11_g64849;
				half Global_Extras_Emissive4203_g60053 = break89_g64789.r;
				float lerpResult4206_g60053 = lerp( 1.0 , Global_Extras_Emissive4203_g60053 , _GlobalEmissive);
				half3 Final_Emissive2476_g60053 = ( (( hdEmission4189_g60053 * tex2D( _EmissiveTex, Emissive_UVs2468_g60053 ) )).rgb * lerpResult4206_g60053 );
				
				half Main_Smoothness227_g60053 = ( tex2DNode35_g60053.a * _MainSmoothnessValue );
				half Second_Smoothness236_g60053 = ( tex2DNode33_g60053.a * _SecondSmoothnessValue );
				float lerpResult266_g60053 = lerp( Main_Smoothness227_g60053 , Second_Smoothness236_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch297_g60053 = Main_Smoothness227_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch297_g60053 = lerpResult266_g60053;
				#else
				float staticSwitch297_g60053 = Main_Smoothness227_g60053;
				#endif
				half Blend_Smoothness314_g60053 = staticSwitch297_g60053;
				half Global_OverlaySmoothness311_g60053 = TVE_OverlaySmoothness;
				float lerpResult343_g60053 = lerp( Blend_Smoothness314_g60053 , Global_OverlaySmoothness311_g60053 , Overlay_Mask269_g60053);
				half Final_Smoothness371_g60053 = lerpResult343_g60053;
				half Global_Extras_Wetness305_g60053 = break89_g64789.g;
				float lerpResult3673_g60053 = lerp( 0.0 , Global_Extras_Wetness305_g60053 , _GlobalWetness);
				half Final_SmoothnessAndWetness4130_g60053 = saturate( ( Final_Smoothness371_g60053 + lerpResult3673_g60053 ) );
				
				float lerpResult240_g60053 = lerp( 1.0 , tex2DNode35_g60053.g , _MainOcclusionValue);
				half Main_Occlusion247_g60053 = lerpResult240_g60053;
				float lerpResult239_g60053 = lerp( 1.0 , tex2DNode33_g60053.g , _SecondOcclusionValue);
				half Second_Occlusion251_g60053 = lerpResult239_g60053;
				float lerpResult294_g60053 = lerp( Main_Occlusion247_g60053 , Second_Occlusion251_g60053 , Mask_Detail147_g60053);
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch310_g60053 = Main_Occlusion247_g60053;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch310_g60053 = lerpResult294_g60053;
				#else
				float staticSwitch310_g60053 = Main_Occlusion247_g60053;
				#endif
				half Blend_Occlusion323_g60053 = staticSwitch310_g60053;
				
				float localCustomAlphaClip9_g64873 = ( 0.0 );
				float vertexToFrag11_g64880 = packedInput.ase_texcoord9.w;
				half Fade_Camera3743_g60053 = vertexToFrag11_g64880;
				float temp_output_41_0_g64856 = ( 1.0 * Fade_Camera3743_g60053 );
				half Final_AlphaFade3727_g60053 = saturate( ( temp_output_41_0_g64856 + ( temp_output_41_0_g64856 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g60053 ) ).r ) ) );
				float Main_Alpha316_g60053 = ( _MainColor.a * tex2DNode29_g60053.a );
				half AlphaTreshold2132_g60053 = _AlphaCutoffValue;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g60053 = ( Main_Alpha316_g60053 - ( AlphaTreshold2132_g60053 - 0.5 ) );
				#else
				float staticSwitch3792_g60053 = Main_Alpha316_g60053;
				#endif
				half Final_Alpha3754_g60053 = staticSwitch3792_g60053;
				float temp_output_661_0_g60053 = ( Final_AlphaFade3727_g60053 * Final_Alpha3754_g60053 );
				float temp_output_3_0_g64817 = temp_output_661_0_g60053;
				half Offest27_g64817 = 0.5;
				half Feather30_g64817 = 0.5;
				float temp_output_3_0_g64873 = ( ( ( temp_output_3_0_g64817 - Offest27_g64817 ) / ( max( fwidth( temp_output_3_0_g64817 ) , 0.001 ) + Feather30_g64817 ) ) + Offest27_g64817 );
				float Alpha9_g64873 = temp_output_3_0_g64873;
				float temp_output_15_0_g64873 = 0.5;
				float Treshold9_g64873 = temp_output_15_0_g64873;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha9_g64873 - Treshold9_g64873);
				#endif
				}
				half Final_Clip914_g60053 = Alpha9_g64873;
				
				surfaceDescription.Albedo = Final_Albedo359_g60053;
				surfaceDescription.Normal = Final_Normal366_g60053;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = Final_Metallic367_g60053;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = Final_Emissive2476_g60053;
				surfaceDescription.Smoothness = Final_SmoothnessAndWetness4130_g60053;
				surfaceDescription.Occlusion = Blend_Occlusion323_g60053;
				surfaceDescription.Alpha = Final_Clip914_g60053;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef _ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);

				PreLightData preLightData = GetPreLightData(V, posInput, bsdfData);

				outColor = float4(0.0, 0.0, 0.0, 0.0);
				#ifdef DEBUG_DISPLAY
				#ifdef OUTPUT_SPLIT_LIGHTING
					outDiffuseLighting = 0;
					ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
				#endif

				bool viewMaterial = false;
				int bufferSize = _DebugViewMaterialArray[0].x;
				if (bufferSize != 0)
				{
					bool needLinearToSRGB = false;
					float3 result = float3(1.0, 0.0, 1.0);

					for (int index = 1; index <= bufferSize; index++)
					{
						int indexMaterialProperty = _DebugViewMaterialArray[index].x;

						if (indexMaterialProperty != 0)
						{
							viewMaterial = true;

							GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
							GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
							GetBuiltinDataDebug(indexMaterialProperty, builtinData, posInput, result, needLinearToSRGB);
							GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
							GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);
						}
					}

					if (!needLinearToSRGB)
						result = SRGBToLinear(max(0, result));

					outColor = float4(result, 1.0);
				}

				if (!viewMaterial)
				{
					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_DIFFUSE_COLOR || _DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_SPECULAR_COLOR)
					{
						float3 result = float3(0.0, 0.0, 0.0);

						GetPBRValidatorDebug(surfaceData, result);

						outColor = float4(result, 1.0f);
					}
					else if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outColor = result;
					}
					else
				#endif
					{
				#ifdef _SURFACE_TYPE_TRANSPARENT
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_TRANSPARENT;
				#else
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_OPAQUE;
				#endif
					
						LightLoopOutput lightLoopOutput;
						LightLoop(V, posInput, preLightData, bsdfData, builtinData, featureFlags, lightLoopOutput);

						// Alias
						float3 diffuseLighting = lightLoopOutput.diffuseLighting;
						float3 specularLighting = lightLoopOutput.specularLighting;
					
						diffuseLighting *= GetCurrentExposureMultiplier();
						specularLighting *= GetCurrentExposureMultiplier();

				#ifdef OUTPUT_SPLIT_LIGHTING
						if (_EnableSubsurfaceScattering != 0 && ShouldOutputSplitLighting(bsdfData))
						{
							outColor = float4(specularLighting, 1.0);
							outDiffuseLighting = float4(TagLightingForSSS(diffuseLighting), 1.0);
						}
						else
						{
							outColor = float4(diffuseLighting + specularLighting, 1.0);
							outDiffuseLighting = 0;
						}
						ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
				#else
						outColor = ApplyBlendMode(diffuseLighting, specularLighting, builtinData.opacity);
						outColor = EvaluateAtmosphericScattering(posInput, V, outColor);
				#endif

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						float4 VPASSpositionCS = float4(packedInput.vpassPositionCS.xy, 0.0, packedInput.vpassPositionCS.z);
						float4 VPASSpreviousPositionCS = float4(packedInput.vpassPreviousPositionCS.xy, 0.0, packedInput.vpassPreviousPositionCS.z);

						bool forceNoMotion = any(unity_MotionVectorsParams.yw == 0.0);
						if (!forceNoMotion)
						{
							float2 motionVec = CalculateMotionVector(VPASSpositionCS, VPASSpreviousPositionCS);
							EncodeMotionVector(motionVec * 0.5, outMotionVec);
							outMotionVec.zw = 1.0;
						}
				#endif
					}

				#ifdef DEBUG_DISPLAY
				}
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}
			ENDHLSL
		}
		
	}
	CustomEditor "TVEShaderCoreGUI"
	
	Fallback "Hidden/BOXOPHOBIC/The Vegetation Engine/Fallback"
}
/*ASEBEGIN
Version=18934
1920;12;1920;1017;3095.445;534.538;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;10;-2176,-768;Half;False;Property;_render_cull;_render_cull;205;1;[HideInInspector];Create;True;0;3;Both;0;Back;1;Front;2;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1792,-768;Half;False;Property;_render_dst;_render_dst;207;1;[HideInInspector];Create;True;0;2;Opaque;0;Transparent;1;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;343;-2176,384;Inherit;False;Define Shader Object;-1;;60051;1237b3cc9fbfe714d8343c91216dc9b4;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;407;-1536,-896;Inherit;False;Compile Core;-1;;60052;634b02fd1f32e6a4c875d8fc2c450956;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-2176,-896;Half;False;Property;_IsPropShader;_IsPropShader;203;1;[HideInInspector];Create;True;0;0;0;True;0;False;1;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;382;-2176,-384;Inherit;False;Base Shader;0;;60053;856f7164d1c579d43a5cf4968a75ca43;77,3880,1,4029,1,4028,1,4204,1,3900,1,3904,1,3903,1,3908,1,4172,1,1300,1,1298,1,4179,1,3586,0,4499,0,3658,0,1708,0,3509,1,1712,0,3873,1,893,0,4544,1,1718,1,1715,1,1717,1,1714,1,916,1,1762,0,1763,0,3568,1,1949,1,1776,0,3475,1,4210,1,1745,0,3479,0,4510,0,3501,0,3221,2,1646,0,1271,0,3889,1,2807,0,3886,0,2953,0,3887,0,3243,0,3888,0,3957,1,2172,0,3883,0,3728,1,3949,0,4781,0,2658,0,1742,0,3484,0,1737,1,1733,1,1735,1,1736,1,3575,1,4837,1,1734,1,878,1,1550,1,860,0,2260,1,2261,1,2032,1,2054,1,2039,1,2062,1,4177,1,4217,1,3592,1,4242,0,2750,0;0;20;FLOAT3;0;FLOAT3;528;FLOAT3;2489;FLOAT;531;FLOAT;4842;FLOAT;529;FLOAT;3678;FLOAT;530;FLOAT;4122;FLOAT;4134;FLOAT;1235;FLOAT3;1230;FLOAT;1461;FLOAT;1290;FLOAT;721;FLOAT;532;FLOAT;629;FLOAT3;534;FLOAT;4867;FLOAT4;4841
Node;AmplifyShaderEditor.RangedFloatNode;17;-1600,-768;Half;False;Property;_render_zw;_render_zw;208;1;[HideInInspector];Create;True;0;2;Opaque;0;Transparent;1;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1984,-768;Half;False;Property;_render_src;_render_src;206;1;[HideInInspector];Create;True;0;0;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;406;-1344,-896;Inherit;False;Compile All Shaders;-1;;64886;e67c8238031dbf04ab79a5d4d63d1b4f;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-1984,-896;Half;False;Property;_IsStandardShader;_IsStandardShader;204;1;[HideInInspector];Create;True;0;0;0;True;0;False;1;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;405;-1920,384;Inherit;False;Define Pipeline HD;-1;;64887;7af1bc24c286d754db63fb6a44aba77b;0;0;1;FLOAT;529
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;403;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;TransparentDepthPostpass;0;9;TransparentDepthPostpass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=TransparentDepthPostpass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;404;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;Forward;0;10;Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;0;True;-19;0;True;-20;1;0;True;-21;0;True;-22;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-28;False;False;False;True;True;True;True;True;0;True;-44;False;False;False;False;False;True;True;0;True;-4;255;False;-1;255;True;-5;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;0;True;-23;True;0;True;-30;False;True;1;LightMode=Forward;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;398;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;DepthOnly;0;4;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;False;False;False;False;False;False;False;False;True;True;0;True;-6;255;False;-1;255;True;-7;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;402;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;TransparentDepthPrepass;0;8;TransparentDepthPrepass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;False;False;False;False;False;False;False;False;True;True;0;False;-1;255;False;-1;255;False;-1;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;3;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=TransparentDepthPrepass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;394;-1376,-384;Float;False;True;-1;2;TVEShaderCoreGUI;0;19;BOXOPHOBIC/The Vegetation Engine/Default/Prop Standard Lit;28cd5599e02859647ae1798e4fcaef6c;True;GBuffer;0;0;GBuffer;35;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;False;False;False;False;False;False;False;False;True;True;0;True;-13;255;False;-1;255;True;-12;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;True;0;True;-14;False;True;1;LightMode=GBuffer;False;False;0;Hidden/BOXOPHOBIC/The Vegetation Engine/Fallback;0;0;Standard;42;Surface Type;0;0;  Rendering Pass;1;0;  Refraction Model;0;0;    Blending Mode;0;0;    Blend Preserves Specular;1;0;  Receive Fog;1;0;  Back Then Front Rendering;0;0;  Transparent Depth Prepass;0;0;  Transparent Depth Postpass;0;0;  Transparent Writes Motion Vector;0;0;  Distortion;0;0;    Distortion Mode;0;0;    Distortion Depth Test;1;0;  ZWrite;0;0;  Z Test;4;0;Double-Sided;1;0;Alpha Clipping;0;0;  Use Shadow Threshold;0;0;Material Type,InvertActionOnDeselection;0;0;  Energy Conserving Specular;1;0;  Transmission;1;0;Receive Decals;1;0;Receives SSR;1;0;Receive SSR Transparent;0;0;Motion Vectors;0;637832003451130352;  Add Precomputed Velocity;0;0;Specular AA;0;0;Specular Occlusion Mode;0;0;Override Baked GI;0;0;Depth Offset;0;0;DOTS Instancing;1;0;LOD CrossFade;1;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Vertex Position;0;0;0;11;True;True;True;True;True;False;False;False;False;False;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;400;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;Distortion;0;6;Distortion;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;True;1;False;-1;1;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;0;True;-10;255;False;-1;255;True;-11;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;False;True;1;LightMode=DistortionVectors;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;399;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;Motion Vectors;0;5;Motion Vectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;False;False;False;False;False;False;False;False;True;True;0;True;-8;255;False;-1;255;True;-9;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=MotionVectors;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;395;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;META;0;1;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;396;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;397;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;401;-1376,-384;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;TransparentBackface;0;7;TransparentBackface;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;0;True;-19;0;True;-20;1;0;True;-21;0;True;-22;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;False;True;True;True;True;True;0;True;-44;False;False;False;False;False;False;False;True;0;True;-23;True;0;True;-31;False;True;1;LightMode=TransparentBackface;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.CommentaryNode;33;-2176,-512;Inherit;False;1022.896;100;Final;0;;0,1,0.5,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;37;-2176,-1024;Inherit;False;1026.438;100;Internal;0;;1,0.252,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;340;-2176,256;Inherit;False;1026.438;100;Features;0;;0,1,0.5,1;0;0
WireConnection;394;0;382;0
WireConnection;394;1;382;528
WireConnection;394;4;382;529
WireConnection;394;6;382;2489
WireConnection;394;7;382;530
WireConnection;394;8;382;531
WireConnection;394;9;382;532
WireConnection;394;11;382;534
ASEEND*/
//CHKSM=212F17F801EE21487C9165B0C05C6DE73584740B
