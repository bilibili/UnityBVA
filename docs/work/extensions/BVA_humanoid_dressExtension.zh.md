# BVA_humanoid_dressExtension

## 概览
此扩展定义了 Avatar 使用的装扮系统。

Avatar装扮系统是通过切换GameObject的`Renderer`和Renderer的`materials`的可见性来实现的。

## 导出前提

要使用人物换装系统，文件中必须包含 `BVA_humanoid_avatarExtension` 扩展，在有Avatar的前提下才可以添加换装功能。


## 换装方案的定义

> `GltfRendererMaterialConfig` 指定节点上可用的材质，节点必须包含一个网格，由于一个网格可能有多个子网格，所以将材质记录为一个数组列表。

```csharp
    public struct GltfRendererMaterialConfig
    {
        public int node;
        public List<int> materials;
    }
```

> `DressUpConfig` 代表一个换装的设置。

```csharp
    public class DressUpConfig
    {
        public string name;
        public List<GltfRendererMaterialConfig> rendererConfigs;
    }
```

> `GltfDress` 把Avatar包含的所有的换装方案放到一个数组列表中。

```csharp
    public struct GltfDress
    {
        public List<DressUpConfig> dressUpConfigs;
    }
```

> 下列参数由`BVA_humanoid_dressExtension`贡献:

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**dressUpConfigs**               | `GltfDress`                                                                        | all dressing solutions.         | Yes   |


> DressUpConfig

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**name**               | `string`                                                                        | The name of the dressing solution.         | Yes   |
|**rendererConfigs**               | `List<GltfRendererMaterialConfig>`                                                                        | All visible renderers and materials used are recorded | Yes   |


> GltfRendererMaterialConfig

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**node**               | `number`                                                                        | The index of the node, in which contains a valid Renderer.         | Yes   |
|**materials**               | `List<int>`                                                                        | All materials on Renderer. | Yes   |
