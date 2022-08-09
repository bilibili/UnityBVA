# Runtime Load

`RuntimeLoad` scene show you how to load avatar assets.

## GLB file

### Common Method (Auto detect whether the model is for Avatar or Scene)

```csharp
await BVASceneManager.Instance.LoadAsync(path);
```

### Avatar Optimized Load (Load synchronously)

```csharp
await BVASceneManager.Instance.LoadAvatar(path);
```

### Scene Method (Load scene in a recursive way)

```csharp
await BVASceneManager.Instance.LoadSceneAsync(path);
```

## PMX (MMD model)

```csharp
Transform alicia = await PMXModelLoader.LoadPMXModel(path, SampleAnimatorController);
```

## VRM (VRoid model)

```csharp
var data = new GlbFileParser(path).Parse();
var vrm = new VRMData(data);
var context = new VRMImporterContext(vrm);
var loaded = context.Load();
```

## Editor Entry

Check `RuntimeLoadMenu.cs` file dive into the details.