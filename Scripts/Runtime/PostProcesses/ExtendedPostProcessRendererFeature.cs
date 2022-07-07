#if URP && !FORCE_HDRP
using System;
using UnityEngine;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses
{
    [Serializable, PostProcessRendererFeature(new[] { typeof(ExtendedPostProcessPass) })]
    public class ExtendedPostProcessRendererFeature : PostProcessRendererFeature
    {
        #region Inspector Data

        [Header("Under Water Effect")]
        [SerializeField]
        private Texture waveTexture;

        #endregion

        #region Properties

        public Texture WaveTexture => waveTexture;

        #endregion

        #region Builtin Methods

        private void Awake()
        {
            if (waveTexture == null)
            {
                waveTexture = Resources.Load<Texture>("Textures/waves");
            }
        }

        #endregion
    }
}
#endif