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
        public const string EXTENSION_BVA = "bva";
        public const string EXTENSION_BVA_SCENE = "scene.bva";
        public const string EXTENSION_BVA_AVATAR = "avatar.bva";

    }
    public enum ExportFileType
    {
        BVA,
        GLB,
        GLTF
    }
}
