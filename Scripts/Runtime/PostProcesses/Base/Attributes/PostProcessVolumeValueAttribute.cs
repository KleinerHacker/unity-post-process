using System;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PostProcessVolumeValueAttribute : Attribute
    {
        public string ShaderParameterName { get; }

        public PostProcessVolumeValueAttribute(string shaderParameterName)
        {
            ShaderParameterName = shaderParameterName;
        }
    }
}