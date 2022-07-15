using UnityEngine;
using BVA.Cache;
using System;
using BVA.Component;

namespace BVA
{
    public enum AssetType
    {
        StandardGLTF,
        Avatar,
        Scene,
        Common,
        Created,
        Unknown
    }
    public class AssetManager :MonoBehaviour, IDisposable
    {
        public HumanoidAvatarLoader avatarLoader { get; private set; }
        public HumanoidMotionLoader motionLoader { get; private set; }
        public AudioClipContainer audioClipContainer { get; private set; }
        public SkyboxContainer skyboxContainer { get; private set; }
        public UrlAssetContainer urlAssetContainer { get; private set; }
        public AssetCache assetCache { get; private set; }

        public void Init(AssetCache cache)
        {
            UnityEngine.Assertions.Assert.IsNotNull(cache, "AssetCache cann't be null!");
            assetCache = cache;
            avatarLoader = new HumanoidAvatarLoader(assetCache);
            motionLoader = new HumanoidMotionLoader(assetCache);
        }

        public void Dispose()
        {
            avatarLoader = null;
            motionLoader = null;
        }

        public void AddContainer(AudioClipContainer container)
        {
            audioClipContainer = container;
        }

        public void AddContainer(SkyboxContainer container)
        {
            skyboxContainer = container;
        }

        public void AddContainer(UrlAssetContainer container)
        {
            urlAssetContainer = container;
        }
    }
}