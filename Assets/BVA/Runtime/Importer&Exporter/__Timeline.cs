using UnityEngine;
using BVA;
using BVA.Component;
using GLTF.Schema;
using GLTF.Schema.BVA;
using System;
using System.Threading.Tasks;
using BVA.Extensions;
using System.Collections.Generic;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async Task LoadTimeline()
        {
            //if (Application.isPlaying && _options.RuntimeImport)
            _assetManager.motionLoader.CreatePlayableGraph();

            for (int index = 0; index < _assetCache.NodeCache.Length; index++)
            {
                var node = _gltfRoot.Nodes[index];
                var nodeObj = _assetCache.NodeCache[index];
                // import playable

                if (hasValidExtension(node, BVA_timeline_playableExtensionFactory.EXTENSION_NAME))
                {
                    IExtension ext = node.Extensions[BVA_timeline_playableExtensionFactory.EXTENSION_NAME];
                    var impl = (BVA_timeline_playableExtensionFactory)ext;
                    if (impl == null) throw new Exception($"cast {nameof(BVA_timeline_playableExtensionFactory)} failed");
                    var playableExt = _gltfRoot.Extensions.Playables[impl.playable.playable.Id];
                    await ImportPlayable(playableExt.track, nodeObj, impl.playable.playOnAwake, impl.playable.loop);
                }
            }
        }

#if UNITY_EDITOR
        public void ReimportPlayableMedia(PlayableController[] controllers, AudioClipContainer audioSourceContainer)
        {
            foreach (var playable in controllers)
            {
                foreach (var v in playable.trackAsset.audioTrackGroup.tracks)
                {
                    v.source = audioSourceContainer.Get(v.sourceId);
                }
            }
        }

        public void ReimportPlayableMedia(PlayableController[] controllers, List<Texture2D> textures)
        {
            foreach (var playable in controllers)
            {
                foreach (var v in playable.trackAsset.materialTextureTrackGroup.tracks)
                {
                    v.value = textures[v.sourceId];
                }
            }
        }
#endif

        public async Task ImportPlayable(TrackAsset asset, GameObject gameObject, bool playOnAwake, bool loop)
        {
            var controller = gameObject.AddComponent<PlayableController>();
            controller.playOnAwake = playOnAwake;
            controller.loop = loop;
            controller.trackAsset = asset;

            foreach (var v in asset.animationTrackGroup.tracks)
            {
                v.animator = _assetCache.NodeCache[v.animatorId.Id].GetOrAddComponent<Animator>();
                v.source = _assetCache.AnimatorClipCache[v.sourceId].LoadedAnimationClip;
            }

            foreach (var v in asset.audioTrackGroup.tracks)
            {
                v.audio = _assetCache.NodeCache[v.audioSourceId.Id].GetOrAddComponent<AudioSource>();
                v.source = _assetManager.audioClipContainer.Get(v.sourceId);
            }

            foreach (var v in asset.blendShapeTrackGroup.tracks)
            {
                v.source = _assetCache.NodeCache[v.sourceId].GetComponent<SkinnedMeshRenderer>();
            }

            foreach (var v in asset.materialCurveFloatTrackGroup.tracks)
            {
                v.source = _assetCache.NodeCache[v.sourceId].GetComponent<Renderer>();
            }

            {
                foreach (var v in asset.materialFloatTrackGroup.tracks)
                {
                    v.source = _assetCache.NodeCache[v.sourceId].GetComponent<Renderer>();
                }
                foreach (var v in asset.materialIntTrackGroup.tracks)
                {
                    v.source = _assetCache.NodeCache[v.sourceId].GetComponent<Renderer>();
                }
                foreach (var v in asset.materialColorTrackGroup.tracks)
                {
                    v.source = _assetCache.NodeCache[v.sourceId].GetComponent<Renderer>();
                }
                foreach (var v in asset.materialTextureTrackGroup.tracks)
                {
                    v.source = _assetCache.NodeCache[v.sourceId].GetComponent<Renderer>();
                    TextureId textureId = v.textureId;
                    var textureCache = _assetCache.TextureCache[textureId.Id];
                    if (textureCache == null)
                    {
                        await ConstructImageBuffer(textureId.Value, textureId.Id);
                        await ConstructTexture(v.textureId.Value, v.textureId.Id, !KeepCPUCopyOfTexture);
                    }
                    v.value = (Texture2D)_assetCache.TextureCache[v.textureId.Id].Texture;
                }
                foreach (var v in asset.materialVectorTrackGroup.tracks)
                {
                    v.source = _assetCache.NodeCache[v.sourceId].GetComponent<Renderer>();
                }
            }
        }
    }

    public partial class GLTFSceneExporter
    {
        public PlayableId ExportPlayable(PlayableController controller)
        {
            BVA_timeline_playableExtension ext = new BVA_timeline_playableExtension(controller.trackAsset, _nodeCache);
            var id = new PlayableId
            {
                Id = _root.Extensions.Playables.Count,
                Root = _root
            };
            _root.Extensions.AddPlayable(ext);
            return id;
        }

        public void ExportPlayableAudios(TrackAsset track)
        {
            foreach (var info in track.audioTrackGroup.tracks)
            {
                AudioId id = ExportAudioInternalBuffer(info.source);
                if (id != null)
                {
                    info.SetSourceId(id.Id);
                    info.SetAudioSourceId(new GLTF.Schema.NodeId() { Id = _nodeCache.GetId(info.audio.gameObject), Root = _root });
                }
            }
        }

        public void ExportPlayableTextures(TrackAsset track)
        {
            foreach (var info in track.materialTextureTrackGroup.tracks)
            {
                var id = ExportTexture(info.value);
                info.SetTextureId(id);
            }
        }

        public void ExportTimeline()
        {
            foreach (var playable in _playables)
            {
                ExportPlayableTextures(playable.trackAsset);
                ExportPlayableAudios(playable.trackAsset);
                //ExportPlayableAnimations(playable.trackAsset); // AnimationClip has exported in __Animation.cs ExportAnimation()

                var playableId = ExportPlayable(playable);
                var nodeId = _nodeCache.GetId(playable.gameObject);
                PlayableProperty property = new PlayableProperty() { playable = playableId, playOnAwake = playable.playOnAwake, loop = playable.loop };
                _root.Nodes[nodeId].AddExtension(_root, BVA_timeline_playableExtensionFactory.EXTENSION_NAME, new BVA_timeline_playableExtensionFactory(property), RequireExtensions);
            }
        }
    }
}