using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtils;

namespace AdvancedController {
    public class CameraController : MonoBehaviour {
        float currentXAngle;
        float currentYAngle;

        [Range(0f, 90f)] public float upperVerticalLimit = 35f;
        [Range(0f, 90f)] public float lowerVerticalLimit = 35f;

        public float cameraSpeed = 5f;
        public bool smoothCameraRotation;

        [Range(1f, 50f)] public float cameraSmoothingFactor = 25f;

        Transform tr;
        Camera cam;
        [SerializeField, Required] InputReader cameraInput;

        public Vector3 GetUpDirection() => tr.up;
        public Vector3 GetFacingDirection () => tr.forward;

        void Awake() {
            tr = transform;
            cam = GetComponentInChildren<Camera>();

            var eulerAngles = tr.localRotation.eulerAngles;
            currentXAngle = eulerAngles.x;
            currentYAngle = eulerAngles.y;

            UpdateRotation();
        }

        void Update() => HandleCameraRotation();

        void HandleCameraRotation() {
            if (cameraInput == null) return;

            RotateCamera(cameraInput.LookDirection.x, -cameraInput.LookDirection.y);
        }

        void RotateCamera(float horizontalInput, float verticalInput) {
            if (smoothCameraRotation) {
                horizontalInput = Mathf.Lerp(0, horizontalInput, Time.deltaTime * cameraSmoothingFactor);
                verticalInput = Mathf.Lerp(0, verticalInput, Time.deltaTime * cameraSmoothingFactor);
            }

            currentXAngle += verticalInput * cameraSpeed * Time.deltaTime;
            currentYAngle += horizontalInput * cameraSpeed * Time.deltaTime;

            currentXAngle = Mathf.Clamp(currentXAngle, -upperVerticalLimit, lowerVerticalLimit);

            UpdateRotation();
        }

        void UpdateRotation() {
            tr.localRotation = Quaternion.Euler(currentXAngle, currentYAngle, 0);
        }

        public void SetRotationAngles(float xAngle, float yAngle) {
            currentXAngle = xAngle;
            currentYAngle = yAngle;
            UpdateRotation();
        }

        public void RotateTowardPosition(Vector3 position, float lookSpeed) {
            RotateTowardDirection((position - tr.position).normalized, lookSpeed);
        }

        public void RotateTowardDirection(Vector3 direction, float lookSpeed) {
            direction = tr.parent.InverseTransformDirection(direction.normalized);
            var currentLookVector = tr.parent.InverseTransformDirection(tr.forward);

            Vector2 angleDifferences = new Vector2(
                VectorMath.GetAngle(new Vector3(0f, currentLookVector.y, 1f), new Vector3(0f, direction.y, 1f), Vector3.right),
                VectorMath.GetAngle(Vector3.ProjectOnPlane(currentLookVector, Vector3.up), Vector3.ProjectOnPlane(direction, Vector3.up), Vector3.up)
            );

            float angleMagnitude = angleDifferences.magnitude;
            if (angleMagnitude > 0) {
                angleDifferences *= Mathf.Min(lookSpeed * Time.deltaTime, angleMagnitude) / angleMagnitude;
                currentXAngle = Mathf.Clamp(currentXAngle + angleDifferences.x, -upperVerticalLimit, lowerVerticalLimit);
                currentYAngle += angleDifferences.y;
                UpdateRotation();
            }
        }
    }
}
