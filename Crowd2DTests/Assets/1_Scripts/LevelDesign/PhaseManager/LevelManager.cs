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

        private void Awake()
        {
            score.Init();
            phaseData.Init();
            phaseData.EndPhaseRegisterCallback(NextPhase);
            phaseData.EndPhaseRegisterCallback(camScaler.Scale);
            phaseIndex = -1;

        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(NextPhaseRoutine());
        }

        // Update is called once per frame
        void Update()
        {
            
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