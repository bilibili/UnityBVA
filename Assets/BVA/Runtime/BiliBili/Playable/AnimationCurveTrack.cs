using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using BVA;
using BVA.Extensions;

namespace BVA
{
    [System.Serializable]
    public abstract class BaseAnimationCurveTrack<T> : BaseTrack<T> where T : UnityEngine.Component
    {
        /// <summary>
        /// default is 0
        /// </summary>
        public int index;

        /// <summary>
        /// default is false
        /// </summary>
        public bool pingpongOnce;
        public AnimationCurve curve;
        public float Evaluate(float time)
        {
            return curve.Evaluate(time);
        }
        public abstract float SetTime(float time);
        public float endPlayTime => curve.keys.Last().time;
        public float endValue => curve.keys.Last().value;
        public BaseAnimationCurveTrack()
        {
        }
        public override float length => endTime - startTime;
        protected override JObject SerializeBase(NodeCache cache)
        {
            JObject jo = base.SerializeBase(cache);
            if (index > 0)
                jo.Add(nameof(index), index);
            if (pingpongOnce)
                jo.Add(nameof(pingpongOnce), index);
            if (curve != null)
                jo.Add(nameof(curve), curve.Serialize());
            return jo;
        }
    }

    [System.Serializable]
    public class BlendShapeCurveTrack : BaseAnimationCurveTrack<SkinnedMeshRenderer>
    {
        public const float BLENDSHAPE_MULTIPLY = 100.0f;

        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = base.SerializeBase(cache);
            jo.Add(nameof(source), cache.GetId(source.gameObject));
            return new JProperty(gltfProperty, jo);
        }

        public override float SetTime(float time)
        {
            var blendWeight = Evaluate(time) * BLENDSHAPE_MULTIPLY;
            source.SetBlendShapeWeight(index, blendWeight);
            Log("blendweight evaluate value:" + blendWeight);
            return blendWeight;
        }
    }

    [System.Serializable]
    public class MaterialCurveFloatTrack : BaseAnimationCurveTrack<Renderer>
    {
        public string propertyName;
        public override float SetTime(float time)
        {
            var ret = Evaluate(time);
            source.materials[index].SetFloat(propertyName, ret);
            Log("material evaluate value:" + ret);
            return ret;
        }

        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = base.SerializeBase(cache);
            jo.Add(nameof(propertyName), propertyName);
            jo.Add(nameof(source), cache.GetId(source.gameObject));
            return new JProperty(gltfProperty, jo);
        }
    }

    [System.Serializable]
    public class MaterialCurveColorTrack : MaterialCurveFloatTrack
    {
        public Color startColor;
        public Color endColor;
        public override float SetTime(float time)
        {
            var ret = Evaluate(time);
            var color = Color.Lerp(startColor, endColor, ret);
            source.materials[index].SetColor(propertyName, color);
            Log("material evaluate color:" + color);
            return ret;
        }

        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = base.SerializeBase(cache);
            jo.Add(nameof(propertyName), propertyName);
            jo.Add(nameof(source), cache.GetId(source.gameObject));
            jo.Add(nameof(startColor), startColor.ToJArray());
            jo.Add(nameof(endColor), endColor.ToJArray());
            return new JProperty(gltfProperty, jo);
        }
    }
}