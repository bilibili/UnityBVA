using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BVA.EventSystem
{
    /// <summary>
    /// 笔或者鼠标事件类型
    /// </summary>
    public enum MouseEvent
    {
        Enter,
        Press,
        Release,
        Exit
    }
    /// <summary>
    /// 脚本事件类型
    /// </summary>
    public enum ScriptEvent
    {
        Add,
        Remove
    }
    /// <summary>
    /// UI事件类型
    /// </summary>
    public enum UIEvent
    {
        PopUp
    }
}
