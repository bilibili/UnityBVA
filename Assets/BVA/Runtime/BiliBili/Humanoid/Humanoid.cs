using BVA.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GLTF.Schema;

namespace BVA
{
    
    struct Channel
    {
        /// <summary>
        /// Animation GameObject position in hierachy
        /// </summary>
        public string path;
        /// <summary>
        /// BlendShape Name or Bone Name with Humanoid Animation
        /// </summary>
        public string blendShapeNameOrHumanoidBoneName;
        public GLTFAnimationChannelPath chanelType;
        public Channel(string t, GLTFAnimationChannelPath p)
        {
            path = t;
            chanelType = p;
            blendShapeNameOrHumanoidBoneName = null;
        }
        public Channel(string t, GLTFAnimationChannelPath p, string w)
        {
            path = t;
            chanelType = p;
            blendShapeNameOrHumanoidBoneName = w;
        }
        public static GLTFAnimationChannelPath GetStandardChannelType(string propertyName)
        {
            if (propertyName.StartsWith("m_LocalPosition.")) return GLTFAnimationChannelPath.translation;
            if (propertyName.StartsWith("m_LocalRotation.")) return GLTFAnimationChannelPath.rotation;
            if (propertyName.StartsWith("localEulerAnglesRaw.")) return GLTFAnimationChannelPath.eulerAngle;
            if (propertyName.StartsWith("m_LocalScale.")) return GLTFAnimationChannelPath.scale;
            if (propertyName.StartsWith("blendShape.")) return GLTFAnimationChannelPath.weights;
            return GLTFAnimationChannelPath.not_implement;
        }
        public static GLTFAnimationChannelPath GetChannelType(string propertyName)
        {
            if (propertyName.StartsWith("m_LocalPosition.")) return GLTFAnimationChannelPath.translation;
            if (propertyName.StartsWith("m_LocalRotation.")) return GLTFAnimationChannelPath.rotation;
            if (propertyName.StartsWith("localEulerAnglesRaw.")) return GLTFAnimationChannelPath.eulerAngle;
            if (propertyName.StartsWith("m_LocalScale.")) return GLTFAnimationChannelPath.scale;
            if (propertyName.StartsWith("blendShape.")) return GLTFAnimationChannelPath.weights;
            if (Humanoid.IsHumanMuscle(propertyName)) return GLTFAnimationChannelPath.animator_muscle;
            var (hasHumanoidAnimation, _, TRS, _) = Humanoid.ExtractHumanBone(propertyName);
            if (hasHumanoidAnimation)
            {
                switch (TRS[0])
                {
                    case 'T':
                        return GLTFAnimationChannelPath.animator_T;
                    case 'Q':
                        return GLTFAnimationChannelPath.animator_Q;
                    case 'S':
                        return GLTFAnimationChannelPath.animator_S;
                }
            }
            return GLTFAnimationChannelPath.not_implement;
        }
        public static bool IsStandardGltfAnimationChannel(GLTFAnimationChannelPath channlType)
        {
            return channlType == GLTFAnimationChannelPath.translation || channlType == GLTFAnimationChannelPath.rotation || channlType == GLTFAnimationChannelPath.scale || channlType == GLTFAnimationChannelPath.weights;
        }
    }
    public static class Humanoid
    {
        public static readonly List<string> MuscleEndWith = new List<string>() { "Front-Back", "Left-Right", "Down-Up", "Up-Down", "In-Out", "Close", "Stretch", "Stretched", "Spread" };
        public static readonly List<string> MuscleProperties = new List<string>() {
        "Spine Front-Back",
        "Spine Left-Right",
        "Spine Twist Left-Right",
        "Chest Front-Back",
        "Chest Left-Right",
        "Chest Twist Left-Right",
        "UpperChest Front-Back",
        "UpperChest Left-Right",
        "UpperChest Twist Left-Right",
        "Neck Nod Down-Up",
        "Neck Tilt Left-Right",
        "Neck Turn Left-Right",
        "Head Nod Down-Up",
        "Head Tilt Left-Right",
        "Head Turn Left-Right",
        "Left Eye Down-Up",
        "Left Eye In-Out",
        "Right Eye Down-Up",
        "Right Eye In-Out",
        "Jaw Close",
        "Jaw Left-Right",
        "Left Upper Leg Front-Back",
        "Left Upper Leg In-Out",
        "Left Upper Leg Twist In-Out",
        "Left Lower Leg Stretch",
        "Left Lower Leg Twist In-Out",
        "Left Foot Up-Down",
        "Left Foot Twist In-Out",
        "Left Toes Up-Down",
        "Right Upper Leg Front-Back",
        "Right Upper Leg In-Out",
        "Right Upper Leg Twist In-Out",
        "Right Lower Leg Stretch",
        "Right Lower Leg Twist In-Out",
        "Right Foot Up-Down",
        "Right Foot Twist In-Out",
        "Right Toes Up-Down",
        "Left Shoulder Down-Up",
        "Left Shoulder Front-Back",
        "Left Arm Down-Up",
        "Left Arm Front-Back",
        "Left Arm Twist In-Out",
        "Left Forearm Stretch",
        "Left Forearm Twist In-Out",
        "Left Hand Down-Up",
        "Left Hand In-Out",
        "Right Shoulder Down-Up",
        "Right Shoulder Front-Back",
        "Right Arm Down-Up",
        "Right Arm Front-Back",
        "Right Arm Twist In-Out",
        "Right Forearm Stretch",
        "Right Forearm Twist In-Out",
        "Right Hand Down-Up",
        "Right Hand In-Out",
        "LeftHand.Thumb.1 Stretched",
        "LeftHand.Thumb.Spread",
        "LeftHand.Thumb.2 Stretched",
        "LeftHand.Thumb.3 Stretched",
        "LeftHand.Index.1 Stretched",
        "LeftHand.Index.Spread",
        "LeftHand.Index.2 Stretched",
        "LeftHand.Index.3 Stretched",
        "LeftHand.Middle.1 Stretched",
        "LeftHand.Middle.Spread",
        "LeftHand.Middle.2 Stretched",
        "LeftHand.Middle.3 Stretched",
        "LeftHand.Ring.1 Stretched",
        "LeftHand.Ring.Spread",
        "LeftHand.Ring.2 Stretched",
        "LeftHand.Ring.3 Stretched",
        "LeftHand.Little.1 Stretched",
        "LeftHand.Little.Spread",
        "LeftHand.Little.2 Stretched",
        "LeftHand.Little.3 Stretched",
        "RightHand.Thumb.1 Stretched",
        "RightHand.Thumb.Spread",
        "RightHand.Thumb.2 Stretched",
        "RightHand.Thumb.3 Stretched",
        "RightHand.Index.1 Stretched",
        "RightHand.Index.Spread",
        "RightHand.Index.2 Stretched",
        "RightHand.Index.3 Stretched",
        "RightHand.Middle.1 Stretched",
        "RightHand.Middle.Spread",
        "RightHand.Middle.2 Stretched",
        "RightHand.Middle.3 Stretched",
        "RightHand.Ring.1 Stretched",
        "RightHand.Ring.Spread",
        "RightHand.Ring.2 Stretched",
        "RightHand.Ring.3 Stretched",
        "RightHand.Little.1 Stretched",
        "RightHand.Little.Spread",
        "RightHand.Little.2 Stretched",
        "RightHand.Little.3 Stretched",
        "Spine Front-Back",
        "Spine Left-Right",
        "Spine Twist Left-Right",
        "Chest Front-Back",
        "Chest Left-Right",
        "Chest Twist Left-Right",
        "UpperChest Front-Back",
        "UpperChest Left-Right",
        "UpperChest Twist Left-Right",
        "Neck Nod Down-Up",
        "Neck Tilt Left-Right",
        "Neck Turn Left-Right",
        "Head Nod Down-Up",
        "Head Tilt Left-Right",
        "Head Turn Left-Right",
        "Left Eye Down-Up",
        "Left Eye In-Out",
        "Right Eye Down-Up",
        "Right Eye In-Out",
        "Jaw Close",
        "Jaw Left-Right",
        "Left Upper Leg Front-Back",
        "Left Upper Leg In-Out",
        "Left Upper Leg Twist In-Out",
        "Left Lower Leg Stretch",
        "Left Lower Leg Twist In-Out",
        "Left Foot Up-Down",
        "Left Foot Twist In-Out",
        "Left Toes Up-Down",
        "Right Upper Leg Front-Back",
        "Right Upper Leg In-Out",
        "Right Upper Leg Twist In-Out",
        "Right Lower Leg Stretch",
        "Right Lower Leg Twist In-Out",
        "Right Foot Up-Down",
        "Right Foot Twist In-Out",
        "Right Toes Up-Down",
        "Left Shoulder Down-Up",
        "Left Shoulder Front-Back",
        "Left Arm Down-Up",
        "Left Arm Front-Back",
        "Left Arm Twist In-Out",
        "Left Forearm Stretch",
        "Left Forearm Twist In-Out",
        "Left Hand Down-Up",
        "Left Hand In-Out",
        "Right Shoulder Down-Up",
        "Right Shoulder Front-Back",
        "Right Arm Down-Up",
        "Right Arm Front-Back",
        "Right Arm Twist In-Out",
        "Right Forearm Stretch",
        "Right Forearm Twist In-Out",
        "Right Hand Down-Up",
        "Right Hand In-Out",
        "LeftHand.Thumb.1 Stretched",
        "LeftHand.Thumb.Spread",
        "LeftHand.Thumb.2 Stretched",
        "LeftHand.Thumb.3 Stretched",
        "LeftHand.Index.1 Stretched",
        "LeftHand.Index.Spread",
        "LeftHand.Index.2 Stretched",
        "LeftHand.Index.3 Stretched",
        "LeftHand.Middle.1 Stretched",
        "LeftHand.Middle.Spread",
        "LeftHand.Middle.2 Stretched",
        "LeftHand.Middle.3 Stretched",
        "LeftHand.Ring.1 Stretched",
        "LeftHand.Ring.Spread",
        "LeftHand.Ring.2 Stretched",
        "LeftHand.Ring.3 Stretched",
        "LeftHand.Little.1 Stretched",
        "LeftHand.Little.Spread",
        "LeftHand.Little.2 Stretched",
        "LeftHand.Little.3 Stretched",
        "RightHand.Thumb.1 Stretched",
        "RightHand.Thumb.Spread",
        "RightHand.Thumb.2 Stretched",
        "RightHand.Thumb.3 Stretched",
        "RightHand.Index.1 Stretched",
        "RightHand.Index.Spread",
        "RightHand.Index.2 Stretched",
        "RightHand.Index.3 Stretched",
        "RightHand.Middle.1 Stretched",
        "RightHand.Middle.Spread",
        "RightHand.Middle.2 Stretched",
        "RightHand.Middle.3 Stretched",
        "RightHand.Ring.1 Stretched",
        "RightHand.Ring.Spread",
        "RightHand.Ring.2 Stretched",
        "RightHand.Ring.3 Stretched",
        "RightHand.Little.1 Stretched",
        "RightHand.Little.Spread",
        "RightHand.Little.2 Stretched",
        "RightHand.Little.3 Stretched"
    };
        public static bool IsHumanMuscle(string property)
        {
            return MuscleEndWith.Any(x => property.EndsWith(x));
        }
        public static bool IsHumanBone(string property)
        {
            return HumanTrait.BoneName.Any(x => x == property);
        }
        private static (bool, string, string, string) extractHumanBoneImpl(string property)
        {
            int dotIndex = property.LastIndexOf('.');
            string TRS = property.Substring(dotIndex - 1, 1);
            string element = property.Substring(dotIndex + 1);
            if (element.Length != 1)
                throw new LoadException("The element after dot must be xyzw");
            return (true, property, TRS, element);
        }
        /// <summary>
        /// extract human bone name,include root motion
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static (bool, string, string, string) ExtractHumanBone(string property)
        {
            if (property.StartsWith("Root"))
                return extractHumanBoneImpl(property);
            foreach (var v in HumanTrait.BoneName)
            {
                if (property.StartsWith(v))
                {
                    return extractHumanBoneImpl(property);
                }
            }
            return (false, null, null, null);
        }
        public static void SetPose(Avatar avatar, Transform transform, HumanPose pose)
        {
            var handler = new HumanPoseHandler(avatar, transform);
            handler.SetHumanPose(ref pose);
        }
        public static void SetTPose(Avatar avatar, Transform transform)
        {
            var humanPoseClip = Resources.Load<HumanPoseClip>(HumanPoseClip.TPoseResourcePath);
            var pose = humanPoseClip.GetPose();
            SetPose(avatar, transform, pose);
        }
        /// <summary>
        /// when export avatar, make sure it is a TPose state.otherwise the animation will looks wired
        /// </summary>
        /// <param name="animator"></param>
        [System.Obsolete("use EnforceTPose2 instead")]
        public static void EnforceTPose(Animator animator)
        {
            Transform LeftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            Transform LeftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            Transform RightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            Transform RightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            if (LeftLowerArm == null || LeftUpperArm == null || RightLowerArm == null || RightUpperArm == null) return;

            var left = (LeftLowerArm.position - LeftUpperArm.position).normalized;
            if (left.x > 0.9f) return;
            LeftUpperArm.rotation = Quaternion.FromToRotation(left, Vector3.left) * LeftUpperArm.rotation;
            LeftLowerArm.rotation = LeftUpperArm.rotation;
            var right = (RightLowerArm.position - RightUpperArm.position).normalized;
            if (right.x < -0.9f) return;
            RightUpperArm.rotation = Quaternion.FromToRotation(right, Vector3.right) * RightUpperArm.rotation;
            RightLowerArm.rotation = RightUpperArm.rotation;
        }
        public static void EnforceTPose2(Animator animator)
        {
            Vector3 right = animator.transform.right;
            Vector3 left = -right;

            Transform LeftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            Transform LeftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            Transform RightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            Transform RightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            if ((LeftLowerArm.position - LeftUpperArm.position).normalized.x < -0.9f) return;
            if ((RightLowerArm.position - RightUpperArm.position).normalized.x > 0.9f) return;

            Vector3 rightUpperArmToLowerArm = RightLowerArm.position - RightUpperArm.position;
            Vector3 leftUpperArmToLowerArm = LeftLowerArm.position - LeftUpperArm.position;
            RightUpperArm.Rotate(Vector3.Cross(rightUpperArmToLowerArm, right), Vector3.Angle(rightUpperArmToLowerArm, right));
            LeftUpperArm.Rotate(Vector3.Cross(leftUpperArmToLowerArm, left), Vector3.Angle(leftUpperArmToLowerArm, left));

            Transform LeftMiddleProximal = animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);
            Transform LeftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            Transform RightMiddleProximal = animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
            Transform RightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);

