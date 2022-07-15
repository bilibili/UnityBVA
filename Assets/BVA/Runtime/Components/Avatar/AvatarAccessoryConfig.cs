using System.Collections.Generic;
using UnityEngine;

namespace BVA.Component
{
    [System.Serializable]
    public class AccessoryConfig
    {
        public string name;
        public string catagory;
        [HideInInspector]
        public int node;
        public GameObject gameObject;
    }

    /// <summary>
    /// Save all active accessory GameObject and its corresponding bone transforms.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AvatarAccessoryConfig : MonoBehaviour
    {
        public List<AccessoryConfig> accessories;
        private Animator animator;
        public HumanBodyBones GetAccessoryBone(GameObject gameObject)
        {
            if (gameObject == null || gameObject.transform.parent == null)
                return HumanBodyBones.LastBone;
            Transform parent = gameObject.transform.parent;
            for (int i = 0; i < (int)HumanBodyBones.LastBone; i++)
            {
                HumanBodyBones bodyBones = (HumanBodyBones)i;
                Transform boneTransform = animator.GetBoneTransform(bodyBones);
                if (boneTransform == parent)
                {
                    return bodyBones;
                }
            }
            return HumanBodyBones.LastBone;
        }
        private void Start()
        {
            animator = GetComponent<Animator>();
            foreach (var config in accessories)
            {
                config.gameObject.SetActive(false);
            }
        }
    }
}