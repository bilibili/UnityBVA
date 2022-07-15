# BVA_setting_renderSettingExtension

## 概览
RenderSetting 影响场景的许多图形方面：
- 阴影
- 多雾路段
- 环境光
- 反射
- 耀斑
- 光环


|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**subtractiveShadowColor**               | `Id`                                 |  在Subtractive模式下的太阳光阴影颜色      | No   |
|**ambientMode**               | `enum`                                          |  环境光模式，决定从哪里获取环境光       | No   |
|**ambientIntensity**               | `float`                                    |  环境光影响场景的强度         | No   |
|**ambientLight**               | `Color`                                        |  环境光颜色        | No   |
|**ambientSkyColor**               | `Color`                                     |  来自上方的环境照明        | No  |
|**ambientEquatorColor**               | `Color`                                 |  来自侧面的环境照明         | No   |
|**ambientGroundColor**               | `Color`                                  |  来自下方的环境照明      | No   |
|**reflectionIntensity**               | `float`                                 |  天空盒/自定义立方体贴图反射对场景的影响程度        | No  |
|**reflectionBounces**               | `float`                                   |  反射包含其他反射的次数          | No   |
|**defaultReflectionResolution**               | `float`                         |  默认反射的立方体贴图分辨率         | No   |
|**flareFadeSpeed**               | `float`                                      |  场景中所有耀斑的淡入淡出速度        | No  |
|**flareStrength**               | `float`                                       |  场景中所有耀斑的强度         | No  |
|**haloStrength**               | `float`                                        |  光晕的大小          | No   |
|**fog**               | `bool`                                                  |  是否开启雾效       | No   |
|**fogColor**               | `Color`                                            |  雾的颜色        | No  |
|**fogDensity**               | `float`                                          |  指数雾的密度        | No  |
|**fogStartDistance**               | `float`                                    |  线性雾的起始距离         | No   |
|**fogEndDistance**               | `float`                                      |  线性雾的终止距离      | No   |
|**fogMode**               | `enum`                                              |  要使用的雾模式        | No  |

