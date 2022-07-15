using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public static class ShaderVariantExplorerInternal
{
    public static string[] GetShaderGlobalKeywords(Shader s) => ShaderUtil.GetShaderGlobalKeywords(s).OrderBy(x => x).ToArray();
    public static string[] GetShaderLocalKeywords(Shader s) => ShaderUtil.GetShaderLocalKeywords(s).OrderBy(x => x).ToArray();

#if UNITY_2021_2_OR_NEWER
    public static ShaderData.PreprocessedVariant PreprocessShaderVariant(
        Shader shader,
        int subShaderIndex,
        int passId,
        ShaderType shaderType,
        BuiltinShaderDefine[] platformKeywords,
        string[] keywords,
        ShaderCompilerPlatform shaderCompilerPlatform,
        BuildTarget buildTarget,
        GraphicsTier tier,
        bool stripLineDirectives)
    {
        // platformKeywords = ShaderUtil.GetShaderPlatformKeywordsForBuildTarget()
        return ShaderUtil.PreprocessShaderVariant(shader, subShaderIndex, passId, shaderType, platformKeywords, keywords, shaderCompilerPlatform, buildTarget, tier, stripLineDirectives);
    }

    private static FieldInfo m_SubShaderIndex;
    private static FieldInfo m_PassIndex;
    public static LocalKeyword[] GetPassKeywords(Shader shader, int subShaderIndex, int passId)
    {
        var pi = GetPassIdentifier(subShaderIndex, passId);
        var keywords = ShaderUtil.GetPassKeywords(shader, pi);
        return keywords;
    }

    public static PassIdentifier GetPassIdentifier(int subShaderIndex, int passId)
    {
        if(m_SubShaderIndex == null) m_SubShaderIndex = typeof(PassIdentifier).GetField("m_SubShaderIndex", (BindingFlags)(-1));
        if (m_PassIndex == null) m_PassIndex = typeof(PassIdentifier).GetField("m_PassIndex", (BindingFlags)(-1));
        
        // Mock Pass Identifier
        var pi = new PassIdentifier();
        var obj = (object) pi;
        m_SubShaderIndex?.SetValue(obj, (uint)subShaderIndex);
        m_PassIndex?.SetValue(obj, (uint)passId);
        pi = (PassIdentifier) obj;

        return pi;
    }
#endif
}
