using Newtonsoft.Json.Linq;
using UnityEngine;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;

namespace GLTF.Schema.BVA
{
    [AsyncComponentExtra]
public class BVA_Light_ReflectionProbe_Extra :  IAsyncComponentExtra
{
public Vector3 size;
public Vector3 center;
public float nearClipPlane;
public float farClipPlane;
public float intensity;
public bool hdr;
public bool renderDynamicObjects;
public float shadowDistance;
public int resolution;
public int cullingMask;
public UnityEngine.Rendering.ReflectionProbeClearFlags clearFlags;
public Color backgroundColor;
public float blendDistance;
public bool boxProjection;
public UnityEngine.Rendering.ReflectionProbeMode mode;
public int importance;
public UnityEngine.Rendering.ReflectionProbeRefreshMode refreshMode;
public UnityEngine.Rendering.ReflectionProbeTimeSlicingMode timeSlicingMode;
public CubemapId bakedTexture;
public CubemapId customBakedTexture;
public string ComponentName => ComponentType.Name;
public string ExtraName => GetType().Name;
public System.Type ComponentType => typeof(ReflectionProbe);
public void SetData(Component component, ExportTexture exportTexture, ExportMaterial exportMaterial, ExportSprite exportSprite, ExportCubemap exportCubemap)
{
var target = component as ReflectionProbe;
this.size = target.size;
this.center = target.center;
this.nearClipPlane = target.nearClipPlane;
this.farClipPlane = target.farClipPlane;
this.intensity = target.intensity;
this.hdr = target.hdr;
this.renderDynamicObjects = target.renderDynamicObjects;
this.shadowDistance = target.shadowDistance;
this.resolution = target.resolution;
this.cullingMask = target.cullingMask;
this.clearFlags = target.clearFlags;
this.backgroundColor = target.backgroundColor;
this.blendDistance = target.blendDistance;
this.boxProjection = target.boxProjection;
this.mode = target.mode;
this.importance = target.importance;
this.refreshMode = target.refreshMode;
this.timeSlicingMode = target.timeSlicingMode;
			if(target.bakedTexture!=null)
this.bakedTexture =exportCubemap(target.bakedTexture as Cubemap);
            if (target.customBakedTexture != null)
                this.customBakedTexture = exportCubemap(target.customBakedTexture as Cubemap);
        }
public async Task Deserialize(GLTFRoot root, JsonReader reader, Component component, AsyncLoadTexture loadTexture,AsyncLoadMaterial loadMaterial, AsyncLoadSprite loadSprite,AsyncLoadCubemap loadCubemap)
{
var target = component as ReflectionProbe;
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(target.size):
target.size =  reader.ReadAsVector3();
break;
case nameof(target.center):
target.center =  reader.ReadAsVector3();
break;
case nameof(target.nearClipPlane):
target.nearClipPlane =  reader.ReadAsFloat();
break;
case nameof(target.farClipPlane):
target.farClipPlane =  reader.ReadAsFloat();
break;
case nameof(target.intensity):
target.intensity =  reader.ReadAsFloat();
break;
case nameof(target.hdr):
target.hdr =  reader.ReadAsBoolean().Value;
break;
case nameof(target.renderDynamicObjects):
target.renderDynamicObjects =  reader.ReadAsBoolean().Value;
break;
case nameof(target.shadowDistance):
target.shadowDistance =  reader.ReadAsFloat();
break;
case nameof(target.resolution):
target.resolution =  reader.ReadAsInt32().Value;
break;
case nameof(target.cullingMask):
target.cullingMask =  reader.ReadAsInt32().Value;
break;
case nameof(target.clearFlags):
target.clearFlags = reader.ReadStringEnum<UnityEngine.Rendering.ReflectionProbeClearFlags>();
break;
case nameof(target.backgroundColor):
target.backgroundColor =  reader.ReadAsRGBAColor();
break;
case nameof(target.blendDistance):
target.blendDistance =  reader.ReadAsFloat();
break;
case nameof(target.boxProjection):
target.boxProjection =  reader.ReadAsBoolean().Value;
break;
case nameof(target.mode):
target.mode = reader.ReadStringEnum<UnityEngine.Rendering.ReflectionProbeMode>();
break;
case nameof(target.importance):
target.importance =  reader.ReadAsInt32().Value;
break;
case nameof(target.refreshMode):
target.refreshMode = reader.ReadStringEnum<UnityEngine.Rendering.ReflectionProbeRefreshMode>();
break;
case nameof(target.timeSlicingMode):
target.timeSlicingMode = reader.ReadStringEnum<UnityEngine.Rendering.ReflectionProbeTimeSlicingMode>();
break;
case nameof(target.bakedTexture):
int bakedTextureIndex = reader.ReadAsInt32().Value;
	target.bakedTexture = await loadCubemap(new CubemapId() { Id = bakedTextureIndex, Root = root });
break;
case nameof(target.customBakedTexture):
int customBakedTextureIndex = reader.ReadAsInt32().Value;
	target.customBakedTexture = await loadCubemap(new CubemapId() { Id = customBakedTextureIndex, Root = root });
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(size), size.ToJArray());
jo.Add(nameof(center), center.ToJArray());
jo.Add(nameof(nearClipPlane), nearClipPlane);
jo.Add(nameof(farClipPlane), farClipPlane);
jo.Add(nameof(intensity), intensity);
jo.Add(nameof(hdr), hdr);
jo.Add(nameof(renderDynamicObjects), renderDynamicObjects);
jo.Add(nameof(shadowDistance), shadowDistance);
jo.Add(nameof(resolution), resolution);
jo.Add(nameof(cullingMask), cullingMask);
jo.Add(nameof(clearFlags), clearFlags.ToString());
jo.Add(nameof(backgroundColor), backgroundColor.ToJArray());
jo.Add(nameof(blendDistance), blendDistance);
jo.Add(nameof(boxProjection), boxProjection);
jo.Add(nameof(mode), mode.ToString());
jo.Add(nameof(importance), importance);
jo.Add(nameof(refreshMode), refreshMode.ToString());
jo.Add(nameof(timeSlicingMode), timeSlicingMode.ToString());
if(bakedTexture!=null)jo.Add(nameof(bakedTexture), bakedTexture.Id);
if(customBakedTexture!=null)jo.Add(nameof(customBakedTexture), customBakedTexture.Id);
return new JProperty(ComponentName, jo);
}

        public object Clone()
        {
            return new BVA_Light_ReflectionProbe_Extra();
        }
    }
}
