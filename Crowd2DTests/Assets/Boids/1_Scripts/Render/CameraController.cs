using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField, Range(0.0f, 1.0f)] private float lerpValue = 0.2f;
        private Vector2 lookAtPosition = default;
        void Update()
        {
            transform.position = Vector2.Lerp(transform.position, lookAtPosition, lerpValue);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
        }

        public void LookAt(Vector2 _pos)
        {
            lookAtPosition = _pos;
        }
    }
}