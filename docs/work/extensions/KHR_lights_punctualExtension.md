# KHR_lights_punctualExtension

## Overview

Official Extension support Light Component.

Only `Realtime` or `Mixed` Mode are support export, `Baked` light is not necessary to export.

Supported Type : `Directional`, `Point`, `Spot`. Area Light is baked only.

All light types share the common set of properties listed below.

### Light Shared Properties

| Property | Description | Required |
|:-----------------------|:------------------------------------------| :--------------------------|
| `name` | Name of the light. | No, Default: `""` |
| `color` | RGB value for light's color in linear space. | No, Default: `[1.0, 1.0, 1.0]` |
| `intensity` | Brightness of light in. The units that this is defined in depend on the type of light. `point` and `spot` lights use luminous intensity in candela (lm/sr) while `directional` lights use illuminance in lux (lm/m<sup>2</sup>) | No, Default: `1.0` |
| `type` | Declares the type of the light. | :white_check_mark: Yes |
| `range` | Hint defining a distance cutoff at which the light's intensity may be considered to have reached zero. Supported only for `point` and `spot` lights. Must be > 0. When undefined, range is assumed to be infinite. | No |


When a light's `type` is `spot`, the `spot` property on the light is required. Its properties (below) are optional.

| Property | Description | Required |
|:-----------------------|:------------------------------------------| :--------------------------|
| `innerConeAngle` | Angle, in radians, from centre of spotlight where falloff begins. Must be greater than or equal to `0` and less than `outerConeAngle`. | No, Default: `0` |
| `outerConeAngle` | Angle, in radians, from centre of spotlight where falloff ends.  Must be greater than `innerConeAngle` and less than or equal to `PI / 2.0`. | No, Default: `PI / 4.0` |


A typycial extension like this:

```javascript
"extensions": {
    "KHR_lights_punctual" : {
        "lights": [
            {
                "spot": {
                    "innerConeAngle": 0.785398163397448,
                    "outerConeAngle": 1.57079632679,
                },
                "color": [
                    1.0,
                    1.0,
                    1.0
                ],
                "type": "spot"
            }
        ]
    }
}
```