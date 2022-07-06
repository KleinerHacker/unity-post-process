#if URP && !FORCE_HDRP
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Utils.Extensions;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base
{
    [Serializable]
    public abstract class PostProcessRenderPass : ScriptableRenderPass
    {
        public abstract void SetupMaterial(PostProcessRendererFeature rendererFeature);
    }

    [Serializable]
    public abstract class PostProcessRenderPass<T, TFeature> : PostProcessRenderPass where T : PostProcessVolumeComponent where TFeature : PostProcessRendererFeature
    {
        // Used to render from camera to post processing
        // back and forth, until we render the final image to
        // the camera
        private RenderTargetIdentifier _source;
        private RenderTargetIdentifier _destinationA;
        private RenderTargetIdentifier _destinationB;
        private RenderTargetIdentifier _latestDest;

        private readonly int _temporaryRTIdA = Shader.PropertyToID("_TempRT");
        private readonly int _temporaryRTIdB = Shader.PropertyToID("_TempRTB");

        protected readonly Material _material;
        private readonly PostProcessRenderPassAttribute _attribute;

        public PostProcessRenderPass(Shader shader)
        {
            _attribute = GetType().GetCustomAttribute<PostProcessRenderPassAttribute>();
            if (_attribute == null)
                throw new InvalidOperationException("Unable to find required attribute " + nameof(PostProcessRenderPassAttribute) + " on class " + GetType().Name);
            
            // Set the render pass event
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            _material = CoreUtils.CreateEngineMaterial(shader);
            if (_material == null)
            {
                Debug.LogError("Shader cannot convert to material!");
            }
            else
            {
                _material.renderQueue = _attribute.RenderQueue;
            }
        }

        public override void SetupMaterial(PostProcessRendererFeature rendererFeature) => SetupMaterial((TFeature) rendererFeature);

        protected abstract void SetupMaterial(TFeature rendererFeature);

        public sealed override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Grab the camera target descriptor. We will use this when creating a temporary render texture.
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            var renderer = renderingData.cameraData.renderer;
            _source = renderer.cameraColorTarget;

            // Create a temporary render texture using the descriptor from above.
            cmd.GetTemporaryRT(_temporaryRTIdA, descriptor, FilterMode.Bilinear);
            _destinationA = new RenderTargetIdentifier(_temporaryRTIdA);
            cmd.GetTemporaryRT(_temporaryRTIdB, descriptor, FilterMode.Bilinear);
            _destinationB = new RenderTargetIdentifier(_temporaryRTIdB);
        }

        // The actual execution of the pass. This is where custom rendering occurs.
        public sealed override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null)
                return;
            if (!_attribute.ShowInScene && renderingData.cameraData.isSceneViewCamera)
                return;
            if (!renderingData.postProcessingEnabled || !renderingData.cameraData.postProcessEnabled)
                return;

            var cmd = CommandBufferPool.Get("Miko Kyra Post Processing");
            cmd.Clear();

            // This holds all the current Volumes information
            // which we will need later
            var stack = VolumeManager.instance.stack;

            // Starts with the camera source
            _latestDest = _source;

            //---Custom effect here---
            var myEffect = stack.GetComponent<T>();
            // Only process if the effect is active
            if (myEffect != null && myEffect.IsActive())
            {
                UpdateMaterialData(myEffect, _material);
                BlitTo(_material);
            }

            // Add any other custom effect/component you want, in your preferred order
            // Custom effect 2, 3 , ...


            // DONE! Now that we have processed all our custom effects, applies the final result to camera
            Blit(cmd, _latestDest, _source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            #region Local Methods

            // Swaps render destinations back and forth, so that
            // we can have multiple passes and similar with only a few textures
            void BlitTo(Material mat, int pass = 0)
            {
                var first = _latestDest;
                var last = first == _destinationA ? _destinationB : _destinationA;
                Blit(cmd, first, last, mat, pass);

                _latestDest = last;
            }

            #endregion
        }

        //Cleans the temporary RTs when we don't need them anymore
        public sealed override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_temporaryRTIdA);
            cmd.ReleaseTemporaryRT(_temporaryRTIdB);
        }

        protected virtual void UpdateMaterialData(T myEffect, Material material)
        {
            foreach (var fieldData in myEffect.FieldDataList)
            {
                material.SetProperty(fieldData.KeyID, fieldData.FieldInfo.GetValue(myEffect), fieldData.Type);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PostProcessRenderPassAttribute : Attribute
    {
        public bool ShowInScene { get; set; } = true;
        public int RenderQueue { get; set; } = (int)UnityEngine.Rendering.RenderQueue.Overlay;
    }
}
#endif