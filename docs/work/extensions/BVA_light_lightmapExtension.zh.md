# BVA_light_lightmapExtension

## 概览

这个扩展定义了烘焙光照贴图。

光照贴图是预先计算场景中表面亮度的过程。

### 烘焙光照属性

> 以下参数由 `BVA_light_lightmapExtension` 扩展提供：

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**lightmapsMode**              | `enum`             | 光照贴图（和光照）配置模式，控制光照贴图如何与光照交互以及它们存储什么样的信息 | Yes                   |
|**lightmapsEncoding**              | `enum`             | 不同的压缩和编码方案，取决于目标平台和压缩设置  | Yes                   |
|**lightmaps**              | `LightmapTextureInfo[]`             | 纹理存储每个灯光的遮挡遮罩(ShadowMask，最多四个灯光)  | No                   |


### 烘焙光照贴图属性

> `LightmapTextureInfo`的结构:

```csharp
    public struct LightmapTextureInfo
    {
        public TextureId lightmapColor;
        public TextureId lightmapDir;
        public TextureId shadowMask;
    }
```

> 每个纹理存储有助于场景照明的不同信息：

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**lightmapColor**              | `Id`             | 存储入射光的颜色  | Yes                   |
|**lightmapDir**              | `Id`             | 存储入射光的主要方向 | Yes                   |
|**shadowMask**              | `Id`             | 存储每盏灯的遮挡遮罩  | No                   |