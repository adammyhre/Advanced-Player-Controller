using System.Collections.Generic;
using System;
using UnityEngine;

namespace AdvancedController {
    public class RaycastSensor {
        public float castLength = 1f;
        public LayerMask layermask = 255;
        
        Vector3 origin = Vector3.zero;
        Transform tr;
        
        public enum CastDirection { Forward, Right, Up, Backward, Left, Down }
        CastDirection castDirection;
        
        RaycastHit hitInfo;

        public RaycastSensor(Transform playerTransform) {
            tr = playerTransform;
        }

        public void Cast() {
            Vector3 worldOrigin = tr.TransformPoint(origin);
            Vector3 worldDirection = GetCastDirection();
            
            Physics.Raycast(worldOrigin, worldDirection, out hitInfo, castLength, layermask, QueryTriggerInteraction.Ignore);
        }
        
        public bool HasDetectedHit() => hitInfo.collider != null;
        public float GetDistance() => hitInfo.distance;
        public Vector3 GetNormal() => hitInfo.normal;
        public Vector3 GetPosition() => hitInfo.point;
        public Collider GetCollider() => hitInfo.collider;
        public Transform GetTransform() => hitInfo.transform;
        
        public void SetCastDirection(CastDirection direction) => castDirection = direction;
        public void SetCastOrigin(Vector3 pos) => origin = tr.InverseTransformPoint(pos);

        Vector3 GetCastDirection() {
            return castDirection switch {
                CastDirection.Forward => tr.forward,
                CastDirection.Right => tr.right,
                CastDirection.Up => tr.up,
                CastDirection.Backward => -tr.forward,
                CastDirection.Left => -tr.right,
                CastDirection.Down => -tr.up,
                _ => Vector3.one
            };
        }
        
        public void DrawDebug() {
            if (!HasDetectedHit()) return;

            Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, Time.deltaTime);
            float markerSize = 0.2f;
            Debug.DrawLine(hitInfo.point + Vector3.up * markerSize, hitInfo.point - Vector3.up * markerSize, Color.green, Time.deltaTime);
            Debug.DrawLine(hitInfo.point + Vector3.right * markerSize, hitInfo.point - Vector3.right * markerSize, Color.green, Time.deltaTime);
            Debug.DrawLine(hitInfo.point + Vector3.forward * markerSize, hitInfo.point - Vector3.forward * markerSize, Color.green, Time.deltaTime);
        }
    }
}