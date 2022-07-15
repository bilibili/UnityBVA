using UnityEngine;

namespace BVA.EventSystem
{
    public class AddComponentEventArgs : BaseEventArgs
    {
        public UnityEngine.Component Component { private set; get; }
        public AddComponentEventArgs Config(ScriptEvent _t, GameObject _sender, UnityEngine.Component component)
        {
            base.Config(_t, _sender);
            this.Component = component;
            return this;
        }
    }
}