# BVA_url_videoExtension

## 概念

流媒体是从服务器源以连续方式交付和消费的多媒体，在网络元素中几乎没有或没有中间存储。流媒体是指内容的交付方式，而不是内容本身。

```csharp
    public abstract class UrlAsset<T>
    {
        public string name;
        public string url;
        public string mimeType;
        public string[] alternate;
    }
```
目前只有视频流可用，但其他类型的媒体内容也将使用这种结构。

## URL属性

以下参数由 `BVA_url_videoExtension` 扩展提供：

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**name**            | `string`      | URL名称          | No    |
|**url**               | `string`      | URL地址         | Yes   |
|**mimeType**              | `string`        | 视频格式  | No   |
|**alternate**              | `List<string>`        | 备用地址 | No   |