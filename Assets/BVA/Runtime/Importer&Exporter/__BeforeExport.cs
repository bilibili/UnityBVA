using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BVA
{
    public partial class GLTFSceneExporter
    {
        #region MMD Export PreProcess
        private enum BlendShapeLogic
        {
            WithBlendShape,
            WithoutBlendShape,
        }
        /// <summary>
        /// MMD mesh combined all meshes in one mesh, this will cause too large file sizes when exporting
        /// </summary>
        /// <param name="go">Avatar root GameObject, usually has an Animator on it</param>
        public static void SeparateMMDMeshProcessing(GameObject avatar)
        {
            var skinnedMeshRenderers = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderers.Length > 1) return;
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                if (skinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
                {
                    SeparatePolyWithBlendShape(skinnedMeshRenderer);
                }
            }
        }

        /// <summary>
        /// Extract blendshape vertex part
        /// </summary>
        /// <param name="skinnedMeshRendererInput"></param>
        private static void SeparatePolyWithBlendShape(SkinnedMeshRenderer skinnedMeshRendererInput)
        {
            var indicesUsedByBlendShape = new HashSet<int>();
            var mesh = skinnedMeshRendererInput.sharedMesh;

            // retrieve the original BlendShape data
            for (int i = 0; i < mesh.blendShapeCount; ++i)
            {
                var deltaVertices = new Vector3[mesh.vertexCount];
                var deltaNormals = new Vector3[mesh.vertexCount];
                var deltaTangents = new Vector3[mesh.vertexCount];
                mesh.GetBlendShapeFrameVertices(i, 0, deltaVertices, deltaNormals, deltaTangents);

                for (int j = 0; j < deltaVertices.Length; j++)
                {
                    if (!deltaVertices[j].Equals(Vector3.zero))
                    {
                        if (!indicesUsedByBlendShape.Contains(j))
                        {
                            indicesUsedByBlendShape.Add(j);
                        }
                    }
                }
            }

            var subMeshCount = mesh.subMeshCount;
            var submeshesWithBlendShape = new Dictionary<int, int[]>();
            var submeshesWithoutBlendShape = new Dictionary<int, int[]>();
            var vertexIndexWithBlendShape = new Dictionary<int, int>();
            var vertexCounterWithBlendShape = 0;
            var vertexIndexWithoutBlendShape = new Dictionary<int, int>();
            var vertexCounterWithoutBlendShape = 0;

            // check blendshape's vertex index from submesh
            for (int i = 0; i < subMeshCount; i++)
            {
                var triangle = mesh.GetTriangles(i);
                var submeshWithBlendShape = new List<int>();
                var submeshWithoutBlendShape = new List<int>();

                for (int j = 0; j < triangle.Length; j += 3)
                {
                    if (indicesUsedByBlendShape.Contains(triangle[j]) ||
                        indicesUsedByBlendShape.Contains(triangle[j + 1]) ||
                        indicesUsedByBlendShape.Contains(triangle[j + 2]))
                    {
                        BuildNewTriangleList(vertexIndexWithBlendShape, triangle, j, submeshWithBlendShape, ref vertexCounterWithBlendShape);
                    }
                    else
                    {
                        BuildNewTriangleList(vertexIndexWithoutBlendShape, triangle, j, submeshWithoutBlendShape, ref vertexCounterWithoutBlendShape);
                    }
                }
                if (submeshWithBlendShape.Count > 0)
                    submeshesWithBlendShape.Add(i, submeshWithBlendShape.ToArray());
                if (submeshWithoutBlendShape.Count > 0)
                    submeshesWithoutBlendShape.Add(i, submeshWithoutBlendShape.ToArray()); ;
            }

            // check if any BlendShape exists
            if (submeshesWithoutBlendShape.Count > 0)
            {
                // put the mesh without BlendShape in a new SkinnedMeshRenderer
                var srcGameObject = skinnedMeshRendererInput.gameObject;
                var srcTransform = skinnedMeshRendererInput.transform.parent;
                var targetObjectForMeshWithoutBS = GameObject.Instantiate(srcGameObject);

                targetObjectForMeshWithoutBS.name = srcGameObject.name + "_WithoutBlendShape";
                targetObjectForMeshWithoutBS.transform.SetParent(srcTransform);
                var skinnedMeshRendererWithoutBS = targetObjectForMeshWithoutBS.GetComponent<SkinnedMeshRenderer>();
                skinnedMeshRendererWithoutBS.bones = skinnedMeshRendererInput.bones;
                skinnedMeshRendererWithoutBS.rootBone = skinnedMeshRendererInput.rootBone;

                // build meshes with/without BlendShape
                BuildNewMesh(skinnedMeshRendererInput, vertexIndexWithBlendShape, submeshesWithBlendShape, BlendShapeLogic.WithBlendShape);
                BuildNewMesh(skinnedMeshRendererWithoutBS, vertexIndexWithoutBlendShape, submeshesWithoutBlendShape, BlendShapeLogic.WithoutBlendShape);

                var comps = skinnedMeshRendererWithoutBS.GetComponents<UnityEngine.Component>();
                foreach (var comp in comps)
                {
                    if (comp is Transform || comp is SkinnedMeshRenderer)
                        continue;
                    GameObject.DestroyImmediate(comp);
                }
                while (skinnedMeshRendererWithoutBS.transform.childCount > 0)
                {
                    GameObject.DestroyImmediate(skinnedMeshRendererWithoutBS.transform.GetChild(0).gameObject);
                }
            }
        }

        private static void BuildNewTriangleList(Dictionary<int, int> newVerticesListLookUp, int[] triangleList, int index,
                                                 List<int> newTriangleList, ref int vertexCounter)
        {
            // build new vertex list and triangle list
            // vertex 1
            if (!newVerticesListLookUp.Keys.Contains(triangleList[index]))
            {
                newVerticesListLookUp.Add(triangleList[index], vertexCounter);
                newTriangleList.Add(vertexCounter);
                vertexCounter++;
            }
            else
            {
                var newVertexIndex = newVerticesListLookUp[triangleList[index]];
                newTriangleList.Add(newVertexIndex);
            }
            // vertex 2
            if (!newVerticesListLookUp.Keys.Contains(triangleList[index + 1]))
            {
                newVerticesListLookUp.Add(triangleList[index + 1], vertexCounter);
                newTriangleList.Add(vertexCounter);
                vertexCounter++;
            }
            else
            {
                var newVertexIndex = newVerticesListLookUp[triangleList[index + 1]];
                newTriangleList.Add(newVertexIndex);
            }
            // vertex 3
            if (!newVerticesListLookUp.Keys.Contains(triangleList[index + 2]))
            {
                newVerticesListLookUp.Add(triangleList[index + 2], vertexCounter);
                newTriangleList.Add(vertexCounter);
                vertexCounter++;
            }
            else
            {
                var newVertexIndex = newVerticesListLookUp[triangleList[index + 2]];
                newTriangleList.Add(newVertexIndex);
            }
        }

        private static void BuildNewMesh(SkinnedMeshRenderer skinnedMeshRenderer, Dictionary<int, int> newIndexLookUpDict, Dictionary<int, int[]> subMeshes, BlendShapeLogic blendShapeLabel)
        {
            // get original mesh data
            var materialList = new List<Material>();
            skinnedMeshRenderer.GetSharedMaterials(materialList);
            var mesh = skinnedMeshRenderer.sharedMesh;
            var meshVertices = mesh.vertices;
            var meshNormals = mesh.normals;
            var meshTangents = mesh.tangents;
            var meshColors = mesh.colors;
            var meshBoneWeights = mesh.boneWeights;
            var meshUVs = mesh.uv;

            // build new mesh
            var materialListNew = new List<Material>();
            var newMesh = new Mesh();

            if (mesh.vertexCount > ushort.MaxValue)
            {
                newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }

            var newDataLength = newIndexLookUpDict.Count;
            var newIndexLookUp = newIndexLookUpDict.Keys.ToArray();

            newMesh.vertices = newIndexLookUp.Select(x => meshVertices[x]).ToArray();
            if (meshNormals.Length > 0) newMesh.normals = newIndexLookUp.Select(x => meshNormals[x]).ToArray();
            if (meshTangents.Length > 0) newMesh.tangents = newIndexLookUp.Select(x => meshTangents[x]).ToArray();
            if (meshColors.Length > 0) newMesh.colors = newIndexLookUp.Select(x => meshColors[x]).ToArray();
            if (meshBoneWeights.Length > 0) newMesh.boneWeights = newIndexLookUp.Select(x => meshBoneWeights[x]).ToArray();
            if (meshUVs.Length > 0) newMesh.uv = newIndexLookUp.Select(x => meshUVs[x]).ToArray();
            newMesh.bindposes = mesh.bindposes;

            // add BlendShape data
            if (blendShapeLabel == BlendShapeLogic.WithBlendShape)
            {
                for (int i = 0; i < mesh.blendShapeCount; i++)
                {
                    // get original BlendShape data
                    var srcVertices = new Vector3[mesh.vertexCount];
                    var srcNormals = new Vector3[mesh.vertexCount];
                    var srcTangents = new Vector3[mesh.vertexCount];
                    mesh.GetBlendShapeFrameVertices(i, 0, srcVertices, srcNormals, srcTangents);

                    var dstVertices = new Vector3[newDataLength];
                    var dstNormals = new Vector3[newDataLength];
                    var dstTangents = new Vector3[newDataLength];

                    dstVertices = newIndexLookUp.Select(x => srcVertices[x]).ToArray();
                    dstNormals = newIndexLookUp.Select(x => srcNormals[x]).ToArray();
                    dstTangents = newIndexLookUp.Select(x => srcTangents[x]).ToArray();
                    newMesh.AddBlendShapeFrame(mesh.GetBlendShapeName(i), mesh.GetBlendShapeFrameWeight(i, 0), dstVertices, dstNormals, dstTangents);
                }
            }

            newMesh.subMeshCount = subMeshes.Count;
            var cosMaterialIndex = subMeshes.Keys.ToArray();

            // build material list
            for (int i = 0; i < subMeshes.Count; i++)
            {
                newMesh.SetTriangles(subMeshes[cosMaterialIndex[i]], i);
                materialListNew.Add(materialList[cosMaterialIndex[i]]);
            }
            skinnedMeshRenderer.sharedMaterials = materialListNew.ToArray();
            skinnedMeshRenderer.sharedMesh = newMesh;
        }
        #endregion
    }
}