// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

namespace Crest
{
    using UnityEngine;
    using UnityEngine.Rendering;
#if CREST_HDRP
    using UnityEngine.Rendering.HighDefinition;
#endif

    /// <summary>
    /// General purpose helpers which, at the moment, do not warrant a seperate file.
    /// </summary>
    public static class Helpers
    {
        public static bool IsMSAAEnabled(Camera camera)
        {
#if CREST_HDRP
            if (RenderPipelineHelper.IsHighDefinition)
            {
                var hdCamera = HDCamera.GetOrCreate(camera);
                return hdCamera.msaaSamples != UnityEngine.Rendering.MSAASamples.None && hdCamera.frameSettings.IsEnabled(FrameSettingsField.MSAA);
            }
            else
#endif
            {
                return camera.allowMSAA && QualitySettings.antiAliasing > 0f;
            }
        }

        public static bool IsMotionVectorsEnabled()
        {
#if CREST_HDRP
            if (RenderPipelineHelper.IsHighDefinition)
            {
                // Only check the RP asset for now. This can happen at run-time, but a developer should not change the
                // quality setting when performance matters like gameplay.
                return (GraphicsSettings.currentRenderPipeline as HDRenderPipelineAsset)
                    .currentPlatformRenderPipelineSettings.supportMotionVectors;
            }
#endif // CREST_HDRP

            // Default to false until we support MVs.
            return false;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            var temp = b;
            b = a;
            a = temp;
        }
    }

    static class Extensions
    {
        public static void SetKeyword(this Material material, string keyword, bool enabled)
        {
            if (enabled)
            {
                material.EnableKeyword(keyword);
            }
            else
            {
                material.DisableKeyword(keyword);
            }
        }

        public static void SetKeyword(this ComputeShader shader, string keyword, bool enabled)
        {
            if (enabled)
            {
                shader.EnableKeyword(keyword);
            }
            else
            {
                shader.DisableKeyword(keyword);
            }
        }

        public static void SetShaderKeyword(this CommandBuffer buffer, string keyword, bool enabled)
        {
            if (enabled)
            {
                buffer.EnableShaderKeyword(keyword);
            }
            else
            {
                buffer.DisableShaderKeyword(keyword);
            }
        }
    }
}
