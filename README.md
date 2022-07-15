# Overview

BVA is a GLTF-based, cross-platform file format that can handle the 3d data and more than that. The format can be supported by many platform such as game engines, WebGL, Maya,Blender. The 3d data is stored in standard gltf format, the other info that BVA specific provide were totally extend by Extension & Extra and these are the things that distinguish it, and it doesn't break the gltf-standard, so it can be opened directly by Windows 3D Viewer, Babylon.js, three.js etc.Any tools that support gltf will also support viewing or editing the 3D part data of BVA.

What's more, it is designed to contains more than just the information that construct the 3D world, it can also store any kind of data, like Multi-Media, animation clips, Custom data, even scripting.

Furthermore, it might helps you create metaverse-like game like Roblox easily after the integretion of scripting.

## Installing BVA

### 2.1 System Requirements 

- Unity 2020.3 or later, works best on 2021.3 LTS

### 2.2 Build Target

- Standalone(Windows10 testified, Mac or Linux should also be OK)
- Android(vulkan or gles3.0 and linear texture must be supported)
- iOS 10 or later
- WebGL(works fine on Unity 2021 later,not tested)

## 2.3 Sample Build Requirements
All examples are located in `Assets/BVA/Samples`

- Windows10 or later
- MacOS
- Android or iOS(only scene `WebLoad` is currently available, OpenFileDialog support standalone platform only)


# Working with UnityBVA

As a 3D gltf compatible file format, Virtual World Asset gives the ability to Load 3D GameObject regardless of the Engine's version, build target platform, which ease you the pain of creating the Apps that involve user creation. After all, creating such a format is not a piece of cake. A complete set of tools takes even more effort.

- [Editor Export](docs/Export.md)
- [Editor Import](docs/Import.md)
- [Programming](docs/work/programing/Programing.md)

# Features

We believe that UGC will overwhelm the whole industry and everybody were encourage to devote their effort on building the wonderland and spread the imaginatin.

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
- DynamicBone Physics ([Automatic-DynamicBone by OneYoungMan](https://github.com/OneYoungMean/Automatic-DynamicBone))
- PostProcess
- Custom Material Import & Export Code Generation
- Component Code Generation (No nested structure)
- RenderSettings
- UI Framework
- Scripting


# Third-Party Cited

Forked from another open source repository , but some modifications have been made.
thanks the following open source projects

## Draco
https://github.com/atteneder/DracoUnity

### License
Copyright (c) 2019 Andreas Atteneder, All Rights Reserved. Licensed under the Apache License, Version 2.0 (the "License"); you may not use files in this repository except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

### Third party notice
Builds upon and includes builds of Google's Draco 3D data compression library (released under the terms of Apache License 2.0).

## NAudio
https://github.com/naudio/NAudio

## LiliumToonGraph
https://github.com/you-ri/LiliumToonGraph

## UniVRM
https://github.com/vrm-c/UniVRM

## NaughtyAttributes
https://github.com/dbrizov/NaughtyAttributes

## UnityPMXRuntimeLoader
https://github.com/hobosore/UnityPMXRuntimeLoader

## unity-wrapper-vorbis
https://github.com/khindemit/unity-wrapper-vorbis
- 
## shader-variant-explorer
https://github.com/needle-tools/shader-variant-explorer

### Useful commercial resources
- [Animation Converter](https://assetstore.unity.com/packages/tools/animation/animation-converter-107688) Convert animation clips (*.anim) between all 3 animation types (humanoid ⇆ generic ⇆ legacy).