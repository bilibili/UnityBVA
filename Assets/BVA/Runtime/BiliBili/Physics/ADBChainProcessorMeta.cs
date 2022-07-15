using ADBRuntime;
using ADBRuntime.Mono;
using UnityEngine;
using BVA;
using BVA.Cache;

namespace GLTF.Schema.BVA
{
    public class ADBChainProcessorMeta
    {
        /// <summary>
        /// all point's transform index
        /// </summary>
        internal int[] transformIndex;
        /// <summary>
        /// all point's parent point index
        /// </summary>
        internal int[] pointParentIndex;
        /// <summary>
        /// all point's colliderMask
        /// </summary>
        internal int[] pointColliderMasks;
        /// <summary>
        /// all point's
        /// </summary>
        internal float[] pointHitRadiuss;
        /// <summary>
        /// bone chain
        /// </summary>
        private ADBChainProcessor chainProcessor;
        /// <summary>
        ///  settingFile
        /// </summary>
        public ADBPhysicsSetting physicsSetting;
        public string keyWord;

        public ADBChainProcessorMeta(ADBChainProcessor chainProcessor, NodeCache nodeCache)
        {
            keyWord = chainProcessor.keyWord;
            physicsSetting = chainProcessor.GetADBSetting();
            this.chainProcessor = chainProcessor;
            transformIndex = new int[chainProcessor.allPointList.Count];
            pointParentIndex = new int[chainProcessor.allPointList.Count];
            if (chainProcessor.isUseLocalRadiusAndColliderMask)
            {
                pointColliderMasks = new int[chainProcessor.allPointList.Count];
                pointHitRadiuss = new float[chainProcessor.allPointList.Count];
            }
            for (int i = 0; i < chainProcessor.allPointList.Count; i++)
            {
                var point = chainProcessor.allPointList[i];
                transformIndex[i] = nodeCache.GetId(point.gameObject);
                pointParentIndex[i] = point.pointRead.parentIndex;
                if (chainProcessor.isUseLocalRadiusAndColliderMask)
                {
                    pointColliderMasks[i] = point.pointRead.colliderMask;
                    pointHitRadiuss[i] = point.pointRead.radius; 
                }
            }
        }

        public ADBChainProcessorMeta(string keyWord, int[] transformIndex, int[] pointParentIndex, ADBPhysicsSetting physicsSetting, int[] pointColliderMasks, float[] pointHitRadiuss)
        {
            this.keyWord = keyWord;
            this.transformIndex = transformIndex;
            this.pointParentIndex = pointParentIndex;
            this.physicsSetting = physicsSetting;
            this.pointColliderMasks = pointColliderMasks;
            this.pointHitRadiuss = pointHitRadiuss;
        }

        internal void Deserialize(GameObject nodeObj,AssetCache assetCache)
        {
            chainProcessor = ADBChainProcessor.CreateADBChainProcessor(nodeObj.transform, keyWord, physicsSetting);
            chainProcessor.isUseLocalRadiusAndColliderMask = pointColliderMasks != null;

            ADBRuntimePoint[] runtimePoints = new ADBRuntimePoint[transformIndex.Length];
            for (int i = 0; i < transformIndex.Length; i++)
            {

                GameObject pointTrans = assetCache.NodeCache[transformIndex[i]];
                ADBRuntimePoint parent = pointParentIndex[i] == -1 ? chainProcessor : runtimePoints[pointParentIndex[i]];
                runtimePoints[i] = ADBRuntimePoint.CreateRuntimePoint(pointTrans.transform, parent.depth + 1, chainProcessor.keyWord);

                parent.AddChild(runtimePoints[i]);
                if (chainProcessor.isUseLocalRadiusAndColliderMask)
                {
                    runtimePoints[i].pointRead.colliderMask = pointColliderMasks[i];
                    runtimePoints[i].pointRead.radius = pointHitRadiuss[i];
                }


            }
            //OYM:²âÊÔ´úÂë
            chainProcessor.Initialize();
        }
    }
}