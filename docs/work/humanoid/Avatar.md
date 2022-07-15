# Avatar

## Overview

The avatar which we usually know is that when Unity imports Model files that contain `Humanoid Rigs`, it reconcile the bone structure of the Model by mapping each bone in the file to a Humanoid Avatar and then pass it value to the `Animator` Component so that it can play any `Humanoid Animation` properly.

## Export
While avatar also can be construct at runtime, it enable us to export it to external file.
Basiclly, it records all the bones info which build up human rig.

it contains all Avatar bones transform node index, and the muscle properties.

Before export it, make sure the model is at the state of T-Pose, otherwise the animation will look abnormal when it plays.

```csharp
public List<GlTFHumanoidBone> humanBones = new List<GlTFHumanoidBone>();

public float armStretch = 0.05f;

public float legStretch = 0.05f;

public float upperArmTwist = 0.5f;

public float lowerArmTwist = 0.5f;

public float upperLegTwist = 0.5f;

public float lowerLegTwist = 0.5f;

public float feetSpacing = 0;

public bool hasTranslationDoF = false;
```

## Import
Unity build avatar by calling 

```csharp
AvatarBuilder.BuildHumanAvatar(root.gameObject, humanDescription);
```

if everything goes fine, it will return a Avatar instance.


When saved it to Editor, write it as usual Unity Asset Object which has the extension `.asset`