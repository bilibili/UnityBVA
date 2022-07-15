using System.Collections.Generic;
using GLTF.Schema.BVA;
using GLTF.Schema;
using BVA.Component;
using UnityEngine;
using System.Threading.Tasks;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async Task LoadVariableCollection(Node node, GameObject nodeObj)
        {
            if (hasValidExtension(node, BVA_variable_collectionExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_variable_collectionExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_variable_collectionExtensionFactory)ext;
                if (impl == null) throw new System.Exception($"cast {nameof(BVA_url_videoExtensionFactory)} failed");
                await ImportVariableCollection(_gltfRoot.Extensions.VariableCollections[impl.id.Id], node, nodeObj);
            }
        }
        public async Task ImportVariableCollection(BVA_variable_collectionExtension ext, Node node, GameObject nodeObj)
        {
            switch (ext.type)
            {
                case "texture":
                    {
                        var comp = nodeObj.AddComponent<TextureVariableCollection>();
                        comp.variables = new List<Texture2D>();
                        foreach (var v in ext.collections)
                            comp.variables.Add(await GetTexture(new TextureId() { Id = v, Root = _gltfRoot }) as Texture2D);
                        break;
                    }
                case "material":
                    {
                        var comp = nodeObj.AddComponent<MaterialVariableCollection>();
                        comp.variables = new List<Material>();
                        foreach (var v in ext.collections)
                            comp.variables.Add(await LoadMaterial(new MaterialId() { Id = v, Root = _gltfRoot }));
                        break;
                    }
                case "audio":
                    {
                        var comp = nodeObj.AddComponent<AudioClipVariableCollection>();
                        comp.variables = new List<AudioClip>();
                        foreach (var v in ext.collections)
                            comp.variables.Add(AssetManager.audioClipContainer.Get(v));
                        break;
                    }
            }
        }
    }
    public partial class GLTFSceneExporter
    {
        private VaribleCollectionId ExportVariableCollection(BVA_variable_collectionExtension ext)
        {
            var id = new VaribleCollectionId
            {
                Id = _root.Extensions.VariableCollections.Count,
                Root = _root
            };
            _root.Extensions.AddVariableCollection(ext);
            return id;
        }

        private VaribleCollectionId ExportVariableCollection(MaterialVariableCollection materialCollection)
        {
            List<MaterialId> materialIds = new List<MaterialId>();
            foreach (var material in materialCollection.variables)
            {
                materialIds.Add(ExportMaterial(material));
            }
            var ext = new BVA_variable_collectionExtension(materialIds);
            return ExportVariableCollection(ext);
        }

        private VaribleCollectionId ExportVariableCollection(TextureVariableCollection textureCollection)
        {
            List<TextureId> textureIds = new List<TextureId>();
            foreach (var texture in textureCollection.variables)
            {
                textureIds.Add(ExportTextureInfo(texture).Index);
            }
            var ext = new BVA_variable_collectionExtension(textureIds);
            return ExportVariableCollection(ext);
        }

        private VaribleCollectionId ExportVariableCollection(CubemapVariableCollection textureCollection)
        {
            List<CubemapId> cubemapIds = new List<CubemapId>();
            foreach (var texture in textureCollection.variables)
            {
                cubemapIds.Add(ExportCubemap(texture));
            }
            var ext = new BVA_variable_collectionExtension(cubemapIds);
            return ExportVariableCollection(ext);
        }

        private VaribleCollectionId ExportVariableCollection(AudioClipVariableCollection audioCollection)
        {
            List<AudioId> audioIds = new List<AudioId>();
            foreach (var audio in audioCollection.variables)
            {
                AudioId id = ExportAudioInternalBuffer(audio);
                if (id != null) audioIds.Add(id);
            }
            var ext = new BVA_variable_collectionExtension(audioIds);
            return ExportVariableCollection(ext);
        }
    }
}