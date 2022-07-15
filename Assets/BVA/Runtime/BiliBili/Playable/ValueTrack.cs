using Newtonsoft.Json.Linq;
using UnityEngine;
using BVA;
using BVA.Extensions;
using GLTF.Schema;

namespace BVA
{
    [System.Serializable]
    public abstract class BaseValueTrack<U, T> : BaseTrack<T> where T :UnityEngine.Component
    {
        public int index;
        public string propertyName;
        public U value;
        
        public BaseValueTrack()
        {
        }
        protected override JObject SerializeBase(NodeCache cache)
        {
            var jo = base.SerializeBase(cache);
            jo.Add(nameof(index), index);
            jo.Add(nameof(propertyName), propertyName);
            jo.Add(nameof(source), cache.GetId(source.gameObject));
            return jo;
        }
        public abstract void SetValue();
        public override float length => endTime - startTime;
    }
    [System.Serializable]
    public class MaterialFloatTrack : BaseValueTrack<float, Renderer>
    {
        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = SerializeBase(cache);
            jo.Add(nameof(value), value);
            return new JProperty(gltfProperty, jo);
        }

        public override void SetValue()
        {
            source.materials[index].SetFloat(propertyName, value);
        }
    }
    [System.Serializable]
    public class MaterialIntTrack : BaseValueTrack<int, Renderer>
    {
        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = SerializeBase(cache);
            jo.Add(nameof(value), value);
            return new JProperty(gltfProperty, jo);
        }

        public override void SetValue()
        {
            source.materials[index].SetInt(propertyName, value);
        }
    }
    [System.Serializable]
    public class MaterialVectorTrack : BaseValueTrack<Vector4, Renderer>
    {
        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = SerializeBase(cache);
            jo.Add(nameof(value), value.ToGltfVector4Raw().ToJArray());
            return new JProperty(gltfProperty, jo);
        }

        public override void SetValue()
        {
            source.materials[index].SetVector(propertyName, value);
        }
    }
    [System.Serializable]
    public class MaterialTexture2DTrack : BaseValueTrack<Texture2D, Renderer>
    {
        public TextureId textureId { private set; get; }
        public void SetTextureId(TextureId id)
        {
            textureId = id;
        }
        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = SerializeBase(cache);
            jo.Add(nameof(value), textureId.Id);
            return new JProperty(gltfProperty, jo);
        }

        public override void SetValue()
        {
            source.materials[index].SetTexture(propertyName, value);
        }
    }
    [System.Serializable]
    public class MaterialColorTrack : BaseValueTrack<Color, Renderer>
    {
        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = SerializeBase(cache);
            jo.Add(nameof(value), value.ToNumericsColorRaw().ToJArray());
            return new JProperty(gltfProperty, jo);
        }

        public override void SetValue()
        {
            source.materials[index].SetColor(propertyName, value);
        }
    }
    [System.Serializable]
    public class ComponentEnableTrack : BaseValueTrack<bool,MonoBehaviour>
    {
        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = SerializeBase(cache);
            jo.Add(nameof(value), value);
            return new JProperty(gltfProperty, jo);
        }

        public override void SetValue()
        {
            source.enabled = value;
        }
    }
    [System.Serializable]
    public class GameObjectActiveTrack: BaseTrack<GameObject>
    {
        public bool active;

        public GameObjectActiveTrack()
        {
        }
        public void SetValue()
        {
            source.SetActive(active);
        }

        public override JProperty Serialize(NodeCache cache)
        {
            var jo = base.SerializeBase(cache);
            jo.Add(nameof(source), cache.GetId(source.gameObject));
            jo.Add(nameof(active), active);
            return new JProperty(gltfProperty, jo);
        }

        public override float length => endTime - startTime;
    }
}