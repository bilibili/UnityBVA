using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using GLTF.Schema;

namespace BVA.Cache
{
    /// <summary>
    /// Caches data in order to construct a unity object
    /// </summary>
    public class AssetCache : IDisposable
    {
        /// <summary>
        /// Streams to the images to be loaded
        /// </summary>
        public Stream[] ImageStreamCache { get; private set; }

        /// <summary>
        /// Loaded raw texture data
        /// </summary>
        public Texture2D[] ImageCache { get; private set; }

        /// <summary>
        /// Loaded cubemap data
        /// </summary>
        public Cubemap[] CubemapCache { get; private set; }

        /// <summary>
        /// Loaded sprite data
        /// </summary>
        public Sprite[] SpriteCache { get; private set; }

        /// <summary>
        /// Textures to be used for assets. Textures from image cache with samplers applied
        /// </summary>
        public TextureCacheData[] TextureCache { get; private set; }

        /// <summary>
        /// Cache for materials to be applied to the meshes
        /// </summary>
        public MaterialCacheData[] MaterialCache { get; private set; }

        /// <summary>
        /// Byte buffers that represent the binary contents that get parsed
        /// </summary>
        public BufferCacheData[] BufferCache { get; private set; }

        /// <summary>
        /// Cache of loaded meshes
        /// </summary>
        public MeshCacheData[] MeshCache { get; private set; }

        /// <summary>
        /// Cache of loaded animations
        /// </summary>
        public AnimationCacheData[] AnimationCache { get; private set; }

        /// <summary>
        /// Cache of loaded animatorClips
        /// </summary>
        public AnimationCacheData[] AnimatorClipCache { get; private set; }

        /// <summary>
        /// Cache of loaded node objects
        /// </summary>
        public GameObject[] NodeCache { get; private set; }

        /// <summary>
        /// Creates an asset cache which caches objects used in scene
        /// </summary>
        /// <param name="root">A glTF root whose assets will eventually be cached here</param>
        public AssetCache(GLTFRoot root)
        {
            ImageCache = new Texture2D[root.Images?.Count ?? 0];
            ImageStreamCache = new Stream[ImageCache.Length];
            TextureCache = new TextureCacheData[root.Textures?.Count ?? 0];
            MaterialCache = new MaterialCacheData[root.Materials?.Count ?? 0];
            BufferCache = new BufferCacheData[root.Buffers?.Count ?? 0];
            MeshCache = new MeshCacheData[root.Meshes?.Count ?? 0];
            NodeCache = new GameObject[root.Nodes?.Count ?? 0];
            AnimationCache = new AnimationCacheData[root.Animations?.Count ?? 0];
            CubemapCache = new Cubemap[root.Extensions?.Cubemaps?.Count ?? 0];
            SpriteCache=new Sprite[root.Extensions?.Sprites?.Count ?? 0];
            AnimatorClipCache = new AnimationCacheData[root.Extensions?.HumanoidAnimationClips?.Count ?? 0];
        }

        public void Dispose()
        {
            ImageCache = null;
            ImageStreamCache = null;
            TextureCache = null;
            CubemapCache = null;
            SpriteCache = null;
            MaterialCache = null;
            if (BufferCache != null)
            {
                foreach (BufferCacheData bufferCacheData in BufferCache)
                {
                    if (bufferCacheData != null)
                    {
                        if (bufferCacheData.Stream != null)
                        {
                            bufferCacheData.Stream.Close();
                        }

                        bufferCacheData.Dispose();
                    }
                }
                BufferCache = null;
            }

            MeshCache = null;
            AnimationCache = null;
            AnimatorClipCache = null;
        }
    }
}
