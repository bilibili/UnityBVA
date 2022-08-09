# Runtime Export

## Overview

Export BVA at runtime has some limitation, such like animation clip won't be able to export(can't access frame data with runtime API). But still, most of the features are available.

## *Export as GLB*

Pass the Transform root into the GLTFScceneExporter object, assign the path, then call SaveGLB.

```csharp
var exportOptions = new ExportOptions { TexturePathRetriever = null };
var exporter = new GLTFSceneExporter(new Transform[] { root }, exportOptions);
var path = SFB.StandaloneFileBrowser.SaveFilePanel("Runtime Export", "", "RuntimeExport", "glb");
if (!string.IsNullOrEmpty(path))
{
    exporter.SaveGLB(Path.GetDirectoryName(path),Path.GetFileNameWithoutExtension(path));
}
```

## *Export as GLTF*

Only need to change the function

```csharp
exporter.SaveGLTFandBin(Path.GetDirectoryName(path),Path.GetFileNameWithoutExtension(path));
```


## Avatar Specific

When export an avatar, a `Enforce T-Pose` will be applyed. Only in this way can ensure the correctness of humanoid animation.


## Runtime Export Limitation

Animation will not be export at Runtime.

## Editor Entry

Check `ExportSceneMenu.cs` file dive into the details.