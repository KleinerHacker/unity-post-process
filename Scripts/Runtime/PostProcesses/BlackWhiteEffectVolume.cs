using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [Serializable, VolumeComponentMenu("Extensions/Black White Effect")]
    public sealed class BlackWhiteEffectVolume : PostProcessVolumeComponent
    {
        #region Inspector Data

        [SerializeField]
        [PostProcessVolumeValue("_Intensity")]
        private ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, true);

        [SerializeField]
        [PostProcessVolumeValue("_DarkColor")]
        private ColorParameter darkColor = new ColorParameter(Color.black);
        
        [SerializeField]
        [PostProcessVolumeValue("_BrightColor")]
        private ColorParameter brightColor = new ColorParameter(Color.white);

        [SerializeField]
        [PostProcessVolumeValue("_Factor")]
        private ClampedFloatParameter factor = new ClampedFloatParameter(0.5f, 0f, 1f);

        #endregion

        #region Properties

        public ClampedFloatParameter Intensity
        {
            get => intensity;
            set => intensity = value;
        }

        public ColorParameter DarkColor
        {
            get => darkColor;
            set => darkColor = value;
        }

        public ColorParameter BrightColor
        {
            get => brightColor;
            set => brightColor = value;
        }

        public ClampedFloatParameter Factor
        {
            get => factor;
            set => factor = value;
        }

        #endregion
        
        public override bool IsActive() => intensity.value > 0f;
        
#if HDRP && !FORCE_URP
        public override Shader GetShader()
        {
            return Resources.Load<Shader>("Shaders/BlackWhite_HDRP");
        }
#endif
    }
}