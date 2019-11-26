using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class GoldenSnitchBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform player = default;
        [SerializeField] private AnimationCurve celerityCurve = default;
        [SerializeField] private float durationRef = 1.0f;
        [SerializeField] private float maxDistance = 1.5f;

        private Vector2 waypoint;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            NewWaypoint();

            float timer = 0.0f;
            float duration = (waypoint - (Vector2)transform.position).magnitude / maxDistance * durationRef;
            Vector2 origin = transform.position;


            while (timer < duration)
            {
                transform.position = Vector2.Lerp(origin, waypoint,celerityCurve.Evaluate(timer / duration));

                yield return null;
                timer += Time.deltaTime;
            }

            transform.position = waypoint;
            yield return null;

            StartCoroutine(Move());
        }

        private void NewWaypoint()
        {
            waypoint = (Vector2)transform.position+Random.insideUnitCircle * maxDistance;
        }
    }
}