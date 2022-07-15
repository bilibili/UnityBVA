# BVA_collisions_colliderExtension

## Overview

This extension defines varying Collider components. Almost all engines support build-in  implementations of collider types. The most commonly used types are `Box`, `Sphere`, `Capsule`, `Mesh`.
Components in Unity3D that corresponds to: `BoxCollider`, `SphereCollider`, `CapsuleCollider`, `MeshCollider`

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

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**type**               | `enum`                                                                        | The type of collider shape.         | Yes   |
|**isTrigger**               | `bool`                                                                        | Enumeration value for a category.         | Yes   |
|**center**               | `Vector3`                                                                        |  The center of the collider         | Yes   |
|**size**               | `Vector3`                                                                        | The size of the box         | No   |
|**radius**               | `float`                                                                        | The radius of the sphere(Sphere collider only).         | No  |
|**height**              | `float`             | The height of the capsule(Capsule collider only).   | No                   |
|**direction**              | `enum`             | The direction of the capsule(Capsule collider only).   | No                   |
|**convex**              | `bool`             | Use a convex collider from the mesh(Mesh collider only).   | No                   |