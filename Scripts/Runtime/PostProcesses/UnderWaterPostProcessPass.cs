#if URP && !FORCE_HDRP
using UnityEngine;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [System.Serializable, PostProcessRenderPass]
    public sealed class UnderWaterPostProcessPass : PostProcessRenderPass<UnderWaterEffectVolume, ExtendedPostProcessRendererFeature>
    {
        public UnderWaterPostProcessPass() : base(Resources.Load<Shader>("Shaders/UnderWater_URP"))
        {
        }

        protected override void SetupMaterial(ExtendedPostProcessRendererFeature rendererFeature)
        {
            _material.SetTexture("_WaveNormal", rendererFeature.WaveTexture);
        }
    }
}
#endif