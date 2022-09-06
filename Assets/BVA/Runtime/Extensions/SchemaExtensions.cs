using GLTF;
using GLTF.Schema;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace BVA.Extensions
{
    public static class SchemaExtensions
    {
        /// <summary>
        /// Define the transformation between Unity coordinate space and glTF.
        /// glTF is a right-handed coordinate system, where the 'right' direction is -X relative to
        /// Unity's coordinate system.
        /// glTF matrix: column vectors, column-major storage, +Y up, +Z forward, -X right, right-handed
        /// unity matrix: column vectors, column-major storage, +Y up, +Z forward, +X right, left-handed
        /// multiply by a negative X scale to convert handedness
        /// </summary>
        public static readonly Vector3 CoordinateSpaceConversionScale = new Vector3(-1, 1, 1);

        /// <summary>
        /// Define whether the coordinate space scale conversion above means we have a change in handedness.
        /// This is used when determining the conventional direction of rotation - the right-hand rule states
        /// that rotations are clockwise in left-handed systems and counter-clockwise in right-handed systems.
        /// Reversing the direction of one or three axes of reverses the handedness.
        /// </summary>
        public static bool CoordinateSpaceConversionRequiresHandednessFlip
        {
            get
            {
                return CoordinateSpaceConversionScale.x * CoordinateSpaceConversionScale.y * CoordinateSpaceConversionScale.z < 0.0f;
            }
        }

        public static readonly Vector4 TangentSpaceConversionScale = new Vector4(-1, 1, 1, -1);

        /// <summary>
        /// Get the converted unity translation, rotation, and scale from a gltf node
        /// </summary>
        /// <param name="node">gltf node</param>
        /// <param name="position">unity translation vector</param>
        /// <param name="rotation">unity rotation quaternion</param>
        /// <param name="scale">unity scale vector</param>
        public static void GetUnityTRSProperties(this Node node, out Vector3 position, out Quaternion rotation,
            out Vector3 scale)
        {
            if (!node.UseTRS)
            {
                Matrix4x4 unityMat = node.Matrix.ToUnityMatrix4x4Convert();
                unityMat.GetTRSProperties(out position, out rotation, out scale);
            }
            else
            {
                position = node.Translation.ToUnityVector3Convert();
                rotation = node.Rotation.ToUnityQuaternionConvert();
                scale = node.Scale;
            }
        }

        /// <summary>
        /// Set a gltf node's converted translation, rotation, and scale from a unity transform
        /// </summary>
        /// <param name="node">gltf node to modify</param>
        /// <param name="transform">unity transform to convert</param>
        public static void SetUnityTransform(this Node node, Transform transform)
        {
            node.Translation = transform.localPosition.ToGltfVector3Convert();
            node.Rotation = transform.localRotation.ToGltfQuaternionConvert();
            node.Scale = transform.localScale;
        }

        // todo: move to utility class
        /// <summary>
        /// Get unity translation, rotation, and scale from a unity matrix
        /// </summary>
        /// <param name="mat">unity matrix to get properties from</param>
        /// <param name="position">unity translation vector</param>
        /// <param name="rotation">unity rotation quaternion</param>
        /// <param name="scale">unity scale vector</param>
        public static void GetTRSProperties(this Matrix4x4 mat, out Vector3 position, out Quaternion rotation,
            out Vector3 scale)
        {
            position = mat.GetColumn(3);

            Vector3 x = mat.GetColumn(0);
            Vector3 y = mat.GetColumn(1);
            Vector3 z = mat.GetColumn(2);
            Vector3 calculatedZ = Vector3.Cross(x, y);
            bool mirrored = Vector3.Dot(calculatedZ, z) < 0.0f;

            scale = new Vector3(x.magnitude * (mirrored ? -1.0f : 1.0f), y.magnitude, z.magnitude);

            rotation = Quaternion.LookRotation(mat.GetColumn(2), mat.GetColumn(1));
        }
        /// <summary>
        /// Get a gltf column vector from a gltf matrix
        /// </summary>
        /// <param name="mat">gltf matrix</param>
        /// <param name="columnNum">the specified column vector from the matrix</param>
        /// <returns></returns>
        public static Vector4 GetColumn(this Matrix4x4 mat, uint columnNum)
        {
            switch (columnNum)
            {
                case 0:
                    {
                        return new Vector4(mat.m00, mat.m10, mat.m20, mat.m30);
                    }
                case 1:
                    {
                        return new Vector4(mat.m01, mat.m11, mat.m21, mat.m31);
                    }
                case 2:
                    {
                        return new Vector4(mat.m02, mat.m12, mat.m22, mat.m32);
                    }
                case 3:
                    {
                        return new Vector4(mat.m03, mat.m13, mat.m23, mat.m33);
                    }
                default:
                    throw new ArgumentOutOfRangeException("column num is out of bounds");
            }
        }

        /// <summary>
        /// Convert gltf quaternion to a unity quaternion
        /// </summary>
        /// <param name="gltfQuat">gltf quaternion</param>
        /// <returns>unity quaternion</returns>
        public static Quaternion ToUnityQuaternionConvert(this Quaternion gltfQuat)
        {
            Vector3 fromAxisOfRotation = new Vector3(gltfQuat.x, gltfQuat.y, gltfQuat.z);
            float axisFlipScale = CoordinateSpaceConversionRequiresHandednessFlip ? -1.0f : 1.0f;
            Vector3 toAxisOfRotation = axisFlipScale * Vector3.Scale(fromAxisOfRotation, CoordinateSpaceConversionScale);

            return new Quaternion(toAxisOfRotation.x, toAxisOfRotation.y, toAxisOfRotation.z, gltfQuat.w);
        }

        /// <summary>
        /// Convert unity quaternion to a gltf quaternion
        /// </summary>
        /// <param name="unityQuat">unity quaternion</param>
        /// <returns>gltf quaternion</returns>
        public static Quaternion ToGltfQuaternionConvert(this Quaternion unityQuat)
        {
            Vector3 fromAxisOfRotation = new Vector3(unityQuat.x, unityQuat.y, unityQuat.z);
            float axisFlipScale = CoordinateSpaceConversionRequiresHandednessFlip ? -1.0f : 1.0f;
            Vector3 toAxisOfRotation = axisFlipScale * Vector3.Scale(fromAxisOfRotation, CoordinateSpaceConversionScale);

            return new Quaternion(toAxisOfRotation.x, toAxisOfRotation.y, toAxisOfRotation.z, unityQuat.w);
        }
        /// <summary>
        /// export as unity vector4 type
        /// </summary>
        /// <param name="unityQuat"></param>
        /// <returns></returns>
        public static Vector4 ToGltfQuaternionConvertToUnityVector4(this Quaternion unityQuat)
        {
            Vector3 fromAxisOfRotation = new Vector3(unityQuat.x, unityQuat.y, unityQuat.z);
            float axisFlipScale = CoordinateSpaceConversionRequiresHandednessFlip ? -1.0f : 1.0f;
            Vector3 toAxisOfRotation = axisFlipScale * Vector3.Scale(fromAxisOfRotation, CoordinateSpaceConversionScale);

            return new Vector4(toAxisOfRotation.x, toAxisOfRotation.y, toAxisOfRotation.z, unityQuat.w);
        }
        /// <summary>
        /// Convert gltf matrix to a unity matrix
        /// </summary>
        /// <param name="gltfMat">gltf matrix</param>
        /// <returns>unity matrix</returns>
        public static Matrix4x4 ToUnityMatrix4x4Convert(this Matrix4x4 gltfMat)
        {
            Matrix4x4 convert = Matrix4x4.Scale(CoordinateSpaceConversionScale);
            Matrix4x4 unityMat = convert * gltfMat * convert;
            return unityMat;
        }
        public static void SetValue(ref Matrix4x4 matrix, uint i, float value)
        {
            switch (i)
            {
                case 0: matrix.m00 = value; break;
                case 1: matrix.m10 = value; break;
                case 2: matrix.m20 = value; break;
                case 3: matrix.m30 = value; break;
                case 4: matrix.m01 = value; break;
                case 5: matrix.m11 = value; break;
                case 6: matrix.m21 = value; break;
                case 7: matrix.m31 = value; break;
                case 8: matrix.m02 = value; break;
                case 9: matrix.m12 = value; break;
                case 10: matrix.m22 = value; break;
                case 11: matrix.m32 = value; break;
                case 12: matrix.m03 = value; break;
                case 13: matrix.m13 = value; break;
                case 14: matrix.m23 = value; break;
                case 15: matrix.m33 = value; break;
            }
        }
        /// <summary>
        /// Convert gltf matrix to a unity matrix
        /// </summary>
        /// <param name="unityMat">unity matrix</param>
        /// <returns>gltf matrix</returns>
        public static Matrix4x4 ToGltfMatrix4x4Convert(this Matrix4x4 unityMat)
        {
            Debug.Log("matrix");
            Matrix4x4 convert = Matrix4x4.Scale(CoordinateSpaceConversionScale);
            Matrix4x4 gltfMat = convert * unityMat * convert;
            return gltfMat;
        }

        /// <summary>
        /// Convert gltf Vector3 to unity Vector3
        /// </summary>
        /// <param name="gltfVec3">gltf vector3</param>
        /// <returns>unity vector3</returns>
        public static Vector3 ToUnityVector3Convert(this Vector3 gltfVec3)
        {
            Vector3 unityVec3 = Vector3.Scale(gltfVec3, CoordinateSpaceConversionScale);
            return unityVec3;
        }

        /// <summary>
        /// Convert unity Vector3 to gltf Vector3
        /// </summary>
        /// <param name="unityVec3">unity Vector3</param>
        /// <returns>gltf Vector3</returns>
        public static Vector3 ToGltfVector3Convert(this Vector3 unityVec3)
        {
            Vector3 gltfVec3 = Vector3.Scale(unityVec3, CoordinateSpaceConversionScale);
            return gltfVec3;
        }
        public static Vector3 ToGltfVector3ConvertToUnityVector3(this Vector3 unityVec3)
        {
            Vector3 gltfVec3 = Vector3.Scale(unityVec3, CoordinateSpaceConversionScale);
            return gltfVec3;
        }
        public static Vector4 ToVector4(this Rect unityRect)
        {
            Vector4 gltfVec4 = new Vector4(unityRect.x, unityRect.y, unityRect.width, unityRect.height);
            return gltfVec4;
        }

        public static JArray ToJArray(this Vector3 vec3)
        {
            return new JArray(vec3.x, vec3.y, vec3.z);
        }

        public static JArray ToJArray(this Vector2 vec2)
        {
            return new JArray(vec2.x, vec2.y);
        }

        public static JArray ToJArray(this Vector4 vec4)
        {
            return new JArray(vec4.x, vec4.y, vec4.z, vec4.w);
        }

        public static JArray ToJArray(this Quaternion quat)
        {
            return new JArray(quat.x, quat.y, quat.z, quat.w);
        }

        public static void ToUnityRaw<T>(this T[] inArr, T[] outArr, int offset)
        {
            Array.Copy(inArr, 0, outArr, offset, inArr.Length);
        }

        public static JArray ToJArray(this Color color)
        {
            return new JArray(color.r, color.g, color.b, color.a);
        }

        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }

        public static Rect ToRect(this Vector4 vec4)
        {
            return new Rect(vec4.x, vec4.y, vec4.z, vec4.w);
        }

        public static int[] ToIntArray(this uint[] uintArr)
        {
            int[] intArr = new int[uintArr.Length];
            for (int i = 0; i < uintArr.Length; ++i)
            {
                uint uintVal = uintArr[i];
                intArr[i] = (int)uintVal;
            }
            return intArr;
        }
        public static int[] ToIntArrayFlipTriangleFaces(this uint[] uintArr)
        {
            int[] intArr = new int[uintArr.Length];

            for (int i = 0; i < intArr.Length; i += 3)
            {
                intArr[i] = (int)uintArr[i + 2];
                intArr[i + 1] = (int)uintArr[i + 1];
                intArr[i + 2] = (int)uintArr[i];
            }

            return intArr;
        }
        /// <summary>
        /// Flips the V component of the UV (1-V) to put from glTF into Unity space
        /// </summary>
        /// <param name="attributeAccessor">The attribute accessor to modify</param>
        public static void FlipTexCoordArrayV(ref AttributeAccessor attributeAccessor)
        {
            for (var i = 0; i < attributeAccessor.AccessorContent.AsVec2s.Length; i++)
            {
                attributeAccessor.AccessorContent.AsVec2s[i].y = 1.0f - attributeAccessor.AccessorContent.AsVec2s[i].y;
            }
        }

        /// <summary>
        /// Flip the V component of the UV (1-V)
        /// </summary>
        /// <param name="array">The array to copy from and modify</param>
        /// <returns>Copied Vector2 with coordinates in glTF space</returns>
        public static Vector2[] FlipTexCoordArrayVAndCopy(Vector2[] array)
        {
            var returnArray = new Vector2[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                returnArray[i].x = array[i].x;
                returnArray[i].y = 1.0f - array[i].y;
            }

            return returnArray;
        }

        /// <summary>
        /// Converts vector3 to specified coordinate space
        /// </summary>
        /// <param name="attributeAccessor">The attribute accessor to modify</param>
        /// <param name="coordinateSpaceCoordinateScale">The coordinate space to move into</param>
        public static void ConvertVector3CoordinateSpace(ref AttributeAccessor attributeAccessor, Vector3 coordinateSpaceCoordinateScale)
        {
            for (int i = 0; i < attributeAccessor.AccessorContent.AsVertices.Length; i++)
            {
                attributeAccessor.AccessorContent.AsVertices[i].x *= coordinateSpaceCoordinateScale.x;
                attributeAccessor.AccessorContent.AsVertices[i].y *= coordinateSpaceCoordinateScale.y;
                attributeAccessor.AccessorContent.AsVertices[i].z *= coordinateSpaceCoordinateScale.z;
            }
        }

        /// <summary>
        /// Converts and copies based on the specified coordinate scale
        /// </summary>
        /// <param name="array">The array to convert and copy</param>
        /// <param name="coordinateSpaceCoordinateScale">The specified coordinate space</param>
        /// <returns>The copied and converted coordinate space</returns>
        public static Vector3[] ConvertVector3CoordinateSpaceAndCopy(Vector3[] array, Vector3 coordinateSpaceCoordinateScale)
        {
            var returnArray = new Vector3[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                returnArray[i].x = array[i].x * coordinateSpaceCoordinateScale.x;
                returnArray[i].y = array[i].y * coordinateSpaceCoordinateScale.y;
                returnArray[i].z = array[i].z * coordinateSpaceCoordinateScale.z;
            }

            return returnArray;
        }

        /// <summary>
        /// Converts and copies based on the specified coordinate scale
        /// </summary>
        /// <param name="array">The array to convert and copy</param>
        /// <param name="coordinateSpaceCoordinateScale">The specified coordinate space</param>
        /// <returns>The copied and converted coordinate space</returns>
        public static Vector3[] ConvertVector3CoordinateSpaceAndCopyNormalized(Vector3[] array, Vector3 coordinateSpaceCoordinateScale)
        {
            var returnArray = new Vector3[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                returnArray[i].x = array[i].x * coordinateSpaceCoordinateScale.x;
                returnArray[i].y = array[i].y * coordinateSpaceCoordinateScale.y;
                returnArray[i].z = array[i].z * coordinateSpaceCoordinateScale.z;
                returnArray[i] = returnArray[i].normalized;
            }

            return returnArray;
        }

        /// <summary>
        /// Converts vector4 to specified coordinate space
        /// </summary>
        /// <param name="attributeAccessor">The attribute accessor to modify</param>
        /// <param name="coordinateSpaceCoordinateScale">The coordinate space to move into</param>
        public static void ConvertVector4CoordinateSpace(ref AttributeAccessor attributeAccessor, Vector4 coordinateSpaceCoordinateScale)
        {
            for (int i = 0; i < attributeAccessor.AccessorContent.AsVec4s.Length; i++)
            {
                attributeAccessor.AccessorContent.AsVec4s[i].x *= coordinateSpaceCoordinateScale.x;
                attributeAccessor.AccessorContent.AsVec4s[i].y *= coordinateSpaceCoordinateScale.y;
                attributeAccessor.AccessorContent.AsVec4s[i].z *= coordinateSpaceCoordinateScale.z;
                attributeAccessor.AccessorContent.AsVec4s[i].w *= coordinateSpaceCoordinateScale.w;
            }
        }

        /// <summary>
        /// Converts and copies based on the specified coordinate scale
        /// </summary>
        /// <param name="array">The array to convert and copy</param>
        /// <param name="coordinateSpaceCoordinateScale">The specified coordinate space</param>
        /// <returns>The copied and converted coordinate space</returns>
        public static Vector4[] ConvertVector4CoordinateSpaceAndCopy(Vector4[] array, Vector4 coordinateSpaceCoordinateScale)
        {
            var returnArray = new Vector4[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                returnArray[i].x = array[i].x * coordinateSpaceCoordinateScale.x;
                returnArray[i].y = array[i].y * coordinateSpaceCoordinateScale.y;
                returnArray[i].z = array[i].z * coordinateSpaceCoordinateScale.z;
                returnArray[i].w = array[i].w * coordinateSpaceCoordinateScale.w;
            }

            return returnArray;
        }

        /// <summary>
        /// Rewinds the indicies into Unity coordinate space from glTF space
        /// </summary>
        /// <param name="attributeAccessor">The attribute accessor to modify</param>
        public static void FlipTriangleFaces(int[] indices)
        {
            for (int i = 0; i < indices.Length; i += 3)
            {
                (indices[i], indices[i + 2]) = (indices[i + 2], indices[i]);
            }
        }
    }
}
