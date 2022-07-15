using System.Collections.Generic;
using UnityEngine;
#if PUERTS_INTEGRATION
using Puerts;
#endif
using System.Linq;

namespace BVA.Component
{
#if PUERTS_INTEGRATION
    [System.Serializable]
    public struct ScriptData
    {
        public PuertsEvent.EventType eventType;
        public PuertsCall.CallType callType;
        public KeyCode keyCode;
        public string name;
        public string description;
        public TextAsset script;
    }
#endif
    [AddComponentMenu("BVA/Scripting/Call")]
    public class PuertsCall : MonoBehaviour
    {
#if PUERTS_INTEGRATION
        public enum CallType
        {
            //Awake,
            Start,
            OnEnable,
            OnDisable,
            OnDestroy,
            Update,
            LateUpdate,
            FixedUpdate
        }

        public GameObject[] gameObjects;
        public List<ScriptData> scripts;
        static JsEnv jsEnv;
        Dictionary<CallType, string[]> callMap;
        public string[] GetScripts(CallType callType)
        {
            var script = scripts.FindAll(x => x.callType == callType);
            if (script != null && script.Count > 0)
            {
                string[] texts = script.Select(x => x.script.text).ToArray();
                return texts;
            }
            else
            {
                return null;
            }
        }

        public void Do(CallType callType)
        {
            if (callMap!=null && callMap.TryGetValue(callType, out string[] startCall))
            {
                if (startCall != null)
                {
                    foreach (var call in startCall)
                    {
                        jsEnv.Eval(call);
                    }
                }
            }
        }
        public virtual void Start()
        {
            jsEnv ??= new JsEnv();
            callMap ??= new Dictionary<CallType, string[]>();
            var enums = System.Enum.GetValues(typeof(CallType)).Cast<CallType>().ToList();
            foreach (var callType in enums)
            {
                callMap.Add(callType, GetScripts(callType));
            }

            Do(CallType.Start);
        }
        public virtual void OnDestroy()
        {
            jsEnv?.Dispose();
            Do(CallType.OnDestroy);
        }
        public virtual void Update()
        {
            Do(CallType.Update);
        }
        public virtual void LateUpdate()
        {
            Do(CallType.LateUpdate);
        }
        public virtual void FixedUpdate()
        {
            Do(CallType.FixedUpdate);
        }
        public virtual void OnEnable()
        {
            Do(CallType.OnEnable);
        }
        public virtual void OnDisable()
        {
            Do(CallType.OnDisable);
        }
#endif
    }
}