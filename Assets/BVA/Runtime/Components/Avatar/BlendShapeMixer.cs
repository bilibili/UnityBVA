using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BVA.Component
{
    [DisallowMultipleComponent]
    public class BlendShapeMixer : MonoBehaviour
    {
        //[HideInInspector]
        public List<BlendShapeKey> keys;
        //public BlendShapeMixerPreset activePreset;
        //public BlendShapeKey activeKey;
        //public BlendShapeKey lastActiveKey;
        public BlendShapeKey GetPreset(BlendShapeMixerPreset preset)
        {
            return keys[(int)preset];
        }
        //public BlendShapeKey GetActiveKey()
        //{
        //    if (keys.Count > (int)activePreset)
        //        return keys[(int)activePreset];
        //    return null;
        //}

        private IEnumerator ItorSetState(BlendShapeKey key, float value, float transitionTime)
        {
            float sTime = 0f;
            while (sTime < transitionTime)
            {
                sTime += Time.deltaTime;
                key.Set(sTime / transitionTime * value);
                yield return null;
            }
            key.Set(value);
            yield return null;
        }

        public void SetKey(BlendShapeMixerPreset preset, float value = 1.0f, float transitionTime = -1)
        {
            //lastActiveKey = activeKey;
            var key = GetPreset(preset);
            if (transitionTime <= 0)
                key.Set(value);
            else
                StartCoroutine(ItorSetState(key, value, transitionTime));
        }

        public BlendShapeMixer()
        {
            if (keys == null || keys.Count == 0)
            {
                CreateDefaultPreset();
            }
            //activePreset = BlendShapeMixerPreset.Neutral;
            //lastActiveKey = GetPreset(activePreset);
        }
        public void RemoveNullClip()
        {
            if (keys == null)
            {
                return;
            }
            for (int i = keys.Count - 1; i >= 0; --i)
            {
                if (keys[i] == null)
                {
                    keys.RemoveAt(i);
                }
            }
        }

        public void CreateDefaultPreset()
        {
            var presets = System.Enum.GetValues(typeof(BlendShapeMixerPreset)) as BlendShapeMixerPreset[];

            foreach (var preset in presets)
            {
                CreateDefaultPreset(preset);
            }
        }

        void CreateDefaultPreset(BlendShapeMixerPreset preset)
        {
            BlendShapeKey key = null;
            if (keys != null)
            {
                foreach (var c in keys)
                {
                    if (c.preset == preset)
                    {
                        key = c;
                        break;
                    }
                }
            }
            else
            {
                keys = new List<BlendShapeKey>();
            }

            if (key != null) return;

            key = new BlendShapeKey
            {
                keyName = preset.ToString(),
                preset = preset
            };
            keys.Add(key);
        }
    }
}