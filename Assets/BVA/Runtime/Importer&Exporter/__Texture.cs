using GLTF.Schema;
using GLTF.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using BVA.Cache;
using GLTF.Schema.BVA;
using BVA.Extensions;
using Unity.Collections;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using WrapMode = GLTF.Schema.WrapMode;

namespace BVA
{
    public enum ImageMimeType
    {
        COMMON,
        KTX,
        BASIS
    }
    public partial class GLTFSceneImporter
    {
        protected async Task<Cubemap> LoadCubemap(CubemapId id)
        {
            var ext = _gltfRoot.Extensions.Cubemaps[id.Id];
            int cubemapIndex = id.Id;
            Cubemap cubemap = _assetCache.CubemapCache[cubemapIndex];
            if (cubemap != null)
            {
                return cubemap;
            }
            return await ConstructCubemap(id, ext.texture, ext.mipmap);
        }

        protected async Task<Cubemap> ConstructCubemap(CubemapId cubemapId, TextureId textureId, bool mipmap)
        {
            await ConstructUnityCubemap(cubemapId, textureId, mipmap);
            return _assetCache.CubemapCache[cubemapId.Id];
        }

        protected async Task ConstructUnityCubemap(CubemapId cubemapId, TextureId textureId, bool mipmap)
        {
            await ConstructImageBuffer(textureId.Value, textureId.Id);
            await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
            Texture2D texture = _assetCache.TextureCache[textureId.Id].Texture as Texture2D;
            string name = texture.name;
            Cubemap cubemap = CubemapExtensions.CreateCubemapFromTexture(texture, mipmap);
            cubemap.name = name;
            if (Application.isEditor)
                Object.DestroyImmediate(texture);
            else
                Object.Destroy(texture);
            _assetCache.CubemapCache[cubemapId.Id] = cubemap;
        }
        protected async Task<Texture2D> GetLightmap(TextureId textureId)
        {
            await ConstructImageBuffer(textureId.Value, textureId.Id);
            await ConstructTexture(textureId.Value, textureId.Id, true, true);
            var texture = _assetCache.TextureCache[textureId.Id].Texture;
            //if (lightmapEncoding == BVA_light_lightmapExtension.LightmapEncoding.None)
            //    return texture as Texture2D;
            var destRenderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);

            //GL.sRGBWrite = false;
            var encodeLightmapShader = Resources.Load<Shader>("EncodeLightmap");
            var _lightmapEncodeMaterial = new Material(encodeLightmapShader);
            _lightmapEncodeMaterial.SetInt("_MODE", 2);
            Graphics.Blit(texture, destRenderTexture, _lightmapEncodeMaterial);

            var exportTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBAHalf, false, true);
            exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
            exportTexture.Apply();

