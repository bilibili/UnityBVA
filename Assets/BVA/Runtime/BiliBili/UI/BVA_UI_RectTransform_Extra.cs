using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_RectTransform_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_RectTransform_Extra";
        public UnityEngine.Vector2 anchorMin;
        public UnityEngine.Vector2 anchorMax;
        public UnityEngine.Vector2 anchoredPosition;
        public UnityEngine.Vector2 sizeDelta;
        public UnityEngine.Vector2 pivot;
        public UnityEngine.Vector3 anchoredPosition3D;
        public UnityEngine.Vector2 offsetMin;
        public UnityEngine.Vector2 offsetMax;
        public BVA_UI_RectTransform_Extra() { }

        public BVA_UI_RectTransform_Extra(UnityEngine.RectTransform target)
        {
            this.anchorMin = target.anchorMin;
            this.anchorMax = target.anchorMax;
            this.anchoredPosition = target.anchoredPosition;
            this.sizeDelta = target.sizeDelta;
            this.pivot = target.pivot;
            this.anchoredPosition3D = target.anchoredPosition3D;
            this.offsetMin = target.offsetMin;
            this.offsetMax = target.offsetMax;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.RectTransform target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_UI_RectTransform_Extra.anchorMin):
                            target.anchorMin = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(BVA_UI_RectTransform_Extra.anchorMax):
                            target.anchorMax = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(BVA_UI_RectTransform_Extra.anchoredPosition):
                            target.anchoredPosition = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(BVA_UI_RectTransform_Extra.sizeDelta):
                            target.sizeDelta = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(BVA_UI_RectTransform_Extra.pivot):
                            target.pivot = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(BVA_UI_RectTransform_Extra.anchoredPosition3D):
                            target.anchoredPosition3D = reader.ReadAsVector3().ToUnityVector3Raw();
                            break;
                        case nameof(BVA_UI_RectTransform_Extra.offsetMin):
                            target.offsetMin = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(BVA_UI_RectTransform_Extra.offsetMax):
                            target.offsetMax = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(anchorMin), anchorMin.ToGltfVector2Raw().ToJArray());
            jo.Add(nameof(anchorMax), anchorMax.ToGltfVector2Raw().ToJArray());
            jo.Add(nameof(anchoredPosition), anchoredPosition.ToGltfVector2Raw().ToJArray());
            jo.Add(nameof(sizeDelta), sizeDelta.ToGltfVector2Raw().ToJArray());
            jo.Add(nameof(pivot), pivot.ToGltfVector2Raw().ToJArray());
            jo.Add(nameof(anchoredPosition3D), anchoredPosition3D.ToGltfVector3Raw().ToJArray());
            jo.Add(nameof(offsetMin), offsetMin.ToGltfVector2Raw().ToJArray());
            jo.Add(nameof(offsetMax), offsetMax.ToGltfVector2Raw().ToJArray());
            return new JProperty(BVA_UI_RectTransform_Extra.PROPERTY, jo);
        }
    }
}
