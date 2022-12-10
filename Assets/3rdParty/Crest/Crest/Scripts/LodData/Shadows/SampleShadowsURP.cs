// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

#if CREST_URP

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Crest
{
    public class SampleShadowsURP : ScriptableRenderPass
    {
        static SampleShadowsURP _instance;
        public static bool Created => _instance != null;

        public SampleShadowsURP(RenderPassEvent renderPassEvent)
        {
            this.renderPassEvent = renderPassEvent;
        }

        public static void Enable()
        {
            if (_instance == null)
            {
                _instance = new SampleShadowsURP(RenderPassEvent.AfterRenderingSkybox);
            }

            RenderPipelineManager.beginCameraRendering -= EnqueueSampleShadowPass;
            RenderPipelineManager.beginCameraRendering += EnqueueSampleShadowPass;
        }

        public static void Disable()
        {
            RenderPipelineManager.beginCameraRendering -= EnqueueSampleShadowPass;
        }

        public static void EnqueueSampleShadowPass(ScriptableRenderContext context, Camera camera)
        {
            var ocean = OceanRenderer.Instance;

            if (ocean == null || ocean._lodDataShadow == null)
            {
                return;
            }

            // Only sample shadows for the main camera.
            if (!ReferenceEquals(ocean.ViewCamera, camera))
            {
                return;
            }

            if (camera.TryGetComponent<UniversalAdditionalCameraData>(out var cameraData))
            {
                cameraData.scriptableRenderer.EnqueuePass(_instance);
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var ocean = OceanRenderer.Instance;

            if (ocean == null || ocean._lodDataShadow == null)
            {
                return;
            }

            // TODO: This may not be the same as OceanRenderer._primaryLight. Not certain how to support overriding the
            // main light for shadows yet.
            var mainLightIndex = renderingData.lightData.mainLightIndex;

            if (mainLightIndex == -1)
            {
                return;
            }

            // No shadow caster in view. Shadow map will be unbound.
            if (!renderingData.cullResults.GetShadowCasterBounds(mainLightIndex, out _))
            {
                return;
            }

            var camera = renderingData.cameraData.camera;

            var buffer = CommandBufferPool.Get("Crest Shadow Data");

            // Disable for XR SPI otherwise input will not have correct world position.
            if (renderingData.cameraData.xrRendering && XRHelpers.IsSinglePass)
            {
                buffer.DisableShaderKeyword("STEREO_INSTANCING_ON");
            }

            ocean._lodDataShadow.BuildCommandBuffer(ocean, buffer);

            // Restore matrices otherwise remaining render will have incorrect matrices. Each pass is responsible for
            // restoring matrices if required.
            buffer.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);

            // Restore XR SPI as we cannot rely on remaining pipeline to do it for us.
            if (renderingData.cameraData.xrRendering && XRHelpers.IsSinglePass)
            {
                buffer.EnableShaderKeyword("STEREO_INSTANCING_ON");
            }

            context.ExecuteCommandBuffer(buffer);
            CommandBufferPool.Release(buffer);
        }
    }
}

#endif // CREST_URP
