using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRM;
using System.Collections;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;


namespace BVA
{
    internal class BVASpringBoneInternal
    {
        private float stiffnessForce;
        private float gravityPower;
        private Vector3 gravityDir;
        private float dragForce;
        private float hitRadius;
        private VRMSpringBoneColliderGroup[] colliderGroups;
        private List<Transform> rootBones;

        public BVASpringBoneInternal(float stiffnessForce, float gravityPower, Vector3 gravityDir, float dragForce, float hitRadius, VRMSpringBoneColliderGroup[] colliderGroups, List<Transform> rootBones)
        {
            this.stiffnessForce = stiffnessForce;
            this.gravityPower = gravityPower;
            this.gravityDir = gravityDir;
            this.dragForce = dragForce;
            this.hitRadius = hitRadius;
            this.colliderGroups = colliderGroups;
            this.rootBones = rootBones;
        }

        public void Initial()
        {
            
        }

        public void Update()
        {
            
        }
    }

    
}