using UnityEngine;
using BVA.Extensions;
using GLTF.Schema.BVA;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public void ImportCollision(BVA_collisions_colliderExtension ext, GameObject nodeObj)
        {
            if (ext.type == CollisionType.Box)
            {
                var colObj = nodeObj.AddComponent<BoxCollider>();
                colObj.isTrigger = ext.isTrigger;
                colObj.center = ext.center;
                colObj.size = ext.size;
            }
            else if (ext.type == CollisionType.Sphere)
            {
                var colObj = nodeObj.AddComponent<SphereCollider>();
                colObj.isTrigger = ext.isTrigger;
                colObj.center = ext.center;
                colObj.radius = ext.radius;
            }
            else if (ext.type == CollisionType.Capsule)
            {
                var colObj = nodeObj.AddComponent<CapsuleCollider>();
                colObj.isTrigger = ext.isTrigger;
                colObj.center = ext.center;
                colObj.radius = ext.radius;
                colObj.height = ext.height;
                colObj.direction = (int)ext.direction;
            }
            else if (ext.type == CollisionType.Mesh)
            {
                var filter = nodeObj.GetComponent<MeshFilter>();
                if(filter != null)
                {
                    var mesh = filter.sharedMesh;
                    if (mesh != null)
                    {
                        var colObj = nodeObj.AddComponent<MeshCollider>();
                        colObj.sharedMesh = mesh;
                    }
                }


            }
        }
    }

    public partial class GLTFSceneExporter
    {
        private bool ShouldExportCollision(Collider collision)
        {
            if (collision == null)
                return false;
            if (collision.GetType() == typeof(BoxCollider) || collision.GetType() == typeof(SphereCollider) || collision.GetType() == typeof(CapsuleCollider))
                return true;
            else if (collision.GetType() == typeof(MeshCollider))
            {
                var meshFilter = collision.gameObject.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh == (collision as MeshCollider).sharedMesh)
                    return true;
            }
            return false;
        }
        private CollisionId ExportCollision(Collider collider)
        {
            BVA_collisions_colliderExtension ext;
            if (collider is BoxCollider box)
            {
                ext = new BVA_collisions_colliderExtension(CollisionType.Box, box.isTrigger, box.center, 0, 0, Direction.x);
            }
            else if (collider is SphereCollider sphere)
            {
                ext = new BVA_collisions_colliderExtension(CollisionType.Sphere, sphere.isTrigger, sphere.center, sphere.radius, 0, Direction.x);
            }
            else if (collider is CapsuleCollider capsule)
            {
                ext = new BVA_collisions_colliderExtension(CollisionType.Capsule, capsule.isTrigger, capsule.center, capsule.radius, capsule.height, (Direction)capsule.direction);
            }
            else if (collider is MeshCollider mesh)
            {
                ext = new BVA_collisions_colliderExtension(CollisionType.Mesh, mesh.isTrigger, mesh.convex);
            }
            else
            {
                LogPool.ExportLogger.LogError(LogPart.Collision, $"{collider.gameObject.name} is not exported!");
                return null;
            }

            var id = new CollisionId
            {
                Id = _root.Extensions.Collisions.Count,
                Root = _root
            };
            _root.Extensions.AddCollision(ext);
            return id;
        }
    }
}