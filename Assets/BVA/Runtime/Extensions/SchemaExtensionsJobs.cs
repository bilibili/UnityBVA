using System;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using UnityEngine;
namespace BVA.Extensions.LowLevel.Unsafe
{
    public static unsafe class SchemaExtensionsJobs
    {
        [BurstCompile]
        public struct FlipTexCoordArrayVJob : IJobParallelFor
        {
            [NoAlias]
            public NativeArray<GLTF.Math.Vector2> inOutArr;
            public void Execute(int index)
            {
                var value = inOutArr[index];
                value.Y = -value.Y;
                inOutArr[index] = value;
            }
        }
        [BurstCompile]
        public struct FlipTexCoordArrayVAndCopyJob : IJobParallelFor
        {
            [NoAlias]
            public NativeArray<UnityEngine.Vector2> inOutArr;
            public void Execute(int index)
            {
                var value = inOutArr[index];
                value.y = 1 - value.y;
                inOutArr[index] = value;
            }
        }

        [BurstCompile]
        public struct ConvertVector3CoordinateSpaceJob : IJobParallelFor
        {
            [NoAlias]
            public NativeArray<GLTF.Math.Vector3> inOutArr;
            internal GLTF.Math.Vector3 coordinateSpaceCoordinateScale;

            public void Execute(int index)
            {
                var value = inOutArr[index];
                value.X *= coordinateSpaceCoordinateScale.X;
                value.Y *= coordinateSpaceCoordinateScale.Y;
                value.Z *= coordinateSpaceCoordinateScale.Z;
                inOutArr[index] = value;
            }
        }

        [BurstCompile]
        public struct ConvertVector3CoordinateSpaceAndCopyJob : IJobParallelFor
        {
            [NoAlias]
            public NativeArray<UnityEngine.Vector3> inOutArr;
            internal GLTF.Math.Vector3 coordinateSpaceCoordinateScale;

            public void Execute(int index)
            {
                var value = inOutArr[index];
                value.x *= coordinateSpaceCoordinateScale.X;
                value.y *= coordinateSpaceCoordinateScale.Y;
                value.z *= coordinateSpaceCoordinateScale.Z;
                inOutArr[index] = value;
            }
        }

        [BurstCompile]
        public struct ConvertVector4CoordinateSpaceJob : IJobParallelFor
        {
            [NoAlias]
            public NativeArray<GLTF.Math.Vector4> inOutArr;
            internal GLTF.Math.Vector4 coordinateSpaceCoordinateScale;

            public void Execute(int index)
            {
                var value = inOutArr[index];
                value.X *= coordinateSpaceCoordinateScale.X;
                value.Y *= coordinateSpaceCoordinateScale.Y;
                value.Z *= coordinateSpaceCoordinateScale.Z;
                value.W *= coordinateSpaceCoordinateScale.W;
                inOutArr[index] = value;
            }
        }
        [BurstCompile]
        public struct ConvertVector4CoordinateSpaceAndCopyJob : IJobParallelFor
        {
            [NoAlias]
            public NativeArray<UnityEngine.Vector4> inOutArr;
            internal GLTF.Math.Vector4 coordinateSpaceCoordinateScale;

            public void Execute(int index)
            {
                var value = inOutArr[index];
                value.x *= coordinateSpaceCoordinateScale.X;
                value.y *= coordinateSpaceCoordinateScale.Y;
                value.z *= coordinateSpaceCoordinateScale.Z;
                value.w *= coordinateSpaceCoordinateScale.W;
                inOutArr[index] = value;
            }
        }
        [BurstCompile]
        public struct FlipTriangleFacesJob : IJobParallelFor
        {
            [NativeDisableUnsafePtrRestriction]
            public int* inOutArrPtr;

            public void Execute(int index)
            {
                if (index % 3 == 0)
                {
                    (inOutArrPtr[index + 2], inOutArrPtr[index]) = (inOutArrPtr[index], inOutArrPtr[index + 2]);
                }
            }
        }
    }

    public unsafe static class SchemaExtensionsUnsafe
    {

