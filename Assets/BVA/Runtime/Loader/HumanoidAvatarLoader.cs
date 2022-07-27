using System.Collections.Generic;
using UnityEngine;
using GLTF.Schema.BVA;
using BVA.Cache;
using BVA.Extensions;

namespace BVA
{
    public class HumanoidAvatarLoader : AssetLoader
    {
        public List<Avatar> avatars;
        public HumanoidAvatarLoader(AssetCache cache) : base(cache)
        {
            avatars = new List<Avatar>();
        }
        private List<SkeletonBone> GetTransforms(Transform t)
        {
            SkeletonBone skeletonBone = new SkeletonBone()
            {
                name = t.name,
                position = t.localPosition,
                rotation = t.localRotation,
                scale = t.localScale
            };

            List<SkeletonBone> singleParentFamily = new List<SkeletonBone>() { skeletonBone };

            foreach (Transform childT in t)
            {
                singleParentFamily.AddRange(GetTransforms(childT));
            }

            return singleParentFamily;
        }
        public HumanDescription GetHumanDescription(Transform root, GltfAvatar human)
        {
            var skeletonBones = GetTransforms(root);
            var boneCount = human.humanBones.Count;
            var humanBones = new HumanBone[boneCount];
            for (int i = 0; i < boneCount; i++)
            {
                var bonelimit = human.humanBones[i];
                if (bonelimit.node < 0) continue;
                humanBones[i] = new HumanBone()
                {
                    boneName = assetCache.NodeCache[bonelimit.node].name,
                    humanName = bonelimit.bone,
                    limit = new HumanLimit()
                    {
                        useDefaultValues = bonelimit.useDefaultValues,
                        min = bonelimit.min.ToUnityVector3Raw(),
                        max = bonelimit.max.ToUnityVector3Raw(),
                        center = bonelimit.center.ToUnityVector3Raw(),
                        axisLength = bonelimit.axisLength
                    }
                };
            }

            return new HumanDescription
            {
                skeleton = skeletonBones.ToArray(),
                human = humanBones,
                armStretch = human.armStretch,
                legStretch = human.legStretch,
                upperArmTwist = human.upperArmTwist,
                lowerArmTwist = human.lowerArmTwist,
                upperLegTwist = human.upperLegTwist,
                lowerLegTwist = human.lowerLegTwist,
                feetSpacing = human.feetSpacing,
                hasTranslationDoF = human.hasTranslationDoF
            };
        }

        public Avatar CreateAvatar(Transform root, GltfAvatar human)
        {
            var humanDescription = GetHumanDescription(root, human);
            Avatar avatar = AvatarBuilder.BuildHumanAvatar(root.gameObject, humanDescription);
            if (!avatar.isValid || !avatar.isHuman)
            {
                Debug.LogError($"Error when building avatar");
            }
            return avatar;
        }

        public Avatar AddAvatar(GltfAvatar gltfAvatar, Transform root)
        {
            var avatar = CreateAvatar(root, gltfAvatar);
            avatars.Add(avatar);
            return avatar;
        }
        public Avatar GetAvatar(AvatarId id)
        {
            return avatars.Count <= id.Id ? null : avatars[id.Id];
        }
        public override void Dispose()
        {
            avatars = null;
        }
    }
}