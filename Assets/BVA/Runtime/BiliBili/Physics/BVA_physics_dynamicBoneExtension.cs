using BVA;
using GLTF.Extensions;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using ADBRuntime;
using ADBRuntime.Mono;

namespace GLTF.Schema.BVA
{
    public class BVA_physics_dynamicBoneExtension : IExtension
    {
        public BVA_physics_dynamicBoneMeta metaInfo;
        private IADBPhysicMonoComponent dynamicBone;

        public BVA_physics_dynamicBoneExtension(IADBPhysicMonoComponent component, NodeCache nodeCache)
        {
            metaInfo = new BVA_physics_dynamicBoneMeta(component, nodeCache);

        }

        public BVA_physics_dynamicBoneExtension(BVA_physics_dynamicBoneMeta metaInfo)
        {
            this.metaInfo = metaInfo;
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_physics_dynamicBoneExtension(metaInfo);
        }

        public JProperty Serialize()
        {
            JProperty jProperty = new JProperty(BVA_physics_dynamicBoneExtensionFactory.EXTENSION_NAME, metaInfo.SerializeData());
            return jProperty;
        }

        public static BVA_physics_dynamicBoneExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            BVA_physics_dynamicBoneMeta meta = BVA_physics_dynamicBoneMeta.Deserialize(root, reader);
            return new BVA_physics_dynamicBoneExtension(meta);
        }
    }

    public class BVA_physics_dynamicBoneExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_physics_dynamicBone";
        public const string EXTENSION_ELEMENT_NAME = "physics_dynamicBone";
        public List<Physics_dynamicBoneID> ids;
        public BVA_physics_dynamicBoneExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_physics_dynamicBoneExtensionFactory(List<Physics_dynamicBoneID> ids)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            this.ids = ids;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_physics_dynamicBoneExtensionFactory() { ids = ids };
        }

        public JProperty Serialize()
        {
            var array = new JArray();
            foreach (var v in ids)
            {
                array.Add(v.Id);
            }
            JProperty node = new JProperty(EXTENSION_ELEMENT_NAME, array);
            return new JProperty(EXTENSION_NAME, new JObject(node));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            List<int> idInt = new List<int>();
            ids = new List<Physics_dynamicBoneID>();
            if (extensionToken != null)
            {
                JToken indexToken = extensionToken.Value[EXTENSION_ELEMENT_NAME];
                idInt = indexToken != null ? indexToken.DeserializeAsIntList() : idInt;
            }
            foreach (var v in idInt)
            {
                ids.Add(new Physics_dynamicBoneID() { Id = v, Root = root });
            }
            return new BVA_physics_dynamicBoneExtensionFactory(ids);

        }
    }

}