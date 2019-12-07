using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/Movement/Linear")]
    public class LinearMovement : MovementSet
    {
        [SerializeField] private AnimationCurve curve = default;
        [SerializeField] private float duration = 1.0f;
        [SerializeField] private float delay = 0.0f;
        [SerializeField] private Vector2 direction = default;
        [SerializeField] private float range = 1.0f;
        public override IEnumerator Move(Transform _transform, Vector3 _origin)
        {
            Vector3 startPos = _origin - (Vector3)direction.normalized * range / 2.0f;
            Vector3 endPos = _origin + (Vector3)direction.normalized * range / 2.0f;
            float timer = 0.0f;

            while (timer < duration)
            {
                _transform.position = Vector3.Lerp(startPos, endPos, curve.Evaluate(timer / duration));

                yield return null;
                timer += Time.deltaTime;
            }

            _transform.position = endPos;

            yield return new WaitForSeconds(delay);

            timer = 0.0f;

            while (timer < duration)
            {
                _transform.position = Vector3.Lerp(endPos, startPos, curve.Evaluate(timer / duration));

                yield return null;
                timer += Time.deltaTime;
            }

            _transform.position = startPos;
        }
    }
}