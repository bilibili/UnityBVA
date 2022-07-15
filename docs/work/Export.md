# Export Types

## File Format
- *GLTF*
- *GLB*


The GLB file locates all of the elements, including animations, materials, node hierarchy and cameras in one single file. In comparison, the GLTF file requires external files, such as for textures data. For more info about gltf, check this [link](https://www.khronos.org/registry/glTF/specs/2.0/glTF-2.0.html).


## Usage Scenarios
- [*Avatar*](./Avatar.md)
- [*Scene*](./Scene.md)

Avatar export with extra informatin such as BlendShape, MetaInfo, and should export only one Humanoid Model at once. This make sure the exported avatar can be used in Streaming or Gaming correctly.

## MMD Model Convert

Since the MMD model's body and head and all the vertices are on a mesh, the blendshape on head will take effect on all vertices, this can result in very large output files, which can seriously affect the efficiency of exporting and loading. Therefore, before exporting MMD, the vertices affected by BlendShape will be separated to form a Submesh.This significantly reduces the size of the exported file, comparable to the size of the MMD file itself.