# BVA_audio_audioClipExtension

## 概述
此扩展定义了 AudioClip。它将以 WAV 或 OGG 的形式在 Unity 中导出 AudioClip。

`默认情况下，短音频剪辑会导出为WAV，长音频剪辑会自动压缩为OGG。`


## 定义音频片段
只有在导出单独的 gltf 文件时，`uri` 才会有用。如果您将资产导出为 glb，则将使用 `bufferView`字段存储信息。

`mimeType` 指定音频格式。目前支持`WAV`、`OGG`。

每个音频剪辑包含一个

- 以支持的图像格式之一指向外部文件的URI
- 对bufferView的引用；在这种情况下，必须定义`mimeType`

> 以下参数由 `BVA_audio_audioClipExtension` 扩展提供，并包含在的extensions字段下以集合的形式存在:

|                | 类型       | 描述           | 是否必需             |
|----------------|------------|---------------|----------------------|
|**name**           | `string`                                                                        | 音频名称         | No   |
|**uri**             | `string`                                                                        | 一种受支持的图像格式的外部文件的 URI         | No   |
|**mimeType**        | `string`                                                                        | 指示支持的图像格式，如果从 bufferView 而不是 uri 读取数据，则必须定义       | No   |
|**bufferView**      | `bufferViewId`                                                                        | 定义从文件中检索的原始数据，当定义后者时，图像数据必须匹配 mimeType 属性         | No   |
|**lengthSamples**               | `number`                                                                        | 样本中音频剪辑的长度。        | Yes  |
|**channels**       | `number`             | 音频剪辑中的通道数 | Yes             |
|**frequency**      | `number`                    | 以赫兹为单位的剪辑的采样频率。        | Yes   |

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

### AudioSource属性
> 以下参数由 `BVA_audio_audioClipExtension` 扩展提供，并存在与`Node`节点下面:

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**Audio**               | `Id`                                                                        | 引用将要播放的声音剪辑文件         | No   |
|**playOnAwake**               | `bool`                                                                        | 如果启用，声音将在场景启动时开始播放        | No   |
|**loop**               | `bool`                                                                        |  启用此选项使音频剪辑在到达结束时循环         | No   |
|**volume**             | `float`                                                                        | 声音有多大           | No   |
|**pitch**             | `float`                                                                        | 由于声音播放的速度引起的音频音高的变化量         | Yes  |
|**panStereo**         | `float`             | 设置2D立体声左右的音量 | Yes                   |
|**spatialBlend**      | `float`                                                                        | 3D音频对声音的空间影响程度         | Yes   |
|**rolloffMode**               | `enum`                                                                        | 声音消退的速度。数值越高，监听器在听到声音之前就离得越近          | No   |
|**dopplerLevel**              | `float`                                                                        | 确定多少多普勒效应将应用到这个音频源(如果设置为0，则不应用效果)         | Yes  |
|**spread**              | `float`             | 设置扬声器空间中3D立体声或多声道声音的传播角度大小 | Yes                   |
|**spatialBlend**      | `float`                                                                        | 设置3D引擎对音频源的影响程度        | Yes   |
|**minDistance**      | `float`                                                                        | 在最小距离内，声音将保持在最大的可能。在最小距离之外，它将开始衰减         | Yes   |
|**maxDistance**      | `float`                                                                        | 声音停止衰减的距离。超过这个点，它将保持音量，它将是在MaxDistance单位从侦听器，将不再衰减        | Yes   |


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