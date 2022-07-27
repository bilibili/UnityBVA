using GLTF;
using GLTF.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BVA.Cache;
using BVA.Extensions;
#if UNITY_STANDALONE
using Draco.Encoder;
using Draco;
#endif
using UnityEngine;
using UnityEngine.Rendering;

namespace BVA
{
    // Known issues, submeshs while first primitive is not contains all vertex
    public partial class GLTFSceneImporter
    {
        /// <summary>
        /// Load a Mesh from the glTF by index
        /// </summary>
        /// <param name="meshIndex"></param>
        /// <returns></returns>
        public virtual async Task<Mesh> LoadMeshAsync(int meshIndex, System.Threading.CancellationToken cancellationToken)
        {
            await SetupLoad(async () =>
            {
                if (meshIndex < 0 || meshIndex >= _gltfRoot.Meshes.Count)
                {
                    throw new System.ArgumentException($"There is no mesh for index {meshIndex}");
                }

                if (_assetCache.MeshCache[meshIndex] == null)
                {
                    var def = _gltfRoot.Meshes[meshIndex];

                    await ConstructMeshAttributes(def, new MeshId() { Id = meshIndex, Root = _gltfRoot });
                    await ConstructMesh(def, meshIndex);
                }
            });
            return _assetCache.MeshCache[meshIndex].LoadedMesh;
        }
#if UNITY_STANDALONE
        private bool UseDracoMeshCompression(MeshPrimitive meshPrim)
        {
            return _gltfRoot.ExtensionsUsed != null && meshPrim.Extensions != null && meshPrim.Extensions.ContainsKey(KHR_draco_mesh_compression_ExtensionFactory.EXTENSION_NAME);
        }
#endif
        /// <summary>
        /// Triggers loading, converting, and constructing of a UnityEngine.Mesh, and stores it in the asset cache
        /// </summary>
        /// <param name="mesh">The definition of the mesh to generate</param>
        /// <param name="meshIndex">The index of the mesh to generate</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task that completes when the mesh is attached to the given GameObject</returns>
        protected virtual async Task ConstructMesh(GLTFMesh mesh, int meshIndex)
        {
            if (_assetCache.MeshCache[meshIndex] == null)
            {
                throw new System.Exception("Cannot generate mesh before ConstructMeshAttributes is called!");
            }
            else if (_assetCache.MeshCache[meshIndex].LoadedMesh)
            {
                return;
            }

            List<MeshPrimitive> meshPrimitives = mesh.Primitives;
            uint totalVertCount = meshPrimitives.Aggregate((uint)0, (sum, p) => sum + p.Attributes[SemanticProperties.POSITION].Value.Count);
            uint firstPrimVertexCount = meshPrimitives.First().Attributes[SemanticProperties.POSITION].Value.Count;
            // regard first primitive contains all meshes vertex
            bool firstPrimContainsAll = totalVertCount / firstPrimVertexCount == meshPrimitives.Count && totalVertCount % firstPrimVertexCount == 0;

            MeshPrimitive firstPrim = meshPrimitives[0];
            MeshCacheData meshCache = _assetCache.MeshCache[meshIndex];
#if UNITY_STANDALONE
            if (UseDracoMeshCompression(firstPrim))
            {
                var extention = firstPrim.Extensions[KHR_draco_mesh_compression_ExtensionFactory.EXTENSION_NAME] as KHR_draco_mesh_compressionExtension;
                BufferId bufferId = extention.bufferView.Value.Buffer;
                BufferCacheData bufferData = await GetBufferData(bufferId);
                GLTFHelpers.LoadBufferView(extention.bufferView.Value, bufferData, out byte[] dracoData);

                DracoMeshLoader meshLoader = new DracoMeshLoader(true);
                Mesh unityMesh = await meshLoader.ConvertDracoMeshToUnity(dracoData);
                unityMesh.name = mesh.Name;

                _assetCache.MeshCache[meshIndex].LoadedMesh = unityMesh;

                for (int i = 0; i < meshPrimitives.Count; ++i)
                {
                    GLTFMaterial materialToLoad = meshPrimitives[i].Material.Value;
                    await ConstructMaterial(materialToLoad, meshPrimitives[i].Material.Id);
                }

                unityMesh.RecalculateBounds();
                if (unityMesh.normals == null && unityMesh.GetTopology(0) == MeshTopology.Triangles)
                {
                    unityMesh.RecalculateNormals();
                }

                if (!KeepCPUCopyOfMesh)
                {
                    unityMesh.UploadMeshData(true);
                }
            }
            else
#endif
            {
                if (firstPrimContainsAll)
                {
                    UnityMeshData unityData = new UnityMeshData()
                    {
                        Vertices = new Vector3[firstPrimVertexCount],
                        Normals = firstPrim.Attributes.ContainsKey(SemanticProperties.NORMAL) ? new Vector3[firstPrimVertexCount] : null,
                        Tangents = firstPrim.Attributes.ContainsKey(SemanticProperties.TANGENT) ? new Vector4[firstPrimVertexCount] : null,
                        Uv1 = firstPrim.Attributes.ContainsKey(SemanticProperties.TEXCOORD_0) ? new Vector2[firstPrimVertexCount] : null,
                        // for decal projector, it might generate uv2 for use
                        Uv2 = meshPrimitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_1)) ? new Vector2[firstPrimVertexCount] : null,
                        Uv3 = meshPrimitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_2)) ? new Vector2[firstPrimVertexCount] : null,
                        Uv4 = meshPrimitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_3)) ? new Vector2[firstPrimVertexCount] : null,
                        Uv5 = meshPrimitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_4)) ? new Vector2[firstPrimVertexCount] : null,
                        Uv6 = meshPrimitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_5)) ? new Vector2[firstPrimVertexCount] : null,
                        Uv7 = meshPrimitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_6)) ? new Vector2[firstPrimVertexCount] : null,
                        Uv8 = meshPrimitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_7)) ? new Vector2[firstPrimVertexCount] : null,
                        BoneWeights = firstPrim.Attributes.ContainsKey(SemanticProperties.WEIGHTS_0) ? new BoneWeight[firstPrimVertexCount] : null,
                        Colors = firstPrim.Attributes.ContainsKey(SemanticProperties.COLOR_0) ? new Color[firstPrimVertexCount] : null,

                        MorphTargetVertices = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.POSITION) ? Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, firstPrimVertexCount) : null,
                        MorphTargetNormals = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.NORMAL) ? Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, firstPrimVertexCount) : null,
                        MorphTargetTangents = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.TANGENT) ? Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, firstPrimVertexCount) : null,

                        Topology = new MeshTopology[meshPrimitives.Count],
                        Indices = new int[meshPrimitives.Count][],
                        FirstPrimContainsAllVertex = firstPrimContainsAll
                    };

                    ConvertAttributeAccessorsToUnityTypes(meshCache.Primitives[0], unityData, 0);

                    for (int i = 0; i < meshPrimitives.Count; ++i)
                    {
                        var primitive = meshPrimitives[i];
                        uint vertCount = primitive.Attributes[SemanticProperties.POSITION].Value.Count;
                        var primCache = meshCache.Primitives[i];
                        unityData.Topology[i] = GetTopology(primitive.Mode);
                        var meshAttributes = primCache.Attributes;


                        int[] indices = unityData.Topology[i] == MeshTopology.Triangles ? meshAttributes.ContainsKey(SemanticProperties.INDICES) ?
                        meshAttributes[SemanticProperties.INDICES].AccessorContent.AsUInts.ToIntArrayFlipTriangleFaces() : MeshPrimitive.GenerateIndices(vertCount) :
                         meshAttributes.ContainsKey(SemanticProperties.INDICES) ? meshAttributes[SemanticProperties.INDICES].AccessorContent.AsUInts.ToIntArrayRaw() : MeshPrimitive.GenerateIndices(vertCount);

                        int truncate = indices.Length % 3;
                        if (truncate != 0)
                            indices = indices.Take(indices.Length - truncate).ToArray();

                        unityData.Indices[i] = indices;

                        bool shouldUseDefaultMaterial = primitive.Material == null;
                        GLTFMaterial materialToLoad = shouldUseDefaultMaterial ? DefaultMaterial : primitive.Material.Value;
                        if ((shouldUseDefaultMaterial && _defaultLoadedMaterial == null) || (!shouldUseDefaultMaterial && _assetCache.MaterialCache[primitive.Material.Id] == null))
                        {
                            await ConstructMaterial(materialToLoad, shouldUseDefaultMaterial ? -1 : primitive.Material.Id);
                        }


                        if (unityData.Topology[i] == MeshTopology.Triangles && primitive.Indices != null && primitive.Indices.Value != null)
                        {
                            Statistics.TriangleCount += primitive.Indices.Value.Count / 3;
                        }
                    }
                    Statistics.VertexCount += firstPrimVertexCount;

                    ConstructUnityMesh(unityData, meshIndex, mesh.Name, firstPrimContainsAll);
                }
                else
                {
                    int vertOffset = 0;
                    UnityMeshData unityData = new UnityMeshData()
                    {
                        Vertices = new Vector3[totalVertCount],
                        Normals = firstPrim.Attributes.ContainsKey(SemanticProperties.NORMAL) ? new Vector3[totalVertCount] : null,
                        Tangents = firstPrim.Attributes.ContainsKey(SemanticProperties.TANGENT) ? new Vector4[totalVertCount] : null,
                        Uv1 = firstPrim.Attributes.ContainsKey(SemanticProperties.TEXCOORD_0) ? new Vector2[totalVertCount] : null,
                        // for decal projector, it might generate uv2 for use
                        Uv2 = mesh.Primitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_1)) ? new Vector2[totalVertCount] : null,
                        Uv3 = mesh.Primitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_2)) ? new Vector2[totalVertCount] : null,
                        Uv4 = mesh.Primitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_3)) ? new Vector2[totalVertCount] : null,
                        Uv5 = mesh.Primitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_4)) ? new Vector2[totalVertCount] : null,
                        Uv6 = mesh.Primitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_5)) ? new Vector2[totalVertCount] : null,
                        Uv7 = mesh.Primitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_6)) ? new Vector2[totalVertCount] : null,
                        Uv8 = mesh.Primitives.Any((x) => x.Attributes.ContainsKey(SemanticProperties.TEXCOORD_7)) ? new Vector2[totalVertCount] : null,
                        BoneWeights = firstPrim.Attributes.ContainsKey(SemanticProperties.WEIGHTS_0) ? new BoneWeight[totalVertCount] : null,
                        Colors = firstPrim.Attributes.ContainsKey(SemanticProperties.COLOR_0) ? new Color[totalVertCount] : null,

                        MorphTargetVertices = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.POSITION) ?
                        Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, totalVertCount) : null,
                        MorphTargetNormals = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.NORMAL) ?
                        Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, totalVertCount) : null,
                        MorphTargetTangents = firstPrim.Targets != null && firstPrim.Targets[0].ContainsKey(SemanticProperties.TANGENT) ?
                        Allocate2dArray<Vector3>((uint)firstPrim.Targets.Count, totalVertCount) : null,

                        Topology = new MeshTopology[mesh.Primitives.Count],
                        Indices = new int[mesh.Primitives.Count][],
                        FirstPrimContainsAllVertex = firstPrimContainsAll
                    };
                    for (int i = 0; i < mesh.Primitives.Count; ++i)
                    {
                        var primitive = mesh.Primitives[i];
                        var primCache = meshCache.Primitives[i];
                        unityData.Topology[i] = GetTopology(primitive.Mode);

                        ConvertAttributeAccessorsToUnityTypes(primCache, unityData, vertOffset, i);

                        bool shouldUseDefaultMaterial = primitive.Material == null;
                        GLTFMaterial materialToLoad = shouldUseDefaultMaterial ? DefaultMaterial : primitive.Material.Value;
                        if ((shouldUseDefaultMaterial && _defaultLoadedMaterial == null) ||
                            (!shouldUseDefaultMaterial && _assetCache.MaterialCache[primitive.Material.Id] == null))
                        {
                            await ConstructMaterial(materialToLoad, shouldUseDefaultMaterial ? -1 : primitive.Material.Id);
                        }

                        var vertCount = primitive.Attributes[SemanticProperties.POSITION].Value.Count;
                        vertOffset += (int)vertCount;

                        if (unityData.Topology[i] == MeshTopology.Triangles && primitive.Indices != null && primitive.Indices.Value != null)
                        {
                            Statistics.TriangleCount += primitive.Indices.Value.Count / 3;
                        }
                    }
                    Statistics.VertexCount += vertOffset;
                    ConstructUnityMesh(unityData, meshIndex, mesh.Name, firstPrimContainsAll);
                }
            }
        }

        private async Task ConstructMeshAttributes(GLTFMesh mesh, MeshId meshId)
        {
            int meshIndex = meshId.Id;

            if (_assetCache.MeshCache[meshIndex] == null)
                _assetCache.MeshCache[meshIndex] = new MeshCacheData();
            else if (_assetCache.MeshCache[meshIndex].Primitives.Count > 0)
                return;

            for (int i = 0; i < mesh.Primitives.Count; ++i)
            {
                MeshPrimitive primitive = mesh.Primitives[i];
                await ConstructPrimitiveAttributes(primitive, meshIndex, i);

                if (primitive.Material != null)
                {
                    await ConstructMaterialImageBuffers(primitive.Material.Value);
                }
                //await ConstructMeshTargets(primitive, meshIndex, i); 
            }
            //each primitive shared the same targets,so just construct once and copy to others
            if (mesh.Primitives[0].Targets != null)
            {
                // read mesh primitive targets into assetcache
                await ConstructMeshTargets(mesh.Primitives[0], meshIndex, 0);
                for (int i = 1; i < mesh.Primitives.Count; i++)
                    _assetCache.MeshCache[meshIndex].Primitives[i].Targets = _assetCache.MeshCache[meshIndex].Primitives[0].Targets;
            }
        }
        protected void ConvertAttributeAccessorsToUnityTypes(MeshCacheData.PrimitiveCacheData primData, UnityMeshData unityData, int vertOffset, int indexOffset)
        {
            // todo optimize: There are multiple copies being performed to turn the buffer data into mesh data. Look into reducing them
            var meshAttributes = primData.Attributes;
            uint vertexCount = meshAttributes[SemanticProperties.POSITION].AccessorId.Value.Count;

            var indices = meshAttributes.ContainsKey(SemanticProperties.INDICES)
                ? meshAttributes[SemanticProperties.INDICES].AccessorContent.AsUInts.ToIntArrayRaw()
                : MeshPrimitive.GenerateIndices(vertexCount);
            if (unityData.Topology[indexOffset] == MeshTopology.Triangles)
                SchemaExtensions.FlipTriangleFaces(indices);

            int truncate = indices.Length % 3;
            if (truncate != 0)
                indices = indices.Take(indices.Length - truncate).ToArray();
            unityData.Indices[indexOffset] = indices;

            if (meshAttributes.ContainsKey(SemanticProperties.Weight[0]) && meshAttributes.ContainsKey(SemanticProperties.Joint[0]))
            {
                CreateBoneWeightArray(
                    meshAttributes[SemanticProperties.Joint[0]].AccessorContent.AsVec4s.ToUnityVector4Raw(),
                    meshAttributes[SemanticProperties.Weight[0]].AccessorContent.AsVec4s.ToUnityVector4Raw(),
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
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[2]))
            {
                meshAttributes[SemanticProperties.TexCoord[2]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv3, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[3]))
            {
                meshAttributes[SemanticProperties.TexCoord[3]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv4, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[4]))
            {
                meshAttributes[SemanticProperties.TexCoord[4]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv5, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[5]))
            {
                meshAttributes[SemanticProperties.TexCoord[5]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv6, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[6]))
            {
                meshAttributes[SemanticProperties.TexCoord[6]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv7, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[7]))
            {
                meshAttributes[SemanticProperties.TexCoord[7]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv8, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.Color[0]))
            {
                meshAttributes[SemanticProperties.Color[0]].AccessorContent.AsColors.ToUnityColorRaw(unityData.Colors, vertOffset);
            }

            var targets = primData.Targets;
            if (targets != null)
            {
                for (int i = 0; i < targets.Count; ++i)
                {
                    if (targets[i].ContainsKey(SemanticProperties.POSITION))
                    {
                        targets[i][SemanticProperties.POSITION].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetVertices[i], vertOffset);
                    }
                    if (targets[i].ContainsKey(SemanticProperties.NORMAL))
                    {
                        targets[i][SemanticProperties.NORMAL].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetNormals[i], vertOffset);
                    }
                    if (targets[i].ContainsKey(SemanticProperties.TANGENT))
                    {
                        targets[i][SemanticProperties.TANGENT].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetTangents[i], vertOffset);
                    }
                }
            }
        }
        protected void ConvertAttributeAccessorsToUnityTypes(MeshCacheData.PrimitiveCacheData primData, UnityMeshData unityData, int vertOffset)
        {
            // todo optimize: There are multiple copies being performed to turn the buffer data into mesh data. Look into reducing them
            var meshAttributes = primData.Attributes;

            if (meshAttributes.ContainsKey(SemanticProperties.Weight[0]) && meshAttributes.ContainsKey(SemanticProperties.Joint[0]))
            {
                CreateBoneWeightArray(meshAttributes[SemanticProperties.Joint[0]].AccessorContent.AsVec4s.ToUnityVector4Raw(), meshAttributes[SemanticProperties.Weight[0]].AccessorContent.AsVec4s.ToUnityVector4Raw(), ref unityData.BoneWeights, vertOffset);
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
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[2]))
            {
                meshAttributes[SemanticProperties.TexCoord[2]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv3, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[3]))
            {
                meshAttributes[SemanticProperties.TexCoord[3]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv4, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[4]))
            {
                meshAttributes[SemanticProperties.TexCoord[4]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv5, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[5]))
            {
                meshAttributes[SemanticProperties.TexCoord[5]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv6, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[6]))
            {
                meshAttributes[SemanticProperties.TexCoord[6]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv7, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.TexCoord[7]))
            {
                meshAttributes[SemanticProperties.TexCoord[7]].AccessorContent.AsTexcoords.ToUnityVector2Raw(unityData.Uv8, vertOffset);
            }
            if (meshAttributes.ContainsKey(SemanticProperties.Color[0]))
            {
                meshAttributes[SemanticProperties.Color[0]].AccessorContent.AsColors.ToUnityColorRaw(unityData.Colors, vertOffset);
            }

            var targets = primData.Targets;
            if (targets != null)
            {
                for (int i = 0; i < targets.Count; ++i)
                {
                    if (targets[i].ContainsKey(SemanticProperties.POSITION))
                    {
                        targets[i][SemanticProperties.POSITION].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetVertices[i], vertOffset);
                    }
                    if (targets[i].ContainsKey(SemanticProperties.NORMAL))
                    {
                        targets[i][SemanticProperties.NORMAL].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetNormals[i], vertOffset);
                    }
                    if (targets[i].ContainsKey(SemanticProperties.TANGENT))
                    {
                        targets[i][SemanticProperties.TANGENT].AccessorContent.AsVec3s.ToUnityVector3Raw(unityData.MorphTargetTangents[i], vertOffset);
                    }
                }
            }
        }

        /// <summary>
        /// Populate a UnityEngine.Mesh from preloaded and preprocessed buffer data
        /// </summary>
        /// <param name="meshConstructionData"></param>
        /// <param name="meshId"></param>
        /// <param name="primitiveIndex"></param>
        /// <param name="unityMeshData"></param>
        /// <returns></returns>
        protected void ConstructUnityMesh(UnityMeshData unityMeshData, int meshIndex, string meshName, bool firstPrimContainsAll)
        {
            Mesh mesh = new Mesh
            {
                name = meshName,
                indexFormat = unityMeshData.Vertices.Length > UInt16.MaxValue ? IndexFormat.UInt32 : IndexFormat.UInt16
            };

            mesh.vertices = unityMeshData.Vertices;
            mesh.normals = unityMeshData.Normals;
            mesh.tangents = unityMeshData.Tangents;
            mesh.uv = unityMeshData.Uv1;
            mesh.uv2 = unityMeshData.Uv2;
            mesh.uv3 = unityMeshData.Uv3;
            mesh.uv4 = unityMeshData.Uv4;
            mesh.uv5 = unityMeshData.Uv5;
            mesh.uv6 = unityMeshData.Uv6;
            mesh.uv7 = unityMeshData.Uv7;
            mesh.uv8 = unityMeshData.Uv8;
            mesh.colors = unityMeshData.Colors;
            mesh.boneWeights = unityMeshData.BoneWeights;
            mesh.subMeshCount = unityMeshData.Indices.Length;

            if (firstPrimContainsAll)
            {
                for (int i = 0; i < unityMeshData.Indices.Length; i++)
                {
                    mesh.SetTriangles(unityMeshData.Indices[i], i, false);
                }
            }
            else
            {
                uint baseVertex = 0;
                for (int i = 0; i < unityMeshData.Indices.Length; i++)
                {
                    mesh.SetIndices(unityMeshData.Indices[i], unityMeshData.Topology[i], i, false, (int)baseVertex);
                    baseVertex += _assetCache.MeshCache[meshIndex].Primitives[i].Attributes[SemanticProperties.POSITION].AccessorId.Value.Count;
                }
            }

            mesh.RecalculateBounds();

            if (unityMeshData.MorphTargetVertices != null)
            {
                var firstPrim = _gltfRoot.Meshes[meshIndex].Primitives[0];
                for (int i = 0; i < firstPrim.Targets.Count; i++)
                {
                    var targetName = firstPrim.TargetNames != null ? firstPrim.TargetNames[i] : $"Morphtarget{i}";
                    mesh.AddBlendShapeFrame(targetName, 100,
                        unityMeshData.MorphTargetVertices[i],
                        unityMeshData.MorphTargetNormals != null ? unityMeshData.MorphTargetNormals[i] : null,
                        unityMeshData.MorphTargetTangents != null ? unityMeshData.MorphTargetTangents[i] : null
                    );
                }
            }

            if (unityMeshData.Normals == null && unityMeshData.Topology[0] == MeshTopology.Triangles)
            {
                mesh.RecalculateNormals();
            }

            if (!KeepCPUCopyOfMesh)
            {
                mesh.UploadMeshData(false);
            }

            _assetCache.MeshCache[meshIndex].LoadedMesh = mesh;
        }
        /// <summary>
        /// Submesh share the blenshapes AttributeAccessor, so what we need is just to read the first one
        /// </summary>
        /// <param name="primitive"></param>
        /// <param name="meshIndex"></param>
        /// <param name="primitiveIndex"></param>
        /// <param name="shareTargets">should share the blendshape targets</param>
        /// <returns></returns>
        protected async Task ConstructMeshTargets(MeshPrimitive primitive, int meshIndex, int primitiveIndex)
        {
            var newTargets = new List<Dictionary<string, AttributeAccessor>>(primitive.Targets.Count);
            _assetCache.MeshCache[meshIndex].Primitives[primitiveIndex].Targets = newTargets;
            async Task SetTarget(int i)
            {
                var target = primitive.Targets[i];
                newTargets.Add(new Dictionary<string, AttributeAccessor>());

                //NORMALS, POSITIONS, TANGENTS
                foreach (var targetAttribute in target)
                {
                    BufferId bufferIdPair = targetAttribute.Value.Value.BufferView.Value.Buffer;
                    GLTFBuffer buffer = bufferIdPair.Value;
                    int bufferID = bufferIdPair.Id;

                    if (_assetCache.BufferCache[bufferID] == null)
                    {
                        await ConstructBuffer(buffer, bufferID);
                    }

                    newTargets[i][targetAttribute.Key] = new AttributeAccessor
                    {
                        AccessorId = targetAttribute.Value,
                        Stream = _assetCache.BufferCache[bufferID].Stream,
                        Offset = (uint)_assetCache.BufferCache[bufferID].ChunkOffset
                    };
                }
                LoadMeshTargetAttribute(newTargets[i]);
            }
            for (int i = 0; i < primitive.Targets.Count; i++)
            {
                await SetTarget(i);
            }
        }

        // Flip vectors to Unity coordinate system
        private void TransformTargets(ref Dictionary<string, AttributeAccessor> attributeAccessors)
        {
            if (attributeAccessors.ContainsKey(SemanticProperties.POSITION))
            {
                AttributeAccessor attributeAccessor = attributeAccessors[SemanticProperties.POSITION];
                SchemaExtensions.ConvertVector3CoordinateSpace(ref attributeAccessor, SchemaExtensions.CoordinateSpaceConversionScale);
            }

            if (attributeAccessors.ContainsKey(SemanticProperties.NORMAL))
            {
                AttributeAccessor attributeAccessor = attributeAccessors[SemanticProperties.NORMAL];
                SchemaExtensions.ConvertVector3CoordinateSpace(ref attributeAccessor, SchemaExtensions.CoordinateSpaceConversionScale);
            }

            if (attributeAccessors.ContainsKey(SemanticProperties.TANGENT))
            {
                AttributeAccessor attributeAccessor = attributeAccessors[SemanticProperties.TANGENT];
                SchemaExtensions.ConvertVector3CoordinateSpace(ref attributeAccessor, SchemaExtensions.CoordinateSpaceConversionScale);
            }
        }

        protected async Task ConstructPrimitiveAttributes(MeshPrimitive primitive, int meshIndex, int primitiveIndex)
        {
#if UNITY_STANDALONE
            if (UseDracoMeshCompression(primitive))
                return;
#endif
            var primData = new MeshCacheData.PrimitiveCacheData();
            _assetCache.MeshCache[meshIndex].Primitives.Add(primData);

            Dictionary<string, AttributeAccessor> attributeAccessors = primData.Attributes;

            foreach (var attributePair in primitive.Attributes)
            {
                var bufferId = attributePair.Value.Value.BufferView.Value.Buffer;

                var bufferData = await GetBufferData(bufferId);

                attributeAccessors[attributePair.Key] = new AttributeAccessor
                {
                    AccessorId = attributePair.Value,
                    Stream = bufferData.Stream,
                    Offset = (uint)bufferData.ChunkOffset
                };
            }

            if (primitive.Indices != null)
            {
                var bufferId = primitive.Indices.Value.BufferView.Value.Buffer;
                var bufferData = await GetBufferData(bufferId);

                attributeAccessors[SemanticProperties.INDICES] = new AttributeAccessor
                {
                    AccessorId = primitive.Indices,
                    Stream = bufferData.Stream,
                    Offset = (uint)bufferData.ChunkOffset
                };
            }
            try
            {
                GLTFHelpers.BuildMeshAttributes(ref attributeAccessors);
            }
            catch (LoadException e)
            {
                LogPool.ImportLogger.LogWarning(LogPart.Mesh, e.ToString());
            }
            TransformAttributes(ref attributeAccessors);
        }

        protected void TransformAttributes(ref Dictionary<string, AttributeAccessor> attributeAccessors)
        {
            foreach (var name in attributeAccessors.Keys)
            {
                var aa = attributeAccessors[name];
                switch (name)
                {
                    case SemanticProperties.POSITION:
                    case SemanticProperties.NORMAL:
                        SchemaExtensions.ConvertVector3CoordinateSpace(ref aa, SchemaExtensions.CoordinateSpaceConversionScale);
                        break;
                    case SemanticProperties.TANGENT:
                        SchemaExtensions.ConvertVector4CoordinateSpace(ref aa, SchemaExtensions.TangentSpaceConversionScale);
                        break;
                    case SemanticProperties.TEXCOORD_0:
                    case SemanticProperties.TEXCOORD_1:
                    case SemanticProperties.TEXCOORD_2:
                    case SemanticProperties.TEXCOORD_3:
                    case SemanticProperties.TEXCOORD_4:
                    case SemanticProperties.TEXCOORD_5:
                    case SemanticProperties.TEXCOORD_6:
                    case SemanticProperties.TEXCOORD_7:
                    case SemanticProperties.TEXCOORD_8:
                    case SemanticProperties.TEXCOORD_9:
                        SchemaExtensions.FlipTexCoordArrayV(ref aa);
                        break;
                }
            }
        }

    }
    public partial class GLTFSceneExporter
    {
        public static readonly int[] intArrStd = new int[3] { 0, 1, 2 };
        public struct Vector4Int
        {
            public static Vector4Int zero { get { return new Vector4Int(0, 0, 0, 0); } }
            public static Vector4Int one { get { return new Vector4Int(1, 1, 1, 1); } }

            public int x, y, z, w;
            public Vector4Int(int _x, int _y, int _z, int _w)
            {
                x = _x;
                y = _y;
                z = _z;
                w = _w;
            }
        }
        public static Vector2[] RemapTextureAtlasCoordinates(Vector2[] source, int index, Rect[] uvs, bool lerp = false)
        {
            Vector2[] array = new Vector2[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                if (lerp)
                {
                    float xMin = uvs[index].xMin;
                    float xMax = uvs[index].xMax;
                    float yMin = uvs[index].yMin;
                    float yMax = uvs[index].yMax;
                    array[i].x = Mathf.Lerp(xMin, xMax, source[i].x);
                    array[i].y = Mathf.Lerp(yMin, yMax, source[i].y);
                }
                else
                {
                    array[i].x = source[i].x * uvs[index].width + uvs[index].x;
                    array[i].y = source[i].y * uvs[index].height + uvs[index].y;
                }
            }
            return array;
        }
        public static Vector2[] UpdateLightmapCoords(Mesh mesh, int lightmapIndex, Vector4 lightmapOffset)
        {
            Vector2[] result;
            if (lightmapIndex >= 0 && lightmapIndex <= 65000)
            {
                result = RemapTextureAtlasCoordinates(mesh.uv2, 0, new Rect[]
                {
                    new Rect(lightmapOffset.z, lightmapOffset.w, lightmapOffset.x, lightmapOffset.y)
                }, false);
            }
            else
            {
                result = mesh.uv2;
            }
            return result;
        }

        private MeshId ExportMesh(string name, GameObject[] primitives)
        {
            // check if this set of primitives is already a mesh
            MeshId existingMeshId = null;
            var key = new PrimKey();
            foreach (var prim in primitives)
            {
                var smr = prim.GetComponent<SkinnedMeshRenderer>();
                if (smr != null)
                {
                    key.Mesh = smr.sharedMesh;
                    key.Material = smr.sharedMaterial;
                }
                else
                {
                    var filter = prim.GetComponent<MeshFilter>();
                    var renderer = prim.GetComponent<MeshRenderer>();
                    key.Mesh = filter.sharedMesh;
                    key.Material = renderer.sharedMaterial;
                }

                if (_primOwner.TryGetValue(key, out MeshId tempMeshId) && (existingMeshId == null || tempMeshId == existingMeshId))
                {
                    existingMeshId = tempMeshId;
                }
                else
                {
                    existingMeshId = null;
                    break;
                }
            }

            // if so, return that mesh id
            if (existingMeshId != null)
            {
                return existingMeshId;
            }

            // if not, create new mesh and return its id
            var mesh = new GLTFMesh();

            if (ExportNames)
            {
                mesh.Name = name;
            }

            mesh.Primitives = new List<MeshPrimitive>(primitives.Length);
            foreach (var prim in primitives)
            {
                MeshPrimitive[] meshPrimitives = ExportPrimitive(prim, mesh);
                if (meshPrimitives != null)
                {
                    mesh.Primitives.AddRange(meshPrimitives);
                }
            }

            var id = new MeshId
            {
                Id = _root.Meshes.Count,
                Root = _root
            };
            _root.Meshes.Add(mesh);

            return id;
        }

        // don't support Skinned Mesh,and Submesh more than one primitive
        private bool ShouldCompressionMesh(Mesh meshObj, Renderer renderer, MeshPrimitive[] primitives)
        {
            return DracoMeshCompression && !(renderer is SkinnedMeshRenderer) && primitives.Length == 1 && meshObj.GetTopology(0) == MeshTopology.Triangles;
        }

        // a mesh *might* decode to multiple prims if there are submeshes
        private MeshPrimitive[] ExportPrimitive(GameObject gameObject, GLTFMesh mesh)
        {
            Mesh meshObj;
            SkinnedMeshRenderer smr = null;

            var filter = gameObject.GetComponent<MeshFilter>();
            if (filter != null)
            {
                meshObj = filter.sharedMesh;
            }
            else
            {
                smr = gameObject.GetComponent<SkinnedMeshRenderer>();
                meshObj = smr.sharedMesh;
            }
            if (meshObj == null)
            {
                LogPool.ExportLogger.LogWarning(LogPart.Mesh, string.Format("MeshFilter.sharedMesh on gameobject:{0} is missing , skipping", gameObject.name));
                return null;
            }

            var renderer = gameObject.GetComponent<MeshRenderer>();
            var materialsObj = renderer != null ? renderer.sharedMaterials : smr.sharedMaterials;

            if (materialsObj.Length != meshObj.subMeshCount)
            {
                throw new Exception($"{meshObj.name} subMeshCount is not equal to Materials Length, please make sure they have equal count\nsubMeshCount={meshObj.subMeshCount},Materials Length={materialsObj.Length}");
            }
            var prims = new MeshPrimitive[meshObj.subMeshCount];

            // don't export any more accessors if this mesh is already exported
            if (_meshToPrims.TryGetValue(meshObj, out MeshPrimitive[] primVariations)
                && meshObj.subMeshCount == primVariations.Length)
            {
                for (var i = 0; i < primVariations.Length; i++)
                {
                    prims[i] = new MeshPrimitive(primVariations[i], _root)
                    {
                        Material = ExportMaterial(materialsObj[i])
                    };
                }

                return prims;
            }

            AccessorId aPosition = null, aNormal = null, aTangent = null,
                aTexcoord0 = null, aTexcoord1 = null, aColor0 = null,
                aJoints0 = null, aWeights0 = null;
#if UNITY_STANDALONE
            if (ShouldCompressionMesh(meshObj, smr, prims))
            {
                var rightHandMesh = ConvertLeftHandToRightHand(meshObj);

                var EncodeResultDatas = DracoEncoder.EncodeMesh(rightHandMesh);

                AccessorId[] dracoDataID = new AccessorId[prims.Length];
                for (int i = 0; i < prims.Length; i++)
                {
                    dracoDataID[i] = ExportAccessor(EncodeResultDatas[i].data.ToArray());
                    EncodeResultDatas[i].Dispose();

                }
                VertexAttributeDescriptor[] Attributes = meshObj.GetVertexAttributes();

                MaterialId lastMaterialId = new MaterialId();
                aPosition = GetDracoAccessorID(meshObj.vertices);

                if (Attributes.Any(a => a.attribute == VertexAttribute.Normal))
                    aNormal = GetDracoAccessorID(meshObj.normals);

                if (ExportTangent && Attributes.Any(a => a.attribute == VertexAttribute.Tangent))
                    aTangent = GetDracoAccessorID(meshObj.tangents);

                if (Attributes.Any(a => a.attribute == VertexAttribute.TexCoord0))
                    aTexcoord0 = GetDracoAccessorID(meshObj.uv);

                if (Attributes.Any(a => a.attribute == VertexAttribute.TexCoord1))
                    aTexcoord1 = GetDracoAccessorID(meshObj.uv2);

                if (ExportVertexColor && Attributes.Any(a => a.attribute == VertexAttribute.Color))
                    aColor0 = GetDracoAccessorID(meshObj.colors);

                // export JOINTS,WEIGHTS attribute ,for skin
                // skin is not supported yet in draco
                //if (IsValidSkinMeshRenderer(smr) && Attributes.Any(a => a.attribute == VertexAttribute.BlendIndices))
                //{
                //    Vector4Int[] joints = new Vector4Int[meshObj.vertices.Length];
                //    Vector4[] weights = new Vector4[meshObj.vertices.Length];
                //    SetupBoneWeights(smr, ref joints, ref weights);
                //    aJoints0 = GetDracoAccessorID(joints);
                //    aWeights0 = GetDracoAccessorID(weights);
                //}

                for (var submesh = 0; submesh < prims.Length; submesh++)
                {
                    var primitive = new MeshPrimitive();

                    var extentionAttributes = new Dictionary<string, AccessorId>();
                    var extensionsBufferView = dracoDataID[submesh].Value.BufferView;

                    var topology = meshObj.GetTopology(submesh);
                    var indices = meshObj.GetIndices(submesh);
                    if (topology == MeshTopology.Triangles) SchemaExtensions.FlipTriangleFaces(indices);

                    primitive.Mode = GetDrawMode(meshObj.GetTopology(submesh));
                    primitive.Indices = GetDracoAccessorID(indices, true);

                    primitive.Attributes = new Dictionary<string, AccessorId>();
                    primitive.Attributes.Add(SemanticProperties.POSITION, aPosition);
                    if (aNormal != null) primitive.Attributes.Add(SemanticProperties.NORMAL, aNormal);
                    if (aTangent != null) primitive.Attributes.Add(SemanticProperties.TANGENT, aTangent);
                    if (aTexcoord0 != null) primitive.Attributes.Add(SemanticProperties.TEXCOORD_0, aTexcoord0);
                    if (aTexcoord1 != null) primitive.Attributes.Add(SemanticProperties.TEXCOORD_1, aTexcoord1);
                    if (aColor0 != null) primitive.Attributes.Add(SemanticProperties.COLOR_0, aColor0);
                    if (aJoints0 != null) primitive.Attributes.Add(SemanticProperties.JOINTS_0, aJoints0);
                    if (aWeights0 != null) primitive.Attributes.Add(SemanticProperties.WEIGHTS_0, aWeights0);

                    int index = 0;
                    foreach (var item in Attributes)
                    {
                        string name = SemanticProperties.VertexAttributeName_Convert_GLTFName[item.attribute.ToString()];
                        extentionAttributes.Add(name, new AccessorId() { Root = _root, Id = index++ });
                    }

                    if (submesh < materialsObj.Length)
                    {
                        primitive.Material = ExportMaterial(materialsObj[submesh]);
                        lastMaterialId = primitive.Material;
                    }
                    else
                    {
                        primitive.Material = lastMaterialId;
                    }
                    var extension = new KHR_draco_mesh_compressionExtension(extentionAttributes, extensionsBufferView);

                    primitive.AddExtension(_root, KHR_draco_mesh_compression_ExtensionFactory.EXTENSION_NAME, extension, true);

                    prims[submesh] = primitive;
                }
            }
            else
#endif
            {
                aPosition = ExportAccessor(SchemaExtensions.ConvertVector3CoordinateSpaceAndCopy(meshObj.vertices, SchemaExtensions.CoordinateSpaceConversionScale));
                if (meshObj.normals.Length != 0)
                    aNormal = ExportAccessor(SchemaExtensions.ConvertVector3CoordinateSpaceAndCopyNormalized(meshObj.normals, SchemaExtensions.CoordinateSpaceConversionScale));

                if (meshObj.tangents.Length != 0)
                    aTangent = ExportAccessor(SchemaExtensions.ConvertVector4CoordinateSpaceAndCopy(meshObj.tangents, SchemaExtensions.TangentSpaceConversionScale));

                if (meshObj.uv.Length != 0)
                    aTexcoord0 = ExportAccessor(SchemaExtensions.FlipTexCoordArrayVAndCopy(meshObj.uv));

                if (meshObj.uv2.Length != 0)
                    aTexcoord1 = ExportAccessor(SchemaExtensions.FlipTexCoordArrayVAndCopy(meshObj.uv2));

                if (ExportVertexColor && meshObj.colors.Length != 0)
                    aColor0 = ExportAccessor(meshObj.colors);

                // export JOINTS,WEIGHTS attribute ,for skin
                if (IsValidSkinnedMeshRenderer(smr))
                {
                    Vector4Int[] joints = new Vector4Int[meshObj.vertices.Length];
                    Vector4[] weights = new Vector4[meshObj.vertices.Length];
                    SetupBoneWeights(smr, ref joints, ref weights);
                    aJoints0 = ExportAccessor(joints);
                    aWeights0 = ExportAccessor(weights);
                }

                MaterialId lastMaterialId = null;
                for (var submesh = 0; submesh < meshObj.subMeshCount; submesh++)
                {
                    var primitive = new MeshPrimitive();

                    var topology = meshObj.GetTopology(submesh);
                    var indices = meshObj.GetIndices(submesh);
                    if (topology == MeshTopology.Triangles) SchemaExtensions.FlipTriangleFaces(indices);

                    primitive.Mode = GetDrawMode(topology);
                    primitive.Indices = ExportAccessor(indices, true);

                    primitive.Attributes = new Dictionary<string, AccessorId>();
                    primitive.Attributes.Add(SemanticProperties.POSITION, aPosition);

                    if (aNormal != null)
                        primitive.Attributes.Add(SemanticProperties.NORMAL, aNormal);
                    if (aTangent != null)
                        primitive.Attributes.Add(SemanticProperties.TANGENT, aTangent);
                    if (aTexcoord0 != null)
                        primitive.Attributes.Add(SemanticProperties.TEXCOORD_0, aTexcoord0);
                    if (aTexcoord1 != null)
                        primitive.Attributes.Add(SemanticProperties.TEXCOORD_1, aTexcoord1);
                    if (aColor0 != null)
                        primitive.Attributes.Add(SemanticProperties.COLOR_0, aColor0);
                    if (aJoints0 != null)
                        primitive.Attributes.Add(SemanticProperties.JOINTS_0, aJoints0);
                    if (aWeights0 != null)
                        primitive.Attributes.Add(SemanticProperties.WEIGHTS_0, aWeights0);

                    if (submesh < materialsObj.Length)
                    {
                        primitive.Material = ExportMaterial(materialsObj[submesh]);
                        lastMaterialId = primitive.Material;
                    }
                    else
                    {
                        primitive.Material = lastMaterialId;
                    }

                    prims[submesh] = primitive;
                    if (ExportBlendShape)
                    {
                        if (submesh == 0)
                            ExportBlendShapes(smr, meshObj, primitive, mesh);
                        else
                        {

                            prims[submesh].Targets = prims[0].Targets;
                            prims[submesh].TargetNames = prims[0].TargetNames;
                        }
                    }
                }
            }

            _meshToPrims[meshObj] = prims;

            return prims;
        }


        // Blend Shapes / Morph Targets
        // Adopted from Gary Hsu (bghgary)
        // https://github.com/bghgary/glTF-Tools-for-Unity/blob/master/UnityProject/Assets/Gltf/Editor/Exporter.cs
        private void ExportBlendShapes(SkinnedMeshRenderer smr, Mesh meshObj, MeshPrimitive primitive, GLTFMesh mesh)
        {
            if (smr != null && meshObj.blendShapeCount > 0)
            {
                List<Dictionary<string, AccessorId>> targets = new List<Dictionary<string, AccessorId>>(meshObj.blendShapeCount);
                List<double> weights = new List<double>(meshObj.blendShapeCount);
                List<string> targetNames = new List<string>(meshObj.blendShapeCount);

                for (int blendShapeIndex = 0; blendShapeIndex < meshObj.blendShapeCount; blendShapeIndex++)
                {
                    targetNames.Add(meshObj.GetBlendShapeName(blendShapeIndex));
                    // As described above, a blend shape can have multiple frames. Given that glTF only supports a single frame
                    // per blend shape, we'll always use the final frame (the one that would be for when 100% weight is applied).
                    int frameIndex = meshObj.GetBlendShapeFrameCount(blendShapeIndex) - 1;

                    var deltaVertices = new Vector3[meshObj.vertexCount];
                    var deltaNormals = new Vector3[meshObj.vertexCount];
                    meshObj.GetBlendShapeFrameVertices(blendShapeIndex, frameIndex, deltaVertices, deltaNormals, null);

                    targets.Add(new Dictionary<string, AccessorId>
                        {
                            { SemanticProperties.POSITION, ExportAccessor(SchemaExtensions.ConvertVector3CoordinateSpaceAndCopy(deltaVertices, SchemaExtensions.CoordinateSpaceConversionScale)) },
                            { SemanticProperties.NORMAL, ExportAccessor(SchemaExtensions.ConvertVector3CoordinateSpaceAndCopy(deltaNormals,SchemaExtensions.CoordinateSpaceConversionScale))},
                         });

                    // We need to get the weight from the SkinnedMeshRenderer because this represents the currently
                    // defined weight by the user to apply to this blend shape.  If we instead got the value from
                    // the unityMesh, it would be a _per frame_ weight, and for a single-frame blend shape, that would
                    // always be 100.  A blend shape might have more than one frame if a user wanted to more tightly
                    // control how a blend shape will be animated during weight changes (e.g. maybe they want changes
                    // between 0-50% to be really minor, but between 50-100 to be extreme, hence they'd have two frames
                    // where the first frame would have a weight of 50 (meaning any weight between 0-50 should be relative
                    // to the values in this frame) and then any weight between 50-100 would be relevant to the weights in
                    // the second frame.  See Post 20 for more info:
                    // https://forum.unity3d.com/threads/is-there-some-method-to-add-blendshape-in-editor.298002/#post-2015679
                    weights.Add(smr.GetBlendShapeWeight(blendShapeIndex) / 100);
                }

                mesh.Weights = weights;
                primitive.Targets = targets;
                primitive.TargetNames = targetNames;
            }
        }

        private static bool ContainsValidRenderer(GameObject gameObject)
        {
            var mr = gameObject.GetComponent<MeshRenderer>();
            var smr = gameObject.GetComponent<SkinnedMeshRenderer>();
            if (smr != null) return smr.sharedMesh != null && smr.sharedMesh.vertexCount > 0;
            if (mr != null)
            {
                var meshFilter = gameObject.GetComponent<MeshFilter>();
                return meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.vertexCount > 0;
            }
            return false;
        }

        private static bool IsValidSkinnedMeshRenderer(SkinnedMeshRenderer smr)
        {
            return smr != null && smr.rootBone != null && smr.bones.Length > 0;
        }

        /// <summary>
        ///  add primitive if the root object also has a mesh,but for node, it will not add incase stackoverflow
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="primitives"></param>
        /// <param name="nonPrimitives"></param>
        private void FilterPrimitives(Transform transform, out GameObject[] primitives, out GameObject[] nonPrimitives)
        {
            var childCount = transform.childCount;
            var prims = new List<GameObject>(childCount);
            var nonPrims = new List<GameObject>(childCount);

            // add another primitive if the root object also has a mesh
            //if (transform.gameObject.activeSelf)
            //{
            //    if (ContainsValidRenderer(transform.gameObject))
            //    {
            //        prims.Add(transform.gameObject);
            //    }
            //}
            for (var i = 0; i < childCount; i++)
            {
                var go = transform.GetChild(i).gameObject;
                if (IsPrimitive(go))
                    prims.Add(go);
                else
                    nonPrims.Add(go);
            }

            primitives = prims.ToArray();
            nonPrimitives = nonPrims.ToArray();
        }

        private AccessorId ExportAccessor(byte[] arr, bool isIndices = false)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.SCALAR;

            int min = arr[0];
            int max = arr[0];

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur < min)
                {
                    min = cur;
                }
                if (cur > max)
                {
                    max = cur;
                }
            }

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            if (max <= byte.MaxValue && min >= byte.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedByte;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((byte)v);
                }
            }
            else if (max <= sbyte.MaxValue && min >= sbyte.MinValue && !isIndices)
            {
                accessor.ComponentType = GLTFComponentType.Byte;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((sbyte)v);
                }
            }
            else if (max <= short.MaxValue && min >= short.MinValue && !isIndices)
            {
                accessor.ComponentType = GLTFComponentType.Short;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((short)v);
                }
            }
            else if (max <= ushort.MaxValue && min >= ushort.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedShort;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((ushort)v);
                }
            }
            else if (min >= uint.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedInt;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((uint)v);
                }
            }
            else
            {
                accessor.ComponentType = GLTFComponentType.Float;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((float)v);
                }
            }

            accessor.Min = new List<double> { min };
            accessor.Max = new List<double> { max };

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
#if UNITY_STANDALONE
        private AccessorId GetDracoAccessorID(int[] arr, bool isIndices = false)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }


            var accessor = new Accessor();
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.SCALAR;
            int min = arr[0];
            int max = arr[0];
            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur < min)
                {
                    min = cur;
                }
                if (cur > max)
                {
                    max = cur;
                }
            }

            if (max <= byte.MaxValue && min >= byte.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedByte;
            }
            else if (max <= sbyte.MaxValue && min >= sbyte.MinValue && !isIndices)
            {
                accessor.ComponentType = GLTFComponentType.Byte;
            }
            else if (max <= short.MaxValue && min >= short.MinValue && !isIndices)
            {
                accessor.ComponentType = GLTFComponentType.Short;
            }
            else if (max <= ushort.MaxValue && min >= ushort.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedShort;
            }
            else if (min >= uint.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedInt;
            }
            else
            {
                accessor.ComponentType = GLTFComponentType.Float;
            }

            accessor.Min = new List<double> { min };
            accessor.Max = new List<double> { max };

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
#endif
        private AccessorId ExportAccessor(int[] arr, bool isIndices = false)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.SCALAR;

            int min = arr[0];
            int max = arr[0];

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur < min)
                {
                    min = cur;
                }
                if (cur > max)
                {
                    max = cur;
                }
            }

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            if (max <= byte.MaxValue && min >= byte.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedByte;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((byte)v);
                }
            }
            else if (max <= sbyte.MaxValue && min >= sbyte.MinValue && !isIndices)
            {
                accessor.ComponentType = GLTFComponentType.Byte;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((sbyte)v);
                }
            }
            else if (max <= short.MaxValue && min >= short.MinValue && !isIndices)
            {
                accessor.ComponentType = GLTFComponentType.Short;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((short)v);
                }
            }
            else if (max <= ushort.MaxValue && min >= ushort.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedShort;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((ushort)v);
                }
            }
            else if (min >= uint.MinValue)
            {
                accessor.ComponentType = GLTFComponentType.UnsignedInt;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((uint)v);
                }
            }
            else
            {
                accessor.ComponentType = GLTFComponentType.Float;

                foreach (var v in arr)
                {
                    _bufferWriter.Write((float)v);
                }
            }

            accessor.Min = new List<double> { min };
            accessor.Max = new List<double> { max };

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
#if UNITY_STANDALONE
        private AccessorId GetDracoAccessorID(float[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }


            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.SCALAR;
            float min = arr[0];
            float max = arr[0];
            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur < min)
                {
                    min = cur;
                }
                if (cur > max)
                {
                    max = cur;
                }
            }

            accessor.Min = new List<double> { min };
            accessor.Max = new List<double> { max };

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId GetDracoAccessorID(Vector2[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }


            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC2;
            float minX = arr[0].x;
            float minY = arr[0].y;
            float maxX = arr[0].x;
            float maxY = arr[0].y;
            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.x < minX)
                {
                    minX = cur.x;
                }
                if (cur.y < minY)
                {
                    minY = cur.y;
                }
                if (cur.x > maxX)
                {
                    maxX = cur.x;
                }
                if (cur.y > maxY)
                {
                    maxY = cur.y;
                }
            }
            accessor.Min = new List<double> { minX, minY };
            accessor.Max = new List<double> { maxX, maxY };

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId GetDracoAccessorID(Vector3[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }


            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC3;

            float minX = arr[0].x;
            float minY = arr[0].y;
            float minZ = arr[0].z;
            float maxX = arr[0].x;
            float maxY = arr[0].y;
            float maxZ = arr[0].z;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.x < minX)
                {
                    minX = cur.x;
                }
                if (cur.y < minY)
                {
                    minY = cur.y;
                }
                if (cur.z < minZ)
                {
                    minZ = cur.z;
                }
                if (cur.x > maxX)
                {
                    maxX = cur.x;
                }
                if (cur.y > maxY)
                {
                    maxY = cur.y;
                }
                if (cur.z > maxZ)
                {
                    maxZ = cur.z;
                }
            }

            accessor.Min = new List<double> { minX, minY, minZ };
            accessor.Max = new List<double> { maxX, maxY, maxZ };

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId GetDracoAccessorID(Vector4[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }


            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC4;

            float minX = arr[0].x;
            float minY = arr[0].y;
            float minZ = arr[0].z;
            float minW = arr[0].w;
            float maxX = arr[0].x;
            float maxY = arr[0].y;
            float maxZ = arr[0].z;
            float maxW = arr[0].w;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.x < minX)
                {
                    minX = cur.x;
                }
                if (cur.y < minY)
                {
                    minY = cur.y;
                }
                if (cur.z < minZ)
                {
                    minZ = cur.z;
                }
                if (cur.w < minW)
                {
                    minW = cur.w;
                }
                if (cur.x > maxX)
                {
                    maxX = cur.x;
                }
                if (cur.y > maxY)
                {
                    maxY = cur.y;
                }
                if (cur.z > maxZ)
                {
                    maxZ = cur.z;
                }
                if (cur.w > maxW)
                {
                    maxW = cur.w;
                }
            }

            accessor.Min = new List<double> { minX, minY, minZ, minW };
            accessor.Max = new List<double> { maxX, maxY, maxZ, maxW };

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId GetDracoAccessorID(Vector4Int[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }


            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.UnsignedShort;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC4;

            int minX = arr[0].x;
            int minY = arr[0].y;
            int minZ = arr[0].z;
            int minW = arr[0].w;
            int maxX = arr[0].x;
            int maxY = arr[0].y;
            int maxZ = arr[0].z;
            int maxW = arr[0].w;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.x < minX)
                {
                    minX = cur.x;
                }
                if (cur.y < minY)
                {
                    minY = cur.y;
                }
                if (cur.z < minZ)
                {
                    minZ = cur.z;
                }
                if (cur.w < minW)
                {
                    minW = cur.w;
                }
                if (cur.x > maxX)
                {
                    maxX = cur.x;
                }
                if (cur.y > maxY)
                {
                    maxY = cur.y;
                }
                if (cur.z > maxZ)
                {
                    maxZ = cur.z;
                }
                if (cur.w > maxW)
                {
                    maxW = cur.w;
                }
            }

            accessor.Min = new List<double> { minX, minY, minZ, minW };
            accessor.Max = new List<double> { maxX, maxY, maxZ, maxW };

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }

        private AccessorId GetDracoAccessorID(Color[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }


            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC4;

            float minR = arr[0].r;
            float minG = arr[0].g;
            float minB = arr[0].b;
            float minA = arr[0].a;
            float maxR = arr[0].r;
            float maxG = arr[0].g;
            float maxB = arr[0].b;
            float maxA = arr[0].a;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.r < minR)
                {
                    minR = cur.r;
                }
                if (cur.g < minG)
                {
                    minG = cur.g;
                }
                if (cur.b < minB)
                {
                    minB = cur.b;
                }
                if (cur.a < minA)
                {
                    minA = cur.a;
                }
                if (cur.r > maxR)
                {
                    maxR = cur.r;
                }
                if (cur.g > maxG)
                {
                    maxG = cur.g;
                }
                if (cur.b > maxB)
                {
                    maxB = cur.b;
                }
                if (cur.a > maxA)
                {
                    maxA = cur.a;
                }
            }

            accessor.Min = new List<double> { minR, minG, minB, minA };
            accessor.Max = new List<double> { maxR, maxG, maxB, maxA };


            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
