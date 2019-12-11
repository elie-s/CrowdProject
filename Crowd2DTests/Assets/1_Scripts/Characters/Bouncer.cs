using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class Bouncer : MonoBehaviour
    {
        [SerializeField] private float baseForce = 5.0f;
        [Header("Feedbacks")]
        [SerializeField] private float feedbackDuration = 0.2f;
        [SerializeField] private float feedbackScaleMultiplier = 1.25f;
        [SerializeField] private AnimationCurve feedbackCurve = default;
        [SerializeField] private Gradient feedbackColor = default;
        [Header("Components")]
        [SerializeField] private SpriteRenderer sRenderer = default;
        [SerializeField] private AudioSource sfx = default;

        private Vector3 baseScale;

        private void Start()
        {
            sRenderer.color = feedbackColor.Evaluate(0);
            baseScale = transform.localScale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<BlobController>()?.Repulse((collision.transform.position - transform.position).normalized, baseForce);
                StartCoroutine(FeedbackRoutine());
                sfx.Play();
            }

            if (collision.tag == "Bounceable")
            {
                collision.GetComponent<MovementBehaviour>()?.Repulse((collision.transform.position - transform.position).normalized, baseForce);
                StartCoroutine(FeedbackRoutine());
                sfx.Play();
            }
        }

        private IEnumerator FeedbackRoutine()
        {
            float timer = 0.0f;

            while (timer < feedbackDuration)
            {
                float feedbackValue = feedbackCurve.Evaluate(timer / feedbackDuration);
                transform.localScale = baseScale + baseScale * feedbackValue * feedbackScaleMultiplier;
                sRenderer.color = feedbackColor.Evaluate(feedbackValue);

                yield return null;
                timer += Time.deltaTime;
            }
            sRenderer.color = feedbackColor.Evaluate(0);
            transform.localScale = baseScale;
        }
    }
}