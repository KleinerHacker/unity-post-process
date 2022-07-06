using System.Reflection;

namespace UnityPostProcess.Runtime.post_process.Scripts.Runtime.Types
{
    internal record FieldData
    {
        public FieldInfo FieldInfo { get; }
        public int KeyID { get; }
        public ShaderParameterType Type { get; }

        public FieldData(FieldInfo fieldInfo, int keyID, ShaderParameterType type)
        {
            FieldInfo = fieldInfo;
            KeyID = keyID;
            Type = type;
        }
    }
}