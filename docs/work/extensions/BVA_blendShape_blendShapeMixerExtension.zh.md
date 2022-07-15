# BVA_blendShape_blendShapeMixerExtension

## 概览

当导出BlendshapeMixer组件时，必要的信息将作为扩展导出，然后在节点下引用它。

> BlendshapeMixer提供以下两个关键功能:

- 设置网格混合形状
- 设置材料参数

## BlendShapeMixer定义

### 表达的基本类别

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

### 每个表达式类型的配置

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

## 类型

> 以下参数由 `BVA_blendShape_blendShapeMixerExtension` 扩展提供:

### 属性

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**keyName**               | `string`                                                                        | 表情的名称         | No   |
|**preset**               | `enum`                                                                        | 表情的枚举值.         | Yes   |
|**blendShapeValues**               | `List<BlendShapeValueBinding>`                                                                        |  BlendShape的索引和相应的权重         | No   |
|**materialVector4Values**               | `List<MaterialVector4ValueBinding>`                                                                        | `Vector4`类型的材质参数调整       | No   |
|**materialColorValues**               | `List<MaterialColorValueBinding>`                                                                        | The `Color`类型的材质参数调整          | No  |
|**materialFloatValues**              | `List<MaterialFloatValueBinding>`             | `float`类型的材质参数调整   | No                   |