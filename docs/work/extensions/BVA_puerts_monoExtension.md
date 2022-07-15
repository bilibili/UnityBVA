# BVA_puerts_monoExtension

## Overview

In the current phase, we are using `Puerts` as the engine for hot updates to the code. `Lua` will be supported soon.

Currently, we uses a naming scheme to identify which function to call for a particular event.

```csharp
    public class PuertsCall 
    {
        public enum CallType
        {
            Start,
            OnEnable,
            OnDisable,
            OnDestroy,
            Update,
            LateUpdate,
            FixedUpdate
        }
    }
```

And for these events that need to be triggered manually, to respond to user input.
Currently only Keyboard and Mouse event are being listening.

> All devices listed below will be supported in the future:

- Keyboards and mice
- Joysticks
- Controllers
- Touch screens
- Movement-sensing capabilities of mobile devices, such as accelerometers or gyroscopes
- VR and AR controllers

```csharp
    public class PuertsEvent
    {
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
    }
```

> The `BVA_puerts_monoExtension` store information in the following layout:

```csharp
    public struct ScriptData
    {
        public PuertsEvent.EventType eventType;
        public PuertsCall.CallType callType;
        public KeyCode keyCode;
        public string name;
        public string description;
        public TextAsset script;
    }
```

Support for general rendering events and events from various input devices, the script engine will execute the script after receiving the corresponding event.