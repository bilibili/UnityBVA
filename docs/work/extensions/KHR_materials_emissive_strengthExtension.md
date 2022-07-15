# KHR_materials_unlit

## Exclusions

* This extension must not be used on a material that also uses `KHR_materials_unlit`.

## Overview

The core glTF 2.0 material model includes an `emissiveFactor` and an `emissiveTexture` to control the color and
intensity of the light being emitted by the material, clamped to the range [0.0, 1.0]. However, in PBR environments
with high-dynamic range reflections and lighting, stronger emission effects may be desirable.

In this extension, a new `emissiveStrength` scalar factor is supplied, that governs the upper limit of emissive
strength per material.

**Implementation Note**: This strength can be colored and tempered using the core material's `emissiveFactor`
and `emissiveTexture` controls, permitting the strength to vary across the surface of the material.
Supplying values above 1.0 for `emissiveStrength` can have an influence on
reflections, tonemapping, blooming, and more.

### Physical Units

This extension supplies a unitless multiplier to the glTF 2.0 specification's emissive factor and
texture.  Including this multiplier does not alter the physical units defined in glTF 2.0's
[additional textures section](https://www.khronos.org/registry/glTF/specs/2.0/glTF-2.0.html#additional-textures),
under the **emissive** texture.

## Extending Materials

*This section is non-normative.*

Any material with an `emissiveFactor` (and optionally an `emissiveTexture`) can have its strength modulated
or amplified by the inclusion of this extension.  For example:

```json
{
    "materials": [
        {
            "emissiveFactor": [
                1.0,
                1.0,
                1.0
            ],
            "emissiveTexture": {
                "index": 3
            },
            "extensions": {
                "KHR_materials_emissive_strength": {
                    "emissiveStrength": 5.0
                }
            }
        }
    ]
}
```

In the above example, the `emissiveFactor` has been set to its maximum value, to enable the `emissiveTexture`.
The `emissiveStrength` has been set to 5.0, making the texture five times brighter than it otherwise
would have been.

### Parameters

The following parameters are contributed by the `KHR_materials_emissive_strength` extension:

| Name                   | Type       | Description                                                                   | Required           |
|------------------------|------------|-------------------------------------------------------------------------------|--------------------|
| **emissiveStrength**   | `number`   | The strength adjustment to be multiplied with the material's emissive value.  | No, default: `1.0` |


## Implementation Notes

*This section is non-normative.*

A typical (pseudocode) implementation might look like the following:

```
color += emissiveFactor.rgb * sRGB_to_Linear(emissiveTexture.rgb) * emissiveStrength;
```

## Schema

- [glTF.KHR_materials_emissive_strength.schema.json](schema/glTF.KHR_materials_emissive_strength.schema.json)
