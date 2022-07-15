using System;
using GLTF.Extensions;
using Newtonsoft.Json;

namespace GLTF.Schema
{
    /// <summary>
    /// The index of the node and TRS property that an animation channel targets.
    /// </summary>
    public class AnimationChannelTarget : GLTFProperty
    {
        /// <summary>
        /// The index of the node to target.
        /// </summary>
        public NodeId Node;

        /// <summary>
        /// The name of the node's TRS property to modify.
        /// </summary>
        public GLTFAnimationChannelPath Path;
        public string Channel;
        public static AnimationChannelTarget Deserialize(GLTFRoot root, JsonReader reader)
        {
            var animationChannelTarget = new AnimationChannelTarget();

            if (reader.Read() && reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Animation channel target must be an object.");
            }

            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case "node":
                        animationChannelTarget.Node = NodeId.Deserialize(root, reader);
                        break;
                    case "path":
                        animationChannelTarget.Channel = reader.ReadAsString();
                        Enum.TryParse<GLTFAnimationChannelPath>(animationChannelTarget.Channel, out animationChannelTarget.Path);
                        break;
                    default:
                        animationChannelTarget.DefaultPropertyDeserializer(root, reader);
                        break;
                }
            }

            return animationChannelTarget;
        }

        public AnimationChannelTarget()
        {
        }

        public AnimationChannelTarget(AnimationChannelTarget channelTarget, GLTFRoot gltfRoot) : base(channelTarget)
        {
            if (channelTarget == null) return;

            Node = new NodeId(channelTarget.Node, gltfRoot);
            Path = channelTarget.Path;
            Channel = channelTarget.Channel;
        }

        public override void Serialize(JsonWriter writer)
        {
            writer.WriteStartObject();

            if (Node.Id >= 0)
            {
                writer.WritePropertyName("node");
                writer.WriteValue(Node.Id);
            }

            //if it's not supported by khronos ,export as weights
            writer.WritePropertyName("path");
            if (Channel != null && Path.ToString() != Channel)
                writer.WriteValue(Channel.ToString());
            else
                writer.WriteValue(Path.ToString());

            base.Serialize(writer);

            writer.WriteEndObject();
        }
    }

    public enum GLTFAnimationChannelPath
    {
        translation,
        eulerAngle,
        rotation,
        scale,
        weights,
        animator_muscle,
        animator_T,
        animator_Q,
        animator_S,
        not_implement
    }
}
