using GLTF;
using GLTF.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using BVA.Cache;
using BVA.Extensions;
using GLTF.Schema.BVA;
using System.Linq;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async Task LoadAnimation(GameObject sceneRoot, CancellationToken cancellationToken)
        {
            if (_gltfRoot.Animations != null && _gltfRoot.Animations.Count > 0)
            {
                // attach Animation component to the GameObject
                bool hasCreateAnimation = false;
                for (int i = 0; i < _gltfRoot.Nodes.Count; i++)
                {
                    var node = _gltfRoot.Nodes[i];
                    if (node.Extras != null && node.Extras.Count > 0)
                    {
                        foreach (var extra in node.Extras)
                        {
                            var (propertyName, reader) = GetExtraProperty(extra);
                            if (propertyName == BVA_Animation_Extra.PROPERTY)
                            {
                                hasCreateAnimation = true;
                                Animation animation = _assetCache.NodeCache[i].AddComponent<Animation>();
                                await BVA_Animation_Extra.Deserialize(_gltfRoot, reader, animation, ConstructClip, _assetCache.NodeCache[i].transform, cancellationToken);
                            }
                        }
                    }
                }

                // for standard gltf without BVA_Animation_Extra, add Animation Component on the root node.
                if (!hasCreateAnimation)
                {
                    Animation animation = sceneRoot.GetOrAddComponent<Animation>();
                    for (int i = 0; i < _gltfRoot.Animations.Count; i++)
                    {
                        AnimationClip animationClip = await ConstructClip(false, animation.transform, i, cancellationToken);
                        animation.AddClip(animationClip, animationClip.name);
                    }
                }
            }

            // find the BVA_Humanoid_AnimatorClip extra, if founded, add Animator Component on it then bind AnimatorController on it
            // after scene import, extract the AnimatorController to local disk,so firstly,record everything in Humanoid class.
            if (_gltfRoot.Extensions.HumanoidAnimationClips != null && _gltfRoot.Extensions.HumanoidAnimationClips.Count > 0)
            {
                for (int i = 0; i < _gltfRoot.Extensions.HumanoidAnimationClips.Count; ++i)
                {
                    var animatorClip = _gltfRoot.Extensions.HumanoidAnimationClips[i];
                    if (animatorClip.Extras != null && animatorClip.Extras.Count > 0)
                    {
                        var clip = await ConstructClip(true, sceneRoot.transform, i, cancellationToken);
                    }
                }
            }
        }
        static string RelativePathFrom(Transform self, Transform root)
        {
            var path = new List<string>();
            for (var current = self; current != null; current = current.parent)
            {
                if (current == root)
                {
                    return string.Join("/", path.ToArray());
                }

                path.Insert(0, current.name);
            }

            throw new Exception("no RelativePath");
        }

        protected virtual async Task BuildAnimationSamplers(bool isAnimatorClip, GLTFAnimation animation, int animationId)
        {
            // look up expected data types
            var typeMap = new Dictionary<int, string>();
            foreach (var channel in animation.Channels)
            {
                typeMap[channel.Sampler.Id] = channel.Target.Channel;
            }

            var samplers = isAnimatorClip ? _assetCache.AnimatorClipCache[animationId].Samplers : _assetCache.AnimationCache[animationId].Samplers;
            var samplersByType = new Dictionary<string, List<AttributeAccessor>>
            {
                {"time", new List<AttributeAccessor>(animation.Samplers.Count)}
            };

            for (var i = 0; i < animation.Samplers.Count; i++)
            {
                // no sense generating unused samplers
                if (!typeMap.ContainsKey(i))
                {
                    continue;
                }

                var samplerDef = animation.Samplers[i];

                samplers[i].Interpolation = samplerDef.Interpolation;

                // set up input accessors
                BufferCacheData inputBufferCacheData = await GetBufferData(samplerDef.Input.Value.BufferView.Value.Buffer);
                AttributeAccessor attributeAccessor = new AttributeAccessor
                {
                    AccessorId = samplerDef.Input,
                    Stream = inputBufferCacheData.Stream,
                    Offset = inputBufferCacheData.ChunkOffset
                };

                samplers[i].Input = attributeAccessor;
                samplersByType["time"].Add(attributeAccessor);

                // set up output accessors
                BufferCacheData outputBufferCacheData = await GetBufferData(samplerDef.Output.Value.BufferView.Value.Buffer);
                attributeAccessor = new AttributeAccessor
                {
                    AccessorId = samplerDef.Output,
                    Stream = outputBufferCacheData.Stream,
                    Offset = outputBufferCacheData.ChunkOffset
                };

                samplers[i].Output = attributeAccessor;

                if (!samplersByType.ContainsKey(typeMap[i]))
                {
                    samplersByType[typeMap[i]] = new List<AttributeAccessor>();
                }

                samplersByType[typeMap[i]].Add(attributeAccessor);
            }

            // populate attributeAccessors with buffer data
            GLTFHelpers.BuildAnimationSamplers(ref samplersByType);
        }

        protected void SetAnimationCurve(
            AnimationClip clip,
            string relativePath,
            string[] propertyNames,
            NumericArray input,
            NumericArray output,
            InterpolationType mode,
            Type curveType,
            ValuesConvertion getConvertedValues)
        {

            var channelCount = propertyNames.Length;
            var frameCount = input.AsFloats.Length;

            // copy all the key frame data to cache
            Keyframe[][] keyframes = new Keyframe[channelCount][];
            for (var ci = 0; ci < channelCount; ++ci)
            {
                keyframes[ci] = new Keyframe[frameCount];
            }

            for (var i = 0; i < frameCount; ++i)
            {
                var time = input.AsFloats[i];

                float[] values;
                float[] inTangents = null;
                float[] outTangents = null;
                if (mode == InterpolationType.CUBICSPLINE)
                {
                    // For cubic spline, the output will contain 3 values per keyframe; inTangent, dataPoint, and outTangent.
                    // https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/README.md#appendix-c-spline-interpolation

                    var cubicIndex = i * 3;
                    inTangents = getConvertedValues(output, cubicIndex);
                    values = getConvertedValues(output, cubicIndex + 1);
                    outTangents = getConvertedValues(output, cubicIndex + 2);
                }
                else
                {
                    // For other interpolation types, the output will only contain one value per keyframe
                    values = getConvertedValues(output, i);
                }

                for (var ci = 0; ci < channelCount; ++ci)
                {
                    if (mode == InterpolationType.CUBICSPLINE)
                    {
                        keyframes[ci][i] = new Keyframe(time, values[ci], inTangents[ci], outTangents[ci]);
                    }
                    else
                    {
                        keyframes[ci][i] = new Keyframe(time, values[ci]);
                    }
                }
            }

            for (var ci = 0; ci < channelCount; ++ci)
            {
                // copy all key frames data to animation curve and add it to the clip
                AnimationCurve curve = new AnimationCurve(keyframes[ci]);

                // For cubic spline interpolation, the inTangents and outTangents are already explicitly defined.
                // For the rest, set them appropriately.
                if (mode != InterpolationType.CUBICSPLINE)
                {
                    for (var i = 0; i < keyframes[ci].Length; i++)
                    {
                        SetTangentMode(curve, keyframes[ci], i, mode);
                    }
                }
                clip.SetCurve(relativePath, curveType, propertyNames[ci], curve);
            }
        }

        private static void SetTangentMode(AnimationCurve curve, Keyframe[] keyframes, int keyframeIndex, InterpolationType interpolation)
        {
            var key = keyframes[keyframeIndex];

            switch (interpolation)
            {
                case InterpolationType.CATMULLROMSPLINE:
                    key.inTangent = 0;
                    key.outTangent = 0;
                    break;
                case InterpolationType.LINEAR:
                    key.inTangent = GetCurveKeyframeLeftLinearSlope(keyframes, keyframeIndex);
                    key.outTangent = GetCurveKeyframeLeftLinearSlope(keyframes, keyframeIndex + 1);
                    break;
                case InterpolationType.STEP:
                    key.inTangent = float.PositiveInfinity;
                    key.outTangent = float.PositiveInfinity;
                    break;

                default:
                    throw new NotImplementedException();
            }

            curve.MoveKey(keyframeIndex, key);
        }

        private static float GetCurveKeyframeLeftLinearSlope(Keyframe[] keyframes, int keyframeIndex)
        {
            if (keyframeIndex <= 0 || keyframeIndex >= keyframes.Length)
            {
                return 0;
            }

            var valueDelta = keyframes[keyframeIndex].value - keyframes[keyframeIndex - 1].value;
            var timeDelta = keyframes[keyframeIndex].time - keyframes[keyframeIndex - 1].time;

            Debug.Assert(timeDelta > 0, "Unity does not allow you to put two keyframes in with the same time, so this should never occur.");

            return valueDelta / timeDelta;
        }

        protected async Task<AnimationClip> ConstructClip(bool isAnimatorClip, Transform root, int animationId, CancellationToken cancellationToken)
        {
            GLTFAnimation animation = isAnimatorClip ? _gltfRoot.Extensions.HumanoidAnimationClips[animationId] : _gltfRoot.Animations[animationId];

            BVA_Animation_animationClip_Descriptor clipDesc = null;
            if (animation.Extras != null && animation.Extras.Count > 0)
            {
                foreach (var extra in animation.Extras)
                {
                    var (propertyName, reader) = GetExtraProperty(extra);
                    if (propertyName != BVA_Animation_animationClip_Descriptor.NAME)
                    {
                        LogPool.ImportLogger.LogWarning(LogPart.Animation, $"unknow extra {propertyName}");
                    }
                    clipDesc = BVA_Animation_animationClip_Descriptor.Deserialize(_gltfRoot, reader);
                }
            }
            // clip on animations also can play on animator , extra:legacy define which component should be use
            bool isAnimatorMotion = false;
            if ((clipDesc != null && clipDesc.legacy == false) || isAnimatorClip)
            {
                isAnimatorMotion = true;
            }
            // return if cache exists
            AnimationCacheData animationCache = isAnimatorClip ? _assetCache.AnimatorClipCache[animationId] : _assetCache.AnimationCache[animationId];
            if (animationCache == null)
            {
                animationCache = new AnimationCacheData(animation.Samplers.Count);
                if (isAnimatorClip)
                    _assetCache.AnimatorClipCache[animationId] = animationCache;
                else
                    _assetCache.AnimationCache[animationId] = animationCache;
            }
            else if (animationCache.LoadedAnimationClip != null)
            {
                if (isAnimatorMotion && clipDesc != null)
                {
                    foreach (var nodeId in clipDesc.node)
                    {
                        _assetManager.motionLoader.AddAnimationClip(nodeId, animationCache.LoadedAnimationClip);
                    }
                }
                return animationCache.LoadedAnimationClip;
            }

            // unpack accessors
            await BuildAnimationSamplers(isAnimatorClip, animation, animationId);

            // init clip
            AnimationClip clip = new AnimationClip
            {
                name = animation.Name ?? string.Format("animation_{0}", animationId),
                legacy = !isAnimatorMotion
            };
            if (isAnimatorClip)
                _assetCache.AnimatorClipCache[animationId].LoadedAnimationClip = clip;
            else
                _assetCache.AnimationCache[animationId].LoadedAnimationClip = clip;

            foreach (AnimationChannel channel in animation.Channels)
            {
                AnimationSamplerCacheData samplerCache = animationCache.Samplers[channel.Sampler.Id];
                if (isAnimatorMotion)
                {
                    //animator has no node target,and it drive avatar work
                    NumericArray input = samplerCache.Input.AccessorContent, output = samplerCache.Output.AccessorContent;
                    string[] propertyNames;
                    propertyNames = new string[] { channel.Target.Channel };
                    string relativePath = "";
                    SetAnimationCurve(clip, relativePath, propertyNames, input, output,
                                      samplerCache.Interpolation, typeof(Animator),
                                      (data, frame) =>
                                      {
                                          var scale = data.AsFloats[frame];
                                          return new float[] { scale };
                                      });
                }
                else
                {
                    if (channel.Target.Node == null)
                    {
                        continue;
                    }

                    var node = await GetNode(channel.Target.Node.Id);
                    string relativePath = RelativePathFrom(node.transform, root);
                    Debug.Log(relativePath);
                    NumericArray input = samplerCache.Input.AccessorContent, output = samplerCache.Output.AccessorContent;

                    string[] propertyNames;

                    switch (channel.Target.Path)
                    {
                        case GLTFAnimationChannelPath.translation:
                            propertyNames = new string[] { "localPosition.x", "localPosition.y", "localPosition.z" };

                            SetAnimationCurve(clip, relativePath, propertyNames, input, output,
                                              samplerCache.Interpolation, typeof(Transform),
                                              (data, frame) =>
                                              {
                                                  var position = data.AsVec3s[frame].ToUnityVector3Convert();
                                                  return new float[] { position.x, position.y, position.z };
                                              });
                            break;

                        case GLTFAnimationChannelPath.rotation:
                            propertyNames = new string[] { "localRotation.x", "localRotation.y", "localRotation.z", "localRotation.w" };

                            SetAnimationCurve(clip, relativePath, propertyNames, input, output,
                                              samplerCache.Interpolation, typeof(Transform),
                                              (data, frame) =>
                                              {
                                                  var rotation = data.AsVec4s[frame];
                                                  var quaternion = new GLTF.Math.Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W).ToUnityQuaternionConvert();
                                                  return new float[] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };
                                              });

                            break;

                        case GLTFAnimationChannelPath.scale:
                            propertyNames = new string[] { "localScale.x", "localScale.y", "localScale.z" };

                            SetAnimationCurve(clip, relativePath, propertyNames, input, output,
                                              samplerCache.Interpolation, typeof(Transform),
                                              (data, frame) =>
                                              {
                                                  var scale = data.AsVec3s[frame].ToUnityVector3Raw();
                                                  return new float[] { scale.x, scale.y, scale.z };
                                              });
                            break;

                        case GLTFAnimationChannelPath.weights:
                            var primitives = channel.Target.Node.Value.Mesh.Value.Primitives;
                            var targets = primitives[0].Targets;
                            var targetCount = targets.Count;

                            for (int primitiveIndex = 0; primitiveIndex < primitives.Count; primitiveIndex++)
                            {
                                List<string> propertiesNames = new List<string>();
                                for (int targetIndex = 0; targetIndex < targetCount; targetIndex++)
                                {
                                    if (primitives[primitiveIndex].TargetNames == null)
                                        propertiesNames.Add($"blendShape.Morphtarget{targetIndex}");
                                    else
                                        propertiesNames.Add($"blendShape.{primitives[primitiveIndex].TargetNames[targetIndex]}");
                                }
                                var sampler = animation.Samplers[channel.Sampler.Id];

                                SetAnimationCurve(clip, relativePath, propertiesNames.ToArray(), input, output, samplerCache.Interpolation, typeof(SkinnedMeshRenderer),
                                    (data, frame) =>
                                    {
                                        float[] r = new float[targetCount];
                                        for (int targetIndex = 0; targetIndex < targetCount; targetIndex++)
                                        {
                                            int index = frame * targetCount + targetIndex;
                                            float weight = data.AsFloats[index] * 100.0f;
                                            r[targetIndex] = weight;
                                        }
                                        return r;
                                    });
                            }
                            break;
                    }
                }
            }

            clip.EnsureQuaternionContinuity();

            if (isAnimatorMotion && clipDesc != null)
            {
                foreach (var nodeId in clipDesc.node)
                {
                    _assetManager.motionLoader.AddAnimationClip(nodeId, clip);
                }
            }
            return clip;
        }
    }
