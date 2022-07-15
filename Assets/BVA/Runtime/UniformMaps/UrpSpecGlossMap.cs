using System;
using UnityEngine;

namespace BVA
{
	public class UrpSpecGlossMap : UrpMap, ISpecGlossUniformMap
	{
		public UrpSpecGlossMap(bool clearcoat = false,int MaxLOD = 1000) : base(clearcoat? "Universal Render Pipeline/Complex Lit":"Universal Render Pipeline/Lit", MaxLOD) { }
		public UrpSpecGlossMap(string shaderName, int MaxLOD = 1000) : base(shaderName, MaxLOD) { }
		protected UrpSpecGlossMap(Material m, int MaxLOD = 1000) : base(m, MaxLOD) { }
		private Vector2 diffuseOffset = new Vector2(0, 0);
		private Vector2 specGlossOffset = new Vector2(0, 0);

		public Texture DiffuseTexture
		{
			get { return _material.GetTexture("_BaseMap"); }
			set { _material.SetTexture("_BaseMap", value); }
		}

		// not implemented by the Standard shader
		public int DiffuseTexCoord
		{
			get { return 0; }
			set { return; }
		}

		public Vector2 DiffuseXOffset
		{
			get { return diffuseOffset; }
			set
			{
				diffuseOffset = value;
				var unitySpaceVec = new Vector2(diffuseOffset.x, 1 - DiffuseXScale.y - diffuseOffset.y);
				_material.SetTextureOffset("_BaseMap", unitySpaceVec);
			}
		}

		public double DiffuseXRotation
		{
			get { return 0; }
			set { return; }
		}

		public Vector2 DiffuseXScale
		{
			get { return _material.GetTextureScale("_BaseMap"); }
			set
			{
				_material.SetTextureScale("_BaseMap", value);
				DiffuseXOffset = diffuseOffset;
			}
		}

		public int DiffuseXTexCoord
		{
			get { return 0; }
			set { return; }
		}

		public Color DiffuseFactor
		{
			get { return _material.GetColor("_BaseColor"); }
			set { _material.SetColor("_BaseColor", value); }
		}

		public Texture SpecularGlossinessTexture
		{
			get { return _material.GetTexture("_SpecGlossMap"); }
			set
			{
				_material.SetTexture("_SpecGlossMap", value);
				_material.SetFloat("_SmoothnessTextureChannel", 0);
				_material.EnableKeyword("_SPECGLOSSMAP");
			}
		}

		// not implemented by the Standard shader
		public int SpecularGlossinessTexCoord
		{
			get { return 0; }
			set { return; }
		}

		public Vector2 SpecularGlossinessXOffset
		{
			get { return specGlossOffset; }
			set
			{
				specGlossOffset = value;
				var unitySpaceVec = new Vector2(specGlossOffset.x, 1 - SpecularGlossinessXScale.y - specGlossOffset.y);
				_material.SetTextureOffset("_SpecGlossMap", unitySpaceVec);
			}
		}

		public double SpecularGlossinessXRotation
		{
			get { return 0; }
			set { return; }
		}

		public Vector2 SpecularGlossinessXScale
		{
			get { return _material.GetTextureScale("_SpecGlossMap"); }
			set
			{
				_material.SetTextureScale("_SpecGlossMap", value);
				SpecularGlossinessXOffset = specGlossOffset;
			}
		}

		public int SpecularGlossinessXTexCoord
		{
			get { return 0; }
			set { return; }
		}

		public Vector3 SpecularFactor
		{
			get { return _material.GetVector("_SpecColor"); }
			set { _material.SetVector("_SpecColor", value); }
		}

		public double GlossinessFactor
		{
			get { return _material.GetFloat("_GlossMapScale"); }
			set
			{
				_material.SetFloat("_GlossMapScale", (float)value);
				_material.SetFloat("_Smoothness", (float)value);
			}
		}

		public override IUniformMap Clone()
		{
			var copy = new UrpSpecGlossMap(new Material(_material));
			base.Copy(copy);
			return copy;
		}
	}
}
