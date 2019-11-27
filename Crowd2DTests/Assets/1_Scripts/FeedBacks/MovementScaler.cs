using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class MovementScaler : MonoBehaviour
    {
        [SerializeField] private float multiplier = 5.0f;
        private Vector2 oldPosition = default;
        private Vector2 scaleRef;

        private void Start()
        {
            scaleRef = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Adapt()
        {
            Vector2 direction = (Vector2)transform.position - oldPosition;
            float weight = direction.magnitude;

        }
    }
}