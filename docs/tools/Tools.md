# Tools

### Material Code-Gen
- Select shader,
- select the `Folder inside Assets`
- export and wait a seconds for compiling.

### Component Code-Gen
- Select a `GameObject` on the scene
- Select the `Component` you want to gen
- Select the `Folder inside Assets`
- Export and wait a seconds for compiling.

### Cubemap Converter
- Texture2Cubemap (convert six `1:1` textures to 1 textures(row,column,panorama))
- Cubemap2Texture (convert cubemap to 6 textures or 6 on 1 row,column textures, can render the ReflectionProbe to Cubemap)
- Panorama2Cubemap (`2:1 360 degree` texture, direction can be set)

The common texture types as follow:

![glb](pics/CubeLayout6Faces.png)

### Texture Converter
- DownSample
- Convert Format (png,jpeg,tga,exr)

### Enforce Avatar T-Pose
- Enforce an Avatar to T-Pose state, This is critical when exporting `Humanoid` Avatar, or the `Animation` will become abnormal.