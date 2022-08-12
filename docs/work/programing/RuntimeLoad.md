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

Check `RuntimeLoadMenu.cs` dive into the details.

## Import Settings

These options can be founded at `GLTFSceneImporter.cs`

|    Name       | Type       | Description     | Default             |
|-----------|-------------------|------------------------|----------------------|
|**MaximumLod**   | `int`           | Maximum LOD         | 300   |
|**IsMultithreaded**  | `bool`      |  Use Multithreading or not. In editor, this is always false. This is to prevent a freeze in editor   | true   |
|**SceneParent**   | `Transform` | The parent transform for the created GameObject         | null   |
|**CreatedObject** | `GameObject`| The last created object         | null  |
|**Collider**      | `ColliderType` | Adds colliders to primitive objects when created | None                   |
|**KeepCPUCopyOfMesh**  | `bool`   | Whether to keep a CPU-side copy of the mesh after upload to GPU (for example, in case normals/tangents need recalculation)  | null   |
|**KeepCPUCopyOfTexture**  | `bool`   | Whether to keep a CPU-side copy of the texture after upload to GPU  | null   |
|**GenerateMipMapsForTextures**  | `bool`   | Specifies whether the MipMap chain should be generated for model textures  | null   |
|**EnableEnvironmentReflection**  | `bool`   | Specifies whether Environment Reflection should be enabled for materials  | null   |
|**EnableSpecularHighlight**  | `bool`   | Specifies whether Specular Highlight should be enabled for materials   | null   |
|**CullFarLOD**  | `bool`   | When screen coverage is above threashold and no LOD mesh cull the object  | null   |