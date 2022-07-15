using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using System.IO;
using OggVorbis;
using NAudio.Wave;

public class AudioTest
{
    [Test]
    public void SaveOGG()
    {
        //audio frequency is only support 44100
        var source = Selection.activeGameObject?.GetComponent<AudioSource>();
        var clip = source.clip;
        VorbisPlugin.Save("E://a.ogg", clip);
    }
    [Test]
    public void SaveMP3()
    {
        //audio frequency is only support 44100
        var source = Selection.activeGameObject?.GetComponent<AudioSource>();
        var clip = source.clip;
        WaveUtil.SaveToMP3File(clip,"E://output.mp3");
    }
    [Test]
    public void LoadFromMemory()
    {
        var bytes = File.ReadAllBytes("E://a.ogg");
        AudioClip audioClip = VorbisPlugin.ToAudioClip(bytes, "a");
        var source = Selection.activeGameObject?.GetComponent<AudioSource>();
        source.clip = audioClip;
        source.Play();
    }
}
