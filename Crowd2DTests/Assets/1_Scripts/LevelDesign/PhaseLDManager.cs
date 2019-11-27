using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class PhaseLDManager : MonoBehaviour
    {
        [SerializeField] private LDPhaseSettings[] phases = default;
        [SerializeField] private int index = 0;
        [SerializeField] private CameraScaler scaler = default;

        private LDPhaseSettings currentPhase => phases[index];

        private void Awake()
        {
            InitPhases();
        }

        [ContextMenu("Set Current Phase")]
        public void SetCurrentPhase()
        {
            currentPhase.Set();

            if (currentPhase.isFinished) index++;
        }

        private void InitPhases()
        {
            foreach (LDPhaseSettings phaseSettings in phases)
            {
                phaseSettings.Init(OnPhaseFinished);
            }
        }

        private void OnPhaseFinished()
        {
            scaler.Scale();
        }
    }
}