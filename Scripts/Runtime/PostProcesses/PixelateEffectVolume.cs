using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [Serializable, VolumeComponentMenu("Extensions/Pixelate Effect")]
    public sealed class PixelateEffectVolume : PostProcessVolumeComponent
    {
        #region Inspector Data

        [SerializeField]
        [PostProcessVolumeValue("_CountOfBlocks")]
        private IntParameter countOfBlocks = new ClampedIntParameter(1, 1, 100, true);

        #endregion

        #region Properties

        public IntParameter CountOfBlocks
        {
            get => countOfBlocks;
            set => countOfBlocks = value;
        }

        #endregion

        public override bool IsActive() => CountOfBlocks.value > 1;
        
#if HDRP && !FORCE_URP
        public override Shader GetShader()
        {
            return Resources.Load<Shader>("Shaders/Pixelate_HDRP");
        }
#endif
    }
}