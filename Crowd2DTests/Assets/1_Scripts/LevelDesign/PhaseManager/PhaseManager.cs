using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class PhaseManager : MonoBehaviour
    {
        [SerializeField] private ScoreData score = default;
        [SerializeField] private GameObject[] parts = default;
        [SerializeField] private int partIndex = -1;
        [SerializeField] private PhaseData phaseData = default;
        [SerializeField] private float delayBetweenParts = 1.5f;
        [SerializeField] private bool finished = false;

        private void Awake()
        {
            phaseData.Reset();
            phaseData.GetCollectableRegisterCallback(score.AddCollectable);
            phaseData.NextPartRegisterCallback(NextPart);
            partIndex = -1;
            finished = false;
        }


        void Start()
        {
            NextPart();
        }

        private void LoadPart()
        {
            Instantiate(parts[partIndex], transform);
        }

        private void NextPart()
        {
            StartCoroutine(NextPartRoutine());
        }

        private IEnumerator NextPartRoutine()
        {
            partIndex++;
            yield return new WaitForSeconds(delayBetweenParts);

            if (partIndex == parts.Length)
            {
                finished = true;
                phaseData.OnPhaseEnd();
            }
            else LoadPart();
        }
    }
}