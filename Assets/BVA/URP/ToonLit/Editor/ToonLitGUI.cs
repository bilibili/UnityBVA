using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
namespace ToonLit
{
    public static class ToonLitGUI
    {


        public enum WorkflowMode
        {
            Specular = 0,
            Metallic
        }

        public enum ToonSpecMode
        {
            Common = 0,
            Hair = 1
        }

        public enum RoughnessMapChannel
        {
            SpecularMetallicAlpha,
            AlbedoAlpha,
        }

        public static class Styles
        {
            public static GUIContent workflowModeText = new GUIContent("Workflow Mode (PBR)",
                "Select a workflow that fits your textures. Choose between Metallic or Specular.");

            public static GUIContent toonLightDivid_M =
                new GUIContent("Toon Light Divid M", "Divid the Light side and the dark side.");
            public static GUIContent toonLightDivid_D =
                new GUIContent("Toon Light Divid D", "Divid the Light side and the dark side.");
            public static GUIContent toonDiffuseBright =
               new GUIContent("Toon Diffuse Brightness", "The brightness of toon.");
            public static GUIContent toonLightSharp =
                new GUIContent("Toon Light Sharp", "The sharpness of the light-dark-dividing.");
            public static GUIContent toonDarkFaceColor =
                new GUIContent("Toon Dark Color", "Dark Color");
            public static GUIContent toonDeepDarkColor =
               new GUIContent("Toon Deep Dark Color", "Deep Dark Color");

            public static GUIContent _SSSColor =
               new GUIContent("SSS Color", "Subsurface Scattering Color");
            public static GUIContent _SSSWeight =
               new GUIContent("SSS Weight", "Weight of SSS");
            public static GUIContent _SSSSize =
               new GUIContent("SSS Size", "Size of SSS");
            public static GUIContent _SSForwardAtt =
               new GUIContent("SSS ForwardAtt", "Atten of SS in forward Dir");

            public static GUIContent _ClearCoatMask =
              new GUIContent("Clear Coat Mask", "_ClearCoatMaskMap\n[R]: Range\n[G]: Gloss\n[B]: Mult");
            public static GUIContent _ClearCoatColor =
               new GUIContent("Clear Coat Color", "_ClearCoatColor");
            public static GUIContent _ClearCoatRange =
              new GUIContent("Clear Coat Range", "_ClearCoatRange");
            public static GUIContent _ClearCoatGloss =
              new GUIContent("Clear Coat Gloss", "_ClearCoatGloss");
            public static GUIContent _ClearCoatMult =
              new GUIContent("Clear Coat Mult", "_ClearCoatMult");

            public static GUIContent _SpecularColorMask =
              new GUIContent("Specular Color Mask", "_SpecularColorMaskMap\n[R]: Range\n[G]: Gloss\n[B]: Mult");
            public static GUIContent _SpecularColor =
               new GUIContent("Specular Color", "_SpecularColor");
            public static GUIContent _SpecularRange =
              new GUIContent("Specular Range", "_SpecularRange");
            public static GUIContent _SpecularMulti =
              new GUIContent("Specular Multi", "_SpecularMulti");
            public static GUIContent _SpecularGloss =
              new GUIContent("Specular Gloss", "_SpecularGloss");

            public static GUIContent useToonSpec =
                new GUIContent("Toon Spec", "Want to use toon spec?");
            public static GUIContent toonSpecMap =
                new GUIContent("Spec Color", "[RGB]: Spec Color.\n[A]: 2nd's tangent-shift.");
            public static GUIContent toonSpecOptMap =
                new GUIContent("Spec Opt Map", "Specular Color Option.\n[R]: Noise.\n[G]: Noise Mask.\n[B]: Feather.\nRight: OptMap's ST");
            public static GUIContent toonSpecMode =
                new GUIContent("Toon Spec Mode", "Select a toon-spec-mode.");

