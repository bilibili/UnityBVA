# BVA_audio_audioClipExtension

## Overview

This extension defines AudioClip. It will export AudioClip in Unity in the form of WAV or OGG.

`By default, short AudioClip will export as WAV, long AudioClip will automately compress as OGG.`

## Defining AudioClip 
only when exporting seperate gltf file, will the `uri` take effect. If you export assets as glb, `bufferView` will be used instead.

`mimeType` specify the audio format. Currently, `WAV`, `OGG` are supported.

> Each audio clip contains one of:

- a URI to an external file in one of the supported image formats, or
- a reference to a bufferView; in that case mimeType MUST be defined.

> The following parameters are contributed by the `BVA_audio_audioClipExtension` extension, and exists as a collection under the Extensions field of:

|           | Type       | Description     | Required             |
|-----------|-------------------|------------------------|----------------------|
|**name**               | `string`                                                                        | The name of the audio.         | No   |
|**uri**               | `string`                                                                        | a URI to an external file in one of the supported image formats.         | No   |
|**mimeType**               | `string`                                                                        |  Indicate the supported image formats, MUST be defined if read data from bufferView instead of uri.         | No   |
|**bufferView**               | `bufferViewId`                                                                        | Define the raw data for retrieval from the file, the image data MUST match the image.mimeType property when the latter is defined.         | No   |
|**lengthSamples**               | `number`                                                                        | The length of the audio clip in samples.         | Yes  |
|**channels**              | `number`             | The number of channels in the audio clip. | Yes                   |
|**frequency**      | `number`                                                                        | The sample frequency of the clip in Hertz.         | Yes   |

```csharp
    public class AudioAsset : GLTFProperty
    {
        public string name;
        public string url;
        public string mimeType;
        public BufferViewId bufferView;
        public int lengthSamples;
        public int channels;
        public int frequency;
    }
```

### AudioSource Property
                   
> The following parameters are contributed by the `BVA_audio_audioClipExtension` extension:  

|            | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**Audio**               | `Id`                                                                        | Reference to the sound clip file that will be played.         | No   |
|**playOnAwake**               | `bool`                                                                        | If enabled, the sound will start playing the moment the scene launches.         | No   |
|**loop**               | `bool`                                                                        |  Enable this to make the Audio Clip loop when it reaches the end.         | No   |
|**volume**               | `float`                                                                        | How loud the sound is.         | No   |
|**pitch**               | `float`                                                                        | Amount of change in pitch due to slowdown/speed up of the Audio.         | Yes  |
|**panStereo**              | `float`             | Sets the position in the stereo field of 2D sounds. | Yes                   |
|**spatialBlend**      | `float`                                                                        | Sets how much the 3D engine has an effect on the audio source.         | Yes   |
|**rolloffMode**               | `enum`                                                                        | How fast the sound fades. The higher the value, the closer the Listener has to be before hearing the sound.          | No   |
|**dopplerLevel**               | `float`                                                                        | Determines how much doppler effect will be applied to this audio source (if is set to 0, then no effect is applied).         | Yes  |
|**spread**              | `float`             | Sets the spread angle to 3D stereo or multichannel sound in speaker space. | Yes                   |
|**spatialBlend**      | `float`                                                                        | Sets how much the 3D engine has an effect on the audio source.         | Yes   |
|**minDistance**      | `float`                                                                        | Within the MinDistance, the sound will stay at loudest possible. Outside MinDistance it will begin to attenuate.         | Yes   |
|**maxDistance**      | `float`                                                                        | The distance where the sound stops attenuating at. Beyond this point it will stay at the volume it would be at MaxDistance units from the listener and will not attenuate any more.        | Yes   |


```csharp
    public class AudioSourceProperty
    {
        public AudioId audio;
        public bool playOnAwake;
        public bool loop;
        public float volume;
        public float pitch;
        public float panStereo;
        public float spatialBlend;
        public AudioRolloffMode rolloffMode;
        public float dopplerLevel;
        public float spread;
        public float minDistance;
        public float maxDistance;
    }
```