using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BVA
{
    [System.Serializable]
    public class UrlAssetGroup
    {
        public List<AudioUrlAsset> audioUrlAssets;
        public List<ImageUrlAsset> imageUrlAssets;
        public List<VideoUrlAsset> videoUrlAssets;
        public UrlAssetGroup()
        {
            audioUrlAssets = new List<AudioUrlAsset>();
            imageUrlAssets = new List<ImageUrlAsset>();
            videoUrlAssets = new List<VideoUrlAsset>();
        }
    }
    public class UrlAssetContainer : MonoSingleton<UrlAssetContainer>
    {
        public UrlAssetGroup urlAssetGroup;
    }

}
