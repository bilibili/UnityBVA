# 导出

## 文件格式

- *GLTF*
- *GLB*


GLB文件包含了所有的元素，包括动画，材质，节点和相机到一个文件里。对比之下，GLTF需要额外的文件，必须贴图数据。需要了解更多的数据，点击这个[链接](https://www.khronos.org/registry/glTF/specs/2.0/glTF-2.0.html).


## 使用场景

- [*角色*](./Avatar.md)
- [*场景*](./Scene.md)

角色导出包含 BlendShape 与面捕的混合, 元数据等, 并且仅一次只能导出一个。 这保证了导出的橘色可以被直播或者游戏正确使用。

## 受支持的材质

这些材质被BVA支持。他们分别是 [Lit](material/Lit.md), [Complex Lit](material/ComplexLit.md), [Unlit](material/Unlit.md), [MToon(URP ver)](https://vrm.dev/univrm/shaders/shader_mtoon.html), [UTS2](https://github.com/unity3d-jp/UnityChanToonShaderVer2_Project), [LiliumToonGraph](https://github.com/you-ri/LiliumToonGraph), ToonLit

![glb](pics/Material_1.png)

![glb](pics/Material_2.png)

![glb](pics/Material_3.png)

对于自定义Shader，我们实现了一个代码生成工具，所以你可以选择你想要导出的Shader参数，然后生成代码来支持导出后导入使用。

> 所有的自定义Shader都会添加一个 `KHR_materials_unlitExtension` 扩展到材质下。

为了兼容标准的gltf参数，使用这些参数的材质将会导出为标准的GLTF材质参数。

|      类型     | 参数名称  | 
|--------------|-----------|
|**BaseColor**   | `{ "_BaseColor", "_Color", "_MainColor" }`      | 
|**BaseMap**       | `{ "_BaseMap", "_MainTex", "_BaseTexture", "_MainTexture" }`      | 
|**NormalMap**  | `{ "_BumpMap", "_NormalMap", "_DetailNormalMap", "_BumpTexture", "_NormalTexture", "_BumpTex", "_NormalTex" }`        |
|**EmissionMap** | `{ "_EmissionMap", "_EmissionTex", "_EmissionTexture" }`        | 
|**OcclusionMap** | `{ "_OcclusionMap", "_OcclusionTex", "_OcclusionTexture" }`        | 
|**Metallic** | `{ "_Metallic", "_MetallicFactor" }`        | 
|**Smoothness** | `{ "_Smoothness", "_SmoothnessFactor" }`        | 
|**Roughness** | `{ "_Roughness", "_RoughnessFactor" }`        | 
|**MetallicGlossMap** | `{ "_MetallicGlossMap", "_SpecGlossMap", "_GlossMap", "_SpecularMap" }`        | 

> NormalMap 必须遵循以下命名规范，因为 `Unpack` 的一个操作需要在导出前执行。可以在这里查看NormalMap的详细信息 https://docs.unity3d.com/Manual/StandardShaderMaterialParameterNormalMap.html

> 为了减少Shader的变体，强烈推荐你只使用 `Exponential Squared` 作为唯一的雾模式。


## MMD 模型导出处理

由于MMD模型的主体和头部以及所有顶点都在网格上，因此头部的Blendshape将影响所有顶点，这可能会导致输出文件的体积变得非常的大，会严重影响导出和加载的效率。

因此，在导出MMD之前，将分离受混合形状影响的顶点以形成子网格。这大大减少了导出文件的大小，以达到与MMD文件本身的大小相当。