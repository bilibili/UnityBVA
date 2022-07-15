#if PUERTS_INTEGRATION
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

        public void ImportPuertsEvent(BVA_puerts_monoExtension ext, GameObject nodeObj, AssetCache assetCache)
        {
            ext.puertsEventMeta.Deserialize(nodeObj, assetCache);
        }

        private void LoadPuertsEventsComponent(Node node, GameObject nodeObj)
        {
            if (hasValidExtension(node, BVA_puerts_monoExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_puerts_monoExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_puerts_monoExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_puerts_monoExtensionFactory)} failed");
                foreach (var v in impl.ids)
                {
                    var puertsEventExt = _gltfRoot.Extensions.PuertsEvents[v.Id];
                    if (puertsEventExt != null)
                        ImportPuertsEvent(puertsEventExt, nodeObj, _assetCache);
                }
            }
        }
    }

    public partial class GLTFSceneExporter
    {

        private PuertsMonoId ExportPuertsMonos(MonoBehaviour puertsEvent)
        {
            BVA_puerts_monoExtension ext = new BVA_puerts_monoExtension(puertsEvent, _nodeCache);

            var id = new PuertsMonoId
            {
                Id = _root.Extensions.PuertsEvents.Count,
                Root = _root
            };
            _root.Extensions.AddPuertsEvent(ext);
            return id;
        }

        private void ExportPuertsEvent()
        {
            // add dynamicBone
            _puertsMonos = _rootTransforms.SelectMany(x => x.GetComponentsInChildren<MonoBehaviour>())
                .Where(x => x is PuertsEvent || x is PuertsCall).ToList();

            if (_puertsMonos != null && _puertsMonos.Count > 0)
            {
                List<PuertsMonoId>[] idsArray = new List<PuertsMonoId>[_root.Nodes.Count];
                for (int i = 0; i < _puertsMonos.Count; i++)
                {
                    int targetID = _nodeCache.GetId(_puertsMonos[i].gameObject);
                    if (idsArray[targetID] == null)
                    {
                        idsArray[targetID] = new List<PuertsMonoId>();
                    }
                    var id = ExportPuertsMonos(_puertsMonos[i]);
                    idsArray[targetID].Add(id);
                    //
                }
                for (int i = 0; i < idsArray.Length; i++)
                {
                    var ids = idsArray[i];
                    if (ids != null)
                    {
                        Node node = _root.Nodes[i];
                        node.AddExtension(_root, BVA_puerts_monoExtensionFactory.EXTENSION_NAME, new BVA_puerts_monoExtensionFactory(ids), RequireExtensions);
                    }
                }
            }
        }
    }
}
#endif