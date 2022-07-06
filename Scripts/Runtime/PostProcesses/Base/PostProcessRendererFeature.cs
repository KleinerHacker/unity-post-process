#if URP && !FORCE_HDRP
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.PostProcesses.Base
{
    [Serializable]
    public abstract class PostProcessRendererFeature : ScriptableRendererFeature
    {
        private readonly Type[] _renderPassTypes;
        private ScriptableRenderPass[] _renderPasses = Array.Empty<ScriptableRenderPass>();

        protected PostProcessRendererFeature()
        {
            var attribute = GetType().GetCustomAttribute<PostProcessRendererFeatureAttribute>();
            if (attribute == null)
                throw new InvalidOperationException("Unable to find required attribute " + nameof(PostProcessRendererFeatureAttribute) + " on class " + GetType().Name);

            _renderPassTypes = attribute.Types;
        }

        public sealed override void Create()
        {
            _renderPasses = _renderPassTypes
                .Select(x =>
                {
                    var constructor = x.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                        throw new InvalidOperationException("Unable to find public constructor without parameters in class " + x.Name);

                    var instance = (PostProcessRenderPass) constructor.Invoke(Array.Empty<object>());
                    instance.SetupMaterial(this);
                    
                    return instance;
                })
                .Select(x => (ScriptableRenderPass)x)
                .ToArray();
        }

        public sealed override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            foreach (var renderPass in _renderPasses)
            {
                renderer.EnqueuePass(renderPass);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PostProcessRendererFeatureAttribute : Attribute
    {
        public Type[] Types { get; }

        public PostProcessRendererFeatureAttribute(Type[] types)
        {
            Types = types;
        }
    }
}
#endif