            Vector3 leftHandToMiddleFinger = LeftMiddleProximal.position - LeftHand.position;
            Vector3 rightHandToMiddleFinger = RightMiddleProximal.position - RightHand.position;
            Vector3 projectedLeftHandToMiddleFinger = Vector3.ProjectOnPlane(leftHandToMiddleFinger, animator.transform.up);
            Vector3 projectedRightHandToMiddleFinger = Vector3.ProjectOnPlane(rightHandToMiddleFinger, animator.transform.up);
            LeftHand.Rotate(Vector3.Cross(leftHandToMiddleFinger, projectedLeftHandToMiddleFinger), Vector3.Angle(leftHandToMiddleFinger, projectedLeftHandToMiddleFinger));
            RightHand.Rotate(Vector3.Cross(rightHandToMiddleFinger, projectedRightHandToMiddleFinger), Vector3.Angle(rightHandToMiddleFinger, projectedRightHandToMiddleFinger));

            rightHandToMiddleFinger = RightMiddleProximal.position - RightHand.position;
            leftHandToMiddleFinger = LeftMiddleProximal.position - LeftHand.position;
            RightHand.Rotate(-Vector3.Cross(rightHandToMiddleFinger, right), Vector3.Angle(rightHandToMiddleFinger, right));
            LeftHand.Rotate(-Vector3.Cross(leftHandToMiddleFinger, left), Vector3.Angle(leftHandToMiddleFinger, left));
        }
    }
}