            public static GUIContent toonSpecGloss =
                new GUIContent("Spec Gloss", "ToonSpecGloss.");
            public static GUIContent toonSpecFeatherLevel =
                new GUIContent("Spec Feather Level", "ToonSpecFeatherLevel.");
            public static GUIContent toonSpecMaskScale =
                new GUIContent("Spec Mask Scale", "ToonSpecMaskScale.");
            public static GUIContent toonSpecMaskOffset =
                new GUIContent("Spec Mask Offset", "ToonSpecMaskOffset.");
            public static GUIContent toonSpecAnisoHighLightPower_1st =
                new GUIContent("Aniso Power 1st", "ToonSpecAnisoHighLightPower 1st.");
            public static GUIContent toonSpecAnisoHighLightPower_2nd =
                new GUIContent("Aniso Power 2nd", "ToonSpecAnisoHighLightPower 2nd.");
            public static GUIContent toonSpecAnisoHighLightStrength_1st =
                new GUIContent("Aniso Strength 1st", "ToonSpecAnisoHighLightStrength 1st.");
            public static GUIContent toonSpecAnisoHighLightStrength_2nd =
                new GUIContent("Aniso Strength 2nd", "ToonSpecAnisoHighLightStrength 2nd.");
            public static GUIContent toonSpecShiftTangent_1st =
               new GUIContent("Shift Tangent 1st", "ToonSpecShiftTangent 1st.");
            public static GUIContent toonSpecShiftTangent_2nd =
               new GUIContent("Shift Tangent 2nd", "ToonSpecShiftTangent 2nd.");

            public static GUIContent specularMapText =
                new GUIContent("Specular Map", "Sets and configures the map and color for the Specular workflow. USE the [rgb] Channel.");

            public static GUIContent metallicMapText =
                new GUIContent("Metallic Map", "Sets and configures the map for the Metallic workflow. USE the [r] Channel.");

            public static GUIContent roughnessText = new GUIContent("Roughness",
                "Controls the spread of highlights and reflections on the surface.");

            public static GUIContent roughnessMapChannelText =
                new GUIContent("Source",
                    "Specifies where to sample a Roughness map from. By default, uses the alpha channel for your map.");

            public static GUIContent occlusionText = new GUIContent("Occlusion Map",
                "Sets an occlusion map to simulate shadowing from ambient lighting.  USE the [g] Channel !");

            public static readonly string[] metallicRoughnessChannelNames = { "Metallic Alpha", "Albedo Alpha" };
            public static readonly string[] specularRoughnessChannelNames = { "Specular Alpha", "Albedo Alpha" };
        }


        public struct LitProperties
        {
            // Surface Option Props
            public MaterialProperty workflowMode;

            // Surface Input Props
            public MaterialProperty toonLightDivid_M;
            public MaterialProperty toonLightDivid_D;
            public MaterialProperty toonDiffuseBright;
            public MaterialProperty toonLightSharp;
            public MaterialProperty toonDarkColor;
            public MaterialProperty toonDeepDarkColor;

            public MaterialProperty _SSSColor;
            public MaterialProperty _SSSWeight;
            public MaterialProperty _SSSSize;
            public MaterialProperty _SSForwardAtt;

            public MaterialProperty _ClearCoatMask;
            public MaterialProperty _ClearCoatColor;
            public MaterialProperty _ClearCoatRange;
            public MaterialProperty _ClearCoatGloss;
            public MaterialProperty _ClearCoatMult;

            public MaterialProperty _SpecularColorMask;
            public MaterialProperty _SpecularColor;
            public MaterialProperty _SpecularRange;
            public MaterialProperty _SpecularMulti;
            public MaterialProperty _SpecularGloss;

            public MaterialProperty useToonSpecProp;
            public MaterialProperty useToonHairSpecProp;

            public MaterialProperty toonSpecMapProp;
            public MaterialProperty toonSpecColorProp;
            public MaterialProperty toonSpecOptMapProp;
            public MaterialProperty toonSpecOptMapSTProp;

            public MaterialProperty toonSpecGlossProp;
            public MaterialProperty toonSpecFeatherLevelProp;
            public MaterialProperty toonSpecMaskScaleProp;
            public MaterialProperty toonSpecMaskOffsetProp;

            public MaterialProperty toonSpecAnisoHighLightPower_1st;
            public MaterialProperty toonSpecAnisoHighLightPower_2nd;
            public MaterialProperty toonSpecAnisoHighLightStrength_1st;
            public MaterialProperty toonSpecAnisoHighLightStrength_2nd;
            public MaterialProperty toonSpecShiftTangent_1st;
            public MaterialProperty toonSpecShiftTangent_2nd;

            public MaterialProperty metallic;
            public MaterialProperty specColor;
            public MaterialProperty metallicGlossMap;
            public MaterialProperty specGlossMap;

