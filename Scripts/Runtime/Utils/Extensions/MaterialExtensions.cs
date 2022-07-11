using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Types;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.Utils.Extensions
{
    internal static class MaterialExtensions
    {
        public static void SetProperty(this Material material, ShaderKey shaderKey, object value, ShaderParameterType typeCode)
        {
            switch (typeCode)
            {
                case ShaderParameterType.Integer:
                    material.SetInt(shaderKey.ID, ((IntParameter)value).value);
                    break;
                case ShaderParameterType.Float:
                    material.SetFloat(shaderKey.ID, ((FloatParameter)value).value);
                    break;
                case ShaderParameterType.Vector:
                    switch (value)
                    {
                        case Vector2Parameter vector2Parameter:
                            material.SetVector(shaderKey.ID, vector2Parameter.value);
                            break;
                        case Vector3Parameter vector3Parameter:
                            material.SetVector(shaderKey.ID, vector3Parameter.value);
                            break;
                        case Vector4Parameter vector4Parameter:
                            material.SetVector(shaderKey.ID, vector4Parameter.value);
                            break;
                        default:
                            throw new InvalidOperationException("Wrong parameter type? " + value.GetType().Name);
                    }

                    break;
                case ShaderParameterType.Texture:
                    material.SetTexture(shaderKey.ID, ((TextureParameter)value).value);
                    break;
                case ShaderParameterType.Color:
                    material.SetColor(shaderKey.ID, ((ColorParameter)value).value);
                    break;
                case ShaderParameterType.Boolean:
                    material.SetKeyword(new LocalKeyword(material.shader, shaderKey.Name), ((BoolParameter)value).value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}