using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
public class HumanoidTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void HumanTraits()
    {
        string[] muscleName = HumanTrait.MuscleName;
        for (int i = 0; i < HumanTrait.BoneCount; ++i)
        {
            Debug.Log(muscleName[i]);
        }
    }
    // A Test behaves as an ordinary method
    [Test]
    public void HumanMuscleProperty()
    {
        var g = Selection.gameObjects;
        if (g.Length < 1) return;
        var animator = g[0].GetComponent<Animator>();
        if (animator != null)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            AnimationClip[] animationClips = ac.animationClips;
            foreach (var clip in animationClips)
            {
                foreach (var binding in UnityEditor.AnimationUtility.GetCurveBindings(clip))
                {
                    Debug.Log($"\"{binding.propertyName}\"");
                }
            }
        }
    }
}
