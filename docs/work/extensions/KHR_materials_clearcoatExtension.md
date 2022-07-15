# KHR_materials_clearcoatExtension

## Overview

This extension defines a clear coating that can be layered on top of an existing glTF material definition.  A clear coat is a common technique used in Physically-Based Rendering to represent a protective layer applied to a base material.  See [Theory, Documentation and Implementations](#theory-documentation-and-implementations)

## Unity Universal RenderPipeline
Material that using shader `Universal Render Pipeline/Complex Lit` will export the extension. 

Unity _ClearCoatMap encode mask and smoothness into texture channel.

- Red: the Mask property. 
- Green: the Smoothness property.

This involve Texture encoding & decoding.
clearcoatRoughnessTexture & clearcoatNormalTexture doesn't support yet, in view of rarely using.

## Extending Materials

The PBR clearcoat materials are defined by adding the `KHR_materials_clearcoat` extension to any compatible glTF material (excluding those listed above).  For example, the following defines a material like varnish using clearcoat parameters.

```json
{
    "materials": [
        {
            "name": "varnish",
            "extensions": {
                "KHR_materials_clearcoat": {
                    "clearcoatFactor": 1.0
                }
            }
        }
    ]
}
```

### Clearcoat

The following parameters are contributed by the `KHR_materials_clearcoat` extension:
|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**clearcoatFactor**               | `number`                                                                        | The clearcoat layer intensity.         | No, default: `0.0`   |
|**clearcoatTexture**              | [`textureInfo`](https://www.khronos.org/registry/glTF/specs/2.0/glTF-2.0.html#reference-textureinfo)             | The clearcoat layer intensity texture. | No                   |
|**clearcoatRoughnessFactor**      | `number`                                                                        | The clearcoat layer roughness.         | No, default: `0.0`   |
|**clearcoatRoughnessTexture**     | [`textureInfo`](https://www.khronos.org/registry/glTF/specs/2.0/glTF-2.0.html#reference-textureinfo)             | The clearcoat layer roughness texture. | No                   |
|**clearcoatNormalTexture**        | [`normalTextureInfo`](https://www.khronos.org/registry/glTF/specs/2.0/glTF-2.0.html#reference-material-normaltextureinfo) | The clearcoat normal map texture.      | No                   |

If `clearcoatFactor` (in the extension) is zero, the whole clearcoat layer is disabled.

The values for clearcoat layer intensity and clearcoat roughness can be defined using factors, textures, or both. If the `clearcoatTexture` or `clearcoatRoughnessTexture` is not given, respective texture components are assumed to have a value of 1.0. All clearcoat textures contain RGB components in linear space. If both factors and textures are present, the factor value acts as a linear multiplier for the corresponding texture values.

```
clearcoat = clearcoatFactor * clearcoatTexture.r
clearcoatRoughness = clearcoatRoughnessFactor * clearcoatRoughnessTexture.g
```

If `clearcoatNormalTexture` is not given, no normal mapping is applied to the clear coat layer, even if normal mapping is applied to the base material.  Otherwise, `clearcoatNormalTexture` may be a reference to the same normal map used by the base material, or any other compatible normal map.

