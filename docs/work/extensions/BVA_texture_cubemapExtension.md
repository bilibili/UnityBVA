# BVA_texture_cubemap

## Overview
A Cubemap is a collection of six square textures that represent the reflections on an environment. The six squares form the faces of an imaginary cube that surrounds an object; each face represents the view along the directions of the world axes (up, down, left, right, forward and back).

## Image Types

- 6 textures in a row or column.
- Panorama images(aka 360° images).

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
|          | Type    | Description             | Required       |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**imageType**            | `enum`      | The layout of the cubemap.           | No    |
|**mipmap**               | `bool`      | Whether to generate mipmap.          | Yes   |
|**texture**              | `id`        | The texture that contains 6 faces data. | Yes   |