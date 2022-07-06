using UnityEngine;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [System.Serializable, PostProcessRenderPass]
    public class ColorChangeInDepthPostProcessPass : PostProcessRenderPass<ColorChangeInDepthEffectVolume, ExtendedPostProcessRendererFeature>
    {
        public ColorChangeInDepthPostProcessPass() : base(Resources.Load<Shader>("Shaders/ColorChangeInDepth_URP"))
        {
        }

        protected override void SetupMaterial(ExtendedPostProcessRendererFeature rendererFeature)
        {
        }
    }
}