using System.Collections.Generic;
using UnityEngine.Video;


namespace GLTF.Schema.BVA
{
    public class VideoPlayerProperty
    {
        //public UrlVideoId video;
        public bool playOnAwake;
        public bool waitForFirstFrame;
        public bool loop;
        public float playbackSpeed;
        public int renderer;
        public string propertyName;
        public string url;
        public VideoAudioOutputMode audioOutputMode;
        public List<AudioSourceProperty> audioSourceProperties;

        public VideoPlayerProperty() 
        { 
            playOnAwake = true;
            waitForFirstFrame = true; 
            audioSourceProperties = new List<AudioSourceProperty>();
        }

        public VideoPlayerProperty(VideoPlayer player)
        {
            playOnAwake = player.playOnAwake;
            waitForFirstFrame = player.waitForFirstFrame;
            loop = player.isLooping;
            playbackSpeed = player.playbackSpeed;
            url = player.url;
            propertyName = player.targetMaterialProperty;
            audioOutputMode = player.audioOutputMode;
            audioSourceProperties = new List<AudioSourceProperty>();
        }
    }
}
namespace BVA
{

}