using System;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Types;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.Utils
{
    public static class ShaderParameterTypeUtils
    {
        public static ShaderParameterType FromType(Type type)
        {
            if (!typeof(VolumeParameter).IsAssignableFrom(type))
                throw new NotSupportedException("Type " + type.Name + " not supported. Use a " + nameof(VolumeParameter) + " instead");
            
            if (typeof(IntParameter).IsAssignableFrom(type))
                return ShaderParameterType.Integer;

            if (typeof(FloatParameter).IsAssignableFrom(type))
                return ShaderParameterType.Float;

            if (typeof(Vector2Parameter).IsAssignableFrom(type) || typeof(Vector3Parameter).IsAssignableFrom(type) || typeof(Vector4Parameter).IsAssignableFrom(type))
                return ShaderParameterType.Vector;

            if (typeof(TextureParameter).IsAssignableFrom(type))
                return ShaderParameterType.Texture;

            if (typeof(ColorParameter).IsAssignableFrom(type))
                return ShaderParameterType.Color;

            if (typeof(BoolParameter).IsAssignableFrom(type))
                return ShaderParameterType.Boolean;

            throw new NotSupportedException("Type " + type.Name + " not supported yet");
        }
    }
}