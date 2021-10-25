﻿/******************************************************************************/
/*
  Project   - MudBun
  Publisher - Long Bunny Labs
              http://LongBunnyLabs.com
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

Shader "MudBun/Mud Mesh Single-Textured (Built-In RP)"
{
  Properties
  {
    _AlphaCutoutThreshold("Alpha Cutout Threshold", Range(0.0, 1.0)) = 0.5
    _Dithering("Dithering", Range(0.0, 1.0)) = 0.0
    _DitherTexture("Dither Texture", 2D) = "black"
    _DitherTextureSize("Dither TextureSize", Int) = 256
    [Toggle] _RandomDither("Random Dither", Int) = 0

    [Toggle] _UseTex0("Use Texture", Int) = 0
      _MainTex("Albedo", 2D) = "white" {}
      [Toggle] _MainTexX("     X Axis Projection", Int) = 1
      [Toggle] _MainTexY("     Y Axis Projection", Int) = 1
      [Toggle] _MainTexZ("     Z Axis Projection", Int) = 1

    [Toggle] _UseNorm0("Use Normal Map", Int) = 0
      _MainNorm("Albedo", 2D) = "normal" {}
      [Toggle] _MainNormX("     X Axis Projection", Int) = 1
      [Toggle] _MainNormY("     Y Axis Projection", Int) = 1
      [Toggle] _MainNormZ("     Z Axis Projection", Int) = 1
  }
  SubShader
  {
    ZWrite On
    Cull Back
    Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }

    CGPROGRAM

    #define MUDBUN_BUILT_IN_RP
    #pragma multi_compile_instancing
    #pragma multi_compile _ MUDBUN_PROCEDURAL
    #pragma surface surf Standard vertex:vert addshadow fullforwardshadows
    #pragma target 3.5

    #include "UnityCG.cginc"

    #include "../../../Shader/Render/ShaderCommon.cginc"

    #if MUDBUN_VALID
      #include "../../../Shader/Render/MeshCommon.cginc"
    #endif

    void vert(inout Vertex i, out Input o)
    {
      UNITY_INITIALIZE_OUTPUT(Input, o);

      #if MUDBUN_VALID
        float sdfValue;
        float3 tangentWs;
        float3 tangentLs;
        float3 normal2dLs;
        float3 normal2dWs;
        mudbun_mesh_vert(i.id, i.vertex, o.localPos, i.normal, o.localNorm, tangentWs, tangentLs, i.color, o.emissionHash, o.metallicSmoothness, o.texWeight, sdfValue, normal2dLs, normal2dWs);
        i.tangent = o.tangent = float4(tangentWs, 0.0f);
      #endif
    }

    void surf(Input i, inout SurfaceOutputStandard o)
    {
      float4 color = 1.0f;

      float4 texColor = 0.0f;
      float totalTexWeight = 0.0f;

      float4 normColor = 0.0f;
      float totalNormWeight = 0.0f;

      float3 triWeight = abs(i.localNorm);

      if (_UseTex0)
      {
        texColor += tex2D_triplanar(_MainTex, _MainTex_ST, triWeight, i.localPos, _MainTexX, _MainTexY, _MainTexZ);
        totalTexWeight += 1.0f;
      }

      if (totalTexWeight > 0.0f)
      {
        color = texColor / totalTexWeight;
      }

      if (_UseNorm0)
      {
        normColor += tex2D_triplanar(_MainNorm, _MainNorm_ST, triWeight, i.localPos, _MainNormX, _MainNormY, _MainNormZ);
        totalNormWeight += 1.0f;
      }

      if (totalNormWeight > 0.0f)
      {
        o.Normal = UnpackNormal(normColor / totalNormWeight);
      }

      float3 albedo = i.color.rgb * _Color.rgb * color.rgb;
      float alpha = i.color.a * _Color.a * color.a;
      float alphaThreshold;
      float2 screenPos = i.screenPos.xy * _ScreenParams.xy / (i.screenPos.w + kEpsilon);
      computeOpaqueTransparency(screenPos, i.localPos, i.emissionHash.a, _DitherTexture, _DitherTextureSize, _RandomDither > 0, _AlphaCutoutThreshold, _Dithering, alpha, alphaThreshold);
      clip(alpha - alphaThreshold);

      o.Albedo = albedo;
      o.Emission = float4(i.emissionHash.rgb, 1.0f)  * _Emission;
      o.Metallic = i.metallicSmoothness.x * _Metallic;
      o.Smoothness = i.metallicSmoothness.y * _Smoothness;
    }

    ENDCG
  }

  CustomEditor "MudBun.MudMeshSingleTexturedMaterialEditor"
}