            public MaterialProperty roughnessMap;
            public MaterialProperty roughness;
            public MaterialProperty roughnessMapChannel;
            public MaterialProperty bumpMapProp;
            public MaterialProperty bumpScaleProp;
            public MaterialProperty occlusionStrength;
            public MaterialProperty occlusionMap;
            //public MaterialProperty useSSS;
            //public MaterialProperty diffusionProfileAsset;
            //public MaterialProperty diffusionProfileHash;
            //public MaterialProperty diffusionProfileIndex;
            //public MaterialProperty subsurfaceMask;
            //public MaterialProperty subsurfaceMaskMap;
            // Advanced Props
            public MaterialProperty highlights;
            public MaterialProperty reflections;

            public LitProperties(MaterialProperty[] properties)
            {

                // Surface Option Props
                workflowMode = BaseShaderGUI.FindProperty("_WorkflowMode", properties, false);
                // Surface Input Props
                toonLightDivid_M = BaseShaderGUI.FindProperty("_ToonLightDivid_M", properties);
                toonLightDivid_D = BaseShaderGUI.FindProperty("_ToonLightDivid_D", properties);
                toonDiffuseBright = BaseShaderGUI.FindProperty("_ToonDiffuseBright", properties);
                toonLightSharp = BaseShaderGUI.FindProperty("_BoundSharp", properties);
                toonDarkColor = BaseShaderGUI.FindProperty("_DarkFaceColor", properties);
                toonDeepDarkColor = BaseShaderGUI.FindProperty("_DeepDarkColor", properties);

                _SSSColor = BaseShaderGUI.FindProperty("_SSSColor", properties);
                _SSSWeight = BaseShaderGUI.FindProperty("_SSSWeight", properties);
                _SSSSize = BaseShaderGUI.FindProperty("_SSSSize", properties);
                _SSForwardAtt = BaseShaderGUI.FindProperty("_SSForwardAtt", properties);

                _ClearCoatMask = BaseShaderGUI.FindProperty("_ClearCoatMaskMap", properties);
                _ClearCoatColor = BaseShaderGUI.FindProperty("_ClearCoatColor", properties);
                _ClearCoatRange = BaseShaderGUI.FindProperty("_ClearCoatRange", properties);
                _ClearCoatGloss = BaseShaderGUI.FindProperty("_ClearCoatGloss", properties);
                _ClearCoatMult = BaseShaderGUI.FindProperty("_ClearCoatMult", properties);

                _SpecularColorMask = BaseShaderGUI.FindProperty("_SpecularMaskMap", properties);
                _SpecularColor = BaseShaderGUI.FindProperty("_SpecularColor", properties);
                _SpecularRange = BaseShaderGUI.FindProperty("_SpecularRange", properties);
                _SpecularMulti = BaseShaderGUI.FindProperty("_SpecularMulti", properties);
                _SpecularGloss = BaseShaderGUI.FindProperty("_SpecularGloss", properties);

                useToonSpecProp = BaseShaderGUI.FindProperty("_UseToonSpec", properties);
                useToonHairSpecProp = BaseShaderGUI.FindProperty("_UseToonHairSpec", properties);

                toonSpecMapProp = BaseShaderGUI.FindProperty("_ToonSpecMap", properties);
                toonSpecColorProp = BaseShaderGUI.FindProperty("_ToonSpecColor", properties);
                toonSpecOptMapProp = BaseShaderGUI.FindProperty("_ToonSpecOptMap", properties);
                toonSpecOptMapSTProp = BaseShaderGUI.FindProperty("_ToonSpecOptMapST", properties);

                toonSpecGlossProp = BaseShaderGUI.FindProperty("_ToonSpecGloss", properties);
                toonSpecFeatherLevelProp = BaseShaderGUI.FindProperty("_ToonSpecFeatherLevel", properties);
                toonSpecMaskScaleProp = BaseShaderGUI.FindProperty("_ToonSpecMaskScale", properties);
                toonSpecMaskOffsetProp = BaseShaderGUI.FindProperty("_ToonSpecMaskOffset", properties);

                toonSpecAnisoHighLightPower_1st = BaseShaderGUI.FindProperty("_ToonSpecAnisoHighLightPower_1st", properties);
                toonSpecAnisoHighLightPower_2nd = BaseShaderGUI.FindProperty("_ToonSpecAnisoHighLightPower_2nd", properties);
                toonSpecAnisoHighLightStrength_1st = BaseShaderGUI.FindProperty("_ToonSpecAnisoHighLightStrength_1st", properties);
                toonSpecAnisoHighLightStrength_2nd = BaseShaderGUI.FindProperty("_ToonSpecAnisoHighLightStrength_2nd", properties);
                toonSpecShiftTangent_1st = BaseShaderGUI.FindProperty("_ToonSpecShiftTangent_1st", properties);
                toonSpecShiftTangent_2nd = BaseShaderGUI.FindProperty("_ToonSpecShiftTangent_2nd", properties);

                metallic = BaseShaderGUI.FindProperty("_Metallic", properties);
                specColor = BaseShaderGUI.FindProperty("_SpecColor", properties, false);
                metallicGlossMap = BaseShaderGUI.FindProperty("_MetallicGlossMap", properties);
                specGlossMap = BaseShaderGUI.FindProperty("_SpecGlossMap", properties, false);

                roughnessMap = BaseShaderGUI.FindProperty("_RoughnessMap", properties, false);
                roughness = BaseShaderGUI.FindProperty("_Roughness", properties, false);
                roughnessMapChannel = BaseShaderGUI.FindProperty("_RoughnessTextureChannel", properties, false);
                bumpMapProp = BaseShaderGUI.FindProperty("_BumpMap", properties, false);
                bumpScaleProp = BaseShaderGUI.FindProperty("_BumpScale", properties, false);

                occlusionStrength = BaseShaderGUI.FindProperty("_OcclusionStrength", properties, false);
                occlusionMap = BaseShaderGUI.FindProperty("_OcclusionMap", properties, false);
                /*
                //SSS
                useSSS = BaseShaderGUI.FindProperty("_useSSS", properties, false);
                diffusionProfileAsset = BaseShaderGUI.FindProperty("_DiffusionProfileAsset", properties, false);
                diffusionProfileHash = BaseShaderGUI.FindProperty("_DiffusionProfileHash", properties, false);
                diffusionProfileIndex = BaseShaderGUI.FindProperty("_DiffusionProfile", properties, false);
                subsurfaceMask = BaseShaderGUI.FindProperty("_SubsurfaceMask", properties, false);
                subsurfaceMaskMap = BaseShaderGUI.FindProperty("_SubsurfaceMaskMap", properties, false);*/
                // Advanced Props
                highlights = BaseShaderGUI.FindProperty("_SpecularHighlights", properties, false);
                reflections = BaseShaderGUI.FindProperty("_EnvironmentReflections", properties, false);
            }
        }



