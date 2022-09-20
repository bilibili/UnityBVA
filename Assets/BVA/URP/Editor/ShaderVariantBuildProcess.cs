using GLTF.Schema.BVA;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderVariantBuildProcess : IPreprocessShaders
{
    public int callbackOrder { get { return 0; } }

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        if ("VRM10/MToon10" == shader.name || "VRM/MToon" == shader.name || "Standard" == shader.name || shader.name == "Hidden/Shader Graph/FallbackError")
        {
            data.Clear();
        }
        if ("VRM/URP/MToon" == shader.name)
        {
            ShaderKeyword debugNormal = new ShaderKeyword(shader, "MTOON_DEBUG_NORMAL");
            ShaderKeyword debugLit = new ShaderKeyword(shader, "MTOON_DEBUG_LITSHADERATE");
            for (int i = data.Count - 1; i >= 0; --i)
            {
                if (data[i].shaderKeywordSet.IsEnabled(debugNormal) || data[i].shaderKeywordSet.IsEnabled(debugLit))
                {
                    data.RemoveAt(i);
                    continue;
                }
            }
        }
        if ("Shader Graphs/ZeldaToon" == shader.name)
        {
            ShaderKeyword sampleGI = new ShaderKeyword(shader, "_SAMPLE_GI");
            ShaderKeyword lightmapOn = new ShaderKeyword(shader, "LIGHTMAP_ON");
            for (int i = data.Count - 1; i >= 0; --i)
            {
                if (data[i].shaderKeywordSet.IsEnabled(sampleGI) || data[i].shaderKeywordSet.IsEnabled(lightmapOn))
                {
                    data.RemoveAt(i);
                    continue;
                }
            }
        }
        if (shader.name.StartsWith("Shader Graphs/Toon"))
        {
            ShaderKeyword lightmapOn = new ShaderKeyword(shader, "LIGHTMAP_ON");
            for (int i = data.Count - 1; i >= 0; --i)
            {
                if (data[i].shaderKeywordSet.IsEnabled(lightmapOn))
                {
                    data.RemoveAt(i);
                    continue;
                }
            }
        }

        if (shader.name == "Universal Render Pipeline/Toon")
        {
            ShaderKeyword shadingGradeMap = new ShaderKeyword(shader, "_SHADINGGRADEMAP");
            // used in ShadingGradeMap
            List<ShaderKeyword> usedInShadingGradeMap = new List<ShaderKeyword>(){
                new ShaderKeyword(shader, "_IS_TRANSCLIPPING_OFF"),
             new ShaderKeyword(shader, "_IS_TRANSCLIPPING_ON")};
            // used in DoubleShadeWithFeather
            List<ShaderKeyword> usedInDoubleShadeWithFeather = new List<ShaderKeyword>(){
                new ShaderKeyword(shader, "_IS_CLIPPING_OFF"),
             new ShaderKeyword(shader, "_IS_CLIPPING_MODE"),
             new ShaderKeyword(shader, "_IS_CLIPPING_TRANSMODE") };

            for (int i = data.Count - 1; i >= 0; --i)
            {
                if (data[i].shaderKeywordSet.IsEnabled(shadingGradeMap))
                {
                    if (data[i].shaderKeywordSet.IsEnabled(usedInDoubleShadeWithFeather[1])|| data[i].shaderKeywordSet.IsEnabled(usedInDoubleShadeWithFeather[2]))
                    {
                        data.RemoveAt(i);
                        continue;
                    }
                }
                else
                {
                    if (data[i].shaderKeywordSet.IsEnabled(usedInShadingGradeMap[1]))
                    {
                        data.RemoveAt(i);
                        continue;
                    }
                }
            }
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("shader={3}, passType={0}, passName={1}, shaderType={2}\n",
            snippet.passType, snippet.passName, snippet.shaderType, shader.name);

        for (int i = 0; i < data.Count; ++i)
        {
            var pdata = data[i];
            sb.AppendFormat("{0}.{1},{2}: ", i, pdata.graphicsTier, pdata.shaderCompilerPlatform);
            var ks = pdata.shaderKeywordSet.GetShaderKeywords();
            foreach (var k in ks)
            {
                sb.AppendFormat("{0}, ", k.name);
            }
            sb.Append("\n");
        }
        Debug.Log(sb.ToString());
    }
}
