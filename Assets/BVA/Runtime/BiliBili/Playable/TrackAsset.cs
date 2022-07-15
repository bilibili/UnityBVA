using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;
using BVA;
using System.Linq;

namespace BVA
{
    public interface IPlayableGroup
    {
        public void Validate(ref PlayableGraph graph);
        public void Update(float time);
        public float MaxLength { get; }
    }

    [System.Serializable]
    public class PlayableGroup<T> : IPlayableGroup where T : IBaseTrack
    {
        public PlayableGroup() { tracks = new List<T>(); }
        public List<T> tracks;

        public float MaxLength => tracks == null || tracks.Count == 0 ? 0.0f : tracks.Max(x => x.length);

        public virtual void Validate(ref PlayableGraph graph)
        {
            foreach (var track in tracks)
            {
                if (track == null || track.SourceIsNull())
                    continue;
            }
        }
        public virtual void Update(float time) { }
    }

    [System.Serializable]
    public class AnimationClipPlayableGroup : PlayableGroup<AnimationTrack>
    {
        public override void Validate(ref PlayableGraph graph)
        {
            foreach (var track in tracks)
            {
                if (track == null || track.SourceIsNull() || track.ComponentIsNull())
                    continue;

                var animationOutput = AnimationPlayableOutput.Create(graph, "Animation", track.animator);
                track.playable = AnimationClipPlayable.Create(graph, track.source);
                track.playable.Pause();
                animationOutput.SetSourcePlayable(track.playable);
            }
        }
        public override void Update(float time)
        {
            foreach (var track in tracks)
            {
                if (!track.isValid)
                    continue;
                float realTime = time - track.startTime;
                if (realTime >= 0 && (time <= track.endPlayTime))
                {
                    track.SetTime(realTime);
                }
                else
                {
                    track.Stop();
                }
            }
        }
    }

