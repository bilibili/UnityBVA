# Runtime Load

`RuntimeLoad` scene show you how to load avatar assets.

## Simplest Method

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

### Using Shader Loader

Before execute load method, set Shader Loader firstly.

```csharp
var shaderLoader = new AssetBundleShaderLoader(Application.platform);
shaderLoader.LoadFiles(shaderAssetBundleLocation);
BVASceneManager.Instance.SetShaderLoader(shaderLoader);
```

## General Method

```csharp
if (string.IsNullOrEmpty(path)) return;
var importOptions = new ImportOptions
{
    RuntimeImport = true,
};

string directoryPath = URIHelper.GetDirectoryName(path);
importOptions.DataLoader = new CryptoZipFileLoader(directoryPath);

var Factory = ScriptableObject.CreateInstance<DefaultImporterFactory>();
var sceneImporter = Factory.CreateSceneImporter(path, importOptions);
await sceneImporter.LoadAsync();
```

## Using Shader Loader
pass `IShaderLoader` to `GLTFSceneImporter` construction method.

```csharp
public GLTFSceneImporter(string gltfFileName, ImportOptions options, IShaderLoader shaderLoader = null)
```

By default, BVA privide 2 Shader Loader:
- BuildinShaderLoader
- AssetBundleShaderLoader


```csharp
var shaderLoader = new AssetBundleShaderLoader(Application.platform);
shaderLoader.LoadFiles(shaderAssetBundleLocation);
```
For more information, check [Shader Loader](ShaderLoader.md)

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