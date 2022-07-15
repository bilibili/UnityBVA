using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Collections;
using Unity.Jobs;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using System.Linq;

public class NormalAverageTool
{

    public const string CopyAssetLabel = "Average Mesh Normal";

    [MenuItem("BVA/Developer Tools/Average Mesh Normal(write in uv8)")]
    public static void WirteAverageNormalsUV8()
    {
        if (Selection.activeGameObject == null)
            return;
        MeshFilter[] meshFilters = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            Debug.Log(mesh.name);
            mesh = CheckCopyMesh(mesh);
            //return;
            Vector3[] backedNormals = DoAverageNormal(mesh);
            mesh.SetUVs(7, backedNormals);
            /*
            Vector4[] ts = new Vector4[backedNormals.Length];
            for(int i=0; i<ts.Length; i++)
            {
                ts[i] = backedNormals[i];
            }
            mesh.tangents = ts; */
            //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(mesh));
            AssetDatabase.Refresh();
            meshFilter.sharedMesh = mesh;
        }

        SkinnedMeshRenderer[] skinMeshRenders = Selection.activeGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var skinMeshRender in skinMeshRenders)
        {
            Mesh mesh = skinMeshRender.sharedMesh;
            Debug.Log(mesh.name);
            mesh = CheckCopyMesh(mesh);
            //return;
            Vector3[] backedNormals = DoAverageNormal(mesh);
            mesh.SetUVs(7, backedNormals);
            /*
            Vector4[] ts = new Vector4[backedNormals.Length];
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i] = backedNormals[i];
            }
            mesh.tangents = ts;*/
            //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(mesh));
            AssetDatabase.Refresh();
            skinMeshRender.sharedMesh = mesh;
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("Write average normal success！");
    }


    public static Mesh CheckCopyMesh(Mesh _mesh)
    { 
        string path1 = AssetDatabase.GetAssetPath(_mesh.GetInstanceID()); 
        bool needCopy = false;
        if(path1.Contains("Library/unity")) 
        {
            needCopy = false;
        }
        else
        {
            if (AssetDatabase.GetLabels(_mesh).Contains(CopyAssetLabel) == false)//如果不是由这个工具复制出来的
            {
                needCopy = true;
            }
            else
            {
                Debug.Log(_mesh.name + " is already a duplication");
            }
        }
       
        if(needCopy)
        {
            string folderPath = Path.Combine(Path.GetDirectoryName(path1), "Copyed_" + Path.GetFileNameWithoutExtension(path1));
            Debug.Log("" + folderPath);
            CreateFolder(folderPath);
            Mesh mesh = GameObject.Instantiate(_mesh);
            mesh.name = _mesh.name;
            string meshAssetPath = Path.Combine(folderPath, mesh.name + ".asset");
            CreateCopyMeshOrUpdate(mesh, meshAssetPath);
            Mesh newMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshAssetPath);
            AssetDatabase.SetLabels(newMesh, new string[] { CopyAssetLabel });
            return newMesh;
        }
        else
        {
            return _mesh;
        } 
    }

     
     
    private static void CreateFolder(string path)
    {
        var target = "";
        var splitChars = new char[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar };
        foreach (var dir in path.Split(splitChars))
        {
            var parent = target;
            target = System.IO.Path.Combine(target, dir);
            if (!AssetDatabase.IsValidFolder(target))
            {
                AssetDatabase.CreateFolder(parent, dir);
            }
        }
    }
     
    private static void CreateCopyMeshOrUpdate(Object newAsset, string assetPath)
    {
        var oldAsset = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
        if (oldAsset == null)
        {
            AssetDatabase.CreateAsset(newAsset, assetPath);
        }
        else
        {
            EditorUtility.CopySerializedIfDifferent(newAsset, oldAsset);
            AssetDatabase.SaveAssets();
        }
    }

    public struct CollectNormalJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> normals, vertrx;
        [NativeDisableContainerSafetyRestriction]
        public NativeArray<UnsafeParallelHashMap<Vector3, Vector3>.ParallelWriter> result;

        public CollectNormalJob(NativeArray<Vector3> normals, NativeArray<Vector3> vertrx, NativeArray<UnsafeParallelHashMap<Vector3, Vector3>.ParallelWriter> result)
        {
            this.normals = normals;
            this.vertrx = vertrx;
            this.result = result;
        }

        void IJobParallelFor.Execute(int index)
        {
            for (int i = 0; i < result.Length + 1; i++)
            {
                if (i == result.Length)
                {
                    Debug.LogWarning($"merge vertex count（{i}）overflow");
                    break;
                } 
                if (result[i].TryAdd(vertrx[index], normals[index]))
                {
                    break;
                }
            }
        }
    }

    public struct BakeNormalJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> vertrx, normals;
        [ReadOnly] public NativeArray<Vector4> tangents;
        [NativeDisableContainerSafetyRestriction]
        [ReadOnly] public NativeArray<UnsafeParallelHashMap<Vector3, Vector3>> result; 
        public NativeArray<Vector3> Vector3s;

        public BakeNormalJob(NativeArray<Vector3> vertrx, NativeArray<Vector3> normals, NativeArray<Vector4> tangents, NativeArray<UnsafeParallelHashMap<Vector3, Vector3>> result, NativeArray<Vector3> Vector3s)
        {
            this.vertrx = vertrx;
            this.normals = normals;
            this.tangents = tangents;
            this.result = result; 
            this.Vector3s = Vector3s;
        }

        void IJobParallelFor.Execute(int index)
        {
            Vector3 smoothedNormals = Vector3.zero;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i][vertrx[index]] != Vector3.zero)
                    smoothedNormals += result[i][vertrx[index]];
                else
                    break;
            }
            smoothedNormals = smoothedNormals.normalized;
            
            Vector3 binormal = (Vector3.Cross(normals[index], tangents[index]) * tangents[index].w).normalized;
            Matrix4x4 tbn = new Matrix4x4(tangents[index], binormal, normals[index],Vector3.zero);
            tbn = tbn.transpose;
            // Object Space to Tangent Space
            Vector3 bakedNormal = tbn.MultiplyVector(smoothedNormals).normalized;

            //bakedNormal = smoothedNormals;

            Vector3 newVec3 = new Vector3();
            newVec3.x = bakedNormal.x;
            newVec3.y = bakedNormal.y;
            newVec3.z = bakedNormal.z;

            Vector3s[index] = newVec3;
        }
    }


    private static Vector3[] DoAverageNormal(Mesh mesh, int maxOverlapvertices = 10)
    {
        int vertexCount = mesh.vertexCount;

        NativeArray<Vector3> normals = new NativeArray<Vector3>(mesh.normals, Allocator.Persistent);
        NativeArray<Vector3> vertrx = new NativeArray<Vector3>(mesh.vertices, Allocator.Persistent);
        var tangents = new NativeArray<Vector4>(mesh.tangents, Allocator.Persistent);
        var result = new NativeArray<UnsafeParallelHashMap<Vector3, Vector3>>(maxOverlapvertices, Allocator.Persistent);
        var result_writer = new NativeArray<UnsafeParallelHashMap<Vector3, Vector3>.ParallelWriter>(result.Length, Allocator.Persistent);
        var Vector3s = new NativeArray<Vector3>(vertexCount, Allocator.Persistent);

        void DisposeAll()
        {
            // remove this forloop will cause Persistent memory leak, leaked memory can only be released when Unity shut down
            for (int i = 0; i < result.Length; i++)
            {
                result[i].Dispose();
            } 
            normals.Dispose();
            vertrx.Dispose();
            tangents.Dispose();
            result.Dispose();
            result_writer.Dispose();
            Vector3s.Dispose();
        }

        // safe check before baking
        if (mesh.normals.Length != 0 && mesh.tangents.Length == 0)
        {
            Debug.LogError("Please refer to Model Import Inspector[<color=blue>" + mesh.name + "</color>] and calculate the tangent.(Model Import Page——Tangents——Calculate)"); 
            DisposeAll();
            return null; 
        }

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new UnsafeParallelHashMap<Vector3, Vector3>(vertexCount * 8, Allocator.Persistent);
            result_writer[i] = result[i].AsParallelWriter();
        }

        CollectNormalJob collectNormalJob = new CollectNormalJob(normals, vertrx, result_writer);
        BakeNormalJob normalBakeJob = new BakeNormalJob(vertrx, normals, tangents, result, Vector3s);

        normalBakeJob.Schedule(vertexCount, 100, collectNormalJob.Schedule(vertexCount, 100)).Complete();

        Vector3[] resultVector3s = new Vector3[Vector3s.Length];
        Vector3s.CopyTo(resultVector3s);

        DisposeAll();

        return resultVector3s;
    }
}
