using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class Pulse : MonoBehaviour
    {
        [SerializeField] private PulsationSettings settings;
        [SerializeField] private float delay = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(PulseRoutine());
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator PulseRoutine()
        {
            yield return StartCoroutine(settings.Play(transform));
            yield return new WaitForSeconds(delay);

            StartCoroutine(PulseRoutine());
        }

    }
}