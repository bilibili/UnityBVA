# BVA_metaExtension

## 概览

元数据是“提供有关其他数据的信息的数据”，但不是数据的内容，例如消息的文本或图像本身。有许多不同类型的元数据，包括：

- 描述性元数据——关于资源的描述性信息。它用于发现和识别。它包括标题、摘要、作者和关键字等元素。按顺序排列以形成章节。它描述了数字资料的类型、版本、关系和其他特征。
- 管理元数据 - 帮助管理资源的信息，如资源类型、权限。
- 法律元数据 - 提供有关创建者、版权所有者和公共许可的信息（如果提供）。
- 参考元数据 - 关于内容的信息。


## 元数据属性

> 以下参数由 `BVA_metaExtension` 扩展提供：

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**formatVersion**               | `string`                                                                        | 格式版本         | Yes   |
|**title**               | `string`                                                                        | 资产标题         | Yes   |
|**version**               | `string`                                                                        |  资产版本         | Yes   |
|**author**               | `string`                                                                        | 作者         | No   |
|**contact**               | `string`                                                                        | 联系信息       | No  |
|**reference**              | `string`             | 参考网址   | No                   |
|**thumbnail**              | `Id`             | 资产缩略图   | No                   |
|**contentType**              | `enum`             | 资产类型是场景还是角色   | No                   |
|**legalUser**              | `enum`             | 什么样的用户是合法的  | No                   |
|**violentUsage**              | `enum`             | 是否允许在暴力场景中使用   | No                   |
|**sexualUsage**              | `enum`             | 是否允许在性暴露场景中使用   | No                   |
|**commercialUsage**              | `enum`             | 是否允许商用   | No                   |
|**licenseType**              | `enum`             | 许可证类型  | No                   |
|**customLicenseUrl**              | `string`             | 如需制定具体协议，请在此处补充协议链接   | No                   |