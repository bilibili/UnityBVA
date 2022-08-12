using System;
using System.Collections.Generic;
using System.IO;
using GLTF.Schema;
using UnityEngine;
using UnityEngine.Video;
using BVA.Component;
using ADBRuntime.Mono;
using GLTF.Schema.BVA;

namespace BVA
{
    public class ExportOptions
    {
        public GLTFSceneExporter.RetrieveTexturePathDelegate TexturePathRetriever = (texture) => texture.name;
        public bool ExportInactivePrimitives = true;
        public bool ExportAvatar = false;
    }

    public enum AudioFormat
    {
        WAV,
        MP3,
        OGG,
    }

    public partial class GLTFSceneExporter
    {
        public delegate string RetrieveTexturePathDelegate(Texture texture);

        private enum IMAGETYPE
        {
            RGB,
            RGBA,
            R,
            G,
            B,
            A,
            G_INVERT
        }

        private enum TextureMapType
        {
            Main,
            Bump,
            SpecGloss,
            Emission,
            MetallicGloss,
            LightMapColor,
            LightMapDir,
            Occlusion
        }

        private struct ImageInfo
        {
            public Texture2D texture;
            public TextureMapType textureMapType;
        }
        private struct AudioInfo
        {
            public AudioClip audio;
            public string url;
        }
        private Dictionary<string, Transform[]> _listRootTransforms;
        private Transform[] _rootTransforms;
        private GLTFRoot _root;
        private BufferId _bufferId;
        private GLTFBuffer _buffer;
        private BinaryWriter _bufferWriter;
        private List<ImageInfo> _imageInfos;
        private List<Texture> _textures;
        private List<Material> _materials;
        private List<Cubemap> _cubemaps;
        private List<Animation> _animations;
        private List<Animator> _animators;
        private List<AudioInfo> _audios;
        private List<AnimationClip> _animatorClips;
        private List<UnityEngine.Rendering.VolumeProfile> _urpVolumeProfiles;
        private List<VideoPlayer> _videoPlayers;
        private List<BlendShapeMixer> _blendShapeMixers;
        private List<PlayableController> _playables;
        private List<IADBPhysicMonoComponent> _dynamicBones;
        private List<MonoBehaviour> _puertsMonos;

        private bool _shouldUseInternalBufferForImages;

        private ExportOptions _exportOptions;

        private const uint MagicGLTF = 0x46546C67;
        private const uint Version = 2;
        private const uint MagicJson = 0x4E4F534A;
        private const uint MagicBin = 0x004E4942;
        private const int GLTFHeaderSize = 12;
        private const int SectionHeaderSize = 8;

        protected struct PrimKey
        {
            public Mesh Mesh;
            public Material Material;
        }
        private readonly Dictionary<PrimKey, MeshId> _primOwner = new Dictionary<PrimKey, MeshId>();
        private readonly Dictionary<Mesh, MeshPrimitive[]> _meshToPrims = new Dictionary<Mesh, MeshPrimitive[]>();

        private NodeCache _nodeCache;
        public TimeSpan ExportDuration { get; private set; }

        // Settings
        public static bool ExportNames = true;
        public static bool ExportFullPath = true;
        public static bool ExportVertexColor = false;
        public static bool ExportTangent = true;
        public static bool ExportBlendShape = true;
        public static bool RequireExtensions = false;
        public static bool ExportOriginalTextureFile = false;
        public static bool ExportInActiveGameObject = false;
        public static bool ExportLightmap = false;
        public static bool ExportRenderSetting = false;
        public static bool ExportSkybox = false;
        /// <summary>
        /// use draco compress mesh object, reflection probe has issue when using it
        /// </summary>
        public static bool DracoMeshCompression = false;
        /// <summary>
        /// When export materials that using custom shader,export unlit material extension ,for the preview of the third-party tools
        /// </summary>
        public static bool ExportUnlitWhenUsingCustomShader = true;

        public static float ExportAudioQuality = 0.4f;
        /// <summary>
        /// If audio length > this value, export as ogg if ExportAudioFormat is DEFAULT
        /// </summary>
        public static float ExportOggAudioClipLength = 5.0f;

