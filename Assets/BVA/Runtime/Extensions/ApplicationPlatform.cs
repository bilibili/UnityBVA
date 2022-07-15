using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace BVA.Extensions
{
    public sealed class ApplicationPlatform
    {
        public enum UnityRenderPipeline
        {
            Buildin,
            URP,
            HDRP
        }
        public static UnityRenderPipeline RenderPipeline
        {
            get
            {
                if (GraphicsSettings.renderPipelineAsset == null) return UnityRenderPipeline.Buildin;
                else if (GraphicsSettings.renderPipelineAsset.GetType().Name.Contains("HDRenderPipelineAsset"))return UnityRenderPipeline.HDRP;
                else return UnityRenderPipeline.URP;
            }
        }
    }
}