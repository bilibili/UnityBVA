using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;
using System;
using System.Collections.Generic;

namespace BVA.EditorSettingsValidator
{
    public sealed class UnityColorSpaceSettingsValidator : IUnitySettingsValidator
    {
        public bool IsValid => PlayerSettings.colorSpace == UnityEngine.ColorSpace.Linear;
        public string HeaderDescription => Messages.ColorSpace.Msg();
        public string CurrentValueDescription => PlayerSettings.colorSpace == UnityEngine.ColorSpace.Linear
            ? Messages.ColorSpaceLinear.Msg() : Messages.ColorSpaceGamma.Msg();
        public string RecommendedValueDescription => Messages.ColorSpaceLinear.Msg();
        
        public void Validate()
        {
            PlayerSettings.colorSpace = UnityEngine.ColorSpace.Linear;
        }
    }

    public sealed class UnityURPSettingsValidator : IUnitySettingsValidator
    {
        public bool IsValid => QualitySettings.renderPipeline != null && GraphicsSettings.renderPipelineAsset != null;
        public string HeaderDescription => Messages.PipeLineSetting.Msg();
        public string CurrentValueDescription => "";
        public string RecommendedValueDescription => "Fix it";

        public void Validate()
        {
            UniversalRenderPipelineAsset pipeLineAsset = AssetDatabase.LoadAssetAtPath("Packages/com.bilibili.bva/BVA/URP/BVAUniversalRenderPipelineAsset.asset", typeof(UniversalRenderPipelineAsset)) as UniversalRenderPipelineAsset;
            if (pipeLineAsset == null)
            {
                pipeLineAsset = AssetDatabase.LoadAssetAtPath("Assets/BVA/URP/BVAUniversalRenderPipelineAsset.asset", typeof(UniversalRenderPipelineAsset)) as UniversalRenderPipelineAsset;
            }
            Debug.Assert(pipeLineAsset != null);

            QualitySettings.renderPipeline = GraphicsSettings.renderPipelineAsset = pipeLineAsset;
        }
    }

    public sealed class UnityCSCValidator : IUnitySettingsValidator
    {
        string[] importText = new string[] { "-r:System.IO.Compression.dll", "-r:System.IO.Compression.FileSystem.dll" };
        private List<string> needWrite;

        public bool IsValid => IsVaildFunc();
        public string HeaderDescription => Messages.PipeLineSetting.Msg();
        public string CurrentValueDescription => "";
        public string RecommendedValueDescription => "Fix it";

        public bool IsVaildFunc()
        {
            needWrite = new List<string>();
            if (File.Exists("Assets/csc.rsp"))
            {
                string[] context = File.ReadAllLines("Assets/csc.rsp");
                needWrite.AddRange(context);
                for (int i = 0; i < importText.Length; i++)
                {
                    if (!needWrite.Contains(importText[i]))
                    {
                        needWrite.Add(importText[i]);
                    }
                }
                return needWrite.Count == context.Length;
            }
            return false;
        }
        public void Validate()
        {
            File.WriteAllLines("Assets/csc.rsp", needWrite);
        }
    }
}
