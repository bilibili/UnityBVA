# KHR_materials_pbrSpecularGlossinessExtension (Archived, Not recommended use)

## Overview

This extension defines the specular-glossiness material model from `Physically-Based Rendering (PBR)`. This extensions allows glTF to support this additional workflow.

The best practices section specifies what an implementation must to do when encountering this extension, and how the extension interacts with the materials defined in the base specification.

## Unity Universal RenderPipeline

Material that using shader `Universal Render Pipeline/Lit` or  `Universal Render Pipeline/Complex Lit` with `Specular` workflow will export the extension. 

Unity `_ClearCoatMap` encode mask and smoothness into texture channel.

- Red: the Mask property. 
- Green: the Smoothness property.

This involve Texture encoding & decoding.
`clearcoatRoughnessTexture & clearcoatNormalTexture` doesn't support yet, in view of rarely using.

