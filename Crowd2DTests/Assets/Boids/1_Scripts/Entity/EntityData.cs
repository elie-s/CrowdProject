using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{
    public class EntityData : ScriptableObject
    {
        public Vector3 position = Vector3.zero;
        public Vector3 projectedHorizontalPos = Vector3.zero;
        public Vector3 projectedVerticalPos = Vector3.zero;
        public Vector3 projectedPos = Vector3.zero;
        public Vector3 direction = Vector3.zero;
        public List<EntityData> neighbours = new List<EntityData>();

        public bool IsInFOV(Vector3 _position, float _angleRAD)
        {
            if (Mathf.Approximately(_angleRAD, 0.0f)) return false;

            Vector2 testedDirection = (_position - position).normalized;

            return _angleRAD / 2 * Mathf.Rad2Deg < Vector2.Angle(testedDirection, direction);
        }
    }
}