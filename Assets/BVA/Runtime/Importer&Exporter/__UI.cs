using System.Collections.Generic;
using GLTF.Schema.BVA;
using UnityEngine;
using GLTF.Schema;
using BVA.Extensions;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async Task<Sprite> LoadSprite(SpriteId id)
        {
            int spriteIndex = id.Id;
            Sprite sprite = _assetCache.SpriteCache[spriteIndex];
            if (sprite != null)
            {
                return sprite;
            }
            BVA_ui_spriteExtension ext = _gltfRoot.Extensions.Sprites[id.Id];
            Texture2D texture = await GetTexture(ext.texture) as Texture2D;
            sprite = Sprite.Create(texture, new Rect(ext.rect.x, ext.rect.y, ext.rect.z, ext.rect.w), ext.pivot, ext.pixelsPerUnit, 0, SpriteMeshType.FullRect, ext.border, ext.generateFallbackPhysicsShape);
            _assetCache.SpriteCache[spriteIndex] = sprite;
            return sprite;
        }
    }

    public partial class GLTFSceneExporter
    {
        public SpriteId ExportSprite(Sprite sprite)
        {
            TextureId textureId = ExportTexture(sprite.texture);
            BVA_ui_spriteExtension ext = new BVA_ui_spriteExtension(sprite, textureId);
            _root.AddExtension(_root, BVA_ui_spriteExtensionFactory.EXTENSION_NAME, null, RequireExtensions);
            var id = new SpriteId
            {
                Id = _root.Extensions.Sprites.Count,
                Root = _root
            };
            _root.Extensions.AddSprite(ext);
            return id;
        }
    }
}