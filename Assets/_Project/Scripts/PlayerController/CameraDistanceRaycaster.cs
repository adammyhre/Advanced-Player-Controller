using Sirenix.OdinInspector;
using UnityEngine;

namespace AdvancedController 
{
    public class CameraDistanceRaycaster : MonoBehaviour {
        [SerializeField, Required] Transform cameraTransform;
        [SerializeField, Required] Transform cameraTargetTransform;
        
        public LayerMask layerMask = Physics.AllLayers;
        public float minimumDistanceFromObstacles = 0.1f;
        public float smoothingFactor = 25f;
        
        Transform tr;
        float currentDistance;

        void Awake() {
            tr = transform;
            
            layerMask &= ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
            currentDistance = (cameraTargetTransform.position - tr.position).magnitude;
        }

        void LateUpdate() {
            Vector3 castDirection = cameraTargetTransform.position - tr.position;
            
            float distance = GetCameraDistance(castDirection);
            
            currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * smoothingFactor);
            cameraTransform.position = tr.position + castDirection.normalized * currentDistance;
        }

        float GetCameraDistance(Vector3 castDirection) {
            float distance = castDirection.magnitude + minimumDistanceFromObstacles;
            // if (Physics.Raycast(new Ray(tr.position, castDirection), out RaycastHit hit, distance, layerMask, QueryTriggerInteraction.Ignore)) {
            //     return Mathf.Max(0f, hit.distance - minimumDistanceFromObstacles);
            // }
            float sphereRadius = 0.5f;
            if (Physics.SphereCast(new Ray(tr.position, castDirection), sphereRadius, out RaycastHit hit, distance, layerMask, QueryTriggerInteraction.Ignore)) {
                return Mathf.Max(0f, hit.distance - minimumDistanceFromObstacles);
            }
            return castDirection.magnitude;
        }
    }
}
