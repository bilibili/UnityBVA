using GLTF;
using GLTF.Schema;
using BVA.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        protected void SetupBones(Skin skin, SkinnedMeshRenderer renderer)
        {
            var boneCount = skin.Joints.Count;
            Transform[] bones = new Transform[boneCount];

            // TODO: build bindpose arrays only once per skin, instead of once per node
            Matrix4x4[] gltfBindPoses = null;
            if (skin.InverseBindMatrices != null)
            {
                int bufferId = skin.InverseBindMatrices.Value.BufferView.Value.Buffer.Id;
                AttributeAccessor attributeAccessor = new AttributeAccessor
                {
                    AccessorId = skin.InverseBindMatrices,
                    Stream = _assetCache.BufferCache[bufferId].Stream,
                    Offset = _assetCache.BufferCache[bufferId].ChunkOffset
                };

                GLTFHelpers.BuildBindPoseSamplers(ref attributeAccessor);
                gltfBindPoses = attributeAccessor.AccessorContent.AsMatrix4x4s;
            }

            Matrix4x4[] bindPoses = new Matrix4x4[boneCount];
            for (int i = 0; i < boneCount; i++)
            {
                int jointId = skin.Joints[i].Id;
                if (jointId < 0 || _assetCache.NodeCache.Length <= jointId)
                {
                    LogPool.ImportLogger.LogWarning(LogPart.Skin, "The skin node is not exist");
                    continue;
                }
                var node = _assetCache.NodeCache[jointId];
                if (node == null)
                {
                    LogPool.ImportLogger.LogWarning(LogPart.Skin, "The skin node is null");
                    continue;
                }
                bones[i] = node.transform;
                bindPoses[i] = gltfBindPoses != null ? gltfBindPoses[i].ToUnityMatrix4x4Convert() : Matrix4x4.identity;
            }

            if (skin.Skeleton != null)
            {
                var rootBoneNode = _assetCache.NodeCache[skin.Skeleton.Id];
                renderer.rootBone = rootBoneNode.transform;
            }
            else
            {
                var rootBoneId = GLTFHelpers.FindCommonAncestor(skin.Joints);
                if (rootBoneId != null)
                {
                    var rootBoneNode = _assetCache.NodeCache[rootBoneId.Id];
                    renderer.rootBone = rootBoneNode.transform;
                }
                else
                {
                    throw new ArgumentException("glTF skin joints do not share a root node!");
                }
            }
            renderer.sharedMesh.bindposes = bindPoses;
            renderer.bones = bones;
        }

        private void CreateBoneWeightArray(Vector4[] joints, Vector4[] weights, ref BoneWeight[] destArr, int offset = 0)
        {
            // normalize weights (built-in normalize function only normalizes three components)
            for (int i = 0; i < weights.Length; i++)
            {
                var weightSum = (weights[i].x + weights[i].y + weights[i].z + weights[i].w);

                if (!Mathf.Approximately(weightSum, 0))
                {
                    weights[i] /= weightSum;
                }
            }

            for (int i = 0; i < joints.Length; i++)
            {
                destArr[offset + i].boneIndex0 = (int)joints[i].x;
                destArr[offset + i].boneIndex1 = (int)joints[i].y;
                destArr[offset + i].boneIndex2 = (int)joints[i].z;
                destArr[offset + i].boneIndex3 = (int)joints[i].w;

                destArr[offset + i].weight0 = weights[i].x;
                destArr[offset + i].weight1 = weights[i].y;
                destArr[offset + i].weight2 = weights[i].z;
                destArr[offset + i].weight3 = weights[i].w;
            }
        }
    }
    public partial class GLTFSceneExporter
    {
        public void SetupBoneWeights(SkinnedMeshRenderer renderer, ref Vector4Int[] joints, ref Vector4[] weights)
        {
            Mesh mesh = renderer.sharedMesh;
            List<BoneWeight> boneWeights = new List<BoneWeight>();
            mesh.GetBoneWeights(boneWeights);

            for (int i = 0; i < boneWeights.Count; i++)
            {
                joints[i] = new Vector4Int(boneWeights[i].boneIndex0, boneWeights[i].boneIndex1, boneWeights[i].boneIndex2, boneWeights[i].boneIndex3);
                weights[i] = new Vector4(boneWeights[i].weight0, boneWeights[i].weight1, boneWeights[i].weight2, boneWeights[i].weight3);
            }
        }
        /// <summary>
        /// export skin when export mesh with SkinMeshRenderer
        /// </summary>
        /// <param name="render"></param>
        /// <returns></returns>
        public SkinId ExportSkin(SkinnedMeshRenderer render)
        {
            // skeleton is the root bone transform in Unity,and in gltf,it point to a node ,get it from _nodeCache
            bool isValidSkin = true;
            Skin skin = new Skin();
            skin.Skeleton = new NodeId() { Root = _root, Id = _nodeCache.GetId(render.rootBone.gameObject) };
            if (skin.Skeleton.Id < 0)
            {
                Debug.LogError("The Skin you try to export has some bone transform that are not exist. " + render.rootBone.gameObject.name);
                isValidSkin = false;
            }
            // joints is point to a list of node
            List<NodeId> joints = new List<NodeId>();
            foreach (var bone in render.bones)
            {
                int boneId = _nodeCache.GetId(bone.gameObject);
                if (boneId < 0)
                {
                    Debug.LogError("The Skin you try to export has some bone transform that are not exist. " + bone.gameObject.name);
                    isValidSkin = false;
                }
                joints.Add(new NodeId() { Root = _root, Id = boneId });
            }

            if (!isValidSkin) 
                return null;

            skin.Joints = joints;

            //mesh bindpose 
            //var boneCount = skin.Joints.Count;
            //Transform[] bones = new Transform[boneCount];
            var bindPoses = render.sharedMesh.bindposes;
            //reverse z coordinate
            for (int i = 0; i < bindPoses.Length; i++)
                bindPoses[i] = bindPoses[i].ReverseX();
            skin.InverseBindMatrices = ExportAccessor(bindPoses);
            var id = new SkinId
            {
                Id = _root.Skins.Count,
                Root = _root
            };

            _root.Skins.Add(skin);
            return id;
        }
    }
}