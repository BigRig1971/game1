// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

#if CREST_URP

namespace Crest
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    internal class UnderwaterMaskPassURP : ScriptableRenderPass
    {
        const string k_ShaderPathOceanMask = "Hidden/Crest/Underwater/Ocean Mask URP";

        readonly PropertyWrapperMaterial _oceanMaskMaterial;

        static UnderwaterMaskPassURP s_instance;
        UnderwaterRenderer _underwaterRenderer;

        public UnderwaterMaskPassURP()
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
            _oceanMaskMaterial = new PropertyWrapperMaterial(k_ShaderPathOceanMask);
        }

        ~UnderwaterMaskPassURP()
        {
            CoreUtils.Destroy(_oceanMaskMaterial.material);
        }

        public static void Enable(UnderwaterRenderer underwaterRenderer)
        {
            if (s_instance == null)
            {
                s_instance = new UnderwaterMaskPassURP();
            }

            UnderwaterRenderer.Instance.SetUpFixMaskArtefactsShader();

            s_instance._underwaterRenderer = underwaterRenderer;

            RenderPipelineManager.beginCameraRendering -= EnqueuePass;
            RenderPipelineManager.beginCameraRendering += EnqueuePass;
        }

        public static void Disable()
        {
            RenderPipelineManager.beginCameraRendering -= EnqueuePass;
        }

        static void EnqueuePass(ScriptableRenderContext context, Camera camera)
        {
            if (!s_instance._underwaterRenderer.IsActive)
            {
                return;
            }

            // Only support main camera for now.
            if (!ReferenceEquals(OceanRenderer.Instance.ViewCamera, camera))
            {
                return;
            }

            // Only support game cameras for now.
            if (camera.cameraType != CameraType.Game)
            {
                return;
            }

            // Enqueue the pass. This happens every frame.
            if (camera.TryGetComponent<UniversalAdditionalCameraData>(out var cameraData))
            {
                cameraData.scriptableRenderer.EnqueuePass(s_instance);
            }
        }

        // Called before Configure.
        public override void OnCameraSetup(CommandBuffer buffer, ref RenderingData renderingData)
        {
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            UnderwaterRenderer.SetUpMaskTextures(buffer, descriptor);
        }

        // Called before Execute.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            ConfigureTarget(UnderwaterRenderer.Instance._maskTarget, UnderwaterRenderer.Instance._depthTarget);
            ConfigureClear(ClearFlag.All, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var camera = renderingData.cameraData.camera;

            XRHelpers.Update(camera);
            XRHelpers.UpdatePassIndex(ref UnderwaterRenderer.s_xrPassIndex);

            CommandBuffer commandBuffer = CommandBufferPool.Get("Ocean Mask");

            commandBuffer.SetGlobalTexture(UnderwaterRenderer.sp_CrestOceanMaskTexture, UnderwaterRenderer.Instance._maskTarget);
            commandBuffer.SetGlobalTexture(UnderwaterRenderer.sp_CrestOceanMaskDepthTexture, UnderwaterRenderer.Instance._depthTarget);

            UnderwaterRenderer.PopulateOceanMask(
                commandBuffer,
                camera,
                OceanRenderer.Instance.Tiles,
                _underwaterRenderer._cameraFrustumPlanes,
                _oceanMaskMaterial.material,
                _underwaterRenderer._farPlaneMultiplier,
                _underwaterRenderer._debug._disableOceanMask
            );

            UnderwaterRenderer.Instance.FixMaskArtefacts
            (
                commandBuffer,
                renderingData.cameraData.cameraTargetDescriptor,
                UnderwaterRenderer.Instance._maskTarget
            );

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }
    }
}

#endif // CREST_URP
