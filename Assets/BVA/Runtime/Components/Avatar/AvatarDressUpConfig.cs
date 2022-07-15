using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BVA.Component
{
    [System.Serializable]
    public struct RendererMaterialConfig
    {
        public Renderer renderer;
        public Material[] materials;
        public override bool Equals(object obj)
        {
            var v = (RendererMaterialConfig)obj;
            if (materials.Length != v.materials.Length)
                return false;
            bool allSameMaterial = true;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] != v.materials[i])
                    allSameMaterial = false;
            }
            return renderer == v.renderer && allSameMaterial;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [System.Serializable]
    public struct DressUpConfig
    {
        public string name;
        public List<RendererMaterialConfig> rendererConfigs;

        public override bool Equals(object obj)
        {
            var v = (DressUpConfig)obj;
            if (rendererConfigs.Count != v.rendererConfigs.Count)
                return false;
            bool allSameConfig = true;
            for (int i = 0; i < rendererConfigs.Count; i++)
            {
                if (!rendererConfigs[i].Equals(v.rendererConfigs[i]))
                    allSameConfig = false;
            }
            return allSameConfig;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Save all active renderer and its corresponding materials,first config is default config
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AvatarDressUpConfig : MonoBehaviour
    {
        public List<DressUpConfig> dressUpConfigs;
        private Animator _animator;
        public Animator animator { get { if (_animator == null) _animator = GetComponent<Animator>(); return _animator; } }

        private int _currentDressIndex = 0;
        public int CurrentDressIndex
        {
            get
            {
                if (_currentDressIndex > DressCount - 1)
                {
                    if (DressCount != 0)
                    {
                        SwitchDress(0);
                        _currentDressIndex = 0;
                    }
                }
                return _currentDressIndex;
            }
        }

        public int DressCount => dressUpConfigs.Count;
        public void SwitchDress(bool next = true)
        {
            if (dressUpConfigs.Count < 2)
                return;
            int index = next ? _currentDressIndex + 1 : _currentDressIndex - 1;
            if (index >= DressCount)
            {
                index = 0;
            }
            if (index < 0)
            {
                index = DressCount - 1;
            }
            _currentDressIndex = index;
            SwitchDress(dressUpConfigs[_currentDressIndex]);

        }
        public void SwitchDress(int i = 0)
        {
            _currentDressIndex = i;
            SwitchDress(dressUpConfigs[i]);
        }
        private void deactiveAllRenderer()
        {
            Renderer[] renderers = animator.gameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.gameObject.SetActive(false);
            }
        }
        private void SwitchDress(DressUpConfig config)
        {
            deactiveAllRenderer();
            foreach (var cfg in config.rendererConfigs)
            {
                cfg.renderer.gameObject.SetActive(true);
                cfg.renderer.sharedMaterials = cfg.materials;
            }
        }
        public DressUpConfig BuildConfig(Transform avatarRoot)
        {
            DressUpConfig config = new DressUpConfig() { name = "Element" + dressUpConfigs.Count };
            config.rendererConfigs = new List<RendererMaterialConfig>();
            var renderers = avatarRoot.GetComponentsInChildren<Renderer>(false);

            foreach (var renderer in renderers)
            {
                var renderCfg = new RendererMaterialConfig() { renderer = renderer, materials = renderer.sharedMaterials };
                config.rendererConfigs.Add(renderCfg);
            }
            return config;
        }
    }
}