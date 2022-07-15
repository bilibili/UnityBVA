# Extras的设计原理

## 概览

extras则偏向于添加不能被通用化的一些信息，像Unity的很多组件，这些就不太可能也被其他引擎支持或开发需求广泛用到~

其组件可能一直在更新，极有可能存在版本更新后属性命名更改或添加新字段的事情发生，为此设计为extra，并提供代码生成器快速生成代码，这个东西也是为了使用该SDK的厂商做自己的功能性补充。但是已有的开发为extension的东西都是具备很强通用性的信息。

> 举例说明使用extras作为信息记录的场景：

- `Blendshape`的名称，标准的`gltf`不支持保存网格`blendshape`的名称，但是一般3D模型导出的时候都包含了名称这一信息，为了方便直接在extras里面记录名称信息。
- `Camera`的更多信息，我们在延用官方extension以保证可以在其他所有支持标准gltf的工具中使用的同时，也对其`Camera`中的其余的必要信息做了extras的导出，以便更好的支持实际场景的还原。
- 材质信息，目前属于还没有确定要被广泛支持的材质，在此不确定情况下的通过`code-gen`生成导入导出的代码。并且这些自定义材质，会导出额外的`KHR_materials_unlitExtension`扩展，以保证能被其他工具使用。
- UI，框架并未确定具体按什么标准去实现，所以目前还是设计为extras，待整个设计确定下来后可以设计为extension

目前受支持的信息有动画，声音，相机，灯光，材质，物理，渲染等类型的组件均有信息导出，自定义的脚本公开访问的变量也支持使用代码生成工具导出。

> 导出方式 - 主要使用代码生成工具导出，目前有两个方向的代码生成工具

- 用于导出材质参数
- 用于导出组件上的公共变量

### 材质参数导出

gltf标准下，标准的PBR材质属性依旧会被导出，而且`KHR_materials_unlit`的extension会被导出，以被其他标准工具打开，默认的"_MainTex"或"_BaseMap"名称得贴图会被视为`baseColorTexture`，自定义得材质参数全部在extras下面导出。

> 结构如下所示，先指定使用的着色器类型`Shader Graphs/MToon`，括号里面再包含各项材质参数信息:

```json
    {
      "pbrMetallicRoughness": {
        "baseColorTexture": {
          "index": 0
        }
      },
      "name": "0.jouhanshin",
      "extensions": {
        "KHR_materials_unlit": {}
      },
      "extras": {
        "Shader Graphs/MToon": {
          "_Color": [
            1.0,
            1.0,
            1.0,
            1.0
          ],
          "_MainTex": {
            "index": 0
          },
          "_ShadeColor": [
            1.0,
            1.0,
            1.0,
            1.0
          ],
          "_ShadeTexture": {
            "index": 0
          },
          "_ShadeToony": 0.9,
          "_EmissionColor": [
            0.0,
            0.0,
            0.0,
            1.0
          ],
          "_OutlineWidth": 0.04,
          "_ToonyLighting": 1.0,
          "_ShadeShift": 0.0,
          "_OutlineColor": [
            0.713726,
            0.517647,
            0.486274958,
            1.0
          ],
          "_CutOff": 0.5,
          "_QueueOffset": 0.0,
          "_QueueControl": 0.0
        }
      }
    }
```

### 组件参数导出

组件通常在Nodes下导出为extras信息

> 如下所示，先指定组件的类型`Animation`，括号里面再包含各项公共参数信息:

```json
"nodes": [
    {
      "children": [
        1,
        548,
        551
      ],
      "name": "VRM_Legacy",
      "extensions": {
        "BVA_meta": {
          "meta": 0
        },
        "BVA_humanoid_avatarExtension": {
          "avatars": 0
        },
        "BVA_blendShape_blendShapeMixer": {
          "mixer": 0
        },
        "BVA_physics_dynamicBone": {
          "physics_dynamicBone": [
            0
          ]
        }
      },
      "extras": {
        "Animation": {
          "playAutomatically": true,
          "wrapMode": "Default",
          "animatePhysics": false,
          "cullingType": "AlwaysAnimate",
          "animation": 7,
          "animations": [
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7
          ]
        }
      }
    }
]
```