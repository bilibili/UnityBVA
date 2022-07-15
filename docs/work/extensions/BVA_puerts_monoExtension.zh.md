# BVA_puerts_monoExtension

## 概览

在当前阶段，我们使用 `Puerts` 作为代码热更新的引擎。 `Lua` 将很快得到支持。

目前，我们使用命名方案来识别为特定事件调用哪个函数。

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

而对于这些需要手动触发的事件，目前只有键盘和鼠标事件被监听。

> 未来将支持下面列出的所有设备：

- 键盘和鼠标
- 操纵杆
- 控制器
- 触摸屏
- 移动设备的运动感应功能，例如加速度计或陀螺仪
- VR和AR控制器

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

> `BVA_puerts_monoExtension`以下列形式记录信息:

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

支持通用渲染事件和来自各种输入设备的事件，脚本引擎在接收到相应的事件后会执行脚本。