#endif
        private AccessorId ExportAccessor(float[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.SCALAR;

            float min = arr[0];
            float max = arr[0];

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur < min)
                {
                    min = cur;
                }
                if (cur > max)
                {
                    max = cur;
                }
            }

            accessor.Min = new List<double> { min };
            accessor.Max = new List<double> { max };

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            foreach (var vec in arr)
            {
                _bufferWriter.Write(vec);
            }

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId ExportAccessor(Vector2[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC2;

            float minX = arr[0].x;
            float minY = arr[0].y;
            float maxX = arr[0].x;
            float maxY = arr[0].y;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.x < minX)
                {
                    minX = cur.x;
                }
                if (cur.y < minY)
                {
                    minY = cur.y;
                }
                if (cur.x > maxX)
                {
                    maxX = cur.x;
                }
                if (cur.y > maxY)
                {
                    maxY = cur.y;
                }
            }

            accessor.Min = new List<double> { minX, minY };
            accessor.Max = new List<double> { maxX, maxY };

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            foreach (var vec in arr)
            {
                _bufferWriter.Write(vec.x);
                _bufferWriter.Write(vec.y);
            }

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId ExportAccessor(Vector3[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC3;

            float minX = arr[0].x;
            float minY = arr[0].y;
            float minZ = arr[0].z;
            float maxX = arr[0].x;
            float maxY = arr[0].y;
            float maxZ = arr[0].z;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.x < minX)
                {
                    minX = cur.x;
                }
                if (cur.y < minY)
                {
                    minY = cur.y;
                }
                if (cur.z < minZ)
                {
                    minZ = cur.z;
                }
                if (cur.x > maxX)
                {
                    maxX = cur.x;
                }
                if (cur.y > maxY)
                {
                    maxY = cur.y;
                }
                if (cur.z > maxZ)
                {
                    maxZ = cur.z;
                }
            }

            accessor.Min = new List<double> { minX, minY, minZ };
            accessor.Max = new List<double> { maxX, maxY, maxZ };

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            foreach (var vec in arr)
            {
                _bufferWriter.Write(vec.x);
                _bufferWriter.Write(vec.y);
                _bufferWriter.Write(vec.z);
            }

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId ExportAccessor(Vector4[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC4;

            float minX = arr[0].x;
            float minY = arr[0].y;
            float minZ = arr[0].z;
            float minW = arr[0].w;
            float maxX = arr[0].x;
            float maxY = arr[0].y;
            float maxZ = arr[0].z;
            float maxW = arr[0].w;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.x < minX)
                {
                    minX = cur.x;
                }
                if (cur.y < minY)
                {
                    minY = cur.y;
                }
                if (cur.z < minZ)
                {
                    minZ = cur.z;
                }
                if (cur.w < minW)
                {
                    minW = cur.w;
                }
                if (cur.x > maxX)
                {
                    maxX = cur.x;
                }
                if (cur.y > maxY)
                {
                    maxY = cur.y;
                }
                if (cur.z > maxZ)
                {
                    maxZ = cur.z;
                }
                if (cur.w > maxW)
                {
                    maxW = cur.w;
                }
            }

            accessor.Min = new List<double> { minX, minY, minZ, minW };
            accessor.Max = new List<double> { maxX, maxY, maxZ, maxW };

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            foreach (var vec in arr)
            {
                _bufferWriter.Write(vec.x);
                _bufferWriter.Write(vec.y);
                _bufferWriter.Write(vec.z);
                _bufferWriter.Write(vec.w);
            }

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        /// <summary>
        /// export unsigned short joints data
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// 
        private AccessorId ExportAccessor(Vector4Int[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.UnsignedShort;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC4;

            int minX = arr[0].x;
            int minY = arr[0].y;
            int minZ = arr[0].z;
            int minW = arr[0].w;
            int maxX = arr[0].x;
            int maxY = arr[0].y;
            int maxZ = arr[0].z;
            int maxW = arr[0].w;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.x < minX)
                {
                    minX = cur.x;
                }
                if (cur.y < minY)
                {
                    minY = cur.y;
                }
                if (cur.z < minZ)
                {
                    minZ = cur.z;
                }
                if (cur.w < minW)
                {
                    minW = cur.w;
                }
                if (cur.x > maxX)
                {
                    maxX = cur.x;
                }
                if (cur.y > maxY)
                {
                    maxY = cur.y;
                }
                if (cur.z > maxZ)
                {
                    maxZ = cur.z;
                }
                if (cur.w > maxW)
                {
                    maxW = cur.w;
                }
            }

            accessor.Min = new List<double> { minX, minY, minZ, minW };
            accessor.Max = new List<double> { maxX, maxY, maxZ, maxW };

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            foreach (var vec in arr)
            {
                _bufferWriter.Write((ushort)vec.x);
                _bufferWriter.Write((ushort)vec.y);
                _bufferWriter.Write((ushort)vec.z);
                _bufferWriter.Write((ushort)vec.w);
            }

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId ExportAccessor(Matrix4x4[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.MAT4;

            float min00 = arr[0].m00;
            float min10 = arr[0].m10;
            float min20 = arr[0].m20;
            float min30 = arr[0].m30;
            float min01 = arr[0].m01;
            float min11 = arr[0].m11;
            float min21 = arr[0].m21;
            float min31 = arr[0].m31;
            float min02 = arr[0].m02;
            float min12 = arr[0].m12;
            float min22 = arr[0].m22;
            float min32 = arr[0].m32;
            float min03 = arr[0].m03;
            float min13 = arr[0].m13;
            float min23 = arr[0].m23;
            float min33 = arr[0].m33;

            float max00 = arr[0].m00;
            float max10 = arr[0].m10;
            float max20 = arr[0].m20;
            float max30 = arr[0].m30;
            float max01 = arr[0].m01;
            float max11 = arr[0].m11;
            float max21 = arr[0].m21;
            float max31 = arr[0].m31;
            float max02 = arr[0].m02;
            float max12 = arr[0].m12;
            float max22 = arr[0].m22;
            float max32 = arr[0].m32;
            float max03 = arr[0].m03;
            float max13 = arr[0].m13;
            float max23 = arr[0].m23;
            float max33 = arr[0].m33;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.m00 < min00)
                    min00 = cur.m00;
                if (cur.m10 < min10)
                    min10 = cur.m10;
                if (cur.m20 < min20)
                    min20 = cur.m20;
                if (cur.m30 < min30)
                    min30 = cur.m30;

                if (cur.m01 < min01)
                    min01 = cur.m01;
                if (cur.m11 < min11)
                    min11 = cur.m11;
                if (cur.m21 < min21)
                    min21 = cur.m21;
                if (cur.m31 < min31)
                    min31 = cur.m31;

                if (cur.m02 < min02)
                    min02 = cur.m02;
                if (cur.m12 < min12)
                    min12 = cur.m12;
                if (cur.m22 < min22)
                    min22 = cur.m22;
                if (cur.m32 < min32)
                    min32 = cur.m32;

                if (cur.m03 < min03)
                    min03 = cur.m03;
                if (cur.m13 < min13)
                    min13 = cur.m13;
                if (cur.m23 < min23)
                    min23 = cur.m23;
                if (cur.m33 < min33)
                    min33 = cur.m33;


                if (cur.m00 > max00)
                    max00 = cur.m00;
                if (cur.m10 > max10)
                    max10 = cur.m10;
                if (cur.m20 > max20)
                    max20 = cur.m20;
                if (cur.m30 > max30)
                    max30 = cur.m30;

                if (cur.m01 > max01)
                    max01 = cur.m01;
                if (cur.m11 > max11)
                    max11 = cur.m11;
                if (cur.m21 > max21)
                    max21 = cur.m21;
                if (cur.m31 > max31)
                    max31 = cur.m31;

                if (cur.m02 > max02)
                    max02 = cur.m02;
                if (cur.m12 > max12)
                    max12 = cur.m12;
                if (cur.m22 > max22)
                    max22 = cur.m22;
                if (cur.m32 > max32)
                    max32 = cur.m32;

                if (cur.m03 > max03)
                    max03 = cur.m03;
                if (cur.m13 > max13)
                    max13 = cur.m13;
                if (cur.m23 > max23)
                    max23 = cur.m23;
                if (cur.m33 > max33)
                    max33 = cur.m33;
            }

            accessor.Min = new List<double> { min00, min10, min20, min30, min01, min11, min21, min31, min02, min12, min22, min32, min03, min13, min23, min33 };
            accessor.Max = new List<double> { max00, max10, max20, max30, max01, max11, max21, max31, max02, max12, max22, max32, max03, max13, max23, max33 };

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            foreach (var vec in arr)
            {
                _bufferWriter.Write(vec.m00);
                _bufferWriter.Write(vec.m10);
                _bufferWriter.Write(vec.m20);
                _bufferWriter.Write(vec.m30);
                _bufferWriter.Write(vec.m01);
                _bufferWriter.Write(vec.m11);
                _bufferWriter.Write(vec.m21);
                _bufferWriter.Write(vec.m31);
                _bufferWriter.Write(vec.m02);
                _bufferWriter.Write(vec.m12);
                _bufferWriter.Write(vec.m22);
                _bufferWriter.Write(vec.m32);
                _bufferWriter.Write(vec.m03);
                _bufferWriter.Write(vec.m13);
                _bufferWriter.Write(vec.m23);
                _bufferWriter.Write(vec.m33);
            }

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private AccessorId ExportAccessor(Color[] arr)
        {
            uint count = (uint)arr.Length;

            if (count == 0)
            {
                throw new Exception("Accessors can not have a count of 0.");
            }

            var accessor = new Accessor();
            accessor.ComponentType = GLTFComponentType.Float;
            accessor.Count = count;
            accessor.Type = GLTFAccessorAttributeType.VEC4;

            float minR = arr[0].r;
            float minG = arr[0].g;
            float minB = arr[0].b;
            float minA = arr[0].a;
            float maxR = arr[0].r;
            float maxG = arr[0].g;
            float maxB = arr[0].b;
            float maxA = arr[0].a;

            for (var i = 1; i < count; i++)
            {
                var cur = arr[i];

                if (cur.r < minR)
                {
                    minR = cur.r;
                }
                if (cur.g < minG)
                {
                    minG = cur.g;
                }
                if (cur.b < minB)
                {
                    minB = cur.b;
                }
                if (cur.a < minA)
                {
                    minA = cur.a;
                }
                if (cur.r > maxR)
                {
                    maxR = cur.r;
                }
                if (cur.g > maxG)
                {
                    maxG = cur.g;
                }
                if (cur.b > maxB)
                {
                    maxB = cur.b;
                }
                if (cur.a > maxA)
                {
                    maxA = cur.a;
                }
            }

            accessor.Min = new List<double> { minR, minG, minB, minA };
            accessor.Max = new List<double> { maxR, maxG, maxB, maxA };

            AlignToBoundary(_bufferWriter.BaseStream, 0x00);
            uint byteOffset = CalculateAlignment((uint)_bufferWriter.BaseStream.Position, 4);

            foreach (var color in arr)
            {
                _bufferWriter.Write(color.r);
                _bufferWriter.Write(color.g);
                _bufferWriter.Write(color.b);
                _bufferWriter.Write(color.a);
            }

            uint byteLength = CalculateAlignment((uint)_bufferWriter.BaseStream.Position - byteOffset, 4);

            accessor.BufferView = ExportBufferView(byteOffset, byteLength);

            var id = new AccessorId
            {
                Id = _root.Accessors.Count,
                Root = _root
            };
            _root.Accessors.Add(accessor);

            return id;
        }
        private BufferViewId ExportBufferView(uint byteOffset, uint byteLength)
        {
            var bufferView = new BufferView
            {
                Buffer = _bufferId,
                ByteOffset = byteOffset,
                ByteLength = byteLength
            };

            var id = new BufferViewId
            {
                Id = _root.BufferViews.Count,
                Root = _root
            };

            _root.BufferViews.Add(bufferView);

            return id;
        }
        protected static DrawMode GetDrawMode(MeshTopology topology)
        {
            switch (topology)
            {
                case MeshTopology.Points: return DrawMode.Points;
                case MeshTopology.Lines: return DrawMode.Lines;
                case MeshTopology.LineStrip: return DrawMode.LineStrip;
                case MeshTopology.Triangles: return DrawMode.Triangles;
                case MeshTopology.Quads:
                    break;
            }

            throw new Exception("glTF does not support Unity mesh topology: " + topology);
        }
        private static bool IsPrimitive(GameObject gameObject)
        {
            var isPrimitive = gameObject.transform.childCount == 0 /*
                && gameObject.transform.localPosition == Vector3.zero
                && gameObject.transform.localRotation == Quaternion.identity
                && gameObject.transform.localScale == Vector3.one*/
                && ContainsValidRenderer(gameObject);
            return isPrimitive;
        }
        private static Mesh ConvertLeftHandToRightHand(Mesh leftHandMesh)
        {
            VertexAttributeDescriptor[] Attributes = leftHandMesh.GetVertexAttributes();
            int subMeshCount = leftHandMesh.subMeshCount;
            Mesh rightHandMesh = new Mesh();
            int visitIndex = 0;

            if (Attributes.Length > visitIndex && Attributes[visitIndex].attribute == VertexAttribute.Position)
            {
                var vertices = SchemaExtensions.ConvertVector3CoordinateSpaceAndCopy(leftHandMesh.vertices, SchemaExtensions.CoordinateSpaceConversionScale);
                rightHandMesh.vertices = vertices;
                visitIndex++;
            }

            if (Attributes.Length > visitIndex && Attributes[visitIndex].attribute == VertexAttribute.Normal)
            {
                var normals = SchemaExtensions.ConvertVector3CoordinateSpaceAndCopy(leftHandMesh.normals, SchemaExtensions.CoordinateSpaceConversionScale);
                rightHandMesh.normals = normals;
                visitIndex++;
            }


            if (Attributes.Length > visitIndex && Attributes[visitIndex].attribute == VertexAttribute.Tangent)
            {
                var tangents = SchemaExtensions.ConvertVector4CoordinateSpaceAndCopy(leftHandMesh.tangents, SchemaExtensions.TangentSpaceConversionScale);
                rightHandMesh.tangents = tangents;
                visitIndex++;
            }

            if (Attributes.Length > visitIndex && Attributes[visitIndex].attribute == VertexAttribute.Color)
            {
                var colors = leftHandMesh.colors;
                rightHandMesh.colors = colors;
                visitIndex++;
            }

            if (Attributes.Length > visitIndex && Attributes[visitIndex].attribute == VertexAttribute.TexCoord0)
            {
                var texcoord0 = SchemaExtensions.FlipTexCoordArrayVAndCopy(leftHandMesh.uv);
                rightHandMesh.uv = texcoord0;
                visitIndex++;
            }

            if (Attributes.Length > visitIndex && Attributes[visitIndex].attribute == VertexAttribute.TexCoord1)
            {
                var texcord1 = SchemaExtensions.FlipTexCoordArrayVAndCopy(leftHandMesh.uv2);
                rightHandMesh.uv2 = texcord1;
                visitIndex++;
            }

            // export JOINTS,WEIGHTS attribute ,for skin
            if (Attributes.Length > visitIndex && Attributes[visitIndex].attribute == VertexAttribute.BlendIndices)
            {
                rightHandMesh.boneWeights = leftHandMesh.boneWeights;
            }
            rightHandMesh.subMeshCount = subMeshCount;
            for (var submesh = 0; submesh < subMeshCount; submesh++)
            {
                var topology = leftHandMesh.GetTopology(submesh);
                var indices = leftHandMesh.GetIndices(submesh);
                if (topology == MeshTopology.Triangles) SchemaExtensions.FlipTriangleFaces(indices);

                rightHandMesh.SetIndices(indices, topology, submesh);
            }
            rightHandMesh.UploadMeshData(true);
            return rightHandMesh;
        }
    }


}