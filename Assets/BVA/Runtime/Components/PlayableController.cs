using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace BVA.Component
{
    public interface IPlayable
    {
        void Play();
        void Stop();
        void Pause();
        void PlayFromStart();
    }
    [DisallowMultipleComponent]
    public class PlayableController : MonoBehaviour, IPlayable
    {
        public TrackAsset trackAsset;
        public bool playOnAwake;
        public bool loop;
        public float currentTime;
        private PlayableGraph graph;
        void Start()
        {
            Create();
        }
        public void Create()
        {
            graph = PlayableGraph.Create();
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            trackAsset.Validate(ref graph);
            trackAsset.RecalculateLength();
        }
        void Update()
        {
            if (graph.IsPlaying())
            {
                currentTime += Time.deltaTime;
            }
            if (currentTime > 0)
            {
                trackAsset.animationTrackGroup.Update(currentTime);
                trackAsset.audioTrackGroup.Update(currentTime);
                trackAsset.Update(currentTime);
            }
        }
        [ContextMenu("Play")]
        public void Play()
        {
            graph.Play();
        }
        [ContextMenu("Stop")]
        public void Stop()
        {
            graph.Stop();
        }
        [ContextMenu("Pause")]
        public void Pause()
        {
            graph.Stop();
        }
        [ContextMenu("Play From Start")]
        public void PlayFromStart()
        {
            currentTime = 0;
            Play();
        }
        [ContextMenu("Export Unity Json")]
        public void ExportUnityJson()
        {
            Debug.Log(JsonUtility.ToJson(trackAsset));
        }
        [ContextMenu("Export JProperty Json")]
        public void ExportAssetJson()
        {
            Debug.Log(trackAsset.Serialize(new NodeCache()));
        }
        public void OnDestroy()
        {
            if(graph.IsValid())
                graph.Destroy();
        }
    }
}