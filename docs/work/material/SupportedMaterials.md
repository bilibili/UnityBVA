# Mateirals

There are four types of materials (shaders) supported by BVA. These are [unlit](Unlit.md), [lit](Lit.md), [complex lit](ComplexLit.md),  [MToon(VRM)](MToon.md).

For custom shader, we implement a code-gen tools, so you can select shader parameters that you want to Export & Import, then generate code to import and export material information. 

`All custom material information stores in extras, and will export KHR_materials_unlitExtension under the material.`

To compatible with standard gltf parameter, material with these parameters will export as standard gltf material parameters.

|     类型     | 参数名称   | 
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

> NormalMap must follow naming rules, Because `Unpack` is required when exporting. See how https://docs.unity3d.com/Manual/StandardShaderMaterialParameterNormalMap.html

> To decrease the shader variants, we highly recommend that you only use `Exponential Squared` mode as the only fog mode.