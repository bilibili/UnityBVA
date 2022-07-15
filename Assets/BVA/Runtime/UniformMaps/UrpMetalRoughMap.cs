using UnityEngine;

namespace BVA
{
    public class UrpMetalRoughMap : UrpMap, IMetalRoughUniformMap
    {
        private Vector2 baseColorOffset = new Vector2(0, 0);

        public UrpMetalRoughMap(bool clearcoat = false, int MaxLOD = 1000) : base(clearcoat ? "Universal Render Pipeline/Complex Lit" : "Universal Render Pipeline/Lit", MaxLOD) { }
        protected UrpMetalRoughMap(string shaderName, int MaxLOD = 1000) : base(shaderName, MaxLOD) { }
        protected UrpMetalRoughMap(Material m, int MaxLOD = 1000) : base(m, MaxLOD) { }
        public Texture BaseColorTexture
        {
            get { return _material.GetTexture("_BaseMap"); }
            set { _material.SetTexture("_BaseMap", value); }
        }

        // not implemented by the Standard shader
        public int BaseColorTexCoord
        {
            get { return 0; }
            set { return; }
        }

        public Vector2 BaseColorXOffset
        {
            get { return baseColorOffset; }
            set
            {
                baseColorOffset = value;
                var unitySpaceVec = new Vector2(baseColorOffset.x, 1 - BaseColorXScale.y - baseColorOffset.y);
                _material.SetTextureOffset("_BaseMap", unitySpaceVec);
            }
        }

        public double BaseColorXRotation
        {
            get { return 0; }
            set { return; }
        }

        public Vector2 BaseColorXScale
        {
            get { return _material.GetTextureScale("_BaseMap"); }
            set
            {
                _material.SetTextureScale("_BaseMap", value);
                BaseColorXOffset = baseColorOffset;
            }
        }

        public int BaseColorXTexCoord
        {
            get { return 0; }
            set { return; }
        }

        public Color BaseColorFactor
        {
            get { return _material.GetColor("_BaseColor"); }
            set { _material.SetColor("_BaseColor", value); }
        }

        public Texture MetallicRoughnessTexture
        {
            get { return _material.GetTexture("_MetallicGlossMap"); }
            set
            {
                _material.SetTexture("_MetallicGlossMap", value);
                _material.EnableKeyword("_METALLICSPECGLOSSMAP");
            }
        }

        // not implemented by the Standard shader
        public int MetallicRoughnessTexCoord
        {
            get { return 0; }
            set { return; }
        }

        public Vector2 MetallicRoughnessXOffset
        {
            get { return new Vector2(0, 0); }
            set { return; }
        }

        public double MetallicRoughnessXRotation
        {
            get { return 0; }
            set { return; }
        }

        public Vector2 MetallicRoughnessXScale
        {
            get { return new Vector2(1, 1); }
            set { return; }
        }

        public int MetallicRoughnessXTexCoord
        {
            get { return 0; }
            set { return; }
        }

        public double MetallicFactor
        {
            get { return _material.GetFloat("_Metallic"); }
            set { _material.SetFloat("_Metallic", (float)value); }
        }

        // not supported by the Standard shader
        public double RoughnessFactor
        {
            get { return 1.0f - _material.GetFloat("_Smoothness"); }
            set { _material.SetFloat("_Smoothness", 1.0f - (float)value); }
        }

        public override IUniformMap Clone()
        {
            var copy = new UrpMetalRoughMap(new Material(_material));
            base.Copy(copy);
            return copy;
        }
    }
}