    [System.Serializable]
    public class AudioClipPlayableGroup : PlayableGroup<AudioTrack>
    {
        public override void Validate(ref PlayableGraph graph)
        {
            foreach (var track in tracks)
            {
                if (track == null || track.SourceIsNull() || track.ComponentIsNull())
                    continue;
                var audioOutput = AudioPlayableOutput.Create(graph, "Audio", track.audio);
                track.playable = AudioClipPlayable.Create(graph, track.source, false);
                track.playable.Pause();
                audioOutput.SetSourcePlayable(track.playable);
            }
        }
        public override void Update(float time)
        {
            foreach (var track in tracks)
            {
                if (!track.isValid)
                    continue;
                float realTime = time - track.startTime;
                if (track.playable.IsChannelPlaying())
                {
                    if (track.endTime != 0 && time > track.endTime)
                    {
                        track.Stop();
                        continue;
                    }
                }
                else
                {
                    if (realTime >= 0 && (track.endTime == 0 || time <= track.endTime))
                    {
                        track.SetTime(realTime);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class BlendShapePlayableGroup : PlayableGroup<BlendShapeCurveTrack>
    {
        public override void Update(float time)
        {
            foreach (var track in tracks)
            {
                if (!track.isValid)
                    continue;
                float realTime = time - track.startTime;
                if (realTime >= 0 && (time <= track.endTime))
                {
                    float value = track.SetTime(realTime);
                }
            }
        }
    }

    public class MaterialCurveGroup<T> : PlayableGroup<T> where T : MaterialCurveFloatTrack
    {
        public override void Update(float time)
        {
            foreach (var track in tracks)
            {
                if (!track.isValid)
                    continue;
                float realTime = time - track.startTime;
                if (realTime >= 0 && (time <= track.endTime))
                {
                    float value = track.SetTime(realTime);
                }
            }
        }
    }

    [System.Serializable]
    public class MaterialCurveFloatPlayableGroup : MaterialCurveGroup<MaterialCurveFloatTrack> { }
    [System.Serializable]
    public class MaterialCurveColorPlayableGroup : MaterialCurveGroup<MaterialCurveColorTrack> { }

    [System.Serializable]
    public class RendererMaterialSetValuePlayableGroup<U, T> : PlayableGroup<T> where T : BaseValueTrack<U, Renderer>
    {
        public override void Update(float time)
        {
            foreach (var track in tracks)
            {
                if (!track.isValid)
                    continue;
                float realTime = time - track.startTime;
                if (realTime >= 0 && (time <= track.endTime))
                {
                    track.SetValue();
                }
            }
        }
    }

    [System.Serializable]
    public class MaterialFloatPlayableGroup : RendererMaterialSetValuePlayableGroup<float, MaterialFloatTrack> { }
    [System.Serializable]
    public class MaterialIntPlayableGroup : RendererMaterialSetValuePlayableGroup<int, MaterialIntTrack> { }
    [System.Serializable]
    public class MaterialColorPlayableGroup : RendererMaterialSetValuePlayableGroup<Color, MaterialColorTrack> { }
    [System.Serializable]
    public class MaterialVectorPlayableGroup : RendererMaterialSetValuePlayableGroup<Vector4, MaterialVectorTrack> { }
    [System.Serializable]
    public class MaterialTexture2DPlayableGroup : RendererMaterialSetValuePlayableGroup<Texture2D, MaterialTexture2DTrack> { }

    [System.Serializable]
    public class GameObjectActivePlayableGroup : PlayableGroup<GameObjectActiveTrack>
    {
        public override void Update(float time)
        {
            foreach (var track in tracks)
            {
                if (!track.isValid)
                    continue;
                float realTime = time - track.startTime;
                if (realTime >= 0 && (time <= track.endTime))
                {
                    track.SetValue();
                }
            }
        }
    }

    [System.Serializable]
    public class ComponentEnablePlayableGroup : PlayableGroup<ComponentEnableTrack>
    {
        public override void Update(float time)
        {
            foreach (var track in tracks)
            {
                if (!track.isValid)
                    continue;
                float realTime = time - track.startTime;
                if (realTime >= 0 && (time <= track.endTime))
                {
                    track.SetValue();
                }
            }
        }
    }
    [System.Serializable]
    public class TrackAsset
    {
        public string name;
        public AnimationClipPlayableGroup animationTrackGroup;
        public AudioClipPlayableGroup audioTrackGroup;
        public BlendShapePlayableGroup blendShapeTrackGroup;

        public MaterialCurveFloatPlayableGroup materialCurveFloatTrackGroup;
        public MaterialCurveColorPlayableGroup materialCurveColorTrackGroup;

        public MaterialFloatPlayableGroup materialFloatTrackGroup;
        public MaterialIntPlayableGroup materialIntTrackGroup;
        public MaterialColorPlayableGroup materialColorTrackGroup;
        public MaterialVectorPlayableGroup materialVectorTrackGroup;
        public MaterialTexture2DPlayableGroup materialTextureTrackGroup;

        public GameObjectActivePlayableGroup gameObjectActiveTrackGroup;
        public ComponentEnablePlayableGroup componentEnableTrackGroup;
        private List<IPlayableGroup> _groups;
        public float length;
        public TrackAsset()
        {
            animationTrackGroup = new AnimationClipPlayableGroup();
            audioTrackGroup = new AudioClipPlayableGroup();
            blendShapeTrackGroup = new BlendShapePlayableGroup();

            materialCurveFloatTrackGroup = new MaterialCurveFloatPlayableGroup();
            materialCurveColorTrackGroup = new MaterialCurveColorPlayableGroup();

            materialFloatTrackGroup = new MaterialFloatPlayableGroup();
            materialIntTrackGroup = new MaterialIntPlayableGroup();
            materialColorTrackGroup = new MaterialColorPlayableGroup();
            materialVectorTrackGroup = new MaterialVectorPlayableGroup();
            materialTextureTrackGroup = new MaterialTexture2DPlayableGroup();

            gameObjectActiveTrackGroup = new GameObjectActivePlayableGroup();
            componentEnableTrackGroup = new ComponentEnablePlayableGroup();

            _groups = new List<IPlayableGroup>()
            {
                animationTrackGroup,
                audioTrackGroup,
                blendShapeTrackGroup,
                materialCurveFloatTrackGroup,
                materialCurveColorTrackGroup,
                materialFloatTrackGroup,
                materialIntTrackGroup,
                materialColorTrackGroup,
                materialVectorTrackGroup ,
                materialTextureTrackGroup,
                gameObjectActiveTrackGroup,
                componentEnableTrackGroup ,
            };
        }
        public void RecalculateLength()
        {
            length = _groups.Max(x => x.MaxLength);
        }
        public void Validate(ref PlayableGraph graph)
        {
            foreach (var group in _groups)
            {
                group.Validate(ref graph);
            }
        }
        public void Update(float time)
        {
            foreach (var group in _groups)
            {
                group.Update(time);
            }
        }
        public JObject Serialize(NodeCache cache)
        {
            JObject jo = new JObject();
            if (!string.IsNullOrEmpty(name)) jo.Add(nameof(name), name);

            JArray animations = new JArray();
            foreach (var v in animationTrackGroup.tracks)
                animations.Add(v.Serialize(cache).Value);
            jo.Add(nameof(animationTrackGroup), animations);

            JArray audios = new JArray();
            foreach (var v in audioTrackGroup.tracks)
                audios.Add(v.Serialize(cache).Value);
            jo.Add(nameof(audioTrackGroup), audios);

            JArray blendShapes = new JArray();
            foreach (var v in blendShapeTrackGroup.tracks)
                blendShapes.Add(v.Serialize(cache).Value);
            jo.Add(nameof(blendShapeTrackGroup), blendShapes);

            JArray materialFloatCurves = new JArray();
            foreach (var v in materialCurveFloatTrackGroup.tracks)
                materialFloatCurves.Add(v.Serialize(cache).Value);
            jo.Add(nameof(materialCurveFloatTrackGroup), materialFloatCurves);

            JArray materialColorCurves = new JArray();
            foreach (var v in materialCurveColorTrackGroup.tracks)
                materialColorCurves.Add(v.Serialize(cache).Value);
            jo.Add(nameof(materialCurveColorTrackGroup), materialColorCurves);

            JArray materialFloats = new JArray();
            foreach (var v in materialFloatTrackGroup.tracks)
                materialFloats.Add(v.Serialize(cache).Value);
            jo.Add(nameof(materialFloatTrackGroup), materialFloats);

            JArray materialInts = new JArray();
            foreach (var v in materialIntTrackGroup.tracks)
                materialInts.Add(v.Serialize(cache).Value);
            jo.Add(nameof(materialIntTrackGroup), materialInts);

            JArray materialColors = new JArray();
            foreach (var v in materialColorTrackGroup.tracks)
                materialColors.Add(v.Serialize(cache).Value);
            jo.Add(nameof(materialColorTrackGroup), materialColors);

            JArray materialVectors = new JArray();
            foreach (var v in materialVectorTrackGroup.tracks)
                materialVectors.Add(v.Serialize(cache).Value);
            jo.Add(nameof(materialVectorTrackGroup), materialVectors);

            JArray materialTextures = new JArray();
            foreach (var v in materialTextureTrackGroup.tracks)
                materialTextures.Add(v.Serialize(cache).Value);
            jo.Add(nameof(materialTextureTrackGroup), materialTextures);

            JArray gameObjectActives = new JArray();
            foreach (var v in gameObjectActiveTrackGroup.tracks)
                gameObjectActives.Add(v.Serialize(cache).Value);
            jo.Add(nameof(gameObjectActiveTrackGroup), gameObjectActives);

            JArray componentEnables = new JArray();
            foreach (var v in componentEnableTrackGroup.tracks)
                componentEnables.Add(v.Serialize(cache).Value);
            jo.Add(nameof(componentEnableTrackGroup), componentEnables);

            return jo;
        }
    }
}