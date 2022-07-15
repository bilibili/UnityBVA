using Newtonsoft.Json.Linq;
using UnityEngine;
using BVA;
using BVA.Extensions;

namespace BVA
{
    public interface IBaseTrack
    {
        bool SourceIsNull();
        public float length { get; }

    }
    [System.Serializable]
    public abstract class BaseTrack<T> : IBaseTrack where T : Object
    {
        public string name;
        public float startTime;
        public float endTime;
        public abstract float length { get; }
        public abstract JProperty Serialize(NodeCache cache);
        protected string gltfProperty => GetType().ToString().FirstLowercase();
        protected virtual JObject SerializeBase(NodeCache cache)
        {
            JObject jo = new JObject();
            if (!string.IsNullOrEmpty(name)) jo.Add(nameof(name), name);
            if (startTime > 0.0f) jo.Add(nameof(startTime), startTime);
            if (endTime > 0.0f) jo.Add(nameof(endTime), endTime);
            return jo;
        }
        public BaseTrack()
        {
            startTime = 0;
            endTime = 0;
            isValid = true;
        }
        public T source;
        public int sourceId { protected set; get; }
        public void SetSourceId(int id)
        {
            sourceId = id;
        }
        public bool isValid { protected set; get; }
#if UNITY_EDITOR
        public static bool showLog = false;
#else
        public static bool showLog = false;
#endif
        public void Log(object s)
        {
            if (showLog)
                Debug.Log($"name = {name} startTime = {startTime} endTime = {endTime} isValid = {isValid} \n Info : {s}");
        }
        public bool SourceIsNull()
        {
            if (source == null)
            {
                Debug.LogError($"{name} is null");
                isValid = false;
                return true;
            }
            return false;
        }
    }
}