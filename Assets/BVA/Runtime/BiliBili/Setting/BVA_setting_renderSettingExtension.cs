using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using BVA.Extensions;
using UnityEngine.Rendering;

namespace GLTF.Schema.BVA
{
    public class BVA_setting_renderSettingExtension : IExtension
    {
        public IExtension Clone(GLTFRoot root)
        {
            return this;
        }
        public MaterialId skybox;
        public NodeId sun;
        public CubemapId customReflection;
        public JProperty Serialize()
        {
            JObject propObj = new JObject();

            float[] sh = new float[27];
            for (int rgb = 0; rgb < 3; rgb++)
            {
                for (int cofficient = 0; cofficient < 9; cofficient++)
                {
                    sh[rgb * 9 + cofficient] = RenderSettings.ambientProbe[rgb, cofficient];
                }
            }
            JArray shArray = new JArray(sh);
            propObj.Add(nameof(RenderSettings.ambientProbe), shArray);

            if (skybox != null && skybox.IsValid) propObj.Add(nameof(skybox), skybox.Id);
            if (sun != null && sun.IsValid) propObj.Add(nameof(sun), sun.Id);
            if (customReflection != null && customReflection.IsValid) propObj.Add(nameof(customReflection), customReflection.Id);

            propObj.Add(nameof(RenderSettings.subtractiveShadowColor), RenderSettings.subtractiveShadowColor.ToJArray());

            propObj.Add(nameof(RenderSettings.ambientMode), RenderSettings.ambientMode.ToString());
            propObj.Add(nameof(RenderSettings.ambientIntensity), RenderSettings.ambientIntensity);
            propObj.Add(nameof(RenderSettings.ambientLight), RenderSettings.ambientLight.ToJArray());
            propObj.Add(nameof(RenderSettings.ambientSkyColor), RenderSettings.ambientSkyColor.ToJArray());
            propObj.Add(nameof(RenderSettings.ambientEquatorColor), RenderSettings.ambientEquatorColor.ToJArray());
            propObj.Add(nameof(RenderSettings.ambientGroundColor), RenderSettings.ambientGroundColor.ToJArray());

            propObj.Add(nameof(RenderSettings.reflectionIntensity), RenderSettings.reflectionIntensity);
            propObj.Add(nameof(RenderSettings.reflectionBounces), RenderSettings.reflectionBounces);
            propObj.Add(nameof(RenderSettings.defaultReflectionMode), RenderSettings.defaultReflectionMode.ToString());
            propObj.Add(nameof(RenderSettings.defaultReflectionResolution), RenderSettings.defaultReflectionResolution);

            propObj.Add(nameof(RenderSettings.flareFadeSpeed), RenderSettings.flareFadeSpeed);
            propObj.Add(nameof(RenderSettings.flareStrength), RenderSettings.flareStrength);

            propObj.Add(nameof(RenderSettings.haloStrength), RenderSettings.haloStrength);

            propObj.Add(nameof(RenderSettings.fog), RenderSettings.fog);
            propObj.Add(nameof(RenderSettings.fogColor), RenderSettings.fogColor.ToJArray());
            propObj.Add(nameof(RenderSettings.fogDensity), RenderSettings.fogDensity);
            propObj.Add(nameof(RenderSettings.fogStartDistance), RenderSettings.fogStartDistance);
            propObj.Add(nameof(RenderSettings.fogEndDistance), RenderSettings.fogEndDistance);
            propObj.Add(nameof(RenderSettings.fogMode), RenderSettings.fogMode.ToString());

            JProperty jProperty = new JProperty(BVA_light_lightmapExtensionFactory.EXTENSION_NAME, propObj);

            return jProperty;
        }
        private JToken jsonObj;
        /// <summary>
        /// Keep the JsonReader value as JToken, until we need to use it later will us Deserialize
        /// </summary>
        /// <param name="root"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static BVA_setting_renderSettingExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new BVA_setting_renderSettingExtension() { jsonObj = JToken.ReadFrom(reader) };
        }
        public BVA_setting_renderSettingExtension Deserialize(GLTFRoot root)
        {
            MaterialId skybox = null;
            NodeId sun = null;
            CubemapId customReflection = null;
            JsonReader reader = jsonObj.CreateReader();
            reader.Read();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(RenderSettings.subtractiveShadowColor):
                        RenderSettings.subtractiveShadowColor = reader.ReadAsRGBAColor();
                        break;
                    case nameof(RenderSettings.skybox):
                        skybox = new MaterialId() { Id = reader.ReadAsInt32().Value, Root = root };
                        break;
                    case nameof(RenderSettings.sun):
                        sun = new NodeId() { Id = reader.ReadAsInt32().Value, Root = root };
                        break;
                    case nameof(RenderSettings.defaultReflectionMode):
                        RenderSettings.defaultReflectionMode = reader.ReadStringEnum<DefaultReflectionMode>();
                        break;
                    case nameof(RenderSettings.defaultReflectionResolution):
                        RenderSettings.defaultReflectionResolution = reader.ReadAsInt32().Value;
                        break;
                    case nameof(RenderSettings.customReflection):
                        customReflection = new CubemapId() { Id = reader.ReadAsInt32().Value, Root = root };
                        break;
                    case nameof(RenderSettings.ambientMode):
                        RenderSettings.ambientMode = reader.ReadStringEnum<AmbientMode>();
                        break;
                    case nameof(RenderSettings.ambientProbe):
                        {
                            var shData = reader.ReadFloatList();
                            var sh = new SphericalHarmonicsL2();
                            for (int rgb = 0; rgb < 3; rgb++)
                            {
                                for (int cofficient = 0; cofficient < 9; cofficient++)
                                {
                                    sh[rgb, cofficient] = shData[rgb * 9 + cofficient];
                                }
                            }
                            RenderSettings.ambientProbe = sh;
                        }
                        break;
                    case nameof(RenderSettings.ambientIntensity):
                        RenderSettings.ambientIntensity = reader.ReadAsFloat();
                        break;
                    case nameof(RenderSettings.ambientLight):
                        RenderSettings.ambientLight = reader.ReadAsRGBAColor();
                        break;
                    case nameof(RenderSettings.ambientSkyColor):
                        RenderSettings.ambientSkyColor = reader.ReadAsRGBAColor();
                        break;
                    case nameof(RenderSettings.ambientEquatorColor):
                        RenderSettings.ambientEquatorColor = reader.ReadAsRGBAColor();
                        break;
                    case nameof(RenderSettings.ambientGroundColor):
                        RenderSettings.ambientGroundColor = reader.ReadAsRGBAColor();
                        break;
                    case nameof(RenderSettings.reflectionIntensity):
                        RenderSettings.reflectionIntensity = reader.ReadAsFloat();
                        break;
                    case nameof(RenderSettings.reflectionBounces):
                        RenderSettings.reflectionBounces = reader.ReadAsInt32().Value;
                        break;
                    case nameof(RenderSettings.flareFadeSpeed):
                        RenderSettings.flareFadeSpeed = reader.ReadAsFloat();
                        break;
                    case nameof(RenderSettings.flareStrength):
                        RenderSettings.flareStrength = reader.ReadAsFloat();
                        break;
                    case nameof(RenderSettings.haloStrength):
                        RenderSettings.haloStrength = reader.ReadAsFloat();
                        break;
                    case nameof(RenderSettings.fog):
                        RenderSettings.fog = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(RenderSettings.fogColor):
                        RenderSettings.fogColor = reader.ReadAsRGBAColor();
                        break;
                    case nameof(RenderSettings.fogDensity):
                        RenderSettings.fogDensity = reader.ReadAsFloat();
                        break;
                    case nameof(RenderSettings.fogStartDistance):
                        RenderSettings.fogStartDistance = reader.ReadAsFloat();
                        break;
                    case nameof(RenderSettings.fogEndDistance):
                        RenderSettings.fogEndDistance = reader.ReadAsFloat();
                        break;
                    case nameof(RenderSettings.fogMode):
                        RenderSettings.fogMode = reader.ReadStringEnum<FogMode>();
                        break;
                }
            }
            return new BVA_setting_renderSettingExtension() { skybox = skybox, sun = sun, customReflection = customReflection };
        }

    }
    public class BVA_setting_renderSettingExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_setting_renderSettingExtension";
        public const string EXTENSION_ELEMENT_NAME = "renderSettings";
        public BVA_setting_renderSettingExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_setting_renderSettingExtensionFactory(RenderSettingId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public RenderSettingId id;
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_setting_renderSettingExtensionFactory(id);
        }

        public JProperty Serialize()
        {
            JProperty node = new JProperty(EXTENSION_ELEMENT_NAME, id.Id);
            return new JProperty(EXTENSION_NAME, new JObject(node));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            if (extensionToken != null)
            {
                JToken indexToken = extensionToken.Value[EXTENSION_ELEMENT_NAME];
                int _id = indexToken != null ? indexToken.DeserializeAsInt() : -1;
                id = new RenderSettingId() { Id = _id, Root = root };
            }
            return new BVA_setting_renderSettingExtensionFactory(id);
        }
    }
}