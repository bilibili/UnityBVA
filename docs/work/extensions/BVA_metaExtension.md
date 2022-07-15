# BVA_metaExtension

## Overview

Metadata is "data that provides information about other data", but not the content of the data, such as the text of a message or the image itself. There are many distinct types of metadata, including:

- Descriptive metadata — the descriptive information about a resource. It is used for discovery and identification. It includes elements such as title, abstract, author, and keywords.ordered to form chapters. It describes the types, versions, relationships and other characteristics of digital materials.
- Administrative metadata — the information to help manage a resource, like resource type, permissions.
- Legal metadata — provides information about the creator, copyright holder, and public licensing, if provided.
- Reference metadata — the information about the contents.


## Metainfo Properties
> The following parameters are contributed by the `BVA_metaExtension` extension:

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**formatVersion**               | `string`                                                                        | Format version.         | Yes   |
|**title**               | `string`                                                                        | Title for this asset.         | Yes   |
|**version**               | `string`                                                                        |  Asset version.         | Yes   |
|**author**               | `string`                                                                        | Author         | No   |
|**contact**               | `string`                                                                        | Contact information.       | No  |
|**reference**              | `string`             | Reference URL.   | No                   |
|**thumbnail**              | `Id`             | Thumbnail of asset.   | No                   |
|**contentType**              | `enum`             | Indicate whether this asset is a avatar or a scene.   | No                   |
|**legalUser**              | `enum`             | What kind of person can use this asset.   | No                   |
|**violentUsage**              | `enum`             | Whether it is allowed in violent situations.   | No                   |
|**sexualUsage**              | `enum`             | Whether it is allowed in sexual situations.   | No                   |
|**commercialUsage**              | `enum`             | Whether commercial use is permitted.   | No                   |
|**licenseType**              | `enum`             | License type.   | No                   |
|**customLicenseUrl**              | `string`             | If you need to draw up specific agreements please supplement the agreement link here.   | No                   |