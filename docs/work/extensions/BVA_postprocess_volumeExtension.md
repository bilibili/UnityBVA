# BVA_postprocess_urpVolumeExtension

## Overview

Volumes components contain properties that control how they affect Cameras and how they interact with other Volumes.

It is full-screen effects and has widely used in 3D rendering that can greatly improve the appearance of your application with little set-up time.

> The properties of a volume component are described below:

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**id**               | `Id`                                                                        | Volume profile ID         | No   |
|**isGlobal**               | `bool`                                                                        | Take effect globally or only inside the collider.         | Yes   |
|**weight**               | `float`                                                                        | The amount of influence the Volume has on the Scene.          | Yes   |
|**blendDistance**               | `float`                                                                        |  Blend distance controls how Volume blend with each other.         | Yes   |
|**priority**               | `number`                                                                        | uses this value to determine which Volume it uses when Volumes have an equal amount of influence on the Scene. Uses Volumes with higher priorities first.         | Yes, Default: `0`  |

## Supported PostProcessing Effect List

- Ambient Occlusion
- Bloom
- Channel Mixer
- Chromatic Aberration
- Color Adjustments
- Color Curves
- Depth of Field
- Film Grain
- Lens Distortion
- Lift, Gamma, and Gain
- Motion Blur
- Panini Projection
- Shadows Midtones Highlights
- Split Toning
- Tonemapping
- Vignette
- White Balance

We will take one of these Effects and dive into the detail. 

### Vignette

In photography, vignetting is the term for the darkening and/or desaturating towards the edges of an image compared to the center. 

> Here are the properties of `Vignette`:

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**color**               | `Color`                                                                        | The color of the vignette.        | No   |
|**center**               | `Vector2`                                                                        | The vignette center point. For reference, the screen center is [0.5, 0.5].         | Yes   |
|**intensity**               | `float`                                                                        | The strength of the vignette effect.          | Yes   |
|**smoothness**               | `float`                                                                        |  the smoothness of the vignette borders.          | Yes   |
|**rounded**               | `bool`                                                                        | When enabled, the vignette is perfectly round. When disabled, the vignette matches the shape on the current aspect ratio.         | Yes, Default: `0`  |