using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class CollectableBehaviour : MonoBehaviour
    {
        [SerializeField] private Behaviour[] enableAfterPoping = default;
        [SerializeField] private Object[] destroyedOnCollision = default;
        [SerializeField] private CollectableScaleOverTimeSet shrink = default;
        [SerializeField] private CollectableScaleOverTimeSet pop = default;
        [SerializeField] private PhaseData phaseData = default;
        [Header("Get To The Score")]
        [SerializeField] private AnimationCurve movementCurve = default;
        [SerializeField] private float reachDuration = 0.55f;

        private bool shrinking = false;
        private bool poping = false;
        private LDPhaseSettings phase;

        private void OnEnable()
        {
            phaseData?.AddCollectable();
            StartCoroutine(Pop());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (shrinking) return;

            if (collision.tag == "Player")
            {
                //if(phase) phase.RemoveCollectable(this);
               // Debug.Log("collision with: " + gameObject.name);
                //phaseData?.RemoveCollectable();
                //DestroyOnCollision();
                
                StartCoroutine(ReachScore());
            }
        }

        private IEnumerator Shrink()
        {
            shrinking = true;

            yield return StartCoroutine(shrink.Scale(transform));

            Destroy(gameObject);
        }

        private IEnumerator ReachScore()
        {
            Vector3 goal = GameObject.Find("Circle#1").transform.position;
            Vector3 start = transform.position;

            DestroyOnCollision();

            float timer = 0.0f;

            shrinking = true;

            while (timer<reachDuration)
            {
                transform.position = Vector3.Lerp(start, goal, movementCurve.Evaluate(timer / reachDuration));

                yield return null;
                timer += Time.deltaTime;
            }

            transform.position = goal;
            phaseData?.RemoveCollectable();
            Destroy(gameObject);
            
        }

        private IEnumerator Pop()
        {
            poping = true;

            yield return StartCoroutine(pop.Scale(transform));

            poping = false;
            EnableComponents();
        }

        public void AddPhaseSettings(LDPhaseSettings _settings)
        {
            phase = _settings;
            phase.AddCollectables(this);
        }

        private void DestroyOnCollision()
        {
            for (int i = 0; i < destroyedOnCollision.Length; i++)
            {
                Destroy(destroyedOnCollision[i]);
            }
        }

        private void DisableComponents()
        {
            for (int i = 0; i < enableAfterPoping.Length; i++)
            {
                enableAfterPoping[i].enabled = false;
            }
        }

        private void EnableComponents()
        {
            for (int i = 0; i < enableAfterPoping.Length; i++)
            {
                if(enableAfterPoping[i]!= null)
                    enableAfterPoping[i].enabled = true;
            }
        }
    }
}