
#if PUERTS_INTEGRATION
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using BVA.Component;
using static GLTF.Extensions.JsonReaderExtensions;
using UnityEngine;
using BVA.Cache;

namespace GLTF.Schema.BVA
{
    /// <summary>
    /// </summary>
    public class PuertsMonoMeta : IEquatable<PuertsMonoMeta>
    {
        public string puertsMonoTypeName;
        public List<ScriptData> scriptDatas;
        public int[] transformIndex;
        public PuertsMonoMeta() { }
        public PuertsMonoMeta(MonoBehaviour puertsMono, BVA.NodeCache _nodeCache)
        {
            puertsMonoTypeName = puertsMono.GetType().Name;
            GameObject[] gameObjects = null;
            switch (puertsMonoTypeName)
            {
                case nameof(PuertsEvent):
                    var puertsEvent = puertsMono as PuertsEvent;
                    scriptDatas = puertsEvent.scripts;
                    gameObjects = puertsEvent.gameObjects;
                    break;
                case nameof(PuertsCall):
                    var puertsCall = puertsMono as PuertsCall;
                    scriptDatas = puertsCall.scripts;
                    gameObjects = puertsCall.gameObjects;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            if (gameObjects.Length != 0)
            {
                transformIndex = gameObjects.Select(x => _nodeCache.GetId(x)).ToArray();
            }
        }
        public bool Equals(PuertsMonoMeta v)
        {
            var targetScriptDatas = v.scriptDatas;
            var targetTransformIndex = v.transformIndex;
            if (targetScriptDatas.Count != scriptDatas.Count)
            {
                return false;
            }
            for (int i = 0; i < targetTransformIndex.Length; i++)
            {
                if (transformIndex[i] != targetTransformIndex[i])
                {
                    return false;
                }
            }
            for (int i = 0; i < targetScriptDatas.Count; i++)
            {
                var originScript = scriptDatas[i];
                var targetScript = targetScriptDatas[i];
                if (!(originScript.eventType == targetScript.eventType &&
                    originScript.callType == targetScript.callType &&
                    originScript.keyCode == targetScript.keyCode &&
                    originScript.name == targetScript.name &&

                    originScript.description == targetScript.description &&
                    originScript.script.text == targetScript.script.text
                    ))
                {
                    return false;
                }
            }
            return true;
        }

        public JObject Serialize()
        {
            JArray array = new JArray();
            for (int i = 0; i < scriptDatas.Count; i++)
            {
                JObject propObj = new JObject();

                var scriptData = scriptDatas[i];

                propObj.Add(nameof(scriptData.eventType), scriptData.eventType.ToString());
                propObj.Add(nameof(scriptData.callType), scriptData.callType.ToString());
                propObj.Add(nameof(scriptData.keyCode), scriptData.keyCode.ToString());
                propObj.Add(nameof(scriptData.name), scriptData.name);
                propObj.Add(nameof(scriptData.description), scriptData.description.ToString());
                if (scriptData.script != null)
                {
                    propObj.Add(nameof(scriptData.script), scriptData.script.text.ToString());
                }
                array.Add(propObj);
            }

            JObject jo = new JObject();
            jo.Add(nameof(puertsMonoTypeName), puertsMonoTypeName);
            jo.Add(nameof(scriptDatas), array);
            if (transformIndex != null)
            {
                jo.Add(nameof(transformIndex), new JArray(transformIndex));
            }

            return jo;
        }

        internal static PuertsMonoMeta Deserialize(GLTFRoot root, JsonReader reader)
        {
            PuertsMonoMeta puertsMonoMeta = new PuertsMonoMeta();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();
                switch (curProp)
                {
                    case nameof(puertsMonoTypeName):
                        puertsMonoMeta.puertsMonoTypeName = reader.ReadAsString();
                        break;
                    case nameof(scriptDatas):
                        puertsMonoMeta.scriptDatas = reader.ReadList(() => ReadScriptData(root, reader));
                        break;
                    case nameof(transformIndex):
                        puertsMonoMeta.transformIndex = reader.ReadInt32List().ToArray();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            return puertsMonoMeta;
        }
        public void Deserialize(GameObject nodeObj, AssetCache assetCache)
        {
            switch (puertsMonoTypeName)
            {
                case nameof(PuertsEvent):
                    var puertsEvent = nodeObj.AddComponent<PuertsEvent>();
                    puertsEvent.scripts = scriptDatas;
                    if (transformIndex != null)
                    {
                        puertsEvent.gameObjects = transformIndex.Select(x => assetCache.NodeCache[x]).ToArray();
                    }

                    break;
                case nameof(PuertsCall):
                    var puertsCall = nodeObj.AddComponent<PuertsCall>();
                    puertsCall.scripts = scriptDatas;
                    if (transformIndex != null)
                    {
                        puertsCall.gameObjects = transformIndex.Select(x => assetCache.NodeCache[x]).ToArray();
                    }
                    puertsCall.OnEnable(); // manual call OnEnable
                    break;
                default:
                    throw new InvalidOperationException();
            }

        }

        private static ScriptData ReadScriptData(GLTFRoot root, JsonReader reader)
        {
            ScriptData scriptData = new ScriptData();
            string textname = "";
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(scriptData.eventType):
                        scriptData.eventType = reader.ReadStringEnum<PuertsEvent.EventType>();
                        break;
                    case nameof(scriptData.callType):
                        scriptData.callType = reader.ReadStringEnum<PuertsCall.CallType>();
                        break;
                    case nameof(scriptData.keyCode):
                        scriptData.keyCode = reader.ReadStringEnum<UnityEngine.KeyCode>();
                        break;
                    case nameof(scriptData.name):
                        scriptData.name = reader.ReadAsString();
                        break;
                    case nameof(scriptData.description):
                        scriptData.description = reader.ReadAsString();
                        break;
                    case nameof(scriptData.script):
                        scriptData.script = new UnityEngine.TextAsset(reader.ReadAsString());
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            if (scriptData.script != null)
            {
                scriptData.script.name = textname;
            }
            return scriptData;
        }
    }
}
#endif