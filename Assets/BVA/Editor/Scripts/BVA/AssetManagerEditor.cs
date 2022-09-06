using BVA.Component;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;
using BVA.Extensions;

namespace BVA
{
    [CustomEditor(typeof(AssetManager))]
    public class AssetManagerEditor : Editor
    {
        private AssetManager _target;
        private Animation _animation;
        private AudioSource _audioSource;

        AnimBool m_ShowAnimationFields, m_ShowAudioFields;
        private void OnEnable()
        {
            _target = target as AssetManager;
            _animation = _target.GetComponent<Animation>();

            m_ShowAnimationFields = new AnimBool(true); m_ShowAudioFields = new AnimBool(true);
            m_ShowAnimationFields.valueChanged.AddListener(Repaint); m_ShowAudioFields.valueChanged.AddListener(Repaint);
        }
        public override void OnInspectorGUI()
        {
            if (_target == null || _target.assetCache == null) return;
            if (_target.assetCache.AnimationCache != null && _target.assetCache.AnimationCache.Length > 0)
            {
                m_ShowAnimationFields.target = EditorGUILayout.ToggleLeft("Animation", m_ShowAnimationFields.target);
                if (EditorGUILayout.BeginFadeGroup(m_ShowAnimationFields.faded))
                {
                    foreach (var animationCache in _target.assetCache.AnimationCache)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (animationCache.LoadedAnimationClip)
                        {
                            EditorGUILayout.ObjectField(animationCache.LoadedAnimationClip, typeof(AnimationClip), false);
                            if (GUILayout.Button("Play"))
                            {
                                if (_animation)
                                {
                                    _animation.Play(animationCache.LoadedAnimationClip.name);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            EditorGUILayout.EndFadeGroup();

            if (_target.audioClipContainer)
            {
                m_ShowAudioFields.target = EditorGUILayout.ToggleLeft("Audio", m_ShowAudioFields.target);
                if (EditorGUILayout.BeginFadeGroup(m_ShowAudioFields.faded))
                {
                    foreach (var audioClip in _target.audioClipContainer.audioClips)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (audioClip)
                        {
                            EditorGUILayout.ObjectField(audioClip, typeof(AudioClip), false);
                            if (GUILayout.Button("Play"))
                            {
                                if (_audioSource == null)
                                {
                                    _audioSource = _target.gameObject.GetOrAddComponent<AudioSource>();
                                }
                                _audioSource.clip = audioClip;
                                _audioSource.Play();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.EndFadeGroup();
            }
        }
    }
}