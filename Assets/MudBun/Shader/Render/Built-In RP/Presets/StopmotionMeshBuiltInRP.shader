// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MudBun/Stopmotion Mesh (Built-In RP)"
{
	Properties
	{
		[HideInInspector]_Color("Color", Color) = (1,1,1,1)
		[HideInInspector]_Emission("Emission", Color) = (1,1,1,1)
		[HideInInspector]_Metallic("Metallic", Range( 0 , 1)) = 0
		[HideInInspector]_Smoothness("Smoothness", Range( 0 , 1)) = 1
		[HideInInspector]_IsMeshRenderMaterial("Is Mesh Render Material", Float) = 1
		_AlphaCutoutThreshold("Alpha Cutout Threshold", Range( 0 , 1)) = 0
		_Dithering("Dithering", Range( 0 , 1)) = 1
		_NoiseSize("Noise Size", Float) = 0.5
		_OffsetAmount("Offset Amount", Float) = 0.005
		_TimeInterval("Time Interval", Float) = 0.15
		[NoScaleOffset]_DisplacementMap("Displacement Map", 2D) = "gray" {}
		_Displacement("Displacement", Float) = 0
		[Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		[NoScaleOffset]_RoughnessMap("Roughness Map", 2D) = "black" {}
		[Toggle]_RandomDither("Random Dither", Range( 0 , 1)) = 0
		[NoScaleOffset]_DitherTexture("Dither Texture", 2D) = "white" {}
		_DitherTextureSize("Dither Texture Size", Int) = 256
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.5
		#define MUDBUN_BUILT_IN_RP
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
			float3 vertexToFrag98;
			float3 vertexToFrag97;
			float4 vertexToFrag5_g300;
			float3 vertexToFrag162;
			float3 vertexToFrag27_g351;
			uint ase_vertexId;
			float3 vertexToFrag6_g300;
			float vertexToFrag8_g300;
			float vertexToFrag7_g300;
		};

		uniform float _Displacement;
		uniform sampler2D _DisplacementMap;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _TimeInterval;
		uniform float _NoiseSize;
		uniform float _OffsetAmount;
		uniform float _IsMeshRenderMaterial;
		uniform sampler2D _RoughnessMap;


		float3 SimplexNoiseGradient6_g350( float3 Position, float Size )
		{
			#ifdef MUDBUN_VALID
			return snoise_grad(Position / max(1e-6, Size)).xyz;
			#else
			return Position;
			#endif
		}


		float4x4 LocalToWorldMatrix167(  )
		{
			return localToWorld;
		}


		void vertexDataFunc( inout appdata_full_custom v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float localMudBunMeshPoint4_g300 = ( 0.0 );
			int VertexID4_g300 = v.ase_vertexId;
			float3 PositionWs4_g300 = float3( 0,0,0 );
			float3 PositionLs4_g300 = float3( 0,0,0 );
			float3 NormalWs4_g300 = float3( 0,0,0 );
			float3 NormalLs4_g300 = float3( 0,0,0 );
			float3 TangentWs4_g300 = float3( 0,0,0 );
			float3 TangentLs4_g300 = float3( 0,0,0 );
			float4 Color4_g300 = float4( 0,0,0,0 );
			float4 EmissionHash4_g300 = float4( 0,0,0,0 );
			float Metallic4_g300 = 0;
			float Smoothness4_g300 = 0;
			float4 TextureWeight4_g300 = float4( 1,0,0,0 );
			float SdfValue4_g300 = 0;
			float3 Outward2dNormalLs4_g300 = float3( 0,0,0 );
			float3 Outward2dNormalWs4_g300 = float3( 0,0,0 );
			{
			float4 positionWs;
			float2 metallicSmoothness;
			mudbun_mesh_vert(VertexID4_g300, positionWs, PositionLs4_g300, NormalWs4_g300, NormalLs4_g300, TangentWs4_g300, TangentLs4_g300, Color4_g300, EmissionHash4_g300, metallicSmoothness, TextureWeight4_g300, SdfValue4_g300, Outward2dNormalLs4_g300, Outward2dNormalWs4_g300);
			PositionWs4_g300 = positionWs.xyz;
			Metallic4_g300 = metallicSmoothness.x;
			Smoothness4_g300 = metallicSmoothness.y;
			#ifdef MUDBUN_BUILT_IN_RP
			#ifndef MUDBUN_VERTEX_SHADER
			v.tangent = float4(TangentWs4_g300, 0.0f);
			#define MUDBUN_VERTEX_SHADER
			#endif
			#endif
			}
			float3 vertexToFrag98 = NormalLs4_g300;
			float3 temp_output_11_0_g346 = abs( vertexToFrag98 );
			float3 break14_g346 = temp_output_11_0_g346;
			float3 temp_output_77_32 = PositionLs4_g300;
			float3 vertexToFrag97 = temp_output_77_32;
			float2 temp_output_35_0_g346 = _NormalMap_ST.xy;
			float2 temp_output_34_0_g346 = _NormalMap_ST.xy;
			float4 appendResult23_g346 = (float4(temp_output_11_0_g346 , 0.0));
			float4 appendResult24_g346 = (float4(temp_output_11_0_g346 , 1.0));
			float4 break10_g347 = ( ( break14_g346.x + break14_g346.y + break14_g346.z ) > 0.0 ? appendResult23_g346 : appendResult24_g346 );
			float4 color20_g346 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float3 temp_output_77_2 = NormalWs4_g300;
			float3 temp_output_77_0 = PositionWs4_g300;
			float2 temp_cast_4 = (floor( ( _Time.y / _TimeInterval ) )).xx;
			float dotResult4_g345 = dot( temp_cast_4 , float2( 12.9898,78.233 ) );
			float lerpResult10_g345 = lerp( 0.0 , 10000.0 , frac( ( sin( dotResult4_g345 ) * 43758.55 ) ));
			float3 Position6_g350 = ( temp_output_77_32 + lerpResult10_g345 );
			float Size6_g350 = _NoiseSize;
			float3 localSimplexNoiseGradient6_g350 = SimplexNoiseGradient6_g350( Position6_g350 , Size6_g350 );
			float3 temp_output_159_0 = ( ( _Displacement * ( (( ( ( ( break14_g346.x > 0.0 ? tex2Dlod( _DisplacementMap, float4( ( ( (vertexToFrag97).yz * temp_output_35_0_g346 ) + temp_output_34_0_g346 ), 0, 0.0) ) : float4( 0,0,0,0 ) ) * break10_g347.x ) + ( ( break14_g346.y > 0.0 ? tex2Dlod( _DisplacementMap, float4( ( ( (vertexToFrag97).zx * temp_output_35_0_g346 ) + temp_output_34_0_g346 ), 0, 0.0) ) : float4( 0,0,0,0 ) ) * break10_g347.y ) + ( ( break14_g346.z > 0.0 ? tex2Dlod( _DisplacementMap, float4( ( ( (vertexToFrag97).xy * temp_output_35_0_g346 ) + temp_output_34_0_g346 ), 0, 0.0) ) : float4( 0,0,0,0 ) ) * break10_g347.z ) + ( color20_g346 * break10_g347.w ) ) / ( break10_g347.x + break10_g347.y + break10_g347.z + break10_g347.w ) )).x - 0.5 ) * temp_output_77_2 ) + ( temp_output_77_0 + ( localSimplexNoiseGradient6_g350 * _OffsetAmount ) ) );
			v.vertex.xyz = temp_output_159_0;
			v.vertex.w = 1;
			v.normal = temp_output_77_2;
			o.vertexToFrag98 = NormalLs4_g300;
			o.vertexToFrag97 = temp_output_77_32;
			o.vertexToFrag5_g300 = Color4_g300;
			float4x4 localLocalToWorldMatrix167 = LocalToWorldMatrix167();
			float4 appendResult168 = (float4(temp_output_159_0 , 1.0));
			float3 finalVertexPositionWs160 = (mul( localLocalToWorldMatrix167, appendResult168 )).xyz;
			o.vertexToFrag162 = finalVertexPositionWs160;
			o.vertexToFrag27_g351 = temp_output_77_0;
			o.ase_vertexId = v.ase_vertexId;
			o.vertexToFrag6_g300 = (EmissionHash4_g300).xyz;
			o.vertexToFrag8_g300 = Metallic4_g300;
			o.vertexToFrag7_g300 = Smoothness4_g300;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 temp_output_11_0_g352 = abs( i.vertexToFrag98 );
			float3 break14_g352 = temp_output_11_0_g352;
			float2 temp_output_35_0_g352 = _NormalMap_ST.xy;
			float2 temp_output_34_0_g352 = _NormalMap_ST.zw;
			float4 appendResult23_g352 = (float4(temp_output_11_0_g352 , 0.0));
			float4 appendResult24_g352 = (float4(temp_output_11_0_g352 , 1.0));
			float4 break10_g353 = ( ( break14_g352.x + break14_g352.y + break14_g352.z ) > 0.0 ? appendResult23_g352 : appendResult24_g352 );
			float4 color20_g352 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			o.Normal = UnpackNormal( ( ( ( ( break14_g352.x > 0.0 ? tex2D( _NormalMap, ( ( (i.vertexToFrag97).yz * temp_output_35_0_g352 ) + temp_output_34_0_g352 ) ) : float4( 0,0,0,0 ) ) * break10_g353.x ) + ( ( break14_g352.y > 0.0 ? tex2D( _NormalMap, ( ( (i.vertexToFrag97).zx * temp_output_35_0_g352 ) + temp_output_34_0_g352 ) ) : float4( 0,0,0,0 ) ) * break10_g353.y ) + ( ( break14_g352.z > 0.0 ? tex2D( _NormalMap, ( ( (i.vertexToFrag97).xy * temp_output_35_0_g352 ) + temp_output_34_0_g352 ) ) : float4( 0,0,0,0 ) ) * break10_g353.z ) + ( color20_g352 * break10_g353.w ) ) / ( break10_g353.x + break10_g353.y + break10_g353.z + break10_g353.w ) ) );
			float4 temp_output_25_0_g300 = ( _IsMeshRenderMaterial * _Color * i.vertexToFrag5_g300 );
			float localComputeOpaqueTransparency20_g351 = ( 0.0 );
			float4 unityObjectToClipPos1_g354 = UnityObjectToClipPos( i.vertexToFrag162 );
			float4 computeScreenPos3_g354 = ComputeScreenPos( unityObjectToClipPos1_g354 );
			float2 ScreenPos20_g351 = (( ( computeScreenPos3_g354 / (computeScreenPos3_g354).w ) * _ScreenParams )).xy;
			float3 VertPos20_g351 = i.vertexToFrag27_g351;
			float localMudBunMeshPoint4_g300 = ( 0.0 );
			int VertexID4_g300 = i.ase_vertexId;
			float3 PositionWs4_g300 = float3( 0,0,0 );
			float3 PositionLs4_g300 = float3( 0,0,0 );
			float3 NormalWs4_g300 = float3( 0,0,0 );
			float3 NormalLs4_g300 = float3( 0,0,0 );
			float3 TangentWs4_g300 = float3( 0,0,0 );
			float3 TangentLs4_g300 = float3( 0,0,0 );
			float4 Color4_g300 = float4( 0,0,0,0 );
			float4 EmissionHash4_g300 = float4( 0,0,0,0 );
			float Metallic4_g300 = 0;
			float Smoothness4_g300 = 0;
			float4 TextureWeight4_g300 = float4( 1,0,0,0 );
			float SdfValue4_g300 = 0;
			float3 Outward2dNormalLs4_g300 = float3( 0,0,0 );
			float3 Outward2dNormalWs4_g300 = float3( 0,0,0 );
			{
			float4 positionWs;
			float2 metallicSmoothness;
			mudbun_mesh_vert(VertexID4_g300, positionWs, PositionLs4_g300, NormalWs4_g300, NormalLs4_g300, TangentWs4_g300, TangentLs4_g300, Color4_g300, EmissionHash4_g300, metallicSmoothness, TextureWeight4_g300, SdfValue4_g300, Outward2dNormalLs4_g300, Outward2dNormalWs4_g300);
			PositionWs4_g300 = positionWs.xyz;
			Metallic4_g300 = metallicSmoothness.x;
			Smoothness4_g300 = metallicSmoothness.y;
			#ifdef MUDBUN_BUILT_IN_RP
			#ifndef MUDBUN_VERTEX_SHADER
			v.tangent = float4(TangentWs4_g300, 0.0f);
			#define MUDBUN_VERTEX_SHADER
			#endif
			#endif
			}
			float Hash20_g351 = (EmissionHash4_g300).w;
			float AlphaIn20_g351 = (temp_output_25_0_g300).a;
			float AlphaOut20_g351 = 0;
			float AlphaThreshold20_g351 = 0;
			sampler2D DitherNoiseTexture20_g351 = _DitherTexture;
			int DitherNoiseTextureSize20_g351 = _DitherTextureSize;
			int UseRandomDither20_g351 = (int)_RandomDither;
			float AlphaCutoutThreshold20_g351 = _AlphaCutoutThreshold;
			float DitherBlend20_g351 = _Dithering;
			{
			float alpha = AlphaIn20_g351;
			computeOpaqueTransparency(ScreenPos20_g351, VertPos20_g351, Hash20_g351, DitherNoiseTexture20_g351, DitherNoiseTextureSize20_g351, UseRandomDither20_g351 > 0, AlphaCutoutThreshold20_g351, DitherBlend20_g351,  alpha, AlphaThreshold20_g351);
			AlphaOut20_g351 = alpha;
			}
			clip( AlphaOut20_g351 - AlphaThreshold20_g351);
			o.Albedo = temp_output_25_0_g300.rgb;
			o.Emission = ( i.vertexToFrag6_g300 * (_Emission).rgb );
			o.Metallic = ( _Metallic * i.vertexToFrag8_g300 );
			float3 temp_output_11_0_g348 = abs( i.vertexToFrag98 );
			float3 break14_g348 = temp_output_11_0_g348;
			float2 temp_output_35_0_g348 = _NormalMap_ST.xy;
			float2 temp_output_34_0_g348 = _NormalMap_ST.zw;
			float4 appendResult23_g348 = (float4(temp_output_11_0_g348 , 0.0));
			float4 appendResult24_g348 = (float4(temp_output_11_0_g348 , 1.0));
			float4 break10_g349 = ( ( break14_g348.x + break14_g348.y + break14_g348.z ) > 0.0 ? appendResult23_g348 : appendResult24_g348 );
			float4 color20_g348 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			o.Smoothness = ( ( 1.0 - (( ( ( ( break14_g348.x > 0.0 ? tex2D( _RoughnessMap, ( ( (i.vertexToFrag97).yz * temp_output_35_0_g348 ) + temp_output_34_0_g348 ) ) : float4( 0,0,0,0 ) ) * break10_g349.x ) + ( ( break14_g348.y > 0.0 ? tex2D( _RoughnessMap, ( ( (i.vertexToFrag97).zx * temp_output_35_0_g348 ) + temp_output_34_0_g348 ) ) : float4( 0,0,0,0 ) ) * break10_g349.y ) + ( ( break14_g348.z > 0.0 ? tex2D( _RoughnessMap, ( ( (i.vertexToFrag97).xy * temp_output_35_0_g348 ) + temp_output_34_0_g348 ) ) : float4( 0,0,0,0 ) ) * break10_g349.z ) + ( color20_g348 * break10_g349.w ) ) / ( break10_g349.x + break10_g349.y + break10_g349.z + break10_g349.w ) )).x ) * ( _Smoothness * i.vertexToFrag7_g300 ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
-1610;238;1302;678;591.1387;1352.462;1.936978;True;False
Node;AmplifyShaderEditor.FunctionNode;77;-1408,0;Inherit;False;Mud Mesh;1;;300;4f444db5091a94140ab2b15b933d37b6;0;0;17;COLOR;9;FLOAT;13;FLOAT3;10;FLOAT;11;FLOAT;12;FLOAT4;33;FLOAT3;0;FLOAT3;32;FLOAT3;2;FLOAT3;31;FLOAT3;53;FLOAT3;52;FLOAT3;48;FLOAT3;46;FLOAT;45;FLOAT2;15;FLOAT;41
Node;AmplifyShaderEditor.RangedFloatNode;23;-1408,768;Inherit;False;Property;_TimeInterval;Time Interval;11;0;Create;True;0;0;0;False;0;False;0.15;3.402823E+38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;22;-1408,672;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexToFragmentNode;98;-896,-64;Inherit;False;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;105;-1152,-512;Inherit;True;Property;_NormalMap;Normal Map;14;1;[Normal];Create;True;0;0;0;False;0;False;None;679204acdc00b564398a68f691979695;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-1152,704;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexToFragmentNode;97;-896,-144;Inherit;False;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureTransformNode;146;-896,-384;Inherit;False;-1;False;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.FloorOpNode;26;-992,704;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;150;-512,-928;Inherit;False;FLOAT2;1;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;153;-1152,-1024;Inherit;True;Property;_DisplacementMap;Displacement Map;12;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;8fb1a6acf59188448bca62119afcccde;False;gray;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SwizzleNode;147;-512,-736;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;148;-512,-640;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwizzleNode;149;-512,-832;Inherit;False;FLOAT2;2;0;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;27;-832,704;Inherit;False;Random Range;-1;;345;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;10000;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;151;-256,-1024;Inherit;False;Mud Triplanar Sample;-1;;346;d9088f0d6015c424b98757b174010394;0;8;3;SAMPLER2D;0,0,0;False;26;SAMPLERSTATE;0,0,0;False;35;FLOAT2;0,0;False;34;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;11;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1408,480;Inherit;False;Property;_NoiseSize;Noise Size;9;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1408,576;Inherit;False;Property;_OffsetAmount;Offset Amount;10;0;Create;True;0;0;0;False;0;False;0.005;0.002;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-256,608;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;156;32,-1024;Inherit;False;True;False;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;65;-64,560;Inherit;False;Mud Noise Gradient;-1;;350;ded4656e0e0531448b1f2a26fd64d584;0;3;2;FLOAT3;0,0,0;False;5;FLOAT;0.1;False;7;FLOAT;0.1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;157;256,-1120;Inherit;False;Property;_Displacement;Displacement;13;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;155;256,-1024;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;512,-1024;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;256,512;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;159;768,-1024;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomExpressionNode;167;976,-1152;Inherit;False;return localToWorld@;6;Create;0;Local To World Matrix;True;False;0;;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.DynamicAppendNode;168;1024,-1056;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;1168,-1152;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;165;1328,-1152;Inherit;False;True;True;True;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;160;1536,-1152;Inherit;False;finalVertexPositionWs;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwizzleNode;136;-512,-1248;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;127;-512,-1344;Inherit;False;FLOAT2;2;0;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;128;-512,-1440;Inherit;False;FLOAT2;1;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;129;-512,-1152;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;163;-1408,864;Inherit;False;160;finalVertexPositionWs;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;130;-896,-1536;Inherit;True;Property;_RoughnessMap;Roughness Map;15;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;5b4f3b34a6be3bd4585c339dff8d1a37;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.VertexToFragmentNode;162;-1152,864;Inherit;False;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;145;-256,-1536;Inherit;False;Mud Triplanar Sample;-1;;348;d9088f0d6015c424b98757b174010394;0;8;3;SAMPLER2D;0,0,0;False;26;SAMPLERSTATE;0,0,0;False;35;FLOAT2;0,0;False;34;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;11;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SwizzleNode;92;-512,-416;Inherit;False;FLOAT2;1;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;161;-928,864;Inherit;False;World To Screen;-1;;354;50b3ac8846f702445a58bf980e772412;0;1;8;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1424,1280;Inherit;False;Property;_RandomDither;Random Dither;16;1;[Toggle];Fetch;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;95;-512,-128;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.IntNode;8;-1424,1184;Inherit;False;Property;_DitherTextureSize;Dither Texture Size;18;0;Fetch;True;0;0;0;False;0;False;256;256;False;0;1;INT;0
Node;AmplifyShaderEditor.ComponentMaskNode;132;32,-1536;Inherit;False;True;False;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1424,1472;Inherit;False;Property;_Dithering;Dithering;8;0;Fetch;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1424,1376;Inherit;False;Property;_AlphaCutoutThreshold;Alpha Cutout Threshold;7;0;Fetch;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;93;-512,-320;Inherit;False;FLOAT2;2;0;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;94;-512,-224;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-1424,960;Inherit;True;Property;_DitherTexture;Dither Texture;17;1;[NoScaleOffset];Fetch;True;0;0;0;False;0;False;f240bbb7854046345b218811e5681a54;f240bbb7854046345b218811e5681a54;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.OneMinusNode;139;256,-1536;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;9;-400,768;Inherit;False;Mud Alpha Threshold;-1;;351;926535703f4c32948ac1f55275a22bf0;0;9;8;FLOAT2;0,0;False;15;FLOAT3;0,0,0;False;18;FLOAT;0;False;22;FLOAT;0;False;19;SAMPLER2D;0;False;26;INT;256;False;9;INT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;2;FLOAT;24;FLOAT;25
Node;AmplifyShaderEditor.FunctionNode;144;-256,-512;Inherit;False;Mud Triplanar Sample;-1;;352;d9088f0d6015c424b98757b174010394;0;8;3;SAMPLER2D;0,0,0;False;26;SAMPLERSTATE;0,0,0;False;35;FLOAT2;0,0;False;34;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;11;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;82;32,-512;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClipNode;16;544,512;Inherit;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;416,-1536;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1024,0;Float;False;True;-1;3;ASEMaterialInspector;0;0;Standard;MudBun/Stopmotion Mesh (Built-In RP);False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Absolute;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;1;Define;MUDBUN_BUILT_IN_RP;False;;Custom;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;98;0;77;31
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;97;0;77;32
WireConnection;146;0;105;0
WireConnection;26;0;24;0
WireConnection;150;0;97;0
WireConnection;147;0;97;0
WireConnection;148;0;98;0
WireConnection;149;0;97;0
WireConnection;27;1;26;0
WireConnection;151;3;153;0
WireConnection;151;26;153;1
WireConnection;151;35;146;0
WireConnection;151;34;146;0
WireConnection;151;4;150;0
WireConnection;151;5;149;0
WireConnection;151;6;147;0
WireConnection;151;11;148;0
WireConnection;28;0;77;32
WireConnection;28;1;27;0
WireConnection;156;0;151;0
WireConnection;65;2;28;0
WireConnection;65;5;11;0
WireConnection;65;7;19;0
WireConnection;155;0;156;0
WireConnection;158;0;157;0
WireConnection;158;1;155;0
WireConnection;158;2;77;2
WireConnection;14;0;77;0
WireConnection;14;1;65;0
WireConnection;159;0;158;0
WireConnection;159;1;14;0
WireConnection;168;0;159;0
WireConnection;166;0;167;0
WireConnection;166;1;168;0
WireConnection;165;0;166;0
WireConnection;160;0;165;0
WireConnection;136;0;97;0
WireConnection;127;0;97;0
WireConnection;128;0;97;0
WireConnection;129;0;98;0
WireConnection;162;0;163;0
WireConnection;145;3;130;0
WireConnection;145;26;130;1
WireConnection;145;35;146;0
WireConnection;145;34;146;1
WireConnection;145;4;128;0
WireConnection;145;5;127;0
WireConnection;145;6;136;0
WireConnection;145;11;129;0
WireConnection;92;0;97;0
WireConnection;161;8;162;0
WireConnection;95;0;98;0
WireConnection;132;0;145;0
WireConnection;93;0;97;0
WireConnection;94;0;97;0
WireConnection;139;0;132;0
WireConnection;9;8;161;0
WireConnection;9;15;77;0
WireConnection;9;18;77;41
WireConnection;9;22;77;13
WireConnection;9;19;4;0
WireConnection;9;26;8;0
WireConnection;9;9;6;0
WireConnection;9;6;7;0
WireConnection;9;7;5;0
WireConnection;144;3;105;0
WireConnection;144;26;105;1
WireConnection;144;35;146;0
WireConnection;144;34;146;1
WireConnection;144;4;92;0
WireConnection;144;5;93;0
WireConnection;144;6;94;0
WireConnection;144;11;95;0
WireConnection;82;0;144;0
WireConnection;16;0;77;9
WireConnection;16;1;9;24
WireConnection;16;2;9;25
WireConnection;138;0;139;0
WireConnection;138;1;77;12
WireConnection;0;0;16;0
WireConnection;0;1;82;0
WireConnection;0;2;77;10
WireConnection;0;3;77;11
WireConnection;0;4;138;0
WireConnection;0;11;159;0
WireConnection;0;12;77;2
ASEEND*/
//CHKSM=FBF67DBCF70537BE5B6B370595D83A891CD0CCAC