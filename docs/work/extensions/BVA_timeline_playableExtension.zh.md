# BVA_timeline_playableExtension

使用时间线，通过直观地排列链接到场景中游戏对象的轨迹和剪辑，创建过场动画、电影和游戏播放序列，混合不同类型的基于时间轴的控制。

```csharp
    public class TrackAsset
    {
        public string name;//轨道资源的名称
        public AnimationClipPlayableGroup animationTrackGroup;//动画轨道组
        public AudioClipPlayableGroup audioTrackGroup;//音频轨道组
        public BlendShapePlayableGroup blendShapeTrackGroup;//Blendshape的轨道组（通常用于面捕表情的动画）

        public MaterialCurveFloatPlayableGroup materialCurveFloatTrackGroup;//材质参数曲线轨道组，曲线会随着时间进行输出值的变化（float浮点数类型的参数）
        public MaterialCurveColorPlayableGroup materialCurveColorTrackGroup;//材质参数曲线轨道组，曲线会随着时间进行输出值的变化（Color颜色类型的参数）

        public MaterialFloatPlayableGroup materialFloatTrackGroup;//材质参数轨道组（float浮点数类型的参数）
        public MaterialIntPlayableGroup materialIntTrackGroup;//材质参数轨道组（int整数类型的参数）
        public MaterialColorPlayableGroup materialColorTrackGroup;//材质参数轨道组（Color类型的参数）
        public MaterialVectorPlayableGroup materialVectorTrackGroup;//材质参数轨道组（Vector4类型的参数）
        public MaterialTexture2DPlayableGroup materialTextureTrackGroup;//材质参数轨道组（Texture2D贴图类型的参数）
        public GameObjectActivePlayableGroup gameObjectActiveTrackGroup;//物体是否激活
        public ComponentEnablePlayableGroup componentEnableTrackGroup;//组件是否激活
        public float length;//整个轨道的长度
    }
```

### 所有的轨道都包含以下几个参数，其中sourceId指定了数据源的下标位置，可能是动画，贴图，音频等数据

```csharp
    public abstract class BaseTrack<T> : IBaseTrack where T : Object
    {
        public string name;
        public float startTime;
        public float endTime;
        public int sourceId { protected set; get; }
    }
```

### 基本轨道参数

|                    | 类型         | 描述        | 是否必需 |
|--------------------|--------------|------------|----------|
|**name**            | `string`     | 轨道名称    | No    |
|**startTime**       | `float`      | 开始时间    | Yes   |
|**endTime**         | `float`      | 结束时间    | Yes   |

### 通过一个泛型类型表示每一个类别的轨道组里包含的子轨道数据

```csharp
    public class PlayableGroup<T> : IPlayableGroup where T : IBaseTrack
    {
        public List<T> tracks; // 一个类别下的所有子轨道集合
    }
```

### 继承指定泛型的类型后即可得到不同类型的轨道数据类, 如表示动画和音频的两个轨道组

```csharp
    public class AnimationClipPlayableGroup : PlayableGroup<AnimationTrack>
```

```csharp
    public class AudioClipPlayableGroup : PlayableGroup<AudioTrack>
```

### 对于材质Curve曲线参数类，都继承自泛型类型BaseAnimationCurveTrack

```csharp
    public abstract class BaseAnimationCurveTrack<T> : BaseTrack<T> where T : UnityEngine.Component
    {
        public int index;           // 指定了使用Renderer上多个materials的其中一个material
        public bool pingpongOnce;   // 是否在执行结束后再反向执行一遍
        public AnimationCurve curve;// 曲线数据
    }
```

### BlendShape的曲线，用于平滑的进行人物面捕表情转换

```csharp
    public class BlendShapeCurveTrack : BaseAnimationCurveTrack<SkinnedMeshRenderer>
```

### 材质的浮点参数曲线，基于时间不停更新材质的浮点类型参数

```csharp
    public class MaterialCurveFloatTrack : BaseAnimationCurveTrack<Renderer>
    {
        public string propertyName; //要设置的材质参数名
    }
```

### 材质的颜色参数曲线，基于时间不停更新材质的颜色类型参数，继承自上述类，但是多了开始和结束时的颜色，然后基于时间进行插值运算，不停的进行每一帧颜色的更新

```csharp
    public class MaterialCurveColorTrack : MaterialCurveFloatTrack
    {
        public Color startColor;
        public Color endColor;
    }
```

### 获取到指定Node上的Animator组件，只有NodeId会被导出，其余变量在加载时创建

```csharp
    public class AnimationTrack : BasePlayableTrack<AnimationClip>
    {        
        public Animator animator;
        public AnimationClipPlayable playable;
        public NodeId animatorId { private set; get; }
    }
```

```csharp
    public class AudioTrack : BasePlayableTrack<AudioClip>
    {
        public AudioSource audio;
        public AudioClipPlayable playable;
        public NodeId audioSourceId { private set; get; }
    }
```

### 在Node节点使用该扩展时附带的参数，指定了该Timeline的播放行为

```csharp
    public class PlayableProperty
    {
        public PlayableId playable;
        public bool playOnAwake;
        public bool loop;
    }
```

### 播放参数

|                | 类型          | 描述                        | 是否必需   |
|----------------|---------------|--------------------------|-----|
|**playable**        | `id`      | 轨道名称                  | Yes |
|**playOnAwake**     | `bool`    | 是否加载后即自动进行播放   | No  |
|**loop**            | `bool`    | 是否循环播放              | No  |