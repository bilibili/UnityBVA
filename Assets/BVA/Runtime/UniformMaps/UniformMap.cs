using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BVA
{
    public interface IUniformMap
    {
        Material Material { get; }

        Texture NormalTexture { get; set; }
        int NormalTexCoord { get; set; }
        float NormalTexScale { get; set; }
        Vector2 NormalXOffset { get; set; }
        float NormalXRotation { get; set; }
        Vector2 NormalXScale { get; set; }
        int NormalXTexCoord { get; set; }

        Texture OcclusionTexture { get; set; }
        int OcclusionTexCoord { get; set; }
        float OcclusionTexStrength { get; set; }
        Vector2 OcclusionXOffset { get; set; }
        float OcclusionXRotation { get; set; }
        Vector2 OcclusionXScale { get; set; }
        int OcclusionXTexCoord { get; set; }

        Texture EmissiveTexture { get; set; }
        int EmissiveTexCoord { get; set; }
        Color EmissiveFactor { get; set; }
        Vector2 EmissiveXOffset { get; set; }
        float EmissiveXRotation { get; set; }
        Vector2 EmissiveXScale { get; set; }
        int EmissiveXTexCoord { get; set; }

        float ClearcoatFactor { get; set; }
        Texture ClearcoatTexture { get; set; }
        float ClearcoatRoughnessFactor { get; set; }
        Texture ClearcoatRoughnessTexture { get; set; }


        GLTF.Schema.AlphaMode AlphaMode { get; set; }
        GLTF.Schema.BlendMode BlendMode { get; set; }
        float AlphaCutoff { get; set; }
        bool DoubleSided { get; set; }
        bool VertexColorsEnabled { get; set; }
        bool EnableInstance { get; set; }
        IUniformMap Clone();
    }

    public interface IMetalRoughUniformMap : IUniformMap
    {
        Texture BaseColorTexture { get; set; }
        int BaseColorTexCoord { get; set; }
        Vector2 BaseColorXOffset { get; set; }
        float BaseColorXRotation { get; set; }
        Vector2 BaseColorXScale { get; set; }
        int BaseColorXTexCoord { get; set; }

        Color BaseColorFactor { get; set; }

        Texture MetallicRoughnessTexture { get; set; }
        int MetallicRoughnessTexCoord { get; set; }
        Vector2 MetallicRoughnessXOffset { get; set; }
        float MetallicRoughnessXRotation { get; set; }
        Vector2 MetallicRoughnessXScale { get; set; }
        int MetallicRoughnessXTexCoord { get; set; }

        float MetallicFactor { get; set; }
        float RoughnessFactor { get; set; }
    }

    public interface ISpecGlossUniformMap : IUniformMap
    {
        Texture DiffuseTexture { get; set; }
        int DiffuseTexCoord { get; set; }
        Vector2 DiffuseXOffset { get; set; }
        float DiffuseXRotation { get; set; }
        Vector2 DiffuseXScale { get; set; }
        int DiffuseXTexCoord { get; set; }

        Color DiffuseFactor { get; set; }

        Texture SpecularGlossinessTexture { get; set; }
        int SpecularGlossinessTexCoord { get; set; }
        Vector2 SpecularGlossinessXOffset { get; set; }
        float SpecularGlossinessXRotation { get; set; }
        Vector2 SpecularGlossinessXScale { get; set; }
        int SpecularGlossinessXTexCoord { get; set; }

        Vector3 SpecularFactor { get; set; }
        float GlossinessFactor { get; set; }
    }

    public interface IUnlitUniformMap : IUniformMap
    {
        Texture BaseColorTexture { get; set; }
        int BaseColorTexCoord { get; set; }
        Vector2 BaseColorXOffset { get; set; }
        float BaseColorXRotation { get; set; }
        Vector2 BaseColorXScale { get; set; }
        int BaseColorXTexCoord { get; set; }

        Color BaseColorFactor { get; set; }
    }
}
