using GLTF.Extensions;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema
{
    public class KHR_texture_transformExtension : IExtension
	{
		/// <summary>
		/// The offset of the UV coordinate origin as a percentage of the texture dimensions.
		/// </summary>
		public Vector2 Offset = new Vector2(0, 0);
		public static readonly Vector2 OFFSET_DEFAULT = new Vector2(0, 0);

		/// <summary>
		/// Rotate the UVs by this many radians counter-clockwise around the origin. This is equivalent
		/// to a similar rotation of the image clockwise.
		/// </summary>
		public float Rotation = 0.0f;
		public static readonly float ROTATION_DEFAULT = 0.0f;

		/// <summary>
		/// The scale factor applied to the components of the UV coordinates.
		/// </summary>
		public Vector2 Scale = new Vector2(1, 1);
		public static readonly Vector2 SCALE_DEFAULT = new Vector2(1, 1);

		/// <summary>
		/// Overrides the textureInfo texCoord value if this extension is supported.
		/// </summary>
		public int TexCoord = 0;
		public static readonly int TEXCOORD_DEFAULT = 0;

		public KHR_texture_transformExtension(Vector2 offset, float rotation, Vector2 scale, int texCoord)
		{
			Offset = offset;
			Rotation = rotation;
			Scale = scale;
			TexCoord = texCoord;
		}

		public IExtension Clone(GLTFRoot root)
		{
			return new KHR_texture_transformExtension(Offset, Rotation, Scale, TexCoord);
		}

		public JProperty Serialize()
		{
			JObject ext = new JObject();

			if (Offset != OFFSET_DEFAULT)
			{
				ext.Add(new JProperty(
					KHR_texture_transformExtensionFactory.OFFSET,
					new JArray(Offset.x, Offset.y)
				));
			}

			if (Rotation != ROTATION_DEFAULT)
			{
				ext.Add(new JProperty(
					KHR_texture_transformExtensionFactory.ROTATION,
					Rotation
				));
			}

			if (Scale != SCALE_DEFAULT)
			{
				ext.Add(new JProperty(
					KHR_texture_transformExtensionFactory.SCALE,
					new JArray(Scale.x, Scale.y)
				));
			}

			if (TexCoord != TEXCOORD_DEFAULT)
			{
				ext.Add(new JProperty(
					KHR_texture_transformExtensionFactory.TEXCOORD,
					TexCoord
				));
			}

			return new JProperty(KHR_texture_transformExtensionFactory.EXTENSION_NAME, ext);
		}
	}
	public class KHR_texture_transformExtensionFactory : ExtensionFactory
	{
		public const string EXTENSION_NAME = "KHR_texture_transform";
		public const string OFFSET = "offset";
		public const string ROTATION = "rotation";
		public const string SCALE = "scale";
		public const string TEXCOORD = "texCoord";

		public KHR_texture_transformExtensionFactory()
		{
			ExtensionName = EXTENSION_NAME;
		}

		public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
		{
			Vector2 offset = KHR_texture_transformExtension.OFFSET_DEFAULT;
			float rotation = 0;
			Vector2 scale = KHR_texture_transformExtension.SCALE_DEFAULT;
			int texCoord = KHR_texture_transformExtension.TEXCOORD_DEFAULT;

			if (extensionToken != null)
			{
				JToken offsetToken = extensionToken.Value[OFFSET];
				offset = offsetToken != null ? offsetToken.DeserializeAsVector2() : offset;

				JToken rotationToken = extensionToken.Value[ROTATION];
				rotation = rotationToken != null ? rotationToken.DeserializeAsFloat() : rotation;

				JToken scaleToken = extensionToken.Value[SCALE];
				scale = scaleToken != null ? scaleToken.DeserializeAsVector2() : scale;

				JToken texCoordToken = extensionToken.Value[TEXCOORD];
				texCoord = texCoordToken != null ? texCoordToken.DeserializeAsInt() : texCoord;
			}

			return new KHR_texture_transformExtension(offset, rotation, scale, texCoord);
		}
	}
}
