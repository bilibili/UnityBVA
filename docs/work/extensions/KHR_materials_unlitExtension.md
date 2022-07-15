# KHR_materials_unlit

Currently, for any custom material, BVA will export it with unlit extension, only in this way the preview for other tools is available for any types of material.

Custom material store the info in Extras. When load a material with Mateiral Extras, the loader ignore this extension, use the Extras instead. 

## Extending Materials

The common Unlit material is defined by adding the
`KHR_materials_unlit` extension to any glTF material. When present, the
extension indicates that a material should be unlit and use available

`baseColor` values, alpha values, and vertex colors while ignoring all
properties of the default PBR model related to lighting or color. Alpha
coverage and doubleSided still apply to unlit materials.

## Example

The following example defines an unlit material with a constant red color.

```json
"materials": [
    {
        "name": "red_unlit_material",
        "pbrMetallicRoughness": {
            "baseColorFactor": [ 1.0, 0.0, 0.0, 1.0 ]
        },
        "extensions": {
            "KHR_materials_unlit": {}
        }
    }
]
```
