using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/Juicy/Scale Settings")]
    public class CollectableScaleOverTimeSet : ScriptableObject
    {
        public AnimationCurve pulsation;
        public float duration;

        public IEnumerator Scale(Transform _transform)
        {
            float timer = 0.0f;
            Vector3 endScale = _transform.localScale;

            _transform.localScale = Vector3.zero;

            while (timer < duration)
            {
                _transform.localScale = endScale * pulsation.Evaluate(timer / duration);

                yield return null;
                timer += Time.deltaTime;
            }

            _transform.localScale = endScale;
        }
    }
}