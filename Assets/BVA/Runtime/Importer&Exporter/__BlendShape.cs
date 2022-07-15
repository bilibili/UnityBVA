using GLTF.Schema.BVA;
using UnityEngine;
using BVA.Component;
using GLTF.Schema;
using System;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public void LoadBlendShapeMixer(Node node, GameObject nodeObj)
        {
            if (hasValidExtension(node, BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_blendShape_blendShapeMixerExtensionFactory)ext;
                if (impl == null)
                    throw new Exception($"cast {nameof(BVA_blendShape_blendShapeMixerExtensionFactory)} failed");

                ImportBlendShapeMixer(impl.id, nodeObj);
            }
        }
        public void ImportBlendShapeMixer(BlendshapeMixerId id, GameObject gameObject)
        {
            var mixer = gameObject.AddComponent<BlendShapeMixer>();
            mixer.keys = _gltfRoot.Extensions.BlendShapeMixers[id.Id].keys;
            foreach (var key in mixer.keys)
            {
                foreach (var blendShape in key.blendShapeValues)
                {
                    blendShape.node = _assetCache.NodeCache[blendShape.nodeId].GetComponent<SkinnedMeshRenderer>();
                }
            }
        }
    }

    public partial class GLTFSceneExporter
    {
        private BlendshapeMixerId ExportBlendShapeMixer(BlendShapeMixer mixer)
        {
            var id = new BlendshapeMixerId
            {
                Id = _root.Extensions.BlendShapeMixers.Count,
                Root = _root
            };
            _root.Extensions.AddBlendShapeMixer(new BVA_blendShape_blendShapeMixerExtension(mixer.keys, _nodeCache));
            return id;
        }

        private void ExportBlendShapeMixer()
        {
            foreach (var mixer in _blendShapeMixers)
            {
                BlendshapeMixerId id = ExportBlendShapeMixer(mixer);
                var nodeId = _nodeCache.GetId(mixer.gameObject);
                _root.Nodes[nodeId].AddExtension(_root, BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_NAME, new BVA_blendShape_blendShapeMixerExtensionFactory(id), RequireExtensions);
            }
        }
    }
}