using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public enum EMaterialParameterType
    {
        Float,
        Float2,
        Float3,
        Float4,
        Matrix4x4,
        Color,
        Texture
    }

    public class MaterialParam<T>
    {
        public EMaterialParameterType type;
        public string ParamName;
        public T Value;

        public MaterialParam(string registerName, T value = default(T))
        {
            ParamName = registerName;
            Value = value;
            type = EMaterialParameterType.Float;
            if (value is float)
                type = EMaterialParameterType.Float;
            else if (value is Vector2)
                type = EMaterialParameterType.Float2;
            else if (value is Vector3)
                type = EMaterialParameterType.Float3;
            else if (value is Vector4)
                type = EMaterialParameterType.Float4;
            else if (value is Matrix4x4)
                type = EMaterialParameterType.Matrix4x4;
            else if (value is Color)
                type = EMaterialParameterType.Color;
            else if (value is TextureInfo)
                type = EMaterialParameterType.Texture;
        }
        public virtual JObject Serialize()
        {
            return new JObject(Value);
        }
    }
    public class MaterialTextureParam : MaterialParam<TextureInfo>
    {
        public MaterialTextureParam(string registerName, TextureInfo value = null) : base(registerName, value)
        {
        }
        public override JObject Serialize()
        {
            JObject jo = new JObject();
            jo.Add("index", Value.Index.Id);
            return jo;
        }
    }
    public class MaterialCubemapParam : MaterialParam<CubemapId>
    {
        public MaterialCubemapParam(string registerName, CubemapId value = null) : base(registerName, value)
        {
        }
        public override JObject Serialize()
        {
            JObject jo = new JObject();
            jo.Add("cubemap", Value.Id);
            return jo;
        }
    }
}