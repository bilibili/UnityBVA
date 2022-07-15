# BVA_humanoid_dressExtension

## Overview

This extension defines the dress up system that used by Avatar.

The Avatar dressing system is implemented by switching the visibility of the GameObject's `Renderer` and Renderer's `materials`. 

## Export prerequisites 

To use avatar dressing system, it is necessary that the `BVA_humanoid_avatarExtension` extension be included in the file.

### Dress Defines

`GltfRendererMaterialConfig` specifies which materials are available on the node, the node must contains a mesh, and might has more than one submesh, that's why we record the material in a list.

```csharp
    public struct GltfRendererMaterialConfig
    {
        public int node;
        public List<int> materials;
    }
```

`DressUpConfig` represents an dressing solution.

```csharp
    public class DressUpConfig
    {
        public string name;
        public List<GltfRendererMaterialConfig> rendererConfigs;
    }
```

`GltfDress` contains all dressing solutions in a bundle.

```csharp
    public struct GltfDress
    {
        public List<DressUpConfig> dressUpConfigs;
    }
```

> The following parameters are contributed by the `BVA_humanoid_dressExtension`:

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**dressUpConfigs**               | `GltfDress`                                                                        | all dressing solutions.         | Yes   |

> DressUpConfig 

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**name**               | `string`                                                                        | The name of the dressing solution.         | Yes   |
|**rendererConfigs**               | `List<GltfRendererMaterialConfig>`                                                                        | All visible renderers and materials used are recorded | Yes   |

> GltfRendererMaterialConfig

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**node**               | `number`                                                                        | The index of the node, in which contains a valid Renderer.         | Yes   |
|**materials**               | `List<int>`                                                                        | All materials on Renderer. | Yes   |
