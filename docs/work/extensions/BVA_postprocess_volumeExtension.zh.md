# BVA_postprocess_urpVolumeExtension

## 概览

体积组件包含控制它们如何影响相机以及它们如何与其他体积交互的属性。
它是一种作用于全屏的效果，现在已广泛用于 3D 渲染，可以大大改善渲染效果，并且只需很少的时间设置。

> 以下描述了一个体积组件的属性：

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**id**               | `Id`                                                                        | 配置文件 ID         | No   |
|**isGlobal**               | `bool`                                                                        | 是否是全局生效还是只在碰撞体内部的时候生效         | Yes   |
|**weight**               | `float`                                                                        | 体积组件对场景的影响程度          | Yes   |
|**blendDistance**               | `float`                                                                        |  控制当多个体积组件同时影响的时候，进行效果的混合         | Yes   |
|**priority**               | `number`                                                                        | 当多个体积组件对场景有同等影响时，使用此值来确定它使用的体积。首先使用具有更高优先级的卷。        | Yes, Default: `0`  |

## 支持的后处理效果列表

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

我们将采用其中一种效果并深入研究细节。

### Vignette
在摄影中，渐晕是指与中心相比，图像边缘变暗和/或去饱和的术语。

> 以下是`Vignette`的属性：

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**color**               | `Color`           | 渐晕的颜色        | No   |
|**center**               | `Vector2`        | 中心点，作为参考，屏幕中心为 [0.5, 0.5]        | Yes   |
|**intensity**               | `float`       | 晕影效果的强度         | Yes   |
|**smoothness**               | `float`      | 晕影边框的平滑度。          | Yes   |
|**rounded**               | `bool`          | 启用后，小插图是完美的圆形。禁用时，晕影与当前纵横比的形状相匹配。        | Yes, Default: `0`  |