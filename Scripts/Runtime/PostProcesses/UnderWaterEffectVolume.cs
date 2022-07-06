using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [Serializable, VolumeComponentMenu("Extensions/Under Water Effect")]
    public sealed class UnderWaterEffectVolume : PostProcessVolumeComponent
    {
        #region Inspector Data

        [SerializeField]
        [PostProcessVolumeValue("_Intensity")]
        private ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, true);

        [SerializeField]
        [PostProcessVolumeValue("_WaveNormal")]
        private TextureParameter waveNormal = new TextureParameter(null);

        [SerializeField]
        [PostProcessVolumeValue("_Tiling")]
        private Vector2Parameter tiling = new Vector2Parameter(Vector2.one);

        [Header("Animation")]
        [SerializeField]
        [PostProcessVolumeValue("_Speed")]
        private Vector2Parameter speed = new Vector2Parameter(new Vector2(0.003f, 0.007f));

        #endregion

        #region Properties

        public TextureParameter WaveNormal
        {
            get => waveNormal;
            set => waveNormal = value;
        }

        public ClampedFloatParameter Intensity
        {
            get => intensity;
            set => intensity = value;
        }

        public Vector2Parameter Tiling
        {
            get => tiling;
            set => tiling = value;
        }

        public Vector2Parameter Speed
        {
            get => speed;
            set => speed = value;
        }

        #endregion

        public override bool IsActive() => intensity.value > 0f;

#if HDRP && !FORCE_URP
        public override Shader GetShader()
        {
            return Resources.Load<Shader>("Shaders/UnderWater_HDRP");
        }
#endif
    }
}