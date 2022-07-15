using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
namespace BVA.EventSystem
{
    public class EventManager
    {
        #region 单例
        /// <summary>
        /// 事件总线实例
        /// </summary>
        private static EventManager entity = null;
        protected static readonly object m_staticSyncRoot = new object();
        public static EventManager Instance
        {
            get
            {
                if (null == entity)
                {
                    lock (m_staticSyncRoot)
                    {
                        if (null == entity)
                        {
                            entity = new EventManager();
                        }
                    }
                }
                return entity;
            }
        }
        #endregion

        /// <summary>
        /// 事件链
        /// </summary>
        private Dictionary<Enum, Action<BaseEventArgs>> listeners = null;//使用.Net 默认的 多播委托 Action  
        /// <summary>
        /// 是否中断事件分发,默认不中断
        /// </summary>
        public static bool Interrupt { get; internal set; } = false;

        private EventManager()
        {
            this.Capacity = 60;
            InitEvent();
        }

        /// <summary>
        /// 得到指定枚举项的所有事件链
        /// </summary>
        /// <param name="_type">指定枚举项</param>
        /// <returns>事件链</returns>
        private Action<BaseEventArgs> GetEventList(Enum _type)
        {
            if (!listeners.ContainsKey(_type))
            {
                listeners.Add(_type, default(Action<BaseEventArgs>));
            }
            return listeners[_type];
        }
        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="_type">指定类型</param>
        /// <param name="action">指定事件</param>
        private void AddEvent(Enum _type, Action<BaseEventArgs> action)
        {
            Action<BaseEventArgs> actions = GetEventList(_type);
            if (null != action)
            {
                Delegate[] delegates = actions?.GetInvocationList();
                if (null != delegates)
                {
                    if (!Array.Exists(delegates, v => v == (Delegate)action))
                    {
                        actions += action;
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarningFormat("重复的事件监听：{0}.{1}", _type.GetType().Name, _type.ToString());
                    }
                }
                else
                {
                    actions = action;
                }
                listeners[_type] = actions;
            }
            else
            {
                UnityEngine.Debug.LogWarning("指定添加的事件为 null ！");
            }
        }
        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="_type">指定事件类型</param>
        /// <param name="args">事件参数</param>
        private void CallEvent(BaseEventArgs args)
        {
            Action<BaseEventArgs> actions = GetEventList(args.EventType);
            actions?.Invoke(args);
            Recycle(args); //One Shot Event 
        }
        /// <summary>
        /// 删除指定的事件
        /// </summary>
        /// <param name="_type">指定类型</param>
        /// <param name="action">指定的事件</param>
        private void DelEvent(Enum _type, Action<BaseEventArgs> action)
        {
            if (null != action)
            {
                Action<BaseEventArgs> actions = GetEventList(_type);
                if (null != action)
                {
                    actions -= action;
                }
                listeners[_type] = actions;
            }
            else
            {
                UnityEngine.Debug.LogWarning("指定移除的事件为 null ！");
            }
        }
        /// <summary>
        /// 删除指定的事件(写的这么多就知道不指定事件枚举不被建议了)
        /// </summary>
        /// <param name="action">指定的事件</param>
        private void DelEvent(Action<BaseEventArgs> action)
        {
            if (null != action)
            {
                List<Action<BaseEventArgs>> actionsArr = new List<Action<BaseEventArgs>>(listeners.Values);
                List<Enum> eventTypeArr = new List<Enum>(listeners.Keys);
                Predicate<Action<BaseEventArgs>> predicate = v =>
                {
                    Delegate[] d = v?.GetInvocationList();
                    return (null != d && Array.Exists(d, vv => vv == (Delegate)action));
                };
                int index = actionsArr.FindIndex(predicate);
                if (index != -1)
                {
                    DelEvent(eventTypeArr[index], action);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("移除的事件未曾注册过 ！");
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("指定移除的事件为 null ！");
            }
        }
        /// <summary>
        /// 删除指定事件类型的所有事件
        /// </summary>
        /// <param name="eventType">指定的事件类型</param>
        private void DelEvent(Enum eventType)
        {
            listeners.Remove(eventType);
        }
        /// <summary>
        /// 初始化事件链
        /// </summary>
        private void InitEvent()
        {
            recycled = new Dictionary<Type, Queue<BaseEventArgs>>();
            listeners = new Dictionary<Enum, Action<BaseEventArgs>>();
        }

        #region//--------------------------StaticFunction-------------------------------
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="_type">事件类型</param>
        /// <param name="action">事件</param>
        public static void AddListener(Enum _type, Action<BaseEventArgs> action)
        {
            Instance.AddEvent(_type, action);
        }
        /// <summary>
        /// 事件分发
        /// </summary>
        /// <param name="_type">事件类型</param>
        /// <param name="args">事件参数</param>
        public static void Invoke(BaseEventArgs args)
        {
            if (Interrupt)
            {
                return;
            }
            entity?.CallEvent(args);
        }
        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="_type">事件类型</param>
        /// <param name="action">事件</param>
        public static void DelListener(Enum _type, Action<BaseEventArgs> action)
        {
            entity?.DelEvent(_type, action);
        }
        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="_type">事件类型</param>
        /// <param name="action">事件</param>
        public static void DelListener(Action<BaseEventArgs> action)
        {
            entity?.DelEvent(action);
        }
        /// <summary>
        /// 移除指定类型的所有事件监听
        /// </summary>
        /// <param name="_type">事件类型</param>
        public static void DelListener(Enum _type)
        {
            entity?.DelEvent(_type);
        }
        /// <summary>
        /// 移除所有事件
        /// </summary>
        public static void RemoveAllListener()
        {
            entity?.InitEvent();
        }
        #endregion//--------------------------StaticFunction-------------------------------

        #region  Poolable Extension Function 
        internal Dictionary<Type, Queue<BaseEventArgs>> recycled; //
        int Capacity { get; set; } //池子有多大
        /// <summary>
        /// 分配指定参数类型的实例
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>分配的实例</returns>
        public static T Allocate<T>() where T : BaseEventArgs, new()//分配
        {
            Type type = typeof(T);
            Queue<BaseEventArgs> args;
            if (Instance.recycled.TryGetValue(type, out args))
            {
                if (null != args && args.Count == Instance.Capacity)//回收使用最老
                {
                    T arg = args.Dequeue() as T;
                    arg.Dispose();
                    return arg;
                }
            }
            return new T() as T;
        }
        /// <summary>
        /// 回收参数类型的实例
        /// </summary>
        /// <param name="target">指定的实例</param>
        void Recycle(BaseEventArgs target) //释放
        {
            Type type = target.GetType();
            Queue<BaseEventArgs> args;
            if (!Instance.recycled.TryGetValue(type, out args))
            {
                args = new Queue<BaseEventArgs>();
            }
            if (args.Count < Instance.Capacity && !args.Contains(target))
            {
                args.Enqueue(target);
            }
            else
            {
                target.Dispose();
            }
            Instance.recycled[type] = args;
        }
        #endregion
    }

    #region Method extension for chained programming style
    public static class EventManagerEx
    {
        /// <summary>
        /// 使用该参数类型的实例分发事件
        /// </summary>
        /// <param name="args">参数实例</param>
        public static void Invoke(this BaseEventArgs args)
        {
            EventManager.Invoke(args);
        }
    }
    #endregion
}