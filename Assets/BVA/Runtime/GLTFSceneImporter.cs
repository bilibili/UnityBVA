using GLTF;
using GLTF.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using BVA.Cache;
using BVA.Extensions;
using BVA.Loader;

namespace BVA
{
    public class ImportOptions
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public ILoader ExternalDataLoader = null;
#pragma warning restore CS0618 // Type or member is obsolete

        /// <summary>
        /// Optional <see cref="IDataLoader"/> for loading references from the GLTF to external streams.  May also optionally implement <see cref="IDataLoader2"/>.
        /// </summary>
        public IDataLoader DataLoader = null;
        public AsyncCoroutineHelper AsyncCoroutineHelper = null;
        public bool ThrowOnLowMemory = true;
        public bool RuntimeImport = false;
    }

    public class UnityMeshData
    {
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public Vector4[] Tangents;
        public Vector2[] Uv1;
        public Vector2[] Uv2;
        public Vector2[] Uv3;
        public Vector2[] Uv4;
        public Vector2[] Uv5;
        public Vector2[] Uv6;
        public Vector2[] Uv7;
        public Vector2[] Uv8;
        public Color[] Colors;
        public BoneWeight[] BoneWeights;

        public Vector3[][] MorphTargetVertices;
        public Vector3[][] MorphTargetNormals;
        public Vector3[][] MorphTargetTangents;

        public MeshTopology[] Topology;
        public int[][] Indices;
        public bool FirstPrimContainsAllVertex;
    }
    public class UnityNativeMeshData
    {
        public NativeArray<VertexAttributeDescriptor> Layout;
        public int VertexCount;
        public int IndexCount;
        public int SubMeshCount;
        private Mesh.MeshDataArray _data;
        public Mesh.MeshData MeshData => _data[0];

        public Vector3[][] MorphTargetVertices;
        public Vector3[][] MorphTargetNormals;
        public Vector3[][] MorphTargetTangents;

        public MeshTopology[] Topology;
        public UnityNativeMeshData(NativeArray<VertexAttributeDescriptor> layout, int vertexCount, int indexCount)
        {
            _data = Mesh.AllocateWritableMeshData(1);
            Layout = layout;
            VertexCount = vertexCount;
            IndexCount = indexCount;
            MeshData.SetVertexBufferParams(vertexCount, layout);
            MeshData.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
        }
        public void SetSubMesh(SubMeshDescriptor[] subMeshDescriptors)
        {
            SubMeshCount = subMeshDescriptors.Length;
            for (int i = 0; i < SubMeshCount; i++)
            {
                MeshData.SetSubMesh(i, subMeshDescriptors[i]);
            }
        }
    }
    public struct ImportProgress
    {
        public bool IsDownloaded;

        public int NodeTotal;
        public int NodeLoaded;

        public int TextureTotal;
        public int TextureLoaded;

        public int BuffersTotal;
        public int BuffersLoaded;

        public float Progress
        {
            get
            {
                int total = NodeTotal + TextureTotal + BuffersTotal;
                int loaded = NodeLoaded + TextureLoaded + BuffersLoaded;
                if (total > 0)
                {
                    return (float)loaded / total;
                }
                else
                {
                    return 0.0f;
                }
            }
        }
    }

    public struct ImportStatistics
    {
        public long TriangleCount;
        public long VertexCount;
        public long TextureCount;
    }

    /// <summary>
    /// Converts gltf animation data to unity
    /// </summary>
    public delegate float[] ValuesConvertion(NumericArray data, int frame);

    public partial class GLTFSceneImporter : IDisposable
    {
        public enum ColliderType
        {
            None,
            Box,
            Mesh,
            MeshConvex
        }

        /// <summary>
        /// Maximum LOD
        /// </summary>
        public int MaximumLod = 300;

        /// <summary>
        /// Timeout for certain threading operations
        /// </summary>
        public int Timeout = 8;

        private bool _isMultithreaded = true;

        /// <summary>
        /// Use Multithreading or not.
        /// In editor, this is always false. This is to prevent a freeze in editor (noticed in Unity versions 2017.x and 2018.x)
        /// </summary>
        public bool IsMultithreaded
        {
            get
            {
                return (!Application.isEditor || Application.isPlaying) && _isMultithreaded;
            }
            set
            {
                _isMultithreaded = value;
            }
        }

        /// <summary>
        /// The parent transform for the created GameObject
        /// </summary>
        public Transform SceneParent { get; set; }

        /// <summary>
        /// The last created object
        /// </summary>
        public GameObject CreatedObject { get; private set; }

        /// <summary>
        /// Adds colliders to primitive objects when created
        /// </summary>
        public ColliderType Collider { get; set; }

        /// <summary>
        /// Override for the shader to use on created materials
        /// </summary>
        public string CustomShaderName { get; set; }

        /// <summary>
        /// Whether to keep a CPU-side copy of the mesh after upload to GPU (for example, in case normals/tangents need recalculation)
        /// </summary>
        public bool KeepCPUCopyOfMesh = true;

        /// <summary>
        /// Whether to keep a CPU-side copy of the texture after upload to GPU
        /// </summary>
        /// <remaks>
        /// This is is necessary when a texture is used with different sampler states, as Unity doesn't allow setting
        /// of filter and wrap modes separately form the texture object. Setting this to false will omit making a copy
        /// of a texture in that case and use the original texture's sampler state wherever it's referenced; this is
        /// appropriate in cases such as the filter and wrap modes being specified in the shader instead
        /// </remaks>
        public bool KeepCPUCopyOfTexture = false;

        /// <summary>
        /// Specifies whether the MipMap chain should be generated for model textures
        /// </summary>
        public bool GenerateMipMapsForTextures = true;

        /// <summary>
        /// When screen coverage is above threashold and no LOD mesh cull the object
        /// </summary>
        public bool CullFarLOD = false;

        /// <summary>
        /// Statistics from the scene
        /// </summary>
        public ImportStatistics Statistics;
        protected struct GLBStream
        {
            public Stream Stream;
            public long StartPosition;
        }

        protected ImportOptions _options;
        protected MemoryChecker _memoryChecker;

        protected GameObject _lastLoadedScene;
        protected readonly GLTFMaterial DefaultMaterial = new GLTFMaterial();
        protected MaterialCacheData _defaultLoadedMaterial = null;

        protected string _gltfFileName;
        protected GLBStream _gltfStream;
        protected GLTFRoot _gltfRoot;
        protected AssetCache _assetCache;
        protected AssetManager _assetManager;
        public GLTFRoot Root => _gltfRoot;
        public GameObject LastLoadedScene => _lastLoadedScene;
        public AssetManager AssetManager => _assetManager;
        protected bool _isRunning = false;

        protected ImportProgress progressStatus = default(ImportProgress);
        protected IProgress<ImportProgress> progress = null;

        public GLTFSceneImporter(string gltfFileName, ImportOptions options)
        {
            _gltfFileName = gltfFileName;
            _options = options;
            if (_options.DataLoader == null)
            {
                _options.DataLoader = LegacyLoaderWrapper.Wrap(_options.ExternalDataLoader);
            }
        }

        public GLTFSceneImporter(GLTFRoot rootNode, Stream gltfStream, ImportOptions options)
        {
            _gltfRoot = rootNode;

            if (gltfStream != null)
            {
                _gltfStream = new GLBStream { Stream = gltfStream, StartPosition = gltfStream.Position };
            }

            _options = options;
            if (_options.DataLoader == null)
            {
                _options.DataLoader = LegacyLoaderWrapper.Wrap(_options.ExternalDataLoader);
            }
        }

        public void Dispose()
        {
            Cleanup();
        }


        /// <summary>
        /// Loads a glTF Scene into the LastLoadedScene field
        /// </summary>
        /// <param name="sceneIndex">The scene to load, If the index isn't specified, we use the default index in the file. Failing that we load index 0.</param>
        /// <param name="showSceneObj"></param>
        /// <param name="onLoadComplete">Callback function for when load is completed</param>
        /// <param name="cancellationToken">Cancellation token for loading</param>
        /// <returns></returns>
        public async Task LoadSceneAsync(int sceneIndex = -1, bool showSceneObj = true, Action<GameObject, ExceptionDispatchInfo> onLoadComplete = null, CancellationToken cancellationToken = default(CancellationToken), IProgress<ImportProgress> progress = null)
        {
            try
            {
                lock (this)
                {
                    if (_isRunning)
                    {
                        return;
                        //throw new GLTFLoadException("Cannot call LoadScene while GLTFSceneImporter is already running");
                    }

                    _isRunning = true;
                }

                if (_options.ThrowOnLowMemory)
                {
                    _memoryChecker = new MemoryChecker();
                }

                this.progressStatus = new ImportProgress();
                this.progress = progress;

                Statistics = new ImportStatistics();
                progress?.Report(progressStatus);
                if (_gltfRoot == null)
                {
                    await LoadJson(_gltfFileName);
                    progressStatus.IsDownloaded = true;
                }

                cancellationToken.ThrowIfCancellationRequested();

                if (_assetCache == null)
                {
                    _assetCache = new AssetCache(_gltfRoot);
                }
                await _LoadScene(sceneIndex, showSceneObj, cancellationToken);
            }
            catch (Exception ex)
            {
                Cleanup();

                onLoadComplete?.Invoke(null, ExceptionDispatchInfo.Capture(ex));
                throw;
            }
            finally
            {
                lock (this)
                {
                    _isRunning = false;
                }
            }

            Debug.Assert(progressStatus.NodeLoaded == progressStatus.NodeTotal, $"Nodes loaded ({progressStatus.NodeLoaded}) does not match node total in the scene ({progressStatus.NodeTotal})");
            Debug.Assert(progressStatus.TextureLoaded <= progressStatus.TextureTotal, $"Textures loaded ({progressStatus.TextureLoaded}) is larger than texture total in the scene ({progressStatus.TextureTotal})");

            onLoadComplete?.Invoke(LastLoadedScene, null);
        }

        public IEnumerator LoadScene(int sceneIndex = -1, bool showSceneObj = true, Action<GameObject, ExceptionDispatchInfo> onLoadComplete = null)
        {
            return LoadSceneAsync(sceneIndex, showSceneObj, onLoadComplete).AsCoroutine();
        }

        /// <summary>
        /// Initializes the top-level created node by adding an instantiated GLTF object component to it,
        /// so that it can cleanup after itself properly when destroyed
        /// </summary>
        private void InitializeGltfTopLevelObject()
        {
            InstantiatedGLTFObject instantiatedGltfObject = CreatedObject.AddComponent<InstantiatedGLTFObject>();
            instantiatedGltfObject.CachedData = new RefCountedCacheData
            (
                _assetCache.MaterialCache,
                _assetCache.MeshCache,
                _assetCache.TextureCache,
                _assetCache.ImageCache
            );
        }

        private async Task ConstructBufferData(Node node)
        {
            MeshId mesh = node.Mesh;
            if (mesh != null)
            {
                if (mesh.Value.Primitives != null)
                {
                    await ConstructMeshAttributes(mesh.Value, mesh);
                }
            }

            /*if (node.Children != null)
            {
                foreach (NodeId child in node.Children)
                {
                    await ConstructBufferData(child.Value, cancellationToken);
                }
            }*/

            const string msft_LODExtName = MSFT_LODExtensionFactory.EXTENSION_NAME;
            if (_gltfRoot.ExtensionsUsed != null
                && _gltfRoot.ExtensionsUsed.Contains(msft_LODExtName)
                && node.Extensions != null
                && node.Extensions.ContainsKey(msft_LODExtName))
            {
                MSFT_LODExtension lodsextension = node.Extensions[msft_LODExtName] as MSFT_LODExtension;
                if (lodsextension != null && lodsextension.MeshIds.Count > 0)
                {
                    for (int i = 0; i < lodsextension.MeshIds.Count; i++)
                    {
                        int lodNodeId = lodsextension.MeshIds[i];
                        await ConstructBufferData(_gltfRoot.Nodes[lodNodeId]);
                    }
                }
            }
        }

        protected IEnumerator WaitUntilEnum(WaitUntil waitUntil)
        {
            yield return waitUntil;
        }

        private async Task LoadJson(string jsonFilePath)
        {
            _gltfStream.Stream = await _options.DataLoader.LoadStreamAsync(jsonFilePath);
            _gltfStream.StartPosition = 0;
            GLTFParser.ParseJson(_gltfStream.Stream, out _gltfRoot, _gltfStream.StartPosition);
        }

        private static void RunCoroutineSync(IEnumerator streamEnum)
        {
            var stack = new Stack<IEnumerator>();
            stack.Push(streamEnum);
            while (stack.Count > 0)
            {
                var enumerator = stack.Pop();
                if (enumerator.MoveNext())
                {
                    stack.Push(enumerator);
                    var subEnumerator = enumerator.Current as IEnumerator;
                    if (subEnumerator != null)
                    {
                        stack.Push(subEnumerator);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a scene based off loaded JSON. Includes loading in binary and image data to construct the meshes required.
        /// </summary>
        /// <param name="sceneIndex">The bufferIndex of scene in gltf file to load</param>
        /// <returns></returns>
        protected async Task _LoadScene(int sceneIndex = -1, bool showSceneObj = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            GLTFScene scene;

            if (sceneIndex >= 0 && sceneIndex < _gltfRoot.Scenes.Count)
            {
                scene = _gltfRoot.Scenes[sceneIndex];
            }
            else
            {
                scene = _gltfRoot.GetDefaultScene();
            }

            if (scene == null)
            {
                throw new LoadException("No default scene in gltf file.");
            }

            GetGtlfContentTotals(scene);
            await ConstructScene(scene, showSceneObj, cancellationToken);
            if (SceneParent != null)
            {
                CreatedObject.transform.SetParent(SceneParent, false);
            }

            _lastLoadedScene = CreatedObject;
        }

        private void GetGtlfContentTotals(GLTFScene scene)
        {
            // Count Nodes
            Queue<NodeId> nodeQueue = new Queue<NodeId>();

            // Add scene nodes
            if (scene.Nodes != null)
            {
                for (int i = 0; i < scene.Nodes.Count; ++i)
                {
                    nodeQueue.Enqueue(scene.Nodes[i]);
                }
            }

            // BFS of nodes
            while (nodeQueue.Count > 0)
            {
                var cur = nodeQueue.Dequeue();
                progressStatus.NodeTotal++;

                if (cur.Value.Children != null)
                {
                    for (int i = 0; i < cur.Value.Children.Count; ++i)
                    {
                        nodeQueue.Enqueue(cur.Value.Children[i]);
                    }
                }
            }

            // Total textures
            progressStatus.TextureTotal += _gltfRoot.Textures?.Count ?? 0;

            // Total buffers
            progressStatus.BuffersTotal += _gltfRoot.Buffers?.Count ?? 0;

            // Send report
            progress?.Report(progressStatus);
        }

        private async Task<BufferCacheData> GetBufferData(BufferId bufferId)
        {
            if (_assetCache.BufferCache[bufferId.Id] == null)
            {
                await ConstructBuffer(bufferId.Value, bufferId.Id);
            }

            return _assetCache.BufferCache[bufferId.Id];
        }

        protected async Task ConstructBuffer(GLTFBuffer buffer, int bufferIndex)
        {
            if (buffer.Uri == null)
            {
                Debug.Assert(_assetCache.BufferCache[bufferIndex] == null);
                _assetCache.BufferCache[bufferIndex] = ConstructBufferFromGLB(bufferIndex);

                progressStatus.BuffersLoaded++;
                progress?.Report(progressStatus);
            }
            else
            {
                var uri = buffer.Uri;

                URIHelper.TryParseBase64(uri, out byte[] bufferData);
                Stream bufferDataStream;
                if (bufferData != null)
                {
                    bufferDataStream = new MemoryStream(bufferData, 0, bufferData.Length, false, true);
                }
                else
                {
                    bufferDataStream = await _options.DataLoader.LoadStreamAsync(buffer.Uri);
                }

                Debug.Assert(_assetCache.BufferCache[bufferIndex] == null);
                _assetCache.BufferCache[bufferIndex] = new BufferCacheData
                {
                    Stream = bufferDataStream
                };

                progressStatus.BuffersLoaded++;
                progress?.Report(progressStatus);
            }
        }

        private (string, JsonReader) GetExtraProperty(JToken token)
        {
            var reader = token.CreateReader();
            if (reader.Read() && reader.TokenType != JsonToken.PropertyName)
            {
                throw new GLTFParseException("GLTF extra must be an object");
            }
            string propertyName = reader.Value.ToString();
            return (propertyName, reader);
        }

        protected async Task ConstructScene(GLTFScene scene, bool showSceneObj, CancellationToken cancellationToken)
        {
            var sceneObj = new GameObject(string.IsNullOrEmpty(scene.Name) ? ("GLTFScene") : scene.Name);

            _assetManager = sceneObj.GetOrAddComponent<AssetManager>();
            _assetManager.Init(_assetCache);

            await LoadAudio(sceneObj);
            await LoadSkybox(sceneObj);
            await LoadSceneLightmap(scene);

            try
            {
                sceneObj.SetActive(showSceneObj);

                Transform[] nodeTransforms = new Transform[scene.Nodes.Count];
                for (int i = 0; i < scene.Nodes.Count; ++i)
                {
                    NodeId node = scene.Nodes[i];
                    GameObject nodeObj = await GetNode(node.Id);
                    nodeObj.transform.SetParent(sceneObj.transform, false);
                    nodeTransforms[i] = nodeObj.transform;
                }

                for (int index = 0; index < _assetCache.NodeCache.Length; index++)
                {
                    var node = _gltfRoot.Nodes[index];
                    var nodeObj = _assetCache.NodeCache[index];
                    LoadVideoPlayer(node, nodeObj);
                    ConstructAvatar(node, nodeObj);
                    LoadPhysicsComponent(node, nodeObj);

#if PUERTS_INTEGRATION
                    LoadPuertsEventsComponent(node, nodeObj);
#endif

                    LoadBlendShapeMixer(node, nodeObj);
                    await LoadVariableCollection(node, nodeObj);
                }
                await LoadSceneRenderSetting(scene);
                await LoadAnimation(sceneObj, cancellationToken);
                await LoadTimeline();

                CreatedObject = sceneObj;
                InitializeGltfTopLevelObject();
            }
            catch (Exception ex)
            {
                // If some failure occured during loading, clean up the scene
                GameObject.DestroyImmediate(sceneObj);
                CreatedObject = null;

                if (ex is OutOfMemoryException)
                {
                   await Resources.UnloadUnusedAssets();
                }

                throw ex;
            }
        }

        float GetLodCoverage(List<double> lodcoverageExtras, int lodIndex)
        {
            if (lodcoverageExtras != null && lodIndex < lodcoverageExtras.Count)
            {
                return (float)lodcoverageExtras[lodIndex];
            }
            else
            {
                return 1.0f / (lodIndex + 2);
            }
        }
        /// <summary>
        /// Allocate a generic type 2D array. The size is depending on the given parameters.
        /// </summary>		
        /// <param name="x">Defines the depth of the arrays first dimension</param>
        /// <param name="y">>Defines the depth of the arrays second dimension</param>
        /// <returns></returns>
        private static T[][] Allocate2dArray<T>(uint x, uint y)
        {
            var result = new T[x][];
            for (var i = 0; i < x; i++) result[i] = new T[y];
            return result;
        }

        protected BufferCacheData ConstructBufferFromGLB(int bufferIndex)
        {
            GLTFParser.SeekToBinaryChunk(_gltfStream.Stream, bufferIndex, _gltfStream.StartPosition);  // sets stream to correct start position
            return new BufferCacheData
            {
                Stream = _gltfStream.Stream,
                ChunkOffset = (uint)_gltfStream.Stream.Position
            };
        }

        protected async Task YieldOnTimeoutAndThrowOnLowMemory()
        {
            if (_options.ThrowOnLowMemory)
            {
                _memoryChecker.ThrowIfOutOfMemory();
            }

            if (_options.AsyncCoroutineHelper != null)
            {
                await _options.AsyncCoroutineHelper.YieldOnTimeout();
            }
        }


        /// <summary>
        ///	 Get the absolute path to a gltf uri reference.
        /// </summary>
        /// <param name="gltfPath">The path to the gltf file</param>
        /// <returns>A path without the filename or extension</returns>
        protected static string AbsoluteUriPath(string gltfPath)
        {
            var uri = new Uri(gltfPath);
            var partialPath = uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Query.Length - uri.Segments[uri.Segments.Length - 1].Length);
            return partialPath;
        }

        /// <summary>
        /// Get the absolute path a gltf file directory
        /// </summary>
        /// <param name="gltfPath">The path to the gltf file</param>
        /// <returns>A path without the filename or extension</returns>
        protected static string AbsoluteFilePath(string gltfPath)
        {
            var fileName = Path.GetFileName(gltfPath);
            var lastIndex = gltfPath.IndexOf(fileName);
            var partialPath = gltfPath.Substring(0, lastIndex);
            return partialPath;
        }

        protected static MeshTopology GetTopology(DrawMode mode)
        {
            switch (mode)
            {
                case DrawMode.Points: return MeshTopology.Points;
                case DrawMode.Lines: return MeshTopology.Lines;
                case DrawMode.LineStrip: return MeshTopology.LineStrip;
                case DrawMode.Triangles: return MeshTopology.Triangles;
            }

            throw new Exception("Unity does not support glTF draw mode: " + mode);
        }

        /// <summary>
        /// Cleans up any undisposed streams after loading a scene or a node.
        /// </summary>
        private void Cleanup()
        {
            if (_assetCache != null)
            {
                _assetCache.Dispose();
                _assetCache = null;
            }
            if (_assetManager != null)
            {
                _assetManager.Dispose();
                _assetManager = null;
            }
        }

        private async Task SetupLoad(Func<Task> callback)
        {
            try
            {
                lock (this)
                {
                    if (_isRunning)
                    {
                        throw new LoadException("Cannot start a load while GLTFSceneImporter is already running");
                    }

                    _isRunning = true;
                }

                Statistics = new ImportStatistics();
                if (_options.ThrowOnLowMemory)
                {
                    _memoryChecker = new MemoryChecker();
                }

                if (_gltfRoot == null)
                {
                    await LoadJson(_gltfFileName);
                }

                if (_assetCache == null)
                {
                    _assetCache = new AssetCache(_gltfRoot);
                }

                await callback();
            }
            catch
            {
                Cleanup();
                throw;
            }
            finally
            {
                lock (this)
                {
                    _isRunning = false;
                }
            }
        }
    }
}
