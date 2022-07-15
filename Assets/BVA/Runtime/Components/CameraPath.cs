using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BVA.Component
{

    public class CameraPath : MonoBehaviour
    {
        [System.Serializable]
        public class VisualData
        {
            public Color pathColor = Color.green;
            public Color inactivePathColor = Color.gray;
            public Color frustrumColor = Color.white;
            public Color handleColor = Color.yellow;
        }

        public enum CurveType
        {
            EaseInAndOut,
            Linear,
            Custom
        }

        public enum AfterLoop
        {
            Continue,
            Stop
        }

        [System.Serializable]
        public class CameraPathPoint
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 handleprev;
            public Vector3 handlenext;
            public CurveType curveTypeRotation;
            public AnimationCurve rotationCurve;
            public CurveType curveTypePosition;
            public AnimationCurve positionCurve;
            public bool chained;

            public CameraPathPoint(Vector3 pos, Quaternion rot)
            {
                position = pos;
                rotation = rot;
                handleprev = Vector3.back;
                handlenext = Vector3.forward;
                curveTypeRotation = CurveType.EaseInAndOut;
                rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
                curveTypePosition = CurveType.Linear;
                positionCurve = AnimationCurve.Linear(0, 0, 1, 1);
                chained = true;
            }
        }


        public bool useMainCamera = true;
        public Camera selectedCamera;
        public bool lookAtTarget = false;
        public Transform target;
        public bool playOnAwake = false;
        public float playOnAwakeTime = 10;
        public List<CameraPathPoint> points = new List<CameraPathPoint>();
        public VisualData visual;
        public bool looped = false;
        public bool alwaysShow = true;
        public AfterLoop afterLoop = AfterLoop.Continue;

        private int currentWaypointIndex;
        private float currentTimeInWaypoint;
        private float timePerSegment;

        private bool paused = false;
        private bool playing = false;

        void Start()
        {

            if (Camera.main == null) { Debug.LogError("There is no main camera in the scene!"); }
            if (useMainCamera)
                selectedCamera = Camera.main;
            else if (selectedCamera == null)
            {
                selectedCamera = Camera.main;
                Debug.LogError("No camera selected for following path, defaulting to main camera");
            }

            if (lookAtTarget && target == null)
            {
                lookAtTarget = false;
                Debug.LogError("No target selected to look at, defaulting to normal rotation");
            }

            foreach (var index in points)
            {
                if (index.curveTypeRotation == CurveType.EaseInAndOut) index.rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
                if (index.curveTypeRotation == CurveType.Linear) index.rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
                if (index.curveTypePosition == CurveType.EaseInAndOut) index.positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
                if (index.curveTypePosition == CurveType.Linear) index.positionCurve = AnimationCurve.Linear(0, 0, 1, 1);
            }

            if (playOnAwake)
                PlayPath(playOnAwakeTime);
        }

        /// <summary>
        /// Plays the path
        /// </summary>
        /// <param name="time">The time in seconds how long the camera takes for the entire path</param>
        public void PlayPath(float time)
        {
            if (time <= 0) time = 0.001f;
            paused = false;
            playing = true;
            StopAllCoroutines();
            StartCoroutine(FollowPath(time));
        }

        /// <summary>
        /// Stops the path
        /// </summary>
        public void StopPath()
        {
            playing = false;
            paused = false;
            StopAllCoroutines();
        }

        /// <summary>
        /// Allows to change the time variable specified in PlayPath(float time) on the fly
        /// </summary>
        /// <param name="seconds">New time in seconds for entire path</param>
        public void UpdateTimeInSeconds(float seconds)
        {
            timePerSegment = seconds / ((looped) ? points.Count : points.Count - 1);
        }

        /// <summary>
        /// Pauses the camera's movement - resumable with ResumePath()
        /// </summary>
        public void PausePath()
        {
            paused = true;
            playing = false;
        }

        /// <summary>
        /// Can be called after PausePath() to resume
        /// </summary>
        public void ResumePath()
        {
            if (paused)
                playing = true;
            paused = false;
        }

        /// <summary>
        /// Gets if the path is paused
        /// </summary>
        /// <returns>Returns paused state</returns>
        public bool IsPaused()
        {
            return paused;
        }

        /// <summary>
        /// Gets if the path is playing
        /// </summary>
        /// <returns>Returns playing state</returns>
        public bool IsPlaying()
        {
            return playing;
        }

        /// <summary>
        /// Gets the index of the current waypoint
        /// </summary>
        /// <returns>Returns waypoint index</returns>
        public int GetCurrentWayPoint()
        {
            return currentWaypointIndex;
        }

        /// <summary>
        /// Gets the time within the current waypoint (Range is 0-1)
        /// </summary>
        /// <returns>Returns time of current waypoint (Range is 0-1)</returns>
        public float GetCurrentTimeInWaypoint()
        {
            return currentTimeInWaypoint;
        }

        /// <summary>
        /// Sets the current waypoint index of the path
        /// </summary>
        /// <param name="value">Waypoint index</param>
        public void SetCurrentWayPoint(int value)
        {
            currentWaypointIndex = value;
        }

        /// <summary>
        /// Sets the time in the current waypoint 
        /// </summary>
        /// <param name="value">Waypoint time (Range is 0-1)</param>
        public void SetCurrentTimeInWaypoint(float value)
        {
            currentTimeInWaypoint = value;
        }

        /// <summary>
        /// When index/time are set while the path is not playing, this method will teleport the camera to the position/rotation specified
        /// </summary>
        public void RefreshTransform()
        {
            selectedCamera.transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
            if (!lookAtTarget)
                selectedCamera.transform.rotation = GetLerpRotation(currentWaypointIndex, currentTimeInWaypoint);
            else
                selectedCamera.transform.rotation = Quaternion.LookRotation((target.transform.position - selectedCamera.transform.position).normalized);
        }

        IEnumerator FollowPath(float time)
        {
            UpdateTimeInSeconds(time);
            currentWaypointIndex = 0;
            while (currentWaypointIndex < points.Count)
            {
                currentTimeInWaypoint = 0;
                while (currentTimeInWaypoint < 1)
                {
                    if (!paused)
                    {
                        currentTimeInWaypoint += Time.deltaTime / timePerSegment;
                        selectedCamera.transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
                        if (!lookAtTarget)
                            selectedCamera.transform.rotation = GetLerpRotation(currentWaypointIndex, currentTimeInWaypoint);
                        else
                            selectedCamera.transform.rotation = Quaternion.LookRotation((target.transform.position - selectedCamera.transform.position).normalized);
                    }
                    yield return 0;
                }
                ++currentWaypointIndex;
                if (currentWaypointIndex == points.Count - 1 && !looped) break;
                if (currentWaypointIndex == points.Count && afterLoop == AfterLoop.Continue) currentWaypointIndex = 0;
            }
            StopPath();
        }

        int GetNextIndex(int index)
        {
            if (index == points.Count - 1)
                return 0;
            return index + 1;
        }

        Vector3 GetBezierPosition(int pointIndex, float time)
        {
            float t = points[pointIndex].positionCurve.Evaluate(time);
            int nextIndex = GetNextIndex(pointIndex);
            return
                Vector3.Lerp(
                    Vector3.Lerp(
                        Vector3.Lerp(points[pointIndex].position,
                            points[pointIndex].position + points[pointIndex].handlenext, t),
                        Vector3.Lerp(points[pointIndex].position + points[pointIndex].handlenext,
                            points[nextIndex].position + points[nextIndex].handleprev, t), t),
                    Vector3.Lerp(
                        Vector3.Lerp(points[pointIndex].position + points[pointIndex].handlenext,
                            points[nextIndex].position + points[nextIndex].handleprev, t),
                        Vector3.Lerp(points[nextIndex].position + points[nextIndex].handleprev,
                            points[nextIndex].position, t), t), t);
        }

        private Quaternion GetLerpRotation(int pointIndex, float time)
        {
            return Quaternion.LerpUnclamped(points[pointIndex].rotation, points[GetNextIndex(pointIndex)].rotation, points[pointIndex].rotationCurve.Evaluate(time));
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (UnityEditor.Selection.activeGameObject == gameObject || alwaysShow)
            {
                if (points.Count >= 2)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (i < points.Count - 1)
                        {
                            var index = points[i];
                            var indexNext = points[i + 1];
                            UnityEditor.Handles.DrawBezier(index.position, indexNext.position, index.position + index.handlenext,
                                indexNext.position + indexNext.handleprev, ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor), null, 5);
                        }
                        else if (looped)
                        {
                            var index = points[i];
                            var indexNext = points[0];
                            UnityEditor.Handles.DrawBezier(index.position, indexNext.position, index.position + index.handlenext,
                                indexNext.position + indexNext.handleprev, ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor), null, 5);
                        }
                    }
                }

                for (int i = 0; i < points.Count; i++)
                {
                    var index = points[i];
                    Gizmos.matrix = Matrix4x4.TRS(index.position, index.rotation, Vector3.one);
                    Gizmos.color = visual.frustrumColor;
                    Gizmos.DrawFrustum(Vector3.zero, 90f, 0.25f, 0.01f, 1.78f);
                    Gizmos.matrix = Matrix4x4.identity;
                }
            }
        }
#endif

    }
}