using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class RevolutionMovement : MonoBehaviour
    {
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float angle = 0.0f;

        private float angleRAD => angle * Mathf.Deg2Rad;
        private Vector2 center;

        private void Awake()
        {
            center = transform.position;
        }

        void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            transform.position = ToVector2();
            IncrementAngle();
        }

        private Vector2 ToVector2()
        {
            return new Vector2((float)System.Math.Round(radius * Mathf.Cos(angleRAD), 6) + center.x, (float)System.Math.Round(radius * Mathf.Sin(angleRAD), 6) + center.y);
        }

        private void IncrementAngle()
        {
            angle += Time.deltaTime * speed;

            if (angle > 180) angle = (angle - 180) - 180;
        }
    }
}