using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibMMD;
using LibMMD.Unity3D;
using UnityEngine.Rendering;
using System.Linq;
using LibMMD.Model;
using System;

namespace ADBRuntime.Mono
{
    // [RequireComponent(typeof(MMDModel))]
    public static class ADBPMXRuntimeGenerate 
    {
        static string ColliderNameHead = "MMDCollider";
        public static void Generate(Transform[] bones, MMDModel mmdModel)
        {
            var allPoint = GeneratePoint(bones,mmdModel);
           GenerateCollider(bones, mmdModel, allPoint);

        }
        static ADBRuntimePoint[] GeneratePoint(Transform[] bones, MMDModel mmdModel)
        {
            if (mmdModel == null)
            {
                return null;
            }
            var allHumanBone = mmdModel.OriginalBone_IndexDictionary;
            List<ADBRuntimeCollider> colliderList = new List<ADBRuntimeCollider>();
            List<ADBChainProcessor> pointAndConstraintList = new List<ADBChainProcessor>();

            ADBSettingLinker globalSetting = (ADBSettingLinker)Resources.Load("Setting/MMD/Low/Low");
            var rawMMDModel = mmdModel.RawMMDModel;
            GameObject character = mmdModel.ModelRootTransform.gameObject;
            //OYM：point

            ADBRuntimePoint[] pointList = new ADBRuntimePoint[bones.Length];
            for (int i = 0; i < mmdModel.RawMMDModel.Joints.Length; i++)
            {

                var mmdJoint = mmdModel.RawMMDModel.Joints[i];
                MMDRigidBody jointFromBoneRigidBodyData = mmdModel.RawMMDModel.Rigidbodies[mmdJoint.AssociatedRigidBodyIndex[0]];
                var jointToBoneRigidBodyData = mmdModel.RawMMDModel.Rigidbodies[mmdJoint.AssociatedRigidBodyIndex[1]];

                int jointFromBoneIndex = jointFromBoneRigidBodyData.AssociatedBoneIndex;
                int jointToBoneIndex = jointToBoneRigidBodyData.AssociatedBoneIndex;

                if (bones.Length <= jointFromBoneIndex || bones.Length <= jointToBoneIndex) { continue; }

                Transform fromBone = bones[jointFromBoneIndex];
                Transform toBone = bones[jointToBoneIndex];
                if (pointList[jointFromBoneIndex] == null)
                {
                    pointList[jointFromBoneIndex] = BuildADBRuntimePoint(fromBone, jointFromBoneRigidBodyData, allHumanBone);
                }

                if (pointList[jointToBoneIndex] == null)
                {
                    pointList[jointToBoneIndex] = BuildADBRuntimePoint(toBone, jointToBoneRigidBodyData, allHumanBone);
                }
                if (pointList[jointToBoneIndex].Parent == null&& jointToBoneRigidBodyData.Type!=MMDRigidBody.RigidBodyType.RigidTypeKinematic)//OYM：if parent==null mean's it has therelation

                {
                    pointList[jointFromBoneIndex].AddChild(pointList[jointToBoneIndex]);
                }
            }
            for (int i = 0; i < pointList.Length; i++)
            {
                if (pointList[i] == null) { continue; }
                else if (pointList[i].Parent == null)
                {
                    SearchPointDepth(pointList[i], globalSetting, pointAndConstraintList);
                }
            }

            if (pointAndConstraintList.Count != 0)
            {
                for (int i = 0; i < pointAndConstraintList.Count; i++)
                {
                    pointAndConstraintList[i].Initialize();
                }
                var controller = mmdModel.gameObject.AddComponent<ADBRuntimeController>();
                controller.colliderCollisionType = ColliderCollisionType.Point;

                var swicher = mmdModel.gameObject.AddComponent<ADBPhysicsSettingSwitcher>();
                swicher.currentLinker = globalSetting;
                swicher.targetLinkers.Add((ADBSettingLinker)Resources.Load("Setting/MMD/Low/Low"));
                swicher.targetLinkers.Add((ADBSettingLinker)Resources.Load("Setting/MMD/Medium/Medium"));
                swicher.targetLinkers.Add((ADBSettingLinker)Resources.Load("Setting/MMD/High/High"));
            }


            return pointList;
        }
        static ADBRuntimePoint BuildADBRuntimePoint(Transform transform ,MMDRigidBody rigidBodyData, Dictionary<Transform, int> allHumanBone)
        {

            Transform[] allParent = transform.gameObject.GetComponentsInParent<Transform>();
            string keyWord = null;
            for (int i = 0; i < allParent.Length; i++)
            {
                if (allHumanBone.ContainsKey(allParent[i]))
                {
                    keyWord = MMDModel.Unity_MMDBoneNameDictionaryInverse[allParent[i].name];
                    break;
                }
            }
            if (keyWord == null)
            {
                keyWord = "Other";
            }
            var result=  ADBRuntimePoint.CreateRuntimePoint(transform, 0, keyWord);
            int mask = rigidBodyData.CollisionMask;

            result.pointRead.colliderMask = mask;
            result.pointRead.radius = rigidBodyData.Dimemsions.x/2;
            Action SetRigidBodyData = () =>
             {
                 if (result!=null)
                 {
                     result.pointRead.elasticity = rigidBodyData.TranslateDamp;
                     result.pointRead.elasticityVelocity = rigidBodyData.RotateDamp;
                     result.pointRead.moveInert = rigidBodyData.Restitution;
                 }
             };
            return result;
            
        }
        static void SearchPointDepth(ADBRuntimePoint point,ADBSettingLinker globalSetting, List<ADBChainProcessor> chainProcessList)
        {
            if (point==null)
            {
                return;
            }
            if (point.Parent == null)
            {
                if (point.ChildPoints==null)
                {
                    GameObject.DestroyImmediate(point);
                    return;
                }
                else
                {
                    ADBPhysicsSetting setting = globalSetting.GetSetting(point.keyWord);

                    var  chainProcessor =
                        ADBChainProcessor.CreateADBChainProcessor(point, setting);
                    chainProcessor.isUseLocalRadiusAndColliderMask = true;
                    chainProcessList.Add(chainProcessor);
                    chainProcessor.AddChild(point);
                }
            }
            else
            {
                point.SetDepth(point.Parent.depth + 1);
            }
            var childPoints = point.ChildPoints;
            for (int i = 0; i < childPoints?.Count; i++)
            {
                 SearchPointDepth(childPoints[i],  globalSetting, chainProcessList);
            }

        }

