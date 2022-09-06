using UnityEngine;
using BVA.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using GLTF.Extensions;

namespace GLTF.Schema.BVA
{
    public enum PostProcessType
    {
        Bloom,
        ChannelMixer,
        ChromaticAberration,
        ColorAdjustments,
        ColorCurves,
        ColorLookup,
        DepthOfField,
        FilmGrain,
        LensDistortion,
        LiftGammaGain,
        MotionBlur,
        PaniniProjection,
        ShadowsMidtonesHighlights,
        SplitToning,
        Tonemapping,
        Vignette,
        WhiteBalance,
    }
    #region Base

    /// <summary>
    /// TextureCurve serilization is not supported yet
    /// </summary>
    public class PostProcessAsset : GLTFProperty
    {
        public List<PostProcessBase> postProcesses;
        public PostProcessAsset() { postProcesses = new List<PostProcessBase>(); }
        public PostProcessAsset(UnityEngine.Rendering.VolumeProfile volumeProfile)
        {
            postProcesses = new List<PostProcessBase>();
            foreach (var v in volumeProfile.components)
            {
                if (v is UnityEngine.Rendering.Universal.Bloom)
                {
                    postProcesses.Add(Bloom.From(v as UnityEngine.Rendering.Universal.Bloom));
                }
                if (v is UnityEngine.Rendering.Universal.ChannelMixer)
                {
                    postProcesses.Add(ChannelMixer.From(v as UnityEngine.Rendering.Universal.ChannelMixer));
                }
                if (v is UnityEngine.Rendering.Universal.ChromaticAberration)
                {
                    postProcesses.Add(ChromaticAberration.From(v as UnityEngine.Rendering.Universal.ChromaticAberration));
                }
                if (v is UnityEngine.Rendering.Universal.ColorAdjustments)
                {
                    postProcesses.Add(ColorAdjustments.From(v as UnityEngine.Rendering.Universal.ColorAdjustments));
                }
                if (v is UnityEngine.Rendering.Universal.ColorCurves)
                {
                    postProcesses.Add(ColorCurves.From(v as UnityEngine.Rendering.Universal.ColorCurves));
                }
                if (v is UnityEngine.Rendering.Universal.ColorLookup)
                {
                    postProcesses.Add(ColorLookup.From(v as UnityEngine.Rendering.Universal.ColorLookup));
                }
                if (v is UnityEngine.Rendering.Universal.DepthOfField)
                {
                    postProcesses.Add(DepthOfField.From(v as UnityEngine.Rendering.Universal.DepthOfField));
                }
                if (v is UnityEngine.Rendering.Universal.FilmGrain)
                {
                    postProcesses.Add(FilmGrain.From(v as UnityEngine.Rendering.Universal.FilmGrain));
                }
                if (v is UnityEngine.Rendering.Universal.LensDistortion)
                {
                    postProcesses.Add(LensDistortion.From(v as UnityEngine.Rendering.Universal.LensDistortion));
                }
                if (v is UnityEngine.Rendering.Universal.LiftGammaGain)
                {
                    postProcesses.Add(LiftGammaGain.From(v as UnityEngine.Rendering.Universal.LiftGammaGain));
                }
                if (v is UnityEngine.Rendering.Universal.MotionBlur)
                {
                    postProcesses.Add(MotionBlur.From(v as UnityEngine.Rendering.Universal.MotionBlur));
                }
                if (v is UnityEngine.Rendering.Universal.PaniniProjection)
                {
                    postProcesses.Add(PaniniProjection.From(v as UnityEngine.Rendering.Universal.PaniniProjection));
                }
                if (v is UnityEngine.Rendering.Universal.ShadowsMidtonesHighlights)
                {
                    postProcesses.Add(ShadowsMidtonesHighlights.From(v as UnityEngine.Rendering.Universal.ShadowsMidtonesHighlights));
                }
                if (v is UnityEngine.Rendering.Universal.SplitToning)
                {
                    postProcesses.Add(SplitToning.From(v as UnityEngine.Rendering.Universal.SplitToning));
                }
                if (v is UnityEngine.Rendering.Universal.Tonemapping)
                {
                    postProcesses.Add(Tonemapping.From(v as UnityEngine.Rendering.Universal.Tonemapping));
                }
                if (v is UnityEngine.Rendering.Universal.Vignette)
                {
                    postProcesses.Add(Vignette.From(v as UnityEngine.Rendering.Universal.Vignette));
                }
                if (v is UnityEngine.Rendering.Universal.WhiteBalance)
                {
                    postProcesses.Add(WhiteBalance.From(v as UnityEngine.Rendering.Universal.WhiteBalance));
                }
            }
        }
        public UnityEngine.Rendering.VolumeProfile ToUnityVolumeProfile()
        {
            var profile = ScriptableObject.CreateInstance<UnityEngine.Rendering.VolumeProfile>();
            foreach (var v in postProcesses)
            {
                switch (v.type)
                {
                    case PostProcessType.Bloom:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.Bloom));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.ChannelMixer:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.ChannelMixer));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.ChromaticAberration:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.ChromaticAberration));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.ColorAdjustments:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.ColorAdjustments));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.DepthOfField:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.DepthOfField));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.FilmGrain:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.FilmGrain));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.LensDistortion:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.LensDistortion));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.LiftGammaGain:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.LiftGammaGain));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.MotionBlur:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.MotionBlur));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.PaniniProjection:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.PaniniProjection));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.ShadowsMidtonesHighlights:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.ShadowsMidtonesHighlights));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.SplitToning:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.SplitToning));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.Tonemapping:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.Tonemapping));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.Vignette:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.Vignette));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.WhiteBalance:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.WhiteBalance));
                            v.SetData(component);
                        }
                        break;
                    case PostProcessType.ColorCurves:
                        {
                            //var component = profile.Add(typeof(UnityEngine.Rendering.Universal.ColorCurves));
                            //v.SetData(component);
                        }
                        break;
                    case PostProcessType.ColorLookup:
                        {
                            var component = profile.Add(typeof(UnityEngine.Rendering.Universal.ColorLookup));
                            v.SetData(component);
                        }
                        break;
                }

            }
            return profile;
        }
        public JObject Serialize()
        {
            JArray ja = new JArray();
            foreach (var v in postProcesses)
            {
                ja.Add(v.Serialize());
            }
            JObject jo = new JObject();
            jo.Add(nameof(postProcesses), ja);
            return jo;
        }
        private static PostProcessBase DeserializePostProcess(GLTFRoot root, JsonReader reader)
        {
            PostProcessBase ret = null;
            JObject jo = JObject.ReadFrom(reader) as JObject;
            var type = jo.GetValue(nameof(ret.type)).DeserializeAsEnum<PostProcessType>();
            switch (type)
            {
                case PostProcessType.Bloom:
                    ret = Bloom.Deserialize(root, jo);
                    break;
                case PostProcessType.ChannelMixer:
                    ret = ChannelMixer.Deserialize(root, jo);
                    break;
                case PostProcessType.ChromaticAberration:
                    ret = ChromaticAberration.Deserialize(root, jo);
                    break;
                case PostProcessType.ColorAdjustments:
                    ret = ColorAdjustments.Deserialize(root, jo);
                    break;
                case PostProcessType.ColorCurves:
                    ret = ColorCurves.Deserialize(root, jo);
                    break;
                case PostProcessType.ColorLookup:
                    ret = ColorLookup.Deserialize(root, jo);
                    break;
                case PostProcessType.DepthOfField:
                    ret = DepthOfField.Deserialize(root, jo);
                    break;
                case PostProcessType.FilmGrain:
                    ret = FilmGrain.Deserialize(root, jo);
                    break;
                case PostProcessType.LensDistortion:
                    ret = LensDistortion.Deserialize(root, jo);
                    break;
                case PostProcessType.LiftGammaGain:
                    ret = LiftGammaGain.Deserialize(root, jo);
                    break;
                case PostProcessType.MotionBlur:
                    ret = MotionBlur.Deserialize(root, jo);
                    break;
                case PostProcessType.PaniniProjection:
                    ret = PaniniProjection.Deserialize(root, jo);
                    break;
                case PostProcessType.ShadowsMidtonesHighlights:
                    ret = ShadowsMidtonesHighlights.Deserialize(root, jo);
                    break;
                case PostProcessType.SplitToning:
                    ret = SplitToning.Deserialize(root, jo);
                    break;
                case PostProcessType.Tonemapping:
                    ret = Tonemapping.Deserialize(root, jo);
                    break;
                case PostProcessType.Vignette:
                    ret = Vignette.Deserialize(root, jo);
                    break;
                case PostProcessType.WhiteBalance:
                    ret = WhiteBalance.Deserialize(root, jo);
                    break;
            }
            if (ret == null) Debug.LogError($"DeserializePostProcess Fail,the reader is {reader.Value}");
            return ret;
        }
        public static PostProcessAsset Deserialize(GLTFRoot root, JsonReader reader)
        {
            PostProcessAsset asset = new PostProcessAsset();
            asset.postProcesses = reader.ReadList(() => DeserializePostProcess(root, reader));
            return asset;
        }
    }
    public class PostProcessBase
    {
        public PostProcessType type;
        public bool active;

        public virtual JObject Serialize()
        {
            JObject pro = new JObject();
            pro.Add(new JProperty(nameof(type), type.ToString()));
            pro.Add(new JProperty(nameof(active), active));
            return pro;
        }
        public virtual void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            component.SetAllOverridesTo(true);
        }
    }
    #endregion

    #region Bloon
    public class Bloom : PostProcessBase
    {
        UnityEngine.Rendering.Universal.Bloom value;
        public static Bloom From(UnityEngine.Rendering.Universal.Bloom bloom)
        {
            return new Bloom()
            {
                value = bloom,
                type = PostProcessType.Bloom,
                active = bloom.active,
                intensity = bloom.intensity.value,
                threshold = bloom.threshold.value,
                scatter = bloom.scatter.value,
                clamp = bloom.clamp.value,
                tint = bloom.tint.value,
                highQualityFiltering = bloom.highQualityFiltering.value,
                skipIterations = bloom.skipIterations.value
            };
        }
        public static Bloom Deserialize(GLTFRoot root, JObject jo)
        {
            Bloom bloom = new Bloom();

            JToken jt = jo.GetValue(nameof(bloom.active));
            if (jt != null) bloom.active = jt.DeserializeAsBool();

            jt = jo.GetValue(nameof(bloom.intensity));
            if (jt != null) bloom.intensity = jt.DeserializeAsFloat();

            jt = jo.GetValue(nameof(bloom.threshold));
            if (jt != null) bloom.threshold = jt.DeserializeAsFloat();

            jt = jo.GetValue(nameof(bloom.scatter));
            if (jt != null) bloom.scatter = jt.DeserializeAsFloat();

            jt = jo.GetValue(nameof(bloom.clamp));
            if (jt != null) bloom.clamp = jt.DeserializeAsFloat();

            jt = jo.GetValue(nameof(bloom.tint));
            if (jt != null) bloom.tint = jt.DeserializeAsColor();

            jt = jo.GetValue(nameof(bloom.highQualityFiltering));
            if (jt != null) bloom.highQualityFiltering = jt.DeserializeAsBool();

            jt = jo.GetValue(nameof(bloom.skipIterations));
            if (jt != null) bloom.skipIterations = jt.DeserializeAsInt();

            return bloom;
        }
        public Bloom()
        {
            type = PostProcessType.Bloom;
            active = true;
            intensity = 0f;
            threshold = 0.9f;
            scatter = 0.7f;
            clamp = 65472f;
            tint = Color.white;
            highQualityFiltering = false;
            skipIterations = 1;
            dirtIntensity = 0f;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.intensity.overrideState) pro.Add(new JProperty(nameof(intensity), intensity));
            if (value.threshold.overrideState) pro.Add(new JProperty(nameof(threshold), threshold));
            if (value.scatter.overrideState) pro.Add(new JProperty(nameof(scatter), scatter));
            if (value.clamp.overrideState) pro.Add(new JProperty(nameof(clamp), clamp));
            if (value.tint.overrideState) pro.Add(new JProperty(nameof(tint), tint.ToJArray()));
            if (value.highQualityFiltering.overrideState) pro.Add(new JProperty(nameof(highQualityFiltering), highQualityFiltering));
            if (value.skipIterations.overrideState) pro.Add(new JProperty(nameof(skipIterations), skipIterations));
            if (value.dirtTexture.overrideState && dirtTexture != null)
            {
                pro.Add(new JProperty(nameof(dirtTexture), dirtTexture.Id));
                pro.Add(new JProperty(nameof(dirtIntensity), dirtIntensity));
            }
            return pro;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);

            UnityEngine.Rendering.Universal.Bloom bloom = component as UnityEngine.Rendering.Universal.Bloom;
            bloom.intensity.value = intensity;
            bloom.threshold.value = threshold;
            bloom.active = active;
            bloom.scatter.value = scatter;
            bloom.clamp.value = clamp;
            bloom.tint.value = tint;
            bloom.highQualityFiltering.value = highQualityFiltering;
            bloom.skipIterations.value = skipIterations;
            //bloom.dirtTexture.value = dirtTexture
            bloom.dirtIntensity.value = dirtIntensity;
        }
        public float intensity;
        public float threshold;
        public float scatter;
        public float clamp;
        public Color tint;
        public bool highQualityFiltering;
        public int skipIterations;
        public TextureId dirtTexture;
        public float dirtIntensity;
    }
    #endregion

    #region ChannelMixer
    public class ChannelMixer : PostProcessBase
    {
        UnityEngine.Rendering.Universal.ChannelMixer value;
        public float redOutRedIn = 100;
        public float redOutGreenIn = 0;
        public float redOutBlueIn = 0;
        public float greenOutRedIn = 0;
        public float greenOutGreenIn = 100;
        public float greenOutBlueIn = 0;
        public float blueOutRedIn = 0;
        public float blueOutGreenIn = 0;
        public float blueOutBlueIn = 100;
        public ChannelMixer()
        {
            type = PostProcessType.ChannelMixer;
            active = true;
        }
        public static ChannelMixer From(UnityEngine.Rendering.Universal.ChannelMixer target)
        {
            ChannelMixer result = new ChannelMixer() { value = target };
            result.type = PostProcessType.ChannelMixer;
            result.active = target.active;
            result.redOutRedIn = target.redOutRedIn.value;
            result.redOutGreenIn = target.redOutGreenIn.value;
            result.redOutBlueIn = target.redOutBlueIn.value;
            result.greenOutRedIn = target.greenOutRedIn.value;
            result.greenOutGreenIn = target.greenOutGreenIn.value;
            result.greenOutBlueIn = target.greenOutBlueIn.value;
            result.blueOutRedIn = target.blueOutRedIn.value;
            result.blueOutGreenIn = target.blueOutGreenIn.value;
            result.blueOutBlueIn = target.blueOutBlueIn.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.ChannelMixer result = component as UnityEngine.Rendering.Universal.ChannelMixer;
            result.redOutRedIn.value = redOutRedIn;
            result.redOutGreenIn.value = redOutGreenIn;
            result.redOutBlueIn.value = redOutBlueIn;
            result.greenOutRedIn.value = greenOutRedIn;
            result.greenOutGreenIn.value = greenOutGreenIn;
            result.greenOutBlueIn.value = greenOutBlueIn;
            result.blueOutRedIn.value = blueOutRedIn;
            result.blueOutGreenIn.value = blueOutGreenIn;
            result.blueOutBlueIn.value = blueOutBlueIn;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.redOutBlueIn.overrideState) pro.Add(new JProperty(nameof(redOutRedIn), redOutRedIn));
            if (value.redOutGreenIn.overrideState) pro.Add(new JProperty(nameof(redOutGreenIn), redOutGreenIn));
            if (value.redOutBlueIn.overrideState) pro.Add(new JProperty(nameof(redOutBlueIn), redOutBlueIn));
            if (value.greenOutRedIn.overrideState) pro.Add(new JProperty(nameof(greenOutRedIn), greenOutRedIn));
            if (value.greenOutGreenIn.overrideState) pro.Add(new JProperty(nameof(greenOutGreenIn), greenOutGreenIn));
            if (value.greenOutBlueIn.overrideState) pro.Add(new JProperty(nameof(greenOutBlueIn), greenOutBlueIn));
            if (value.blueOutRedIn.overrideState) pro.Add(new JProperty(nameof(blueOutRedIn), blueOutRedIn));
            if (value.blueOutGreenIn.overrideState) pro.Add(new JProperty(nameof(blueOutGreenIn), blueOutGreenIn));
            if (value.blueOutBlueIn.overrideState) pro.Add(new JProperty(nameof(blueOutBlueIn), blueOutBlueIn));
            return pro;
        }
        public static ChannelMixer Deserialize(GLTFRoot root, JObject jo)
        {
            ChannelMixer result = new ChannelMixer();
            JToken jt;

            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.redOutRedIn));
            if (jt != null) result.redOutRedIn = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.redOutGreenIn));
            if (jt != null) result.redOutGreenIn = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.redOutBlueIn));
            if (jt != null) result.redOutBlueIn = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.greenOutRedIn));
            if (jt != null) result.greenOutRedIn = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.greenOutGreenIn));
            if (jt != null) result.greenOutGreenIn = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.greenOutBlueIn));
            if (jt != null) result.greenOutBlueIn = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.blueOutRedIn));
            if (jt != null) result.blueOutRedIn = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.blueOutGreenIn));
            if (jt != null) result.blueOutGreenIn = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.blueOutBlueIn));
            if (jt != null) result.blueOutBlueIn = jt.DeserializeAsFloat();
            return result;
        }
    }
    #endregion

    #region ChromaticAberration
    public class ChromaticAberration : PostProcessBase
    {
        UnityEngine.Rendering.Universal.ChromaticAberration value;
        public float intensity = 0;
        public ChromaticAberration()
        {
            type = PostProcessType.ChromaticAberration;
            active = true;
        }

        public static ChromaticAberration From(UnityEngine.Rendering.Universal.ChromaticAberration target)
        {
            ChromaticAberration result = new ChromaticAberration() { value = target };
            result.type = PostProcessType.ChromaticAberration;
            result.active = target.active;
            result.intensity = target.intensity.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.ChromaticAberration result = component as UnityEngine.Rendering.Universal.ChromaticAberration;
            result.intensity.value = intensity;
        }
        public static ChromaticAberration Deserialize(GLTFRoot root, JObject jo)
        {
            ChromaticAberration result = new ChromaticAberration();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.intensity));
            if (jt != null) result.intensity = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.intensity.overrideState) pro.Add(new JProperty(nameof(intensity), intensity));
            return pro;
        }
    }
    #endregion

    #region ColorAdjustments
    public class ColorAdjustments : PostProcessBase
    {
        UnityEngine.Rendering.Universal.ColorAdjustments value;
        public float postExposure;
        public float contrast;
        public Color colorFilter = Color.white;
        public float hueShift;
        public float saturation;
        public ColorAdjustments()
        {
            type = PostProcessType.ColorAdjustments;
            active = true;
        }

        public static ColorAdjustments From(UnityEngine.Rendering.Universal.ColorAdjustments target)
        {
            ColorAdjustments result = new ColorAdjustments() { value = target };
            result.type = PostProcessType.ColorAdjustments;
            result.active = target.active;
            result.postExposure = target.postExposure.value;
            result.contrast = target.contrast.value;
            result.colorFilter = target.colorFilter.value;
            result.hueShift = target.hueShift.value;
            result.saturation = target.saturation.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.ColorAdjustments result = component as UnityEngine.Rendering.Universal.ColorAdjustments;
            result.postExposure.value = postExposure;
            result.contrast.value = contrast;
            result.colorFilter.value = colorFilter;
            result.hueShift.value = hueShift;
            result.saturation.value = saturation;
        }
        public static ColorAdjustments Deserialize(GLTFRoot root, JObject jo)
        {
            ColorAdjustments result = new ColorAdjustments();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.postExposure));
            if (jt != null) result.postExposure = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.contrast));
            if (jt != null) result.contrast = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.colorFilter));
            if (jt != null) result.colorFilter = jt.DeserializeAsColor();
            jt = jo.GetValue(nameof(result.hueShift));
            if (jt != null) result.hueShift = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.saturation));
            if (jt != null) result.saturation = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.postExposure.overrideState) pro.Add(new JProperty(nameof(postExposure), postExposure));
            if (value.contrast.overrideState) pro.Add(new JProperty(nameof(contrast), contrast));
            if (value.colorFilter.overrideState) pro.Add(new JProperty(nameof(colorFilter), colorFilter.ToJArray()));
            if (value.hueShift.overrideState) pro.Add(new JProperty(nameof(hueShift), hueShift));
            if (value.saturation.overrideState) pro.Add(new JProperty(nameof(saturation), saturation));
            return pro;
        }
    }
    #endregion

    #region ColorCurves
    public class ColorCurves : PostProcessBase
    {
        // TextureCurve serilization is not supported yet
        public UnityEngine.Rendering.TextureCurve master;
        public UnityEngine.Rendering.TextureCurve red;
        public UnityEngine.Rendering.TextureCurve green;
        public UnityEngine.Rendering.TextureCurve blue;
        public UnityEngine.Rendering.TextureCurve hueVsHue;
        public UnityEngine.Rendering.TextureCurve hueVsSat;
        public UnityEngine.Rendering.TextureCurve satVsSat;
        public UnityEngine.Rendering.TextureCurve lumVsSat;
        public ColorCurves()
        {
            type = PostProcessType.ColorCurves;
            active = true;
        }

        public static ColorCurves From(UnityEngine.Rendering.Universal.ColorCurves target)
        {

            ColorCurves result = new ColorCurves();
            result.type = PostProcessType.ColorCurves;
            result.active = target.active;
            result.master = target.master.value;
            result.red = target.red.value;
            result.green = target.green.value;
            result.blue = target.blue.value;
            result.hueVsHue = target.hueVsHue.value;
            result.hueVsSat = target.hueVsSat.value;
            result.satVsSat = target.satVsSat.value;
            result.lumVsSat = target.lumVsSat.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.ColorCurves result = component as UnityEngine.Rendering.Universal.ColorCurves;
            result.master.value = master;
            result.red.value = red;
            result.green.value = green;
            result.blue.value = blue;
            result.hueVsHue.value = hueVsHue;
            result.hueVsSat.value = hueVsSat;
            result.satVsSat.value = satVsSat;
            result.lumVsSat.value = lumVsSat;
        }
        public static ColorCurves Deserialize(GLTFRoot root, JObject jo)
        {

            ColorCurves result = new ColorCurves();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.master));
            /*            if (jt != null) result.master = jt.;
                        jt = jo.GetValue(nameof(result.red));
                        if (jt != null) result.red = jt.;
                        jt = jo.GetValue(nameof(result.green));
                        if (jt != null) result.green = jt.;
                        jt = jo.GetValue(nameof(result.blue));
                        if (jt != null) result.blue = jt.;
                        jt = jo.GetValue(nameof(result.hueVsHue));
                        if (jt != null) result.hueVsHue = jt.;
                        jt = jo.GetValue(nameof(result.hueVsSat));
                        if (jt != null) result.hueVsSat = jt.;
                        jt = jo.GetValue(nameof(result.satVsSat));
                        if (jt != null) result.satVsSat = jt.;
                        jt = jo.GetValue(nameof(result.lumVsSat));
                        if (jt != null) result.lumVsSat = jt.;*/
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            /*            pro.Add(new JProperty(nameof(master), master.));
                        pro.Add(new JProperty(nameof(red), red.));
                        pro.Add(new JProperty(nameof(green), green.));
                        pro.Add(new JProperty(nameof(blue), blue.));
                        pro.Add(new JProperty(nameof(hueVsHue), hueVsHue.));
                        pro.Add(new JProperty(nameof(hueVsSat), hueVsSat.));
                        pro.Add(new JProperty(nameof(satVsSat), satVsSat.));
                        pro.Add(new JProperty(nameof(lumVsSat), lumVsSat.));*/
            return pro;
        }
    }
    #endregion

    #region ColorLookup
    public class ColorLookup : PostProcessBase
    {
        UnityEngine.Rendering.Universal.ColorLookup value;
        public int textureId { get; private set; }
        public Texture2D texture;
        public float contribution = 1.0f;
        public void SetLookupTextureId(int id) { textureId = id; }
        public ColorLookup()
        {
            type = PostProcessType.ColorLookup;
            active = true;
        }

        public static ColorLookup From(UnityEngine.Rendering.Universal.ColorLookup target)
        {
            ColorLookup result = new ColorLookup() { value = target };
            result.type = PostProcessType.ColorLookup;
            result.active = target.active;
            result.texture = (Texture2D)target.texture.value;
            result.contribution = target.contribution.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.ColorLookup result = component as UnityEngine.Rendering.Universal.ColorLookup;
            result.texture.value = texture;
            result.contribution.value = contribution;
        }
        public static ColorLookup Deserialize(GLTFRoot root, JObject jo)
        {
            ColorLookup result = new ColorLookup();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.texture));
            if (jt != null)
            {
                result.textureId = jt.DeserializeAsInt();
            }
            jt = jo.GetValue(nameof(result.contribution));
            if (jt != null) result.contribution = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.texture.overrideState) pro.Add(new JProperty(nameof(texture), textureId));
            if (value.contribution.overrideState) pro.Add(new JProperty(nameof(contribution), contribution));
            return pro;
        }
    }
    #endregion

    #region DepthOfField
    public class DepthOfField : PostProcessBase
    {
        UnityEngine.Rendering.Universal.DepthOfField value;
        public UnityEngine.Rendering.Universal.DepthOfFieldMode mode = UnityEngine.Rendering.Universal.DepthOfFieldMode.Off;
        public float gaussianStart = 10;
        public float gaussianEnd = 30;
        public float gaussianMaxRadius = 1;
        public bool highQualitySampling = false;
        public float focusDistance = 10;
        public float aperture = 5.6f;
        public float focalLength = 50f;
        public int bladeCount = 5;
        public float bladeCurvature = 1;
        public float bladeRotation = 0;
        public DepthOfField()
        {
            type = PostProcessType.DepthOfField;
            active = true;
        }

        public static DepthOfField From(UnityEngine.Rendering.Universal.DepthOfField target)
        {
            DepthOfField result = new DepthOfField() { value = target };
            result.type = PostProcessType.DepthOfField;
            result.active = target.active;
            result.mode = target.mode.value;
            result.gaussianStart = target.gaussianStart.value;
            result.gaussianEnd = target.gaussianEnd.value;
            result.gaussianMaxRadius = target.gaussianMaxRadius.value;
            result.highQualitySampling = target.highQualitySampling.value;
            result.focusDistance = target.focusDistance.value;
            result.aperture = target.aperture.value;
            result.focalLength = target.focalLength.value;
            result.bladeCount = target.bladeCount.value;
            result.bladeCurvature = target.bladeCurvature.value;
            result.bladeRotation = target.bladeRotation.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.DepthOfField result = component as UnityEngine.Rendering.Universal.DepthOfField;
            result.mode.value = mode;
            result.gaussianStart.value = gaussianStart;
            result.gaussianEnd.value = gaussianEnd;
            result.gaussianMaxRadius.value = gaussianMaxRadius;
            result.highQualitySampling.value = highQualitySampling;
            result.focusDistance.value = focusDistance;
            result.aperture.value = aperture;
            result.focalLength.value = focalLength;
            result.bladeCount.value = bladeCount;
            result.bladeCurvature.value = bladeCurvature;
            result.bladeRotation.value = bladeRotation;
        }
        public static DepthOfField Deserialize(GLTFRoot root, JObject jo)
        {
            DepthOfField result = new DepthOfField();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.mode));
            if (jt != null) result.mode = jt.DeserializeAsEnum<UnityEngine.Rendering.Universal.DepthOfFieldMode>();
            jt = jo.GetValue(nameof(result.gaussianStart));
            if (jt != null) result.gaussianStart = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.gaussianEnd));
            if (jt != null) result.gaussianEnd = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.gaussianMaxRadius));
            if (jt != null) result.gaussianMaxRadius = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.highQualitySampling));
            if (jt != null) result.highQualitySampling = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.focusDistance));
            if (jt != null) result.focusDistance = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.aperture));
            if (jt != null) result.aperture = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.focalLength));
            if (jt != null) result.focalLength = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.bladeCount));
            if (jt != null) result.bladeCount = jt.DeserializeAsInt();
            jt = jo.GetValue(nameof(result.bladeCurvature));
            if (jt != null) result.bladeCurvature = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.bladeRotation));
            if (jt != null) result.bladeRotation = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.mode.overrideState) pro.Add(new JProperty(nameof(mode), mode.ToString()));
            if (value.gaussianStart.overrideState) pro.Add(new JProperty(nameof(gaussianStart), gaussianStart));
            if (value.gaussianEnd.overrideState) pro.Add(new JProperty(nameof(gaussianEnd), gaussianEnd));
            if (value.gaussianMaxRadius.overrideState) pro.Add(new JProperty(nameof(gaussianMaxRadius), gaussianMaxRadius));
            if (value.highQualitySampling.overrideState) pro.Add(new JProperty(nameof(highQualitySampling), highQualitySampling));
            if (value.focusDistance.overrideState) pro.Add(new JProperty(nameof(focusDistance), focusDistance));
            if (value.aperture.overrideState) pro.Add(new JProperty(nameof(aperture), aperture));
            if (value.focalLength.overrideState) pro.Add(new JProperty(nameof(focalLength), focalLength));
            if (value.bladeCount.overrideState) pro.Add(new JProperty(nameof(bladeCount), bladeCount));
            if (value.bladeCurvature.overrideState) pro.Add(new JProperty(nameof(bladeCurvature), bladeCurvature));
            if (value.bladeRotation.overrideState) pro.Add(new JProperty(nameof(bladeRotation), bladeRotation));
            return pro;
        }
    }

    #endregion

    #region FilmGrain
    public class FilmGrain : PostProcessBase
    {
        UnityEngine.Rendering.Universal.FilmGrain value;
        public UnityEngine.Rendering.Universal.FilmGrainLookup LookupType = UnityEngine.Rendering.Universal.FilmGrainLookup.Thin1;
        public float intensity = 0;
        public float response = 0.8f;
        public Texture texture;
        public int textureId { get; private set; }
        public void SetTextureId(int id) { textureId = id; }
        public FilmGrain()
        {
            type = PostProcessType.FilmGrain;
            active = true;
        }

        public static FilmGrain From(UnityEngine.Rendering.Universal.FilmGrain target)
        {
            FilmGrain result = new FilmGrain() { value = target };
            result.type = PostProcessType.FilmGrain;
            result.active = target.active;
            result.LookupType = target.type.value;
            result.intensity = target.intensity.value;
            result.response = target.response.value;
            result.texture = target.texture.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.FilmGrain result = component as UnityEngine.Rendering.Universal.FilmGrain;
            result.type.value = LookupType;
            result.intensity.value = intensity;
            result.response.value = response;
            result.texture.value = texture;
        }
        public static FilmGrain Deserialize(GLTFRoot root, JObject jo)
        {
            FilmGrain result = new FilmGrain();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.LookupType));
            if (jt != null) result.LookupType = jt.DeserializeAsEnum<UnityEngine.Rendering.Universal.FilmGrainLookup>();
            jt = jo.GetValue(nameof(result.intensity));
            if (jt != null) result.intensity = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.response));
            if (jt != null) result.response = jt.DeserializeAsFloat();
            if (jt != null) result.textureId = jt.DeserializeAsInt();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.type.overrideState) pro.Add(new JProperty(nameof(LookupType), LookupType.ToString()));
            if (value.intensity.overrideState) pro.Add(new JProperty(nameof(intensity), intensity));
            if (value.response.overrideState) pro.Add(new JProperty(nameof(response), response));
            pro.Add(new JProperty(nameof(texture), textureId));
            return pro;
        }
    }

    #endregion

    #region LensDistortion
    public class LensDistortion : PostProcessBase
    {
        UnityEngine.Rendering.Universal.LensDistortion value;
        public float intensity = 0;
        public float xMultiplier = 1f;
        public float yMultiplier = 1f;
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public float scale = 1f;
        public LensDistortion()
        {
            type = PostProcessType.LensDistortion;
            active = true;
        }

        public static LensDistortion From(UnityEngine.Rendering.Universal.LensDistortion target)
        {
            LensDistortion result = new LensDistortion() { value = target };
            result.type = PostProcessType.LensDistortion;
            result.active = target.active;
            result.intensity = target.intensity.value;
            result.xMultiplier = target.xMultiplier.value;
            result.yMultiplier = target.yMultiplier.value;
            result.center = target.center.value;
            result.scale = target.scale.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.LensDistortion result = component as UnityEngine.Rendering.Universal.LensDistortion;
            result.intensity.value = intensity;
            result.xMultiplier.value = xMultiplier;
            result.yMultiplier.value = yMultiplier;
            result.center.value = center;
            result.scale.value = scale;
        }
        public static LensDistortion Deserialize(GLTFRoot root, JObject jo)
        {
            LensDistortion result = new LensDistortion();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.intensity));
            if (jt != null) result.intensity = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.xMultiplier));
            if (jt != null) result.xMultiplier = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.yMultiplier));
            if (jt != null) result.yMultiplier = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.center));
            if (jt != null) result.center = jt.DeserializeAsVector2();
            jt = jo.GetValue(nameof(result.scale));
            if (jt != null) result.scale = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.intensity.overrideState) pro.Add(new JProperty(nameof(intensity), intensity));
            if (value.xMultiplier.overrideState) pro.Add(new JProperty(nameof(xMultiplier), xMultiplier));
            if (value.yMultiplier.overrideState) pro.Add(new JProperty(nameof(yMultiplier), yMultiplier));
            if (value.center.overrideState) pro.Add(new JProperty(nameof(center), center.ToJArray()));
            if (value.scale.overrideState) pro.Add(new JProperty(nameof(scale), scale));
            return pro;
        }
    }
    #endregion

    #region LiftGammaGain
    public class LiftGammaGain : PostProcessBase
    {
        UnityEngine.Rendering.Universal.LiftGammaGain value;
        public Vector4 lift = new Vector4(1f, 1f, 1f, 0f);
        public Vector4 gamma = new Vector4(1f, 1f, 1f, 0f);
        public Vector4 gain = new Vector4(1f, 1f, 1f, 0f);
        public LiftGammaGain()
        {
            type = PostProcessType.LiftGammaGain;
            active = true;
        }

        public static LiftGammaGain From(UnityEngine.Rendering.Universal.LiftGammaGain target)
        {
            LiftGammaGain result = new LiftGammaGain() { value = target };
            result.type = PostProcessType.LiftGammaGain;
            result.active = target.active;
            result.lift = target.lift.value;
            result.gamma = target.gamma.value;
            result.gain = target.gain.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.LiftGammaGain result = component as UnityEngine.Rendering.Universal.LiftGammaGain;
            result.lift.value = lift;
            result.gamma.value = gamma;
            result.gain.value = gain;
        }
        public static LiftGammaGain Deserialize(GLTFRoot root, JObject jo)
        {
            LiftGammaGain result = new LiftGammaGain();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.lift));
            if (jt != null) result.lift = jt.DeserializeAsColor().ToVector4();
            jt = jo.GetValue(nameof(result.gamma));
            if (jt != null) result.gamma = jt.DeserializeAsColor().ToVector4();
            jt = jo.GetValue(nameof(result.gain));
            if (jt != null) result.gain = jt.DeserializeAsColor().ToVector4();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.lift.overrideState) pro.Add(new JProperty(nameof(lift), lift.ToJArray()));
            if (value.gamma.overrideState) pro.Add(new JProperty(nameof(gamma), gamma.ToJArray()));
            if (value.gain.overrideState) pro.Add(new JProperty(nameof(gain), gain.ToJArray()));
            return pro;
        }
    }
    #endregion

    #region MotionBlur
    public class MotionBlur : PostProcessBase
    {
        UnityEngine.Rendering.Universal.MotionBlur value;
        public UnityEngine.Rendering.Universal.MotionBlurMode mode = UnityEngine.Rendering.Universal.MotionBlurMode.CameraOnly;
        public UnityEngine.Rendering.Universal.MotionBlurQuality quality = UnityEngine.Rendering.Universal.MotionBlurQuality.Low;
        public float intensity = 0;
        public float clamp = 0.05f;
        public MotionBlur()
        {
            type = PostProcessType.MotionBlur;
            active = true;
        }

        public static MotionBlur From(UnityEngine.Rendering.Universal.MotionBlur target)
        {
            MotionBlur result = new MotionBlur() { value = target };
            result.type = PostProcessType.MotionBlur;
            result.active = target.active;
            result.mode = target.mode.value;
            result.quality = target.quality.value;
            result.intensity = target.intensity.value;
            result.clamp = target.clamp.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.MotionBlur result = component as UnityEngine.Rendering.Universal.MotionBlur;
            result.mode.value = mode;
            result.quality.value = quality;
            result.intensity.value = intensity;
            result.clamp.value = clamp;
        }
        public static MotionBlur Deserialize(GLTFRoot root, JObject jo)
        {
            MotionBlur result = new MotionBlur();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.mode));
            if (jt != null) result.mode = jt.DeserializeAsEnum<UnityEngine.Rendering.Universal.MotionBlurMode>();
            jt = jo.GetValue(nameof(result.quality));
            if (jt != null) result.quality = jt.DeserializeAsEnum<UnityEngine.Rendering.Universal.MotionBlurQuality>();
            jt = jo.GetValue(nameof(result.intensity));
            if (jt != null) result.intensity = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.clamp));
            if (jt != null) result.clamp = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.mode.overrideState) pro.Add(new JProperty(nameof(mode), mode.ToString()));
            if (value.quality.overrideState) pro.Add(new JProperty(nameof(quality), quality.ToString()));
            if (value.intensity.overrideState) pro.Add(new JProperty(nameof(intensity), intensity));
            if (value.clamp.overrideState) pro.Add(new JProperty(nameof(clamp), clamp));
            return pro;
        }
    }
    #endregion

    #region PaniniProjection
    public class PaniniProjection : PostProcessBase
    {
        UnityEngine.Rendering.Universal.PaniniProjection value;
        public float distance = 0f;
        public float cropToFit = 1f;
        public PaniniProjection()
        {
            type = PostProcessType.PaniniProjection;
            active = true;
        }

        public static PaniniProjection From(UnityEngine.Rendering.Universal.PaniniProjection target)
        {
            PaniniProjection result = new PaniniProjection() { value = target };
            result.type = PostProcessType.PaniniProjection;
            result.active = target.active;
            result.distance = target.distance.value;
            result.cropToFit = target.cropToFit.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.PaniniProjection result = component as UnityEngine.Rendering.Universal.PaniniProjection;
            result.distance.value = distance;
            result.cropToFit.value = cropToFit;
        }
        public static PaniniProjection Deserialize(GLTFRoot root, JObject jo)
        {
            PaniniProjection result = new PaniniProjection();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.distance));
            if (jt != null) result.distance = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.cropToFit));
            if (jt != null) result.cropToFit = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.distance.overrideState) pro.Add(new JProperty(nameof(distance), distance));
            if (value.cropToFit.overrideState) pro.Add(new JProperty(nameof(cropToFit), cropToFit));
            return pro;
        }
    }
    #endregion

    #region ShadowsMidtonesHighlights
    public class ShadowsMidtonesHighlights : PostProcessBase
    {
        UnityEngine.Rendering.Universal.ShadowsMidtonesHighlights value;
        public Vector4 shadows = new Vector4(1f, 1f, 1f, 0f);
        public Vector4 midtones = new Vector4(1f, 1f, 1f, 0f);
        public Vector4 highlights = new Vector4(1f, 1f, 1f, 0f);
        public float shadowsStart = 0;
        public float shadowsEnd = 0.3f;
        public float highlightsStart = 0.55f;
        public float highlightsEnd = 1f;
        public ShadowsMidtonesHighlights()
        {
            type = PostProcessType.ShadowsMidtonesHighlights;
            active = true;
        }

        public static ShadowsMidtonesHighlights From(UnityEngine.Rendering.Universal.ShadowsMidtonesHighlights target)
        {
            ShadowsMidtonesHighlights result = new ShadowsMidtonesHighlights() { value = target };
            result.type = PostProcessType.ShadowsMidtonesHighlights;
            result.active = target.active;
            result.shadows = target.shadows.value;
            result.midtones = target.midtones.value;
            result.highlights = target.highlights.value;
            result.shadowsStart = target.shadowsStart.value;
            result.shadowsEnd = target.shadowsEnd.value;
            result.highlightsStart = target.highlightsStart.value;
            result.highlightsEnd = target.highlightsEnd.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.ShadowsMidtonesHighlights result = component as UnityEngine.Rendering.Universal.ShadowsMidtonesHighlights;
            result.shadows.value = shadows;
            result.midtones.value = midtones;
            result.highlights.value = highlights;
            result.shadowsStart.value = shadowsStart;
            result.shadowsEnd.value = shadowsEnd;
            result.highlightsStart.value = highlightsStart;
            result.highlightsEnd.value = highlightsEnd;
        }
        public static ShadowsMidtonesHighlights Deserialize(GLTFRoot root, JObject jo)
        {
            ShadowsMidtonesHighlights result = new ShadowsMidtonesHighlights();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.shadows));
            if (jt != null) result.shadows = jt.DeserializeAsColor().ToVector4();
            jt = jo.GetValue(nameof(result.midtones));
            if (jt != null) result.midtones = jt.DeserializeAsColor().ToVector4();
            jt = jo.GetValue(nameof(result.highlights));
            if (jt != null) result.highlights = jt.DeserializeAsColor().ToVector4();
            jt = jo.GetValue(nameof(result.shadowsStart));
            if (jt != null) result.shadowsStart = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.shadowsEnd));
            if (jt != null) result.shadowsEnd = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.highlightsStart));
            if (jt != null) result.highlightsStart = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.highlightsEnd));
            if (jt != null) result.highlightsEnd = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.shadows.overrideState) pro.Add(new JProperty(nameof(shadows), shadows.ToJArray()));
            if (value.midtones.overrideState) pro.Add(new JProperty(nameof(midtones), midtones.ToJArray()));
            if (value.highlights.overrideState) pro.Add(new JProperty(nameof(highlights), highlights.ToJArray()));
            if (value.shadowsStart.overrideState) pro.Add(new JProperty(nameof(shadowsStart), shadowsStart));
            if (value.shadowsEnd.overrideState) pro.Add(new JProperty(nameof(shadowsEnd), shadowsEnd));
            if (value.highlightsStart.overrideState) pro.Add(new JProperty(nameof(highlightsStart), highlightsStart));
            if (value.highlightsEnd.overrideState) pro.Add(new JProperty(nameof(highlightsEnd), highlightsEnd));
            return pro;
        }
    }
    #endregion

    #region SplitToning
    public class SplitToning : PostProcessBase
    {
        UnityEngine.Rendering.Universal.SplitToning value;
        public Color shadows = Color.grey;
        public Color highlights = Color.grey;
        public float balance = 0;
        public SplitToning()
        {
            type = PostProcessType.SplitToning;
            active = true;
        }

        public static SplitToning From(UnityEngine.Rendering.Universal.SplitToning target)
        {
            SplitToning result = new SplitToning() { value = target };
            result.type = PostProcessType.SplitToning;
            result.active = target.active;
            result.shadows = target.shadows.value;
            result.highlights = target.highlights.value;
            result.balance = target.balance.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.SplitToning result = component as UnityEngine.Rendering.Universal.SplitToning;
            result.shadows.value = shadows;
            result.highlights.value = highlights;
            result.balance.value = balance;
        }
        public static SplitToning Deserialize(GLTFRoot root, JObject jo)
        {
            SplitToning result = new SplitToning();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.shadows));
            if (jt != null) result.shadows = jt.DeserializeAsColor();
            jt = jo.GetValue(nameof(result.highlights));
            if (jt != null) result.highlights = jt.DeserializeAsColor();
            jt = jo.GetValue(nameof(result.balance));
            if (jt != null) result.balance = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.shadows.overrideState) pro.Add(new JProperty(nameof(shadows), shadows.ToJArray()));
            if (value.shadows.overrideState) pro.Add(new JProperty(nameof(highlights), highlights.ToJArray()));
            if (value.shadows.overrideState) pro.Add(new JProperty(nameof(balance), balance));
            return pro;
        }
    }
    #endregion

    #region Tonemapping

    public class Tonemapping : PostProcessBase
    {
        UnityEngine.Rendering.Universal.Tonemapping value;
        public UnityEngine.Rendering.Universal.TonemappingMode mode;
        public Tonemapping()
        {
            type = PostProcessType.Tonemapping;
            active = true;
        }

        public static Tonemapping From(UnityEngine.Rendering.Universal.Tonemapping target)
        {
            Tonemapping result = new Tonemapping() { value = target };
            result.type = PostProcessType.Tonemapping;
            result.active = target.active;
            result.mode = target.mode.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.Tonemapping result = component as UnityEngine.Rendering.Universal.Tonemapping;
            result.mode.value = mode;
        }
        public static Tonemapping Deserialize(GLTFRoot root, JObject jo)
        {
            Tonemapping result = new Tonemapping();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.mode));
            if (jt != null) result.mode = jt.DeserializeAsEnum<UnityEngine.Rendering.Universal.TonemappingMode>();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.mode.overrideState) pro.Add(new JProperty(nameof(mode), mode.ToString()));
            return pro;
        }
    }
    #endregion

    #region Vignette
    public class Vignette : PostProcessBase
    {
        UnityEngine.Rendering.Universal.Vignette value;
        public Color color = Color.black;
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public float intensity = 0f;
        public float smoothness = 0.2f;
        public bool rounded = false;
        public Vignette()
        {
            type = PostProcessType.Vignette;
            active = true;
            color = new Color(0f, 0f, 0f, 1f);
            center = new Vector2(0.5f, 0.5f);
            intensity = 0f;
            smoothness = 0.2f;
            rounded = false;
        }
        public static Vignette From(UnityEngine.Rendering.Universal.Vignette vignette)
        {
            return new Vignette()
            {
                value = vignette,
                type = PostProcessType.Vignette,
                active = vignette.active,
                color = vignette.color.value,
                center = vignette.center.value,
                intensity = vignette.intensity.value,
                smoothness = vignette.smoothness.value,
                rounded = vignette.rounded.value,
            };
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);

            UnityEngine.Rendering.Universal.Vignette vignette = component as UnityEngine.Rendering.Universal.Vignette;
            vignette.color.value = color;
            vignette.center.value = center;
            vignette.active = active;
            vignette.intensity.value = intensity;
            vignette.smoothness.value = smoothness;
            vignette.rounded.value = rounded;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.color.overrideState) pro.Add(new JProperty(nameof(color), color.ToJArray()));
            if (value.center.overrideState) pro.Add(new JProperty(nameof(center), center.ToJArray()));
            if (value.intensity.overrideState) pro.Add(new JProperty(nameof(intensity), intensity));
            if (value.smoothness.overrideState) pro.Add(new JProperty(nameof(smoothness), smoothness));
            if (value.rounded.overrideState) pro.Add(new JProperty(nameof(rounded), rounded));
            return pro;
        }
        public static Vignette Deserialize(GLTFRoot root, JObject jo)
        {
            Vignette vignette = new Vignette();

            JToken jt = jo.GetValue(nameof(vignette.active));
            if (jt != null) vignette.active = jt.DeserializeAsBool();

            jt = jo.GetValue(nameof(vignette.color));
            if (jt != null) vignette.color = jt.DeserializeAsColor();

            jt = jo.GetValue(nameof(vignette.center));
            if (jt != null) vignette.center = jt.DeserializeAsVector2();

            jt = jo.GetValue(nameof(vignette.intensity));
            if (jt != null) vignette.intensity = jt.DeserializeAsFloat();

            jt = jo.GetValue(nameof(vignette.smoothness));
            if (jt != null) vignette.smoothness = jt.DeserializeAsFloat();

            jt = jo.GetValue(nameof(vignette.rounded));
            if (jt != null) vignette.rounded = jt.DeserializeAsBool();

            return vignette;
        }
    }

    #endregion

    #region WhiteBalance
    public class WhiteBalance : PostProcessBase
    {
        UnityEngine.Rendering.Universal.WhiteBalance value;
        public float temperature = 0;
        public float tint = 0;
        public WhiteBalance()
        {
            type = PostProcessType.WhiteBalance;
            active = true;
        }

        public static WhiteBalance From(UnityEngine.Rendering.Universal.WhiteBalance target)
        {
            WhiteBalance result = new WhiteBalance() { value = target };
            result.type = PostProcessType.WhiteBalance;
            result.active = target.active;
            result.temperature = target.temperature.value;
            result.tint = target.tint.value;
            return result;
        }
        public override void SetData(UnityEngine.Rendering.VolumeComponent component)
        {
            base.SetData(component);
            UnityEngine.Rendering.Universal.WhiteBalance result = component as UnityEngine.Rendering.Universal.WhiteBalance;
            result.temperature.value = temperature;
            result.tint.value = tint;
        }
        public static WhiteBalance Deserialize(GLTFRoot root, JObject jo)
        {
            WhiteBalance result = new WhiteBalance();
            JToken jt;
            jt = jo.GetValue(nameof(result.active));
            if (jt != null) result.active = jt.DeserializeAsBool();
            jt = jo.GetValue(nameof(result.temperature));
            if (jt != null) result.temperature = jt.DeserializeAsFloat();
            jt = jo.GetValue(nameof(result.tint));
            if (jt != null) result.tint = jt.DeserializeAsFloat();
            return result;
        }
        public override JObject Serialize()
        {
            JObject pro = base.Serialize();
            if (value.temperature.overrideState) pro.Add(new JProperty(nameof(temperature), temperature));
            if (value.tint.overrideState) pro.Add(new JProperty(nameof(tint), tint));
            return pro;
        }
    }
    #endregion
}