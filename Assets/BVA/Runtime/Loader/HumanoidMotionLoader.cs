using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using BVA.Extensions;
using BVA.Cache;

namespace BVA
{
    public class HumanoidMotion
    {
        private AnimationClip clip;
        public AnimationClip motion { get { return clip; } set { clip = value; } }
        public string name => clip.name;
        public WrapMode wrapMode => clip.wrapMode;
        public bool isHumanMotion => clip.humanMotion;
        public HumanoidMotion(AnimationClip clip)
        {
            this.clip = clip;
        }
    }
    public class HumanoidMotions
    {
        public Avatar avatar;
        public List<HumanoidMotion> animationClips;
        public HumanoidMotions()
        {
            animationClips = new List<HumanoidMotion>();
        }
    }
    public class HumanoidMotionLoader : AssetLoader
    {
        /// <summary>
        /// key: nodeId  value: animationClips
        /// </summary>
        public Dictionary<int, HumanoidMotions> animatorInfo;
        public HumanoidMotionLoader(AssetCache cache) : base(cache)
        {
        }
        public void AddAnimationClip(int nodeId, AnimationClip clip)
        {
            animatorInfo ??= new Dictionary<int, HumanoidMotions>();
            if (nodeId < 0)
            {
                Debug.LogError("NodeId can't be less than zero");
                return;
            }
            if (animatorInfo.TryGetValue(nodeId, out HumanoidMotions humanoidAnimationClips))
            {
                humanoidAnimationClips.animationClips.Add(new HumanoidMotion(clip));
            }
            else
            {
                humanoidAnimationClips = new HumanoidMotions();
                humanoidAnimationClips.animationClips.Add(new HumanoidMotion(clip));
                animatorInfo.Add(nodeId, humanoidAnimationClips);
            }
        }

        private void CreatePlayableGraph(GameObject gameObject, HumanoidMotions humanoidMotions)
        {
            Animator animator = gameObject.GetOrAddComponent<Animator>();
            animator.applyRootMotion = true;

            bool hasValidHumanAvatar = animator.avatar != null && animator.avatar.isHuman;
            if (hasValidHumanAvatar)
                humanoidMotions.avatar = animator.avatar;
            else
                return;

            //PlayableController controller = gameObject.GetOrAddComponent<PlayableController>();
        }

        /// <summary>
        /// create animator controller with animation clips
        /// </summary>
        /// <param name="folder"></param>
        public void CreatePlayableGraph()
        {
            if (animatorInfo == null) return;
            foreach (var v in animatorInfo)
            {
                GameObject targetGameObject = assetCache.NodeCache[v.Key];
                CreatePlayableGraph(targetGameObject, v.Value);
            }
        }
        public override void Dispose()
        {
            animatorInfo = null;
        }
    }
}