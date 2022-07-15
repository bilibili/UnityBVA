using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BVA
{
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
    public sealed class LogPool
    {
#if UNITY_EDITOR || ENABLE_DEBUG
        public const bool LOG_CONSOLE = true;
#else

        public const bool LOG_CONSOLE = false;
#endif
        private static LogPool _export;
        public static LogPool ExportLogger
        {
            get { if (_export == null) _export = new LogPool(); return _export; }
        }

        private static LogPool _import;
        public static LogPool ImportLogger
        {
            get { if (_import == null) _import = new LogPool(); return _import; }
        }

        private static LogPool _runtime;
        public static LogPool RuntimeLogger
        {
            get { if (_runtime == null) _runtime = new LogPool(); return _runtime; }
        }
        public System.Action<LogType, string> LogCallBack;
        public List<Dictionary<LogType, List<string>>> logs { private set; get; }
        public LogPool()
        {
            logs = new List<Dictionary<LogType, List<string>>>((int)LogPart.Count);
            for (int i = 0; i < (int)LogPart.Count; i++)
            {
                logs.Add(new Dictionary<LogType, List<string>>()
                {
                    {LogType.Log,new List<string>()},
                    {LogType.Warning,new List<string>()},
                    {LogType.Error,new List<string>()}
                }
                    );
            }
        }
        public void Clear()
        {
            foreach (var v in logs)
                v.Clear();

            for (int i = 0; i < (int)LogPart.Count; i++)
            {
                logs[i] = new Dictionary<LogType, List<string>>()
                {
                    {LogType.Log,new List<string>()},
                    {LogType.Warning,new List<string>()},
                    {LogType.Error,new List<string>()}
                };
            }
        }
        public void LogWarning(LogPart part, string msg)
        {
            var list = logs[(int)part][LogType.Warning];
            if (list == null) list = new List<string>();
            list.Add(msg);
#if UNITY_EDITOR || ENABLE_DEBUG
            Debug.LogWarning(msg);
#endif
        }
        public void Log(LogPart part, string msg)
        {
            var list = logs[(int)part][LogType.Log];
            if (list == null) list = new List<string>();
            list.Add(msg);
#if UNITY_EDITOR || ENABLE_DEBUG
            Debug.Log(msg);
#endif
        }
        public void LogError(LogPart part, string msg)
        {
            var list = logs[(int)part][LogType.Error];
            if (list == null) list = new List<string>();
            list.Add(msg);
#if UNITY_EDITOR || ENABLE_DEBUG
            Debug.LogError(msg);
#endif
        }
#if UNITY_EDITOR
        MessageType MsgType(LogType t)
        {
            if (t == LogType.Warning) return MessageType.Warning;
            if (t == LogType.Error) return MessageType.Error;
            if (t == LogType.Log) return MessageType.Info;
            return MessageType.None;
        }
        static bool[] _folderEditor = new bool[(int)LogPart.Count];
        public void OnGUI()
        {
            var exportLog = ExportLogger;
            for (int i = 0; i < exportLog.logs.Count; i++)
            {
                var log = logs[i];
                _folderEditor[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_folderEditor[i], ((LogPart)i).ToString());
                if (_folderEditor[i])
                {
                    foreach (var kv in log)
                    {
                        foreach (var msg in kv.Value)
                            EditorGUILayout.HelpBox(msg, MsgType(kv.Key));
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

        }
#endif
    }
}