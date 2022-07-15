using Newtonsoft.Json.Linq;
using UnityEngine;
using GLTF.Schema;
using UnityEngine.Rendering.Universal;
using BVA.Extensions;
using Newtonsoft.Json;
using GLTF.Extensions;
using System.Reflection;

namespace GLTF.Schema.BVA
{
    public class BVA_Camera_URP_Extra : IExtra
    {
        public const string PROPERTY = "UniversalAdditionalCameraData";
        public Color backgroundColor;
        public CameraClearFlags clearFlags;
        public Vector4 rect;
        public bool renderPostProcessing;
        public bool renderShadows;
        public bool dithering;
        public bool stopNaN;
        public int rendererIndex;
        public AntialiasingMode antialiasing;
        public AntialiasingQuality antialiasingQuality;

        public BVA_Camera_URP_Extra(Camera camera)
        {
            backgroundColor = camera.backgroundColor;
            clearFlags = camera.clearFlags;
            rect = new Vector4(camera.rect.x, camera.rect.y, camera.rect.width, camera.rect.height);
            var cameraData = camera.GetUniversalAdditionalCameraData();

            var property = cameraData.GetType().GetField("m_RendererIndex", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(cameraData);
            rendererIndex = (int)property;

            renderPostProcessing = cameraData.renderPostProcessing;
            renderShadows = cameraData.renderShadows;
            dithering = cameraData.dithering;
            stopNaN = cameraData.stopNaN;
            antialiasing = cameraData.antialiasing;
            antialiasingQuality = cameraData.antialiasingQuality;
        }

        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(backgroundColor), backgroundColor.ToNumericsColorRaw().ToJArray());
            jo.Add(nameof(clearFlags), clearFlags.ToString());
            jo.Add(nameof(rect), rect.ToGltfVector4Raw().ToJArray());
            if (renderPostProcessing) jo.Add(nameof(renderPostProcessing), renderPostProcessing);
            if (renderShadows) jo.Add(nameof(renderShadows), renderShadows);
            if (dithering) jo.Add(nameof(dithering), dithering);
            if (stopNaN) jo.Add(nameof(stopNaN), stopNaN);
            if (antialiasing != AntialiasingMode.None) jo.Add(nameof(antialiasing), antialiasing.ToString());
            if (antialiasing != AntialiasingMode.None) jo.Add(nameof(antialiasingQuality), antialiasingQuality.ToString());
            if (rendererIndex != 0) jo.Add(nameof(rendererIndex), rendererIndex);
            return new JProperty(PROPERTY, jo);
        }
        public static void Deserialize(GLTFRoot _gltfRoot, JsonReader reader, Camera camera)
        {
            var additionalData = camera.GetUniversalAdditionalCameraData();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(backgroundColor):
                            camera.backgroundColor = reader.ReadAsRGBAColor().ToUnityColorRaw();
                            break;
                        case nameof(clearFlags):
                            camera.clearFlags = reader.ReadStringEnum<CameraClearFlags>();
                            break;
                        case nameof(rect):
                            Math.Vector4 rectData = reader.ReadAsVector4();
                            camera.rect = new Rect(rectData.X, rectData.Y, rectData.Z, rectData.W);
                            break;
                        case nameof(renderPostProcessing):
                            additionalData.renderPostProcessing = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(renderShadows):
                            additionalData.renderShadows = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(dithering):
                            additionalData.dithering = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(stopNaN):
                            additionalData.stopNaN = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(antialiasing):
                            additionalData.antialiasing = reader.ReadStringEnum<AntialiasingMode>();
                            break;
                        case nameof(antialiasingQuality):
                            additionalData.antialiasingQuality = reader.ReadStringEnum<AntialiasingQuality>();
                            break;
                        case nameof(rendererIndex):
                            additionalData.SetRenderer(reader.ReadAsInt32().Value);
                            break;
                    }
                }
            }
        }
    }
}