        static void GenerateCollider(Transform[] bones, MMDModel mmdModel, ADBRuntimePoint[] allPoint)
        {
            HashSet<Transform> except = new HashSet<Transform>();
            for (int i = 0; i < allPoint.Length; i++)
            {
                if (allPoint[i]!=null)
                {
                    except.Add(allPoint[i].transform);
                }
            }

            int bonesCount = bones.Length;
            Transform parentOfAll = mmdModel.gameObject.transform;

            int rigidbodiesCount = mmdModel.RawMMDModel.Rigidbodies.Length;
            for (int i = 0; i < rigidbodiesCount; i++)
            {
                MMDRigidBody mmdRigidBody = mmdModel.RawMMDModel.Rigidbodies[i];
                if (bones.Length <= mmdRigidBody.AssociatedBoneIndex) { continue; }
                GameObject bone = bones[mmdRigidBody.AssociatedBoneIndex].gameObject;
                if (mmdModel.Alt_OriginalBoneDictionary.ContainsKey(bone.transform))
                {
                    bone = mmdModel.Alt_OriginalBoneDictionary[bone.transform].gameObject;
                }
                //OYM:remove all move point's collider
                if (
                    mmdRigidBody.Type == MMDRigidBody.RigidBodyType.RigidTypePhysicsGhost&&
                   mmdRigidBody.Type == MMDRigidBody.RigidBodyType.RigidTypePhysicsStrict

                     )
                {
                    continue;
                }
                Transform[] allParent = bone.GetComponentsInParent<Transform>();

                if (allParent.Any(x=> except.Contains(x)))
                {
                    continue;
                }

                GameObject colliderObject = new GameObject(ColliderNameHead +mmdRigidBody.Name);
                colliderObject.transform.parent = bone.transform;
                Vector3 bonePosition = bone.transform.position - parentOfAll.position;
                colliderObject.transform.localPosition = mmdRigidBody.Position - bonePosition;
                colliderObject.transform.localRotation = Quaternion.Euler(mmdRigidBody.Rotation);
                switch (mmdRigidBody.Shape)
                {
                    case MMDRigidBody.RigidBodyShape.RigidShapeBox:
                        BoxCollider boxCollider = colliderObject.AddComponent<BoxCollider>();
                        boxCollider.size = mmdRigidBody.Dimemsions*2;
                        break;
                    case MMDRigidBody.RigidBodyShape.RigidShapeCapsule:
                        CapsuleCollider capsuleCollider = colliderObject.AddComponent<CapsuleCollider>();
                        capsuleCollider.radius = mmdRigidBody.Dimemsions.x;
                        capsuleCollider.height = mmdRigidBody.Dimemsions.y;
                        break;
                    case MMDRigidBody.RigidBodyShape.RigidShapeSphere:
                        SphereCollider sphereCollider = colliderObject.AddComponent<SphereCollider>();
                        sphereCollider.radius = mmdRigidBody.Dimemsions.x;
                        break;
                }


                var colliderReader = colliderObject.AddComponent<ADBColliderReader>();
                colliderReader.colliderMask = (ColliderChoice)(1<<mmdRigidBody.CollisionGroup);
            }
        }
    }
}



