using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [Serializable, VolumeComponentMenu("Post-processing/Extensions/Color/Gray Scale Effect")]
    public sealed class GrayScaleEffectVolume : PostProcessVolumeComponent
    {
        #region Inspector Data

        [SerializeField]
        [PostProcessVolumeValue("_Intensity")]
        private ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, true);

        [Header("Channels")]
        [SerializeField]
        [PostProcessVolumeValue("_Red")]
        private ClampedFloatParameter red = new ClampedFloatParameter(0.39f, 0f, 1f);
        
        [SerializeField]
        [PostProcessVolumeValue("_Green")]
        private ClampedFloatParameter green = new ClampedFloatParameter(0.41f, 0f, 1f);
        
        [SerializeField]
        [PostProcessVolumeValue("_Blue")]
        private ClampedFloatParameter blue = new ClampedFloatParameter(0.2f, 0f, 1f);
        
        [Header("Colors")]
        [SerializeField]
        [PostProcessVolumeValue("_CUSTOMCOLORS")]
        private BoolParameter customColors = new BoolParameter(true);
        
        [SerializeField]
        [PostProcessVolumeValue("_DarkColor")]
        private ColorParameter darkColor = new ColorParameter(Color.black);
        
        [SerializeField]
        [PostProcessVolumeValue("_BrightColor")]
        private ColorParameter brightColor = new ColorParameter(Color.white);

        #endregion

        #region Properties

        public ClampedFloatParameter Red
        {
            get => red;
            set => red = value;
        }

        public ClampedFloatParameter Green
        {
            get => green;
            set => green = value;
        }

        public ClampedFloatParameter Blue
        {
            get => blue;
            set => blue = value;
        }

        public ClampedFloatParameter Intensity
        {
            get => intensity;
            set => intensity = value;
        }

        public BoolParameter CustomColors
        {
            get => customColors;
            set => customColors = value;
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

        #endregion
        
        public override bool IsActive() => intensity.value > 0f;
        
#if HDRP && !FORCE_URP
        public override Shader GetShader()
        {
            return Resources.Load<Shader>("Shaders/GrayScale_HDRP");
        }
#endif
    }
}