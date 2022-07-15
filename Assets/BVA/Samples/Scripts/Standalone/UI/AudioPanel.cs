using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BVA.Component;

namespace BVA.Sampler
{
    public class AudioPanel : MonoBehaviour
    {
        public Transform content;
        public AudioClipContainer clipContainer;
        AudioSource audioSource;
        List<Button> buttons;
        void Awake()
        {
            //SetAudioContainer(clipContainer);
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void SetAudioContainer(AudioClipContainer container)
        {
            if (container == null)
            {
                gameObject.SetActive(false);
                return;
            }
            buttons = new List<Button>();
            clipContainer = container;
            var clips = container.audioClips;
            var element = content.GetChild(0);
            Text text = element.GetChild(0).GetComponent<Text>();
            Button button = element.GetChild(1).GetComponent<Button>();
            buttons.Add(button);
            for (int i = 0; i < clips.Count; i++)
            {
                if (i > 0)
                {
                    element = GameObject.Instantiate(element);
                    element.name = clips[i].name;
                    element.SetParent(content, false);
                    text = element.GetChild(0).GetComponent<Text>();
                    button = element.GetChild(1).GetComponent<Button>();
                    buttons.Add(button);
                }

                text.text = clips[i].name;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                var clip = clipContainer.audioClips[i];
                buttons[i].onClick.AddListener(() =>
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                });
            }
        }
    }
}