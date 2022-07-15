# BVA_texture_cubemap

## 概览

Cubemap 是六个方形纹理的集合，它们代表环境中的反射。六个正方形组成了一个围绕一个物体的假想立方体的面；每个面代表沿世界轴方向（上、下、左、右、前和后）的视图。

## 图像类型

- 一行或一列中的 6 个纹理
- 全景图像（又名 360° 图像）

![glb](../../tools/pics/CubeLayout6Faces.png)

```csharp
    public enum CubemapImageType
    {
        Row,
        Column,
        Equirect,
        Unknown
    }
```

## Cubemap Properties
|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**imageType**            | `enum`      | 立方体贴图的布局          | No    |
|**mipmap**               | `bool`      | 是否生成mipmap          | Yes   |
|**texture**              | `id`        | 包含 6 个面数据的纹理 | Yes   |