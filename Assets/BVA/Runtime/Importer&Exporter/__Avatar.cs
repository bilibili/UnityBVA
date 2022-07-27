using GLTF.Schema.BVA;
using BVA.Component;
using GLTF.Schema;
using UnityEngine;
using BVA.Extensions;
using System;
using System.Collections.Generic;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public async void ConstructAvatar(Node node, GameObject nodeObj)
        {
            if (_gltfRoot.Skins == null || _gltfRoot.Skins.Count == 0)
                return;

            if (node.Skin != null)
                SetupBones(node.Skin.Value, nodeObj.GetComponent<SkinnedMeshRenderer>());
            // import avatar 
            if (hasValidExtension(node, BVA_humanoid_avatarExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_humanoid_avatarExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_humanoid_avatarExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_humanoid_avatarExtensionFactory)} failed");
                var avatar = AssetManager.avatarLoader.GetAvatar(impl.id);
                if (avatar == null)
                {
                    var avatarExt = _gltfRoot.Extensions.Avatars[impl.id.Id];
                    if (avatarExt != null)
                    {
                        avatar = AssetManager.avatarLoader.AddAvatar(avatarExt.human, nodeObj.transform);
                    }
                }

                var animator = nodeObj.GetOrAddComponent<Animator>();
                animator.avatar = avatar ?? throw new Exception($"create Avatar failed on {nodeObj.name}");
                animator.avatar.name = animator.name;
                animator.applyRootMotion = true;

                var head = animator.GetBoneTransform(HumanBodyBones.Head);
                foreach (var smr in animator.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    smr.probeAnchor = head;
                }
            }
            if (hasValidExtension(node, BVA_humanoid_dressExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_humanoid_dressExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_humanoid_dressExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_humanoid_dressExtensionFactory)} failed");
                var gltfDress = impl.id.Value.gltfObject;
                var dressUp = nodeObj.AddComponent<AvatarDressUpConfig>();
                dressUp.dressUpConfigs = new List<DressUpConfig>();
                foreach (var config in gltfDress.dressUpConfigs)
                {

                    var dressUpConfig = new DressUpConfig() { name = config.name, rendererConfigs = new List<RendererMaterialConfig>() };
                    foreach (var renderCfg in config.rendererConfigs)
                    {
                        Renderer renderer = _assetCache.NodeCache[renderCfg.node].GetComponent<Renderer>();
                        Material[] materials = new Material[renderCfg.materials.Count];
                        for (int i = 0; i < renderCfg.materials.Count; i++)
                        {
                            materials[i] = await LoadMaterial(new MaterialId() { Id = renderCfg.materials[i], Root = _gltfRoot });
                        }
                        dressUpConfig.rendererConfigs.Add(new RendererMaterialConfig() { renderer = renderer, materials = materials });
                    }
                    dressUp.dressUpConfigs.Add(dressUpConfig);
                }
                if (dressUp.dressUpConfigs.Count > 0)
                    dressUp.SwitchDress(0);
            }

            if (hasValidExtension(node, BVA_humanoid_accessoryExtensionFactory.EXTENSION_NAME))
            {
                IExtension ext = node.Extensions[BVA_humanoid_accessoryExtensionFactory.EXTENSION_NAME];
                var impl = (BVA_humanoid_accessoryExtensionFactory)ext;
                if (impl == null) throw new Exception($"cast {nameof(BVA_humanoid_accessoryExtensionFactory)} failed");

                var accessoryConfigs = impl.id.Value.accessoryConfigs;
                foreach (var config in accessoryConfigs)
                {
                    config.gameObject = _assetCache.NodeCache[config.node];
                }

                var dressUp = nodeObj.AddComponent<AvatarAccessoryConfig>();
                dressUp.accessories = accessoryConfigs;
            }
        }
    }

    public partial class GLTFSceneExporter
    {
        private static bool HasValidHumanAvatar(Animator animator)
        {
            return animator != null && animator.avatar != null && animator.avatar.isHuman;
        }
        private GltfAvatar GetHumanoidFromAvatar(Avatar avatar, Animator animator)
        {
            GltfAvatar human = new GltfAvatar();
            human.armStretch = avatar.humanDescription.armStretch;
            human.legStretch = avatar.humanDescription.legStretch;
            human.legStretch = avatar.humanDescription.legStretch;
            human.upperArmTwist = avatar.humanDescription.upperArmTwist;
            human.lowerArmTwist = avatar.humanDescription.lowerArmTwist;
            human.upperLegTwist = avatar.humanDescription.upperLegTwist;
            human.lowerLegTwist = avatar.humanDescription.lowerLegTwist;
            human.feetSpacing = avatar.humanDescription.feetSpacing;
            human.hasTranslationDoF = avatar.humanDescription.hasTranslationDoF;
            foreach (var v in avatar.humanDescription.human)
            {
                HumanLimit limit = v.limit;
                GlTFHumanoidBone bone = new GlTFHumanoidBone();
                bone.axisLength = limit.axisLength;
                bone.center = limit.center.ToGltfVector3Raw();
                bone.min = limit.min.ToGltfVector3Raw();
                bone.max = limit.max.ToGltfVector3Raw();
                bone.useDefaultValues = limit.useDefaultValues;
                bone.bone = v.humanName; //convert first character to low case,gltf format require it 
                var transform = animator.transform.DeepFindChild(v.boneName);
                if (transform != null)
                {
                    bone.node = _nodeCache.GetId(transform.gameObject);
                }
                else
                {
                    LogPool.ExportLogger.LogError(LogPart.Avatar, $"{v.boneName} not available");
                }
                human.humanBones.Add(bone);
            }
            return human;
        }


        private AvatarId ExportAvatar(Animator animator)
        {
            var avatar = animator.avatar;
            if (!avatar.isHuman || !avatar.isValid) { throw new System.Exception($"{avatar.name} is not a valid avatar"); }
            var human = GetHumanoidFromAvatar(avatar, animator);
            BVA_humanoid_avatarExtension ext = new BVA_humanoid_avatarExtension(human);

            var id = new AvatarId
            {
                Id = _root.Extensions.Avatars.Count,
                Root = _root
            };
            _root.Extensions.AddAvatar(ext);
            return id;
        }
        private void ExportAvatar()
        {
            foreach (var animator in _animators)
            {
                if (HasValidHumanAvatar(animator))
                {
                    int nodeId = _nodeCache.GetId(animator.gameObject);
                    Node node = _root.Nodes[nodeId];
                    var avatarId = ExportAvatar(animator);
                    node.AddExtension(_root, BVA_humanoid_avatarExtensionFactory.EXTENSION_NAME, new BVA_humanoid_avatarExtensionFactory(avatarId), RequireExtensions);
                }

                // export dress
                var dressUp = animator.GetComponent<AvatarDressUpConfig>();
                if (dressUp != null && dressUp.dressUpConfigs.Count > 1)
                {
                    int nodeId = _nodeCache.GetId(animator.gameObject);
                    Node node = _root.Nodes[nodeId];
                    var dressId = ExportDress(dressUp);
                    node.AddExtension(_root, BVA_humanoid_dressExtensionFactory.EXTENSION_NAME, new BVA_humanoid_dressExtensionFactory(dressId), RequireExtensions);
                }

                // export accessory
                var accessory = animator.GetComponent<AvatarAccessoryConfig>();
                if (accessory != null && accessory.accessories.Count > 0)
                {
                    int nodeId = _nodeCache.GetId(animator.gameObject);
                    Node node = _root.Nodes[nodeId];
                    var accessoryId = ExportAccessory(accessory);
                    node.AddExtension(_root, BVA_humanoid_accessoryExtensionFactory.EXTENSION_NAME, new BVA_humanoid_accessoryExtensionFactory(accessoryId), RequireExtensions);
                }
            }
        }

        private GltfDress ConvertDress(AvatarDressUpConfig dressUp)
        {
            int nodeId = _nodeCache.GetId(dressUp.animator.gameObject);
            GltfDress dress = new GltfDress() { dressUpConfigs = new List<GltfDress.DressUpConfig>() };
            foreach (var unityStruct in dressUp.dressUpConfigs)
            {
                var rendererCfg = new List<GltfDress.GltfRendererMaterialConfig>();
                foreach (var cfg in unityStruct.rendererConfigs)
                {
                    var matList = new List<int>();
                    rendererCfg.Add(new GltfDress.GltfRendererMaterialConfig() { node = _nodeCache.GetId(cfg.renderer.gameObject), materials = matList });
                    foreach (var material in cfg.materials)
                        matList.Add(ExportMaterial(material).Id);
                }
                dress.dressUpConfigs.Add(new GltfDress.DressUpConfig() { name = unityStruct.name, rendererConfigs = rendererCfg });
            }
            return dress;
        }

        private DressId ExportDress(AvatarDressUpConfig dressUp)
        {
            var gltfDress = ConvertDress(dressUp);
            BVA_humanoid_dressExtension ext = new BVA_humanoid_dressExtension(gltfDress);

            var id = new DressId
            {
                Id = _root.Extensions.Dresses.Count,
                Root = _root
            };
            _root.Extensions.AddDress(ext);
            return id;
        }

        private AccessoryId ExportAccessory(AvatarAccessoryConfig accessory)
        {
            for (int i = 0; i < accessory.accessories.Count; i++)
            {
                AccessoryConfig item = accessory.accessories[i];
                item.node = _nodeCache.GetId(item.gameObject);
            }

            BVA_humanoid_accessoryExtension ext = new BVA_humanoid_accessoryExtension(accessory.accessories);
            var id = new AccessoryId
            {
                Id = _root.Extensions.Accessories.Count,
                Root = _root
            };
            _root.Extensions.AddAccessory(ext);
            return id;
        }
    }
}