            RenderTexture.ReleaseTemporary(destRenderTexture);
            _assetCache.TextureCache[textureId.Id].Texture = exportTexture;
            return exportTexture;
        }
        protected async Task<Texture2D> GetTextureLinear2Gamma(TextureId textureId)
        {
            await ConstructImageBuffer(textureId.Value, textureId.Id);
            await ConstructTexture(textureId.Value, textureId.Id, true, false);
            var texture = _assetCache.TextureCache[textureId.Id].Texture;
            //if (lightmapEncoding == BVA_light_lightmapExtension.LightmapEncoding.None)
            //    return texture as Texture2D;
            var destRenderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);

            //GL.sRGBWrite = false;
            var encodeLightmapShader = Resources.Load<Shader>("EncodeLightmap");
            var _lightmapEncodeMaterial = new Material(encodeLightmapShader);
            _lightmapEncodeMaterial.SetInt("_MODE", 5);
            Graphics.Blit(texture, destRenderTexture, _lightmapEncodeMaterial);

            var exportTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBAHalf, false, true);
            exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
            exportTexture.Apply();

            RenderTexture.ReleaseTemporary(destRenderTexture);
            _assetCache.TextureCache[textureId.Id].Texture = exportTexture;
            return exportTexture;
        }
        protected ImageMimeType GetImageMimeType(GLTFImage image)
        {
            if (!string.IsNullOrEmpty(image.Uri))
            {
                string extension = image.Uri.ToLower();
                if (extension.EndsWith("ktx") || extension.EndsWith("ktx2"))
                    return ImageMimeType.KTX;

                if (extension.EndsWith("basis") || extension.EndsWith("basisu"))
                    return ImageMimeType.BASIS;
            }
            if (image.MimeType == "image/ktx2" || image.MimeType == "image/ktx")
                return ImageMimeType.KTX;
            if (image.MimeType == "image/basis" || image.MimeType == "image/basisu")
                return ImageMimeType.BASIS;
            return ImageMimeType.COMMON;
        }
        protected async Task ConstructImage(GLTFImage image, int imageCacheIndex, bool markGpuOnly, bool isLinear, bool mipmap, TextureWrapMode wrapMode, FilterMode filterMode)
        {
            if (_assetCache.ImageCache[imageCacheIndex] == null)
            {
                Stream stream;
                ImageMimeType mimeType = GetImageMimeType(image);
                if (image.Uri == null)
                {
                    var bufferView = image.BufferView.Value;
                    var data = new byte[bufferView.ByteLength];
                    BufferCacheData bufferContents = _assetCache.BufferCache[bufferView.Buffer.Id];
                    bufferContents.Stream.Position = bufferView.ByteOffset + bufferContents.ChunkOffset;
                    stream = new SubStream(bufferContents.Stream, 0, data.Length);
                }
                else
                {
                    string uri = image.Uri;

                    URIHelper.TryParseBase64(uri, out byte[] bufferData);
                    if (bufferData != null)
                    {
                        stream = new MemoryStream(bufferData, 0, bufferData.Length, false, true);
                    }
                    else
                    {
                        stream = _assetCache.ImageStreamCache[imageCacheIndex];
                    }
                }
                await ConstructUnityTexture(mimeType, stream, markGpuOnly, isLinear, mipmap, wrapMode, filterMode, image, imageCacheIndex);
            }
        }

        protected async Task ConstructUnityTexture(ImageMimeType mimeType, Stream stream, bool markGpuOnly, bool isLinear, bool mipmap, TextureWrapMode wrapMode, FilterMode filterMode, GLTFImage image, int imageCacheIndex)
        {
            if (mimeType == ImageMimeType.KTX)
            {
                // Create KTX texture instance
                var ktxTexture = new KtxUnity.KtxTexture();

                // Linear color sampling. Needed for non-color value textures (e.g. normal maps) 
                bool linearColor = true;
                if (stream is MemoryStream)
                {
                    using (MemoryStream memoryStream = stream as MemoryStream)
                    {
                        NativeArray<byte> buffer = new NativeArray<byte>(memoryStream.GetBuffer(), Allocator.Persistent);
                        var result = await ktxTexture.LoadFromBytes(buffer, linearColor);
                        _assetCache.ImageCache[imageCacheIndex] = result.texture;
                    }
                }
                else
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    NativeArray<byte> buffer = new NativeArray<byte>(bytes, Allocator.Persistent);

                    var result = await ktxTexture.LoadFromBytes(buffer, linearColor);
                    // NOTE: the second parameter of LoadImage() marks non-readable, but we can't mark it until after we call Apply()
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        markGpuOnly = false;
#endif
                    _assetCache.ImageCache[imageCacheIndex] = result.texture;
                }
                return;
            }
            if (mimeType == ImageMimeType.BASIS)
            {
                // Create KTX texture instance
                var basisTexture = new KtxUnity.BasisUniversalTexture();

                // Linear color sampling. Needed for non-color value textures (e.g. normal maps) 
                bool linearColor = true;
                if (stream is MemoryStream)
                {
                    using (MemoryStream memoryStream = stream as MemoryStream)
                    {
                        NativeArray<byte> buffer = new NativeArray<byte>(memoryStream.GetBuffer(), Allocator.Persistent);
                        var result = await basisTexture.LoadFromBytes(buffer, linearColor);
                        _assetCache.ImageCache[imageCacheIndex] = result.texture;
                    }
                }
                else
                {
                    byte[] bytes = new byte[stream.Length];

                    stream.Read(bytes, 0, (int)stream.Length);
                    NativeArray<byte> buffer = new NativeArray<byte>(bytes, Allocator.Persistent);

                    var result = await basisTexture.LoadFromBytes(buffer, linearColor);
                    // NOTE: the second parameter of LoadImage() marks non-readable, but we can't mark it until after we call Apply()

                    _assetCache.ImageCache[imageCacheIndex] = result.texture;
                }
                return;
            }
            //Texture2D texture = isLinear ? new Texture2D(0, 0, UnityEngine.Experimental.Rendering.DefaultFormat.LDR, UnityEngine.Experimental.Rendering.TextureCreationFlags.None) :
            //new Texture2D(0, 0, UnityEngine.Experimental.Rendering.DefaultFormat.LDR, UnityEngine.Experimental.Rendering.TextureCreationFlags.None);
            Texture2D texture = new Texture2D(0, 0, TextureFormat.ARGB32, mipmap && _options.GenerateMipMapsForTextures, isLinear);
            texture.name = image.Name ?? "No name";
            texture.filterMode = filterMode;
            texture.wrapMode = wrapMode;

            //NOTE: the second parameter of LoadImage() marks non-readable, but we can't mark it until after we call Apply()
#if UNITY_EDITOR
            if (!Application.isPlaying && Application.isEditor)
                markGpuOnly = false;
