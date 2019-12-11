using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class PhaseManager : MonoBehaviour
    {
        //[SerializeField] private ScoreData score = default;
        [SerializeField] private GameObject[] parts = default;
        [SerializeField] private int partIndex = -1;
        [SerializeField] private PhaseData phaseData = default;
        [SerializeField] private float delayBetweenParts = 1.5f;

        private GameObject currentPhase = default;

        private void Awake()
        {
            phaseData.Reset();
            //phaseData.GetCollectableRegisterCallback(score.AddCollectable);
            phaseData.nextPart.Register(NextPart);
            partIndex = -1;
        }


        void Start()
        {
            NextPart();
        }

        private void LoadPart()
        {
            currentPhase = Instantiate(parts[partIndex], transform);
        }

        private void NextPart()
        {
            //Destroy(currentPhase);
            StartCoroutine(NextPartRoutine());
        }

        private IEnumerator NextPartRoutine()
        {
            partIndex++;
            yield return new WaitForSeconds(delayBetweenParts);

            if (partIndex == parts.Length)
            {
                phaseData.nextPart.Unregister(NextPart);
                phaseData.OnPhaseEnd();
            }
            else LoadPart();
        }
    }
}