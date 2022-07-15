using GLTF.Schema.BVA;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;
using BVA;
using GLTF.Schema;

namespace BVA
{
    [System.Serializable]
    public abstract class BasePlayableTrack<T> : BaseTrack<T> where T : Object
    {
        public abstract void SetTime(float time);
        public abstract void Stop();
        public abstract bool ComponentIsNull();
        public BasePlayableTrack()
        {
        }
    }
    [System.Serializable]
    public class AnimationTrack : BasePlayableTrack<AnimationClip>
    {
        public Animator animator;
        public AnimationClipPlayable playable;
        public NodeId animatorId { private set; get; }
        public void SetAnimatorId(NodeId id)
        {
            animatorId = id;
        }
        public override void SetTime(float time)
        {
            playable.SetTime(time);
        }
        public override void Stop()
        {
            playable.Pause();
        }

        public override bool ComponentIsNull()
        {
            if (animator == null)
            {
                Debug.LogError($"{name}'s {animator.GetType()} is null");
                isValid = false;
                return true;
            }
            return false;
        }

        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = SerializeBase(cache);
            jo.Add(nameof(animator), cache.GetId(animator.gameObject));
            jo.Add(nameof(source), sourceId);
            return new JProperty(gltfProperty, jo);
        }

        public override float length => source.length;
        public float endPlayTime => endTime == 0 ? (float)length : endTime;
    }
    [System.Serializable]
    public class AudioTrack : BasePlayableTrack<AudioClip>
    {
        public AudioSource audio;
        public AudioClipPlayable playable;
        public NodeId audioSourceId { private set; get; }
        public void SetAudioSourceId(NodeId id)
        {
            audioSourceId = id;
        }
        public override void SetTime(float time)
        {
            playable.Play();
            playable.SetTime(time);
        }
        public override void Stop()
        {
            playable.SetSpeed(0);
        }

        public override bool ComponentIsNull()
        {
            if (audio == null)
            {
                Debug.LogError($"{name}'s AudioSource is null");
                isValid = false;
                return true;
            }
            return false;
        }

        public override JProperty Serialize(NodeCache cache)
        {
            JObject jo = SerializeBase(cache);
            jo.Add(nameof(audio), audioSourceId.Id);
            jo.Add(nameof(source), sourceId);
            return new JProperty(gltfProperty, jo);
        }

        public override float length => source.length;
    }
}