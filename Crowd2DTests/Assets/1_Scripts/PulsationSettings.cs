using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/Juicy/Pulsation Settings")]
    public class PulsationSettings : ScriptableObject
    {
        public AnimationCurve pulsation;
        public float multiplier;
        public float duration;

        public IEnumerator Play(Transform _transform)
        {
            float timer = 0.0f;
            Vector3 baseScale = _transform.localScale;

            while (timer < duration)
            {
                _transform.localScale = baseScale + baseScale * pulsation.Evaluate(timer / duration) * multiplier;

                timer += Time.deltaTime;
                yield return null;
            }

            _transform.localScale = baseScale;
        }
    }
}