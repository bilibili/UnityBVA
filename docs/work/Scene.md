# Scene Export

## Export Sample

In order to explain the workflow of BVA import and export in detail, the following demonstrates the import of gltf resources downloaded from the Internet to the project, and then configured and output as a BVA scene file.

1. Download a room model and a DJ Mixer model from `Sketchfab`

sydney-apartment-night             |  pioneer-xdj
:-------------------------:|:-------------------------:
[![sydney-apartment-night](pics/sydney-apartment-night-custom-home.png)](https://sketchfab.com/3d-models/sydney-apartment-night-custom-home-07da6393b6434adcb74023858d858ec6)  |  [![pioneer-xdj](pics/pioneer-xdj.png)](https://sketchfab.com/3d-models/pioneer-xdj-rx2-fbx-2020-73ff0de3ac0346fbbbc5784d416080a1)


2. Unzip the downloaded file and drag the entire folder of the model into the Unity resource bar

![glb](pics/import_gltf.png)

Wait a while for the imported resources to be generated

![glb](pics/imported_gltf.png)

You can also see the Preview view on the right, at which point you can drag the file directly into the scene like FBX, and Unity will treat it as a Prefab

![glb](pics/gltf_preview.png)

3. Create a new empty scene, add lights and cameras, add two `Area Lights` in the room and drag them to the right size, place one on both sides of Y axis, place two `Spot Lights`, set the material of the two lights to self-illuminating and set to `Baked Global Illumination`, so that all the baked lights are set

![glb](pics/scene_lamp_selected.png)

![glb](pics/lamp_material_emission.png)

4. Adjust the overall scale of the room, and drag all the objects out to the root directory, so that the exported scenes can be displayed one by one when loading, so that the loading process looks more coherent

![glb](pics/scene_0.png)
![glb](pics/scene_1.png)

5. Set all objects in the room to static and set the level of objects to `BVA Scene Static(Lightmap)`, this level is used to bake static objects
![glb](pics/scene_3.png)

6. Open the Lighting Settings panel and uncheck `Realtime Global Illumination`, check `Backed Global Illumination`, set each baking parameter and click `Generate Lighting` bake the scene and wait a few minutes for the baking to finish

![glb](pics/lighting_0.png)

7. Add a `Directional Light`，set to `Realtime` mode, and set `Culling Mask` to `BVA Scene Realtime`, then drag the DJ Mixer into scene, set Layer to `BVA Scene Realtime`, only in this way this DJ Mixer will be affected by this realtime light.

![glb](pics/lighting_1.png)

![glb](pics/pioneer_gameObject.png)

8. At last, add post-process, on `Hierarchy` panel, right click `Volume/Global Volume`. Then add `Bloom` and `Tonemapping`。

![global volume](pics/add_global_volume.png)

9. Open export panel by Editor menu `BVA/Export/Export Scene`, select `Export Single Scene`, check `export original texture`, this will improve the quility of export model, then check the `Export RenderSetting` & `Export Lightmap`. In general, only the scene where the baking is used needs to check these two items.

![global volume](pics/export_scene_setting.png)

10. The output file can be loaded Sample, either in Editor and build applications, click on the image below to see the loading state on the mobile platform

[![Watch the video](../pics/Cover_2.png)](https://www.bilibili.com/video/BV1fr4y1V7Rk)

## Export inside editor supports all features. Three types exporting method

1. Export Root GameObject with all its childs
![glb](pics/scene_import_0.png)

1. Export Single Scene
![glb](pics/scene_import_1.png)

1. Export Multiple Scene
![glb](pics/scene_import_2.png)

## Settings
- NonSkin Mesh(mesh renderered by `MeshRenderer` instead of `SkinnedMeshRenderer`) can apply [google draco](https://github.com/google/draco) compression.It is intended to improve the storage and transmission of 3D graphics.

- When you try to export a scene with baked lighting. Lightingmap will be created in your project. To correctly export lightmap, make sure toggele off the `Realtime Global Illumination`,and Turn on the `Baked Global Illumination` at `Lighting` setting panel.

![glb](pics/scene_export_setting_0.png)

- RenderSettings contains some global settings, which can access from static class `UnityEngine.RenderSetttings`, most of info can also altered by hand through `Environment` tab on `Lighting` panel

![glb](pics/scene_export_setting_1.png)

- To decrease the shader variants, we highly recommend that you only use `Exponential Squared` mode as the only fog mode. This can be modified in `PlayerSetting/Graphics` Tab.

![glb](../pics/shader_stripping.png)

- Skybox export