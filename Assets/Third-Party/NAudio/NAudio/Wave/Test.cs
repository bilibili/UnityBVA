using NAudio.Wave;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        var bytes = File.ReadAllBytes("D://Time.mp3");
        AudioClip audioClip = await WaveUtil.ToAudioClip(bytes, "Time");
        GameObject g = new GameObject();
        var source = g.AddComponent<AudioSource>();
        source.clip = audioClip;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
