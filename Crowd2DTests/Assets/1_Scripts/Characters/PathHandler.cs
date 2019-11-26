using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class PathHandler : MonoBehaviour
    {
        [SerializeField] private VirtualGrid grid = default;
        [SerializeField] private int waypointsAmount = 2;
        [SerializeField] private List<Vector2> waypoints = new List<Vector2>();
        [SerializeField] private int waypointIndex = 0;
        [Header("Mouvements Settings")]
        [SerializeField] private float delayMaxBeforeFirstMove = 1.0f;
        [SerializeField] private bool randomDelayBeforeFirstMove = true;
        [SerializeField] private AnimationCurve celerityCurve = default;
        [SerializeField] private float durationRef = 1.0f;

        private Vector2 waypoint => waypoints[waypointIndex];

        private void Awake()
        {
            grid.Update(transform.position);
            InitializeWaypoints();
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(FirstMove());
        }

        private void InitializeWaypoints()
        {
            List<Vector2Int> pickedCells = new List<Vector2Int>();

            for (int i = 0; i < waypointsAmount; i++)
            {
                Vector2Int cell;

                do
                {
                    cell = new Vector2Int(Random.Range(0, grid.size.x), Random.Range(0, grid.size.y));
                } while (pickedCells.Contains(cell));

                pickedCells.Add(cell);

                Vector2 position = grid.RandomPosInCell(cell, 0.5f);
                waypoints.Add(position);
            }

            transform.position = waypoint;
        }

        private IEnumerator FirstMove()
        {
            if(randomDelayBeforeFirstMove)
            {
                yield return new WaitForSeconds(Random.Range(0.0f, delayMaxBeforeFirstMove));
            }
            else
            {
                yield return new WaitForSeconds(delayMaxBeforeFirstMove);
            }

            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            NextWaypoint();

            float timer = 0.0f;
            float duration = (waypoint - (Vector2)transform.position).magnitude / grid.cellWidth * durationRef;
            Vector2 origin = transform.position;


            while (timer < duration)
            {
                transform.position = Vector2.Lerp(origin, waypoint, celerityCurve.Evaluate(timer / duration));

                yield return null;
                timer += Time.deltaTime;
            }

            transform.position = waypoint;
            yield return null;

            StartCoroutine(Move());
        }

        private void NextWaypoint()
        {
            waypointIndex++;

            if (waypointIndex == waypointsAmount)
            {
                waypointIndex = 0;
            }
        }
    }
}