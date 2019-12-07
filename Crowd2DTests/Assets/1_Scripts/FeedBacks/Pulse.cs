using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class Pulse : MonoBehaviour
    {
        [SerializeField] private PulsationSettings settings = default;
        [SerializeField] private float delay = 0.5f;
        [SerializeField] private bool loop = true;
        [SerializeField] private bool playOnStart = true;
        // Start is called before the first frame update
        void Start()
        {
            if(playOnStart) StartCoroutine(PulseRoutine());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Play()
        {
            StartCoroutine(PulseRoutine());
        }

        private IEnumerator PulseRoutine()
        {
            yield return StartCoroutine(settings.Play(transform));
            yield return new WaitForSeconds(delay);

            if(loop) StartCoroutine(PulseRoutine());
        }

    }
}