using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public abstract class MovementBehaviour : MonoBehaviour
    {
        [Header("Repulsion")]
        [SerializeField] private AnimationCurve repulsionCurve = default;
        [SerializeField] private float repulsionDuration = 2.0f;

        private Vector3 oldPosition = Vector3.zero;
        private IEnumerator repulse;

        public Vector3 direction => (transform.position - oldPosition).normalized;

        private void Awake()
        {
            repulse = RepulsionRoutine(Vector2.zero, 0);
        }

        private void LateUpdate()
        {
            oldPosition = transform.position;
        }

        public void Repulse(Vector2 _direction, float _force)
        {
            StopCoroutine(repulse);
            repulse = RepulsionRoutine(_direction, _force);
            StartCoroutine(repulse);
        }

        private IEnumerator RepulsionRoutine(Vector2 _direction, float _force)
        {
            float timer = 0.0f;

            while (timer < repulsionDuration)
            {
                transform.position += (Vector3)_direction * Time.deltaTime * _force * repulsionCurve.Evaluate(timer / repulsionDuration);

                yield return null;
                timer += Time.deltaTime;
            }
        }
    }
}