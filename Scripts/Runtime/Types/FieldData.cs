using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.Types
{
    internal record FieldData
    {
        public FieldInfo FieldInfo { get; }
        public ShaderKey ShaderKey { get; }
        public ShaderParameterType Type { get; }

        public FieldData(FieldInfo fieldInfo, ShaderKey shaderKey, ShaderParameterType type)
        {
            FieldInfo = fieldInfo;
            ShaderKey = shaderKey;
            Type = type;
        }
    }

    internal record ShaderKey
    {
        public int ID { get; }
        public string Name { get; }

        public ShaderKey(string name)
        {
            ID = Shader.PropertyToID(name);
            Name = name;
        }
    }
}