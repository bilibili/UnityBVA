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
        private static readonly EditorSettingsValidator Validator = new EditorSettingsValidator();
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
            if (EditorApplication.isPlaying || EditorApplication.isCompiling) return;
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
            ExportCommon.ShowLanguageSwitchButton(); ;
            if (Validator.IsValid())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(ExportCommon.Localization("所有问题已经被修复了\n谢谢", "All setting is vaild\n Thankyou!"), new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 20,
                });

                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label(ExportCommon.Localization("可以关闭该窗口", "You Can Close This Window"));
                if (GUILayout.Button(ExportCommon.Localization("关闭", "Close")))
                {
                    Close();
                }

                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Space(5);
                GUILayout.Label(ExportCommon.Localization("设置BVA必备选项", "Recommended ProjectSettings For BVA"));
                GUILayout.Space(5);
                GUILayout.EndVertical();

                GUILayout.Space(10);

                foreach (var validator in Validator.Validators)
                {
                    if (validator.CanFix)
                    {

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(validator.HeaderDescription);
                        var oldEnabled = GUI.enabled;
                        GUI.enabled = !validator.IsValid;


                        if (GUILayout.Button(validator.RecommendedValueDescription))
                        {
                            validator.Validate();
                        }

                        GUI.enabled = oldEnabled;
                        GUILayout.EndHorizontal();

                        GUILayout.Space(5);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox(validator.HeaderDescription, MessageType.Warning);
                    }
                }

                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical(GUI.skin.box);
                if (GUILayout.Button(ExportCommon.Localization("修复全部", "Fix All")))
                {
                    foreach (var validator in Validator.Validators)
                    {
                        if (!validator.IsValid)
                        {
                            validator.Validate();
                        }
                    }
                }

                GUILayout.EndVertical();
            }
        }
    }
}