        public static bool ExportAnimationClips = false;
        public static void SetDefaultAvatarExportSetting()
        {
            GLTFSceneExporter.ExportFullPath = true;
            GLTFSceneExporter.ExportNames = true;
            GLTFSceneExporter.ExportVertexColor = false;
            GLTFSceneExporter.ExportTangent = false;
            GLTFSceneExporter.ExportBlendShape = true;
            GLTFSceneExporter.RequireExtensions = false;
            GLTFSceneExporter.ExportUnlitWhenUsingCustomShader = true;
            GLTFSceneExporter.ExportInActiveGameObject = false;
            GLTFSceneExporter.ExportAudioQuality = 0.6f;
        }
        public GLTFSceneExporter(Transform[] rootTransforms, ExportOptions options)
        {
            _exportOptions = options;
            _rootTransforms = rootTransforms;
            Construct();
        }

        /// <summary>
        /// Create a GLTFExporter that exports out list of scenes
        /// </summary>
        /// <param name="rootTransforms">Root transform of object to export</param>
        public GLTFSceneExporter(Dictionary<string, Transform[]> listRootTransforms, Transform[] rootTransforms, ExportOptions options)
        {
            _exportOptions = options;
            _rootTransforms = rootTransforms;
            _listRootTransforms = listRootTransforms;
            Construct();
        }

        private void Construct()
        {
            LogPool.ExportLogger.Clear();

            _root = new GLTFRoot
            {
                Accessors = new List<Accessor>(),
                Asset = new Asset
                {
                    Version = "2.0",
                    Generator = BVAConst.GetGeneratorName(_exportOptions.ExportAvatar ? SceneType.Avatar : SceneType.Scene)
                },
                Buffers = new List<GLTFBuffer>(),
                BufferViews = new List<BufferView>(),
                Cameras = new List<GLTFCamera>(),
                Images = new List<GLTFImage>(),
                Materials = new List<GLTFMaterial>(),
                Meshes = new List<GLTFMesh>(),
                Nodes = new List<Node>(),
                Samplers = new List<Sampler>(),
                Scenes = new List<GLTFScene>(),
                Textures = new List<GLTFTexture>(),
                Animations = new List<GLTFAnimation>(),
                Skins = new List<Skin>(),
                Extensions = new GLTFExtensions()
            };

            _nodeCache = new NodeCache();
            _imageInfos = new List<ImageInfo>();
            _materials = new List<Material>();
            _textures = new List<Texture>();
            _cubemaps = new List<Cubemap>();
            _animations = new List<Animation>();
            _animators = new List<Animator>();
            _audios = new List<AudioInfo>();
            _videoPlayers = new List<VideoPlayer>();
            _urpVolumeProfiles = new List<UnityEngine.Rendering.VolumeProfile>();
            _playables = new List<PlayableController>();
            _blendShapeMixers = new List<BlendShapeMixer>();
            _dynamicBones = new List<IADBPhysicMonoComponent>();

            _buffer = new GLTFBuffer();
            _bufferId = new BufferId
            {
                Id = _root.Buffers.Count,
                Root = _root
            };
            _root.Buffers.Add(_buffer);
        }
        /// <summary>
        /// Gets the root object of the exported GLTF
        /// </summary>
        /// <returns>Root parsed GLTF Json</returns>
        public GLTFRoot GetRoot()
        {
            return _root;
        }

        /// <summary>
        /// Writes a binary GLB file with filename at path.
        /// </summary>
        /// <param name="path">File path for saving the binary file</param>
        /// <param name="fileName">The name of the GLTF file</param>
        public void SaveGLB(string path, string fileName, string ext = "glb")
        {
            _shouldUseInternalBufferForImages = true;
            string fullPath = Path.Combine(path, Path.ChangeExtension(fileName, ext));

            using (FileStream glbFile = new FileStream(fullPath, FileMode.Create))
            {
                SaveGLBToStream(glbFile, fileName);
            }

            if (!_shouldUseInternalBufferForImages)
            {
                ExportImages(path);
            }
        }

