# Unlit Shader

Unlit shader provide the best performance.
it contains only two parameter:
- `base color` - The base color of the material.
- `base map` - The base texture of the material.

`FinalColor = BaseColor * BaseMap`

In BVA, any materials that using custom shader will export Unlit Extension, this ensure that it can be open and view by other gltf tools. 