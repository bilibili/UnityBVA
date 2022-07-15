using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BVA.Component
{
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("BVA/Physics/Pick up")]
    public class PhysicsPickup : MonoBehaviour
    {
        //distance from the camera the item is carried
        public float dist = 2.5f;

        //the object being held
        private GameObject curObject;
        private Rigidbody curBody;

        //the rotation of the curObject at pickup relative to the camera
        private Quaternion relRot;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //on mouse click, either pickup or drop an item
            if (Input.GetMouseButtonDown(0))
            {
                if (curObject == null)
                {
                    PickupItem();
                }
                else
                {
                    DropItem();
                }
            }
        }

        void FixedUpdate()
        {
            if (curObject != null)
            {
                //keep the object in front of the camera
                ReposObject();
            }
        }

        //calculates the new rotation and position of the curObject
        void ReposObject()
        {
            //calculate the target position and rotation of the curbody
            Vector3 targetPos = transform.position + transform.forward * dist;
            Quaternion targetRot = transform.rotation * relRot;

            //interpolate to the target position using velocity
            curBody.velocity = (targetPos - curBody.position) * 10;

            //keep the relative rotation the same
            curBody.rotation = targetRot;

            //no spinning around
            curBody.angularVelocity = Vector3.zero;
        }

        //attempts to pick up an item straigth ahead
        void PickupItem()
        {
            //raycast to find an item
            RaycastHit hitInfo;
            Physics.Raycast(transform.position, transform.forward, out hitInfo, 5f);

            if (hitInfo.rigidbody == null)
                return;


            curBody = hitInfo.rigidbody;
            curBody.useGravity = false;

            curObject = hitInfo.rigidbody.gameObject;


            //hack w/ parenting & unparenting to get the relative rotation
            curObject.transform.parent = transform;
            relRot = curObject.transform.localRotation;
            curObject.transform.parent = null;


        }

        //drops the current item
        void DropItem()
        {
            curBody.useGravity = true;
            curBody = null;
            curObject = null;
        }
    }
}