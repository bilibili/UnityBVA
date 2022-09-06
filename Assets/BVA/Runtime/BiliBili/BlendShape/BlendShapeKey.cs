using System;
using BVA.Extensions;
using GLTF.Extensions;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using GLTF.Schema;
using Newtonsoft.Json;

namespace BVA
{
    public enum BlendShapeMixerPreset
    {
        Neutral,
        A,
        I,
        U,
        E,
        O,

        Blink,
        Blink_L,
        Blink_R,

        Joy,
        Angry,
        Sorrow,
        Fun,

        LookUp,
        LookDown,
        LookLeft,
        LookRight,
        Custom
    }
    public interface IValueBinding
    {
        /// <summary>
        /// 0-1
        /// </summary>
        /// <param name="lerp"></param>
        void Set(float lerp);
        JObject Serialize(NodeCache cache);
    }
    [Serializable]
    public class BlendShapeValueBinding : IValueBinding
    {
        public SkinnedMeshRenderer node;
        public int nodeId { protected set; get; }
        public void SetNodeId(int id)
        {
            nodeId = id;
        }
        public int index;
        public float weight;
        public BlendShapeValueBinding()
        {
            //weight = 1.0f;
        }
        public JObject Serialize(NodeCache cache)
        {
            JObject jo = new JObject();
            jo.Add(nameof(node), cache.GetId(node.gameObject));
            jo.Add(nameof(index), index);
            jo.Add(nameof(weight), weight);
            return jo;
        }
        public static BlendShapeValueBinding Deserialize(GLTFRoot root, JsonReader reader)
        {
            BlendShapeValueBinding binding = new BlendShapeValueBinding();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(binding.node):
                        int nodeIndex = reader.ReadAsInt32().Value;
                        binding.SetNodeId(nodeIndex);
                        break;
                    case nameof(binding.index):
                        binding.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(binding.weight):
                        binding.weight = reader.ReadAsFloat();
                        break;
                }
            }
            return binding;
        }
        public void Set(float lerp)
        {
            node.SetBlendShapeWeight(index, lerp * weight);
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]=>{2}", node.name, index, weight);
        }
    }
    public abstract class MaterialValueBinding<T> : IValueBinding
    {
        public SkinnedMeshRenderer node;
        public int index;
        public string propertyName;
        public T targetValue;
        public T baseValue;
        public int nodeId { protected set; get; }
        public void SetNodeId(int id)
        {
            nodeId = id;
        }

        public virtual JObject Serialize(NodeCache cache)
        {
            JObject jo = new JObject();
            jo.Add(nameof(node), cache.GetId(node.gameObject));
            jo.Add(nameof(index), index);
            jo.Add(nameof(propertyName), propertyName);
            return jo;
        }

        public abstract void Set(float lerp);
    }
    [Serializable]
    public class MaterialFloatValueBinding : MaterialValueBinding<float>
    {
        public override JObject Serialize(NodeCache cache)
        {
            JObject jo = base.Serialize(cache);
            jo.Add(nameof(baseValue), baseValue);
            jo.Add(nameof(targetValue), targetValue);
            return jo;
        }
        public override void Set(float lerp)
        {
            node.materials[index].SetFloat(propertyName, baseValue + (targetValue - baseValue) * lerp);
        }
        public static MaterialFloatValueBinding Deserialize(GLTFRoot root, JsonReader reader)
        {
            MaterialFloatValueBinding binding = new MaterialFloatValueBinding();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(binding.node):
                        int nodeIndex = reader.ReadAsInt32().Value;
                        binding.SetNodeId(nodeIndex);
                        break;
                    case nameof(binding.index):
                        binding.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(binding.propertyName):
                        binding.propertyName = reader.ReadAsString();
                        break;
                    case nameof(binding.baseValue):
                        binding.baseValue = reader.ReadAsFloat();
                        break;
                    case nameof(binding.targetValue):
                        binding.targetValue = reader.ReadAsFloat();
                        break;
                }
            }
            return binding;
        }
    }
    [Serializable]
    public class MaterialColorValueBinding : MaterialValueBinding<Color>
    {
        public override JObject Serialize(NodeCache cache)
        {
            JObject jo = base.Serialize(cache);
            jo.Add(nameof(baseValue), baseValue.ToJArray());
            jo.Add(nameof(targetValue), targetValue.ToJArray());
            return jo;
        }
        public override void Set(float lerp)
        {
            node.materials[index].SetColor(propertyName, Color.Lerp(baseValue, targetValue, lerp));
        }
        public static MaterialColorValueBinding Deserialize(GLTFRoot root, JsonReader reader)
        {
            MaterialColorValueBinding binding = new MaterialColorValueBinding();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(binding.node):
                        int nodeIndex = reader.ReadAsInt32().Value;
                        binding.SetNodeId(nodeIndex);
                        break;
                    case nameof(binding.index):
                        binding.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(binding.propertyName):
                        binding.propertyName = reader.ReadAsString();
                        break;
                    case nameof(binding.baseValue):
                        binding.baseValue = reader.ReadAsRGBAColor();
                        break;
                    case nameof(binding.targetValue):
                        binding.targetValue = reader.ReadAsRGBAColor();
                        break;
                }
            }
            return binding;
        }
    }
    [Serializable]
    public class MaterialVector4ValueBinding : MaterialValueBinding<Vector4>
    {
        public override JObject Serialize(NodeCache cache)
        {
            JObject jo = base.Serialize(cache);
            jo.Add(nameof(baseValue), baseValue.ToJArray());
            jo.Add(nameof(targetValue), targetValue.ToJArray());
            return jo;
        }
        public override void Set(float lerp)
        {
            node.materials[index].SetVector(propertyName, Vector4.Lerp(baseValue, targetValue, lerp));
        }
        public static MaterialVector4ValueBinding Deserialize(GLTFRoot root, JsonReader reader)
        {
            MaterialVector4ValueBinding binding = new MaterialVector4ValueBinding();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(binding.node):
                        int nodeIndex = reader.ReadAsInt32().Value;
                        binding.SetNodeId(nodeIndex);
                        break;
                    case nameof(binding.index):
                        binding.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(binding.propertyName):
                        binding.propertyName = reader.ReadAsString();
                        break;
                    case nameof(binding.baseValue):
                        binding.baseValue = reader.ReadAsRGBAColor().ToVector4();
                        break;
                    case nameof(binding.targetValue):
                        binding.targetValue = reader.ReadAsRGBAColor().ToVector4();
                        break;
                }
            }
            return binding;
        }
    }
    [Serializable]
    public class BlendShapeKey : IValueBinding
    {
        public string keyName = "";
        public BlendShapeMixerPreset preset;
        public List<BlendShapeValueBinding> blendShapeValues;
        public List<MaterialVector4ValueBinding> materialVector4Values;
        public List<MaterialColorValueBinding> materialColorValues;
        public List<MaterialFloatValueBinding> materialFloatValues;
        /// <summary>
        /// if less than 0.5f, regard it as 0. Else regard as 1.0f,default is false
        /// </summary>
        public bool isBinary;
        public BlendShapeKey()
        {
            isBinary = false; 
            blendShapeValues = new List<BlendShapeValueBinding>();
            materialVector4Values = new List<MaterialVector4ValueBinding>();
            materialColorValues = new List<MaterialColorValueBinding>();
            materialFloatValues = new List<MaterialFloatValueBinding>();
        }
        public JObject Serialize(NodeCache cache)
        {

            JObject jo = new JObject();
            jo.Add(nameof(keyName), keyName);
            jo.Add(nameof(preset), preset.ToString());
            if(isBinary) jo.Add(nameof(isBinary), isBinary);
            if (blendShapeValues.Count > 0)
            {
                JArray ja = new JArray();
                foreach (var v in blendShapeValues)
                    ja.Add(v.Serialize(cache));
                jo.Add(nameof(blendShapeValues), ja);
            }

            if (materialVector4Values.Count > 0)
            {
                JArray ja = new JArray();
                foreach (var v in materialVector4Values)
                    ja.Add(v.Serialize(cache));
                jo.Add(nameof(materialVector4Values), ja);
            }

            if (materialColorValues.Count > 0)
            {
                JArray ja = new JArray();
                foreach (var v in materialColorValues)
                    ja.Add(v.Serialize(cache));
                jo.Add(nameof(materialColorValues), ja);
            }

            if (materialFloatValues.Count > 0)
            {
                JArray ja = new JArray();
                foreach (var v in materialFloatValues)
                    ja.Add(v.Serialize(cache));
                jo.Add(nameof(materialFloatValues), ja);
            }
            return jo;
        }
        public void Set(float lerp)
        {
            if (isBinary) lerp = lerp > 0.5f ? 1.0f : 0;
            blendShapeValues.ForEach((x) => x.Set(lerp));
            materialVector4Values.ForEach((x) => x.Set(lerp));
            materialColorValues.ForEach((x) => x.Set(lerp));
            materialFloatValues.ForEach((x) => x.Set(lerp));
        }
        public static BlendShapeKey Deserialize(GLTFRoot root, JsonReader reader)
        {
            BlendShapeKey key = new BlendShapeKey();

            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(key.keyName):
                        key.keyName = reader.ReadAsString();
                        break;
                    case nameof(key.isBinary):
                        key.isBinary = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(key.preset):
                        key.preset = reader.ReadStringEnum<BlendShapeMixerPreset>();
                        break;
                    case nameof(key.blendShapeValues):
                        key.blendShapeValues = reader.ReadList(() => BlendShapeValueBinding.Deserialize(root, reader));
                        break;
                    case nameof(key.materialFloatValues):
                        key.materialFloatValues = reader.ReadList(() => MaterialFloatValueBinding.Deserialize(root, reader));
                        break;
                    case nameof(key.materialColorValues):
                        key.materialColorValues = reader.ReadList(() => MaterialColorValueBinding.Deserialize(root, reader));
                        break;
                    case nameof(key.materialVector4Values):
                        key.materialVector4Values = reader.ReadList(() => MaterialVector4ValueBinding.Deserialize(root, reader));
                        break;
                }
            }
            return key;
        }
    }

}