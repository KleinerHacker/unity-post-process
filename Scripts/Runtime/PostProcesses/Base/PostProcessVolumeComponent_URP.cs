#if URP && !FORCE_HDRP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Types;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Utils;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base
{
    [Serializable]
    public abstract class PostProcessVolumeComponent : VolumeComponent, UnityEngine.Rendering.Universal.IPostProcessComponent
    {
        public abstract bool IsActive();
        public virtual bool IsTileCompatible() => true;
        internal FieldData[] FieldDataList => _fieldData.ToArray();
        private readonly IList<FieldData> _fieldData = new List<FieldData>();

        protected PostProcessVolumeComponent()
        {
            _fieldData = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<SerializeField>() != null && x.GetCustomAttribute<PostProcessVolumeValueAttribute>() != null)
                .Select(x =>
                {
                    var attribute = x.GetCustomAttribute<PostProcessVolumeValueAttribute>();
                    return new FieldData(x, new ShaderKey(attribute.ShaderParameterName), ShaderParameterTypeUtils.FromType(x.FieldType));
                })
                .ToList();
        }
    }
}
#endif