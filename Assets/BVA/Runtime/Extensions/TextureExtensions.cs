using System;
using UnityEngine;

namespace BVA.Extensions
{
	public enum CopyFilterMode
	{
		Source,
		Point,
		Bilinear,
		Trilinear
	}
	public static class TextureExtensions
    {
		public static bool IsRGB24(this Texture2D source)
		{
			return source.format ==  TextureFormat.RGB24 || source.format == TextureFormat.DXT1;
		}
		
		public static Texture2D ToRGBM(this Texture2D source, bool getSafePixels = false)
		{
			if (source == null)
			{
				return null;
			}
			Color[] array = source.GetPixels();
			Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.RGBAFloat, false);
			texture2D.name = source.name;
			int num = 0;
			for (int i = 0; i < source.height; i++)
			{
				for (int j = 0; j < source.width; j++)
				{
					float num2 = array[num].r * 0.125f;
					float num3 = array[num].g * 0.125f;
					float num4 = array[num].b * 0.125f;
					float num5 = Mathf.Max(Mathf.Max(num2, num3), Mathf.Max(num4, 1E-06f));
					num5 = Mathf.Ceil(num5 * 255f) / 255f;
					array[num].r = Mathf.Min(num2 / num5, 1f);
					array[num].g = Mathf.Min(num3 / num5, 1f);
					array[num].b = Mathf.Min(num4 / num5, 1f);
					array[num].a = Mathf.Min(num5, 1f);
					num++;
				}
			}
			if (array != null)
			{
				texture2D.SetPixels(array);
				texture2D.Apply();
			}
			return texture2D;
		}
		public static Texture2D ScaleTexture(this Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }
        public static void Clear(this Texture2D source, Color color)
        {
            if (source == null)
            {
                return;
            }
            int num = 0;
            Color[] array = new Color[source.width * source.height];
            for (int i = 0; i < source.height; i++)
            {
                for (int j = 0; j < source.width; j++)
                {
                    array[num] = color;
                    num++;
                }
            }
            source.SetPixels(array);
            source.Apply();
        }
		public static Texture2D RenderToTexture(this Texture2D source, TextureFormat? format = null, Material material = null, int rt_depth = 24, RenderTextureFormat rt_format =  RenderTextureFormat.ARGB32, RenderTextureReadWrite rt_readwrite =  RenderTextureReadWrite.Default)
		{
			Texture2D texture2D = null;
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, rt_depth, rt_format, rt_readwrite);
			if (temporary != null)
			{
				temporary.filterMode = FilterMode.Bilinear;
				if (material != null)
				{
					Graphics.Blit(source, temporary, material);
				}
				else
				{
					Graphics.Blit(source, temporary);
				}
				RenderTexture active = RenderTexture.active;
				RenderTexture.active = temporary;
				try
				{
					texture2D = new Texture2D(source.width, source.height, (format != null) ? format.Value : source.format, source.mipmapCount > 1);
					texture2D.name = source.name;
					texture2D.ReadPixels(new Rect(0f, 0f, (float)source.width, (float)source.height), 0, 0, false);
					texture2D.Apply();
					texture2D.filterMode =  FilterMode.Bilinear;
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				finally
				{
					RenderTexture.active = active;
					RenderTexture.ReleaseTemporary(temporary);
				}
			}
			return texture2D;
		}
		public static Texture2D RenderToTextureRaw(this Texture source, TextureFormat? format = null, Material material = null, int rt_depth = 24, RenderTextureFormat rt_format =  RenderTextureFormat.ARGB32, RenderTextureReadWrite rt_readwrite =  RenderTextureReadWrite.Default)
		{
			Texture2D texture2D = null;
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, rt_depth, rt_format, rt_readwrite);
			if (temporary != null)
			{
				temporary.filterMode = FilterMode.Bilinear;
				if (material != null)
				{
					Graphics.Blit(source, temporary, material);
				}
				else
				{
					Graphics.Blit(source, temporary);
				}
				RenderTexture active = RenderTexture.active;
				RenderTexture.active = temporary;
				try
				{
					texture2D = new Texture2D(source.width, source.height, (format != null) ? format.Value :  TextureFormat.RGBAFloat, false);
					texture2D.name = source.name;
					texture2D.ReadPixels(new Rect(0f, 0f, (float)source.width, (float)source.height), 0, 0, false);
					texture2D.Apply();
					texture2D.filterMode = FilterMode.Bilinear;
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				finally
				{
					RenderTexture.active = active;
					RenderTexture.ReleaseTemporary(temporary);
				}
			}
			return texture2D;
		}

