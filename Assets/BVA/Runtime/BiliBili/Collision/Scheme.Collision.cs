using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public enum CollisionType
    {
        Box,
        Sphere,
        Capsule,
        Mesh
    }
    public enum Direction : byte
    {
        x = 0, y, z
    }
}