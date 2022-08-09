using GLTF.Schema.BVA;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace BVA.Extensions
{
    public static class CubemapExtensions
    {
        public static CubemapImageType GetCubemapImageType(int width, int height)
        {
            CubemapImageType cubemapImageType = CubemapImageType.Unknown;
            if (width == 6 * height)
                cubemapImageType = CubemapImageType.Row;
            if (height == 6 * width)
                cubemapImageType = CubemapImageType.Column;
            if (width == 2 * height)
                cubemapImageType = CubemapImageType.Equirect;
            return cubemapImageType;
        }
        /// <summary>
        /// Work on Standalone, but loaded failed on Mobile devices
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="mipmap"></param>
        /// <returns></returns>
        public static Cubemap CreateCubemapFromTexture(Texture2D texture, bool mipmap)
        {
            Texture2D exportTexture = null;
            CubemapImageType imageType = GetCubemapImageType(texture.width, texture.height);

            //sometimes it requires use Non-HDR creation, 
            var destRenderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);

            //var flipY = new Material(Shader.Find("Hidden/FlipY"));
            Graphics.Blit(texture, destRenderTexture);

#if UNITY_ANDROID || UNITY_IOS
            exportTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, mipCount: 0, linear: true);
#else
            exportTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBAHalf, mipCount: 0, linear: true);
