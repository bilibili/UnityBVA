#if PUERTS_INTEGRATION
using Puerts;
#endif
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BVA.Component
{
    [AddComponentMenu("BVA/Scripting/Event")]
    public class PuertsEvent : MonoBehaviour
    {
#if PUERTS_INTEGRATION
        public enum EventType
        {
            OnMouseUp,
            OnMouseDown,
            OnMouseEnter,
            OnMouseExit,
            OnMouseOver,
            OnMouseDrag,
            OnMouseUpAsButton,
            KeyDown,
            Key,
            KeyUp,
            AnyKeyDown
        }

        public GameObject[] gameObjects;
        public List<ScriptData> scripts;
        static JsEnv jsEnv;
        Dictionary<EventType, string[]> eventMap;
        public string[] GetScripts(EventType eventType)
        {
            var script = scripts.FindAll(x => x.eventType == eventType);
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
        public void DoEvent(EventType eventType)
        {
            var startCall = eventMap[eventType];
            if (startCall != null)
            {
                foreach (var call in startCall)
                {
                    jsEnv.Eval(call);
                }
            }
        }
        private void Awake()
        {
            jsEnv ??= new JsEnv();
            eventMap ??= new Dictionary<EventType, string[]>();
            var enums = System.Enum.GetValues(typeof(EventType)).Cast<EventType>().ToList();
            foreach (var callType in enums)
            {
                eventMap.Add(callType, GetScripts(callType));
            }
        }
        public void OnMouseUp()
        {
            DoEvent(EventType.OnMouseUp);
        }
        public void OnMouseDown()
        {
            DoEvent(EventType.OnMouseDown);
        }
        public void OnMouseEnter()
        {
            DoEvent(EventType.OnMouseEnter);
        }
        public void OnMouseExit()
        {
            DoEvent(EventType.OnMouseExit);
        }
        public void OnMouseOver()
        {
            DoEvent(EventType.OnMouseOver);
        }
        public void OnMouseDrag()
        {
            DoEvent(EventType.OnMouseDrag);
        }
        public void OnMouseUpAsButton()
        {
            DoEvent(EventType.OnMouseUpAsButton);
        }
        public void Update()
        {
            if (Input.anyKeyDown)
                DoEvent(EventType.AnyKeyDown);
            var scripts = GetScripts(EventType.KeyDown);

        }
#endif
    }
}