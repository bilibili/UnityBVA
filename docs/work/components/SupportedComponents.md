# Supported Components

Unity Components attach to `GameObject`, most of it export as extras under the `Node`. 

There are some exceptions, like `Collider`, `Light`, for a large number of reusable components, it is best to export as extensions.

Most Components can be exported by code generator, these have been officially adopted.


### Unity build-in components:

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


### BVA components:

- BlendshapeMixer
- MetaInfo
- SkyboxContainer
- AudioClipContainer
- AutoBlink
- AvatarAccessoryConfig
- AvatarDressUpConfig
- LookAt
- MirrorObject
- MirrorPlain
- CameraViewPoint
- UrlAudioClipAudioSourceSetter
- UrlTextureRendererSetter
- TextureVariableCollection
- CubemapVariableCollection
- MaterialVariableCollection
- AudioClipVariableCollection