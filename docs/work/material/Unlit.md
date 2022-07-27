# Unlit Shader

Unlit shader provide the best performance.
it contains only two parameter:
- `baseColor` - The base color of the material.
- `baseMap` - The base texture of the material.

`FinalColor = BaseColor * BaseMap`

In BVA, any materials that using custom shader will export `KHR_materials_unlitExtension`, this ensure previewing by other gltf tools. 