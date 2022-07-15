# BVA_ui_spriteExtension

## Overview

Sprites are 2D Graphic objects. If you are used to working in 3D, Sprites are essentially just standard textures but there are special techniques for combining and managing sprite textures for efficiency and convenience during development.

## Sprite Properties

|          | Type    | Description             | Required       |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**name**             | `string`      | The layout of the cubemap.           | No    |
|**texture**               | `id`      | The reference to the used texture. If packed this will point to the atlas, if not packed will point to the source sprite.          | Yes   |
|**rect**             | `Rect`        | Location of the Sprite on the original Texture, specified in pixels. | Yes   |
|**pivot**            | `enum`      | Location of the Sprite's center point in the Rect on the original Texture, specified in pixels.           | Yes    |
|**pixelsPerUnit**               | `float`      | The number of pixels in the sprite that correspond to one unit in world space.          | Yes   |
|**border**           | `Vector4`        | Returns the border sizes of the sprite.(X=left, Y=bottom, Z=right, W=top). | Yes   |
|**generateFallbackPhysicsShape**            | `bool`      |  Generates a default physics shape for the sprite.           | No, Default  `yes`      |
