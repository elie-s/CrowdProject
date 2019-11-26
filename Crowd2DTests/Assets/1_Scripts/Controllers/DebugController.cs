using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class DebugController : MonoBehaviour
    {
        [SerializeField] private float speed = 5.0f;

        void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += (Vector3)GetInputs() * Time.deltaTime * speed;
        }

        private Vector2 GetInputs()
        {
            return new Vector2((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q) ? -1.0f : 0.0f),
                                (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ? -1.0f : 0.0f));
        }
    }
}