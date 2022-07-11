using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [Serializable, VolumeComponentMenu("Post-processing/Extensions/Filter/Swirl Effect")]
    public sealed class SwirlEffectVolume : PostProcessVolumeComponent
    {
        #region Inspector Data

        [SerializeField]
        [PostProcessVolumeValue("_Intensity")]
        private ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, true);

        [Space]
        [SerializeField]
        [PostProcessVolumeValue("_Center")]
        private Vector2Parameter center = new Vector2Parameter(Vector2.one / 2f);

        [SerializeField]
        [PostProcessVolumeValue("_Frequency")]
        private Vector2Parameter frequency = new Vector2Parameter(Vector2.one * 10f);

        #endregion

        #region Properties

        public Vector2Parameter Center
        {
            get => center;
            set => center = value;
        }

        public Vector2Parameter Frequency
        {
            get => frequency;
            set => frequency = value;
        }

        #endregion
        
        public override bool IsActive() => intensity.value > 0f;
        
#if HDRP && !FORCE_URP
        public override Shader GetShader()
        {
            return Resources.Load<Shader>("Shaders/Swirl_HDRP");
        }
#endif
    }
}