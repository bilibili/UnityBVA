using GLTF.Schema;
using System.Collections.Generic;
using UnityEngine;
using BVA.Extensions;
using GLTF.Schema.BVA;
using ADBRuntime.Mono;
using System.Linq;

namespace BVA
{
    using Cache;
    using System;
    using BVA.Component;

    public partial class GLTFSceneImporter
    {
        public void ImportDynamicBone(BVA_physics_dynamicBoneExtension ext, GameObject nodeObj, AssetCache assetCache)
        {
            ext.metaInfo.Deserialize(nodeObj, assetCache);
        }

        private void LoadPhysicsComponent(Node node, GameObject nodeObj)
        {
            if (hasValidExtension(node, BVA_physics_dynamicBoneExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_physics_dynamicBoneExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_physics_dynamicBoneExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_physics_dynamicBoneExtensionFactory)} failed");
                foreach (var v in impl.ids)
                {
                    var collisionExt = _gltfRoot.Extensions.DynamicBones[v.Id];
                    if (collisionExt != null)
                        ImportDynamicBone(collisionExt, nodeObj, _assetCache);
                }
            }
        }
    }

    public partial class GLTFSceneExporter
    {
        private Physics_dynamicBoneID ExportDynamicBone(IADBPhysicMonoComponent dynamicBone)
        {
            BVA_physics_dynamicBoneExtension ext = new BVA_physics_dynamicBoneExtension(dynamicBone, _nodeCache);

            var id = new Physics_dynamicBoneID
            {
                Id = _root.Extensions.DynamicBones.Count,
                Root = _root
            };
            _root.Extensions.AddDynamicBone(ext);
            return id;
        }

        private void ExportDynamicBone()
        {
            // add dynamicBone
            _dynamicBones = _rootTransforms.SelectMany(x => x.GetComponentsInChildren<ADBRuntime.Mono.IADBPhysicMonoComponent>()).ToList();

            if (_dynamicBones != null&& _dynamicBones.Count > 0)
            {
                List <Physics_dynamicBoneID>[] idsArray = new List<Physics_dynamicBoneID>[_root.Nodes.Count];
                for (int i = 0; i < _dynamicBones.Count; i++)
                {
                    int targetID = _nodeCache.GetId(_dynamicBones[i].Target .gameObject);
                    if (idsArray[targetID] ==null)
                    {
                        idsArray[targetID] = new List<Physics_dynamicBoneID>();
                    }
                    var id = ExportDynamicBone(_dynamicBones[i]);
                    idsArray[targetID].Add(id);
                    //
                }
                for (int i = 0; i < idsArray.Length; i++)
                {
                    var ids = idsArray[i];
                    if (ids != null)
                    {
                        Node node = _root.Nodes[i];
                        node.AddExtension(_root, BVA_physics_dynamicBoneExtensionFactory.EXTENSION_NAME, new BVA_physics_dynamicBoneExtensionFactory(ids), RequireExtensions);
                    }
                }
            }
        }
    }
}