        public static void Inputs(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            DoToonBasicArea(properties, materialEditor, material);
            DoToonSpecArea(properties, materialEditor, material);
            EditorGUILayout.Space(20);

            DoMetallicSpecularArea(properties, materialEditor, material);
            ToonLitURPEditorBase.DrawNormalArea(materialEditor, properties.bumpMapProp, properties.bumpScaleProp);


            if (properties.occlusionMap != null)
            {
                materialEditor.TexturePropertySingleLine(Styles.occlusionText, properties.occlusionMap,
                    properties.occlusionMap.textureValue != null ? properties.occlusionStrength : null);
            }

        }

        public static void DoToonBasicArea(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            materialEditor.ShaderProperty(properties.toonLightDivid_M, Styles.toonLightDivid_M);
            materialEditor.ShaderProperty(properties.toonLightDivid_D, Styles.toonLightDivid_D);
            materialEditor.ShaderProperty(properties.toonDiffuseBright, Styles.toonDiffuseBright);
            materialEditor.ShaderProperty(properties.toonLightSharp, Styles.toonLightSharp);
            materialEditor.ShaderProperty(properties.toonDarkColor, Styles.toonDarkFaceColor);
            materialEditor.ShaderProperty(properties.toonDeepDarkColor, Styles.toonDeepDarkColor);

            materialEditor.ShaderProperty(properties._SSSColor, Styles._SSSColor);
            materialEditor.ShaderProperty(properties._SSSWeight, Styles._SSSWeight);
            materialEditor.ShaderProperty(properties._SSSSize, Styles._SSSSize);
            materialEditor.ShaderProperty(properties._SSForwardAtt, Styles._SSForwardAtt);
        }

