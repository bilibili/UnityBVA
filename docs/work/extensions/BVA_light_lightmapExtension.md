# BVA_light_lightmapExtension

## Overview

This extension defines the Baked Lightmaps.

Lightmapping is the process of pre-calculating the brightness of surfaces in a Scene.

### Lightmap Properties
> The following parameters are contributed by the `BVA_light_lightmapExtension` extension:

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**lightmapsMode**              | `enum`             | Lightmap (and lighting) configuration mode, controls how lightmaps interact with lighting and what kind of information they store.  | Yes                   |
|**lightmapsEncoding**              | `enum`             | Different compressions and encoding schemes, depending on the target platform and the compression setting.  | Yes                   |
|**lightmaps**              | `LightmapTextureInfo[]`             | Texture storing occlusion mask per light (ShadowMask, up to four lights).  | No                   |


### Lightmap Texture Properties
> The `LightmapTextureInfo` struct:

```csharp
    public struct LightmapTextureInfo
    {
        public TextureId lightmapColor;
        public TextureId lightmapDir;
        public TextureId shadowMask;
    }
```

> Each texture store the different information that contribute to the scene lighting:

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**lightmapColor**              | `Id`             | Storing color of incoming light.  | Yes                   |
|**lightmapDir**              | `Id`             | Storing dominant direction of incoming light.  | Yes                   |
|**shadowMask**              | `Id`             | Storing occlusion mask per light.  | No                   |