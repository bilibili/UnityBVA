using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;

namespace BVA
{
    public class ProjectConfigurationWindow : EditorWindow, IActiveBuildTargetChanged, IPreprocessBuildWithReport
    {
        [MenuItem("BVA/Configure Project", false, 499)]
        private static void ShowWindowFromMenu()
        {
            ShowWindow();
        }
        private const float Default_Window_Height = 800.0f;
        private const float Default_Window_Width = 600.0f;
        public static ProjectConfigurationWindow Instance { get; private set; }

        public static bool IsOpen => Instance != null;

        public int callbackOrder => 0;
        public static List<string> logConfigurationWarnings;
        private void OnEnable()
        {
            Instance = this;
            EditorApplication.projectChanged += ResetNullableBoolState;
            CompilationPipeline.compilationStarted += CompilationPipeline_compilationStarted;
        }
        static ProjectConfigurationWindow()
        {
            // Detect when we enter player mode so we can try checking for optimal configuration
            EditorApplication.playModeStateChanged += OnPlayStateModeChanged;

            // Subscribe to editor application update which will call us once the editor is initialized and running
            EditorApplication.update += OnInit;
        }

        private static void OnInit()
        {
            // We only want to execute once to initialize, unsubscribe from update event
            EditorApplication.update -= OnInit;

            ShowWindow();
        }
        private void CompilationPipeline_compilationStarted(object obj)
        {
            ResetNullableBoolState();
        }
        private void ResetNullableBoolState()
        {
        }

        private static void ShowWindow()
        {
            // There should be only one configurator window open as a "pop-up". If already open, then just force focus on our instance
            if (IsOpen)
            {
                Instance.ShowUtility();
            }
            else
            {
                var window = CreateInstance<ProjectConfigurationWindow>();
                window.titleContent = new GUIContent("BVA Project Configuration", EditorGUIUtility.IconContent("_Popup").image);
                window.position = new Rect(Screen.width / 2.0f, Screen.height / 2.0f, Default_Window_Height, Default_Window_Width);
                window.ShowUtility();
            }
        }

        private static void OnPlayStateModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                LogConfigurationWarnings();
            }
        }

        private static void LogConfigurationWarnings()
        {
            logConfigurationWarnings = new List<string>();
            logConfigurationWarnings.AddRange(UnityTools.ValidateRenderPipelineSettings());
            logConfigurationWarnings.AddRange(UnityTools.ValidateColorSpaceSettings());
            logConfigurationWarnings.AddRange(UnityTools.ValidateLightmapSettings());
            logConfigurationWarnings.AddRange(UnityTools.ValidatePlayerSettings());
            //UnityTools.ValidateReflectionProbeSettings();
        }

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            LogConfigurationWarnings();
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            string[] infos = BVAConfig.CheckProjectEnvironment();
            if (infos.Length > 0)
            {
                if (EditorUtility.DisplayDialog("project environment is not setup", string.Join("\n", infos), "OK", "Cancel"))
                {
                    BVAConfig.FixProjectEnvironment();
                }
            }
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Validate Settings"))
                {
                    LogConfigurationWarnings();
                }
                if (GUILayout.Button("Fix Settings"))
                {
                    UnityTools.FixDefaultProjectSetttings();
                }
            }
            EditorGUILayout.EndHorizontal();
            if (logConfigurationWarnings != null)
            {
                foreach (var log in logConfigurationWarnings)
                {
                    EditorGUILayout.HelpBox(log, MessageType.Warning);
                }
            }
        }
    }
}