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

        // Start is called before the first frame update
        void Start()
        {
            phase.AddCollectableRegisterCallback(SetBweeps);
            score.UpdateValueCallbackRegister(SetBweeps);
            score.UpdateValueCallbackRegister(SetScore);
            SetScore();
            SetBweeps();
        }

        private void SetBweeps()
        {
            bweepsDisplay.text = (score.collectables+"<color=#FFFFFFBB>/"+score.maxCollectables+"</color>").ToString();
        }

        private void SetScore()
        {
            scoreDisplay.text = score.value.ToString();
        }
    }
}