        public static void DoToonSpecArea(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            materialEditor.ShaderProperty(properties.useToonSpecProp, Styles.useToonSpec);
            bool useToonSpec = properties.useToonSpecProp.floatValue >= 0.5f ? true : false;
            if (useToonSpec)
            {
                EditorGUI.BeginDisabledGroup(!useToonSpec);
                {
                    EditorGUI.indentLevel++;
                    materialEditor.TexturePropertySingleLine(Styles._ClearCoatMask, properties._ClearCoatMask, null);
                    materialEditor.ShaderProperty(properties._ClearCoatColor, Styles._ClearCoatColor);
                    materialEditor.ShaderProperty(properties._ClearCoatRange, Styles._ClearCoatRange);
                    materialEditor.ShaderProperty(properties._ClearCoatGloss, Styles._ClearCoatGloss);
                    materialEditor.ShaderProperty(properties._ClearCoatMult, Styles._ClearCoatMult);

                    GUILayout.Space(20);
                    materialEditor.TexturePropertySingleLine(Styles._SpecularColorMask, properties._SpecularColorMask, null);
                    materialEditor.ShaderProperty(properties._SpecularColor, Styles._SpecularColor);
                    materialEditor.ShaderProperty(properties._SpecularRange, Styles._SpecularRange);
                    materialEditor.ShaderProperty(properties._SpecularGloss, Styles._SpecularGloss);
                    materialEditor.ShaderProperty(properties._SpecularMulti, Styles._SpecularMulti);
                    /*
                    GUILayout.Space(20);

                    materialEditor.TexturePropertySingleLine(Styles.toonSpecMap, properties.toonSpecMapProp, properties.toonSpecColorProp);
                    materialEditor.TexturePropertySingleLine(Styles.toonSpecOptMap, properties.toonSpecOptMapProp, properties.toonSpecOptMapSTProp);

                    EditorGUI.BeginChangeCheck(); 
                    int newToonSpecMode = 
                        EditorGUILayout.Popup("ToonSpecMode", properties.useToonHairSpecProp.floatValue >= 0.5f? 1:0, Enum.GetNames(typeof(JALitGUI.ToonSpecMode)));
                    if (EditorGUI.EndChangeCheck())
                    {
                        materialEditor.RegisterPropertyChangeUndo("Change ToonSpecMode to "+ ((JALitGUI.ToonSpecMode)newToonSpecMode));
                        properties.useToonHairSpecProp.floatValue = newToonSpecMode;
                    }

                    if(newToonSpecMode == 0)//common toon spec
                    {
                        materialEditor.ShaderProperty(properties.toonSpecGlossProp, Styles.toonSpecGloss);
                        materialEditor.ShaderProperty(properties.toonSpecFeatherLevelProp, Styles.toonSpecFeatherLevel);
                        materialEditor.ShaderProperty(properties.toonSpecMaskScaleProp, Styles.toonSpecMaskScale);
                        materialEditor.ShaderProperty(properties.toonSpecMaskOffsetProp, Styles.toonSpecMaskOffset);
                    }
                    else //hair toon spec
                    {
                        materialEditor.ShaderProperty(properties.toonSpecAnisoHighLightPower_1st, Styles.toonSpecAnisoHighLightPower_1st);
                        materialEditor.ShaderProperty(properties.toonSpecAnisoHighLightStrength_1st, Styles.toonSpecAnisoHighLightStrength_1st);
                        materialEditor.ShaderProperty(properties.toonSpecShiftTangent_1st, Styles.toonSpecShiftTangent_1st);

                        materialEditor.ShaderProperty(properties.toonSpecAnisoHighLightPower_2nd, Styles.toonSpecAnisoHighLightPower_2nd);
                        materialEditor.ShaderProperty(properties.toonSpecAnisoHighLightStrength_2nd, Styles.toonSpecAnisoHighLightStrength_2nd);
                        materialEditor.ShaderProperty(properties.toonSpecShiftTangent_2nd, Styles.toonSpecShiftTangent_2nd);
                    }*/
                    EditorGUI.indentLevel--;
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.BeginDisabledGroup(!useToonSpec);
                {
                    EditorGUILayout.LabelField("    ...");
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        public static void DoMetallicSpecularArea(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            string[] roughnessChannelNames;
            bool hasGlossMap = false;
            if (properties.workflowMode == null ||
                (WorkflowMode)properties.workflowMode.floatValue == WorkflowMode.Metallic)
            {
                hasGlossMap = properties.metallicGlossMap.textureValue != null;
                roughnessChannelNames = Styles.metallicRoughnessChannelNames;
                materialEditor.TexturePropertySingleLine(Styles.metallicMapText, properties.metallicGlossMap,
                    hasGlossMap ? null : properties.metallic);
            }
            else
            {
                hasGlossMap = properties.specGlossMap.textureValue != null;
                roughnessChannelNames = Styles.specularRoughnessChannelNames;
                BaseShaderGUI.TextureColorProps(materialEditor, Styles.specularMapText, properties.specGlossMap,
                    hasGlossMap ? null : properties.specColor);
            }
            EditorGUI.indentLevel++;
            DoRoughness(properties, materialEditor, material, roughnessChannelNames);
            EditorGUI.indentLevel--;
        }

        public static void DoRoughness(LitProperties properties, MaterialEditor materialEditor, Material material, string[] roughnessChannelNames)
        {
            bool opaque = ((BaseShaderGUI.SurfaceType)material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
            EditorGUI.indentLevel++;
            /*
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.roughness.hasMixedValue;
            float roughness = EditorGUILayout.Slider(Styles.roughnessText, properties.roughness.floatValue, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
                properties.roughness.floatValue = roughness;
            EditorGUI.showMixedValue = false;*/
            materialEditor.TexturePropertySingleLine(Styles.roughnessText, properties.roughnessMap,
                    properties.roughness);

            if (properties.roughnessMapChannel != null) // smoothness channel
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(!opaque);
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = properties.roughnessMapChannel.hasMixedValue;
                var roughnessSource = (int)properties.roughnessMapChannel.floatValue;
                /*if (opaque)
                    roughnessSource = EditorGUILayout.Popup(Styles.roughnessMapChannelText, roughnessSource,
                        roughnessChannelNames);
                else
                    EditorGUILayout.Popup(Styles.roughnessMapChannelText, 0, roughnessChannelNames);
                */
                if (EditorGUI.EndChangeCheck())
                    properties.roughnessMapChannel.floatValue = roughnessSource;
                EditorGUI.showMixedValue = false;
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        public static RoughnessMapChannel GetRoughnessMapChannel(Material material)
        {
            int ch = (int)material.GetFloat("_RoughnessTextureChannel");
            if (ch == (int)RoughnessMapChannel.AlbedoAlpha)
                return RoughnessMapChannel.AlbedoAlpha;

            return RoughnessMapChannel.SpecularMetallicAlpha;
        }

        public static void SetMaterialKeywords(Material material)
        {
            // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
            // (MaterialProperty value might come from renderer material property block)
            bool hasGlossMap = false;
            bool isSpecularWorkFlow = false;
            bool opaque = ((BaseShaderGUI.SurfaceType)material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
            if (material.HasProperty("_WorkflowMode"))
            {
                isSpecularWorkFlow = (WorkflowMode)material.GetFloat("_WorkflowMode") == WorkflowMode.Specular;
                if (isSpecularWorkFlow)
                    hasGlossMap = material.GetTexture("_SpecGlossMap") != null;
                else
                    hasGlossMap = material.GetTexture("_MetallicGlossMap") != null;
            }
            else
            {
                hasGlossMap = material.GetTexture("_MetallicGlossMap") != null;
            }

            CoreUtils.SetKeyword(material, "_SPECULAR_SETUP", isSpecularWorkFlow);

            CoreUtils.SetKeyword(material, "_METALLICSPECGLOSSMAP", hasGlossMap);

            if (material.HasProperty("_SpecularHighlights"))
                CoreUtils.SetKeyword(material, "_SPECULARHIGHLIGHTS_OFF",
                    material.GetFloat("_SpecularHighlights") == 0.0f);
            if (material.HasProperty("_EnvironmentReflections"))
                CoreUtils.SetKeyword(material, "_ENVIRONMENTREFLECTIONS_OFF",
                    material.GetFloat("_EnvironmentReflections") == 0.0f);
            if (material.HasProperty("_OcclusionMap"))
                CoreUtils.SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));

            if (material.HasProperty("_RoughnessTextureChannel"))
            {
                CoreUtils.SetKeyword(material, "_ROUGHNESS_TEXTURE_ALBEDO_CHANNEL_A",
                    GetRoughnessMapChannel(material) == RoughnessMapChannel.AlbedoAlpha && opaque);
            }
        }

    }
}