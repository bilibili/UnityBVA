using BVA;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public interface IShaderLoader
{
    Shader Find(string name);
    string GetVersion(string name);
}
public class BuildinShaerLoader : IShaderLoader
{
    public Shader Find(string name)
    {
        return Shader.Find(name);
    }

    public string GetVersion(string name)
    {
        return null;
    }
}
public class AssetBundleShaderLoader : IShaderLoader
{
    const string NO_VERSION = "0.0.0";
    public struct ShaderInfo
    {
        public string name;
        public string version;
        public Shader shader;
    }

    public RuntimePlatform RuntimePlatform { get; private set; }
    private readonly Dictionary<string, ShaderInfo> _shaderMap;

    public AssetBundleShaderLoader(RuntimePlatform platform)
    {
        _shaderMap = new Dictionary<string, ShaderInfo>();
        RuntimePlatform = platform;
    }

    public void Load(string assetBundleFile, string version = NO_VERSION)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(assetBundleFile);
        Shader[] shaders = ab.LoadAllAssets<Shader>();
        if (shaders.Length > 0)
        {
            foreach (Shader shader in shaders)
            {
                _shaderMap.Add(shader.name, new ShaderInfo() { name = shader.name, version = version, shader = shader });
                LogPool.ImportLogger.Log(LogPart.Shader, $"Load shader \"{shader.name}\" from AssetBundle {assetBundleFile}");
            }
        }
        else
        {
            LogPool.ImportLogger.LogWarning(LogPart.Shader, $"No shaders were founded in this AssetBundle file {assetBundleFile}");
        }
    }
    public void LoadFiles(string folder, string version = NO_VERSION)
    {
        DirectoryInfo di = new DirectoryInfo(folder);
        FileInfo[] files = di.GetFiles();
        foreach (FileInfo fi in files)
        {
            if (fi.Extension == ".assetbundle")
            {
                Load(fi.FullName, version);
            }
        }
    }

    public Shader Find(string name)
    {
        if (_shaderMap.TryGetValue(name, out ShaderInfo outShader))
        {
            return outShader.shader;
        }
        return Shader.Find(name);
    }

    public string GetVersion(string name)
    {
        if (_shaderMap.TryGetValue(name, out ShaderInfo outShader))
        {
            return outShader.version;
        }
        return NO_VERSION;
    }
}
