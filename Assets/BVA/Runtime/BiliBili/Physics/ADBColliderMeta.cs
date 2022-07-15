using ADBRuntime;
using ADBRuntime.Mono;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public class ADBColliderMeta
    {
        public CollideFunc collideFunc;
        public ColliderChoice colliderChoice;
        private ADBColliderReader colliderReader;

        public ADBColliderMeta(ADBColliderReader colliderReader)
        {
            collideFunc = colliderReader.collideFunc;
            colliderChoice = colliderReader.colliderMask;
        }
        public ADBColliderMeta()
        {
        }

        public void Deserialize(GameObject go)
        {
            var reader = go.AddComponent<ADBColliderReader>();
            reader.collideFunc = collideFunc;
            reader.colliderMask = colliderChoice;
        }
    }
}