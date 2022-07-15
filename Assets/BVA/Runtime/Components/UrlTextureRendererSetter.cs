using UnityEngine;

namespace BVA.Component
{
    [RequireComponent(typeof(Renderer))]
    [AddComponentMenu("BVA/URL/TextureRendererSetter")]
    public class UrlTextureRendererSetter : MonoBehaviour
    {
        public ImageUrlAsset imageUrlAsset;
        public int materialIndex;
        public string propertyName;

        private void OnTextureLoaded(Texture2D texture)
        {
            var renderer = GetComponent<Renderer>();
            if (string.IsNullOrEmpty(propertyName) && materialIndex == 0)
                renderer.material.mainTexture = imageUrlAsset.resource;
            else
                renderer.materials[materialIndex].SetTexture(propertyName, imageUrlAsset.resource);
        }
        private void OnEnable()
        {
            imageUrlAsset.onLoaded = OnTextureLoaded;
            StartCoroutine(imageUrlAsset.Load());
        }
    }
}