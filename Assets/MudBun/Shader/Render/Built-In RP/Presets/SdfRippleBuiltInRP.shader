// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MudBun/SDF Ripple (Built-In RP)"
{
	Properties
	{
		[HideInInspector]_Color("Color", Color) = (1,1,1,1)
		[HideInInspector]_Emission("Emission", Color) = (1,1,1,1)
		[HideInInspector]_Metallic("Metallic", Range( 0 , 1)) = 0
		[HideInInspector]_Smoothness("Smoothness", Range( 0 , 1)) = 1
		[HideInInspector]_IsMeshRenderMaterial("Is Mesh Render Material", Float) = 1
		_TemporarlFrequency("Temporarl Frequency", Float) = 1
		_FadeDistance("Fade Distance", Range( 0.01 , 1)) = 0.3
		_SpatialFrequency("Spatial Frequency", Float) = 10
		_DitherTexture("Dither Texture", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.0
		#define SHADER_GRAPH
		#pragma multi_compile _ MUDBUN_PROCEDURAL
		#include "Assets/MudBun/Shader/Render/ShaderCommon.cginc"
		#include "Assets/MudBun/Shader/Render/MeshCommon.cginc"
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 

		struct appdata_full_custom
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			float4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
			uint ase_vertexId : SV_VertexID;
		};
		struct Input
		{
			float4 vertexToFrag5_g1;
			float vertexToFrag15;
			float3 vertexToFrag16_g1;
			float3 vertexToFrag27_g3;
			uint ase_vertexId;
			float3 vertexToFrag6_g1;
			float vertexToFrag8_g1;
			float vertexToFrag7_g1;
		};

		uniform float _IsMeshRenderMaterial;
		uniform float _SpatialFrequency;
		uniform float _TemporarlFrequency;
		uniform float _FadeDistance;
		uniform sampler2D _DitherTexture;


		float3 MudBunMeshPoint( int VertexID, out float3 PositionLs, out float3 NormalWs, out float3 NormalLs, out float3 TangentWs, out float3 TangentLs, out float4 Color, out float4 EmissionHash, out float Metallic, out float Smoothness, out float4 TextureWeight, out float SdfValue, out float3 Outward2dNormalLs, out float3 Outward2dNormalWs )
		{
			float4 positionWs;
			float2 metallicSmoothness;
			mudbun_mesh_vert(VertexID, positionWs, PositionLs, NormalWs, NormalLs, TangentWs, TangentLs, Color, EmissionHash, metallicSmoothness, TextureWeight, SdfValue, Outward2dNormalLs, Outward2dNormalWs);
			Metallic = metallicSmoothness.x;
			Smoothness = metallicSmoothness.y;
			return positionWs.xyz;
		}


		void vertexDataFunc( inout appdata_full_custom v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			int VertexID4_g1 = v.ase_vertexId;
			float3 PositionLs4_g1 = float3( 0,0,0 );
			float3 NormalWs4_g1 = float3( 0,0,0 );
			float3 NormalLs4_g1 = float3( 0,0,0 );
			float3 TangentWs4_g1 = float3( 0,0,0 );
			float3 TangentLs4_g1 = float3( 0,0,0 );
			float4 Color4_g1 = float4( 0,0,0,0 );
			float4 EmissionHash4_g1 = float4( 0,0,0,0 );
			float Metallic4_g1 = 0.0;
			float Smoothness4_g1 = 0.0;
			float4 TextureWeight4_g1 = float4( 1,0,0,0 );
			float SdfValue4_g1 = 0.0;
			float3 Outward2dNormalLs4_g1 = float3( 0,0,0 );
			float3 Outward2dNormalWs4_g1 = float3( 0,0,0 );
			float3 localMudBunMeshPoint4_g1 = MudBunMeshPoint( VertexID4_g1 , PositionLs4_g1 , NormalWs4_g1 , NormalLs4_g1 , TangentWs4_g1 , TangentLs4_g1 , Color4_g1 , EmissionHash4_g1 , Metallic4_g1 , Smoothness4_g1 , TextureWeight4_g1 , SdfValue4_g1 , Outward2dNormalLs4_g1 , Outward2dNormalWs4_g1 );
			v.vertex.xyz = localMudBunMeshPoint4_g1;
			v.vertex.w = 1;
			v.normal = NormalWs4_g1;
			o.vertexToFrag5_g1 = Color4_g1;
			o.vertexToFrag15 = SdfValue4_g1;
			o.vertexToFrag16_g1 = localMudBunMeshPoint4_g1;
			o.vertexToFrag27_g3 = float3( 0,0,0 );
			o.ase_vertexId = v.ase_vertexId;
			o.vertexToFrag6_g1 = (EmissionHash4_g1).xyz;
			o.vertexToFrag8_g1 = Metallic4_g1;
			o.vertexToFrag7_g1 = Smoothness4_g1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 temp_output_25_0_g1 = ( _IsMeshRenderMaterial * _Color * i.vertexToFrag5_g1 );
			float mulTime10 = _Time.y * ( _TemporarlFrequency * -6.283185 );
			float localComputeOpaqueTransparency20_g3 = ( 0.0 );
			float4 unityObjectToClipPos1_g2 = UnityObjectToClipPos( i.vertexToFrag16_g1 );
			float4 computeScreenPos3_g2 = ComputeScreenPos( unityObjectToClipPos1_g2 );
			float2 ScreenPos20_g3 = (( ( computeScreenPos3_g2 / (computeScreenPos3_g2).w ) * _ScreenParams )).xy;
			float3 VertPos20_g3 = i.vertexToFrag27_g3;
			int VertexID4_g1 = i.ase_vertexId;
			float3 PositionLs4_g1 = float3( 0,0,0 );
			float3 NormalWs4_g1 = float3( 0,0,0 );
			float3 NormalLs4_g1 = float3( 0,0,0 );
			float3 TangentWs4_g1 = float3( 0,0,0 );
			float3 TangentLs4_g1 = float3( 0,0,0 );
			float4 Color4_g1 = float4( 0,0,0,0 );
			float4 EmissionHash4_g1 = float4( 0,0,0,0 );
			float Metallic4_g1 = 0.0;
			float Smoothness4_g1 = 0.0;
			float4 TextureWeight4_g1 = float4( 1,0,0,0 );
			float SdfValue4_g1 = 0.0;
			float3 Outward2dNormalLs4_g1 = float3( 0,0,0 );
			float3 Outward2dNormalWs4_g1 = float3( 0,0,0 );
			float3 localMudBunMeshPoint4_g1 = MudBunMeshPoint( VertexID4_g1 , PositionLs4_g1 , NormalWs4_g1 , NormalLs4_g1 , TangentWs4_g1 , TangentLs4_g1 , Color4_g1 , EmissionHash4_g1 , Metallic4_g1 , Smoothness4_g1 , TextureWeight4_g1 , SdfValue4_g1 , Outward2dNormalLs4_g1 , Outward2dNormalWs4_g1 );
			float Hash20_g3 = (EmissionHash4_g1).w;
			float AlphaIn20_g3 = (temp_output_25_0_g1).a;
			float AlphaOut20_g3 = 0;
			float AlphaThreshold20_g3 = 0;
			sampler2D DitherNoiseTexture20_g3 = _DitherTexture;
			int DitherNoiseTextureSize20_g3 = 256;
			int UseRandomDither20_g3 = 0;
			float AlphaCutoutThreshold20_g3 = 0.0;
			float DitherBlend20_g3 = 1.0;
			{
			float alpha = AlphaIn20_g3;
			computeOpaqueTransparency(ScreenPos20_g3, VertPos20_g3, Hash20_g3, DitherNoiseTexture20_g3, DitherNoiseTextureSize20_g3, UseRandomDither20_g3 > 0, AlphaCutoutThreshold20_g3, DitherBlend20_g3,  alpha, AlphaThreshold20_g3);
			AlphaOut20_g3 = alpha;
			}
			clip( ( ( sin( ( ( _SpatialFrequency * 6.283185 * i.vertexToFrag15 ) + mulTime10 ) ) + 1.0 ) * ( 1.0 - saturate( ( abs( i.vertexToFrag15 ) / _FadeDistance ) ) ) * AlphaOut20_g3 ) - AlphaThreshold20_g3);
			o.Albedo = temp_output_25_0_g1.rgb;
			o.Emission = ( i.vertexToFrag6_g1 * (_Emission).rgb );
			o.Metallic = ( _Metallic * i.vertexToFrag8_g1 );
			o.Smoothness = ( _Smoothness * i.vertexToFrag7_g1 );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
-1610;238;1302;678;1789.602;-105.817;1.577075;True;False
Node;AmplifyShaderEditor.RangedFloatNode;13;-1280,640;Inherit;False;Property;_TemporarlFrequency;Temporarl Frequency;6;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1;-1280,0;Inherit;False;Mud Mesh;0;;1;4f444db5091a94140ab2b15b933d37b6;0;0;17;COLOR;9;FLOAT;13;FLOAT3;10;FLOAT;11;FLOAT;12;FLOAT4;33;FLOAT3;0;FLOAT3;32;FLOAT3;51;FLOAT3;2;FLOAT3;31;FLOAT3;52;FLOAT3;48;FLOAT3;46;FLOAT;45;FLOAT2;15;FLOAT;41
Node;AmplifyShaderEditor.VertexToFragmentNode;15;-768,480;Inherit;False;False;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1280,544;Inherit;False;Property;_SpatialFrequency;Spatial Frequency;8;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-560,640;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-6.283185;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-288,512;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;6.283185;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;10;-288,640;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;22;-288,736;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1280,736;Inherit;False;Property;_FadeDistance;Fade Distance;7;0;Create;True;0;0;0;False;0;False;0.3;0;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;18;-128,768;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-64,512;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;6;64,512;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-1280,928;Inherit;True;Property;_DitherTexture;Dither Texture;9;0;Create;True;0;0;0;False;0;False;f240bbb7854046345b218811e5681a54;f240bbb7854046345b218811e5681a54;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SaturateNode;19;0,768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;192,512;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;20;160,768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;3;-384,896;Inherit;False;Mud Alpha Threshold;-1;;3;926535703f4c32948ac1f55275a22bf0;0;9;8;FLOAT2;0,0;False;15;FLOAT3;0,0,0;False;18;FLOAT;0;False;22;FLOAT;0;False;19;SAMPLER2D;_Sampler193;False;26;INT;256;False;9;INT;0;False;6;FLOAT;0;False;7;FLOAT;1;False;2;FLOAT;24;FLOAT;25
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;384,512;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClipNode;5;688,448;Inherit;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1280,0;Float;False;True;-1;4;ASEMaterialInspector;0;0;Standard;MudBun/SDF Ripple (Built-In RP);False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Absolute;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;1;45
WireConnection;14;0;13;0
WireConnection;11;0;16;0
WireConnection;11;2;15;0
WireConnection;10;0;14;0
WireConnection;22;0;15;0
WireConnection;18;0;22;0
WireConnection;18;1;17;0
WireConnection;12;0;11;0
WireConnection;12;1;10;0
WireConnection;6;0;12;0
WireConnection;19;0;18;0
WireConnection;9;0;6;0
WireConnection;20;0;19;0
WireConnection;3;8;1;15
WireConnection;3;18;1;41
WireConnection;3;22;1;13
WireConnection;3;19;4;0
WireConnection;7;0;9;0
WireConnection;7;1;20;0
WireConnection;7;2;3;24
WireConnection;5;0;1;9
WireConnection;5;1;7;0
WireConnection;5;2;3;25
WireConnection;0;0;5;0
WireConnection;0;2;1;10
WireConnection;0;3;1;11
WireConnection;0;4;1;12
WireConnection;0;11;1;0
WireConnection;0;12;1;2
ASEEND*/
//CHKSM=F23219B89A68380EDC0AC7A61AC260F450913653