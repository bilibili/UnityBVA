#if PUERTS_INTEGRATION
using BVA;
using GLTF.Extensions;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using ADBRuntime;
using ADBRuntime.Mono;
using BVA;
using BVA.Component;

namespace GLTF.Schema.BVA
{
    public class BVA_puerts_monoExtension : IExtension
    {
        public PuertsMonoMeta puertsEventMeta;

        public BVA_puerts_monoExtension(MonoBehaviour puertsEvent, NodeCache _nodeCache)
        {
            puertsEventMeta = new PuertsMonoMeta(puertsEvent, _nodeCache);
        }
        public BVA_puerts_monoExtension(PuertsMonoMeta puertsEventMeta)
        {
            this.puertsEventMeta = puertsEventMeta;
        }
        public BVA_puerts_monoExtension() { }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_puerts_monoExtension(puertsEventMeta);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var v = obj as BVA_puerts_monoExtension;
            return v.puertsEventMeta.Equals(puertsEventMeta);
        }
        public JProperty Serialize()
        {
            JObject jo = puertsEventMeta.Serialize();
            JProperty jProperty = new JProperty(BVA_collisions_colliderExtensionFactory.EXTENSION_NAME, jo);

            return jProperty;
        }

        public static BVA_puerts_monoExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            PuertsMonoMeta targetMeta = PuertsMonoMeta.Deserialize(root,reader);
            return new BVA_puerts_monoExtension(targetMeta);
        } 
    }

    public class BVA_puerts_monoExtensionFactory : ExtensionFactory, IExtension
        {
            public const string EXTENSION_NAME = "BVA_puerts_event";
            public const string EXTENSION_ELEMENT_NAME = "puerts_event";
            public List<PuertsMonoId> ids;
            public BVA_puerts_monoExtensionFactory()
            {
                ExtensionName = EXTENSION_NAME;
                ElementName = EXTENSION_ELEMENT_NAME;
            }
            public BVA_puerts_monoExtensionFactory(List<PuertsMonoId> _id)
            {
                ExtensionName = EXTENSION_NAME;
                ElementName = EXTENSION_ELEMENT_NAME;
                ids = _id;
            }
            public IExtension Clone(GLTFRoot root)
            {
                return new BVA_puerts_monoExtensionFactory() { ids = ids };
            }
            public JProperty Serialize()
            {
                var array = new JArray();
                foreach (var v in ids)
                {
                    array.Add(v.Id);
                }
                return new JProperty(EXTENSION_NAME, new JObject(new JProperty(EXTENSION_ELEMENT_NAME, array)));
            }

            public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
            {
                List<int> id = new List<int>();
                if (extensionToken != null)
                {
                    JToken indexToken = extensionToken.Value[EXTENSION_ELEMENT_NAME];
                    id = indexToken != null ? indexToken.DeserializeAsIntList() : id;
                }
                List<PuertsMonoId> li = new List<PuertsMonoId>();
                foreach (var v in id)
                {
                    li.Add(new PuertsMonoId() { Id = v, Root = root });
                }
                return new BVA_puerts_monoExtensionFactory(li);
            }
        }
    
}
#endif