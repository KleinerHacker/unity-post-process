using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPostProcess.Runtime.post_process.Scripts.Runtime.Types;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.Utils.Extensions
{
    public static class MaterialExtensions
    {
        public static void SetProperty(this Material material, int id, object value, ShaderParameterType typeCode)
        {
            switch (typeCode)
            {
                case ShaderParameterType.Integer:
                    material.SetInt(id, ((IntParameter)value).value);
                    break;
                case ShaderParameterType.Float:
                    material.SetFloat(id, ((FloatParameter)value).value);
                    break;
                case ShaderParameterType.Vector:
                    switch (value)
                    {
                        case Vector2Parameter vector2Parameter:
                            material.SetVector(id, vector2Parameter.value);
                            break;
                        case Vector3Parameter vector3Parameter:
                            material.SetVector(id, vector3Parameter.value);
                            break;
                        case Vector4Parameter vector4Parameter:
                            material.SetVector(id, vector4Parameter.value);
                            break;
                        default:
                            throw new InvalidOperationException("Wrong parameter type? " + value.GetType().Name);
                    }

                    break;
                case ShaderParameterType.Texture:
                    material.SetTexture(id, ((TextureParameter)value).value);
                    break;
                case ShaderParameterType.Color:
                    material.SetColor(id, ((ColorParameter)value).value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}