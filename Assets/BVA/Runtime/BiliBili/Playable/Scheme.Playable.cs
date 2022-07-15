using GLTF.Schema;
using GLTF.Schema.BVA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using GLTF.Extensions;
using BVA.Extensions;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public class PlayableProperty
    {
        public PlayableId playable;
        public bool playOnAwake;
        public bool loop;
    }
}
namespace BVA
{
    public static class Playable
    {

        public static AnimationTrack DeserializeAnimationTrack(GLTFRoot root, JsonReader reader)
        {
            AnimationTrack track = new AnimationTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.animator):
                        int animatorIndex = reader.ReadAsInt32().Value;
                        track.SetAnimatorId(new NodeId() { Id = animatorIndex, Root = root });
                        break;
                }
            }
            return track;
        }

        public static AudioTrack DeserializeAudioTrack(GLTFRoot root, JsonReader reader)
        {
            AudioTrack track = new AudioTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.audio):
                        int audioSourceIndex = reader.ReadAsInt32().Value;
                        track.SetAudioSourceId(new NodeId() { Id = audioSourceIndex, Root = root });
                        break;
                }
            }
            return track;
        }

        public static BlendShapeCurveTrack DeserializeBlendShapeCurveTrack(GLTFRoot root, JsonReader reader)
        {
            BlendShapeCurveTrack track = new BlendShapeCurveTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.curve):
                        track.curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(track.index):
                        track.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(track.pingpongOnce):
                        track.pingpongOnce = reader.ReadAsBoolean().Value;
                        break;
                }
            }
            return track;
        }

        public static MaterialCurveFloatTrack DeserializeMaterialFloatCurveTrack(GLTFRoot root, JsonReader reader)
        {
            MaterialCurveFloatTrack track = new MaterialCurveFloatTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.curve):
                        track.curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(track.index):
                        track.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(track.pingpongOnce):
                        track.pingpongOnce = reader.ReadAsBoolean().Value;
                        break;
                }
            }
            return track;
        }
        public static MaterialCurveColorTrack DeserializeMaterialColorCurveTrack(GLTFRoot root, JsonReader reader)
        {
            MaterialCurveColorTrack track = new MaterialCurveColorTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.curve):
                        track.curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(track.index):
                        track.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(track.pingpongOnce):
                        track.pingpongOnce = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(track.startColor):
                        track.startColor = reader.ReadAsRGBAColor().ToUnityColorRaw();
                        break;
                    case nameof(track.endColor):
                        track.endColor = reader.ReadAsRGBAColor().ToUnityColorRaw();
                        break;
                }
            }
            return track;
        }
        public static MaterialFloatTrack DeserializeMaterialFloatTrack(GLTFRoot root, JsonReader reader)
        {
            MaterialFloatTrack track = new MaterialFloatTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.propertyName):
                        track.propertyName = reader.ReadAsString();
                        break;
                    case nameof(track.index):
                        track.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(track.value):
                        track.value = reader.ReadAsFloat();
                        break;
                }
            }
            return track;
        }
        public static MaterialIntTrack DeserializeMaterialIntTrack(GLTFRoot root, JsonReader reader)
        {
            MaterialIntTrack track = new MaterialIntTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.propertyName):
                        track.propertyName = reader.ReadAsString();
                        break;
                    case nameof(track.index):
                        track.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(track.value):
                        track.value = reader.ReadAsInt32().Value;
                        break;
                }
            }
            return track;
        }
        public static MaterialColorTrack DeserializeMaterialColorTrack(GLTFRoot root, JsonReader reader)
        {
            MaterialColorTrack track = new MaterialColorTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.propertyName):
                        track.propertyName = reader.ReadAsString();
                        break;
                    case nameof(track.index):
                        track.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(track.value):
                        track.value = reader.ReadAsRGBAColor().ToUnityColorRaw();
                        break;
                }
            }
            return track;
        }
        public static MaterialVectorTrack DeserializeMaterialVectorTrack(GLTFRoot root, JsonReader reader)
        {
            MaterialVectorTrack track = new MaterialVectorTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.propertyName):
                        track.propertyName = reader.ReadAsString();
                        break;
                    case nameof(track.index):
                        track.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(track.value):
                        track.value = reader.ReadAsRGBAColor().ToUnityVector4();
                        break;
                }
            }
            return track;
        }
        public static MaterialTexture2DTrack DeserializeMaterialTexture2DTrack(GLTFRoot root, JsonReader reader)
        {
            MaterialTexture2DTrack track = new MaterialTexture2DTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.propertyName):
                        track.propertyName = reader.ReadAsString();
                        break;
                    case nameof(track.index):
                        track.index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(track.value):
                        track.SetTextureId(new TextureId() { Id = reader.ReadAsInt32().Value, Root = root });
                        break;
                }
            }
            return track;
        }
        public static GameObjectActiveTrack DeserializeGameObjectActiveTrack(GLTFRoot root, JsonReader reader)
        {
            GameObjectActiveTrack track = new GameObjectActiveTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.active):
                        track.active = reader.ReadAsBoolean().Value;
                        break;
                }
            }
            return track;
        }
        public static ComponentEnableTrack DeserializeComponentEnableTrack(GLTFRoot root, JsonReader reader)
        {
            ComponentEnableTrack track = new ComponentEnableTrack();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(track.name):
                        track.name = reader.ReadAsString();
                        break;
                    case nameof(track.startTime):
                        track.startTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.endTime):
                        track.endTime = reader.ReadAsFloat();
                        break;
                    case nameof(track.source):
                        int audioIndex = reader.ReadAsInt32().Value;
                        track.SetSourceId(audioIndex);
                        break;
                    case nameof(track.value):
                        track.value = reader.ReadAsBoolean().Value;
                        break;
                }
            }
            return track;
        }
    }
}