        /// <summary>
        /// In-memory GLB creation helper. Useful for platforms where no filesystem is available (e.g. WebGL).
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public byte[] SaveGLBToByteArray(string sceneName, bool shouldUseInternalBufferForImages)
        {
            _shouldUseInternalBufferForImages = shouldUseInternalBufferForImages;
            using (var stream = new MemoryStream())
            {
                SaveGLBToStream(stream, sceneName);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Writes a binary GLB file into a stream (memory stream, filestream, ...)
        /// </summary>
        /// <param name="path">File path for saving the binary file</param>
        /// <param name="fileName">The name of the GLTF file</param>
        public void SaveGLBToStream(Stream stream, string sceneName)
        {
            var startTime = DateTime.Now;

            Stream binStream = new MemoryStream();
            Stream jsonStream = new MemoryStream();

            _bufferWriter = new BinaryWriter(binStream);
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            TextWriter jsonWriter = new StreamWriter(jsonStream, utf8WithoutBom);

            _root.Scene = ExportScene(sceneName, _rootTransforms);
            if (_listRootTransforms != null)
            {
                foreach (var transforms in _listRootTransforms)
                {
                    ExportScene(transforms.Key, transforms.Value);
                }
            }

            _buffer.ByteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Length, 4);

            _root.Serialize(jsonWriter, true);

            _bufferWriter.Flush();
            jsonWriter.Flush();

            // align to 4-byte boundary to comply with spec.
            AlignToBoundary(jsonStream);
            AlignToBoundary(binStream, 0x00);

            int glbLength = (int)(GLTFHeaderSize + SectionHeaderSize +
                jsonStream.Length + SectionHeaderSize + binStream.Length);

            BinaryWriter writer = new BinaryWriter(stream);

            // write header
            writer.Write(MagicGLTF);
            writer.Write(Version);
            writer.Write(glbLength);

            // write JSON chunk header.
            writer.Write((int)jsonStream.Length);
            writer.Write(MagicJson);

            jsonStream.Position = 0;
            CopyStream(jsonStream, writer);

            writer.Write((int)binStream.Length);
            writer.Write(MagicBin);

            binStream.Position = 0;
            CopyStream(binStream, writer);

            writer.Flush();

            ExportDuration = DateTime.Now - startTime;
        }

        /// <summary>
        /// Specifies the path and filename for the GLTF Json and binary
        /// </summary>
        /// <param name="path">File path for saving the GLTF and binary files</param>
        /// <param name="fileName">The name of the GLTF file</param>
        public void SaveGLTFandBin(string path, string fileName)
        {
            var startTime = DateTime.Now;

            _shouldUseInternalBufferForImages = false;
            var binFile = File.Create(Path.Combine(path, fileName + ".bin"));
            _bufferWriter = new BinaryWriter(binFile);
            try
            {
                _root.Scene = ExportScene(fileName, _rootTransforms);
                if (_listRootTransforms != null)
                {
                    foreach (var transforms in _listRootTransforms)
                    {
                        ExportScene(transforms.Key, transforms.Value);
                    }
                }
                AlignToBoundary(_bufferWriter.BaseStream, 0x00);
                _buffer.Uri = fileName + ".bin";
                _buffer.ByteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Length, 4);

                var gltfFile = File.CreateText(Path.Combine(path, fileName + ".gltf"));
                _root.Serialize(gltfFile);
                ExportImages(path);
                ExportAudios(path);
                gltfFile.Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                binFile.Close();
            }

            ExportDuration = DateTime.Now - startTime;
        }

        /// <summary>
        /// Convenience function to copy from a stream to a binary writer, for
        /// compatibility with pre-.NET 4.0.
        /// Note: Does not set position/seek in either stream. After executing,
        /// the input buffer's position should be the end of the stream.
        /// </summary>
        /// <param name="input">Stream to copy from</param>
        /// <param name="output">Stream to copy to.</param>
        private static void CopyStream(Stream input, BinaryWriter output)
        {
            byte[] buffer = new byte[8 * 1024];
            int length;
            while ((length = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, length);
            }
        }

