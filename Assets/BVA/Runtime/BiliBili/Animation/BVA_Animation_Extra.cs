using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GLTF.Schema.BVA
{
    public class BVA_Animation_Extra : IExtra
    {
        public const string PROPERTY = "Animation";
        public bool playAutomatically;
        public UnityEngine.WrapMode wrapMode;
        public bool animatePhysics;
        public UnityEngine.AnimationCullingType cullingType;
        public int animation;
        public List<int> animations;
        public BVA_Animation_Extra()
        {
            this.animations = new List<int>();
            this.playAutomatically = false;
            this.wrapMode = UnityEngine.WrapMode.Default;
            this.animatePhysics = true;
            this.cullingType = UnityEngine.AnimationCullingType.AlwaysAnimate;
        }

        public BVA_Animation_Extra(UnityEngine.Animation target)
        {
            this.playAutomatically = target.playAutomatically;
            this.wrapMode = target.wrapMode;
            this.animatePhysics = target.animatePhysics;
            this.cullingType = target.cullingType;
            this.animations = new List<int>();
        }
        public static async Task Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.Animation target, ConstructClipFunc ConstructClip, UnityEngine.Transform rootTransform, System.Threading.CancellationToken cancellationToken)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_Animation_Extra.playAutomatically):
                            target.playAutomatically = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_Animation_Extra.wrapMode):
                            target.wrapMode = reader.ReadStringEnum<UnityEngine.WrapMode>();
                            break;
                        case nameof(BVA_Animation_Extra.animatePhysics):
                            target.animatePhysics = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_Animation_Extra.cullingType):
                            target.cullingType = reader.ReadStringEnum<UnityEngine.AnimationCullingType>();
                            break;
                        case nameof(BVA_Animation_Extra.animation):
                            int indexOfAnimation = reader.ReadAsInt32().Value;
                            var clip = await ConstructClip(false, rootTransform, indexOfAnimation, cancellationToken);
                            target.clip = clip;
                            break;
                        case nameof(BVA_Animation_Extra.animations):
                            List<int> indexOfAnimations = reader.ReadInt32List();
                            foreach (var index in indexOfAnimations)
                            {
                                var clips = await ConstructClip(false, rootTransform, index, cancellationToken);
                                target.AddClip(clips, clips.name);
                            }
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(playAutomatically), playAutomatically);
            jo.Add(nameof(wrapMode), wrapMode.ToString());
            jo.Add(nameof(animatePhysics), animatePhysics);
            jo.Add(nameof(cullingType), cullingType.ToString());
            if (animation >= 0) jo.Add(nameof(animation), animation);
            if (animations.Count > 0)
            {
                JArray jArray = new JArray();
                foreach (var animation in animations)
                {
                    jArray.Add(animation);
                }
                jo.Add(nameof(animations), jArray);
            }
            return new JProperty(BVA_Animation_Extra.PROPERTY, jo);
        }
    }
}
