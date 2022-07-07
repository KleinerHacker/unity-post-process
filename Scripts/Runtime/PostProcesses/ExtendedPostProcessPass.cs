#if URP && !FORCE_HDRP
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [System.Serializable, PostProcessRenderPass]
    public sealed class ExtendedPostProcessPass : PostProcessRenderPass<ExtendedPostProcessRendererFeature>
    {
        private const string KeyUnderWater = "under-water";
        private const string KeyBlackWhite = "black-white";
        private const string KeyPixelate = "pixelate";

        public ExtendedPostProcessPass() : base(
            (KeyBlackWhite, Resources.Load<Shader>("Shaders/BlackWhite_URP")),
            (KeyUnderWater, Resources.Load<Shader>("Shaders/UnderWater_URP")),
            (KeyPixelate, Resources.Load<Shader>("Shaders/Pixelate_URP"))
        )
        {
        }

        protected override void SetupMaterial(ExtendedPostProcessRendererFeature rendererFeature, ShaderData data)
        {
            switch (data.Identifier)
            {
                case KeyUnderWater:
                    data.Material.SetTexture("_WaveNormal", rendererFeature.WaveTexture);
                    break;
            }
        }

        protected override PostProcessVolumeComponent GetEffect(VolumeStack stack, ShaderData data)
        {
            return data.Identifier switch
            {
                KeyUnderWater => stack.GetComponent<UnderWaterEffectVolume>(),
                KeyBlackWhite => stack.GetComponent<BlackWhiteEffectVolume>(),
                KeyPixelate => stack.GetComponent<PixelateEffectVolume>(),
                _ => throw new NotImplementedException(data.Identifier)
            };
        }
    }
}
#endif