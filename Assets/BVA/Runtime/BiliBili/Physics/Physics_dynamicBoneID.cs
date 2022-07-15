using GLTF.Schema.BVA;
using Newtonsoft.Json;

namespace GLTF.Schema
{
	public class Physics_dynamicBoneID : GLTFId<BVA_physics_dynamicBoneExtension>
	{
		public Physics_dynamicBoneID()
		{
		}

		public Physics_dynamicBoneID(Physics_dynamicBoneID id, GLTFRoot newRoot) : base(id, newRoot)
		{
		}

		public override BVA_physics_dynamicBoneExtension Value
		{
			get { return Root.Extensions.DynamicBones[Id]; }
		}

		public static Physics_dynamicBoneID Deserialize(GLTFRoot root, JsonReader reader)
		{
			return new Physics_dynamicBoneID
			{
				Id = reader.ReadAsInt32().Value,
				Root = root
			};
		}
	}
}