# BVA_blendShape_blendShapeMixerExtension

## Overview

When export BlendshapeMixer component, the necessary info will export as extensions, then reference it under the node.

> BlendshapeMixer provide two key functions which following facial capture:

- set mesh blendshapes 
- set material parameters

## Defining BlendShapeMixer

### Basic categories of expressions

```csharp
    public enum BlendShapeMixerPreset
    {
        Neutral,
        A,
        I,
        U,
        E,
        O,
        Blink,
        Blink_L,
        Blink_R,
        Joy,
        Angry,
        Sorrow,
        Fun,
        LookUp,
        LookDown,
        LookLeft,
        LookRight,
        Custom
    }
```

### Configuration for each expression type

```csharp
    public class BlendShapeKey : IValueBinding
    {
        public string keyName;
        public BlendShapeMixerPreset preset;
        public List<BlendShapeValueBinding> blendShapeValues;
        public List<MaterialVector4ValueBinding> materialVector4Values;
        public List<MaterialColorValueBinding> materialColorValues;
        public List<MaterialFloatValueBinding> materialFloatValues;
    }
```

```csharp
    public class BlendShapeValueBinding : IValueBinding
    {
        public int nodeId;
        public int index;
        public float weight;
    }
```

```csharp
    public abstract class MaterialValueBinding<T> : IValueBinding
    {
        public SkinnedMeshRenderer node;
        public int index;
        public string propertyName;
        public T targetValue;
        public T baseValue;
        public int nodeId;
    }
```

## Types

> The following parameters are contributed by the `BVA_blendShape_blendShapeMixerExtension` extension:

### Shared Properties

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**keyName**               | `string`                                                                        | The name of the expression.         | No   |
|**preset**               | `enum`                                                                        | Enumeration value for a category.         | Yes   |
|**blendShapeValues**               | `List<BlendShapeValueBinding>`                                                                        |  The index and corresponding weight of the BlendShape.         | No   |
|**materialVector4Values**               | `List<MaterialVector4ValueBinding>`                                                                        | Material parameter adjustment for `Vector4` type.         | No   |
|**materialColorValues**               | `List<MaterialColorValueBinding>`                                                                        | The Material parameter adjustment for `Color` type.          | No  |
|**materialFloatValues**              | `List<MaterialFloatValueBinding>`             | Material parameter adjustment for `float` type.   | No                   |