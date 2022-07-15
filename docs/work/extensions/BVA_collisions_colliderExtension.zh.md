# BVA_collisions_colliderExtension

## Overview

这个扩展定义了不同的碰撞器组件。包含了几乎所有引擎都支持碰撞器类型。最常用的类型是 `Box`, `Sphere`, `Capsule`, `Mesh`
Unity3D中对应的组件: `BoxCollider`, `SphereCollider`, `CapsuleCollider`, `MeshCollider`

### Defining Collision Type

```csharp
    public enum CollisionType
    {
        Box,
        Sphere,
        Capsule,
        Mesh
    }
```

### Defining Direction

```csharp
    public enum Direction : byte
    {
        x = 0, y, z
    }
```

### Properties
> The following parameters are contributed by the `BVA_collisions_colliderExtension` extension:

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**type**               | `enum`                                                                        | The 碰撞体类型         | Yes   |
|**isTrigger**               | `bool`                                                                        | 是否当成触发器使用         | Yes   |
|**center**               | `Vector3`                                                                        |  碰撞体的中心点        | Yes   |
|**size**               | `Vector3`                                                                        | 方形碰撞体的大小         | No   |
|**radius**               | `float`                                                                        | 球形碰撞体的半径         | No  |
|**height**              | `float`             | 胶囊碰撞体的高度   | No                   |
|**direction**              | `enum`             | 胶囊碰撞体的方向   | No                   |
|**convex**              | `bool`             | 从网格使用一个凸面碰撞器(仅限网格碰撞体).   | No                   |