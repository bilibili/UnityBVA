# BVA_setting_renderSettingExtension

## Overview
RenderSetting affect many graphic aspect of the scene:
- Shadow
- Fog
- Ambient Light
- Reflection
- Flare
- Halo


|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**subtractiveShadowColor**               | `Id`                                 |  The color used for the sun shadows in the Subtractive lightmode.       | No   |
|**ambientMode**               | `enum`                                          |  Ambient lighting mode, decide where to get ambient light from.         | No   |
|**ambientIntensity**               | `float`                                    |  How much the light from the Ambient Source affects the Scene.         | No   |
|**ambientLight**               | `Color`                                        |  Flat ambient lighting color.         | No   |
|**ambientSkyColor**               | `Color`                                     |  Ambient lighting coming from above.        | No  |
|**ambientEquatorColor**               | `Color`                                 |  Ambient lighting coming from the sides.         | No   |
|**ambientGroundColor**               | `Color`                                  |  Ambient lighting coming from below.       | No   |
|**reflectionIntensity**               | `float`                                 |  How much the skybox / custom cubemap reflection affects the Scene.        | No  |
|**reflectionBounces**               | `float`                                   |  The number of times a reflection includes other reflections.          | No   |
|**defaultReflectionResolution**               | `float`                         |  Cubemap resolution for default reflection.         | No   |
|**flareFadeSpeed**               | `float`                                      |  The fade speed of all flares in the Scene.         | No  |
|**flareStrength**               | `float`                                       |  The intensity of all flares in the Scene.         | No  |
|**haloStrength**               | `float`                                        |  Size of the Light halos.          | No   |
|**fog**               | `bool`                                                  |  Is fog enabled.        | No   |
|**fogColor**               | `Color`                                            |  The color of the fog.         | No  |
|**fogDensity**               | `float`                                          |  The density of the exponential fog.         | No  |
|**fogStartDistance**               | `float`                                    |  The starting distance of linear fog.          | No   |
|**fogEndDistance**               | `float`                                      |  The ending distance of linear fog.       | No   |
|**fogMode**               | `enum`                                              |  Fog mode to use.         | No  |

