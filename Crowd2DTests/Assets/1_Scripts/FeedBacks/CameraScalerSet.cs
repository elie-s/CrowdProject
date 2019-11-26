using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/Camera/Scaler Set")]
    public class CameraScalerSet : ScriptableObject
    {
        public AnimationCurve scalingCurve;
        public float duration;
        public float ratio;

        public IEnumerator Scale(Transform _transform, Camera _camera)
        {
            float timer = 0.0f;
            float startSize = _camera.orthographicSize;
            float targetSize = _camera.orthographicSize * ratio;
            Vector3 startScale = _transform.localScale;
            Vector3 targetScale = _transform.localScale * ratio;

            while (timer < duration)
            {
                float value = scalingCurve.Evaluate(timer / duration);

                _transform.localScale = Vector2.Lerp(startScale, targetScale, value);
                _camera.orthographicSize = Mathf.Lerp(startSize, targetSize, value);

                yield return null;
                timer += Time.deltaTime;
            }

            _transform.localScale = targetScale;
            _camera.orthographicSize = targetSize;
        }
    }
}