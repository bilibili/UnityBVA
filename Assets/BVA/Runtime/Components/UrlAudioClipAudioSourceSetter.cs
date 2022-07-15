using UnityEngine;
namespace BVA.Component
{
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("BVA/URL/AudioSourceSetter")]
    public class UrlAudioClipAudioSourceSetter : MonoBehaviour
    {
        public AudioUrlAsset audioUrlAsset;

        private void OnAudioLoaded(AudioClip clip)
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
        }
        private void OnEnable()
        {
            audioUrlAsset.onLoaded = OnAudioLoaded;
            StartCoroutine(audioUrlAsset.Load());
        }
    }
}