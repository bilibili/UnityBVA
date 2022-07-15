using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using BVA.Component;

namespace BVA
{
    public class ExportInfo
    {
        private GameObject _root;
        public List<Mesh> meshes = new List<Mesh>();
        public List<Avatar> avatars = new List<Avatar>();
        public List<AnimationClip> animationClips = new List<AnimationClip>();
        public List<Material> materials = new List<Material>();
        public List<Texture> textures = new List<Texture>();
        public List<AudioClip> audioClips = new List<AudioClip>();

        public int nodeCount { private set; get; }
        public MeshInfo meshInfo { private set; get; }
        public AudioInfo audioInfo { private set; get; }
        public TextureInfo textureInfo { private set; get; }
        public struct AudioInfo
        {
            public int AudioClipCount;
            public int Size;
        }
        public struct MeshInfo
        {
            public int MeshCount;
            public int VertexCount;
        }
        public struct TextureInfo
        {
            public int TextureCount;
            public int Size;
        }

        public ExportInfo(GameObject root, bool refresh = true)
        {
            _root = root;
            if (refresh && _root != null)
            {
                RefreshInfo(_root.transform);
                CollectTextureInfo();
                CollectMeshInfo();
                CollectAudioInfo();
            }
        }

        public ExportInfo(GameObject[] transforms, bool refresh = true)
        {
            _root = null;
            if (refresh)
            {
                RefreshInfo(transforms);
                CollectTextureInfo();
                CollectMeshInfo();
                CollectAudioInfo();
            }
        }

        public void RefreshInfo(Transform transform)
        {
            CollectInfo(transform);
            if (transform.childCount == 0) return;
            for (int i = 0; i < transform.childCount; i++)
                RefreshInfo(transform.GetChild(i));
        }

        public void RefreshInfo(GameObject[] gameobjects)
        {
            foreach (var v in gameobjects)
            {
                RefreshInfo(v.transform);
            }
        }

        private void CollectInfo(Transform transform)
        {
            ++nodeCount;
            var mr = transform.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                var mf = transform.GetComponent<MeshFilter>();
                if (mf != null)
                {
                    meshes.Add(mf.sharedMesh);
                    materials.AddRange(mr.sharedMaterials);
                }
            }
            var smr = transform.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                meshes.Add(smr.sharedMesh);
                materials.AddRange(smr.sharedMaterials);
            }

            var playable = transform.GetComponent<PlayableController>();
            if (playable != null)
            {
                foreach (var v in playable.trackAsset.animationTrackGroup.tracks)
                {
                    animationClips.Add(v.source);
                }
                foreach (var v in playable.trackAsset.audioTrackGroup.tracks)
                {
                    audioClips.Add(v.source);
                }
                foreach (var v in playable.trackAsset.materialTextureTrackGroup.tracks)
                {
                    textures.Add(v.value);
                }
            }

            var audio = transform.GetComponent<AudioClipContainer>();
            if (audio != null)
                audioClips.AddRange(audio.audioClips);

            var anim = transform.GetComponent<Animator>();
            if (anim != null)
                avatars.Add(anim.avatar);
        }

        private void CollectTextureInfo()
        {
            foreach (var material in materials)
            {
                for (int j = 0; j < ShaderUtil.GetPropertyCount(material.shader); ++j)
                {
                    var propType = ShaderUtil.GetPropertyType(material.shader, j);
                    var name = ShaderUtil.GetPropertyName(material.shader, j);

                    switch (propType)
                    {
                        case ShaderUtil.ShaderPropertyType.TexEnv:
                            Texture texture = material.GetTexture(name);
                            if (texture != null) textures.Add(texture);
                            break;
                    }
                }
            }
        }

        private void CollectMeshInfo()
        {
            int vertexCount = 0;
            foreach (var mesh in meshes)
            {
                vertexCount += mesh.vertexCount;
            }
            meshInfo = new MeshInfo() { MeshCount = meshes.Count, VertexCount = vertexCount };
        }

        private void CollectAudioInfo()
        {
            int size = 0;
            foreach (var clip in audioClips)
            {
                size += clip.samples * clip.channels * sizeof(float);
            }
            audioInfo = new AudioInfo() { AudioClipCount = audioClips.Count, Size = size };
        }
    }
}