# ShaderLoader

By implement the interface `IShaderLoader`, anyone can create their own shader loader.
```csharp
public interface IShaderLoader
{
    Shader Find(string name);
    string GetVersion(string name);
}
```

## BuildinShaderLoader

It's just a wrapping method, call `Shader.Find` method. If passing null into `GLTFSceneImporter` construction, `BuildinShaderLoader` will be used.

```csharp
public GLTFSceneImporter(string gltfFileName, ImportOptions options, IShaderLoader shaderLoader = null)
```

## AssetBundleShaderLoader

Enabling loading Shaders from `AssetBundle`.

Two methods for loading `AssetBundle`

```csharp
- public void Load(string assetBundleFile, string version = NO_VERSION)
- public void LoadFiles(string folder, string version = NO_VERSION)
``` 