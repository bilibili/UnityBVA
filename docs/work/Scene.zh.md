# 场景导出

## 在Editor下的导出支持所有的功能

1. 导出Root物体及其所有子物体
![glb](pics/scene_import_0.png)

1. 导出单个场景
![glb](pics/scene_import_1.png)

1. 导出多个场景
![glb](pics/scene_import_2.png)

## 设定

- 非蒙皮网格（由`MeshRenderer`而不是`SkinndMeshRenderer`渲染的网格）可以使用[google draco](https://github.com/google/draco)进行压缩。其目的是改善三维图形的存储和传输。

- 当您尝试导出具有烘焙照明的场景时。Lightingmap将在项目中创建。要正确导出光照贴图，请确保关闭`Realtime Global Illumination`，并在`Lighting`设置面板上打开`Baked Global Illumination`。

![glb](pics/scene_export_setting_0.png)

- RenderSettings包含一些全局设置，可以从静态类访问`UnityEngine.RenderSetttings`, 大多数信息也可以通过手工修改 `Environment` 栏的 `Lighting` 界面。

![glb](pics/scene_export_setting_1.png)

- 为了减少着色器变体，我们强烈建议您仅使用 `Exponential Squared` 作为唯一雾模式. 这可以在`PlayerSetting/Graphics` 中修改。

![glb](../pics/shader_stripping.png)

- 天空盒导出