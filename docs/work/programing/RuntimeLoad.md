# Runtime Load

RuntimeLoad.unity scene shows you how to load avatar assets.

### GLB file

#### Common Method (Load scene in a recursive way)

```csharp
await BVASceneManager.Instance.LoadSceneAsync(path);
```

#### Avatar Optimized Load (Load parallelly)

```csharp
await BVASceneManager.Instance.LoadAvatar(path);
```

### PMX (MMD model)

```csharp
Transform alicia = await PMXModelLoader.LoadPMXModel(path, SampleAnimatorController);
```

### VRM

```csharp
var data = new GlbFileParser(path).Parse();
var vrm = new VRMData(data);
var context = new VRMImporterContext(vrm);
var loaded = context.Load();
```