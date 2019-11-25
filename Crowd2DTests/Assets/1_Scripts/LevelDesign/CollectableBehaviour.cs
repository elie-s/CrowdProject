using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehaviour : MonoBehaviour
{
    [SerializeField] private Object[] destroyedOnCollision;
    [SerializeField] private AnimationCurve shrinkingCurve;
    [SerializeField] private float shrinkingDuration;

    private bool shrinking = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shrinking) return;


        if (collision.tag == "Player")
        {
            DestroyOnCollision();
            StartCoroutine(Shrink());
        }
    }

    private IEnumerator Shrink()
    {
        shrinking = true;
        float timer = 0.0f;
        Vector3 baseScale = transform.localScale;

        while (timer<shrinkingDuration)
        {
            transform.localScale = baseScale * shrinkingCurve.Evaluate(timer / shrinkingDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void DestroyOnCollision()
    {
        for (int i = 0; i < destroyedOnCollision.Length; i++)
        {
            Destroy(destroyedOnCollision[i]);
        }
    }
}