        /// <summary>
        /// Pads a stream with additional bytes.
        /// </summary>
        /// <param name="stream">The stream to be modified.</param>
        /// <param name="pad">The padding byte to append. Defaults to ASCII
        /// space (' ').</param>
        /// <param name="boundary">The boundary to align with, in bytes.
        /// </param>
        private static void AlignToBoundary(Stream stream, byte pad = (byte)' ', uint boundary = 4)
        {
            uint currentLength = (uint)stream.Length;
            uint newLength = CalculateAlignment(currentLength, boundary);
            for (int i = 0; i < newLength - currentLength; i++)
            {
                stream.WriteByte(pad);
            }
        }

        /// <summary>
        /// Calculates the number of bytes of padding required to align the
        /// size of a buffer with some multiple of byteAllignment.
        /// </summary>
        /// <param name="currentSize">The current size of the buffer.</param>
        /// <param name="byteAlignment">The number of bytes to align with.</param>
        /// <returns></returns>
        public static uint CalculateAlignment(uint currentSize, uint byteAlignment)
        {
            return (currentSize + byteAlignment - 1) / byteAlignment * byteAlignment;
        }

        public void CheckExtension(string extensionName)
        {
            if (_root.ExtensionsUsed == null)
            {
                _root.ExtensionsUsed = new List<string>(new[] { extensionName });
            }
            else if (!_root.ExtensionsUsed.Contains(extensionName))
            {
                _root.ExtensionsUsed.Add(extensionName);
            }

            if (RequireExtensions)
            {
                if (_root.ExtensionsRequired == null)
                {
                    _root.ExtensionsRequired = new List<string>(new[] { extensionName });
                }
                else if (!_root.ExtensionsRequired.Contains(extensionName))
                {
                    _root.ExtensionsRequired.Add(extensionName);
                }
            }
        }

        private void ExportUnitySceneSetting(GLTFScene scene)
        {
            if (ShouldExportLightmap())
            {
                var lightmapId = ExportSceneLightmap();
                scene.AddExtension(_root, BVA_light_lightmapExtensionFactory.EXTENSION_NAME, new BVA_light_lightmapExtensionFactory(lightmapId), RequireExtensions);
            }
            if (ShouldExportRenderSettings())
            {
                var renderSettingId = ExportSceneRenderSetting();
                scene.AddExtension(_root, BVA_setting_renderSettingExtensionFactory.EXTENSION_NAME, new BVA_setting_renderSettingExtensionFactory(renderSettingId), RequireExtensions);
            }
            if (ShouldExportSceneSkybox())
            {
                ExportSkyboxMaterial(RenderSettings.skybox);
            }
        }

        private SceneId ExportScene(string name, Transform[] rootObjTransforms)
        {
            if (rootObjTransforms == null || rootObjTransforms.Length == 0)
                throw new ArgumentException("no root transform ");
            var scene = new GLTFScene() { Name = name };

            ExportUnitySceneSetting(scene);

            scene.Nodes = new List<NodeId>(rootObjTransforms.Length);
            foreach (var transform in rootObjTransforms)
            {
                var audioclip = transform.GetComponent<AudioClipContainer>();
                if (audioclip != null) ExportContainer(audioclip);

                var skybox = transform.GetComponent<SkyboxContainer>();
                if (skybox != null) ExportContainer(skybox);

                if (ExportInActiveGameObject || transform.gameObject.activeSelf)
                    scene.Nodes.Add(ExportNode(transform));
            }

            _root.Scenes.Add(scene);

            foreach (var transform in rootObjTransforms)
            {
                var skinnedMeshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var smr in skinnedMeshRenderers)
                {
                    if (IsValidSkinnedMeshRenderer(smr))
                    {
                        int id = _nodeCache.GetId(smr.gameObject);
                        Node node = _root.Nodes[id];
                        node.Skin = ExportSkin(smr);
                    }
                }
            }

            ExportAvatar();
            ExportBlendShapeMixer();
#if UNITY_EDITOR
            if (ExportAnimationClips)
                ExportAnimation();
#endif
            ExportVideoPlayer();
            //ExportTimeline();

            ExportDynamicBone();

#if PUERTS_INTEGRATION
            ExportPuertsEvent();
#endif
            return new SceneId
            {
                Id = _root.Scenes.Count - 1,
                Root = _root
            };
        }
    }
}
