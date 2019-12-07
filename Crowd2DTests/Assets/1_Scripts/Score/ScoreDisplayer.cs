using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CrowdProject
{
    public class ScoreDisplayer : MonoBehaviour
    {
        [SerializeField] private ScoreData score = default;
        [SerializeField] private PhaseData phase = default;
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI scoreDisplay = default;
        [SerializeField] private TextMeshProUGUI bweepsDisplay = default;
        [Header("Feedbacks")]
        [SerializeField] private Pulse bweepsPulse = default;

        void Start()
        {
            phase.collectableAdd.Register(SetBweeps);
            score.onUpdateValue.Register(SetBweeps);
            score.onUpdateValue.Register(SetScore);
            SetScore();
            SetBweeps();
        }

        private void SetBweeps()
        {
            bweepsDisplay.text = (score.collectables+"<color=#FFFFFFBB>/"+score.maxCollectables+"</color>").ToString();
            bweepsPulse.Play();
        }

        private void SetScore()
        {
            scoreDisplay.text = score.value.ToString();
        }
    }
}