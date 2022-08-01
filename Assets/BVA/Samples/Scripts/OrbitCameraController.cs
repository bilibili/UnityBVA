using UnityEngine;

namespace BVA.Sample
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Camera-Control/OrbitCameraController")]
    public class OrbitCameraController : MonoBehaviour
    {
        [SerializeField]
        private float _scrollWheelSpeed = 10;

        [SerializeField]
        private float _rotationSpeed = 2;

        private Transform _target;
        private Vector3 _oriPosition;
        private Vector3 _oriRotation;

        private Vector2 _op0;
        private Vector2 _op1;

        private float distance = 10.0f;
        public float minDistance = 1.0f;
        public float maxDistance = 30.0f;
        public float move_speed = 0.01f;
        [HideInInspector]
        public bool Enable = true;

        private void Awake()
        {
            _target = transform;
            _oriPosition = transform.localPosition;
            _oriRotation = transform.localEulerAngles;
        }

        private void Update()
        {
            float x = Input.GetAxis("Mouse X") * _rotationSpeed;
            float y = Input.GetAxis("Mouse Y") * _rotationSpeed;

            if (Enable)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                if (_target != null)
                {
                    float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
                    if (scrollWheel > 0)
                    {
                        if (distance > minDistance)
                        {
                            distance -= scrollWheel * _scrollWheelSpeed;
                            transform.localPosition += transform.forward * scrollWheel * _scrollWheelSpeed;
                        }
                    }
                    else
                    {
                        if (distance < maxDistance)
                        {
                            distance -= scrollWheel * _scrollWheelSpeed;
                            transform.localPosition += transform.forward * scrollWheel * _scrollWheelSpeed;
                        }
                    }
                }

                if (Input.GetMouseButton(1))
                {
                    if (_target != null)
                    {
                        transform.RotateAround(_target.transform.position, Vector3.up, x);
                        transform.RotateAround(_target.transform.position, -transform.right, y);
                    }
                }
                if (Input.GetMouseButton(2))
                {
                    if (_target != null)
                    {
                        transform.Translate(Vector3.left * Input.GetAxis("Mouse X") * move_speed * Time.deltaTime);
                        transform.Translate(Vector3.up * Input.GetAxis("Mouse Y") * -move_speed * Time.deltaTime);
                    }
                }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (_target != null)
                {
                    transform.RotateAround(_target.transform.position, Vector3.up, x);
                    transform.RotateAround(_target.transform.position, -transform.right, y);
                }
            }
        }

        if (Input.touchCount > 1)
        {
            var tempPosition0 = Input.GetTouch(0).position;
            var tempPosition1 = Input.GetTouch(1).position;

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                if (IsEnLarge(tempPosition0, tempPosition1))
                {
                    if (distance > 1)
                    {
                        transform.localPosition += transform.forward * _scrollWheelSpeed * 0.1f;
                        distance -= _scrollWheelSpeed * 0.1f;
                    }
                }
                else
                {
                    if (distance < 10)
                    {
                        transform.localPosition -= transform.forward * _scrollWheelSpeed * 0.1f;
                        distance += _scrollWheelSpeed * 0.1f;
                    }
                }
            }

            _op0 = tempPosition0;
            _op1 = tempPosition1;
        }
#endif
            }
        }

        public void ResetCamera(Transform target)
        {
            _target = target;

            transform.localPosition = _oriPosition;
            transform.localPosition += target.localPosition;
            transform.localEulerAngles = _oriRotation;
        }

        private bool IsEnLarge(Vector2 newPosition0, Vector2 newPosition1)
        {
            var oldDistance = Mathf.Sqrt((_op0.x - _op1.x) * (_op0.x - _op1.x) + (_op0.y - _op1.y) * (_op0.y - _op1.y));
            var newDistance = Mathf.Sqrt((newPosition0.x - newPosition1.x) * (newPosition0.x - newPosition1.x) + (newPosition0.y - newPosition1.y) * (newPosition0.y - newPosition1.y));

            if (oldDistance < newDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
