# BVA_humanoid_avatarExtension

## 概览
这个扩展定义了 `Humanoid` 模型使用的 Avatar。

任何作为 Humanoid 动画类型导入的模型都将生成 Avatar 资源，其中存储驱动 Animator 的信息。

Avatar 系统是 Unity 如何识别特定的动画模型在布局上是人形的，以及模型的哪些部分对应于腿、手臂、头部和身体，在这一步之后，动画数据可以被“重用”。

由于不同人形角色之间骨骼结构的相似性，可以将动画从一个人形角色映射到另一个人形角色，从而实现重定位和反向运动学。

## 导出前提

在 Unity 中，模型必须具有 `Animator` 组件和有效的 `Avatar`。

### 骨骼定义

> `AvatarBone` 是一个枚举类型，它定义了不同的骨骼。

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

### Avatar定义

> 人体骨骼和肌肉设置

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

> 以下参数由 `BVA_humanoid_avatarExtension` 提供:

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**humanBones**               | `List<GlTFHumanoidBone>`                                                                        | 所有关节之间的连接和空间变换         | Yes   |
|**armStretch**               | `float`                                                                        | 使用IK时手臂允许伸展的长度         | Yes   |
|**legStretch**               | `float`                                                                        | 使用IK时，腿部允许伸展的长度        | Yes   |
|**upperArmTwist**               | `float`                                                                        | 定义上臂的旋转/扭转如何分布在肩膀和肘关节之间         | Yes   |
|**lowerArmTwist**               | `float`                                                                        | 定义下臂的旋转/扭转如何分布在肘关节和腕关节之间。          | Yes  |
|**upperLegTwist**              | `float`             | 定义了大腿的滚动/扭转是如何分布在大腿和膝关节之间的   | Yes                   |
|**lowerLegTwist**              | `float`             | 定义了小腿的滚动/扭转是如何分布在膝盖和脚踝之间  | Yes                   |
|**feetSpacing**              | `float`             | 修改到一个人形模型脚之间的最小距离   | Yes                   |
|**hasTranslationDoF**              | `bool`             | 对于任何有平移自由度(DoF)的人来说都是如此，默认为false  | Yes                   |

> `node` 指向层次结构中的`bone` 变换。

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

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**bone**               | `string`                                                                        | 关节的名称        | Yes   |
|**node**               | `number`                                                                        | 关节的NodeId       | Yes   |
|**useDefaultValues**               | `bool`                                                                        | 这个限制应该使用默认值吗？       | Yes, Default: `true`  |
|**min**               | `Vector3`                                                                        | 该肌肉可以应用的远离初始值的最大负旋转         | Yes   |
|**max**               | `Vector3`                                                                        | 该肌肉可以应用的远离初始值的最大旋转          | Yes  |
|**center**              | `Vector3`             | 未应用肌肉动作时骨骼的默认方向  | Yes                   |
|**axisLength**              | `float`             | 应用限制的骨骼长度  | Yes                   |
