using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BVA;

public class SkyboxContainer : MonoSingleton<SkyboxContainer>, IContainer<Material>
{
    public readonly static string[] COMPATIBLE_SHADERS = new string[] {"Skybox/6 Sided","Skybox/Cubemap", "Skybox/Panoramic", "Skybox/Procedural" };
    public static bool IsValidSkyboxMaterial(Material material) 
    { 
        bool isValid = material != null && COMPATIBLE_SHADERS.Contains(material.shader.name);
        if (!isValid)
            LogPool.ExportLogger.LogWarning(LogPart.Material, $"The material {material.name} is not a valid Skybox material, you are supposed to use these skybox shader [{COMPATIBLE_SHADERS[0]},{COMPATIBLE_SHADERS[1]},{COMPATIBLE_SHADERS[2]},{COMPATIBLE_SHADERS[3]}]");
        return isValid;
    }
    public List<Material> materials;
    public Material Get(int index)
    {
        if (materials == null) return null;
        if (materials.Count <= index) return null;
        return materials[index];
    }
    public void Add(Material material)
    {
        if (materials == null) materials = new List<Material>();
        if (materials.Contains(material)) return;
        materials.Add(material);
    }
    public bool Has(Material obj)
    {
        return materials.Contains(obj);
    }

    public bool Remove(Material obj)
    {
        return materials.Remove(obj);
    }

    public void RemoveAt(int index)
    {
        materials.RemoveAt(index);
    }

    public void RemoveInvalidOrNull()
    {
        materials.RemoveAll(x => x == null|| !COMPATIBLE_SHADERS.Contains(x.shader.name));
    }
}
