## Supported Import types

- all types of BVA file also be regarded as the standard gltf format, so all glb file can be imported.(.glb)

- VRM file(.vrm)

- MMD model file(.pmx)

As you can see above, except the standard gltf compatible file, we've also add the support of VRM & MMD file, large amounts of anime avatar resources are shared community due to the historical reason.


## Import GLB

Drag the glb file into the project Assets folder, the Textures, Materials, Audios, will import as seperate files, others like Meshes, Animations, Avatar, will keep the data within the glb file. The glb file itself can be used as immutable prefab.

![glb](pics/import_glb.png)


## Import VRM

Drag the VRM file into the project Assets folder, the Textures,Materials, Meshes, Avatars, all related resources import as seperate assets. A prefab called VRM will be created. 

![glb](pics/import_vrm.png)


## Import MMD Model

MMD model file reference textures externally, so must copy whole folder that contains related textures into the project Assets folder, the Textures, Materials, Meshes, Avatars, all related resources import as seperate assets. A prefab has the same name with the pmx file will be created. 

![glb](pics/import_pmx.png)


## Runtime Import

You can load model directly from the Editor Menu. But before that, you have to click `play`

![glb](pics/runtime_load_on_menu.png)