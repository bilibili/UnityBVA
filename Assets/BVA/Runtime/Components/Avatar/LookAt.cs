using BVA.Extensions;
using UnityEngine;

namespace BVA.Component
{
    public class LookAt : MonoBehaviour
    {
        #region Gizmo
        public bool DrawGizmo = true;

        static void DrawMatrix(Matrix4x4 m, float size)
        {
            Gizmos.matrix = m;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.right * size);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, Vector3.up * size);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * size);
        }

        const float SIZE = 0.5f;

        private void OnDrawGizmos()
        {
            if (!DrawGizmo) return;

            if (LeftEye.Transform != null & RightEye.Transform != null)
            {
                DrawMatrix(LeftEye.WorldMatrix, SIZE);
                DrawMatrix(RightEye.WorldMatrix, SIZE);

            }
            else
            {
                DrawMatrix(Head.WorldMatrix, SIZE);
            }
        }
        #endregion

        public Transform Target;

        public TransformOffset Head;

        public TransformOffset LeftEye;
        public bool InverseLeftEyeVerticalDirection;

        public TransformOffset RightEye;
        public bool InverseRightEyeVerticalDirection;

        public void LookFace(Transform t)
        {
            if (Head.Transform == null) return;
            var head = Head.Transform;
            var headPosition = head.position + new Vector3(0, 0.05f, 0);
            t.position = headPosition + Head.WorldMatrix.ExtractRotation() * new Vector3(0, 0, 0.7f);
            t.LookAt(headPosition);
        }
        void Awake()
        {
            if (!TryGetComponent<Animator>(out var animator))
            {
                LogPool.RuntimeLogger.LogWarning(LogPart.Avatar, "animator is not found");
                return;
            }

            var head = animator.GetBoneTransform(HumanBodyBones.Head);
            if (head == null)
            {
                LogPool.RuntimeLogger.LogWarning(LogPart.Avatar, "head is not found");
                return;
            }

            Reset();
        }
        void Update()
        {
            if (Head == null)
            {
                enabled = false;
                return;
            }

            LookWorldPosition();
        }
        private void Reset()
        {
            // If no target has been assigned, look at the MainCamera
            if (Target == null)
                Target = Camera.main.transform;

            if (TryGetComponent<Animator>(out var animator))
            {
                LeftEye = TransformOffset.Create(animator.GetBoneTransform(HumanBodyBones.LeftEye));
                RightEye = TransformOffset.Create(animator.GetBoneTransform(HumanBodyBones.RightEye));
                Head = TransformOffset.Create(animator.GetBoneTransform(HumanBodyBones.Head));
            }

            Head.Setup();
            LeftEye.Setup();
            RightEye.Setup();
        }

        public void LookWorldPosition()
        {
            if (Target == null) return;
            float yaw;
            float pitch;
            LookWorldPosition(Target.position, out yaw, out pitch);
        }

        public void LookWorldPosition(Vector3 targetPosition, out float yaw, out float pitch)
        {
            var localPosition = Head.InitialWorldMatrix.inverse.MultiplyPoint(targetPosition);
            Matrix4x4.identity.CalculateYawPitch(localPosition, out yaw, out pitch);
            ApplyRotations(yaw, pitch);
        }
        float MapDegreeYaw(float src)
        {
            float CurveXRangeDegree = 90f;
            float CurveYRangeDegree = 15f;

            if (src < 0)
            {
                src = 0;
            }
            else if (src > CurveXRangeDegree)
            {
                src = CurveXRangeDegree;
            }
            return (src / CurveXRangeDegree) * CurveYRangeDegree;
        }
        void ApplyRotations(float yaw, float pitch)
        {
            // horizontal
            float leftYaw, rightYaw;
            if (yaw < 0)
            {
                leftYaw = -MapDegreeYaw(-yaw);
                rightYaw = -MapDegreeYaw(-yaw);
            }
            else
            {
                rightYaw = MapDegreeYaw(yaw);
                leftYaw = MapDegreeYaw(yaw);
            }

            // vertical
            if (pitch < 0)
            {
                pitch = -MapDegreeYaw(-pitch);
            }
            else
            {
                pitch = MapDegreeYaw(pitch);
            }

            // Apply
            if (LeftEye.Transform != null && RightEye.Transform != null)
            {
                LeftEye.Transform.rotation = LeftEye.InitialWorldMatrix.ExtractRotation() * Matrix4x4.identity.YawPitchRotation(leftYaw, InverseLeftEyeVerticalDirection ? -pitch : pitch);
                RightEye.Transform.rotation = RightEye.InitialWorldMatrix.ExtractRotation() * Matrix4x4.identity.YawPitchRotation(rightYaw, InverseRightEyeVerticalDirection ? -pitch : pitch);
            }
            else if (Head.Transform != null)
            {
                Head.Transform.rotation = Head.InitialWorldMatrix.ExtractRotation() * Head.OffsetRotation.YawPitchRotation(yaw, pitch);
            }
        }
    }
}