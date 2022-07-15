# Animation

## Overview

The ability to export animations is supported natively by GLTF. But is is limited to `transform & blendshape` properties.

For this reason, only transform or blendshape key frame will be exported, and it's confined to `Animation` only.

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

**Critical Notice** : `Mecanim animation(non-legacy)` doesn't support exporting. (tested it work on Editor, failed to constructing at runtime, so these parts has be removed)

## Alternative Solution

Convert Mecanim animation to traditional animatin(lagacy), the result works perfectly on all gltf applications.

## Load

- GLTF Animation (Internal)
- BVH (External)
- VMD (MMD External)
