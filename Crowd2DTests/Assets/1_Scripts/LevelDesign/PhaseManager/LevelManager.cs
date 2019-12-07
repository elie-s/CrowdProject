using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private ScoreData score = default;
        [SerializeField] private PhaseData phaseData = default;
        [SerializeField] private CameraScaler camScaler = default;
        [SerializeField] private GameObject[] phases = default;
        [SerializeField] private GameObject[] borders = default;
        [SerializeField] private int phaseIndex = -1;
        [SerializeField] private float delayBetweenPhases = 1.5f;
        [Header("Transition")]
        [SerializeField] private GameObject transition = default;
        [SerializeField] private float transitionDuration = 5.0f;
        [Header("Audio Sources")]
        [SerializeField] private AudioSource getBweep = default;
        [SerializeField] private RandomSource endPart = default;

        private void Awake()
        {
            score.Init();
            phaseData.Init();
            phaseData.collectableGot.Register(getBweep.Play);
            phaseData.nextPart.Register(endPart.Play);
            phaseData.endPhase.Register(NextPhase);
            phaseData.endPhase.Register(camScaler.Scale);
            phaseIndex = -1;

        }

        void Start()
        {
            StartCoroutine(NextPhaseRoutine());
        }

        private void NextPhase()
        {
            borders[phaseIndex].SetActive(false);
            StartCoroutine(NextPhaseRoutine());
        }

        private IEnumerator NextPhaseRoutine()
        {
            phaseIndex++;
            yield return new WaitForSeconds(delayBetweenPhases);

            if (phaseIndex == phases.Length)
            {

            }
            else LoadPhase();
        }

        private void LoadPhase()
        {
            Instantiate(phases[phaseIndex], transform);
        }
    }
}