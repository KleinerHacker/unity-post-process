using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [Serializable, VolumeComponentMenu("Extensions/Color Change in Depth Effect")]
    public class ColorChangeInDepthEffectVolume : PostProcessVolumeComponent
    {
        #region Inspector Data

        [SerializeField]
        [PostProcessVolumeValue("_StartDistance")]
        private FloatParameter startDistance = new ClampedFloatParameter(0.5f, 0f, 1f);

        [SerializeField]
        [PostProcessVolumeValue("_EndDistance")]
        private FloatParameter endDistance = new ClampedFloatParameter(1f, 0f, 1f);
        
        [SerializeField]
        [PostProcessVolumeValue("_Multiplier")]
        private FloatParameter multiplier = new ClampedFloatParameter(0.01f, 0f, 1f);

        [SerializeField]
        private BoolParameter activate = new BoolParameter(true, true);

        #endregion

        #region Properties

        public FloatParameter StartDistance
        {
            get => startDistance;
            set => startDistance = value;
        }

        public FloatParameter EndDistance
        {
            get => endDistance;
            set => endDistance = value;
        }

        public FloatParameter Multiplier
        {
            get => multiplier;
            set => multiplier = value;
        }

        #endregion

        public override bool IsActive() => activate.value;

#if HDRP && !FORCE_URP
        public override Shader GetShader()
        {
            return Resources.Load<Shader>("Shaders/ColorChangeInDepth_HDRP");
        }
#endif
    }
}