        public static void ToUnityVector3RawUnsafe(this GLTF.Math.Vector3[] inArr, Vector3[] outArr, int offset = 0)
        {
            GLTF.Math.Vector3* inArrPtr = (GLTF.Math.Vector3*)UnsafeUtility.PinGCArrayAndGetDataAddress(inArr, out ulong inArrGcHandle);
            Vector3* outArrPtr = (Vector3*)UnsafeUtility.PinGCArrayAndGetDataAddress(outArr, out ulong outArrGcHandle);
            outArrPtr += offset;
            UnsafeUtility.MemCpy(outArrPtr, inArrPtr, inArr.Length * UnsafeUtility.SizeOf(typeof(GLTF.Math.Vector3)));
            UnsafeUtility.ReleaseGCObject(inArrGcHandle);
            UnsafeUtility.ReleaseGCObject(outArrGcHandle);

        }

        public static void ToUnityVector4RawUnsafe(this GLTF.Math.Vector4[] inArr, Vector4[] outArr, int offset = 0)
        {
            GLTF.Math.Vector4* inArrPtr = (GLTF.Math.Vector4*)UnsafeUtility.PinGCArrayAndGetDataAddress(inArr, out ulong inArrGcHandle);
            Vector4* outArrPtr = (Vector4*)UnsafeUtility.PinGCArrayAndGetDataAddress(outArr, out ulong outArrGcHandle);
            outArrPtr = outArrPtr + offset;
            UnsafeUtility.MemCpy(outArrPtr, inArrPtr, inArr.Length * UnsafeUtility.SizeOf(typeof(GLTF.Math.Vector4)));
            UnsafeUtility.ReleaseGCObject(inArrGcHandle);
            UnsafeUtility.ReleaseGCObject(outArrGcHandle);

        }

        public static void ToUnityVector2RawUnsafe(this GLTF.Math.Vector2[] inArr, Vector2[] outArr, int offset = 0)
        {
            GLTF.Math.Vector2* inArrPtr = (GLTF.Math.Vector2*)UnsafeUtility.PinGCArrayAndGetDataAddress(inArr, out ulong inArrGcHandle);
            Vector2* outArrPtr = (Vector2*)UnsafeUtility.PinGCArrayAndGetDataAddress(outArr, out ulong outArrGcHandle);
            outArrPtr += offset;
            UnsafeUtility.MemCpy(outArrPtr, inArrPtr, inArr.Length * UnsafeUtility.SizeOf(typeof(GLTF.Math.Vector2)));
            UnsafeUtility.ReleaseGCObject(inArrGcHandle);
            UnsafeUtility.ReleaseGCObject(outArrGcHandle);

        }

        public static void ToUnityVector4RawUnsafe(this GLTF.Math.Color[] inArr, Color[] outArr, int offset = 0)
        {
            GLTF.Math.Color* inArrPtr = (GLTF.Math.Color*)UnsafeUtility.PinGCArrayAndGetDataAddress(inArr, out ulong inArrGcHandle);
            Color* outArrPtr = (Color*)UnsafeUtility.PinGCArrayAndGetDataAddress(outArr, out ulong outArrGcHandle);
            outArrPtr += offset;
            UnsafeUtility.MemCpy(outArrPtr, inArrPtr, inArr.Length * UnsafeUtility.SizeOf(typeof(GLTF.Math.Color)));
            UnsafeUtility.ReleaseGCObject(inArrGcHandle);
            UnsafeUtility.ReleaseGCObject(outArrGcHandle);

        }

        public static void FlipTriangleFacesUnsafe(int[] indices)
        {
            int length = indices.Length;
            if (length % 3 != 0)
            {
                throw new InvalidOperationException();
            }

            int count = length / 3;
            int componentSize = UnsafeUtility.SizeOf(typeof(int));
            int stride = 3 * componentSize;
            int* flipPtr = (int*)UnsafeUtility.Malloc(length * componentSize, 16, Unity.Collections.Allocator.Temp);
            int* rawPtr = (int*)UnsafeUtility.PinGCArrayAndGetDataAddress(indices, out ulong inArrGcHandle);
            {
                UnsafeUtility.MemCpyStride(flipPtr, stride, rawPtr + 2, stride, componentSize, count);
                UnsafeUtility.MemCpyStride(flipPtr + 2, stride, rawPtr, stride, componentSize, count);
                UnsafeUtility.MemCpyStride(flipPtr + 1, stride, rawPtr + 1, stride, componentSize, count);

                UnsafeUtility.MemCpy(rawPtr, flipPtr, length);
            }
            UnsafeUtility.ReleaseGCObject(inArrGcHandle);
            UnsafeUtility.Free(flipPtr, Unity.Collections.Allocator.Temp);
        }
    }
}