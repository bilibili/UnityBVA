using UnityEngine;
using BVA.Component;

/*namespace GLTF.Schema.BVA
{
    public struct ScriptDataMeta
    {
        public PuertsEvent.EventType eventType;
        public PuertsCall.CallType callType;
        public KeyCode keyCode;
        public string name;
        public string description;
        public TextAsset script;

        public ScriptDataMeta(PuertsEvent.ScriptData target)
        {
            this.description = target.description;
            this.eventType = target.callType;
            this.keyCode = target.keyCode;
            this.name = target.name;
            this.script = target.script;

            this.callType = (PuertsCall.CallType)0;
        }

        public ScriptDataMeta(PuertsCall.ScriptData target)
        {
            this.description = target.description;
            this.callType = target.callType;

            this.name = target.name;
            this.script = target.script;

            this.eventType = (PuertsEvent.EventType)0;
            this.keyCode = UnityEngine.KeyCode.None;
        }

        public static implicit operator PuertsEvent.ScriptData(ScriptDataMeta data)
        {
            var result = new PuertsEvent.ScriptData();
            result.description = data.description;
            result.callType = data.eventType;
            result.keyCode = data.keyCode;
            result.name = data.name;
            result.script = data.script;

            return result;
        }

        public static implicit operator PuertsCall.ScriptData(ScriptDataMeta data)
        {
            var result = new PuertsCall.ScriptData();
            result.description = data.description;
            result.callType = data.callType;
            result.name = data.name;
            result.script = data.script;

            return result;
        }
    }
}*/