using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BVA
{
    public static class BVAConst
    {
        public const string FORMAT_VERSION = "0.1.0";
        public const string LAYER_SCENE = "BVA Scene";
        public const string LAYER_AVATAR = "BVA Avatar";
        public const int LAYER_SCENE_INT = 1 << 7;
        public const int LAYER_AVATAR_INT = 1 << 8;
        public const string EXTENSION_GLB = "glb";
        public const string EXTENSION_GLTF = "gltf";
        public const string EXTENSION_BVA_SCENE = "BVA.scene";
        public const string EXTENSION_BVA_AVATAR = "BVA.avatar";
        public const string EXTENSION_BVA_SCENE_GLB = EXTENSION_BVA_SCENE + ".glb";
        public const string EXTENSION_BVA_AVATAR_GLB = EXTENSION_BVA_AVATAR + ".glb";
        public const string EXTENSION_BVA_SCENE_GLTF = EXTENSION_BVA_SCENE + ".gltf";
        public const string EXTENSION_BVA_AVATAR_GLTF = EXTENSION_BVA_AVATAR + ".gltf";

    }
}
