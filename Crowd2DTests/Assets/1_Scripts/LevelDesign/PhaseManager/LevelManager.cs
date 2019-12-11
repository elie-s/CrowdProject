using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
        [SerializeField] private BlobController blob = default;
        [Header("feedback")]
        [SerializeField] private AnimationCurve bloomCurve = default;
        [SerializeField] private float bloomMax = 40.0f;
        [SerializeField] private float feedbackDuration = 0.2f;
        [SerializeField] private PostProcessProfile postProcess = default;
        [Header("Transition")]
        [SerializeField] private GameObject transition = default;
        [SerializeField] private Timer timer = default;
        [SerializeField] private float transitionDuration = 5.0f;
        [SerializeField] private GameObject end = default;
        [Header("Audio Sources")]
        [SerializeField] private AudioSource getBweep = default;
        [SerializeField] private RandomSource endPart = default;

        private void Awake()
        {
            score.Init();
            phaseData.Init();
            phaseData.collectableGot.Register(BweepFeedback);
            phaseData.nextPart.Register(endPart.Play);
            phaseData.endPhase.Register(NextPhase);
            //phaseData.endPhase.Register(camScaler.Scale);
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

            if (phaseIndex == 0)
            {
                yield return new WaitForSeconds(delayBetweenPhases);
            }
            else
            {
                timer?.Stop();

                

                yield return new WaitForSeconds(1.0f);
                blob.enabled = false;
                transition.SetActive(true);
                yield return new WaitForSeconds(delayBetweenPhases/2);
                camScaler.Scale();
                yield return new WaitForSeconds(delayBetweenPhases / 2);
                transition.SetActive(false);
                blob.enabled = true;
                yield return new WaitForSeconds(1.0f);

                
            }

            if (phaseIndex == phases.Length)
            {
                blob.enabled = false;
                end.SetActive(true);
            }
            else
            {
                timer?.Play();
                LoadPhase();
            }
        }

        private IEnumerator BloomBweepRoutine()
        {
            float timer = 0.0f;
            Bloom bloom = postProcess.settings[0] as Bloom;
            LensDistortion lens = postProcess.settings[1] as LensDistortion;

            float baseIntensity = 4.0f;
            float baseLens = 0.0f;

            while (timer < feedbackDuration)
            {
                bloom.intensity.value = Mathf.Lerp(baseIntensity, bloomMax, bloomCurve.Evaluate(timer / feedbackDuration));
                lens.intensity.value = Mathf.Lerp(baseLens, 20, bloomCurve.Evaluate(timer / feedbackDuration));

                yield return null;
                timer += Time.deltaTime;
            }

            bloom.intensity.value = baseIntensity;
            lens.intensity.value = baseLens;
        }

        private void BweepFeedback()
        {
            getBweep.Play();
            StartCoroutine(BloomBweepRoutine());
        }

        private void LoadPhase()
        {
            Instantiate(phases[phaseIndex], transform);
        }
    }
}