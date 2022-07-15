using BVA.Component;
using UnityEngine;
using GLTF.Schema.BVA;
using GLTF.Schema;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async void ImportMeta(BVA_metaExtension ext, GameObject nodeObj)
        {
            var colObj = nodeObj.AddComponent<BVAMetaInfo>();
            colObj.metaInfo = ext.metaInfo;

            if (colObj.metaInfo.thumbnail == null)
            {
                TextureId textureId = colObj.metaInfo.thumbnailId;
                if (textureId != null)
                    colObj.metaInfo.thumbnail = await GetTexture(textureId) as Texture2D;
            }
        }
    }

    public partial class GLTFSceneExporter
    {
        private MetaId ExportMeta(BVAMetaInfo meta)
        {
            var metaInfo = meta.metaInfo;
            if (metaInfo.thumbnail != null)
            {
                var thumbnailId = ExportTexture(metaInfo.thumbnail);
                metaInfo.SetTextureId(thumbnailId);
            }
            BVA_metaExtension ext = new BVA_metaExtension(meta.metaInfo);
            var id = new MetaId
            {
                Id = _root.Extensions.Metas.Count,
                Root = _root
            };
            _root.Extensions.AddMeta(ext);
            return id;
        }
    }
}