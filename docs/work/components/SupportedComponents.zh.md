# 支持的组件

Unity组件附加在`GameObject`上，由此，大部分的组件将作为Extra附加在GLTF的`Node`下面. 

这里有些特例，比如`Collider`组件, `Light`组件，对于大部分可重复使用的组件，最好还是导出为扩展。

大部分组件可以通过[代码生成工具](../../tools/ComponentExtra.zh.md)导出，以下组件已经被BVA正式采纳。


## Unity内置组件

- Camera (with Universal Render Pipeline Additional Data)
- Light (Baked Light will not be exported)
- Renderer (MeshRenderer and SkinnedMeshRenderer)
- Volume (URP PostProcess)
- Collider (BoxCollider,SphereCollider,CapsuleCollider,MeshCollider)
- RigidBody
- Animation
- Animator
- AudioSource (AudioClip will export too)
- VideoPlayer (URL only,local or network)
- ReflectionProbe (Custom Cubemap and Realtime)
- TextMesh


## BVA组件

- BlendshapeMixer
- MetaInfo
- SkyboxContainer
- AudioClipContainer
- MirrorObject
- MirrorPlain
- CameraViewPoint
- UrlAudioClipAudioSourceSetter
- UrlTextureRendererSetter
- TextureVariableCollection
- CubemapVariableCollection
- MaterialVariableCollection
- AudioClipVariableCollection
- AutoBlink
- AvatarAccessoryConfig
- AvatarDressUpConfig
- LookAt