#endif
            if (stream is MemoryStream)
            {
                using (MemoryStream memoryStream = stream as MemoryStream)
                {
#if UNITY_WEBGL
                    texture.LoadImage(memoryStream.ToArray(), markGpuOnly);
#else
                    texture.LoadImage(memoryStream.GetBuffer(), markGpuOnly);
#endif              
                }
            }
            else
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);

                texture.LoadImage(buffer, markGpuOnly);
            }

            Debug.Assert(_assetCache.ImageCache[imageCacheIndex] == null, "ImageCache should not be loaded multiple times");
            progressStatus.TextureLoaded++;
            progress?.Report(progressStatus);
            _assetCache.ImageCache[imageCacheIndex] = texture;
        }

        protected async Task ConstructImageBuffer(GLTFTexture texture, int textureIndex)
        {
            int sourceId = GetTextureSourceId(texture);
            if (_assetCache.ImageStreamCache[sourceId] == null)
            {
                GLTFImage image = _gltfRoot.Images[sourceId];

                // we only load the streams if not a base64 uri, meaning the data is in the uri
                if (image.Uri != null && !URIHelper.IsBase64Uri(image.Uri))
                {
                    _assetCache.ImageStreamCache[sourceId] = await _options.DataLoader.LoadStreamAsync(image.Uri);
                }
                else if (image.Uri == null && image.BufferView != null && _assetCache.BufferCache[image.BufferView.Value.Buffer.Id] == null)
                {
                    int bufferIndex = image.BufferView.Value.Buffer.Id;
                    await ConstructBuffer(_gltfRoot.Buffers[bufferIndex], bufferIndex);
                }
            }

            if (_assetCache.TextureCache[textureIndex] == null)
            {
                _assetCache.TextureCache[textureIndex] = new TextureCacheData
                {
                    TextureDefinition = texture
                };
                ++Statistics.TextureCount;
            }
        }

        protected async Task ConstructTexture(GLTFTexture texture, int textureIndex, bool markGpuOnly, bool isLinear = false)
        {
            if (_assetCache.TextureCache[textureIndex].Texture == null)
            {
                int sourceId = GetTextureSourceId(texture);
                GLTFImage image = _gltfRoot.Images[sourceId];
                TextureWrapMode desiredWrapMode = TextureWrapMode.Repeat;
                FilterMode desiredFilterMode = FilterMode.Bilinear;
                bool mipmap = false;
                if (texture.Sampler != null)
                {
                    var sampler = texture.Sampler.Value;
                    switch (sampler.MinFilter)
                    {
                        case MinFilterMode.Nearest:
                            desiredFilterMode = FilterMode.Point;
                            break;
                        case MinFilterMode.NearestMipmapNearest:
                            desiredFilterMode = FilterMode.Point;
                            mipmap = true;
                            break;
                        case MinFilterMode.Linear:
                            desiredFilterMode = FilterMode.Bilinear;
                            break;
                        case MinFilterMode.NearestMipmapLinear:
                            desiredFilterMode = FilterMode.Bilinear;
                            mipmap = true;
                            break;
                        case MinFilterMode.LinearMipmapNearest:
                            desiredFilterMode = FilterMode.Bilinear;
                            mipmap = true;
                            break;
                        case MinFilterMode.LinearMipmapLinear:
                            desiredFilterMode = FilterMode.Trilinear;
                            mipmap = true;
                            break;
                        default:
                            LogPool.ImportLogger.LogWarning(LogPart.Texture, "Unsupported Sampler.MinFilter: " + sampler.MinFilter);
                            break;
                    }

                    switch (sampler.WrapS)
                    {
                        case WrapMode.ClampToEdge:
                            desiredWrapMode = TextureWrapMode.Clamp;
                            break;
                        case WrapMode.Repeat:
                            desiredWrapMode = TextureWrapMode.Repeat;
                            break;
                        case WrapMode.MirroredRepeat:
                            desiredWrapMode = TextureWrapMode.Mirror;
                            break;
                        default:
                            LogPool.ImportLogger.LogWarning(LogPart.Texture, "Unsupported Sampler.WrapS: " + sampler.WrapS);
                            break;
                    }
                }
                else
                {
                    desiredFilterMode = FilterMode.Bilinear;
                    desiredWrapMode = TextureWrapMode.Repeat;
                }

                await ConstructImage(image, sourceId, markGpuOnly, isLinear, mipmap, desiredWrapMode, desiredFilterMode);
                var unityTexture = _assetCache.ImageCache[sourceId];
                _assetCache.TextureCache[textureIndex].Texture = unityTexture;
            }
        }

        protected KHR_texture_transformExtension GetTextureTransform(TextureInfo def)
        {
            if (_gltfRoot.ExtensionsUsed != null &&
                _gltfRoot.ExtensionsUsed.Contains(KHR_texture_transformExtensionFactory.EXTENSION_NAME) &&
                def.Extensions != null &&
                def.Extensions.TryGetValue(KHR_texture_transformExtensionFactory.EXTENSION_NAME, out IExtension extension))
            {
                return (KHR_texture_transformExtension)extension;
            }
            else return null;
        }
        protected KHR_texture_basisuExtension GetKtxTextureSourceId(GLTFTexture def)
        {
            if (_gltfRoot.ExtensionsUsed != null &&
                _gltfRoot.ExtensionsUsed.Contains(KHR_texture_basisuExtensionFactory.EXTENSION_NAME) &&
                def.Extensions != null &&
                def.Extensions.TryGetValue(KHR_texture_basisuExtensionFactory.EXTENSION_NAME, out IExtension extension))
            {
                return (KHR_texture_basisuExtension)extension;
            }
            else return null;
        }
        protected int GetTextureSourceId(GLTFTexture texture)
        {
            if (texture.Source != null)
                return texture.Source.Id;

            KHR_texture_basisuExtension ext = GetKtxTextureSourceId(texture);
            return ext.Source;
        }

        /// <summary>
        /// Gets texture that has been loaded from CreateTexture
        /// </summary>
        /// <param name="textureIndex">The texture to get</param>
        /// <returns>Created texture</returns>
        public async Task<Texture> GetTexture(TextureId textureId)
        {
            if (_assetCache.TextureCache[textureId.Id] == null)
            {
                await ConstructImageBuffer(textureId.Value, textureId.Id);
                await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture);
            }

            return _assetCache.TextureCache[textureId.Id].Texture;
        }
        public async Task<Texture> GetTextureAsLinear(TextureId textureId)
        {
            if (_assetCache.TextureCache[textureId.Id] == null)
            {
                await ConstructImageBuffer(textureId.Value, textureId.Id);
                await ConstructTexture(textureId.Value, textureId.Id, !_options.KeepCPUCopyOfTexture, true);
            }

            return _assetCache.TextureCache[textureId.Id].Texture;
        }
    }
    public partial class GLTFSceneExporter
    {
#if UNITY_EDITOR
        static readonly string[] supportedEditorOriginalFormat = new string[] { ".png", ".jpg", ".jpeg" };
        private bool ExportOriginalTextureFileFail(Texture image, string outputPath)
        {
            if (ExportOriginalTextureFile)
            {
                var path = AssetDatabase.GetAssetPath(image);
                if (path != null)
                {
                    bool isSupportedFormat = supportedEditorOriginalFormat.Any(x => path.EndsWith(x));
                    if (!isSupportedFormat)
                        return true;
                    else
                    {
                        File.Copy(path, ConstructImageFilenamePath(image, outputPath), true);
                        return false;
                    }
                }
            }
            return true;
        }
#endif
        private void ExportImages(string outputPath)
        {
            for (int t = 0; t < _imageInfos.Count; ++t)
            {
                var image = _imageInfos[t].texture;

                switch (_imageInfos[t].textureMapType)
                {
                    case TextureMapType.MetallicGloss:
                        ExportMetallicGlossTexture(image, outputPath);
                        break;
                    case TextureMapType.Bump:
#if UNITY_EDITOR
                        if (ExportOriginalTextureFileFail(image, outputPath))
                            ExportNormalTexture(image, outputPath);
#else
                        ExportNormalTexture(image, outputPath);
#endif
                        break;
                    case TextureMapType.LightMapDir:
                        ExportLightmapTexture(image, outputPath);
                        break;
                    default:
#if UNITY_EDITOR
                        if (ExportOriginalTextureFileFail(image, outputPath))
                            ExportTexture(image, outputPath);
#else
                        ExportTexture(image, outputPath);
#endif
                        break;
                }
            }
        }

        private void ExportLightmapTexture(Texture texture, string outputPath)
        {
            var destRenderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            Graphics.Blit(texture, destRenderTexture, UnityExtensions.GetLightmapEncodeMaterial(LightmapEncodingMode.EncodeRGBM));

            var exportTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false, false);
            exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
            exportTexture.Apply();

            var finalFilenamePath = ConstructImageFilenamePath(texture, outputPath);
            File.WriteAllBytes(finalFilenamePath, exportTexture.EncodeToPNG());

            RenderTexture.ReleaseTemporary(destRenderTexture);
            if (Application.isEditor)
            {
                GameObject.DestroyImmediate(exportTexture);
            }
            else
            {
                GameObject.Destroy(exportTexture);
            }
        }
        /// <summary>
        /// This converts Unity's metallic-gloss texture representation into GLTF's metallic-roughness specifications.
        /// Unity's metallic-gloss A channel (glossiness) is inverted and goes into GLTF's metallic-roughness G channel (roughness).
        /// Unity's metallic-gloss R channel (metallic) goes into GLTF's metallic-roughess B channel.
        /// </summary>
        /// <param name="texture">Unity's metallic-gloss texture to be exported</param>
        /// <param name="outputPath">The location to export the texture</param>
        private void ExportMetallicGlossTexture(Texture2D texture, string outputPath)
        {
            var destRenderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

            Graphics.Blit(texture, destRenderTexture, UnityExtensions.MetalGlossChannelSwapMaterial);

            var exportTexture = new Texture2D(texture.width, texture.height);
            exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
            exportTexture.Apply();

            var finalFilenamePath = ConstructImageFilenamePath(texture, outputPath);
            File.WriteAllBytes(finalFilenamePath, exportTexture.EncodeToPNG());

            RenderTexture.ReleaseTemporary(destRenderTexture);
            if (Application.isEditor)
            {
                GameObject.DestroyImmediate(exportTexture);
            }
            else
            {
                GameObject.Destroy(exportTexture);
            }
        }

        /// <summary>
        /// This export's the normal texture. If a texture is marked as a normal map, the values are stored in the A and G channel.
        /// To output the correct normal texture, the A channel is put into the R channel.
        /// </summary>
        /// <param name="texture">Unity's normal texture to be exported</param>
        /// <param name="outputPath">The location to export the texture</param>
        private void ExportNormalTexture(Texture2D texture, string outputPath)
        {
            var destRenderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

            Graphics.Blit(texture, destRenderTexture, UnityExtensions.NormalChannelMaterial);

            var exportTexture = new Texture2D(texture.width, texture.height);
            exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
            exportTexture.Apply();

            var finalFilenamePath = ConstructImageFilenamePath(texture, outputPath);
            File.WriteAllBytes(finalFilenamePath, exportTexture.EncodeToPNG());

            RenderTexture.ReleaseTemporary(destRenderTexture);
            if (Application.isEditor)
            {
                GameObject.DestroyImmediate(exportTexture);
            }
            else
            {
                GameObject.Destroy(exportTexture);
            }
        }
        private void ExportTexture(Texture2D texture, string outputPath)
        {
            var destRenderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);

            Graphics.Blit(texture, destRenderTexture);

            var exportTexture = new Texture2D(texture.width, texture.height);
            exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
            exportTexture.Apply();

            var finalFilenamePath = ConstructImageFilenamePath(texture, outputPath);
            File.WriteAllBytes(finalFilenamePath, exportTexture.EncodeToPNG());

            RenderTexture.ReleaseTemporary(destRenderTexture);
            if (Application.isEditor)
            {
                GameObject.DestroyImmediate(exportTexture);
            }
            else
            {
                GameObject.Destroy(exportTexture);
            }
        }

        private string ConstructImageFilenamePath(Texture texture, string outputPath)
        {
            var imagePath = _exportOptions.TexturePathRetriever(texture);
            if (string.IsNullOrEmpty(imagePath))
            {
                imagePath = Path.Combine(outputPath, texture.name);
            }

            var filenamePath = Path.Combine(outputPath, imagePath);
            if (!ExportFullPath)
            {
                filenamePath = outputPath + "/" + texture.name;
            }
            var file = new FileInfo(filenamePath);
            file.Directory.Create();
            return Path.ChangeExtension(filenamePath, ".png");
        }

        private void ExportTextureTransform(TextureInfo def, Material mat, string texName)
        {
            Vector2 offset = mat.GetTextureOffset(texName);
            Vector2 scale = mat.GetTextureScale(texName);

            if (offset == Vector2.zero && scale == Vector2.one) return;

            if (_root.ExtensionsUsed == null)
            {
                _root.ExtensionsUsed = new List<string>(
                    new[] { KHR_texture_transformExtensionFactory.EXTENSION_NAME }
                );
            }
            else if (!_root.ExtensionsUsed.Contains(KHR_texture_transformExtensionFactory.EXTENSION_NAME))
            {
                _root.ExtensionsUsed.Add(KHR_texture_transformExtensionFactory.EXTENSION_NAME);
            }

            if (RequireExtensions)
            {
                if (_root.ExtensionsRequired == null)
                {
                    _root.ExtensionsRequired = new List<string>(
                        new[] { KHR_texture_transformExtensionFactory.EXTENSION_NAME }
                    );
                }
                else if (!_root.ExtensionsRequired.Contains(KHR_texture_transformExtensionFactory.EXTENSION_NAME))
                {
                    _root.ExtensionsRequired.Add(KHR_texture_transformExtensionFactory.EXTENSION_NAME);
                }
            }

            if (def.Extensions == null)
                def.Extensions = new Dictionary<string, IExtension>();

            def.Extensions[KHR_texture_transformExtensionFactory.EXTENSION_NAME] = new KHR_texture_transformExtension(
                new Vector2(offset.x, -offset.y),
                0,
                new Vector2(scale.x, scale.y),
                0
            );
        }

        private NormalTextureInfo ExportNormalTextureInfo(Texture texture, Material material)
        {
            var info = new NormalTextureInfo() { Scale = 1.0f };

            info.Index = ExportTexture(texture, TextureMapType.Bump);

            if (material.HasProperty("_BumpScale"))
            {
                info.Scale = material.GetFloat("_BumpScale");
            }
            if (material.HasProperty("_NormalScale"))
            {
                info.Scale = material.GetFloat("_NormalScale");
            }
            return info;
        }

        private OcclusionTextureInfo ExportOcclusionTextureInfo(Texture texture, TextureMapType textureMapType, Material material)
        {
            var info = new OcclusionTextureInfo();

            info.Index = ExportTexture(texture, textureMapType);

            if (material.HasProperty("_OcclusionStrength"))
            {
                info.Strength = material.GetFloat("_OcclusionStrength");
            }

            return info;
        }

        /// <summary>
        /// export Texture2D or Cubemap
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        private TextureInfo ExportTextureInfo(Texture texture)
        {
            return ExportTextureInfo(texture, TextureMapType.Main);
        }

        /// <summary>
        /// export Texture2D normalMap
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        private TextureInfo ExportNormalTextureInfo(Texture texture)
        {
            return ExportTextureInfo(texture, TextureMapType.Bump);
        }

        private TextureInfo ExportTextureInfo(Texture texture, TextureMapType textureMapType)
        {
            var info = new TextureInfo();

            info.Index = ExportTexture(texture, textureMapType);

            return info;
        }

        private TextureId ExportTexture(Texture textureObj)
        {
            return ExportTexture(textureObj, TextureMapType.Main);
        }

        public CubemapId GetCubemapId(GLTFRoot root, Cubemap cubemapObj)
        {
            for (var i = 0; i < _cubemaps.Count; i++)
            {
                if (_cubemaps[i] == cubemapObj)
                {
                    return new CubemapId
                    {
                        Id = i,
                        Root = root
                    };
                }
            }
            return null;
        }
        private CubemapId ExportCubemap(Cubemap textureObj) { return ExportCubemap(textureObj, CubemapImageType.Row, textureObj.mipmapCount > 0); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureObj"></param>
        /// <param name="cubemapImageType"></param>
        /// <param name="textureId"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private CubemapId ExportCubemap(Cubemap textureObj, CubemapImageType cubemapImageType, bool mipmap)
        {
#pragma warning disable CS0162
#if UNITY_ANDROID || UNITY_IOS
            throw new EditorExportOnlyException("Please switch to Standalone from Build Setting!");
#endif
            if (textureObj == null)
                return null;
            CubemapId id = GetCubemapId(_root, textureObj);
            if (id != null)
                return id;
            TextureId textureId = null;
#if UNITY_EDITOR
            if (ExportOriginalTextureFile)
            {
                var assetPath = AssetDatabase.GetAssetPath(textureObj);
                if (assetPath != null)
                {
                    var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
#if UNITY_2021_1_OR_NEWER
                    importer.GetSourceTextureWidthAndHeight(out int width, out int height);
#else
                    System.Type ty = typeof(TextureImporter);
                    var m = ty.GetMethod("GetWidthAndHeight");
                    int width = 0, height = 0;
                    m.Invoke(importer, new object[] { width, height });
#endif
                    cubemapImageType = CubemapExtensions.GetCubemapImageType(width, height);
                    if (cubemapImageType == CubemapImageType.Row || cubemapImageType == CubemapImageType.Column)
                    {
                        if (supportedEditorOriginalFormat.Any(x => assetPath.EndsWith(x)))
                        {
                            Texture2D texture = new Texture2D(0, 0);
                            texture.name = Path.GetFileNameWithoutExtension(assetPath);
                            texture.LoadImage(File.ReadAllBytes(assetPath));
                            textureId = ExportTexture(texture, TextureMapType.Main);
                        }
                    }
                }
                if (textureId == null)
                {
                    cubemapImageType = CubemapImageType.Row;
                    Texture2D flated = cubemapImageType == CubemapImageType.Column ? textureObj.FlatTexture(true) : textureObj.FlatTexture(false);
                    textureId = ExportTexture(flated, TextureMapType.Main);
                }
            }
            else
            {
                Texture2D flated = cubemapImageType == CubemapImageType.Row ? textureObj.FlatTexture() : textureObj.FlatTexture(true);
                textureId = ExportTexture(flated, TextureMapType.Main);
            }
#else
            Texture2D flated = cubemapImageType == CubemapImageType.Row ? textureObj.FlatTexture() : textureObj.FlatTexture(true);
            textureId = ExportTexture(flated, TextureMapType.Main);
#endif
            BVA_texture_cubemapExtension ext = new BVA_texture_cubemapExtension(textureId, cubemapImageType, mipmap);
            _root.AddExtension(_root, BVA_texture_cubemapExtensionFactory.EXTENSION_NAME, null, RequireExtensions);

            id = new CubemapId
            {
                Id = _root.Extensions.Cubemaps.Count,
                Root = _root
            };
            _cubemaps.Add(textureObj);
            _root.Extensions.AddCubemap(ext);
            return id;
        }

        private TextureId ExportTexture(Texture textureObj, TextureMapType textureMapType)
        {
            TextureId id = GetTextureId(_root, textureObj);
            if (id != null)
            {
                return id;
            }

            var texture = new GLTFTexture();

            //If texture name not set give it a unique name using count
            if (textureObj.name == "")
            {
                textureObj.name = (_root.Textures.Count + 1).ToString();
            }

            if (ExportNames)
            {
                texture.Name = textureObj.name;
            }

            if (_shouldUseInternalBufferForImages)
            {
#if UNITY_EDITOR
                var assetPath = AssetDatabase.GetAssetPath(textureObj);
                if (assetPath != null && (textureMapType == TextureMapType.Main || textureMapType == TextureMapType.Bump) && supportedEditorOriginalFormat.Any(x => assetPath.EndsWith(x)))
                {
                    texture.Source = EditorExportImageInternalBuffer(textureObj, textureMapType);
                }
                else
                {
                    texture.Source = ExportImageInternalBuffer(textureObj, textureMapType);
                }
#else
                texture.Source = ExportImageInternalBuffer(textureObj, textureMapType);
#endif
            }
            else
            {
                texture.Source = ExportImage(textureObj, textureMapType);
            }
            texture.Sampler = ExportSampler(textureObj);

            _textures.Add(textureObj);

            id = new TextureId
            {
                Id = _root.Textures.Count,
                Root = _root
            };

            _root.Textures.Add(texture);

            return id;
        }

        private ImageId ExportImage(Texture texture, TextureMapType texturMapType)
        {
            ImageId id = GetImageId(_root, texture);
            if (id != null)
            {
                return id;
            }

            var image = new GLTFImage();

            if (ExportNames)
            {
                image.Name = texture.name;
            }

            _imageInfos.Add(new ImageInfo
            {
                texture = texture as Texture2D,
                textureMapType = texturMapType
            });

            var imagePath = _exportOptions.TexturePathRetriever(texture);
            if (string.IsNullOrEmpty(imagePath))
            {
                imagePath = texture.name;
            }

            var filenamePath = Path.ChangeExtension(imagePath, ".png");
            if (!ExportFullPath)
            {
                filenamePath = Path.ChangeExtension(texture.name, ".png");
            }
            image.Uri = System.Uri.EscapeUriString(filenamePath);

            id = new ImageId
            {
                Id = _root.Images.Count,
                Root = _root
            };

            _root.Images.Add(image);

            return id;
        }
        private string GetMimeTypeName(ImageMimeType mimeType)
        {
            switch (mimeType)
            {
                case ImageMimeType.COMMON:
                    return "image/png";
                case ImageMimeType.KTX:
                    return "image/ktx";
                case ImageMimeType.BASIS:
                    return "image/basis";
            }
            return null;
        }
        private ImageId ExportImageInternalBuffer(byte[] bytes, ImageMimeType mimeType)
        {
            var image = new GLTFImage
            {
                MimeType = GetMimeTypeName(mimeType)
            };

            var byteOffset = _bufferWriter.BaseStream.Position;
            {
                _bufferWriter.Write(bytes);
            }

            var byteLength = _bufferWriter.BaseStream.Position - byteOffset;
            byteLength = AppendToBufferMultiplyOf4(byteOffset, byteLength);
            image.BufferView = ExportBufferView((uint)byteOffset, (uint)byteLength);
            var id = new ImageId
            {
                Id = _root.Images.Count,
                Root = _root
            };
            _root.Images.Add(image);

            return id;
        }

#if UNITY_EDITOR
        private ImageId EditorExportImageInternalBuffer(Texture texture, TextureMapType textureMapType)
        {
            if (texture == null)
            {
                throw new System.ArgumentNullException("texture can not be NULL.");
            }
            if (textureMapType == TextureMapType.MetallicGloss)
                return ExportImageInternalBuffer(texture, textureMapType);

            bool isSupportedFormat = false;
            if (ExportOriginalTextureFile)
            {
                var path = AssetDatabase.GetAssetPath(texture);
                if (path != null)
                {
                    isSupportedFormat = supportedEditorOriginalFormat.Any(x => path.EndsWith(x));
                }
            }
            if (!isSupportedFormat)
            {
                return ExportImageInternalBuffer(texture, textureMapType);
            }

            var image = new GLTFImage
            {
                MimeType = "image/png"
            };

            var byteOffset = _bufferWriter.BaseStream.Position;
            {
                _bufferWriter.Write(File.ReadAllBytes(AssetDatabase.GetAssetPath(texture)));
            }

            var byteLength = _bufferWriter.BaseStream.Position - byteOffset;
            byteLength = AppendToBufferMultiplyOf4(byteOffset, byteLength);
            image.BufferView = ExportBufferView((uint)byteOffset, (uint)byteLength);
            var id = new ImageId
            {
                Id = _root.Images.Count,
                Root = _root
            };
            _root.Images.Add(image);

            return id;
        }
#endif
        private ImageId ExportImageInternalBuffer(Texture texture, TextureMapType texturMapType)
        {
            if (texture == null)
            {
                throw new System.ArgumentNullException("texture can not be NULL.");
            }

            var image = new GLTFImage
            {
                MimeType = "image/png"
            };

            var byteOffset = _bufferWriter.BaseStream.Position;
            {
                bool isLinear = texturMapType == TextureMapType.Bump || texturMapType == TextureMapType.LightMapDir || texturMapType == TextureMapType.LightMapColor;
                var destRenderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 24, RenderTextureFormat.ARGB32, isLinear ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
                GL.sRGBWrite = !isLinear;
                switch (texturMapType)
                {
                    case TextureMapType.MetallicGloss:
                        Graphics.Blit(texture, destRenderTexture, UnityExtensions.MetalGlossChannelSwapMaterial);
                        break;
                    case TextureMapType.Bump:
                        Graphics.Blit(texture, destRenderTexture, UnityExtensions.NormalChannelMaterial);
                        break;
                    case TextureMapType.LightMapDir:
                        Graphics.Blit(texture, destRenderTexture, UnityExtensions.GetLightmapEncodeMaterial(LightmapEncodingMode.EncodeRGBM));
                        break;
                    default:
                        Graphics.Blit(texture, destRenderTexture);
                        break;
                }

                var exportTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, !isLinear);
                exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
                exportTexture.Apply();

                var pngImageData = exportTexture.EncodeToPNG();
                _bufferWriter.Write(pngImageData);

                RenderTexture.ReleaseTemporary(destRenderTexture);

                GL.sRGBWrite = false;
                if (Application.isEditor)
                    Object.DestroyImmediate(exportTexture);
                else
                    Object.Destroy(exportTexture);
            }

            var byteLength = _bufferWriter.BaseStream.Position - byteOffset;

            byteLength = AppendToBufferMultiplyOf4(byteOffset, byteLength);

            image.BufferView = ExportBufferView((uint)byteOffset, (uint)byteLength);


            var id = new ImageId
            {
                Id = _root.Images.Count,
                Root = _root
            };
            _root.Images.Add(image);

            return id;
        }
        private SamplerId ExportSampler(Texture texture)
        {
            var samplerId = GetSamplerId(_root, texture);
            if (samplerId != null)
                return samplerId;

            var sampler = new Sampler();

            switch (texture.wrapMode)
            {
                case TextureWrapMode.Clamp:
                    sampler.WrapS = WrapMode.ClampToEdge;
                    sampler.WrapT = WrapMode.ClampToEdge;
                    break;
                case TextureWrapMode.Repeat:
                    sampler.WrapS = WrapMode.Repeat;
                    sampler.WrapT = WrapMode.Repeat;
                    break;
                case TextureWrapMode.Mirror:
                    sampler.WrapS = WrapMode.MirroredRepeat;
                    sampler.WrapT = WrapMode.MirroredRepeat;
                    break;
                default:
                    LogPool.ExportLogger.LogWarning(LogPart.Texture, "Unsupported Texture.wrapMode: " + texture.wrapMode);
                    sampler.WrapS = WrapMode.Repeat;
                    sampler.WrapT = WrapMode.Repeat;
                    break;
            }
            bool mipmap = texture.mipmapCount > 1;
            switch (texture.filterMode)
            {
                case FilterMode.Point:
                    sampler.MinFilter = mipmap ? MinFilterMode.NearestMipmapNearest : MinFilterMode.Nearest;
                    sampler.MagFilter = MagFilterMode.Nearest;
                    break;
                case FilterMode.Bilinear:
                    sampler.MinFilter = mipmap ? MinFilterMode.LinearMipmapNearest : MinFilterMode.Linear;
                    sampler.MagFilter = MagFilterMode.Linear;
                    break;
                case FilterMode.Trilinear:
                    sampler.MinFilter = mipmap ? MinFilterMode.LinearMipmapLinear : MinFilterMode.Linear;
                    sampler.MagFilter = MagFilterMode.Linear;
                    break;
                default:
                    LogPool.ExportLogger.LogWarning(LogPart.Texture, "Unsupported Texture.filterMode: " + texture.filterMode);
                    sampler.MinFilter = mipmap ? MinFilterMode.LinearMipmapNearest : MinFilterMode.Linear;
                    sampler.MagFilter = MagFilterMode.Linear;
                    break;
            }

            samplerId = new SamplerId
            {
                Id = _root.Samplers.Count,
                Root = _root
            };

            _root.Samplers.Add(sampler);

            return samplerId;
        }

        public SamplerId GetSamplerId(GLTFRoot root, Texture textureObj)
        {
            for (var i = 0; i < root.Samplers.Count; i++)
            {
                bool filterIsNearest = root.Samplers[i].MinFilter == MinFilterMode.Nearest
                    || root.Samplers[i].MinFilter == MinFilterMode.NearestMipmapNearest
                    || root.Samplers[i].MinFilter == MinFilterMode.NearestMipmapLinear;

                bool filterIsLinear = root.Samplers[i].MinFilter == MinFilterMode.Linear
                    || root.Samplers[i].MinFilter == MinFilterMode.LinearMipmapNearest;

                bool filterMatched = textureObj.filterMode == FilterMode.Point && filterIsNearest
                    || textureObj.filterMode == FilterMode.Bilinear && filterIsLinear
                    || textureObj.filterMode == FilterMode.Trilinear && root.Samplers[i].MinFilter == MinFilterMode.LinearMipmapLinear;

                bool wrapMatched = textureObj.wrapMode == TextureWrapMode.Clamp && root.Samplers[i].WrapS == WrapMode.ClampToEdge
                    || textureObj.wrapMode == TextureWrapMode.Repeat && root.Samplers[i].WrapS == WrapMode.Repeat
                    || textureObj.wrapMode == TextureWrapMode.Mirror && root.Samplers[i].WrapS == WrapMode.MirroredRepeat;

                if (filterMatched && wrapMatched)
                {
                    return new SamplerId
                    {
                        Id = i,
                        Root = root
                    };
                }
            }

            return null;
        }


        public ImageId GetImageId(GLTFRoot root, Texture imageObj)
        {
            for (var i = 0; i < _imageInfos.Count; i++)
            {
                if (_imageInfos[i].texture == imageObj)
                {
                    return new ImageId
                    {
                        Id = i,
                        Root = root
                    };
                }
            }

            return null;
        }
        public TextureId GetTextureId(GLTFRoot root, Texture textureObj)
        {
            for (var i = 0; i < _textures.Count; i++)
            {
                if (_textures[i] == textureObj)
                {
                    return new TextureId
                    {
                        Id = i,
                        Root = root
                    };
                }
            }

            return null;
        }
        private long AppendToBufferMultiplyOf4(long byteOffset, long byteLength)
        {
            var moduloOffset = byteLength % 4;
            if (moduloOffset > 0)
            {
                for (int i = 0; i < (4 - moduloOffset); i++)
                {
                    _bufferWriter.Write((byte)0x00);
                }
                byteLength = _bufferWriter.BaseStream.Position - byteOffset;
            }

            return byteLength;
        }
    }
}