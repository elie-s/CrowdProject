using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{
    public class PredatorBehaviour : MonoBehaviour
    {
        [SerializeField] private EntitiesData entitiesData = default;
        [SerializeField] private State currentState = default;
        [Header("Movement")]
        [SerializeField] private AnimationCurve accelerationCurve = default;
        [SerializeField] private float accelerationDuration = 0.8f;
        [SerializeField] private float decelerationDuration = 1.5f;
        [SerializeField] private float maxSpeed = default;
        [SerializeField] private AnimationCurve steeringCurve = default;

        private EntityData target = default;
        [SerializeField] private float speedTime = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(BehaviourRoutine());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            UpdatePredatorPosition();
            Move(speedTime);
        }

        private void UpdatePredatorPosition()
        {
            entitiesData.predatorPosition = transform.position;
        }

        private void Move(float _speedTime)
        {
            transform.position += transform.right * Speed(_speedTime) / 60.0f;
        }
        private float SteeringForce(float _speedTime)
        {
            return steeringCurve.Evaluate(_speedTime);
        }

        private void ApplyNewDirection(Vector2 _newDirectionNormalized, float _speedTime)
        {
            transform.right = Vector2.Lerp(transform.right, _newDirectionNormalized, SteeringForce(_speedTime) * Time.deltaTime * 60.0f);
        }

        private void ApplyNewDirection()
        {
            ApplyNewDirection((target.position - transform.position).normalized, speedTime);
        }

        private float Speed(float _time)
        {
            return accelerationCurve.Evaluate(_time) * maxSpeed;
        }

        private void GetTarget()
        {
            target = entitiesData.entities[Random.Range(0, entitiesData.entities.Count)];
        }

        private IEnumerator BehaviourRoutine()
        {
            GetTarget();
            currentState = State.TargetingPrey;

            while (Vector2.Angle(transform.right, (target.position - transform.position).normalized) > 45.0f)
            {
                ApplyNewDirection();

                yield return new WaitForFixedUpdate();
            }

            float timer = 0.0f;
            currentState = State.Rushing;

            while (Vector2.Angle(transform.right, (target.position - transform.position).normalized) < 60.0f)
            {
                speedTime = Mathf.Clamp01( timer / accelerationDuration);

                ApplyNewDirection();

                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;
            }

            timer = 0.0f;
            currentState = State.Decelerating;

            while (timer < decelerationDuration)
            {
                speedTime = 1.0f - timer / decelerationDuration;
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;
            }

            speedTime = 0.0f;

            currentState = State.Idling;

            Vector2 direction;
            float maxDuration;
            int loops = Random.Range(1, 4);

            for (int i = 0; i < loops; i++)
            {
                maxDuration = Random.Range(1.0f, 2.5f);
                direction = NewDirection(120.0f, 10.0f);
                timer = 0.0f;
                while (timer < maxDuration / 2.0f)
                {
                    speedTime = Mathf.Clamp01(timer / (maxDuration / 2.0f) );

                    ApplyNewDirection(direction, speedTime);

                    yield return new WaitForFixedUpdate();
                    timer += Time.fixedDeltaTime;
                }

                timer = 0.0f;
                while (timer < maxDuration / 2.0f)
                {
                    speedTime = Mathf.Clamp01(1.0f - timer / (maxDuration / 2.0f));

                    ApplyNewDirection(direction, speedTime);

                    yield return new WaitForFixedUpdate();
                    timer += Time.fixedDeltaTime;
                }
            }
            StartCoroutine(BehaviourRoutine());
        }

        private Vector2 NewDirection(float _maxAngle, float _minAngle)
        {
            Vector2 result = Vector2.zero;
            float angle = 0.0f;

            do
            {
                result = Random.insideUnitCircle.normalized;
                angle = Vector2.Angle(result, transform.right);
            } while (angle > _maxAngle || angle < _minAngle);

            return result;
        }

        public enum State
        {
            TargetingPrey,
            Rushing,
            Decelerating,
            Idling
        }
    }
}