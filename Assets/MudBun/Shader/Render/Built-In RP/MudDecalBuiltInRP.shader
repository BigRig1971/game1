// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MudBun/Mud Decal Mesh (Built-In RP)"
{
	Properties
	{
		_EdgeFadeDistance("Edge Fade Distance", Float) = 0
		_EdgeFadeColor("Edge Fade Color", Color) = (1,1,1,1)
		_ColorBlendSrc("Color Blend Src", Int) = 2
		_ColorBlendDst("Color Blend Dst", Int) = 10
		[HideInInspector]_Color("Color", Color) = (1,1,1,1)
		[HideInInspector]_MaterialNeedsSdfProperties("Material Needs SDF Properties", Float) = 1
		[HideInInspector]_MaterialNeedsVoxelExpansion("Material Needs Voxel Expansion", Float) = 1
		_StencilMask("Stencil Mask", Int) = 32
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		ZWrite Off
		ZTest Always
		Stencil
		{
			Ref [_StencilMask]
			ReadMask [_StencilMask]
			WriteMask [_StencilMask]
			Comp NotEqual
			Pass Keep
			Fail Keep
			ZFail Keep
		}
		Blend [_ColorBlendSrc] [_ColorBlendDst]
		
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.5
		#define SHADER_GRAPH
		#pragma multi_compile _ MUDBUN_PROCEDURAL
		#include "Assets/MudBun/Shader/Render/ShaderCommon.cginc"
		#include "Assets/MudBun/Shader/Render/MeshCommon.cginc"
		#include "Assets/MudBun/Shader/Decal.cginc"
		#pragma surface surf StandardCustomLighting keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 vertexToFrag107_g26;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _EdgeFadeColor;
		uniform int _StencilMask;
		uniform int _ColorBlendSrc;
		uniform int _ColorBlendDst;
		uniform float _MaterialNeedsSdfProperties;
		uniform float _MaterialNeedsVoxelExpansion;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _EdgeFadeDistance;


		float2 UnStereo( float2 UV )
		{
			#if UNITY_SINGLE_PASS_STEREO
			float4 scaleOffset = unity_StereoScaleOffset[ unity_StereoEyeIndex ];
			UV.xy = (UV.xy - scaleOffset.zw) / scaleOffset.xy;
			#endif
			return UV;
		}


		float3 InvertDepthDir6_g27( float3 In )
		{
			float3 result = In;
			#if !defined(ASE_SRP_VERSION) || ASE_SRP_VERSION <= 70301
			result *= float3(1,1,-1);
			#endif
			return result;
		}


		int MyCustomExpression49_g26( float3 PositionWs, out float4 Color, out float3 Emission, out float Metallic, out float Smoothness, out float SdfValue )
		{
			DecalResults res = sdf_decal(PositionWs);
			Color = res.mat.color;
			Emission = res.mat.emissionHash.rgb;
			Metallic = res.mat.metallicSmoothnessSizeTightness.x;
			Smoothness = res.mat.metallicSmoothnessSizeTightness.y;
			SdfValue = res.sdfValue;
			return res.hit;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			o.vertexToFrag107_g26 = ase_vertex3Pos;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float4 unityObjectToClipPos20_g27 = UnityObjectToClipPos( i.vertexToFrag107_g26 );
			float4 computeScreenPos23_g27 = ComputeScreenPos( unityObjectToClipPos20_g27 );
			computeScreenPos23_g27 = computeScreenPos23_g27 / computeScreenPos23_g27.w;
			computeScreenPos23_g27.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? computeScreenPos23_g27.z : computeScreenPos23_g27.z* 0.5 + 0.5;
			float2 UV22_g30 = computeScreenPos23_g27.xy;
			float2 localUnStereo22_g30 = UnStereo( UV22_g30 );
			float2 break9_g27 = localUnStereo22_g30;
			float clampDepth12_g27 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, computeScreenPos23_g27.xy );
			#ifdef UNITY_REVERSED_Z
				float staticSwitch14_g27 = ( 1.0 - clampDepth12_g27 );
			#else
				float staticSwitch14_g27 = clampDepth12_g27;
			#endif
			float3 appendResult16_g27 = (float3(break9_g27.x , break9_g27.y , staticSwitch14_g27));
			float4 appendResult8_g27 = (float4((appendResult16_g27*2.0 + -1.0) , 1.0));
			float4 temp_output_15_0_g27 = mul( unity_CameraInvProjection, appendResult8_g27 );
			float3 temp_output_10_0_g27 = ( (temp_output_15_0_g27).xyz / (temp_output_15_0_g27).w );
			float3 In6_g27 = temp_output_10_0_g27;
			float3 localInvertDepthDir6_g27 = InvertDepthDir6_g27( In6_g27 );
			float4 appendResult2_g27 = (float4(localInvertDepthDir6_g27 , 1.0));
			float3 PositionWs49_g26 = ( _MaterialNeedsSdfProperties * _MaterialNeedsVoxelExpansion * mul( unity_CameraToWorld, appendResult2_g27 ) ).xyz;
			float4 Color49_g26 = float4( 0,0,0,0 );
			float3 Emission49_g26 = float3( 0,0,0 );
			float Metallic49_g26 = 0.0;
			float Smoothness49_g26 = 0.0;
			float SdfValue49_g26 = 0.0;
			int localMyCustomExpression49_g26 = MyCustomExpression49_g26( PositionWs49_g26 , Color49_g26 , Emission49_g26 , Metallic49_g26 , Smoothness49_g26 , SdfValue49_g26 );
			float4 temp_output_25_0_g26 = ( _Color * Color49_g26 );
			float4 lerpResult93 = lerp( _EdgeFadeColor , ( ( ( ( _StencilMask + _ColorBlendSrc + _ColorBlendDst ) * 0 ) + 1 ) * temp_output_25_0_g26 ) , saturate( ( -SdfValue49_g26 / max( _EdgeFadeDistance , 0.0 ) ) ));
			c.rgb = lerpResult93.rgb;
			c.a = (lerpResult93).a;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
	}
	CustomEditor "MudBun.DecalMeshMaterialEditor"
}
/*ASEBEGIN
Version=18800
-1593;178;1302;678;1431.017;353.4172;1.786809;True;False
Node;AmplifyShaderEditor.IntNode;80;-768,32;Inherit;False;Property;_ColorBlendDst;Color Blend Dst;3;0;Create;True;0;0;0;False;0;False;10;10;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;79;-768,-64;Inherit;False;Property;_ColorBlendSrc;Color Blend Src;2;0;Create;True;0;0;0;False;0;False;2;2;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;101;-768,-160;Inherit;False;Property;_StencilMask;Stencil Mask;11;0;Create;True;0;0;0;False;0;False;32;32;False;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-512,-32;Inherit;False;3;3;0;INT;0;False;1;INT;0;False;2;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-768,480;Inherit;False;Property;_EdgeFadeDistance;Edge Fade Distance;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;100;-768,128;Inherit;False;Mud Decal;4;;26;680e80eee6c3d494fb7f6eab0eef7416;0;0;6;COLOR;9;FLOAT;13;FLOAT3;10;FLOAT;11;FLOAT;12;FLOAT;45
Node;AmplifyShaderEditor.NegateNode;88;-448,304;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;86;-512,480;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-384,-32;Inherit;False;2;2;0;INT;0;False;1;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;87;-256,384;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;-240,-32;Inherit;False;2;2;0;INT;0;False;1;INT;1;False;1;INT;0
Node;AmplifyShaderEditor.SaturateNode;90;-96,384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;94;0,512;Inherit;False;Property;_EdgeFadeColor;Edge Fade Color;1;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;0,32;Inherit;False;2;2;0;INT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;93;256,256;Inherit;False;3;0;COLOR;1,1,1,0;False;1;COLOR;1,1,1,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;95;512,368;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;768,-64;Float;False;True;-1;3;MudBun.DecalMeshMaterialEditor;0;0;CustomLighting;MudBun/Mud Decal Mesh (Built-In RP);False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;7;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;True;0;True;101;0;True;101;0;True;101;6;False;-1;1;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;1;2;True;79;10;True;80;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;12;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;82;0;101;0
WireConnection;82;1;79;0
WireConnection;82;2;80;0
WireConnection;88;0;100;45
WireConnection;86;0;85;0
WireConnection;81;0;82;0
WireConnection;87;0;88;0
WireConnection;87;1;86;0
WireConnection;83;0;81;0
WireConnection;90;0;87;0
WireConnection;84;0;83;0
WireConnection;84;1;100;9
WireConnection;93;0;94;0
WireConnection;93;1;84;0
WireConnection;93;2;90;0
WireConnection;95;0;93;0
WireConnection;6;9;95;0
WireConnection;6;13;93;0
ASEEND*/
//CHKSM=7BA9812F950D60612C6391A55907ABA878D8112B