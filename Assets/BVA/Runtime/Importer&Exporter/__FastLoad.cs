using GLTF;
using GLTF.Schema;
using GLTF.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using BVA.Cache;
using BVA.Extensions;
using BVA.Loader;
using Unity.Collections;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        Transform[] _nodeTransforms;
        List<UnityMeshData> _unityMeshDatas;

        /// <summary>
        /// preload accelerate the load speed, but you need to make sure only 1 buffer in using, and 1 scene exist. 
        /// </summary>
        /// <param name="fileName"></param>
        public async Task LoadAvatar()
        {
            if (_gltfRoot == null)
                await LoadJson(_gltfFileName);

            _assetCache = new AssetCache(_gltfRoot);
            _assetCache.BufferCache[0] = ConstructBufferFromGLB(0);

            Thread loadMesh = new Thread(PreloadMeshPrimitives);
            loadMesh.Start();

            await PreloadTextures();
            await PreloadMaterials();
            await PreloadNodes(_gltfRoot.GetDefaultScene());
            _lastLoadedScene = _assetCache.NodeCache[0];
            _assetManager = _lastLoadedScene.GetOrAddComponent<AssetManager>();
            _assetManager.Init(_assetCache);
            await LoadAudio(_lastLoadedScene);
            await LoadAnimation(_lastLoadedScene, CancellationToken.None);

#if !UNITY_WEBGL
            loadMesh.Join();
#endif
            AttachRenderers();


            var node = _gltfRoot.Nodes[0];
            var nodeObj = _assetCache.NodeCache[0];
            ConstructAvatar(node, nodeObj);
            for (int index = 0; index < _assetCache.NodeCache.Length; index++)
            {
                var childNode = _gltfRoot.Nodes[index];
                LoadPhysicsComponent(childNode, _assetCache.NodeCache[index]);
            }
            LoadBlendShapeMixer(node, nodeObj);
            //for (int index = 0; index < _assetCache.NodeCache.Length; index++)
            //{
            //    await LoadVariableCollection(node, nodeObj);
            //}
        }

        public HashSet<int> ExtractNormalTextures()
        {
            HashSet<int> normalTexturesIndex = new HashSet<int>();
            for (int i = 0; i < _gltfRoot.Materials.Count; i++)
            {
                GLTFMaterial gLTFMaterial = _gltfRoot.Materials[i];
                if (gLTFMaterial.NormalTexture != null && gLTFMaterial.NormalTexture.Index.IsValid)
                {
                    normalTexturesIndex.Add(gLTFMaterial.NormalTexture.Index.Id);
                }
            }
            return normalTexturesIndex;
        }
        /// <summary>
        /// load all textures
        /// </summary>
        public async Task PreloadTextures()
        {
            if (_gltfRoot.Textures == null) return;
            HashSet<int> normalTexturesIndex = ExtractNormalTextures();

            for (int i = 0; i < _gltfRoot.Textures.Count; i++)
            {
                GLTFTexture texture = _gltfRoot.Textures[i];
                _assetCache.TextureCache[i] = new TextureCacheData { TextureDefinition = texture };
                ++Statistics.TextureCount;
                int sourceId = GetTextureSourceId(texture);
                GLTFImage image = _gltfRoot.Images[sourceId];
                Texture2D unity_texture;
                bool isLinear = normalTexturesIndex.Contains(i);
                if (_assetCache.ImageCache[sourceId] == null)
                {
                    var bufferView = image.BufferView.Value;
                    var data = new byte[bufferView.ByteLength];
                    BufferCacheData bufferContents = _assetCache.BufferCache[bufferView.Buffer.Id];
                    byte[] buffer;

                    lock (bufferContents.Stream)
                    {
                        bufferContents.Stream.Position = bufferView.ByteOffset + bufferContents.ChunkOffset;
                        Stream stream = new SubStream(bufferContents.Stream, 0, data.Length);

                        buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);
                    }

                    ImageMimeType mimeType = GetImageMimeType(image);

                    if (mimeType == ImageMimeType.KTX)
                    {
                        var ktxTexture = new KtxUnity.KtxTexture();
                        NativeArray<byte> nativeArray = new NativeArray<byte>(buffer, Allocator.Persistent);

                        var result = await ktxTexture.LoadFromBytes(nativeArray, isLinear);
                        unity_texture = result.texture;
                    }
                    else if (mimeType == ImageMimeType.BASIS)
                    {
                        var basisTexture = new KtxUnity.BasisUniversalTexture();
                        NativeArray<byte> nativeArray = new NativeArray<byte>(buffer, Allocator.Persistent);

                        var result = await basisTexture.LoadFromBytes(nativeArray, isLinear);
                        unity_texture = result.texture;
                    }
                    else
                    {
                        unity_texture = new Texture2D(0, 0, TextureFormat.ARGB32, false, isLinear);
                        unity_texture.name = image.Name ?? "No name";
                        unity_texture.LoadImage(buffer, true);
                        _assetCache.ImageCache[sourceId] = unity_texture;
                    }
                }
                else
                {
                    unity_texture = _assetCache.ImageCache[sourceId];
                }

                progressStatus.TextureLoaded++;
                _assetCache.TextureCache[i].Texture = unity_texture;
            }
        }
        public async Task PreloadMaterials()
        {
            for (int i = 0; i < _gltfRoot.Materials.Count; i++)
            {
                GLTFMaterial def = _gltfRoot.Materials[i];
                await ConstructMaterial(def, i);
            }
        }
        /// <summary>
        /// load mesh primitive data
        /// </summary>
        public void PreloadMeshPrimitives()
        {
            _unityMeshDatas = new List<UnityMeshData>(_gltfRoot.Meshes.Count);
            for (int meshIndex = 0; meshIndex < _gltfRoot.Meshes.Count; meshIndex++)
            {
                var mesh = _gltfRoot.Meshes[meshIndex];
                _assetCache.MeshCache[meshIndex] = new MeshCacheData();

                //var loopRet = Parallel.For(0, mesh.Primitives.Count, (i) =>
                for (int i = 0; i < mesh.Primitives.Count; ++i)
                {
                    MeshPrimitive primitive = mesh.Primitives[i];
                    var primData = new MeshCacheData.PrimitiveCacheData();
                    _assetCache.MeshCache[meshIndex].Primitives.Add(primData);

                    Dictionary<string, AttributeAccessor> attribute = primData.Attributes;

                    foreach (var attributePair in primitive.Attributes)
                    {
                        attribute[attributePair.Key] = new AttributeAccessor
                        {
                            AccessorId = attributePair.Value,
                            Stream = _assetCache.BufferCache[0].Stream,
                            Offset = _assetCache.BufferCache[0].ChunkOffset
                        };
                    }

                    if (primitive.Indices != null)
                    {
                        attribute[SemanticProperties.INDICES] = new AttributeAccessor
                        {
                            AccessorId = primitive.Indices,
                            Stream = _assetCache.BufferCache[0].Stream,
                            Offset = _assetCache.BufferCache[0].ChunkOffset
                        };
                    }

                    LoadMeshAttribute(attribute);

                }
                if (mesh.Primitives[0].Targets != null)
                {
                    var newTargets = new List<Dictionary<string, AttributeAccessor>>(mesh.Primitives[0].Targets.Count);
                    for (int i = 0; i < mesh.Primitives[0].Targets.Count; i++)
                        newTargets.Add(new Dictionary<string, AttributeAccessor>());
                    _assetCache.MeshCache[meshIndex].Primitives[0].Targets = newTargets;

                    Parallel.For(0, mesh.Primitives[0].Targets.Count, (i) =>// for (int i = 0; i < mesh.Primitives[0].Targets.Count; i++)
                    {
                        Dictionary<string, AccessorId> target = mesh.Primitives[0].Targets[i];
                        //NORMALS, POSITIONS, TANGENTS
                        foreach (var targetAttribute in target)
                        {
                            BufferId bufferIdPair = targetAttribute.Value.Value.BufferView.Value.Buffer;
                            GLTFBuffer buffer = bufferIdPair.Value;
                            int bufferID = bufferIdPair.Id;

                            newTargets[i][targetAttribute.Key] = new AttributeAccessor
                            {
                                AccessorId = targetAttribute.Value,
                                Stream = _assetCache.BufferCache[bufferID].Stream,
                                Offset = _assetCache.BufferCache[bufferID].ChunkOffset
                            };
                        }
                        LoadMeshTargetAttribute(newTargets[i]);
                    });
                    for (int i = 1; i < mesh.Primitives.Count; i++)
                        _assetCache.MeshCache[meshIndex].Primitives[i].Targets = _assetCache.MeshCache[meshIndex].Primitives[0].Targets;
                }

                int primitiveCount = mesh.Primitives.Count;
                uint firstPrimVertexCount = mesh.Primitives.First().Attributes[SemanticProperties.POSITION].Value.Count;
                uint totalVertCount = mesh.Primitives.Aggregate((uint)0, (sum, p) => sum + p.Attributes[SemanticProperties.POSITION].Value.Count);
                // regard first primitive contains all meshes vertex
                bool firstPrimContainsAll = totalVertCount / firstPrimVertexCount == mesh.Primitives.Count && totalVertCount % firstPrimVertexCount == 0;

                var vertOffset = 0;
                MeshPrimitive firstPrim = mesh.Primitives[0];
                MeshCacheData meshCache = _assetCache.MeshCache[meshIndex];
                uint totalVertexCount = firstPrimContainsAll ? firstPrimVertexCount : totalVertCount;
                var unityData = new UnityMeshData()
                {
                    Vertices = new Vector3[totalVertexCount],
                    Normals = firstPrim.Attributes.ContainsKey(SemanticProperties.NORMAL) ? new Vector3[totalVertexCount] : null,
                    Tangents = firstPrim.Attributes.ContainsKey(SemanticProperties.TANGENT) ? new Vector4[totalVertexCount] : null,
                    Uv1 = firstPrim.Attributes.ContainsKey(SemanticProperties.TEXCOORD_0) ? new Vector2[totalVertexCount] : null,
                    Uv2 = firstPrim.Attributes.ContainsKey(SemanticProperties.TEXCOORD_1) ? new Vector2[totalVertexCount] : null,
                    BoneWeights = firstPrim.Attributes.ContainsKey(SemanticProperties.WEIGHTS_0) ? new BoneWeight[totalVertexCount] : null,
                    Colors = firstPrim.Attributes.ContainsKey(SemanticProperties.COLOR_0) ? new Color[totalVertexCount] : null,

                    MorphTargetVertices = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.POSITION) ? Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, totalVertexCount) : null,
                    MorphTargetNormals = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.NORMAL) ? Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, totalVertexCount) : null,
                    MorphTargetTangents = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.TANGENT) ? Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, totalVertexCount) : null,

                    Topology = new MeshTopology[primitiveCount],
                    Indices = new int[primitiveCount][],
                    FirstPrimContainsAllVertex = firstPrimContainsAll
                };

                var targets = meshCache.Primitives[0].Targets;
                if (targets != null)
                {
                    for (int j = 0; j < targets.Count; ++j)
                    {
                        if (targets[j].ContainsKey(SemanticProperties.POSITION))
                        {
                            targets[j][SemanticProperties.POSITION].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetVertices[j], vertOffset);
                        }
                        if (targets[j].ContainsKey(SemanticProperties.NORMAL))
                        {
                            targets[j][SemanticProperties.NORMAL].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetNormals[j], vertOffset);
                        }
                        if (targets[j].ContainsKey(SemanticProperties.TANGENT))
                        {
                            targets[j][SemanticProperties.TANGENT].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetTangents[j], vertOffset);
                        }
                    }
                }

                for (int i = 0; i < mesh.Primitives.Count; ++i)
                {
                    var primitive = mesh.Primitives[i];
                    var primCache = meshCache.Primitives[i];
                    unityData.Topology[i] = GetTopology(primitive.Mode);

                    var submeshAttributes = primCache.Attributes;
                    uint vertexCount = submeshAttributes[SemanticProperties.POSITION].AccessorId.Value.Count;

                    int[] indices = unityData.Topology[i] == MeshTopology.Triangles ? submeshAttributes.ContainsKey(SemanticProperties.INDICES) ?
                    submeshAttributes[SemanticProperties.INDICES].AccessorContent.AsUInts.ToIntArrayFlipTriangleFaces() : MeshPrimitive.GenerateIndices(vertexCount) :
                     submeshAttributes.ContainsKey(SemanticProperties.INDICES) ? submeshAttributes[SemanticProperties.INDICES].AccessorContent.AsUInts.ToIntArrayRaw() : MeshPrimitive.GenerateIndices(vertexCount);

                    int truncate = indices.Length % 3;
                    if (truncate != 0)
                        indices = indices.Take(indices.Length - truncate).ToArray();
                    unityData.Indices[i] = indices;
                    var meshAttributes = meshCache.Primitives[i].Attributes;

                    if (firstPrimContainsAll && i == 0 || !firstPrimContainsAll)
                    {
                        if (meshAttributes.ContainsKey(SemanticProperties.Weight[0]) && meshAttributes.ContainsKey(SemanticProperties.Joint[0]))
                        {
                            CreateBoneWeightArray(
                                meshAttributes[SemanticProperties.Joint[0]].AccessorContent.AsUnityVec4s,
                                meshAttributes[SemanticProperties.Weight[0]].AccessorContent.AsUnityVec4s,
                                ref unityData.BoneWeights,
                                vertOffset);
                        }

                        if (meshAttributes.ContainsKey(SemanticProperties.POSITION))
                        {
                            meshAttributes[SemanticProperties.POSITION].AccessorContent.AsVertices.ToUnityVector3Raw(unityData.Vertices, vertOffset);
                        }
                        if (meshAttributes.ContainsKey(SemanticProperties.NORMAL))
                        {
                            meshAttributes[SemanticProperties.NORMAL].AccessorContent.AsNormals.ToUnityVector3Raw(unityData.Normals, vertOffset);
                        }
                        if (meshAttributes.ContainsKey(SemanticProperties.TANGENT))
                        {
                            meshAttributes[SemanticProperties.TANGENT].AccessorContent.AsTangents.ToUnityVector4Raw(unityData.Tangents, vertOffset);
                        }
                        if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[0]))
                        {
                            meshAttributes[SemanticProperties.TexCoord[0]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv1, vertOffset);
                        }
                        if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[1]))
                        {
                            meshAttributes[SemanticProperties.TexCoord[1]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv2, vertOffset);
                        }
                    }

                    var vertCount = primitive.Attributes[SemanticProperties.POSITION].Value.Count;
                    vertOffset += (int)vertCount;

                    if (unityData.Topology[i] == MeshTopology.Triangles && primitive.Indices != null && primitive.Indices.Value != null)
                    {
                        Statistics.TriangleCount += primitive.Indices.Value.Count / 3;
                    }
                }
                _unityMeshDatas.Add(unityData);
                Statistics.VertexCount += vertOffset;
            }
        }
        private void LoadMeshTargetAttribute(Dictionary<string, AttributeAccessor> att)
        {
            foreach (var kvp in att)
            {
                AttributeAccessor attributeAccessor = kvp.Value;
                NumericArray resultArray = attributeAccessor.AccessorContent;
                uint offset = GLTFHelpers.LoadBufferView(attributeAccessor, out byte[] bufferViewCache);
                switch (kvp.Key)
                {
                    case SemanticProperties.POSITION:
                    case SemanticProperties.NORMAL:
                    case SemanticProperties.TANGENT:
                        attributeAccessor.AccessorId.Value.AsVector3ArrayConvertCoordinateSpace(ref resultArray, bufferViewCache, offset, SchemaExtensions.CoordinateSpaceConversionScale);
                        break;
                }

                attributeAccessor.AccessorContent = resultArray;
            }
        }
        private void LoadMeshAttribute(Dictionary<string, AttributeAccessor> att)
        {
            foreach (var kvp in att)
            {
                AttributeAccessor attributeAccessor = kvp.Value;
                NumericArray resultArray = attributeAccessor.AccessorContent;
                uint offset = GLTFHelpers.LoadBufferView(attributeAccessor, out byte[] bufferViewCache);

                switch (kvp.Key)
                {
                    case SemanticProperties.POSITION:
                        attributeAccessor.AccessorId.Value.AsVertexArrayConvertCoordinateSpace(ref resultArray, bufferViewCache, offset, SchemaExtensions.CoordinateSpaceConversionScale);
                        break;
                    case SemanticProperties.TEXCOORD_0:
                        attributeAccessor.AccessorId.Value.AsTexcoordArrayFlipY(ref resultArray, bufferViewCache, offset);
                        break;
                    case SemanticProperties.NORMAL:
                        attributeAccessor.AccessorId.Value.AsNormalArrayConvertCoordinateSpace(ref resultArray, bufferViewCache, offset, SchemaExtensions.CoordinateSpaceConversionScale);
                        break;
                    case SemanticProperties.TANGENT:
                        attributeAccessor.AccessorId.Value.AsTangentArrayConvertCoordinateSpace(ref resultArray, bufferViewCache, offset, SchemaExtensions.TangentSpaceConversionScale);
                        break;
                    case SemanticProperties.WEIGHTS_0:
                    case SemanticProperties.JOINTS_0:
                        attributeAccessor.AccessorId.Value.AsVector4Array(ref resultArray, bufferViewCache, offset);
                        break;
                    case SemanticProperties.INDICES:
                        attributeAccessor.AccessorId.Value.AsUIntArray(ref resultArray, bufferViewCache, offset);
                        break;
                    case SemanticProperties.COLOR_0:
                        attributeAccessor.AccessorId.Value.AsColorArray(ref resultArray, bufferViewCache, offset);
                        break;
                    case SemanticProperties.TEXCOORD_1:
                    case SemanticProperties.TEXCOORD_2:
                    case SemanticProperties.TEXCOORD_3:
                    case SemanticProperties.TEXCOORD_4:
                    case SemanticProperties.TEXCOORD_5:
                    case SemanticProperties.TEXCOORD_6:
                    case SemanticProperties.TEXCOORD_7:
                    case SemanticProperties.TEXCOORD_8:
                    case SemanticProperties.TEXCOORD_9:
                        attributeAccessor.AccessorId.Value.AsTexcoordArrayFlipY(ref resultArray, bufferViewCache, offset);
                        break;
                }

                attributeAccessor.AccessorContent = resultArray;
            }
        }

        /// <summary>
        /// load animation clips key frame data 
        /// </summary>
        public void PreloadAnimations()
        {

        }

        /// <summary>
        /// create node but don't add component related with other resources like textures, materials.
        /// </summary>
        public async Task PreloadNodes(GLTFScene scene)
        {
            _nodeTransforms = new Transform[scene.Nodes.Count];
            for (int i = 0; i < scene.Nodes.Count; i++)
            {
                NodeId node = scene.Nodes[i];
                GameObject nodeObj = await GetPreloadNode(node.Id);
                _nodeTransforms[i] = nodeObj.transform;
            }
        }
        private async Task<GameObject> GetPreloadNode(int nodeId)
        {
            if (_assetCache.NodeCache[nodeId] == null)
            {
                var node = _gltfRoot.Nodes[nodeId];
                await ConstructPreloadNode(node, nodeId);
            }

            return _assetCache.NodeCache[nodeId];
        }

        private async Task ConstructPreloadNode(Node node, int nodeIndex)
        {
            if (_assetCache.NodeCache[nodeIndex] != null)
            {
                return;
            }

            var nodeObj = new GameObject(string.IsNullOrEmpty(node.Name) ? ("GLTFNode" + nodeIndex) : node.Name);

            node.GetUnityTRSProperties(out Vector3 position, out Quaternion rotation, out Vector3 scale);
            nodeObj.transform.localPosition = position;
            nodeObj.transform.localRotation = rotation;
            nodeObj.transform.localScale = scale;
            _assetCache.NodeCache[nodeIndex] = nodeObj;

            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    GameObject childObj = await GetPreloadNode(child.Id);
                    childObj.transform.SetParent(nodeObj.transform, false);
                }
            }
        }
        public void AttachRenderers()
        {
            for (int i = 0; i < _gltfRoot.Nodes.Count; i++)
            {
                Node node = _gltfRoot.Nodes[i];
                GameObject nodeObj = _assetCache.NodeCache[i];
                AttachMesh(node, nodeObj);
                AddNodeComponent(node, nodeObj);
            }
        }
        private void AttachMesh(Node node, GameObject nodeObj)
        {
            if (node.Mesh == null) return;
            var meshIndex = node.Mesh.Id;
            var mesh = node.Mesh.Value;

            ConstructUnityMesh(_unityMeshDatas[meshIndex], node.Mesh.Id, mesh.Name, true);

            var unityMesh = _assetCache.MeshCache[node.Mesh.Id].LoadedMesh;
            var materials = node.Mesh.Value.Primitives.Select(p => p.Material != null ? _assetCache.MaterialCache[p.Material.Id].UnityMaterialWithVertexColor : _defaultLoadedMaterial.UnityMaterialWithVertexColor).ToArray();

            var morphTargets = mesh.Primitives[0].Targets;
            var weights = node.Weights ?? mesh.Weights ??
                (morphTargets != null ? new List<double>(morphTargets.Select(mt => 0.0)) : null);

            if (node.Skin != null || weights != null)
            {
                var skinnedMeshRenderer = nodeObj.GetOrAddComponent<SkinnedMeshRenderer>();
                skinnedMeshRenderer.sharedMesh = unityMesh;
                skinnedMeshRenderer.sharedMaterials = materials;
                skinnedMeshRenderer.quality = SkinQuality.Auto;

                if (node.Skin != null)
                    SetupBones(node.Skin.Value, skinnedMeshRenderer);

                if (weights != null)
                {

                    for (int i = 0; i < weights.Count; ++i)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(i, (float)(weights[i] * 100));
                    }
                }
            }

            var renderer = nodeObj.GetComponent<Renderer>();
            if (renderer == null)
            {
                var filter = nodeObj.AddComponent<MeshFilter>();
                filter.sharedMesh = unityMesh;
                renderer = nodeObj.AddComponent<MeshRenderer>();
                renderer.sharedMaterials = materials;
            }

        }
    }
}
