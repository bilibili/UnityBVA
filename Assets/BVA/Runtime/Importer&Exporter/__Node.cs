using BVA.Component;
using BVA.EventSystem;
using BVA.Extensions;
using GLTF.Schema;
using GLTF.Schema.BVA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        private async Task<GameObject> GetNode(int nodeId)
        {
            try
            {
                if (_assetCache.NodeCache[nodeId] == null)
                {
                    if (nodeId >= _gltfRoot.Nodes.Count)
                    {
                        throw new ArgumentException("nodeIndex is out of range");
                    }

                    var node = _gltfRoot.Nodes[nodeId];

                    if (!IsMultithreaded)
                    {
                        await ConstructBufferData(node);
                    }
                    else
                    {
                        await Task.Run(() => ConstructBufferData(node));
                    }
                    // nearly 5s for an Avatar
                    await ConstructNode(node, nodeId);
                }

                return _assetCache.NodeCache[nodeId];
            }
            catch (Exception ex)
            {
                // If some failure occured during loading, remove the node

                if (_assetCache.NodeCache[nodeId] != null)
                {
                    GameObject.DestroyImmediate(_assetCache.NodeCache[nodeId]);
                    _assetCache.NodeCache[nodeId] = null;
                }

                if (ex is OutOfMemoryException)
                {
                    Resources.UnloadUnusedAssets();
                }

                throw ex;
            }
        }

        private bool hasValidExtension(Node node, string extenionName)
        {
            return _gltfRoot.ExtensionsUsed != null
             && _gltfRoot.ExtensionsUsed.Contains(extenionName)
             && node.Extensions != null
             && node.Extensions.ContainsKey(extenionName);
        }

        protected async Task ConstructNode(Node node, int nodeIndex)
        {
            if (_assetCache.NodeCache[nodeIndex] != null)
            {
                return;
            }

            var nodeObj = new GameObject(string.IsNullOrEmpty(node.Name) ? ("GLTFNode" + nodeIndex) : node.Name);
            // If we're creating a really large node, we need it to not be visible in partial stages. So we hide it while we create it
            nodeObj.SetActive(false);

            node.GetUnityTRSProperties(out Vector3 position, out Quaternion rotation, out Vector3 scale);
            nodeObj.transform.localPosition = position;
            nodeObj.transform.localRotation = rotation;
            nodeObj.transform.localScale = scale;
            _assetCache.NodeCache[nodeIndex] = nodeObj;

            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    GameObject childObj = await GetNode(child.Id);
                    childObj.transform.SetParent(nodeObj.transform, false);
                }
            }

            const string msft_LODExtName = MSFT_LODExtensionFactory.EXTENSION_NAME;
            MSFT_LODExtension lodsextension = null;
            if (hasValidExtension(node, msft_LODExtName))
            {
                lodsextension = node.Extensions[msft_LODExtName] as MSFT_LODExtension;
                if (lodsextension != null && lodsextension.MeshIds.Count > 0)
                {
                    int lodCount = lodsextension.MeshIds.Count + 1;
                    if (!CullFarLOD)
                    {
                        //create a final lod with the mesh as the last LOD in file
                        lodCount += 1;
                    }
                    LOD[] lods = new LOD[lodsextension.MeshIds.Count + 2];
                    List<double> lodCoverage = lodsextension.GetLODCoverage(node);

                    var lodGroupNodeObj = new GameObject(string.IsNullOrEmpty(node.Name) ? ("GLTFNode_LODGroup" + nodeIndex) : node.Name);
                    lodGroupNodeObj.SetActive(false);
                    nodeObj.transform.SetParent(lodGroupNodeObj.transform, false);
                    MeshRenderer[] childRenders = nodeObj.GetComponentsInChildren<MeshRenderer>();
                    lods[0] = new LOD(GetLodCoverage(lodCoverage, 0), childRenders);

                    LODGroup lodGroup = lodGroupNodeObj.AddComponent<LODGroup>();
                    for (int i = 0; i < lodsextension.MeshIds.Count; i++)
                    {
                        int lodNodeId = lodsextension.MeshIds[i];
                        var lodNodeObj = await GetNode(lodNodeId);
                        lodNodeObj.transform.SetParent(lodGroupNodeObj.transform, false);
                        childRenders = lodNodeObj.GetComponentsInChildren<MeshRenderer>();
                        int lodIndex = i + 1;
                        lods[lodIndex] = new LOD(GetLodCoverage(lodCoverage, lodIndex), childRenders);
                    }

                    if (!CullFarLOD)
                    {
                        //use the last mesh as the LOD
                        lods[lodsextension.MeshIds.Count + 1] = new LOD(0, childRenders);
                    }

                    lodGroup.SetLODs(lods);
                    lodGroup.RecalculateBounds();
                    lodGroupNodeObj.SetActive(true);
                    _assetCache.NodeCache[nodeIndex] = lodGroupNodeObj;
                }
            }
            if (node.Mesh != null)
            {
                var mesh = node.Mesh.Value;
                await ConstructMesh(mesh, node.Mesh.Id);
                var unityMesh = _assetCache.MeshCache[node.Mesh.Id].LoadedMesh;
                var materials = node.Mesh.Value.Primitives.Select(p =>
                    p.Material != null ?
                    _assetCache.MaterialCache[p.Material.Id].UnityMaterialWithVertexColor :
                    _defaultLoadedMaterial.UnityMaterialWithVertexColor
                ).ToArray();

                var morphTargets = mesh.Primitives[0].Targets;
                var weights = node.Weights ?? mesh.Weights ?? (morphTargets != null ? new List<double>(morphTargets.Select(mt => 0.0)) : null);

                if (unityMesh.boneWeights.Length > 0 || unityMesh.blendShapeCount > 0)
                {
                    var skinnedMeshRenderer = nodeObj.GetOrAddComponent<SkinnedMeshRenderer>();
                    skinnedMeshRenderer.sharedMesh = unityMesh;
                    skinnedMeshRenderer.sharedMaterials = materials;
                    skinnedMeshRenderer.quality = SkinQuality.Auto;
                    skinnedMeshRenderer.UpdateGIMaterials();
                    // setup bones after all nodes has created
                    //if (node.Skin != null)
                    //    SetupBones(node.Skin.Value, skinnedMeshRenderer);

                    //morph target weights
                    if (weights != null)
                    {
                        for (int i = 0; i < unityMesh.blendShapeCount; ++i)
                        {
                            // GLTF weights are [0, 1] range but Unity weights are [0, 100] range
                            skinnedMeshRenderer.SetBlendShapeWeight(i, (float)(weights[i] * 100));
                        }
                    }
                }
                if (!nodeObj.TryGetComponent<Renderer>(out var renderer))
                {
                    var filter = nodeObj.AddComponent<MeshFilter>();
                    filter.sharedMesh = unityMesh;
                    renderer = nodeObj.AddComponent<MeshRenderer>();
                    renderer.sharedMaterials = materials;
                    renderer.UpdateGIMaterials();
                }

                switch (Collider)
                {
                    case ColliderType.Box:
                        var boxCollider = nodeObj.AddComponent<BoxCollider>();
                        boxCollider.center = unityMesh.bounds.center;
                        boxCollider.size = unityMesh.bounds.size;
                        break;
                    case ColliderType.Mesh:
                        var meshCollider = nodeObj.AddComponent<MeshCollider>();
                        meshCollider.sharedMesh = unityMesh;
                        break;
                    case ColliderType.MeshConvex:
                        var meshConvexCollider = nodeObj.AddComponent<MeshCollider>();
                        meshConvexCollider.sharedMesh = unityMesh;
                        meshConvexCollider.convex = true;
                        break;
                }
            }

            AddNodeComponent(node, nodeObj);

            nodeObj.SetActive(true);

            progressStatus.NodeLoaded++;
            progress?.Report(progressStatus);
        }
        public void AddNodeComponent(Node node, GameObject nodeObj)
        {
            // implement camera
            if (node.Camera != null)
            {
                ImportCamera(node.Camera.Value, nodeObj);
            }

            if (hasValidExtension(node, KHR_lights_punctualExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[KHR_lights_punctualExtensionFactory.EXTENSION_NAME];
                var impl = (KHR_lights_punctualExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(KHR_lights_punctualExtensionFactory)} failed");
                var lightExt = _gltfRoot.Extensions.Lights[impl.id.Id];

                if (lightExt != null)
                    ImportLight(lightExt, node, nodeObj);
            }

            // import collision
            if (hasValidExtension(node, BVA_collisions_colliderExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_collisions_colliderExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_collisions_colliderExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_collisions_colliderExtensionFactory)} failed");
                foreach (var v in impl.ids)
                {
                    var collisionExt = _gltfRoot.Extensions.Collisions[v.Id];
                    if (collisionExt != null)
                        ImportCollision(collisionExt, nodeObj);
                }
            }

            if (node.Extras != null && node.Extras.Count > 0)
            {
                foreach (var extra in node.Extras)
                {
                    var (propertyName, reader) = GetExtraProperty(extra);
                    if (ComponentImporter.ImportComponent(propertyName, _gltfRoot, reader, nodeObj, GetTexture, LoadMaterial, LoadSprite, LoadCubemap))
                        continue;
                }
            }

            //import meta
            if (hasValidExtension(node, BVA_metaExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_metaExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_metaExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_metaExtensionFactory)} failed");

                var metaExt = _gltfRoot.Extensions.Metas[impl.id.Id];
                if (metaExt != null)
                    ImportMeta(metaExt, nodeObj);
            }

            //import postprocess
            if (hasValidExtension(node, BVA_postprocess_volumeExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_postprocess_volumeExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_postprocess_volumeExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_postprocess_volumeExtensionFactory)} failed");

                var volumeExt = _gltfRoot.Extensions.PostProcessUrpVolumes[impl.id.Id];
                if (volumeExt != null)
                    ImportPostProcess(volumeExt, impl, nodeObj);
            }

            //import audio source
            if (hasValidExtension(node, BVA_audio_audioClipExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_audio_audioClipExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_audio_audioClipExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_audio_audioClipExtensionFactory)} failed");
                foreach (var v in impl.components)
                {
                    ImportAudio(v, nodeObj);
                }
            }
        }
    }

    public partial class GLTFSceneExporter
    {
        private NodeId ExportNode(Transform nodeTransform)
        {
            var node = new Node();

            if (ExportNames)
            {
                node.Name = nodeTransform.name;
            }

            ComponentImporter.ExportComponentExtra(nodeTransform.gameObject, node,ExportTexture,ExportMaterial,ExportSprite,ExportCubemap);

            //export camera attached to node
            if (nodeTransform.TryGetComponent<Camera>(out var unityCamera))
            {
                node.Camera = ExportCamera(unityCamera);
            }

            //export light attached to node
            Light unityLight = nodeTransform.GetComponent<Light>();
            if (ShouldExportLight(unityLight))
            {
                LightId lightId = ExportLight(unityLight);
                node.AddExtension(_root, KHR_lights_punctualExtensionFactory.EXTENSION_NAME, new KHR_lights_punctualExtensionFactory(lightId), RequireExtensions);

                node.AddExtra(BVA_Light_URP_Extra.PROPERTY, new BVA_Light_URP_Extra(unityLight));
            }

            //export metaInfo attached to node
            if (nodeTransform.TryGetComponent<BVAMetaInfo>(out var metaInfo) && metaInfo.metaInfo != null)
            {
                MetaId metaId = ExportMeta(metaInfo);
                node.AddExtension(_root, BVA_metaExtensionFactory.EXTENSION_NAME, new BVA_metaExtensionFactory(metaId), RequireExtensions);
            }

            //export colliders attached to node
            {
                var cols = nodeTransform.GetComponents<Collider>();
                if (cols.Length > 0)
                {
                    List<CollisionId> collisionIds = new List<CollisionId>();
                    foreach (var col in cols)
                    {
                        if (ShouldExportCollision(col))
                        {
                            CollisionId collisionId = ExportCollision(col);
                            if (collisionId != null)
                                collisionIds.Add(collisionId);
                        }
                        else
                        {
                            LogPool.ExportLogger.LogWarning(LogPart.Node, $"{col.GetType()}  {col.name} is not exported!");
                        }
                    }
                    if (collisionIds.Count > 0)
                        node.AddExtension(_root, BVA_collisions_colliderExtensionFactory.EXTENSION_NAME, new BVA_collisions_colliderExtensionFactory(collisionIds), RequireExtensions);
                }
            }

            // add animations
            if (nodeTransform.TryGetComponent<Animation>(out var animation))
            {
                _animations.Add(animation);
            }
            // add animator
            if (nodeTransform.TryGetComponent<Animator>(out var animator))
            {
                nodeTransform.localRotation = Quaternion.identity;
                nodeTransform.localPosition = Vector3.zero;
                nodeTransform.localScale = Vector3.one;
                _animators.Add(animator);
                // if is valid human avatar,then enforce it to T Pose
                if (HasValidHumanAvatar(animator))
                {
                    //Humanoid.EnforceTPose2(animator);
                    Humanoid.SetTPose(animator.avatar, animator.transform);
                }
            }

            // export audios attached to node
            var audios = nodeTransform.GetComponents<AudioSource>();
            if (audios.Length > 0)
            {
                List<AudioSourceProperty> audioSourceId = new List<AudioSourceProperty>();
                foreach (var audio in audios)
                {
                    if (audio.clip != null)
                    {
                        AudioId audioId = ExportAudioSource(audio);
                        if (audioId != null) audioSourceId.Add(new AudioSourceProperty(audio) { audio = audioId });
                    }
                    else
                    {
                        audioSourceId.Add(new AudioSourceProperty(audio));
                    }
                }
                node.AddExtension(_root, BVA_audio_audioClipExtensionFactory.EXTENSION_NAME, new BVA_audio_audioClipExtensionFactory(audioSourceId), RequireExtensions);
            }

            // export videos attached to node
            if (nodeTransform.TryGetComponent<VideoPlayer>(out var videoPlayer))
            {
                bool export = true;
                if (videoPlayer.source != VideoSource.Url)
                {
                    LogPool.ExportLogger.LogWarning(LogPart.Node, "VideoPlayer only support url video source");
                    export = false;
                }
                if (videoPlayer.renderMode != VideoRenderMode.MaterialOverride)
                {
                    LogPool.ExportLogger.LogWarning(LogPart.Node, $"VideoPlayer only support {VideoRenderMode.MaterialOverride} VideoRenderMode");
                    export = false;
                }
                if (videoPlayer.targetMaterialRenderer == null)
                {
                    LogPool.ExportLogger.LogWarning(LogPart.Node, $"{nameof(VideoPlayer)} target MaterialRenderer can't be null");
                    export = false;
                }
                if (export)
                    _videoPlayers.Add(videoPlayer);
            }

            // add blendshapeMixer
            if (nodeTransform.TryGetComponent<BlendShapeMixer>(out var mixer) && mixer.keys != null && mixer.keys.Count > 0)
            {
                _blendShapeMixers.Add(mixer);
            }

            // add playable
            if (nodeTransform.TryGetComponent<PlayableController>(out var playable) && playable.trackAsset != null)
            {
                _playables.Add(playable);
            }

            // add postprocess
            if (nodeTransform.TryGetComponent<UnityEngine.Rendering.Volume>(out var postprocess) && postprocess.profile != null && postprocess.profile.components != null && postprocess.profile.components.Count > 0)
            {
                PostProcessId postProcessId = ExportPostProcess(postprocess.profile);
                node.AddExtension(_root, BVA_postprocess_volumeExtensionFactory.EXTENSION_NAME, new BVA_postprocess_volumeExtensionFactory(postProcessId, postprocess.isGlobal, postprocess.weight, postprocess.blendDistance, postprocess.priority), RequireExtensions);
            }

            #region Variable Collection
            // material 
            if (nodeTransform.TryGetComponent<MaterialVariableCollection>(out var materialCollection) && materialCollection.variables.Count > 0)
            {
                var variableId = ExportVariableCollection(materialCollection);
                node.AddExtension(_root, BVA_variable_collectionExtensionFactory.EXTENSION_NAME, new BVA_variable_collectionExtensionFactory(variableId), RequireExtensions);
            }

            // texture
            if (nodeTransform.TryGetComponent<TextureVariableCollection>(out var textureCollection) && textureCollection.variables.Count > 0)
            {
                var variableId = ExportVariableCollection(textureCollection);
                node.AddExtension(_root, BVA_variable_collectionExtensionFactory.EXTENSION_NAME, new BVA_variable_collectionExtensionFactory(variableId), RequireExtensions);
            }

            // cubemap
            if (nodeTransform.TryGetComponent<CubemapVariableCollection>(out var cubemapCollection) && cubemapCollection.variables.Count > 0)
            {
                var variableId = ExportVariableCollection(cubemapCollection);
                node.AddExtension(_root, BVA_variable_collectionExtensionFactory.EXTENSION_NAME, new BVA_variable_collectionExtensionFactory(variableId), RequireExtensions);
            }

            // audio
            if (nodeTransform.TryGetComponent<AudioClipVariableCollection>(out var audioCollection) && audioCollection.variables.Count > 0)
            {
                var variableId = ExportVariableCollection(audioCollection);
                node.AddExtension(_root, BVA_variable_collectionExtensionFactory.EXTENSION_NAME, new BVA_variable_collectionExtensionFactory(variableId), RequireExtensions);
            }
            #endregion

            node.SetUnityTransform(nodeTransform);

            var id = new NodeId
            {
                Id = _root.Nodes.Count,
                Root = _root
            };
            _root.Nodes.Add(node);
            _nodeCache.Add(id.Id, nodeTransform.gameObject);

            // children that are primitives get put in a mesh
            FilterPrimitives(nodeTransform, out GameObject[] primitives, out GameObject[] nonPrimitives);
            // associate unity meshes with gltf mesh id
            foreach (var prim in primitives)
            {
                if (prim.TryGetComponent<SkinnedMeshRenderer>(out var smr))
                {
                    _primOwner[new PrimKey { Mesh = smr.sharedMesh, Material = smr.sharedMaterial }] = node.Mesh;
                }
                else
                {
                    var filter = prim.GetComponent<MeshFilter>();
                    var meshRenderer = prim.GetComponent<MeshRenderer>();
                    _primOwner[new PrimKey { Mesh = filter.sharedMesh, Material = meshRenderer.sharedMaterial }] = node.Mesh;
                }

            }

            if (ContainsValidRenderer(nodeTransform.gameObject))
                node.Mesh = ExportMesh(nodeTransform.name, new GameObject[] { nodeTransform.gameObject });

            node.Children = new List<NodeId>(nonPrimitives.Length + primitives.Length);
            for (int i = 0; i < nodeTransform.childCount; i++)
            {
                var child = nodeTransform.GetChild(i);
                if (ExportInActiveGameObject || child.gameObject.activeSelf)
                    node.Children.Add(ExportNode(child));
            }

            return id;
        }
    }
}