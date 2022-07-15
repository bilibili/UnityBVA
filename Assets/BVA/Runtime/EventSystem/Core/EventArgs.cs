using System;
using UnityEngine;

namespace BVA.EventSystem
{
    public abstract class BaseEventArgs:IDisposable
    {
        public  Enum EventType { protected set; get; }
        public GameObject Sender { protected set; get; }
        public virtual void Config(Enum _t, GameObject _sender)
        {
            EventType = _t;
            Sender = _sender;
        }
        public  virtual void Dispose()
        {
            this.Sender = null;
        }
    }
  
    
}

