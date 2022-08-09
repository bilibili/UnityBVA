namespace BVA.EditorSettingsValidator
{
    public enum Messages
    {
        [LangMsg(Languages.en, "Color space")]
        [LangMsg(Languages.ja, "Color space")]
        ColorSpace,

        [LangMsg(Languages.en, " QualitySettings.renderPipeline  or GraphicsSettings.renderPipelineAsset is null")]
        [LangMsg(Languages.ja, " QualitySettings.renderPipeline  or GraphicsSettings.renderPipelineAsset is null")]
        PipeLineSetting,

        [LangMsg(Languages.en, "Gamma")]
        [LangMsg(Languages.ja, "Gamma")]
        ColorSpaceGamma,
        
        [LangMsg(Languages.en, "Linear")]
        [LangMsg(Languages.ja, "Linear")]
        ColorSpaceLinear,
        
        [LangMsg(Languages.en, "You can close this window.")]
        [LangMsg(Languages.ja, "You can close this window.")]
        YouCanCloseThisWindow,
        
        [LangMsg(Languages.en, "Thank you!")]
        [LangMsg(Languages.ja, "Thank you!")]
        ThankYou,
        
        [LangMsg(Languages.en, "Close")]
        [LangMsg(Languages.ja, "Close")]
        CloseWindowButton,
        
        [LangMsg(Languages.en, "Recommended project settings for BVA")]
        [LangMsg(Languages.ja, "Recommended project settings for BVA")]
        RecommendedProjectSettingsForBVA,
        
        [LangMsg(Languages.en, "current")]
        [LangMsg(Languages.ja, "current")]
        CurrentValue,
        
        [LangMsg(Languages.en, "Use recommended")]
        [LangMsg(Languages.ja, "Use recommended")]
        UseRecommended,
        
        [LangMsg(Languages.en, "Accept All")]
        [LangMsg(Languages.ja, "Accept All")]
        AcceptAllButton,
    }
}