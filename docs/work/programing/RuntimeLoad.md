# Runtime Load

RuntimeLoad.unity scene shows you how to load avatar assets.

### GLB file

#### Common Method

```csharp
await BVASceneManager.Instance.LoadSceneAsync(path);
```

#### Avatar Optimized Method

```csharp
await BVASceneManager.Instance.LoadAvatar(path);
```

### PMX file

```csharp
Transform alicia = await PMXModelLoader.LoadPMXModel(path, SampleAnimatorController);
```

### VRM file

```csharp
var data = new GlbFileParser(path).Parse();
var vrm = new VRMData(data);
var context = new VRMImporterContext(vrm);
var loaded = context.Load();
```