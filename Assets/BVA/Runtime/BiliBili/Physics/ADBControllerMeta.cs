using ADBRuntime.Mono;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public class ADBControllerMeta
    {
        public bool isRunAsync = false;
        public bool isParallel = false;
        public bool isDebug=true;
        public bool isOptimize = false;

        public int iteration = 4;
        public float bufferTime = 1f;
        public float windForceScale = 0f;
        public UpdateMode updateMode = UpdateMode.Update;
        public ColliderCollisionType colliderCollisionType = ColliderCollisionType.Constraint;

        public ADBControllerMeta(ADBRuntimeController runtimeController)
        {
            isRunAsync = runtimeController.isRunAsync;
            isParallel = runtimeController.isParallel;
            isDebug = runtimeController.isDrawGizmo;
            isOptimize = runtimeController.isOptimize;

            iteration = runtimeController.iteration;
            bufferTime = runtimeController.bufferTime;

            updateMode = runtimeController.updateMode;
            colliderCollisionType = runtimeController.colliderCollisionType;
        }

        public ADBControllerMeta()
        {
        }

        public void Deserialize(GameObject go)
        {
            var controller = go.AddComponent<ADBRuntimeController>();
            controller.isRunAsync = isRunAsync;
            controller.isParallel = isParallel;
            controller.isDrawGizmo = isDebug;
            controller.isOptimize = isOptimize;
            controller.iteration = iteration;
            controller.bufferTime = bufferTime;

            controller.updateMode = updateMode;
            controller.colliderCollisionType = colliderCollisionType;

        }
    }
}