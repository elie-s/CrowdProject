using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class GoldenSnitchBehaviour : MonoBehaviour
    {
        [SerializeField] private Rect area = default;
        [SerializeField] private Transform player = default;
        [SerializeField] private AnimationCurve celerityCurve = default;
        [SerializeField] private float durationRef = 1.0f;
        [SerializeField] private float maxDistance = 1.5f;

        private Vector2 waypoint;
        private bool canFlee = true;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StartMove());
        }

        private IEnumerator StartMove()
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

        private IEnumerator FleeRoutine()
        {
            canFlee = false;

            yield return new WaitForSeconds(0.5f);

            canFlee = true;
        }

        private IEnumerator Move()
        {
            NewWaypoint();

            float timer = 0.0f;
            float duration = (waypoint - (Vector2)transform.position).magnitude / maxDistance * durationRef;
            Vector2 origin = transform.position;


            while (timer < duration)
            {
                transform.position = Vector2.Lerp(origin, waypoint, celerityCurve.Evaluate(timer / duration));

                if (canFlee && Vector2.Distance(transform.position, player.position) < maxDistance)
                {
                    StartCoroutine(FleeRoutine());
                    goto jump;
                }

                yield return null;
                timer += Time.deltaTime;
            }

            transform.position = waypoint;
            yield return null;


        jump:
            StartCoroutine(Move());
        }

        private void NewWaypoint()
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > maxDistance)
            {
                do
                {
                    waypoint = (Vector2)transform.position + Random.insideUnitCircle * maxDistance;
                } while (CheckBoundaries(waypoint));
            }
            else
            {
                int count = 0;

                do
                {
                    waypoint = (Vector2)transform.position + Random.insideUnitCircle * maxDistance / 2 + (Vector2)(transform.position - player.position);
                    count++;
                } while (CheckBoundaries(waypoint) && count < 20);

                if (count >= 20)
                {
                    do
                    {
                        waypoint = (Vector2)transform.position + Random.insideUnitCircle * maxDistance;
                    } while (CheckBoundaries(waypoint));
                }
            }
        }

        private bool CheckBoundaries(Vector2 _pos)
        {
            bool result = _pos.x < area.x || _pos.x > area.x + area.width || _pos.y < area.y || _pos.y > area.y + area.height;
            return result;
        }
    }
}