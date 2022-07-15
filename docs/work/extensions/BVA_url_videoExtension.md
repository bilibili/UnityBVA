# BVA_url_videoExtension

## Overview

Streaming media is multimedia that is delivered and consumed in a continuous manner from a source, with little or no intermediate storage in network elements. Streaming refers to the delivery method of content, rather than the content itself.

```csharp
    public abstract class UrlAsset<T>
    {
        public string name;
        public string url;
        public string mimeType;
        public string[] alternate;
    }
```
Currently only video streaming is available, but other types of media content will use this structure as well.

## URL Properties

The following parameters are contributed by the `BVA_url_videoExtension` extension:

|          | Type    | Description             | Required       |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**name**            | `string`      | The name of the url.           | No    |
|**url**               | `string`      | The URL of the asset.          | Yes   |
|**mimeType**              | `string`        | The video formats.  | No   |
|**alternate**              | `List<string>`        | Alternate URL address. | No   |