using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BVA.Extensions
{
    public static class CSharpExtensions
    {
        public static string ConvertSpaceString(this string relativeFilePath)
        {
            return relativeFilePath.Replace("%20", " ");
        }
        public static string UnityAssetPath(this string url)
        {
            int start = url.IndexOf("Assets/");
            if (start == -1) return url;
            return url.Substring(start);
        }

        public static string FormatBase64(this string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string FormatAssetPath(this string path)
        {
            string text = path.Replace("\\", "/").Replace(" ", "").Replace("[", "").Replace("]", "").Replace("$", "").Replace("@", "").Replace("#", "").Replace("!", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "");
            return text;
        }

        public static string FormatSafeClipName(this string name)
        {
            string text = name;
            int num = text.IndexOf("@");
            if (num >= 0)
            {
                text = text.Substring(num + 1);
            }
            return text;
        }

        public static string FormatSafeAnimName(this string name)
        {
            return name.Replace(" ", "_").Replace("-", "_");
        }

        public static bool IsUrl(this string url)
        {
            if (url.Contains("localhost:") || Regex.IsMatch(url, @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?"))
                return true;
            else
                return false;
        }

        public static string GetNumberAlphaUnderLine(this string source)
        {
            StringWriter ret = new StringWriter();
            foreach (var v in source)
            {
                if (v >= '0' && v <= '9')
                    ret.Write(v);
                if (v >= 'a' && v <= 'z')
                    ret.Write(v);
                if (v >= 'A' && v <= 'Z')
                    ret.Write(v);
                if (v == '_')
                    ret.Write(v);
            }
            return ret.ToString();
        }

        public static string EncodeUTF8(this string input)
        {
            var jis = Encoding.GetEncoding("UTF-8");
            Byte[] encodedBytes = jis.GetBytes(input);
            String decodedString = jis.GetString(encodedBytes);
            return decodedString;
        }

        public static string FirstLowercase(this string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 1)
            {
                return str;
            }
            str = $"{str.Substring(0, 1).ToLower()}{str.Substring(1)}";
            return str;
        }

        public static string FirstUppercase(this string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 1)
            {
                return str;
            }
            str = $"{str.Substring(0, 1).ToUpper()}{str.Substring(1)}";
            return str;
        }

        public static string RemoveEmpty(this string str)
        {
            str = str.Replace(" ", "");
            return str;
        }

        public static string AddEmptyBeforeUppercase(this string str)
        {
            List<int> uppercaseIndex = new List<int>(4);
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 'A' && str[i] <= 'Z')
                {
                    uppercaseIndex.Add(i);
                }
            }
            int j = 0;
            foreach (var v in uppercaseIndex)
            {
                str = str.Insert(v + j++, " ");
            }
            return str;
        }
        public static string RemoveExtension(this string str)
        {
            return str.Substring(0, str.LastIndexOf('.'));
        }

        public static void ResetCount<T>(this List<T> list, int count) where T : class
        {
            if (list.Count == count) return;
            else if (list.Count > count)
            {
                while (list.Count > count)
                {
                    list.RemoveAt(list.Count - 1);
                }
            }
            else
            {
                int needCount = count - list.Count;
                for (int i = 0; i < needCount; i++)
                {
                    list.Add(null);
                }
            }
        }

        public static string GetUniqueName(this string name, IEnumerable<string> list)
        {
            if (list.Contains(name))
            {
                var prefixMatchedName = new List<string>();
                foreach (var v in list)
                {
                    if (v.StartsWith(name) && v.EndsWith(")"))
                    {
                        prefixMatchedName.Add(v);
                    }
                }
                if (prefixMatchedName.Count() > 0)
                {
                    var number = prefixMatchedName.Select(x =>
                    {
                        int startIndex = x.LastIndexOf('(');
                        int endIndex = x.LastIndexOf(')');
                        string prefixName = x.Substring(0, startIndex);
                        if (startIndex > 0 && endIndex > 0 && endIndex > startIndex)
                        {
                            string numberStr = x.Substring(startIndex + 1, endIndex - startIndex - 1);
                            if (int.TryParse(numberStr, out int number))
                            {
                                return number;
                            }
                        }
                        return -1;
                    });

                    return name + '(' + (number.Max() + 1) + ')';
                }
                else
                {
                    return name += "(0)";
                }
            }
            return name;
        }
    }
    /*					
     	if (_MODE == 0) {
			//result.rgb = DecodeLightmap(result, float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0h, 0.0h));
			result = EncodeRgbm(result);
		}
		else if (_MODE == 1) {
			//result.rgb = DecodeLightmap(result, float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0h, 0.0h));
			result = EncodeDldr(result);
		}
		else if (_MODE == 2) {
			result = DecodeRgbm(result);
		}
		else if (_MODE == 3) {
			//result.rgb = DecodeLightmap(result, float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0h, 0.0h));
			result = DecodeDldr(result);
		}
		else if (_MODE == 4) {
			result.rgb = GammaToLinearSpace(result.rgb);
		}
		else if (_MODE == 5) {
			result.rgb = LinearToGammaSpace(result.rgb);
		}
    */
    public enum LightmapEncodingMode : int
    {
        EncodeRGBM,
        EncodeDLDR,
        DecodeRGBM,
        DecodeDLDR,
        GammaToLinear,
        LinearToGamma
    }
    public static class UnityExtensions
    {
        private static Material _flipYMaterial;
        public static Material FlipYMaterial
        {
            get
            {
                if (_flipYMaterial == null)
                    _flipYMaterial = new Material(Resources.Load<Shader>("FlipY"));
                return _flipYMaterial;
            }
        }

        private static Material _lightmapEncodeMaterial;
        public static Material LightmapEncodeMaterial
        {
            get
            {
                if (_lightmapEncodeMaterial == null)
                    _lightmapEncodeMaterial = new Material(Resources.Load<Shader>("EncodeLightmap"));
                return _lightmapEncodeMaterial;
            }
        }
        private static Material _metalGlossChannelSwapMaterial;
        public static Material MetalGlossChannelSwapMaterial
        {
            get
            {
                if (_metalGlossChannelSwapMaterial == null)
                    _metalGlossChannelSwapMaterial = new Material(Resources.Load<Shader>("MetalGlossChannelSwap"));
                return _metalGlossChannelSwapMaterial;
            }
        }
        private static Material _normalChannelMaterial;
        public static Material NormalChannelMaterial
        {
            get
            {
                if (_normalChannelMaterial == null)
                    _normalChannelMaterial = new Material(Resources.Load<Shader>("NormalChannel"));
                return _normalChannelMaterial;
            }
        }

        public static Material GetLightmapEncodeMaterial(LightmapEncodingMode mode)
        {
            LightmapEncodeMaterial.SetInt("_MODE", (int)mode);
            return LightmapEncodeMaterial;
        }

        public static void DestroyAllChild(this Transform transform)
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
        public static string ImageToBase64String(this string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                string base64String = Convert.ToBase64String(buffer);
                Debug.Log("获取当前图片base64为 :\n\t" + base64String);
                return base64String;
            }
            catch (Exception e)
            {
                Debug.LogError("ImageToBase64String 转换失败:" + e.Message);
            }
            return null;
        }

        /// <summary>
        /// base64编码文本转换成图片
        /// </summary>
        public static Texture2D Base64ToImage(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Texture2D tex2D = new Texture2D(0, 0);
            tex2D.LoadImage(bytes);
            return tex2D;
        }
        public static void CalculateYawPitch(this Matrix4x4 m, Vector3 target, out float yaw, out float pitch)
        {
            var zaxis = Vector3.Project(target, m.GetColumn(2));
            var yaxis = Vector3.Project(target, m.GetColumn(1));
            var xaxis = Vector3.Project(target, m.GetColumn(0));

            var yawPlusMinus = Vector3.Dot(xaxis, m.GetColumn(0)) > 0 ? 1.0f : -1.0f;
            yaw = (float)Math.Atan2(xaxis.magnitude, zaxis.magnitude) * yawPlusMinus * Mathf.Rad2Deg;

            var pitchPlusMinus = Vector3.Dot(yaxis, m.GetColumn(1)) > 0 ? 1.0f : -1.0f;
            pitch = (float)Math.Atan2(yaxis.magnitude, (xaxis + zaxis).magnitude) * pitchPlusMinus * Mathf.Rad2Deg;
        }

        public static Matrix4x4 Matrix4x4FromColumns(Vector4 c0, Vector4 c1, Vector4 c2, Vector4 c3)
        {
            return new Matrix4x4(c0, c1, c2, c3);
        }

        public static Matrix4x4 Matrix4x4FromRotation(Quaternion q)
        {
            return Matrix4x4.Rotate(q);
        }

        public static Matrix4x4 ReverseX(this Matrix4x4 m)
        {
            Vector3 coordinateSpaceConversionScale = SchemaExtensions.CoordinateSpaceConversionScale.ToUnityVector3Raw();
            Matrix4x4 convert = Matrix4x4.Scale(coordinateSpaceConversionScale);
            Matrix4x4 gltfMat = (convert * m * convert);
            return gltfMat;
        }

        public static Matrix4x4 ReverseZ(this Matrix4x4 m)
        {
            Vector3 coordinateSpaceConversionScale = new Vector3(1, 1, -1);
            Matrix4x4 convert = Matrix4x4.Scale(coordinateSpaceConversionScale);
            Matrix4x4 gltfMat = (convert * m * convert);
            return gltfMat;
        }

        public static Matrix4x4 MatrixFromArray(float[] values)
        {
            var m = new Matrix4x4();
            m.m00 = values[0];
            m.m10 = values[1];
            m.m20 = values[2];
            m.m30 = values[3];
            m.m01 = values[4];
            m.m11 = values[5];
            m.m21 = values[6];
            m.m31 = values[7];
            m.m02 = values[8];
            m.m12 = values[9];
            m.m22 = values[10];
            m.m32 = values[11];
            m.m03 = values[12];
            m.m13 = values[13];
            m.m23 = values[14];
            m.m33 = values[15];
            return m;
        }

        public static Quaternion ExtractRotation(this Matrix4x4 matrix)
        {
            Vector3 forward;
            forward.x = matrix.m02;
            forward.y = matrix.m12;
            forward.z = matrix.m22;

            Vector3 upwards;
            upwards.x = matrix.m01;
            upwards.y = matrix.m11;
            upwards.z = matrix.m21;

            return Quaternion.LookRotation(forward, upwards);
        }
        public static Matrix4x4 RotationToWorldAxis(this Matrix4x4 m)
        {
            return UnityExtensions.Matrix4x4FromColumns(
                m.MultiplyVector(Vector3.right),
                m.MultiplyVector(Vector3.up),
                m.MultiplyVector(Vector3.forward),
                new Vector4(0, 0, 0, 1)
                );
        }
        public static Quaternion YawPitchRotation(this Matrix4x4 m, float yaw, float pitch)
        {
            return Quaternion.AngleAxis(yaw, m.GetColumn(1)) * Quaternion.AngleAxis(-pitch, m.GetColumn(0));
        }

        public static Vector3 ExtractPosition(this Matrix4x4 matrix)
        {
            Vector3 position;
            position.x = matrix.m03;
            position.y = matrix.m13;
            position.z = matrix.m23;
            return position;
        }

        public static Vector3 ExtractScale(this Matrix4x4 matrix)
        {
            Vector3 scale;
            scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
            scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
            scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
            return scale;
        }

        public static string RelativePathFrom(this Transform self, Transform root)
        {
            var path = new List<string>();
            for (var current = self; current != null; current = current.parent)
            {
                if (current == root)
                {
                    return string.Join("/", path.ToArray());
                }

                path.Insert(0, current.name);
            }

            throw new Exception("no RelativePath");
        }

        public static Transform GetChildByName(this Transform self, string childName)
        {
            foreach (Transform child in self)
            {
                if (child.name == childName)
                {
                    return child;
                }
            }

            throw new KeyNotFoundException();
        }

        public static bool HasChildComponent<T>(this Transform transform)
        {
            bool flag = false;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                flag = (transform.GetChild(i).GetComponent<T>() != null);
                if (flag)
                {
                    break;
                }
            }
            return flag;
        }
        public static Transform GetFromPath(this Transform self, string path)
        {
            var current = self;

            var split = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var childName in split)
            {
                current = current.GetChildByName(childName);
            }

            return current;
        }

        public static IEnumerable<Transform> GetChildren(this Transform self)
        {
            foreach (Transform child in self)
            {
                yield return child;
            }
        }

        public static float[] ToArray(this Quaternion q)
        {
            return new float[] { q.x, q.y, q.z, q.w };
        }

        public static float[] ToArray(this Vector3 v)
        {
            return new float[] { v.x, v.y, v.z };
        }

        public static float[] ToArray(this Vector4 v)
        {
            return new float[] { v.x, v.y, v.z, v.w };
        }

        public static Mesh GetSharedMesh(this Transform t)
        {
            if (t.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                return meshFilter.sharedMesh;
            }

            if (t.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
            {
                return skinnedMeshRenderer.sharedMesh;
            }

            return null;
        }

        public static Material[] GetSharedMaterials(this Transform t)
        {
            if (t.TryGetComponent<Renderer>(out var renderer))
            {
                return renderer.sharedMaterials;
            }

            return new Material[] { };
        }

        public static bool Has<T>(this Transform transform, T t) where T : UnityEngine.Component
        {
            return transform.GetComponent<T>() == t;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component
        {
            if (gameObject.TryGetComponent<T>(out var component))
                return component;
            return gameObject.AddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this Transform transform) where T : UnityEngine.Component
        {
            if (transform.TryGetComponent<T>(out var component))
                return component;
            return transform.gameObject.AddComponent<T>();
        }
        static public string[] GetAnimationClipNames(this Animation animation)
        {
            if (!animation)
            {
                return new string[0] { };
            }

            List<string> names = new List<string>();
            IEnumerator enumerator = animation.GetEnumerator();
            while (enumerator.MoveNext())
            {
                names.Add((enumerator.Current as AnimationState)?.name ?? "");
            }
            return names.Where(n => !string.IsNullOrWhiteSpace(n)).ToArray();
        }

        static public AnimationClip[] GetAnimationClips(this Animation animation)
        {
            if (!animation)
            {
                return new AnimationClip[0] { };
            }

            List<AnimationClip> clips = new List<AnimationClip>();
            IEnumerator enumerator = animation.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var clip = (enumerator.Current as AnimationState)?.clip;
                clips.Add(clip);
            }
            return clips.ToArray();
        }
        public static Transform DeepFindChild(this Transform parent, string targetName)
        {
            Transform _result = parent.Find(targetName);
            if (_result == null)
            {
                foreach (Transform child in parent)
                {
                    _result = DeepFindChild(child, targetName);
                    if (_result != null)
                    {
                        return _result;
                    }
                }
            }
            return _result;
        }
        public static bool IsSkinTransform(this Transform transform)
        {
            return transform.GetComponent<SkinnedMeshRenderer>() != null;
        }

        public static Color InvertRange(this Color color)
        {
            return new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
        }
        public static Vector3 GetPosition(this Matrix4x4 m)
        {
            return new Vector3(m[0, 3], m[1, 3], m[2, 3]);
        }

        public static Vector3 GetScale(this Matrix4x4 m)
        {
            return new Vector3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
        }
        public static Quaternion GetRotation(this Matrix4x4 m)
        {
            Vector3 scale = m.GetScale();
            float num = m[0, 0] / scale.x;
            float num2 = m[0, 1] / scale.y;
            float num3 = m[0, 2] / scale.z;
            float num4 = m[1, 0] / scale.x;
            float num5 = m[1, 1] / scale.y;
            float num6 = m[1, 2] / scale.z;
            float num7 = m[2, 0] / scale.x;
            float num8 = m[2, 1] / scale.y;
            float num9 = m[2, 2] / scale.z;
            Quaternion quaternion = default(Quaternion);
            quaternion.w = Mathf.Sqrt(Mathf.Max(0f, 1f + num + num5 + num9)) / 2f;
            quaternion.x = Mathf.Sqrt(Mathf.Max(0f, 1f + num - num5 - num9)) / 2f;
            quaternion.y = Mathf.Sqrt(Mathf.Max(0f, 1f - num + num5 - num9)) / 2f;
            quaternion.z = Mathf.Sqrt(Mathf.Max(0f, 1f - num - num5 + num9)) / 2f;
            quaternion.x *= Mathf.Sign(quaternion.x * (num8 - num6));
            quaternion.y *= Mathf.Sign(quaternion.y * (num3 - num7));
            quaternion.z *= Mathf.Sign(quaternion.z * (num4 - num2));
            float num10 = Mathf.Sqrt(quaternion.w * quaternion.w + quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z);
            quaternion.w /= num10;
            quaternion.x /= num10;
            quaternion.y /= num10;
            quaternion.z /= num10;
            return quaternion;
        }
#if UNITY_EDITOR
        public static List<Texture> GetTextures(this Material source)
        {
            Shader shader = source.shader;
            List<Texture> list = new List<Texture>();
            for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
            {
                if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    list.Add(source.GetTexture(ShaderUtil.GetPropertyName(shader, i)));
                }
            }
            return list;
        }
#endif
        public static SkeletonBone ToSkeletonBone(this Transform t)
        {
            var sb = new SkeletonBone();
            sb.name = t.name;
            sb.position = t.localPosition;
            sb.rotation = t.localRotation;
            sb.scale = t.localScale;
            return sb;
        }
        public static bool IsLayerRendered(this ReflectionProbe source, int layer)
        {
            return (source.cullingMask & 1 << layer) != 0;
        }

        public static bool IsLayerRendered(this Camera source, int layer)
        {
            return (source.cullingMask & 1 << layer) != 0;
        }

        public static bool IsLayerRendered(this Light source, int layer)
        {
            return (source.cullingMask & 1 << layer) != 0;
        }
    }
}
