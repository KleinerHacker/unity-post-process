#if HDRP && !FORCE_URP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Types;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Utils;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Utils.Extensions;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base
{
    public abstract class PostProcessVolumeComponent : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public abstract bool IsActive();
        public abstract Shader GetShader();

        private Material _material;
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

        public override void Setup()
        {
            _material = CoreUtils.CreateEngineMaterial(GetShader());
            if (_material == null)
            {
                Debug.LogError("Shader cannot convert to material!");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null)
                return;

            UpdateMaterialData(_material);
            HDUtils.DrawFullScreen(cmd, _material, destination);
        }

        public override void Cleanup() => CoreUtils.Destroy(_material);

        protected virtual void UpdateMaterialData(Material material)
        {
            foreach (var fieldData in _fieldData)
            {
                material.SetProperty(fieldData.KeyID, fieldData.FieldInfo.GetValue(this), fieldData.Type);
            }
        }
    }
}
#endif