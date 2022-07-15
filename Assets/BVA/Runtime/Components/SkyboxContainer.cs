using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BVA;

public class SkyboxContainer : MonoSingleton<SkyboxContainer>, IContainer<Material>
{
    public readonly static string[] COMPATIBLE_SHADERS = new string[] {"Skybox/6 Sided","Skybox/Cubemap" };
    public static bool IsValidMaterial(Material material) { return material != null && COMPATIBLE_SHADERS.Contains(material.shader.name); }
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
