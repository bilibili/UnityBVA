# Tools

## Material Code-Gen

Enabling developer using their own shaders. 
- Select shader
- select a Folder, must  inside `Assets`
- export and wait a seconds for compiling

![glb](pics/material_code_gen.png)

> Please make sure that the NormalMap was identified correctly. Because when exporting NormalMap, an Blit operation of `Decoding` will be applied.
> Use reflection to automatically handle the necessary import and export entries without any additional code. If you have specific keyword processing requirements, you can add an implementation in the MaterialImporter

[Check this out to know how to use the generated code.](MaterialExtra.md)

## Component Code-Gen

- Select a `GameObject` on the scene, then select the `Component` you want to gen
- Or just drag the script on it
- Select a location inside `Assets`
- Export and wait a seconds for compiling

![glb](pics/component_code_gen.png)
  
> Support primary type - bool,byte,short,ushort,int,string,float,double,decimal,LayerMask,Vector2,Vector3,Vector4,Rect,Quaternion,Color,Texture,Material,Sprite,AnimationCurve
> Serializable class or struct
> Array of primary type, List of primary type
> Array of serializable class or struct, List of serializable class or struct
> Attribute is used to automatically handle the necessary import and export entries without adding any additional code. If an error is reported, it can be manually modified. The component is added when the Node (that is, the GameObject) is created.

[Check this out to know how to use the generated code.](ComponentExtra.md)

## Cubemap Converter

- Texture2Cubemap (convert six `1:1` textures to 1 textures(row,column,panorama))
- Cubemap2Texture (convert cubemap to 6 textures or 6 on 1 row,column textures, can render the ReflectionProbe to Cubemap)
- Panorama2Cubemap (`2:1 360 degree` texture, direction can be set)

> **The common texture types**
> 
![glb](pics/CubeLayout6Faces.png)

> **Texture->Cubemap**  -  choose six `Texutre`.
> 
![glb](pics/Texture2Cubemap.png)

> **Panorama->Cubemap**  -  choose a `Panorama Texutre`.
> 
![glb](pics/Panorama2Cubemap.png)

> **Cubemap->Texture**  -  choose either a `Cubemap` or a `Reflection Probe`.
> 
![glb](pics/Cubemap2Texture.png)

## Texture Converter

- DownSample
- Convert Format (png,jpeg,tga,exr)

## Enforce Avatar T-Pose

Manually enforce an Avatar to `T-Pose` state, `T-Pose` state is a must when exporting `Humanoid` Avatar, or the `Animation` will become abnormal. Usually when exporting the humanoid model, it is necessary to stop all animation first, and set it to T-Pose state.