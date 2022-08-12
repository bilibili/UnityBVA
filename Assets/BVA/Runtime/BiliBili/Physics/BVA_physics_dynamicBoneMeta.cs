using ADBRuntime.Mono;
using ADBRuntime;
using GLTF.Extensions;
using BVA.Extensions;
using Newtonsoft.Json.Linq;
using BVA;
using Newtonsoft.Json;
using UnityEngine;
using System;
using BVA.Cache;

namespace GLTF.Schema.BVA
{
    public class BVA_physics_dynamicBoneMeta
    {
        public string typeName;
        
        public ADBChainProcessorMeta chainProcessorMeta;
        public ADBColliderMeta colliderMeta;
        public ADBControllerMeta controllerMeta;

        public BVA_physics_dynamicBoneMeta(IADBPhysicMonoComponent ADBPhysicer, NodeCache nodeCache)
        {
            switch (ADBPhysicer.GetType().Name)
            {
                case nameof(ADBChainProcessor):
                    typeName = nameof(ADBChainProcessorMeta);
                    var chainProcessor = ADBPhysicer as ADBChainProcessor;
                    chainProcessorMeta = new ADBChainProcessorMeta(chainProcessor, nodeCache);
                    break;
                case nameof(ADBColliderReader):
                    typeName = nameof(ADBColliderMeta);
                    var colliderReader = ADBPhysicer as ADBColliderReader;
                    colliderMeta = new ADBColliderMeta(colliderReader);
                    break;
                case nameof(ADBRuntimeController):
                    typeName = nameof(ADBControllerMeta);
                    var runtimeController = ADBPhysicer as ADBRuntimeController;
                    controllerMeta = new ADBControllerMeta(runtimeController);
                    break;
                case nameof(ADBRuntimePoint):
                    typeName = nameof(ADBRuntimePoint);
                    break;
                default:
                    throw new UnassignedReferenceException();
            }

        }
        public BVA_physics_dynamicBoneMeta(ADBChainProcessorMeta chainProcessorMeta)
        {
            typeName = nameof(ADBChainProcessorMeta);
            this.chainProcessorMeta = chainProcessorMeta;
        }
        public BVA_physics_dynamicBoneMeta(ADBColliderMeta colliderMeta)
        {
            typeName = nameof(ADBColliderMeta);
            this.colliderMeta = colliderMeta;
        }
        public BVA_physics_dynamicBoneMeta(ADBControllerMeta controllerMeta)
        {
            typeName = nameof(ADBControllerMeta);
            this.controllerMeta = controllerMeta;
        }
        public BVA_physics_dynamicBoneMeta()
        { }
        internal void Deserialize(GameObject nodeObj, AssetCache assetCache )
        {
            switch (typeName)
            {
                case nameof(ADBControllerMeta):
                    controllerMeta.Deserialize(nodeObj);
                    break;
                case nameof(ADBColliderMeta):
                    colliderMeta.Deserialize(nodeObj);
                    break;
                case nameof(ADBChainProcessorMeta):
                    chainProcessorMeta.Deserialize(nodeObj, assetCache);

                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public JObject SerializeData()
        {
            JObject jo = new JObject();
            switch (typeName)
            {
                case nameof(ADBControllerMeta):
                    jo.Add(typeName, SerializeControllerData());
                    break;
                case nameof(ADBColliderMeta):
                    jo.Add(typeName, SerializeColliderData());
                    break;
                case nameof(ADBChainProcessorMeta):
                    jo.Add(typeName, SerializeChainProcessorData());
                    break;
                default:

                    throw new NotImplementedException();
            }
            return jo;
        }
        public static BVA_physics_dynamicBoneMeta Deserialize(GLTFRoot root, JsonReader reader)
        {

            BVA_physics_dynamicBoneMeta result = null;
            if ( reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();
                while (reader.Read() && reader.TokenType == JsonToken.StartObject)//OYM:read object data
                {
                    switch (curProp)
                    {
                        case nameof(ADBChainProcessorMeta):

                            result = DeserializeChainProcessorData(root, reader);

                            break;
                        case nameof(ADBColliderMeta):

                            result = DeserializeColliderData(root, reader);

                            break;
                        case nameof(ADBControllerMeta):

                            result = DeserializeControllerData(root, reader);

                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            return result;
        }
        private JObject SerializeColliderData()
        {
            JObject jo = new JObject();
            jo.Add(nameof(ADBColliderMeta.collideFunc), colliderMeta.collideFunc.ToString());
            jo.Add(nameof(ADBColliderMeta.colliderChoice), (int)colliderMeta.colliderChoice);
            return jo;
        }
        private static BVA_physics_dynamicBoneMeta DeserializeColliderData(GLTFRoot root, JsonReader reader)
        {
            ADBColliderMeta newMeta = new ADBColliderMeta();
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    break;
                }
                var curProp = reader.Value.ToString();
                switch (curProp)
                {
                    case nameof(ADBColliderMeta.collideFunc):
                        newMeta.collideFunc = reader.ReadStringEnum<CollideFunc>();
                        break;
                    case nameof(ADBColliderMeta.colliderChoice):
                        newMeta.colliderChoice = (ColliderChoice)reader.ReadAsInt32();
                        break;
                }
            }
            return new BVA_physics_dynamicBoneMeta(newMeta);
        }

        private JObject SerializeControllerData()
        {
            JObject jo = new JObject();
            //OYM:4 bool
            jo.Add(nameof(ADBControllerMeta.isRunAsync), controllerMeta.isRunAsync);
            jo.Add(nameof(ADBControllerMeta.isParallel), controllerMeta.isParallel);
            jo.Add(nameof(ADBControllerMeta.isDebug), controllerMeta.isDebug);
            jo.Add(nameof(ADBControllerMeta.isOptimize), controllerMeta.isOptimize);
            //OYM:1 int
            jo.Add(nameof(ADBControllerMeta.iteration), controllerMeta.iteration);
            //OYM:2 float
            jo.Add(nameof(ADBControllerMeta.bufferTime), controllerMeta.bufferTime);
            jo.Add(nameof(ADBControllerMeta.windForceScale), controllerMeta.windForceScale);
            //OYM:2 enum
            jo.Add(nameof(ADBControllerMeta.updateMode), controllerMeta.updateMode.ToString());
            jo.Add(nameof(ADBControllerMeta.colliderCollisionType), controllerMeta.colliderCollisionType.ToString());
            return jo;
        }
        private static BVA_physics_dynamicBoneMeta DeserializeControllerData(GLTFRoot root, JsonReader reader)
        {
            ADBControllerMeta newMeta = new ADBControllerMeta();

            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();
                switch (curProp)
                {
                    case nameof(ADBControllerMeta.isRunAsync):
                        newMeta.isRunAsync = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBControllerMeta.isParallel):
                        newMeta.isParallel = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBControllerMeta.isDebug):
                        newMeta.isDebug = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBControllerMeta.isOptimize):
                        newMeta.isOptimize = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBControllerMeta.iteration):
                        newMeta.iteration = reader.ReadAsInt32().Value;
                        break;
                    case nameof(ADBControllerMeta.bufferTime):
                        newMeta.bufferTime = reader.ReadAsFloat();
                        break;
                    case nameof(ADBControllerMeta.windForceScale):
                        newMeta.windForceScale = reader.ReadAsFloat();
                        break;
                    case nameof(ADBControllerMeta.updateMode):
                        newMeta.updateMode = reader.ReadStringEnum<UpdateMode>();
                        break;
                    case nameof(ADBControllerMeta.colliderCollisionType):
                        newMeta.colliderCollisionType = reader.ReadStringEnum<ColliderCollisionType>();
                        break;
                }
            }
            return new BVA_physics_dynamicBoneMeta(newMeta);
        }
        public JObject SerializeChainProcessorData()
        {
            JObject jo = new JObject();

            ADBPhysicsSetting physicsSetting = chainProcessorMeta.physicsSetting;
            int[] transformIndex = chainProcessorMeta.transformIndex;
            int[] pointParentIndex = chainProcessorMeta.pointParentIndex;
            int[] pointColliderMasks = chainProcessorMeta.pointColliderMasks;
            float[] pointHitRadiuss = chainProcessorMeta.pointHitRadiuss;
            string keyWord = chainProcessorMeta.keyWord;

            jo.Add(nameof(ADBChainProcessorMeta.keyWord), keyWord);

            var transformIndexJArray = new JArray(transformIndex);
            jo.Add(new JProperty(nameof(ADBChainProcessorMeta.transformIndex), transformIndexJArray));

            var pointParentIndexJArray = new JArray(pointParentIndex);
            jo.Add(new JProperty(nameof(ADBChainProcessorMeta.pointParentIndex), pointParentIndexJArray));

            if (pointColliderMasks!=null)
            {
                var pointColliderMasksJArray = new JArray(pointColliderMasks);
                jo.Add(new JProperty(nameof(ADBChainProcessorMeta.pointColliderMasks), pointColliderMasksJArray));

                var pointHitRadiussJArray = new JArray(pointHitRadiuss);
                jo.Add(new JProperty(nameof(ADBChainProcessorMeta.pointHitRadiuss), pointHitRadiussJArray));
            }

            //OYM:1
            jo.Add(nameof(ADBPhysicsSetting.name), physicsSetting.name);
            //OYM:10
            jo.Add(nameof(ADBPhysicsSetting.isfrictionCurve), physicsSetting.isfrictionCurve);
            jo.Add(nameof(ADBPhysicsSetting.isaddForceScaleCurve), physicsSetting.isaddForceScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isgravityScaleCurve), physicsSetting.isgravityScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.ismoveInertCurve), physicsSetting.ismoveInertCurve);
            jo.Add(nameof(ADBPhysicsSetting.isdampingCurve), physicsSetting.isdampingCurve);
            jo.Add(nameof(ADBPhysicsSetting.iselasticityCurve), physicsSetting.iselasticityCurve);
            jo.Add(nameof(ADBPhysicsSetting.isvelocityIncreaseCurve), physicsSetting.isvelocityIncreaseCurve);
            jo.Add(nameof(ADBPhysicsSetting.isstiffnessWorldCurve), physicsSetting.isstiffnessWorldCurve);
            jo.Add(nameof(ADBPhysicsSetting.isstiffnessLocalCurve), physicsSetting.isstiffnessLocalCurve);
            jo.Add(nameof(ADBPhysicsSetting.islengthLimitForceScaleCurve), physicsSetting.islengthLimitForceScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.iselasticityVelocityCurve), physicsSetting.iselasticityVelocityCurve);
            //OYM:12
            jo.Add(nameof(ADBPhysicsSetting.isstructuralShrinkVerticalScaleCurve), physicsSetting.isstructuralShrinkVerticalScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isstructuralStretchVerticalScaleCurve), physicsSetting.isstructuralStretchVerticalScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isstructuralShrinkHorizontalScaleCurve), physicsSetting.isstructuralShrinkHorizontalScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isstructuralStretchHorizontalScaleCurve), physicsSetting.isstructuralStretchHorizontalScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isshearShrinkScaleCurve), physicsSetting.isshearShrinkScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isshearStretchScaleCurve), physicsSetting.isshearStretchScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isbendingShrinkVerticalScaleCurve), physicsSetting.isbendingShrinkVerticalScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isbendingStretchVerticalScaleCurve), physicsSetting.isbendingStretchVerticalScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isbendingShrinkHorizontalScaleCurve), physicsSetting.isbendingShrinkHorizontalScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.isbendingStretchHorizontalScaleCurve), physicsSetting.isbendingStretchHorizontalScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.iscircumferenceShrinkScaleCurve), physicsSetting.iscircumferenceShrinkScaleCurve);
            jo.Add(nameof(ADBPhysicsSetting.iscircumferenceStretchScaleCurve), physicsSetting.iscircumferenceStretchScaleCurve);
            //OYM:1
            jo.Add(nameof(ADBPhysicsSetting.ispointRadiuCurve), physicsSetting.ispointRadiuCurve);
            //OYM:10
            jo.Add(nameof(ADBPhysicsSetting.frictionCurve), physicsSetting.frictionCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.addForceScaleCurve), physicsSetting.addForceScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.gravityScaleCurve), physicsSetting.gravityScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.moveInertCurve), physicsSetting.moveInertCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.dampingCurve), physicsSetting.dampingCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.elasticityCurve), physicsSetting.elasticityCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.elasticityVelocityCurve), physicsSetting.elasticityVelocityCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.velocityIncreaseCurve), physicsSetting.velocityIncreaseCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.stiffnessWorldCurve), physicsSetting.stiffnessWorldCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.stiffnessLocalCurve), physicsSetting.stiffnessLocalCurve.Serialize());

            jo.Add(nameof(ADBPhysicsSetting.vrmStiffnessForceCurve), physicsSetting.vrmStiffnessForceCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.value3Curve), physicsSetting.value3Curve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.value4Curve), physicsSetting.value4Curve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.value5Curve), physicsSetting.value5Curve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.value6Curve), physicsSetting.value6Curve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.value7Curve), physicsSetting.value7Curve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.value8Curve), physicsSetting.value8Curve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.value9Curve), physicsSetting.value9Curve.Serialize());

            jo.Add(nameof(ADBPhysicsSetting.pointRadiuCurve), physicsSetting.pointRadiuCurve.Serialize());



            //OYM:12
            jo.Add(nameof(ADBPhysicsSetting.structuralShrinkVerticalScaleCurve), physicsSetting.structuralShrinkVerticalScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.structuralStretchVerticalScaleCurve), physicsSetting.structuralStretchVerticalScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.structuralShrinkHorizontalScaleCurve), physicsSetting.structuralShrinkHorizontalScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.structuralStretchHorizontalScaleCurve), physicsSetting.structuralStretchHorizontalScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.shearShrinkScaleCurve), physicsSetting.shearShrinkScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.shearStretchScaleCurve), physicsSetting.shearStretchScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.bendingShrinkVerticalScaleCurve), physicsSetting.bendingShrinkVerticalScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.bendingStretchVerticalScaleCurve), physicsSetting.bendingStretchVerticalScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.bendingShrinkHorizontalScaleCurve), physicsSetting.bendingShrinkHorizontalScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.bendingStretchHorizontalScaleCurve), physicsSetting.bendingStretchHorizontalScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.circumferenceShrinkScaleCurve), physicsSetting.circumferenceShrinkScaleCurve.Serialize());
            jo.Add(nameof(ADBPhysicsSetting.circumferenceStretchScaleCurve), physicsSetting.circumferenceStretchScaleCurve.Serialize());

            //OYM:9
            jo.Add(nameof(ADBPhysicsSetting.frictionValue), physicsSetting.frictionValue);
            jo.Add(nameof(ADBPhysicsSetting.addForceScaleValue), physicsSetting.addForceScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.gravityScaleValue), physicsSetting.gravityScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.moveInertValue), physicsSetting.moveInertValue);
            jo.Add(nameof(ADBPhysicsSetting.velocityIncreaseValue), physicsSetting.velocityIncreaseValue);
            jo.Add(nameof(ADBPhysicsSetting.dampingValue), physicsSetting.dampingValue);
            jo.Add(nameof(ADBPhysicsSetting.elasticityValue), physicsSetting.elasticityValue);
            jo.Add(nameof(ADBPhysicsSetting.elasticityVelocityValue), physicsSetting.elasticityVelocityValue);
            jo.Add(nameof(ADBPhysicsSetting.stiffnessWorldValue), physicsSetting.stiffnessWorldValue);
            jo.Add(nameof(ADBPhysicsSetting.stiffnessLocalValue), physicsSetting.stiffnessLocalValue);
            jo.Add(nameof(ADBPhysicsSetting.lengthLimitForceScaleValue), physicsSetting.lengthLimitForceScaleValue);

            jo.Add(nameof(ADBPhysicsSetting.vrmStiffnessForceValue), physicsSetting.vrmStiffnessForceValue);
            jo.Add(nameof(ADBPhysicsSetting.value3Value), physicsSetting.value3Value);
            jo.Add(nameof(ADBPhysicsSetting.value4Value), physicsSetting.value4Value);
            jo.Add(nameof(ADBPhysicsSetting.value5Value), physicsSetting.value5Value);
            jo.Add(nameof(ADBPhysicsSetting.value6Value), physicsSetting.value6Value);
            jo.Add(nameof(ADBPhysicsSetting.value7Value), physicsSetting.value7Value);
            jo.Add(nameof(ADBPhysicsSetting.value8Value), physicsSetting.value8Value);
            jo.Add(nameof(ADBPhysicsSetting.value9Value), physicsSetting.value9Value);


            //OYM:12
            jo.Add(nameof(ADBPhysicsSetting.structuralShrinkVerticalScaleValue), physicsSetting.structuralShrinkVerticalScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.structuralStretchVerticalScaleValue), physicsSetting.structuralStretchVerticalScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.structuralShrinkHorizontalScaleValue), physicsSetting.structuralShrinkHorizontalScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.structuralStretchHorizontalScaleValue), physicsSetting.structuralStretchHorizontalScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.shearShrinkScaleValue), physicsSetting.shearShrinkScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.shearStretchScaleValue), physicsSetting.shearStretchScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.bendingShrinkVerticalScaleValue), physicsSetting.bendingShrinkVerticalScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.bendingStretchVerticalScaleValue), physicsSetting.bendingStretchVerticalScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.bendingShrinkHorizontalScaleValue), physicsSetting.bendingShrinkHorizontalScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.bendingStretchHorizontalScaleValue), physicsSetting.bendingStretchHorizontalScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.circumferenceShrinkScaleValue), physicsSetting.circumferenceShrinkScaleValue);
            jo.Add(nameof(ADBPhysicsSetting.circumferenceStretchScaleValue), physicsSetting.circumferenceStretchScaleValue);

            //OYM:5

            jo.Add(nameof(ADBPhysicsSetting.isComputeVirtual), physicsSetting.isComputeVirtual);
            jo.Add(nameof(ADBPhysicsSetting.isAllowComputeOtherConstraint), physicsSetting.isAllowComputeOtherConstraint);
            jo.Add(nameof(ADBPhysicsSetting.virtualPointAxisLength), physicsSetting.virtualPointAxisLength);
            jo.Add(nameof(ADBPhysicsSetting.ForceLookDown), physicsSetting.ForceLookDown);
            jo.Add(nameof(ADBPhysicsSetting.isFixedPointFreezeRotation), physicsSetting.isFixedPointFreezeRotation);
            //OYM:2
            jo.Add(nameof(ADBPhysicsSetting.isAutoComputeWeight), physicsSetting.isAutoComputeWeight);
            jo.Add(nameof(ADBPhysicsSetting.weightCurve), physicsSetting.weightCurve.Serialize());

            //OYM:10

            jo.Add(nameof(ADBPhysicsSetting.isComputeStructuralVertical), physicsSetting.isComputeStructuralVertical);
            jo.Add(nameof(ADBPhysicsSetting.isComputeStructuralHorizontal), physicsSetting.isComputeStructuralHorizontal);
            jo.Add(nameof(ADBPhysicsSetting.isComputeShear), physicsSetting.isComputeShear);
            jo.Add(nameof(ADBPhysicsSetting.isComputeBendingVertical), physicsSetting.isComputeBendingVertical);
            jo.Add(nameof(ADBPhysicsSetting.isComputeBendingHorizontal), physicsSetting.isComputeBendingHorizontal);
            jo.Add(nameof(ADBPhysicsSetting.isComputeCircumference), physicsSetting.isComputeCircumference);
            jo.Add(nameof(ADBPhysicsSetting.isCollideStructuralVertical), physicsSetting.isCollideStructuralVertical);
            jo.Add(nameof(ADBPhysicsSetting.isCollideStructuralHorizontal), physicsSetting.isCollideStructuralHorizontal);
            jo.Add(nameof(ADBPhysicsSetting.isCollideShear), physicsSetting.isCollideShear);
            jo.Add(nameof(ADBPhysicsSetting.isLoopRootPoints), physicsSetting.isLoopRootPoints);
            //OYM:4
            jo.Add(nameof(ADBPhysicsSetting.isDebugDraw), physicsSetting.isDebugDraw);
            jo.Add(nameof(ADBPhysicsSetting.isFixGravityAxis), physicsSetting.isFixGravityAxis);
            jo.Add(nameof(ADBPhysicsSetting.gravity), physicsSetting.gravity.ToGltfVector3Raw().ToJArray());
            //OYM:1
            jo.Add(nameof(ADBPhysicsSetting.colliderChoice), (int)physicsSetting.colliderChoice);

            return jo;
        }
        public static BVA_physics_dynamicBoneMeta DeserializeChainProcessorData(GLTFRoot root, JsonReader reader)
        {
            /*            int m_transformID = -1;
                        List<string> m_whiteKeyWord = null;
                        List<string> m_blackKeyWord = null;
                       */
            int[] transformIndex = null;
            int[] pointParentIndex = null;
            int[] pointColliderMasks = null;
            float[] pointHitRadiuss = null;
            string keyWord = null;
            ADBPhysicsSetting physicsSetting = (ADBPhysicsSetting)ScriptableObject.CreateInstance(typeof(ADBPhysicsSetting));
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();
                switch (curProp)
                {
                   case nameof(ADBChainProcessorMeta.keyWord):
                        keyWord = reader.ReadAsString();
                        break;
                    case nameof(ADBChainProcessorMeta.transformIndex):
                        transformIndex = reader.ReadInt32List().ToArray();
                        break;
                    case nameof(ADBChainProcessorMeta.pointParentIndex):
                        pointParentIndex = reader.ReadInt32List().ToArray();
                        break;
                    case nameof(ADBChainProcessorMeta.pointColliderMasks):
                        pointColliderMasks = reader.ReadInt32List().ToArray();
                        break;
                    case nameof(ADBChainProcessorMeta.pointHitRadiuss):
                        pointHitRadiuss = reader.ReadFloatList().ToArray();
                        break;
                    /*
                case nameof(blackKeyWord)://OYM:blackList
                    m_blackKeyWord = reader.ReadStringList();
                    break;*/

                    case nameof(ADBPhysicsSetting.name):
                        physicsSetting.name = reader.ReadAsString();
                        break;
                    //OYM:10
                    case nameof(ADBPhysicsSetting.isfrictionCurve):
                        physicsSetting.isfrictionCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isaddForceScaleCurve):
                        physicsSetting.isaddForceScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isgravityScaleCurve):
                        physicsSetting.isgravityScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.ismoveInertCurve):
                        physicsSetting.ismoveInertCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isdampingCurve):
                        physicsSetting.isdampingCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.iselasticityCurve):
                        physicsSetting.iselasticityCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isvelocityIncreaseCurve):
                        physicsSetting.isvelocityIncreaseCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isstiffnessLocalCurve):
                        physicsSetting.isstiffnessLocalCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isstiffnessWorldCurve):
                        physicsSetting.isstiffnessWorldCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.islengthLimitForceScaleCurve):
                        physicsSetting.islengthLimitForceScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.iselasticityVelocityCurve):
                        physicsSetting.iselasticityVelocityCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isstructuralShrinkVerticalScaleCurve):
                        physicsSetting.isstructuralShrinkVerticalScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isstructuralStretchVerticalScaleCurve):
                        physicsSetting.isstructuralStretchVerticalScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isstructuralShrinkHorizontalScaleCurve):
                        physicsSetting.isstructuralShrinkHorizontalScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isstructuralStretchHorizontalScaleCurve):
                        physicsSetting.isstructuralStretchHorizontalScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isshearShrinkScaleCurve):
                        physicsSetting.isshearShrinkScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isshearStretchScaleCurve):
                        physicsSetting.isshearStretchScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isbendingShrinkVerticalScaleCurve):
                        physicsSetting.isbendingShrinkVerticalScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isbendingStretchVerticalScaleCurve):
                        physicsSetting.isbendingStretchVerticalScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isbendingShrinkHorizontalScaleCurve):
                        physicsSetting.isbendingShrinkHorizontalScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isbendingStretchHorizontalScaleCurve):
                        physicsSetting.isbendingStretchHorizontalScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.iscircumferenceShrinkScaleCurve):
                        physicsSetting.iscircumferenceShrinkScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.iscircumferenceStretchScaleCurve):
                        physicsSetting.iscircumferenceStretchScaleCurve = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.ispointRadiuCurve):
                        physicsSetting.ispointRadiuCurve = reader.ReadAsBoolean().Value;
                        break;
                    //OYM:10 cruves about process
                    case nameof(ADBPhysicsSetting.frictionCurve):
                        physicsSetting.frictionCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.addForceScaleCurve):
                        physicsSetting.addForceScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.gravityScaleCurve):
                        physicsSetting.gravityScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.moveInertCurve):
                        physicsSetting.moveInertCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.dampingCurve):
                        physicsSetting.dampingCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.elasticityCurve):
                        physicsSetting.elasticityCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.velocityIncreaseCurve):
                        physicsSetting.velocityIncreaseCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.stiffnessWorldCurve):
                        physicsSetting.stiffnessWorldCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.stiffnessLocalCurve):
                        physicsSetting.stiffnessLocalCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;

                    case nameof(ADBPhysicsSetting.lengthLimitForceScaleCurve):
                        physicsSetting.lengthLimitForceScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.elasticityVelocityCurve):
                        physicsSetting.elasticityVelocityCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;

                    case nameof(ADBPhysicsSetting.vrmStiffnessForceCurve):
                        physicsSetting.vrmStiffnessForceCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.value3Curve):
                        physicsSetting.value3Curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.value4Curve):
                        physicsSetting.value4Curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.value5Curve):
                        physicsSetting.value5Curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.value6Curve):
                        physicsSetting.value6Curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.value7Curve):
                        physicsSetting.value7Curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.value8Curve):
                        physicsSetting.value8Curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.value9Curve):
                        physicsSetting.value9Curve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;

                    case nameof(ADBPhysicsSetting.pointRadiuCurve):
                        physicsSetting.pointRadiuCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;

                    //OYM:12 cruves about constraint
                    case nameof(ADBPhysicsSetting.structuralShrinkVerticalScaleCurve):
                        physicsSetting.structuralShrinkVerticalScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.structuralStretchVerticalScaleCurve):
                        physicsSetting.structuralStretchVerticalScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.structuralShrinkHorizontalScaleCurve):
                        physicsSetting.structuralShrinkHorizontalScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.structuralStretchHorizontalScaleCurve):
                        physicsSetting.structuralStretchHorizontalScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.shearShrinkScaleCurve):
                        physicsSetting.shearShrinkScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.shearStretchScaleCurve):
                        physicsSetting.shearStretchScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.bendingShrinkVerticalScaleCurve):
                        physicsSetting.bendingShrinkVerticalScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.bendingStretchVerticalScaleCurve):
                        physicsSetting.bendingStretchVerticalScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.bendingShrinkHorizontalScaleCurve):
                        physicsSetting.bendingShrinkHorizontalScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.bendingStretchHorizontalScaleCurve):
                        physicsSetting.bendingStretchHorizontalScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.circumferenceShrinkScaleCurve):
                        physicsSetting.circumferenceShrinkScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    case nameof(ADBPhysicsSetting.circumferenceStretchScaleCurve):
                        physicsSetting.circumferenceStretchScaleCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;
                    //OYM:9 value about process
                    case nameof(ADBPhysicsSetting.addForceScaleValue):
                        physicsSetting.addForceScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.gravityScaleValue):
                        physicsSetting.gravityScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.moveInertValue):
                        physicsSetting.moveInertValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.velocityIncreaseValue):
                        physicsSetting.velocityIncreaseValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.dampingValue):
                        physicsSetting.dampingValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.elasticityValue):
                        physicsSetting.elasticityValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.stiffnessWorldValue):
                        physicsSetting.stiffnessWorldValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.stiffnessLocalValue):
                        physicsSetting.stiffnessLocalValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.frictionValue):
                        physicsSetting.frictionValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.lengthLimitForceScaleValue):
                        physicsSetting.lengthLimitForceScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.elasticityVelocityValue):
                        physicsSetting.elasticityVelocityValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.vrmStiffnessForceValue):
                        physicsSetting.vrmStiffnessForceValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.value3Value):
                        physicsSetting.value3Value = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.value4Value):
                        physicsSetting.value4Value = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.value5Value):
                        physicsSetting.value5Value = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.value6Value):
                        physicsSetting.value6Value = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.value7Value):
                        physicsSetting.value7Value = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.value8Value):
                        physicsSetting.value8Value = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.value9Value):
                        physicsSetting.value9Value = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.pointRadiuValue):
                        physicsSetting.pointRadiuValue = reader.ReadAsFloat();
                        break;
                    //OYM:12 cruves about constraint
                    case nameof(ADBPhysicsSetting.structuralShrinkVerticalScaleValue):
                        physicsSetting.structuralShrinkVerticalScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.structuralStretchVerticalScaleValue):
                        physicsSetting.structuralStretchVerticalScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.structuralShrinkHorizontalScaleValue):
                        physicsSetting.structuralShrinkHorizontalScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.structuralStretchHorizontalScaleValue):
                        physicsSetting.structuralStretchHorizontalScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.shearShrinkScaleValue):
                        physicsSetting.shearShrinkScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.shearStretchScaleValue):
                        physicsSetting.shearStretchScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.bendingShrinkVerticalScaleValue):
                        physicsSetting.bendingShrinkVerticalScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.bendingStretchVerticalScaleValue):
                        physicsSetting.bendingStretchVerticalScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.bendingShrinkHorizontalScaleValue):
                        physicsSetting.bendingShrinkHorizontalScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.bendingStretchHorizontalScaleValue):
                        physicsSetting.bendingStretchHorizontalScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.circumferenceShrinkScaleValue):
                        physicsSetting.circumferenceShrinkScaleValue = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.circumferenceStretchScaleValue):
                        physicsSetting.circumferenceStretchScaleValue = reader.ReadAsFloat();
                        break;
                    //OYM:5 value about virtual point;
                    case nameof(ADBPhysicsSetting.isComputeVirtual):
                        physicsSetting.isComputeVirtual = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isAllowComputeOtherConstraint):
                        physicsSetting.isAllowComputeOtherConstraint = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.virtualPointAxisLength):
                        physicsSetting.virtualPointAxisLength = reader.ReadAsFloat();
                        break;
                    case nameof(ADBPhysicsSetting.ForceLookDown):
                        physicsSetting.ForceLookDown = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isFixedPointFreezeRotation):
                        physicsSetting.isFixedPointFreezeRotation = reader.ReadAsBoolean().Value;
                        break;

                    //OYM: 2 value about weight
                    case nameof(ADBPhysicsSetting.isAutoComputeWeight):
                        physicsSetting.isAutoComputeWeight = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.weightCurve):
                        physicsSetting.weightCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                        break;

                    //OYM:10 bool about constraint 
                    case nameof(ADBPhysicsSetting.isComputeStructuralVertical):
                        physicsSetting.isComputeStructuralVertical = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isComputeStructuralHorizontal):
                        physicsSetting.isComputeStructuralHorizontal = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isComputeShear):
                        physicsSetting.isComputeShear = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isComputeBendingVertical):
                        physicsSetting.isComputeBendingVertical = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isComputeBendingHorizontal):
                        physicsSetting.isComputeBendingHorizontal = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isComputeCircumference):
                        physicsSetting.isComputeCircumference = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isCollideStructuralVertical):
                        physicsSetting.isCollideStructuralVertical = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isCollideStructuralHorizontal):
                        physicsSetting.isCollideStructuralHorizontal = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isCollideShear):
                        physicsSetting.isCollideShear = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isLoopRootPoints):
                        physicsSetting.isLoopRootPoints = reader.ReadAsBoolean().Value;
                        break;

                    //OYM: 3 value about otherthing
                    case nameof(ADBPhysicsSetting.isDebugDraw):
                        physicsSetting.isDebugDraw = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.isFixGravityAxis):
                        physicsSetting.isFixGravityAxis = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(ADBPhysicsSetting.gravity):
                        physicsSetting.gravity = reader.ReadAsVector3().ToUnityVector3Convert();
                        break;

                    //OYM:1 value about colliderChoice
                    case nameof(ADBPhysicsSetting.colliderChoice):
                        physicsSetting.colliderChoice = (ColliderChoice)reader.ReadAsInt32().Value;
                        break;
                }
            }

            BVA_physics_dynamicBoneMeta result = new BVA_physics_dynamicBoneMeta(new ADBChainProcessorMeta( keyWord,transformIndex, pointParentIndex, physicsSetting, pointColliderMasks, pointHitRadiuss));

            return result;
        }
    }
}