using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{

    public enum ColorationMode { none, density, orientation}
    public class EntityBehaviour : MonoBehaviour
    {
        public EntityData data { get; private set; }
        [SerializeField] private EntitiesData entitiesData = default;
        [SerializeField] private EntitySettings settings = default;
        [SerializeField] private SpriteRenderer sRenderer = default;
        [Header("Orientation")]
        [SerializeField] private Gradient orientationGradient = default;
        [Header("Density")]
        [SerializeField] private Gradient densityGradient = default;
        [SerializeField] private AnimationCurve densityCurve = default;
        [SerializeField] private float densityMaxDistance = default;
        [Header("Limits")]
        [SerializeField] private bool tpOnLimitsReached = true;

        private Camera cam = default;
        private Rect behaviouralArea;
        private float reachCursorForce = 0.0f;
        private Transform anchor;

        private float anchorDistance => Vector2.Distance(transform.position, anchor.position);

        private void OnEnable()
        {
            data = ScriptableObject.CreateInstance<EntityData>();

            transform.right = Random.insideUnitCircle;
            behaviouralArea = new Rect(settings.area.x * settings.behaviouralAreaSpan, settings.area.y * settings.behaviouralAreaSpan, settings.area.width * settings.behaviouralAreaSpan, settings.area.height * settings.behaviouralAreaSpan);
            cam = Camera.main;

            reachCursorForce = Random.Range(0.00f, 0.01f);
        }

        void Start()
        {

        }

        void FixedUpdate()
        {
            Move();
            if ((tpOnLimitsReached && behaviouralArea.Contains(transform.position)) || !tpOnLimitsReached) CorrectTrajectory();
            ApplyColor();

            UpdateData();
        }

        private void LateUpdate()
        {
            
        }

        private void UpdateData()
        {
            data.direction = (transform.position - data.position).normalized;
            data.position = transform.position;
        }

        private void Move()
        {
            transform.position += transform.right * Speed()/ 60.0f;

            if (tpOnLimitsReached) OnLimitsReached();
        }

        private void CorrectTrajectory()
        {
            Vector3 newDirection = AvoidObstacle();

            if (newDirection != Vector3.zero)
            {
                Debug.Log("Obstacle detected.");
                transform.right = newDirection;
            }
            else
            {
                newDirection = AverageTrajectory() * settings.alignementForce;
                newDirection += (NeighbourhoodCenter() - transform.position) * settings.cohesionForce;
                newDirection += AvoidCrash() * Mathf.Lerp(1.0f, settings.separationForce, Speed() / (settings.speed + settings.maxSpeedModifier));

                transform.right = Vector3.Lerp(transform.right, newDirection.normalized, settings.lerpValue /* * Time.deltaTime * 60.0f*/);
                if (settings.anchoredToCenter) transform.right = Vector2.Lerp(transform.right, anchor.position - transform.position, settings.ownCursorForce ? reachCursorForce : settings.anchoredForce /* * Time.deltaTime * 60.0f*/);
                else
                {
                    if (Input.GetKey(KeyCode.Space)) transform.right = Vector2.Lerp(transform.right, cam.ScreenToWorldPoint(Input.mousePosition) - transform.position, settings.ownCursorForce ? reachCursorForce : settings.anchoredForce /* * Time.deltaTime * 60.0f*/);
                }

                //Flee();
            }
        }

        private float Speed()
        {
            float anchorSpeedTime = Mathf.Clamp01((anchorDistance - settings.minAnchorDistance) / (settings.maxAnchorDistance - settings.minAnchorDistance));

            float speed = (settings.speed + (settings.maxNeighbours - data.neighbours.Count) / 2) + settings.anchorSpeedModifier.Evaluate(anchorSpeedTime)*settings.maxSpeedModifier;

            return speed;

            //return speed + speed * settings.fleeingSpeedCurve.Evaluate(1- distance / settings.predatorMaxRange) * 4;
        }

        private void Flee()
        {
            float distance = Vector2.Distance(transform.position, entitiesData.predatorPosition);
            if (distance > settings.predatorMaxRange) return;

            Vector2 direction = -(entitiesData.predatorPosition - transform.position).normalized;

            transform.right = Vector2.Lerp(transform.right, direction, settings.steerForce.Evaluate(1 - distance / settings.predatorMaxRange) /* * Time.deltaTime * 60.0f*/);
        }

        public void SetAnchor(Transform _anchor)
        {
            anchor = _anchor;
        }

        private void ApplyColor()
        {
            if (settings.colorationMode == ColorationMode.orientation)
            {
                float angle = Mathf.Atan2(transform.right.y, transform.right.x);
                sRenderer.color = orientationGradient.Evaluate((angle + Mathf.PI) / (2 * Mathf.PI));
            }
            else if(settings.colorationMode == ColorationMode.density)
            {
                //Vector2 centerVector = NeighbourhoodCenter() - transform.position;
                float averageDistance = 0.0f;

                for (int i = 0; i < data.neighbours.Count; i++)
                {
                    averageDistance += Vector2.Distance(transform.position, data.neighbours[i].position);
                }

                averageDistance /= data.neighbours.Count;

                sRenderer.color = densityGradient.Evaluate(densityCurve.Evaluate(averageDistance / densityMaxDistance));
            }
        }

        private Vector3 AverageTrajectory()
        {
            Vector3 trajectory = transform.right;

            foreach (EntityData entity in data.neighbours)
            {
                trajectory += entity.direction;
            }

            return trajectory.normalized;
        }

        private Vector3 NeighbourhoodCenter()
        {
            Vector3 center = Vector3.zero;

            foreach (EntityData entity in data.neighbours)
            {
                center += entity.position;
            }

            return center/(data.neighbours.Count);
        }

        private Vector3 AvoidCrash()
        {
            Vector3 trajectory = Vector3.zero;

            foreach (EntityData entity in data.neighbours)
            {
                if(Vector3.Distance(entity.position+entity.direction, transform.position) < Vector3.Distance(entity.position, transform.position) || Vector3.Distance(entity.position, transform.position) < 1.0f)
                {
                    trajectory += (transform.position - entity.position).normalized;
                }
            }


            return trajectory == Vector3.zero ? Vector3.zero : trajectory.normalized;
        }

        private Vector3 AvoidObstacle()
        {
            Vector3 result = Vector3.zero;

            RaycastHit2D frontHit = Physics2D.Raycast(transform.position, transform.right, 1.0f);

            if(frontHit.collider)
            {
                float refAngle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
                float testedAngle = 1.0f;
                RaycastHit2D clockwiseHit;
                RaycastHit2D counterClockwiseHit;

                while (testedAngle < 180.0f)
                {
                    clockwiseHit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos((refAngle - testedAngle) * Mathf.Deg2Rad), Mathf.Sin((refAngle - testedAngle) * Mathf.Deg2Rad)), 1.0f);
                    counterClockwiseHit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos((refAngle + testedAngle) * Mathf.Deg2Rad), Mathf.Sin((refAngle + testedAngle) * Mathf.Deg2Rad)), 1.0f);

                    if (clockwiseHit.collider && counterClockwiseHit.collider)
                    {
                        testedAngle++;
                        continue;
                    }
                    else if(clockwiseHit.collider)
                    {
                        return new Vector2(Mathf.Cos((refAngle + Mathf.Pow(testedAngle, 2)) * Mathf.Deg2Rad), Mathf.Sin((refAngle + Mathf.Pow(testedAngle, 2)) * Mathf.Deg2Rad));
                    }
                    else
                    {
                        return new Vector2(Mathf.Cos((refAngle - Mathf.Pow(testedAngle, 2)) * Mathf.Deg2Rad), Mathf.Sin((refAngle - Mathf.Pow(testedAngle, 2)) * Mathf.Deg2Rad));
                    }
                }

                Debug.Log("Failed to avoid obstacle.");
            }

            return result;
        }

        private void OnLimitsReached()
        {
            float newX;
            float newY;

            if (transform.position.x > settings.area.xMax) newX = settings.area.xMin;
            else if (transform.position.x < settings.area.xMin) newX = settings.area.xMax;
            else newX = transform.position.x;

            if (transform.position.y > settings.area.yMax) newY = settings.area.yMin;
            else if (transform.position.y < settings.area.yMin) newY = settings.area.yMax;
            else newY = transform.position.y;

            transform.position = new Vector2(newX, newY);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            foreach (EntityData neighbourPosition in data.neighbours)
            {
                Gizmos.DrawLine(transform.position, neighbourPosition.position);
            }

            Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.25f);
            for (int i = 0; i < 80; i+=2)
            {
                Gizmos.DrawLine(Vector3.Lerp(transform.position, anchor.position, i/80.0f), Vector3.Lerp(transform.position, anchor.position, (i+1) / 80.0f));
            }
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.grey;

            Gizmos.DrawRay(transform.position, transform.right);
        }
    }
}