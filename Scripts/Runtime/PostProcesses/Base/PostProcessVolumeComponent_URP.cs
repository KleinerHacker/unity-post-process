#if URP && !FORCE_HDRP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Types;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Utils;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base
{
    [Serializable]
    public abstract class PostProcessVolumeComponent : UnityEngine.Rendering.VolumeComponent, UnityEngine.Rendering.Universal.IPostProcessComponent
    {
        public abstract bool IsActive();
        public virtual bool IsTileCompatible() => true;
        internal FieldData[] FieldDataList => _fieldData.ToArray();
        private readonly IList<FieldData> _fieldData = new List<FieldData>();

        protected PostProcessVolumeComponent()
        {
            _fieldData = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<SerializeField>() != null)
                .Select(x =>
                {
                    var attribute = x.GetCustomAttribute<PostProcessVolumeValueAttribute>();
                    if (attribute == null)
                        throw new InvalidOperationException("Unable to find required attribute " + nameof(PostProcessVolumeValueAttribute) + " on field " + x.Name + " in class " + GetType().Name);

                    return new FieldData(x, Shader.PropertyToID(attribute.ShaderParameterName), ShaderParameterTypeUtils.FromType(x.FieldType));
                })
                .ToList();
        }
    }
}
#endif