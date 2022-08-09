using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// static class for config the BVA project
/// </summary>
public static class BVAConfig
{
    public const string URP_RenderPipelineAsset_Path = "Assets/BVA/URP/BVAUniversalRenderPipelineAsset.asset";
    public static bool HasValidRenderPipelineAsset()
    {
        return QualitySettings.renderPipeline != null;
    }
    public static void LoadDefaultRenderPipelineAsset()
    {
        QualitySettings.renderPipeline = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(URP_RenderPipelineAsset_Path);
    }

    public static ColorSpace ColorSpace() => PlayerSettings.colorSpace;
    public static string[] CheckProjectEnvironment()
    {
        List<string> infos = new List<string>();
        if (!HasValidRenderPipelineAsset())
            infos.Add("no valid render pipeline asset founded! please assign the RenderPipelineAsset in QualitySettings and Graphics");
        if (ColorSpace() != UnityEngine.ColorSpace.Linear)
            infos.Add("ColorSpace has to be linear, please switch colorspace in PlayerSetting Panel");
        return infos.ToArray();
    }
    public static void FixProjectEnvironment()
    {
        if (!HasValidRenderPipelineAsset())
            LoadDefaultRenderPipelineAsset();
        Lightmapping.bakedGI = true;
        Lightmapping.realtimeGI = false;
        PlayerSettings.SetNormalMapEncoding(BuildTargetGroup.Standalone, NormalMapEncoding.XYZ);
        PlayerSettings.SetNormalMapEncoding(BuildTargetGroup.Android, NormalMapEncoding.XYZ);
        PlayerSettings.SetNormalMapEncoding(BuildTargetGroup.iOS, NormalMapEncoding.XYZ);
        if (ColorSpace() != UnityEngine.ColorSpace.Linear)
            PlayerSettings.colorSpace = UnityEngine.ColorSpace.Linear;
        PlayerSettings.assemblyVersionValidation = false;
        PlayerSettings.allowUnsafeCode = true;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Standalone, ManagedStrippingLevel.Disabled);
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Disabled);
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Disabled);
    }
}
