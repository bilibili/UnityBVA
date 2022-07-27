# Animation

## Overview

The ability to export animations is supported natively by GLTF. But is is limited to `transform & blendshape` properties.

For this reason, only transform or blendshape key frame will be exported, and only `Animation` component attach when loading a model with animations(only legacy animation can be constructed at runtime).

## Export

All animation clips on Animation Component will be exported. And extras will be filled by flowing information:

```csharp
public bool playAutomatically;
public UnityEngine.WrapMode wrapMode;
public bool animatePhysics;
public UnityEngine.AnimationCullingType cullingType;
public int animation;
public List<int> animations;
```

> **Critical Notice** : Firstly, convert `Mecanim Animation` to skin animation(aka legacy in Unity), the result works perfectly on all gltf applications. This process can takes a lot of time.


## Source

- GLTF Animation (Internal)
- BVH (External)
- VMD (MMD External)