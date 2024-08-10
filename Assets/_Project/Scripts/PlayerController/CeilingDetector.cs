using UnityEngine;

namespace AdvancedController {
    public class CeilingDetector : MonoBehaviour {
        public float ceilingAngleLimit = 10f;
        public bool isInDebugMode;

        bool ceilingWasHit;
        
        float debugDrawDuration = 2.0f;

        Transform tr;

        void Awake() {
            tr = transform;
        }

        void OnCollisionEnter(Collision collision) => CheckFirstContact(collision);
        void OnCollisionStay(Collision collision) => CheckFirstContact(collision);

        void CheckFirstContact(Collision collision) {
            if (collision.contacts.Length == 0) return;

            float angle = Vector3.Angle(-tr.up, collision.contacts[0].normal);

            if (angle < ceilingAngleLimit) {
                ceilingWasHit = true;
            }

            if (isInDebugMode) {
                Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, debugDrawDuration);
            }
        }

        public bool HitCeiling() => ceilingWasHit;
        public void Reset() => ceilingWasHit = false;
    }
}