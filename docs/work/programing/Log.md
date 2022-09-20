# Log

`LogPool` is a class that use for logging essential information, it has three default instance:

- ExportLogger
- ImportLogger
- RuntimeLogger

Three method that you will be used；

- Log(LogPart part, string msg)
- LogWarning(LogPart part, string msg)
- LogError(LogPart part, string msg)

`LogPart` is enum type, it indicates the parts involved.

```csharp
public enum LogPart : int
{
    Scene,
    Video,
    Texture,
    Skin,
    PostProcess,
    Playable,
    Node,
    Mesh,
    Shader,
    Material,
    Light,
    Collision,
    Camera,
    BlendShape,
    Avatar,
    Audio,
    Animation,
    Count
}
```

`LOG_CONSOLE` determine whether logs needs to be sent to the console.

```csharp
#if UNITY_EDITOR || ENABLE_DEBUG
        public const bool LOG_CONSOLE = true;
#else

        public const bool LOG_CONSOLE = false;
#endif
```