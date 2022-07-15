using UnityEngine;
using BVA.Loader;

namespace BVA
{
	public abstract class ImporterFactory : ScriptableObject
	{
		public abstract GLTFSceneImporter CreateSceneImporter(string gltfFileName, ImportOptions options);
	}

	public class DefaultImporterFactory : ImporterFactory
	{
		public override GLTFSceneImporter CreateSceneImporter(string gltfFileName, ImportOptions options)
		{
			return new GLTFSceneImporter(gltfFileName, options);
		}
	}
}
