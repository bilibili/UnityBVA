using System;
using GLTF.Extensions;
using Newtonsoft.Json;

namespace GLTF.Schema
{
    /// <summary>
    /// A perspective camera containing properties to create a perspective projection
    /// matrix.
    /// </summary>
    public class CameraPerspective : GLTFProperty
    {
        /// <summary>
        /// The floating-point aspect ratio of the field of view.
        /// When this is undefined, the aspect ratio of the canvas is used.
        /// <minimum>0.0</minimum>
        /// </summary>
        public float AspectRatio;

        /// <summary>
        /// The floating-point vertical field of view in radians.
        /// <minimum>0.0</minimum>
        /// </summary>
        public float YFov;

        /// <summary>
        /// The floating-point distance to the far clipping plane. When defined,
        /// `zfar` must be greater than `znear`.
        /// If `zfar` is undefined, runtime must use infinite projection matrix.
        /// <minimum>0.0</minimum>
        /// </summary>
        public float ZFar = float.PositiveInfinity;

        /// <summary>
        /// The floating-point distance to the near clipping plane.
        /// <minimum>0.0</minimum>
        /// </summary>
        public float ZNear = 0.1f;

        public CameraPerspective()
        {
        }

        public CameraPerspective(CameraPerspective cameraPerspective) : base(cameraPerspective)
        {
            if (cameraPerspective == null) return;

            AspectRatio = cameraPerspective.AspectRatio;
            YFov = cameraPerspective.YFov;
            ZFar = cameraPerspective.ZFar;
            ZNear = cameraPerspective.ZNear;
        }

        public static CameraPerspective Deserialize(GLTFRoot root, JsonReader reader)
        {
            var cameraPerspective = new CameraPerspective();

            if (reader.Read() && reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Perspective camera must be an object.");
            }

            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case "aspectRatio":
                        cameraPerspective.AspectRatio = reader.ReadAsFloat();
                        break;
                    case "yfov":
                        cameraPerspective.YFov = reader.ReadAsFloat();
                        break;
                    case "zfar":
                        cameraPerspective.ZFar = reader.ReadAsFloat();
                        break;
                    case "znear":
                        cameraPerspective.ZNear = reader.ReadAsFloat();
                        break;
                    default:
                        cameraPerspective.DefaultPropertyDeserializer(root, reader);
                        break;
                }
            }

            return cameraPerspective;
        }

        public override void Serialize(JsonWriter writer)
        {
            writer.WriteStartObject();

            if (AspectRatio != 0)
            {
                writer.WritePropertyName("aspectRatio");
                writer.WriteValue(AspectRatio);
            }

            writer.WritePropertyName("yfov");
            writer.WriteValue(YFov);

            if (ZFar != double.PositiveInfinity)
            {
                writer.WritePropertyName("zfar");
                writer.WriteValue(ZFar);
            }

            writer.WritePropertyName("znear");
            writer.WriteValue(ZNear);

            base.Serialize(writer);

            writer.WriteEndObject();
        }
    }
}