		public static Texture2D RenderToTextureSize(this Texture2D source, int width, int height, TextureFormat? format = null, Material material = null, int rt_depth = 24, RenderTextureFormat rt_format = 0, RenderTextureReadWrite rt_readwrite = 0)
		{
			Texture2D texture2D = null;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, rt_depth, rt_format, rt_readwrite);
			if (temporary != null)
			{
				temporary.filterMode = FilterMode.Bilinear;
				if (material != null)
				{
					Graphics.Blit(source, temporary, material);
				}
				else
				{
					Graphics.Blit(source, temporary);
				}
				RenderTexture active = RenderTexture.active;
				RenderTexture.active = temporary;
				try
				{
					texture2D = new Texture2D(width, height, (format != null) ? format.Value : source.format, source.mipmapCount > 1);
					texture2D.name = source.name;
					texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0, false);
					texture2D.Apply();
					texture2D.filterMode = FilterMode.Bilinear;
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				finally
				{
					RenderTexture.active = active;
					RenderTexture.ReleaseTemporary(temporary);
				}
			}
			return texture2D;
		}
		public static Texture2D Copy(this Texture2D source, TextureFormat format =  TextureFormat.RGBA32, CopyFilterMode filter = CopyFilterMode.Source,  bool generateMipChain = false)
		{
			return source.Copy(0, format, filter, generateMipChain);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0002D2E4 File Offset: 0x0002B4E4
		public static Texture2D Copy(this Texture2D source, int miplevel, TextureFormat format = TextureFormat.RGBA32, CopyFilterMode filter = CopyFilterMode.Source, bool generateMipChain = false)
		{
			if (source == null)
			{
				return null;
			}
			Color[] array = source.GetPixels(miplevel);
			int num = (miplevel > 0) ? Math.Max(1, source.width >> miplevel) : source.width;
			int num2 = (miplevel > 0) ? Math.Max(1, source.height >> miplevel) : source.height;
			Texture2D texture2D = new Texture2D(num, num2, format, generateMipChain);
			texture2D.name = source.name;
			switch (filter)
			{
				case CopyFilterMode.Source:
					texture2D.filterMode = source.filterMode;
					break;
				case CopyFilterMode.Point:
					texture2D.filterMode =  FilterMode.Point;
					break;
				case CopyFilterMode.Bilinear:
					texture2D.filterMode = FilterMode.Bilinear;
					break;
				case CopyFilterMode.Trilinear:
					texture2D.filterMode = FilterMode.Trilinear;
					break;
			}
			if (array != null)
			{
				texture2D.SetPixels(array);
				texture2D.Apply();
			}
			return texture2D;
		}
		private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
		{
			return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
		}

		public static void Scale(this Texture2D source, int newWidth, int newHeight, bool bilinearScaling = true)
		{
			if (source == null)
			{
				return;
			}
			Color32[] pixels = source.GetPixels32();
			Color32[] array = new Color32[newWidth * newHeight];
			float num;
			float num2;
			if (bilinearScaling)
			{
				num = 1f / ((float)newWidth / (float)(source.width - 1));
				num2 = 1f / ((float)newHeight / (float)(source.height - 1));
			}
			else
			{
				num = (float)source.width / (float)newWidth;
				num2 = (float)source.height / (float)newHeight;
			}
			int width = source.width;
			if (bilinearScaling)
			{
				for (int i = 0; i < newHeight; i++)
				{
					int num3 = (int)Mathf.Floor((float)i * num2);
					int num4 = num3 * width;
					int num5 = (num3 + 1) * width;
					int num6 = i * newWidth;
					for (int j = 0; j < newWidth; j++)
					{
						int num7 = (int)Mathf.Floor((float)j * num);
						float value = (float)j * num - (float)num7;
						array[num6 + j] = ColorLerpUnclamped(ColorLerpUnclamped(pixels[num4 + num7], pixels[num4 + num7 + 1], value), ColorLerpUnclamped(pixels[num5 + num7], pixels[num5 + num7 + 1], value), (float)i * num2 - (float)num3);
					}
				}
			}
			else
			{
				for (int k = 0; k < newHeight; k++)
				{
					int num8 = (int)(num2 * (float)k) * width;
					int num9 = k * newWidth;
					for (int l = 0; l < newWidth; l++)
					{
						array[num9 + l] = pixels[(int)((float)num8 + num * (float)l)];
					}
				}
			}
#if UNITY_2021_1_OR_NEWER
			source.Reinitialize(newWidth, newHeight);
#else
			source.Resize(newWidth, newHeight);
#endif
			source.SetPixels32(array);
			source.Apply();
		}

		public static Texture2D Crop(this Texture2D source, Rect area, TextureFormat format = TextureFormat.RGBA32, CopyFilterMode filter = CopyFilterMode.Source)
		{
			if (source == null)
			{
				return null;
			}
			int num = (int)area.yMin;
			int num2 = (int)area.xMin;
			int num3 = (int)area.width;
			int num4 = (int)area.height;
			Texture2D texture2D = new Texture2D(num3, num4, format, false);
			texture2D.name = source.name;
			switch (filter)
			{
				case CopyFilterMode.Source:
					texture2D.filterMode = source.filterMode;
					break;
				case CopyFilterMode.Point:
					texture2D.filterMode = FilterMode.Point;
					break;
				case CopyFilterMode.Bilinear:
					texture2D.filterMode = FilterMode.Bilinear;
					break;
				case CopyFilterMode.Trilinear:
					texture2D.filterMode = FilterMode.Trilinear;
					break;
			}
			Color[] pixels = source.GetPixels(num2, num, num3, num4);
			if (pixels != null)
			{
				texture2D.SetPixels(pixels);
				texture2D.Apply();
			}
			return texture2D;
		}

		public static void NineCrop(this Texture2D source, int gutter, TextureFormat format =  TextureFormat.RGBA32)
		{
			if (source.width != source.height)
			{
				throw new Exception("Failed to nine crop image, source image width and height must be equal.");
			}
			int width = source.width;
			int num = source.width - 1;
			int num2 = source.width * 3;
			int num3 = source.width * 3;
			Color[] pixels = source.GetPixels(0, 0, width, width);
			Texture2D texture2D = new Texture2D(num2, num3, format, false);
			texture2D.SetPixels(0, 0, width, width, pixels);
			texture2D.SetPixels(0, num, width, width, pixels);
			texture2D.SetPixels(0, num * 2, width, width, pixels);
			texture2D.SetPixels(num, 0, width, width, pixels);
			texture2D.SetPixels(num, num, width, width, pixels);
			texture2D.SetPixels(num, num * 2, width, width, pixels);
			texture2D.SetPixels(num * 2, 0, width, width, pixels);
			texture2D.SetPixels(num * 2, num, width, width, pixels);
			texture2D.SetPixels(num * 2, num * 2, width, width, pixels);
			texture2D.Apply();
			int num4 = width * 2;
			int num5 = (num2 - num4) / 2;
			Color[] pixels2 = texture2D.GetPixels(num5, num5, num4, num4);
			Texture2D texture2D2 = new Texture2D(num4, num4, format, false);
			texture2D2.SetPixels(pixels2);
			texture2D2.Apply();
			if (gutter > 0)
			{
				int num6 = width - gutter * 2;
				int num7 = gutter - 1;
				texture2D2.Scale(num6, num6, true);
				Color[] pixels3 = texture2D2.GetPixels(0, 0, num6, num6);
				Texture2D texture2D3 = new Texture2D(width, width, format, false);
				texture2D3.Clear(Color.green);
				texture2D3.SetPixels(num7, num7, num6, num6, pixels3);
				texture2D3.Apply();
#if UNITY_2021_1_OR_NEWER
				source.Reinitialize(width, width);
#else
				source.Resize(width, width);
#endif
				source.SetPixels(texture2D3.GetPixels());
				source.Apply();
				return;
			}
			texture2D2.Scale(width, width, true);
#if UNITY_2021_1_OR_NEWER
				source.Reinitialize(width, width);
#else
			source.Resize(width, width);
#endif
			source.SetPixels(texture2D2.GetPixels());
			source.Apply();
		}

		public static Texture2D Blur(this Texture2D source, int blurSize, int iterations = 2, TextureFormat format =  TextureFormat.RGBA32)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			return FastBlur(source, format, blurSize, iterations, ref num, ref num2, ref num3, ref num4, ref num5);
		}

		private static Texture2D FastBlur(Texture2D image, TextureFormat format, int radius, int iterations, ref float avgR, ref float avgG, ref float avgB, ref float avgA, ref float blurPixelCount)
		{
			Texture2D texture2D = image;
			for (int i = 0; i < iterations; i++)
			{
				texture2D = BlurImage(texture2D, format, radius, true, ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
				texture2D = BlurImage(texture2D, format, radius, false, ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
			}
			return texture2D;
		}

		private static Texture2D BlurImage(Texture2D image, TextureFormat format, int blurSize, bool horizontal, ref float avgR, ref float avgG, ref float avgB, ref float avgA, ref float blurPixelCount)
		{
			Texture2D texture2D = new Texture2D(image.width, image.height, format, false);
			int width = image.width;
			int height = image.height;
			if (horizontal)
			{
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						ResetPixel(ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
						int num = j;
						while (num < j + blurSize && num < width)
						{
							AddPixel(image.GetPixel(num, i), ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
							num++;
						}
						num = j;
						while (num > j - blurSize && num > 0)
						{
							AddPixel(image.GetPixel(num, i), ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
							num--;
						}
						CalcPixel(ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
						num = j;
						while (num < j + blurSize && num < width)
						{
							texture2D.SetPixel(num, i, new Color(avgR, avgG, avgB, 1f));
							num++;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < width; j++)
				{
					for (int i = 0; i < height; i++)
					{
						ResetPixel(ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
						int num2 = i;
						while (num2 < i + blurSize && num2 < height)
						{
							AddPixel(image.GetPixel(j, num2), ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
							num2++;
						}
						num2 = i;
						while (num2 > i - blurSize && num2 > 0)
						{
							AddPixel(image.GetPixel(j, num2), ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
							num2--;
						}
						CalcPixel(ref avgR, ref avgG, ref avgB, ref avgA, ref blurPixelCount);
						num2 = i;
						while (num2 < i + blurSize && num2 < height)
						{
							texture2D.SetPixel(j, num2, new Color(avgR, avgG, avgB, 1f));
							num2++;
						}
					}
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		private static void AddPixel(Color pixel, ref float avgR, ref float avgG, ref float avgB, ref float avgA, ref float blurPixelCount)
		{
			avgR += pixel.r;
			avgG += pixel.g;
			avgB += pixel.b;
			blurPixelCount += 1f;
		}

		private static void ResetPixel(ref float avgR, ref float avgG, ref float avgB, ref float avgA, ref float blurPixelCount)
		{
			avgR = 0f;
			avgG = 0f;
			avgB = 0f;
			blurPixelCount = 0f;
		}

		private static void CalcPixel(ref float avgR, ref float avgG, ref float avgB, ref float avgA, ref float blurPixelCount)
		{
			avgR /= blurPixelCount;
			avgG /= blurPixelCount;
			avgB /= blurPixelCount;
		}
	}
}