using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using BVA.Component;

namespace BVA.Sample
{
    public class TimelinePanel : MonoBehaviour
    {
        public Transform content;
        public List<TrackAsset> clips;

        public void Set(PlayableController track)
        {
            if (track == null) { gameObject.SetActive(false); return; }
            var element = content.GetChild(0);
            Text text = element.GetChild(0).GetComponent<Text>();
            text.text = track.name;
            Button button = element.GetChild(1).GetComponent<Button>();
            button.onClick.AddListener(track.PlayFromStart);
        }
    }
}