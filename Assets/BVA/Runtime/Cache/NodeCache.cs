using System;
using System.Collections.Generic;
using UnityEngine;
using BVA.Extensions;
namespace BVA
{
	public class NodeCache : IDisposable
	{
		private Dictionary<GameObject, int> LoadedGameObjectsReversed { get; set; }
		private Dictionary<int, GameObject> LoadedGameObjects { get; set; }
		public NodeCache()
		{
			LoadedGameObjectsReversed = new Dictionary<GameObject, int>();
			LoadedGameObjects = new Dictionary<int, GameObject>();
		}
		public int GetId(GameObject gameObject)
		{
            if (LoadedGameObjectsReversed.TryGetValue(gameObject, out int id))
			{
				return id;
			}
			return -1;
		}
		public void Add(int id, GameObject obj)
		{
			LoadedGameObjects.Add(id, obj);
			LoadedGameObjectsReversed.Add(obj, id);
		}
		public int Count => LoadedGameObjects.Count;
		public GameObject GetGameObject(int id)
		{
			GameObject obj = null;
			LoadedGameObjects.TryGetValue(id, out obj);
			return obj;
		}
		/// <summary>
		/// make sure that no gameObject use the same name 
		/// </summary>
		/// <param name="root"></param>
		/// <param name=""></param>
		public int GetNodeByBindingPath(Transform root, string bindingPath)
		{
			if (root.name == bindingPath)
			{
				return GetId(root.gameObject);
			}
			Transform finded = root.DeepFindChild(bindingPath);
			if (finded != null)
			{
				return GetId(finded.gameObject);
			}
			return -1;
		}

		public void Dispose()
		{
			if (LoadedGameObjects != null)
			{
				//UnityEngine.Object.Destroy(LoadedGameObject);
				LoadedGameObjects = null;
			}
		}
	}
}