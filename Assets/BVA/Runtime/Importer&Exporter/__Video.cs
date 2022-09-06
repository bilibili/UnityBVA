using System.Collections.Generic;
using GLTF.Schema.BVA;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using BVA;
using GLTF.Schema;
using System;
using BVA.Extensions;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public void LoadVideoPlayer(Node node, GameObject nodeObj)
        {
            if (hasValidExtension(node, BVA_url_videoExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_url_videoExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_url_videoExtensionFactory)ext;
                if (impl == null) throw new InvalidCastException($"cast {nameof(BVA_url_videoExtensionFactory)} failed");
                ImportVideoPlayer(impl.video, nodeObj);
            }
        }
        public void ImportVideoPlayer(VideoPlayerProperty property, GameObject gameObject)
        {
            var videoPlayer = gameObject.AddComponent<VideoPlayer>();
            videoPlayer.enabled = false;//audio play encounter some bugs, must disable then enable it
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.audioOutputMode = property.audioOutputMode;
            videoPlayer.playOnAwake = property.playOnAwake;
            videoPlayer.isLooping = property.loop;
            videoPlayer.playbackSpeed = property.playbackSpeed;
            videoPlayer.targetMaterialProperty = property.propertyName;
            videoPlayer.waitForFirstFrame = property.waitForFirstFrame;
            videoPlayer.url = property.url;
            videoPlayer.targetMaterialRenderer = _assetCache.NodeCache[property.renderer].GetComponent<Renderer>();
            if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
            {
                for (ushort i = 0; i < property.audioSourceProperties.Count; i++)
                {
                    var audioSourceProperty = property.audioSourceProperties[i];
                    AudioSource source = _assetCache.NodeCache[audioSourceProperty.audio.Id].GetOrAddComponent<AudioSource>();
                    audioSourceProperty.Deserialize(source);
                    videoPlayer.SetTargetAudioSource(i, source);
                }
            }
            videoPlayer.enabled = true;
        }
    }
    public partial class GLTFSceneExporter
    {
        private void ExportVideoPlayer()
        {
            foreach (var videoPlayer in _videoPlayers)
            {
                VideoPlayerProperty playerProperty = new VideoPlayerProperty(videoPlayer);
                //playerProperty.video = ExportUrlVideoPlayer(videoPlayer);
                int rendererId = _nodeCache.GetId(videoPlayer.targetMaterialRenderer.gameObject);
                if (rendererId < 0)
                {
                    LogPool.ExportLogger.LogError(LogPart.Video, $"not find renderer GameObject on {videoPlayer.name}, make sure the reference is under the export root.");
                    continue;
                }
                playerProperty.renderer = rendererId;

                List<AudioSourceProperty> audioSourceProperties = new List<AudioSourceProperty>();
                if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
                {
                    for (ushort i = 0; i < videoPlayer.controlledAudioTrackCount; i++)
                    {
                        var audioSource = videoPlayer.GetTargetAudioSource(i);
                        int audioSourceId = _nodeCache.GetId(audioSource.gameObject);
                        if (audioSourceId < 0)
                        {
                            audioSourceProperties = null;
                            LogPool.ExportLogger.LogError(LogPart.Video, $"not find AudioSource on {videoPlayer.name}, index of ControlledAudioTrack is {i}");
                            break;
                        }
                        AudioSourceProperty pro = new AudioSourceProperty(audioSource)
                        {
                            audio = new AudioId() { Id = audioSourceId, Root = _root }
                        };
                        audioSourceProperties.Add(pro);
                    }
                }
                if (audioSourceProperties == null)
                    continue;
                playerProperty.audioSourceProperties = audioSourceProperties;

                var nodeId = _nodeCache.GetId(videoPlayer.gameObject);
                _root.Nodes[nodeId].AddExtension(_root, BVA_url_videoExtensionFactory.EXTENSION_NAME, new BVA_url_videoExtensionFactory(playerProperty), RequireExtensions);
            }
        }
    }
}