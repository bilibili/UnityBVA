# Lit Shader

## Overview
The Lit Shader lets you render real-world surfaces like stone, wood, glass, plastic, and metals in photo-realistic quality. Your light levels and reflections look lifelike and react properly across various lighting conditions, for example bright sunlight, or a dark cave. This Shader uses the most computationally heavy shading model in the Universal Render Pipeline (URP). This shader can be exported as a standard gltf `metallic-roughness` material.

## The `metallic-roughness` material model is defined by the following properties:

### `base color` - The base color of the material.
The base color texture MUST contain 8-bit values encoded with the `sRGB` opto-electronic transfer function so RGB values MUST be decoded to real `linear` values before they are used for any computations. To achieve correct filtering, the transfer function SHOULD be decoded before performing linear interpolation.

### `metalness` - The metalness of the material; values range from 0.0 (non-metal) to 1.0 (metal); see Appendix B for the interpretation of intermediate values.

### `roughness` - The roughness of the material; values range from 0.0 (smooth) to 1.0 (rough).

The textures for metalness and roughness properties are packed together in a single texture called metallicRoughnessTexture. Its green channel contains roughness values and its blue channel contains metalness values. This texture MUST be encoded with linear transfer function and MAY use more than 8 bits per channel.

### `normal` - A tangent space normal texture. 
The texture encodes XYZ components of a normal vector in tangent space as RGB values stored with linear transfer function. Normal textures SHOULD NOT contain alpha channel as it not used anyway. After dequantization, texel values MUST be mapped as follows: red [0.0 .. 1.0] to X [-1 .. 1], green [0.0 .. 1.0] to Y [-1 .. 1], blue (0.5 .. 1.0] maps to Z (0 .. 1]. Normal textures SHOULD NOT contain blue values less than or equal to 0.5.

### `occlusion` - The occlusion texture;

it indicates areas that receive less indirect lighting from ambient sources. Direct lighting is not affected. The red channel of the texture encodes the occlusion value, where 0.0 means fully-occluded area (no indirect lighting) and 1.0 means not occluded area (full indirect lighting). Other texture channels (if present) do not affect occlusion.

The texture binding for occlusion maps MAY optionally contain a scalar strength value that is used to reduce the occlusion effect. When present, it affects the occlusion value as 1.0 + strength * (occlusionTexture - 1.0).

### `emissive` - The emissive texture and factor control the color and intensity of the light being emitted by the material.

The texture MUST contain 8-bit values encoded with the sRGB opto-electronic transfer function so RGB values MUST be decoded to real linear values before they are used for any computations. To achieve correct filtering, the transfer function SHOULD be decoded before performing linear interpolation.

## Global Illumination 

`Baked Global Illumination` can be in use if `emissive` is enable.