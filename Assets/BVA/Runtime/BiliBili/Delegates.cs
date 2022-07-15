using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public delegate Task<Material> AsyncLoadMaterial(MaterialId id);
    public delegate Task<Texture> AsyncLoadTexture(TextureId id);
    public delegate Task<Cubemap> AsyncLoadCubemap(CubemapId id);
    public delegate Task<Sprite> AsyncLoadSprite(SpriteId id);
    public delegate TextureInfo ExportTextureInfo(Texture texture);
    public delegate CubemapId ExportCubemapInfo(Cubemap cubemap);
    public delegate Task AsyncDeserializeCustomMaterial(GLTFRoot root, JsonReader reader, Material matCache, AsyncLoadTexture loadTexture,AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap);
    public delegate Task<AnimationClip> ConstructClipFunc(bool isAnimatorClip, Transform root, int animationId, CancellationToken cancellationToken);

}