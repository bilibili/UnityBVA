using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BVA.Component;
namespace BVA.Sampler
{
    public class BlendshapeMixerPanel : MonoBehaviour
    {
        public Transform content;
        Transform prefab;
        List<Text> names;
        List<Slider> sliders;
        public BlendShapeMixer mixer;
        void Awake()
        {
            names = new List<Text>();
            sliders = new List<Slider>();
            prefab = content.GetChild(0);
            for (int i = 0; i < (int)BlendShapeMixerPreset.Custom - 1; i++)
            {
                if (i == 0)
                {
                    var text = prefab.GetChild(0).GetComponent<Text>();
                    text.text = ((BlendShapeMixerPreset)i).ToString();
                    names.Add(text);
                    sliders.Add(prefab.GetChild(1).GetComponent<Slider>());
                }
                else
                {
                    var inst = GameObject.Instantiate(prefab);
                    inst.name = ((BlendShapeMixerPreset)i).ToString();
                    inst.SetParent(content, false);
                    var text = inst.GetChild(0).GetComponent<Text>();
                    text.text = inst.name;
                    names.Add(text);
                    sliders.Add(inst.GetChild(1).GetComponent<Slider>());
                }
            }
            //SetBlendShapeMixer(mixer);
        }

        public void SetBlendShapeMixer(BlendShapeMixer _mixer)
        {
            mixer = _mixer;
            if (mixer == null) Debug.LogError("BlendShapeMixer is null");
            for (int i = 0; i < (int)BlendShapeMixerPreset.Custom - 1; i++)
            {
                var key = mixer.GetPreset((BlendShapeMixerPreset)i);
                if (key != null)
                    sliders[i].onValueChanged.AddListener(key.Set);
                else
                    Debug.LogError(((BlendShapeMixerPreset)i).ToString() + " key is null");
            }
        }
    }
}