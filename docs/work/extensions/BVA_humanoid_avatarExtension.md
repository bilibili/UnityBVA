# BVA_humanoid_avatarExtension

## Overview

This extension defines the Avatar that used by `Humanoid` Model.

Any model imported as Humanoid animation type will generate an Avatar asset in which store the infomation that drive the Animator.

The Avatar system is how Unity identifies that a particular animated model is humanoid in layout, and which parts of the model correspond to the legs, arms, head and body, after this step, animation data can be `reused`.

Because of the similarity in bone structure between different humanoid characters, it is possible to map animations from one humanoid character to another, allowing retargeting and inverse kinematics.

## Export prerequisites 

In Unity, model must has `Animator` component with a valid `Avatar`.

### Bone Defines

> `AvatarBone` is a enum Type which defines the different bones.

```csharp
    public enum AvatarBone
    {
        hips,
        leftUpperLeg,
        rightUpperLeg,
        leftLowerLeg,
        rightLowerLeg,
        leftFoot,
        rightFoot,
        spine,
        chest,
        neck,
        head,
        leftShoulder,
        rightShoulder,
        leftUpperArm,
        rightUpperArm,
        leftLowerArm,
        rightLowerArm,
        leftHand,
        rightHand,
        leftToes,
        rightToes,
        leftEye,
        rightEye,
        jaw,
        leftThumbProximal,
        leftThumbIntermediate,
        leftThumbDistal,
        leftIndexProximal,
        leftIndexIntermediate,
        leftIndexDistal,
        leftMiddleProximal,
        leftMiddleIntermediate,
        leftMiddleDistal,
        leftRingProximal,
        leftRingIntermediate,
        leftRingDistal,
        leftLittleProximal,
        leftLittleIntermediate,
        leftLittleDistal,
        rightThumbProximal,
        rightThumbIntermediate,
        rightThumbDistal,
        rightIndexProximal,
        rightIndexIntermediate,
        rightIndexDistal,
        rightMiddleProximal,
        rightMiddleIntermediate,
        rightMiddleDistal,
        rightRingProximal,
        rightRingIntermediate,
        rightRingDistal,
        rightLittleProximal,
        rightLittleIntermediate,
        rightLittleDistal,
        upperChest,

        unknown,
    }
```

### Avatar Defines
> humanBones and muscles setting

```csharp
public class GltfAvatar
{
    public List<GlTFHumanoidBone> humanBones = new List<GlTFHumanoidBone>();

    public float armStretch = 0.05f;

    public float legStretch = 0.05f;

    public float upperArmTwist = 0.5f;

    public float lowerArmTwist = 0.5f;

    public float upperLegTwist = 0.5f;

    public float lowerLegTwist = 0.5f;

    public float feetSpacing = 0;

    public bool hasTranslationDoF = false;
}
```
> The following parameters are contributed by the `BVA_humanoid_avatarExtension`:

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**humanBones**               | `List<GlTFHumanoidBone>`                                                                        | The connection between all joints and transform.         | Yes   |
|**armStretch**               | `float`                                                                        | Amount by which the arm's length is allowed to stretch when using IK.         | Yes   |
|**legStretch**               | `float`                                                                        | Amount by which the leg's length is allowed to stretch when using IK.        | Yes   |
|**upperArmTwist**               | `float`                                                                        | Defines how the upper arm's roll/twisting is distributed between the shoulder and elbow joints.         | Yes   |
|**lowerArmTwist**               | `float`                                                                        | Defines how the lower arm's roll/twisting is distributed between the elbow and wrist joints.          | Yes  |
|**upperLegTwist**              | `float`             | Defines how the upper leg's roll/twisting is distributed between the thigh and knee joints.   | Yes                   |
|**lowerLegTwist**              | `float`             | Defines how the lower leg's roll/twisting is distributed between the knee and ankle.  | Yes                   |
|**feetSpacing**              | `float`             | Modification to the minimum distance between the feet of a humanoid model.   | Yes                   |
|**hasTranslationDoF**              | `bool`             | True for any human that has a translation Degree of Freedom (DoF). It is set to false by default.  | Yes                   |

> `node` point to a `bone` transform in hierarchy.

```csharp
public class GlTFHumanoidBone : GLTFProperty
{
    public string bone;

    public int node;

    public bool useDefaultValues;

    public Vector3 min;

    public Vector3 max;

    public Vector3 center;

    public float axisLength;
}
```

|                                  | Type                                                                            | Description                            | Required             |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**bone**               | `List<GlTFHumanoidBone>`                                                                        | The connection between all joints and transform.         | Yes   |
|**node**               | `number`                                                                        | Amount by which the arm's length is allowed to stretch when using IK.         | Yes   |
|**useDefaultValues**               | `bool`                                                                        | Should this limit use the default values?        | Yes, Default: `true`  |
|**min**               | `Vector3`                                                                        | The maximum negative rotation away from the initial value that this muscle can apply.         | Yes   |
|**max**               | `Vector3`                                                                        | The maximum rotation away from the initial value that this muscle can apply.          | Yes  |
|**center**              | `Vector3`             | The default orientation of a bone when no muscle action is applied.   | Yes                   |
|**axisLength**              | `float`             | Length of the bone to which the limit is applied.  | Yes                   |
