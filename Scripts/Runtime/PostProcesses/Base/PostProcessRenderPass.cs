#if URP && !FORCE_HDRP
using System;
using System.Linq;
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
    public abstract class PostProcessRenderPass<TFeature> : PostProcessRenderPass where TFeature : PostProcessRendererFeature
    {
        // Used to render from camera to post processing
        // back and forth, until we render the final image to
        // the camera
        private RenderTargetIdentifier _source;

        private readonly ShaderData[] _materials;
        private readonly PostProcessRenderPassAttribute _attribute;

        public PostProcessRenderPass(params (string, Shader)[] shaders)
        {
            _attribute = GetType().GetCustomAttribute<PostProcessRenderPassAttribute>();
            if (_attribute == null)
                throw new InvalidOperationException("Unable to find required attribute " + nameof(PostProcessRenderPassAttribute) + " on class " + GetType().Name);

            // Set the render pass event
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            _materials = new ShaderData[shaders.Length];
            for (var i = 0; i < shaders.Length; i++)
            {
                _materials[i] = new ShaderData(shaders[i].Item1, shaders[i].Item2, CoreUtils.CreateEngineMaterial(shaders[i].Item2));
                if (_materials[i] == null)
                {
                    Debug.LogError("Shader cannot convert to material: " + shaders[i].Item1);
                }
                else
                {
                    _materials[i].Material.renderQueue = _attribute.RenderQueue;
                }
            }
        }

        public override void SetupMaterial(PostProcessRendererFeature rendererFeature)
        {
            foreach (var material in _materials)
            {
                SetupMaterial((TFeature)rendererFeature, material);
            }
        }

        protected abstract void SetupMaterial(TFeature rendererFeature, ShaderData data);

        public sealed override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Grab the camera target descriptor. We will use this when creating a temporary render texture.
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            var renderer = renderingData.cameraData.renderer;
            _source = renderer.cameraColorTarget;
        }

        // The actual execution of the pass. This is where custom rendering occurs.
        public sealed override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_materials is not { Length: > 0 })
                return;
            if (!_attribute.ShowInScene && renderingData.cameraData.isSceneViewCamera)
                return;
            if (!renderingData.postProcessingEnabled || !renderingData.cameraData.postProcessEnabled)
                return;

            var cmd = CommandBufferPool.Get();
            cmd.Clear();

            // This holds all the current Volumes information
            // which we will need later
            var stack = VolumeManager.instance.stack;
            
            foreach (var material in _materials)
            {
                var myEffect = GetEffect(stack, material);
                // Only process if the effect is active
                if (myEffect != null && myEffect.IsActive())
                {
                    UpdateMaterialData(myEffect, material.Material);
                    Blit(cmd, _source, _source, material.Material);
                }
            }

            // DONE! Now that we have processed all our custom effects, applies the final result to camera
            //Blit(cmd, _levels.Last(), _source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        //Cleans the temporary RTs when we don't need them anymore
        public sealed override void OnCameraCleanup(CommandBuffer cmd)
        {
        }

        protected virtual void UpdateMaterialData(PostProcessVolumeComponent myEffect, Material material)
        {
            foreach (var fieldData in myEffect.FieldDataList)
            {
                material.SetProperty(fieldData.KeyID, fieldData.FieldInfo.GetValue(myEffect), fieldData.Type);
            }
        }

        protected abstract PostProcessVolumeComponent GetEffect(VolumeStack stack, ShaderData data);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PostProcessRenderPassAttribute : Attribute
    {
        public bool ShowInScene { get; set; } = true;
        public int RenderQueue { get; set; } = (int)UnityEngine.Rendering.RenderQueue.Overlay;
    }

    public record ShaderData
    {
        public string Identifier { get; }
        public Shader Shader { get; }
        public Material Material { get; }

        public ShaderData(string identifier, Shader shader, Material material)
        {
            Identifier = identifier;
            Shader = shader;
            Material = material;
        }
    }
}
#endif