#if UNITY_EDITOR
    public partial class GLTFSceneExporter
    {
        /// <summary>
        /// export sample ,add asscess to store the time and value of frames, time and value datas are independent chunks. 
        /// </summary>
        /// <param name="interpolation">InterpolationType</param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public AnimationSampler ExportAnimationSample(InterpolationType interpolation, List<Keyframe[]> frames, GLTFAnimationChannelPath channelPath)
        {
            AnimationSampler sampler = new AnimationSampler();
            sampler.Interpolation = interpolation;
            int propertyCount = frames.Count;
            int frameCount = frames[0].Length;

            float[] time = new float[frameCount];
            float[] values = new float[propertyCount * frameCount];

            for (int j = 0; j < frameCount; ++j)
            {
                time[j] = frames[0][j].time;//time value should be keep same
                for (int i = 0; i < propertyCount; ++i)
                {
                    var frame = frames[i];
                    var index = i + j * propertyCount;
                    values[index] = frame[j].value;
                }
            }

            sampler.Input = ExportAccessor(time);

            if (channelPath == GLTFAnimationChannelPath.rotation)
            {
                Vector4[] vec = new Vector4[frameCount];
                for (int i = 0; i < frameCount; i++)
                {
                    var quat = new Quaternion(values[i * propertyCount], values[i * propertyCount + 1], values[i * propertyCount + 2], values[i * propertyCount + 3]);
                    vec[i] = quat.ToGltfQuaternionConvertToUnityVector4();
                }
                sampler.Output = ExportAccessor(vec);
            }
            else if (channelPath == GLTFAnimationChannelPath.translation)
            {
                Vector3[] vec = new Vector3[frameCount];
                for (int i = 0; i < frameCount; i++)
                {
                    vec[i] = new Vector3(values[i * propertyCount], values[i * propertyCount + 1], values[i * propertyCount + 2]).ToGltfVector3ConvertToUnityVector3();
                }
                sampler.Output = ExportAccessor(vec);
            }
            else if (channelPath == GLTFAnimationChannelPath.scale)
            {
                Vector3[] vec = new Vector3[frameCount];
                for (int i = 0; i < frameCount; i++)
                {
                    vec[i] = new Vector3(values[i * propertyCount], values[i * propertyCount + 1], values[i * propertyCount + 2]);
                }
                sampler.Output = ExportAccessor(vec);
            }
            else if (channelPath == GLTFAnimationChannelPath.weights)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] /= 100.0f;
                sampler.Output = ExportAccessor(values);
            }
            else
            {
                sampler.Output = ExportAccessor(values);
            }
            return sampler;
        }
        /// <summary>
        /// Operation of AnimationCurve in AnimationClip only available at the Editor mode
        /// Critical Notice: Rotation has problem with eularAngle, please select quaternion interplation
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="root">Avatar transform root</param>
        /// <returns></returns>
        public GLTFAnimation ExportAnimationClip(AnimationClip clip, Transform root)
        {
            GLTFAnimation gltfAnim = new GLTFAnimation();
            gltfAnim.Name = clip.name;
            gltfAnim.Samplers = new List<AnimationSampler>();
            gltfAnim.Channels = new List<AnimationChannel>();
            var propertyBindings = AnimationUtility.GetCurveBindings(clip);
            int propertyCount = propertyBindings.Length;
            if (propertyCount < 1)
                LogPool.ExportLogger.LogWarning(LogPart.Animation, $"clip {clip.name} has no animation!");

            // Collect all channel keyframes,channel could on different gameObject,so we need keep the name also
            // Incase conflict with the name,make sure there is no gameObject under root (include root gameObject) use the same name
            Dictionary<Channel, List<Keyframe[]>> channelData = new Dictionary<Channel, List<Keyframe[]>>();
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                if (curve.length == 0)
                    continue;
                var channelType = Channel.GetStandardChannelType(binding.propertyName);
                // ignore RootMotion export when using Animator,only human animation has effect
                if (channelType == GLTFAnimationChannelPath.not_implement)
                {
                    LogPool.ExportLogger.LogWarning(LogPart.Animation, "Not Implemented keyframe property : " + binding.propertyName);
                    continue;
                }
                if (channelType == GLTFAnimationChannelPath.eulerAngle)
                {
                    LogPool.ExportLogger.LogWarning(LogPart.Animation, "eular angle is not officially support by Khoronos! Interpolation setting of AnimationClip should be Quaternion");
                    continue;
                }

                //if animation is BlendShape, each weight is a channel
                if (channelType == GLTFAnimationChannelPath.weights)
                {
                    channelData.Add(new Channel(binding.path, channelType, binding.propertyName), new List<Keyframe[]> { curve.keys });
                }
                else
                {
                    List<Keyframe[]> frames;
                    if (channelData.TryGetValue(new Channel(binding.path, channelType), out frames))
                    {
                        frames.Add(curve.keys);
                    }
                    else
                    {
                        channelData.Add(new Channel(binding.path, channelType), new List<Keyframe[]> { curve.keys });
                    }
                }
            }
            if (channelData.Count == 0)
            {
                LogPool.ExportLogger.LogWarning(LogPart.Animation, $"{clip.name} has no valid animation channel found!");
                return null;
            }
            foreach (var channel in channelData)
            {
                var targetName = channel.Key.path;
                var channelType = channel.Key.chanelType;
                var frames = channel.Value;
                int _nodeId = _nodeCache.GetNodeByBindingPath(root, targetName);

                // Don't export node when animation is non-standard channel type
                if (_nodeId < 0)
                    continue;
                NodeId nodeId = new NodeId() { Root = _root, Id = _nodeId };
                AnimationChannelTarget Target = new AnimationChannelTarget() { Node = nodeId, Path = channelType, Channel = channelType.ToString() };
                SamplerId samplerId = new SamplerId() { Root = _root, Id = gltfAnim.Samplers.Count };
                var gltfAnimChannel = new AnimationChannel() { Sampler = samplerId, Target = Target };
                gltfAnim.Channels.Add(gltfAnimChannel);

                var sample = ExportAnimationSample(InterpolationType.LINEAR, frames, channelType);
                gltfAnim.Samplers.Add(sample);
            }

            var gltfAnimationExtra = new BVA_Animation_animationClip_Descriptor() { node = new List<int> { _nodeCache.GetId(root.gameObject) }, wrapMode = clip.wrapMode, legacy = true };
            gltfAnim.AddExtra(BVA_Animation_animationClip_Descriptor.NAME, gltfAnimationExtra);
            return gltfAnim;
        }

        public List<AnimationClip> ConvertMecanimAnimationClips(Animator animator, bool resetTPose = true)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            if (ac == null)
                return new List<AnimationClip>();

            List<AnimationClip> newAnimationClips = new List<AnimationClip>();
            AnimationClip[] animationClips = ac.animationClips;
            foreach (var clip in animationClips)
            {
                if (clip.isHumanMotion)
                {
                    AnimationClip newClip = new AnimationClip
                    {
                        name = clip.name,
                        legacy = true
                    };
                    CopyAnimation(animator, newClip, clip);
                    //(uniqueAnimatorHumanClips, animator, clip);
                    newAnimationClips.Add(newClip);
                }
            }
            Humanoid.SetTPose(animator.avatar, animator.transform);
            return newAnimationClips;
        }
        public static void SetCurveLinear(AnimationCurve curve)
        {
            for (int i = 0; i < curve.keys.Length; ++i)
            {
                AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Linear);
            }
        }
        public static void CopyAnimation(Animator animator, AnimationClip targetAnimation, AnimationClip currentAnimation)
        {
            float timeStep = 0.02f / 60f * 100f;
            float timeCount = 0f;
            Transform[] transforms = animator.transform.GetComponentsInChildren<Transform>();
            string[] relativePath = transforms.Select(x => AnimationUtility.CalculateTransformPath(x, animator.transform)).ToArray();
            int count = relativePath.Length;
            AnimationCurve[] curveX = new AnimationCurve[count];
            AnimationCurve[] curveY = new AnimationCurve[count];
            AnimationCurve[] curveZ = new AnimationCurve[count];
            AnimationCurve[] curveRX = new AnimationCurve[count];
            AnimationCurve[] curveRY = new AnimationCurve[count];
            AnimationCurve[] curveRZ = new AnimationCurve[count];
            AnimationCurve[] curveRW = new AnimationCurve[count];
            //AnimationCurve[] curveSX = new AnimationCurve[count];
            //AnimationCurve[] curveSY = new AnimationCurve[count];
            //AnimationCurve[] curveSZ = new AnimationCurve[count];
            EditorCurveBinding[] curveBindingX = new EditorCurveBinding[count];
            EditorCurveBinding[] curveBindingY = new EditorCurveBinding[count];
            EditorCurveBinding[] curveBindingZ = new EditorCurveBinding[count];
            EditorCurveBinding[] curveBindingRX = new EditorCurveBinding[count];
            EditorCurveBinding[] curveBindingRY = new EditorCurveBinding[count];
            EditorCurveBinding[] curveBindingRZ = new EditorCurveBinding[count];
            EditorCurveBinding[] curveBindingRW = new EditorCurveBinding[count];
            //EditorCurveBinding[] curveBindingSX = new EditorCurveBinding[count];
            //EditorCurveBinding[] curveBindingSY = new EditorCurveBinding[count];
            //EditorCurveBinding[] curveBindingSZ = new EditorCurveBinding[count];

            for (int i = 0; i < count; ++i)
            {
                curveBindingX[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalPosition.x");
                curveBindingY[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalPosition.y");
                curveBindingZ[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalPosition.z");

                curveBindingRX[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalRotation.x");
                curveBindingRY[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalRotation.y");
                curveBindingRZ[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalRotation.z");
                curveBindingRW[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalRotation.w");
                //curveBindingSX[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalScale.x");
                //curveBindingSY[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalScale.y");
                //curveBindingSZ[i] = EditorCurveBinding.FloatCurve(relativePath[i], typeof(Transform), "m_LocalScale.z");
            }

            for (int i = 0; i < count; ++i)
            {
                curveX[i] = new AnimationCurve();
                curveY[i] = new AnimationCurve();
                curveZ[i] = new AnimationCurve();
                curveRX[i] = new AnimationCurve();
                curveRY[i] = new AnimationCurve();
                curveRZ[i] = new AnimationCurve();
                curveRW[i] = new AnimationCurve();
                //curveSX[i] = new AnimationCurve();
                //curveSY[i] = new AnimationCurve();
                //curveSZ[i] = new AnimationCurve();
            }

            Vector3[] prevPos = new Vector3[count];
            Vector3[] prevAngle = new Vector3[count];
            while (timeCount < currentAnimation.length)
            {
                currentAnimation.SampleAnimation(animator.gameObject, timeCount);
                for (int i = 0; i < count; i++)
                {
                    var originTransform = transforms[i];

                    Vector3 pos = originTransform.localPosition;
                    if (i == 0 || pos != prevPos[i])
                    {
                        curveX[i].AddKey(timeCount, pos.x);
                        curveY[i].AddKey(timeCount, pos.y);
                        curveZ[i].AddKey(timeCount, pos.z);
                        prevPos[i] = pos;
                    }
                    //angles = originTransform.localScale;
                    //curveSX[i].AddKey(timeCount, angles.x);
                    //curveSY[i].AddKey(timeCount, angles.y);
                    //curveSZ[i].AddKey(timeCount, angles.z);

                    Quaternion angles = originTransform.localRotation;
                    Vector3 eulerAngles = originTransform.localEulerAngles;

                    if (i == 0)
                        prevAngle[i] = eulerAngles;
                    else if (prevAngle[i].Equals(eulerAngles))
                        continue;

                    {
                        curveRX[i].AddKey(timeCount, angles.x);
                        curveRY[i].AddKey(timeCount, angles.y);
                        curveRZ[i].AddKey(timeCount, angles.z);
                        curveRW[i].AddKey(timeCount, angles.w);
                    }
                }
                timeCount += timeStep;
            }

            AnimationUtility.SetEditorCurves(targetAnimation, curveBindingX, curveX);
            AnimationUtility.SetEditorCurves(targetAnimation, curveBindingY, curveY);
            AnimationUtility.SetEditorCurves(targetAnimation, curveBindingZ, curveZ);

            //AnimationUtility.SetEditorCurves(targetAnimation, curveBindingSX, curveSX);
            //AnimationUtility.SetEditorCurves(targetAnimation, curveBindingSY, curveSY);
            //AnimationUtility.SetEditorCurves(targetAnimation, curveBindingSZ, curveSZ);

            AnimationUtility.SetEditorCurves(targetAnimation, curveBindingRX, curveRX);
            AnimationUtility.SetEditorCurves(targetAnimation, curveBindingRY, curveRY);
            AnimationUtility.SetEditorCurves(targetAnimation, curveBindingRZ, curveRZ);
            AnimationUtility.SetEditorCurves(targetAnimation, curveBindingRW, curveRW);
            targetAnimation.legacy = true;
        }
        /// <summary>
        /// we can operate AnimationCurve in AnimationClip only at the Editor Mode
        /// notice: Rotation has problem with eularAngle, please select quaternion interplation
        /// </summary>
        /// <param name="clip">Mecanim AnimationClip</param>
        /// <param name="root">Avatar transform root</param>
        /// <param name="animator">Animator component on avatar transform root</param>
        /// <returns></returns>
        public GLTFAnimation ExportMecanimAnimationClip(AnimationClip clip, Transform root, Animator animator)
        {
            if (animator == null) return null;
            GLTFAnimation gltfAnim = new GLTFAnimation();
            gltfAnim.Name = clip.name;
            gltfAnim.Samplers = new List<AnimationSampler>();
            gltfAnim.Channels = new List<AnimationChannel>();
            var propertyBindings = AnimationUtility.GetCurveBindings(clip);
            int propertyCount = propertyBindings.Length;
            if (propertyCount < 1)
                LogPool.ExportLogger.LogWarning(LogPart.Animation, $"clip {clip.name} has no animation!");

            //collect all channel keyframes,channel could on different gameObject,so we need keep the name also
            //incase conflict with the name,make sure there is no gameObject under root (include root gameObject) use the same name
            Dictionary<Channel, List<Keyframe[]>> channelData = new Dictionary<Channel, List<Keyframe[]>>();
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                var channelType = Channel.GetChannelType(binding.propertyName);
                if (channelType == GLTFAnimationChannelPath.not_implement)
                {
                    LogPool.ExportLogger.LogWarning(LogPart.Animation, "Not Implemented keyframe property : " + binding.propertyName);
                    continue;
                }
                if (channelType == GLTFAnimationChannelPath.eulerAngle)
                {
                    LogPool.ExportLogger.LogWarning(LogPart.Animation, "eular angle is not officially support by Khoronos! Interpolation setting of AnimationClip should be Quaternion");
                    continue;
                }
                List<Keyframe[]> frames = null;
                // if animation is blendshape,each belong to the seperater channel
                if (channelType == GLTFAnimationChannelPath.animator_muscle || channelType == GLTFAnimationChannelPath.weights)
                {
                    channelData.Add(new Channel(binding.path, channelType, binding.propertyName), new List<Keyframe[]> { curve.keys });
                }
                else if (channelType == GLTFAnimationChannelPath.animator_T || channelType == GLTFAnimationChannelPath.animator_Q || channelType == GLTFAnimationChannelPath.animator_S)
                {
                    channelData.Add(new Channel(binding.path, channelType, binding.propertyName), new List<Keyframe[]> { curve.keys });
                }
                else
                {
                    if (channelData.TryGetValue(new Channel(binding.path, channelType), out frames))
                    {
                        frames.Add(curve.keys);
                    }
                    else
                    {
                        channelData.Add(new Channel(binding.path, channelType), new List<Keyframe[]> { curve.keys });
                    }
                }
            }
            if (channelData.Count == 0)
            {
                LogPool.ExportLogger.LogWarning(LogPart.Animation, $"{clip.name} has no valid animation channel found!");
                return null;
            }
            foreach (var channel in channelData)
            {
                var targetName = channel.Key.path;
                var channelType = channel.Key.chanelType;
                var channelName = channel.Key.blendShapeNameOrHumanoidBoneName;
                var frames = channel.Value;
                int _nodeId = _nodeCache.GetNodeByBindingPath(root, targetName);
                //don't export node when animation is non-standard channel type
                NodeId nodeId = new NodeId() { Root = _root, Id = _nodeId };
                if (!Channel.IsStandardGltfAnimationChannel(channelType))
                    nodeId.Id = -1;
                AnimationChannelTarget Target = new AnimationChannelTarget() { Node = nodeId, Path = channelType, Channel = channelName };
                SamplerId samplerId = new SamplerId() { Root = _root, Id = gltfAnim.Samplers.Count };
                var gltfAnimChannel = new AnimationChannel() { Sampler = samplerId, Target = Target };

                gltfAnim.Channels.Add(gltfAnimChannel);

                var sample = ExportAnimationSample(InterpolationType.LINEAR, frames, channelType);
                gltfAnim.Samplers.Add(sample);
            }

            var gltfAnimationExtra = new BVA_Animation_animationClip_Descriptor() { node = new List<int> { _nodeCache.GetId(root.gameObject) }, wrapMode = clip.wrapMode, legacy = clip.legacy };
            gltfAnim.AddExtra(BVA_Animation_animationClip_Descriptor.NAME, gltfAnimationExtra);
            return gltfAnim;
        }
        private void ExportAnimationClipNode(Dictionary<AnimationClip, GLTFAnimation> uniqueClips, GameObject gameObject, AnimationClip clip)
        {
            GLTFAnimation anim;
            if (uniqueClips.TryGetValue(clip, out anim))
            {
                var desc = anim.Descriptors[BVA_Animation_animationClip_Descriptor.NAME] as BVA_Animation_animationClip_Descriptor;
                int id = _nodeCache.GetId(gameObject);
                desc.node.Add(id);
            }
            else
            {
                anim = ExportAnimationClip(clip, gameObject.transform);
                if (anim != null)
                {
                    _root.Animations.Add(anim);
                    uniqueClips.Add(clip, anim);
                }
            }
        }
        private GLTFAnimation ExportMecanimAnimationClipNode(Dictionary<AnimationClip, GLTFAnimation> uniqueClips, Animator animator, AnimationClip clip)
        {
            GLTFAnimation anim;
            if (uniqueClips.TryGetValue(clip, out anim))
            {
                var desc = anim.Descriptors[BVA_Animation_animationClip_Descriptor.NAME] as BVA_Animation_animationClip_Descriptor;
                int id = _nodeCache.GetId(animator.gameObject);
                desc.node.Add(id);
            }
            else
            {
                anim = ExportMecanimAnimationClip(clip, animator.transform, animator);
                if (anim != null)
                {
                    _root.Extensions.HumanoidAnimationClips.Add(anim);
                    _root.Extensions.AddExtension(_root, GLTFExtensions.ANIMATOR_CLIP_EXTENSION_NAME, null, RequireExtensions);
                    uniqueClips.Add(clip, anim);
                }
            }
            return anim;
        }
        /// <summary>
        /// export animation only available at editor ,due to animiationCurve data is inacessable at runtime
        /// if isHuman animation export to AnimatorClips
        /// </summary>
        public void ExportAnimation()
        {
#if !UNITY_EDITOR
            throw new EditorExportOnlyException();
#endif
            Dictionary<AnimationClip, GLTFAnimation> uniqueAnimationClips = new Dictionary<AnimationClip, GLTFAnimation>();
            foreach (var animator in _animators)
            {
                if (animator == null) continue;
                var newAnimationClips = ConvertMecanimAnimationClips(animator);

                foreach (var clip in newAnimationClips)
                {
                    ExportAnimationClipNode(uniqueAnimationClips, animator.gameObject, clip);
                }

                var extra = new BVA_Animation_Extra();
                foreach (var animationClip in newAnimationClips)
                {
                    if (uniqueAnimationClips.TryGetValue(animationClip, out GLTFAnimation gltfAnimation))
                    {
                        int indexOfAnimations = _root.Animations.IndexOf(gltfAnimation);
                        if (indexOfAnimations >= 0)
                        {
                            extra.animations.Add(indexOfAnimations);
                        }
                    }
                }
                int nodeId = _nodeCache.GetId(animator.gameObject);
                _root.Nodes[nodeId].AddExtra(BVA_Animation_Extra.PROPERTY, extra);
            }
            foreach (var animation in _animations)
            {
                var animations = animation.GetAnimationClips();
                foreach (var clip in animations)
                {
                    ExportAnimationClipNode(uniqueAnimationClips, animation.gameObject, clip);
                }
            }

            foreach (var animation in _animations)
            {
                if (animation.clip == null) continue;
                var extra = new BVA_Animation_Extra(animation);
                foreach (var animationClip in animation.GetAnimationClips())
                {
                    if (uniqueAnimationClips.TryGetValue(animationClip, out GLTFAnimation gltfAnimation))
                    {
                        int indexOfAnimations = _root.Animations.IndexOf(gltfAnimation);
                        if (indexOfAnimations >= 0)
                        {
                            extra.animations.Add(indexOfAnimations);
                        }
                    }
                }
                if (uniqueAnimationClips.TryGetValue(animation.clip, out GLTFAnimation mainGltfAnimation))
                {
                    extra.animation = _root.Animations.IndexOf(mainGltfAnimation);
                }
                int nodeId = _nodeCache.GetId(animation.gameObject);
                _root.Nodes[nodeId].AddExtra(BVA_Animation_Extra.PROPERTY, extra);
            }

            /*
            Dictionary<AnimationClip, GLTFAnimation> uniqueAnimatorHumanClips = new Dictionary<AnimationClip, GLTFAnimation>();
            foreach (var animator in _animators)
            {
                RuntimeAnimatorController ac = animator.runtimeAnimatorController;
                if (ac == null) continue;
                AnimationClip[] animationClips = ac.animationClips;
                foreach (var clip in animationClips)
                {
                    if (clip.isHumanMotion)
                    {
                        ExportMecanimAnimationClipNode(uniqueAnimatorHumanClips, animator, clip);
                    }
                }
            }
            foreach (var playable in _playables)
            {
                foreach (var info in playable.trackAsset.animationTrackGroup.tracks)
                {
                    info.SetAnimatorId(new NodeId() { Id = _nodeCache.GetId(info.animator.gameObject), Root = _root });

                    GLTFAnimation gLTF = ExportMecanimAnimationClipNode(uniqueAnimatorClips, info.animator, info.source);
                    int index = _root.Extensions.AnimatorClips.IndexOf(gLTF);
                    info.SetSourceId(index);
                }
            }
            */
        }
    }

#endif
}