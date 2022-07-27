# 动画

## 概览

导出动画的能力其实是被GLTF原生支持的，但是局限于 `transform & blendshape` 属性.

基于这个原因, 只有transform或者blendshape的属性会被导出，并且加载时局限于 `Animation` 组件（只有legacy animation可以在运行时被创建）.

## Export

所有位于Animation组件上的动画片段都会被导出，并且extras里会包含一下信息:

```csharp
public bool playAutomatically;
public UnityEngine.WrapMode wrapMode;
public bool animatePhysics;
public UnityEngine.AnimationCullingType cullingType;
public int animation;
public List<int> animations;
```

> **重要提示** : 导出`Mecanim Animation`的时候，需要先转换为骨骼蒙皮动画(即Unity中的legacy动画), 转换的结果在任何工具中都可以运行。这个过程会耗费较长的时间。 


## 来源

- GLTF 动画 (文件内部)
- BVH (外部文件，工业标准的人形动画)
- VMD (外部文件，即MMD动画)
