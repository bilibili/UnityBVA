using GLTF.Extensions;
using GLTF.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class AnimationCurveExtension
{
    public static JArray Serialize(this AnimationCurve curve)
    {
        var array = new JArray();
        foreach (var v in curve.keys)
        {
            var pro = new JObject();
            pro.Add(new JProperty(nameof(v.time), v.time));
            pro.Add(new JProperty(nameof(v.value), v.value));
            if (v.inTangent != 0.0f) pro.Add(new JProperty(nameof(v.inTangent), v.inTangent));
            if (v.outTangent != 0.0f) pro.Add(new JProperty(nameof(v.outTangent), v.outTangent));
            if (v.inWeight != 0.0f) pro.Add(new JProperty(nameof(v.inWeight), v.inWeight));
            if (v.outWeight != 0.0f) pro.Add(new JProperty(nameof(v.outWeight), v.outWeight));
            array.Add(pro);
        }
        return array;
    }

    public static Keyframe ReadKeyFrames(GLTFRoot root, JsonReader reader)
    {
        Keyframe keyframe = new Keyframe();
        while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
        {
            var curProp = reader.Value.ToString();

            switch (curProp)
            {
                case nameof(keyframe.time):
                    keyframe.time = reader.ReadAsFloat();
                    break;
                case nameof(keyframe.value):
                    keyframe.value = reader.ReadAsFloat();
                    break;
                case nameof(keyframe.inTangent):
                    keyframe.inTangent = reader.ReadAsFloat();
                    break;
                case nameof(keyframe.outTangent):
                    keyframe.outTangent = reader.ReadAsFloat();
                    break;
                case nameof(keyframe.inWeight):
                    keyframe.inWeight = reader.ReadAsFloat();
                    break;
                case nameof(keyframe.outWeight):
                    keyframe.outWeight = reader.ReadAsFloat();
                    break;
            }
        }
        return keyframe;
    }
    public static AnimationCurve DeserializeAnimationCurve(GLTFRoot root, JsonReader reader)
    {
        AnimationCurve curve = new AnimationCurve();
        curve.keys = reader.ReadList(() => ReadKeyFrames(root, reader)).ToArray();
        return curve;
    }
}