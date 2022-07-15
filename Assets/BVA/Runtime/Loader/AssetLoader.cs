using System;
using UnityEngine;
using BVA.Cache;

namespace BVA
{
    public abstract class AssetLoader: IDisposable
    {
        public AssetCache assetCache;
        public abstract void Dispose();

        public AssetLoader(AssetCache cache)
        {
            assetCache = cache;
        }
    }
}
