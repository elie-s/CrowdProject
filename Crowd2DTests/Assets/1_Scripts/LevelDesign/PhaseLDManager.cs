﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class PhaseLDManager : MonoBehaviour
    {
        [SerializeField] private LDPhaseSettings[] phases = default;
        [SerializeField] private int index;

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
                phaseSettings.Init();
            }
        }
    }
}