#endif
            RenderTexture.active = destRenderTexture;
            exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
            exportTexture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
            RenderTexture.ReleaseTemporary(destRenderTexture);
            GameObject.DestroyImmediate(texture);

            if (imageType == CubemapImageType.Equirect)
            {
                Texture2D[] cubemapFaces = new Texture2D[6];
                int srcWidth = exportTexture.height / 2;
#if UNITY_ANDROID || UNITY_IOS
                Cubemap cubemap = new Cubemap(srcWidth, TextureFormat.ARGB32, false);
#else
                Cubemap cubemap = new Cubemap(srcWidth, UnityEngine.Experimental.Rendering.DefaultFormat.HDR, 0);
#endif
                for (int i = 0; i < 6; ++i)
                {
                    cubemapFaces[i] = EquirectToCubemapTexture(exportTexture, srcWidth, (CubemapFace)i);
                    Graphics.CopyTexture(cubemapFaces[i], 0, 0, 0, 0, srcWidth, srcWidth, cubemap, i, 0, 0, 0);
                }
                cubemap.Apply();
                return cubemap;
            }
            else
            {
                int srcWidth = Mathf.Min(exportTexture.width, exportTexture.height);
#if UNITY_ANDROID || UNITY_IOS
                Cubemap cubemap = new Cubemap(srcWidth, TextureFormat.ARGB32, false);
#else
                Cubemap cubemap = new Cubemap(srcWidth, UnityEngine.Experimental.Rendering.DefaultFormat.HDR, 0);
#endif
                bool isColumn = imageType == CubemapImageType.Column;
                for (int i = 0; i < 6; ++i)
                {
                    int srcX = isColumn ? 0 : srcWidth * i;
                    int srcY = isColumn ? srcWidth * i : 0;
                    Graphics.CopyTexture(exportTexture, 0, 0, srcX, srcY, srcWidth, srcWidth, cubemap, i, 0, 0, 0);
                }
                cubemap.Apply();
                return cubemap;
            }
        }

        public static Texture2D[] UnpackTextures(this Cubemap src)
        {
            Texture2D[] textures = new Texture2D[6];
            for (int i = 0; i < 6; ++i)
            {
                CubemapFace face = (CubemapFace)i;
                var colors = src.GetPixels(face);
                textures[i] = new Texture2D(src.width, src.width, TextureFormat.RGB24, false);
                textures[i].name = src.name + "." + face.ToString();
                textures[i].SetPixels(colors);
                textures[i].Apply();
            }
            return textures;
        }

        public static Texture2D FlatTexture(this Cubemap src, bool isColumn = false)
        {
            int srcWidth = src.width;

            int newWidth = isColumn ? srcWidth : srcWidth * 6;
            int newHeight = isColumn ? srcWidth * 6 : srcWidth;
            var flatCubemap = new Texture2D(newWidth, newHeight, src.format, false, false);
            for (int i = 0; i < 6; ++i)
            {
                int dstX = isColumn ? 0 : srcWidth * i;
                int dstY = isColumn ? srcWidth * i : 0;
                Graphics.CopyTexture(src, i, 0, 0, 0, srcWidth, srcWidth, flatCubemap, 0, 0, dstX, dstY);
            }

            var destRenderTexture = RenderTexture.GetTemporary(newWidth, newHeight, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);

            Graphics.Blit(flatCubemap, destRenderTexture, UnityExtensions.FlipYMaterial);
            Graphics.Blit(flatCubemap, destRenderTexture, UnityExtensions.GetLightmapEncodeMaterial(LightmapEncodingMode.LinearToGamma));

            var exportTexture = new Texture2D(newWidth, newHeight);
            exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
            exportTexture.Apply();

            RenderTexture.ReleaseTemporary(destRenderTexture);
            GameObject.DestroyImmediate(flatCubemap);

            exportTexture.name = src.name;
            return exportTexture;
        }

        public static byte[] ConvertToEquirectangular(Texture2D[] sourceTextures, int outputWidth, int outputHeight)
        {
            Texture2D equiTexture = new Texture2D(outputWidth, outputHeight, TextureFormat.RGB24, false);
            float u, v; //Normalised texture coordinates, from 0 to 1, starting at lower left corner
            float phi, theta; //Polar coordinates
            int cubeFaceWidth, cubeFaceHeight;

            cubeFaceWidth = sourceTextures[0].width; //4 horizontal faces
            cubeFaceHeight = sourceTextures[0].height; //3 vertical faces


            for (int j = 0; j < equiTexture.height; j++)
            {
                //Rows start from the bottom
                v = 1 - ((float)j / equiTexture.height);
                theta = v * Mathf.PI;

                for (int i = 0; i < equiTexture.width; i++)
                {
                    //Columns start from the left
                    u = ((float)i / equiTexture.width);
                    phi = u * 2 * Mathf.PI;

                    float x, y, z; //Unit vector
                    x = Mathf.Sin(phi) * Mathf.Sin(theta) * -1;
                    y = Mathf.Cos(theta);
                    z = Mathf.Cos(phi) * Mathf.Sin(theta) * -1;

                    float xa, ya, za;
                    float a;

                    a = Mathf.Max(new float[3] { Mathf.Abs(x), Mathf.Abs(y), Mathf.Abs(z)
    });

                    //Vector Parallel to the unit vector that lies on one of the cube faces
                    xa = x / a;
                    ya = y / a;
                    za = z / a;

                    Color color = Color.white;
                    int xPixel, yPixel;
                    int xOffset, yOffset;

                    if (xa == 1)
                    {
                        //Right
                        xPixel = (int)((((za + 1f) / 2f) - 1f) * cubeFaceWidth);
                        xOffset = 0; //Offset
                        yPixel = (int)((((ya + 1f) / 2f)) * cubeFaceHeight);
                        yOffset = 0; //Offset
                    }

                    else if (xa == -1)
                    {
                        //Left
                        xPixel = (int)((((za + 1f) / 2f)) * cubeFaceWidth);
                        xOffset = 0;
                        yPixel = (int)((((ya + 1f) / 2f)) * cubeFaceHeight);
                        yOffset = 0;
                    }
                    else if (ya == 1)
                    {
                        //Up
                        xPixel = (int)((((xa + 1f) / 2f)) * cubeFaceWidth);
                        xOffset = 0;
                        yPixel = (int)((((za + 1f) / 2f) - 1f) * cubeFaceHeight);
                        yOffset = 0;
                    }
                    else if (ya == -1)
                    {
                        //Down
                        xPixel = (int)((((xa + 1f) / 2f)) * cubeFaceWidth);
                        xOffset = 0;
                        yPixel = (int)((((za + 1f) / 2f)) * cubeFaceHeight);
                        yOffset = 0;
                    }
                    else if (za == 1)
                    {
                        //Front
                        xPixel = (int)((((xa + 1f) / 2f)) * cubeFaceWidth);
                        xOffset = 0;
                        yPixel = (int)((((ya + 1f) / 2f)) * cubeFaceHeight);
                        yOffset = 0;
                    }
                    else if (za == -1)
                    {
                        //Back
                        xPixel = (int)((((xa + 1f) / 2f) - 1f) * cubeFaceWidth);
                        xOffset = 0;
                        yPixel = (int)((((ya + 1f) / 2f)) * cubeFaceHeight);
                        yOffset = 0;
                    }
                    else
                    {
                        Debug.LogWarning("Unknown face, something went wrong");
                        xPixel = 0;
                        yPixel = 0;
                        xOffset = 0;
                        yOffset = 0;
                    }

                    xPixel = Mathf.Abs(xPixel);
                    yPixel = Mathf.Abs(yPixel);

                    xPixel += xOffset;
                    yPixel += yOffset;
                    if (xa == 1)
                    {
                        color = sourceTextures[0].GetPixel(xPixel, yPixel);
                    }
                    else if (xa == -1)
                    {
                        color = sourceTextures[1].GetPixel(xPixel, yPixel);
                    }
                    else if (ya == 1)
                    {
                        color = sourceTextures[2].GetPixel(xPixel, yPixel);
                    }
                    else if (ya == -1)
                    {
                        color = sourceTextures[3].GetPixel(xPixel, yPixel);
                    }
                    else if (za == 1)
                    {
                        color = sourceTextures[4].GetPixel(xPixel, yPixel);
                    }
                    else if (za == -1)
                    {
                        color = sourceTextures[5].GetPixel(xPixel, yPixel);
                    }
                    equiTexture.SetPixel(i, j, color);
                }
            }

            equiTexture.Apply();
            var bytes = equiTexture.EncodeToPNG();
            UnityEngine.Object.DestroyImmediate(equiTexture);

            return bytes;
        }
        public static byte[] ConvertToFlat(Texture2D[] sourceTextures, int width, bool isColumn = false)
        {
            int srcWidth = sourceTextures[0].width;
            var flatCubemap = isColumn ? new Texture2D(srcWidth, srcWidth * 6, TextureFormat.RGB24, false) : new Texture2D(srcWidth * 6, srcWidth, TextureFormat.RGB24, false);
            for (int i = 0; i < 6; ++i)
            {
                int dstX = isColumn ? 0 : srcWidth * i;
                int dstY = isColumn ? srcWidth * (5 - i) : 0;
                Graphics.CopyTexture(sourceTextures[i], 0, 0, 0, 0, srcWidth, srcWidth, flatCubemap, 0, 0, dstX, dstY);
            }
            if (width < srcWidth)
            {
                int newWidth = isColumn ? width : width * 6;
                int newHeight = isColumn ? width * 6 : width;
                var destRenderTexture = RenderTexture.GetTemporary(newWidth, newHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);

                Graphics.Blit(flatCubemap, destRenderTexture);

                var exportTexture = new Texture2D(newWidth, newHeight);
                exportTexture.ReadPixels(new Rect(0, 0, destRenderTexture.width, destRenderTexture.height), 0, 0);
                exportTexture.Apply();
                var resizedBytes = exportTexture.EncodeToPNG();

                RenderTexture.ReleaseTemporary(destRenderTexture);
                GameObject.DestroyImmediate(exportTexture);
                return resizedBytes;
            }
            var bytes = flatCubemap.EncodeToPNG();
            UnityEngine.Object.DestroyImmediate(flatCubemap);
            return bytes;
        }
        public static byte[] ConvertToFlat(Texture2D panorama, int width, bool isColumn = false)
        {
            Texture2D[] cubemap = new Texture2D[6];
            for (int i = 0; i < 6; ++i)
            {
                cubemap[i] = EquirectToCubemapTexture(panorama, width, (CubemapFace)i);
            }
            return ConvertToFlat(cubemap, width, isColumn);
        }

        private static Color CalcProjectionSpherical(Texture2D srcTexture, Vector3 vDir, float direction = 0f)
        {
            float theta = Mathf.Atan2(vDir.z, vDir.x);
            float phi = Mathf.Acos(vDir.y);

            theta += direction * Mathf.PI / 180.0f;
            while (theta < -Mathf.PI) theta += Mathf.PI + Mathf.PI;
            while (theta > Mathf.PI) theta -= Mathf.PI + Mathf.PI;

            float dx = theta / Mathf.PI;
            float dy = phi / Mathf.PI;

            dx = dx * 0.5f + 0.5f;
            int px = (int)(dx * (float)srcTexture.width);
            if (px < 0) px = 0;
            if (px >= srcTexture.width) px = srcTexture.width - 1;
            int py = (int)(dy * (float)srcTexture.height);
            if (py < 0) py = 0;
            if (py >= srcTexture.height) py = srcTexture.height - 1;

            Color col = srcTexture.GetPixel(px, srcTexture.height - py - 1);
            return col;
        }

        public static Texture2D EquirectToCubemapTexture(Texture2D panorama, int texSize, CubemapFace faceIndex, string fileName = null)
        {
            Texture2D tex = new Texture2D(texSize, texSize, TextureFormat.RGB24, false, true);

            Vector3[] vDirA = new Vector3[4];
            if (faceIndex == CubemapFace.PositiveZ)
            {
                vDirA[0] = new Vector3(-1.0f, -1.0f, -1.0f);
                vDirA[1] = new Vector3(1.0f, -1.0f, -1.0f);
                vDirA[2] = new Vector3(-1.0f, 1.0f, -1.0f);
                vDirA[3] = new Vector3(1.0f, 1.0f, -1.0f);
            }
            if (faceIndex == CubemapFace.NegativeZ)
            {
                vDirA[0] = new Vector3(1.0f, -1.0f, 1.0f);
                vDirA[1] = new Vector3(-1.0f, -1.0f, 1.0f);
                vDirA[2] = new Vector3(1.0f, 1.0f, 1.0f);
                vDirA[3] = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            if (faceIndex == CubemapFace.PositiveX)
            {
                vDirA[0] = new Vector3(1.0f, -1.0f, -1.0f);
                vDirA[1] = new Vector3(1.0f, -1.0f, 1.0f);
                vDirA[2] = new Vector3(1.0f, 1.0f, -1.0f);
                vDirA[3] = new Vector3(1.0f, 1.0f, 1.0f);
            }
            if (faceIndex == CubemapFace.NegativeX)
            {
                vDirA[0] = new Vector3(-1.0f, -1.0f, 1.0f);
                vDirA[1] = new Vector3(-1.0f, -1.0f, -1.0f);
                vDirA[2] = new Vector3(-1.0f, 1.0f, 1.0f);
                vDirA[3] = new Vector3(-1.0f, 1.0f, -1.0f);
            }
            if (faceIndex == CubemapFace.PositiveY)
            {
                vDirA[0] = new Vector3(-1.0f, 1.0f, -1.0f);
                vDirA[1] = new Vector3(1.0f, 1.0f, -1.0f);
                vDirA[2] = new Vector3(-1.0f, 1.0f, 1.0f);
                vDirA[3] = new Vector3(1.0f, 1.0f, 1.0f);
            }
            if (faceIndex == CubemapFace.NegativeY)
            {
                vDirA[0] = new Vector3(-1.0f, -1.0f, 1.0f);
                vDirA[1] = new Vector3(1.0f, -1.0f, 1.0f);
                vDirA[2] = new Vector3(-1.0f, -1.0f, -1.0f);
                vDirA[3] = new Vector3(1.0f, -1.0f, -1.0f);
            }

            Vector3 rotDX1 = (vDirA[1] - vDirA[0]) / (float)texSize;
            Vector3 rotDX2 = (vDirA[3] - vDirA[2]) / (float)texSize;

            float dy = 1.0f / (float)texSize;
            float fy = 0.0f;

            Color[] cols = new Color[texSize];
            for (int y = 0; y < texSize; y++)
            {
                Vector3 xv1 = vDirA[0];
                Vector3 xv2 = vDirA[2];
                for (int x = 0; x < texSize; x++)
                {
                    Vector3 v = ((xv2 - xv1) * fy) + xv1;
                    v.Normalize();
                    cols[x] = CalcProjectionSpherical(panorama, v);
                    xv1 += rotDX1;
                    xv2 += rotDX2;
                }
                tex.SetPixels(0, y, texSize, 1, cols);
                fy += dy;
            }
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.Apply();

            return tex;
        }

    }
}