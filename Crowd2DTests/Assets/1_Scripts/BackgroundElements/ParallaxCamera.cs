using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class ParallaxCamera : MonoBehaviour
    {
        [SerializeField] private Transform player = default;
        [SerializeField, Range(0.0f, 0.5f)] private float lerpForce = 0.1f;

        void Update()
        {
            transform.position = Vector2.Lerp(Vector2.zero, player.position, lerpForce);
            transform.position += Vector3.forward * -10.0f;
        }
    }
}