# Overview

BVA is a GLTF-based, cross-platform format that store 3D data and far more than that. The format can be supported by many tools such as various game engines, Babylon.js, Maya, Blender. The 3D data is stored in standard gltf scheme, other infomation that BVA provides were totally extended by Extension & Extra ,thus it doesn't break the gltf-standard, and this will continue as tradition, to make it can be opened directly by Windows 3D Viewer, Babylon.js, mainstream modeling software etc. 

> Any tools that support gltf will also support viewing or editing the 3D part data of BVA.

Technically, it can also store any kind of data, like Multi-Media, animation clips, Custom data, even scripting.

Furthermore, it might helps you create metaverse-like game like Roblox easily after integretion of user scripting, which was already on our roadmap.

# Installing BVA

## System Requirements 

- Unity 2020.3 or later, preferred version - 2021.3 LTS

## Build Target

- Standalone (Windows10 testified, Mac or Linux should also be OK)
- Android(vulkan or gles3.0 and linear texture must be supported)
- iOS 10 or later
- WebGL (works fine on Unity 2021 later,not tested)

## Sample Build Requirements

All examples are located in `Assets/BVA/Samples`

- Windows10 or later
- MacOS
- Android or iOS(only scene `WebLoad` is currently available, OpenFileDialog support standalone platform only)

> Find more information [Get Start](docs/Get_Start.md)

# Manuals

- [Get Start](docs/Get_Start.md)
- [Avatar Setup](docs/work/Avatar.md)
- [Tools](docs/tools/Tools.md)


# Samples

- [Explor the files](docs/examples/FileViewer.md)
- [Load BVA,GLB,VRM,PMX at Runtime](docs/examples/RuntimeLoad.md)
- [Load BVA file from an url](docs/examples/WebLoad.md)
- [Export GameObject at Runtime](docs/examples/RuntimeExport.md)
- [Load multiple scene in single file at Runtime](docs/examples/MultipleScenePayload.md)
- [Show how to config an Avatar with Dress Up System](docs/examples/AvatarConfig.md)


# Working with UnityBVA SDK

As a 3D gltf compatible file format, BVA capable of loading 3D GameObjects regardless of the Engine's version, build target platform, which ease you the pain of creating the Apps that involve user-creation-contents (also known as UGC) . After all, creating such a format is not a piece of cake. A complete toolchain takes even more efforts.

- [Editor Export](docs/work/Export.md)
- [Editor Import](docs/work/Import.md)
- [Programming](docs/work/programing/Programing.md)


# Features

We believe that UGC will overwhelm the whole industry in the next decade and everybody will be encouraged to devote their effort on building the metaverse, let go out of imagination and creativity.

Uniform role model specification, provide a standard for facial & motion capture to better fit the model, and it can also be applied to VR games that need an Avatar, a single file can provide cinematic movement, the dressup system for role, scene, lighting, multimedia, all these content pack in a file and no additional attached configuration required, and it is **cross-platform as well as version compatibility, You don't have to use a particular version of the engine.**(AssetBundle has this problem)

It comes with a very efficient dynamic bone system that supports parallel computing, by utilizing Unity’s Job System and Burst compiler to create efficient code. ([Automatic-DynamicBone by OneYoungMan](https://github.com/OneYoungMean/Automatic-DynamicBone))

- Export Avatar & Scene (editor full ability & runtime partial ability)
- Import VRM & MMD model as Prefab in Editor
- Runtime Load Avatar & Scene
- Runtime Load VRM & MMD model
- Collider & Camera & Light
- Avatar
- Cubemap Texture(vertical & horizontal & panorama 360)
- Skybox(6 sided textures & cubemap)
- Lightmap
- Text Mesh
- Decal Projector
- Audio (wav & ogg format)
- VidioPlayer (URL only)
- Face & Motion Capture Compatible(BlendshapeMixer)
- LookAt (eye follwing a target)
- AutoBlink (auto blink eyes)
- Reflection Probe(Custom Texture & Realtime)
- Generic Transform & Blendshape Animation, Humanoid Avatar Animation
- DynamicBone Physics
- PostProcess (`Volume` component in Universal RP)
- Custom Material Import & Export Code Generation
- Component Code Generation (No nested structure)
- RenderSetting


# Third-Party Cited

Forked from another open source repository , but some modifications may have been made.
thanks the following open source projects

## Draco
https://github.com/atteneder/DracoUnity

### License
Copyright (c) 2019 Andreas Atteneder, All Rights Reserved. Licensed under the Apache License, Version 2.0 (the "License"); you may not use files in this repository except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

### Third party notice
> Builds upon and includes builds of Google's Draco 3D data compression library (released under the terms of Apache License 2.0).

## NAudio
https://github.com/naudio/NAudio

## LiliumToonGraph
https://github.com/you-ri/LiliumToonGraph

## UniVRM
https://github.com/vrm-c/UniVRM

## UniHumanoid
https://github.com/ousttrue/UniHumanoid

## KtxUnity
https://github.com/atteneder/KtxUnity

## NaughtyAttributes
https://github.com/dbrizov/NaughtyAttributes

## UnityPMXRuntimeLoader
https://github.com/hobosore/UnityPMXRuntimeLoader

## unity-wrapper-vorbis
https://github.com/khindemit/unity-wrapper-vorbis

## shader-variant-explorer
https://github.com/needle-tools/shader-variant-explorer

## UnityIngameDebugConsole
https://github.com/yasirkula/UnityIngameDebugConsole

# Useful commercial resources
- [Animation Converter](https://assetstore.unity.com/packages/tools/animation/animation-converter-107688) Convert animation clips (*.anim) between all 3 animation types (humanoid ⇆ generic ⇆ legacy).

# License
[Apache License, Version 2.0](License.md)