using UnityEditor;
using UnityEngine;

public class WindowAbout : EditorWindow
    {
       public WindowAbout()
        {
            titleContent = new GUIContent("About");
        }

        static void ShowHelp()
        {
            EditorUtility.OpenWithDefaultApp("https://github.com/bilibili/UnityBVA/blob/main/README.zh.md");
        }
        [@MenuItem("BVA/About", false, 255)]
        static void ShowAbout()
        {
            EditorWindow.GetWindowWithRect(typeof(WindowAbout),new Rect(0,0,512,512));
        }

        private Texture2D logo;

        private void OnEnable()
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/BVA/Editor/Textures/sdk_icon.png");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(logo, GUILayout.Width(360));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (EditorGUILayout.LinkButton("项目地址：https://github.com/bilibili/UnityBVA"))
                EditorUtility.OpenWithDefaultApp("https://github.com/bilibili/UnityBVA");
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(6);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Copyright © 2022 bilibili 版权所有，Apache License, Version 2.0 开源协议"))
                EditorUtility.OpenWithDefaultApp("https://bilibili.com/");
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.Width(270));
            if (GUILayout.Button("开发帮助"))
                ShowHelp();
            if (GUILayout.Button("关闭"))
                Close();
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }