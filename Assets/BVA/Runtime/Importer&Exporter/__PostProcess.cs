using System.Collections.Generic;
using UnityEngine;
using BVA.Extensions;
using GLTF.Schema.BVA;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async void ImportPostProcess(BVA_postprocess_volumeExtension asset, BVA_postprocess_volumeExtensionFactory ext, GameObject nodeObj)
        {
            var volume = nodeObj.AddComponent<UnityEngine.Rendering.Volume>();
            volume.priority = ext.priority;
            volume.isGlobal = ext.isGlobal;
            volume.blendDistance = ext.blendDistance;
            volume.weight = ext.weight;

            foreach (var v in asset.profile.postProcesses)
            {
                if (v is ColorLookup lookup)
                {
                    int texId = lookup.textureId;
                    lookup.texture = (Texture2D)await GetTextureAsLinear(new GLTF.Schema.TextureId() { Id = texId, Root = _gltfRoot });
                }
                if (v is FilmGrain film)
                {
                    int texId = film.textureId;
                    film.texture = (Texture2D)await GetTexture(new GLTF.Schema.TextureId() { Id = texId, Root = _gltfRoot });
                }
            }

            volume.sharedProfile = asset.profile.ToUnityVolumeProfile();
        }
    }

    public partial class GLTFSceneExporter
    {
        private PostProcessId ExportPostProcess(UnityEngine.Rendering.VolumeProfile profile)
        {
            int itemIndex = _urpVolumeProfiles.IndexOf(profile);
            if (itemIndex >= 0)
                return new PostProcessId() { Id = itemIndex, Root = _root };
            _urpVolumeProfiles.Add(profile);
            PostProcessAsset asset = new PostProcessAsset(profile);

            foreach (var v in asset.postProcesses)
            {
                if (v.type == PostProcessType.ColorLookup)
                {
                    var lookup = (v as ColorLookup);
                    var textureId = ExportTexture(lookup.texture);
                    lookup.SetLookupTextureId(textureId.Id);
                }
                if (v.type == PostProcessType.FilmGrain)
                {
                    var filmGrain = (v as FilmGrain);
                    var textureId = ExportTexture(filmGrain.texture);
                    filmGrain.SetTextureId(textureId.Id);
                }
            }
            var id = new PostProcessId
            {
                Id = _root.Extensions.PostProcessUrpVolumes.Count,
                Root = _root
            };

            _root.Extensions.AddPostProcess(new BVA_postprocess_volumeExtension(asset));
            _root.Extensions.AddExtension(_root, BVA_postprocess_volumeExtensionFactory.EXTENSION_NAME, null, RequireExtensions);
            return id;
        }
    }
}