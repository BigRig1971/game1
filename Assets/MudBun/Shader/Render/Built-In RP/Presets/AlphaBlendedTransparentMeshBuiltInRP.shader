// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MudBun/Alpha-Blended Transparent Mesh (Built-In RP)"
{
	Properties
	{
		[HideInInspector]_Color("Color", Color) = (1,1,1,1)
		[HideInInspector]_Emission("Emission", Color) = (1,1,1,1)
		[HideInInspector]_Metallic("Metallic", Range( 0 , 1)) = 0
		[HideInInspector]_Smoothness("Smoothness", Range( 0 , 1)) = 1
		[HideInInspector]_IsMeshRenderMaterial("Is Mesh Render Material", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
		#define SHADER_GRAPH
		#pragma multi_compile _ MUDBUN_PROCEDURAL
		#include "Assets/MudBun/Shader/Render/ShaderCommon.cginc"
		#include "Assets/MudBun/Shader/Render/MeshCommon.cginc"

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
			float3 vertexToFrag6_g1;
			float vertexToFrag8_g1;
			float vertexToFrag7_g1;
		};

		uniform float _IsMeshRenderMaterial;


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
			o.vertexToFrag6_g1 = (EmissionHash4_g1).xyz;
			o.vertexToFrag8_g1 = Metallic4_g1;
			o.vertexToFrag7_g1 = Smoothness4_g1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 temp_output_25_0_g1 = ( _IsMeshRenderMaterial * _Color * i.vertexToFrag5_g1 );
			o.Albedo = temp_output_25_0_g1.rgb;
			o.Emission = ( i.vertexToFrag6_g1 * (_Emission).rgb );
			o.Metallic = ( _Metallic * i.vertexToFrag8_g1 );
			o.Smoothness = ( _Smoothness * i.vertexToFrag7_g1 );
			o.Alpha = (temp_output_25_0_g1).a;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float4 customPack2 : TEXCOORD2;
				float1 customPack3 : TEXCOORD3;
				float3 worldPos : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full_custom v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xyzw = customInputData.vertexToFrag5_g1;
				o.customPack2.xyz = customInputData.vertexToFrag6_g1;
				o.customPack2.w = customInputData.vertexToFrag8_g1;
				o.customPack3.x = customInputData.vertexToFrag7_g1;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.vertexToFrag5_g1 = IN.customPack1.xyzw;
				surfIN.vertexToFrag6_g1 = IN.customPack2.xyz;
				surfIN.vertexToFrag8_g1 = IN.customPack2.w;
				surfIN.vertexToFrag7_g1 = IN.customPack3.x;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
-1610;238;1302;678;1321.913;87.5443;1.3;True;False
Node;AmplifyShaderEditor.FunctionNode;1;-768,0;Inherit;False;Mud Mesh;0;;1;4f444db5091a94140ab2b15b933d37b6;0;0;17;COLOR;9;FLOAT;13;FLOAT3;10;FLOAT;11;FLOAT;12;FLOAT4;33;FLOAT3;0;FLOAT3;32;FLOAT3;51;FLOAT3;2;FLOAT3;31;FLOAT3;52;FLOAT3;48;FLOAT3;46;FLOAT;45;FLOAT2;15;FLOAT;41
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-256,0;Float;False;True;-1;3;ASEMaterialInspector;0;0;Standard;MudBun/Alpha-Blended Transparent Mesh (Built-In RP);False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Absolute;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;1;9
WireConnection;0;2;1;10
WireConnection;0;3;1;11
WireConnection;0;4;1;12
WireConnection;0;9;1;13
WireConnection;0;11;1;0
WireConnection;0;12;1;2
ASEEND*/
//CHKSM=62546F7042C1CD09519617D77FF6CB107C694451