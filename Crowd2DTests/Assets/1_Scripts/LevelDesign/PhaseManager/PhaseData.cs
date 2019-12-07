using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/Phase/Data")]
    public class PhaseData : ScriptableObject
    {
        [SerializeField] private ScoreData score = default;
        private int collectablesLeft = 0;
        public Callback nextPart = default;
        public Callback startPhase = default;
        public Callback endPhase = default;
        public Callback collectableGot = default;
        public Callback collectableAdd = default;

        public void Init()
        {
            score.Init();
            collectablesLeft = 0;
            nextPart = new Callback();
            startPhase = new Callback();
            endPhase = new Callback();
            collectableGot = new Callback();
            collectableAdd = new Callback();
        }

        public void Reset()
        {
            collectablesLeft = 0;
        }

        public void AddCollectable()
        {
            collectablesLeft++;
            score.maxCollectables++;
            OnCollectableAdded();
        }

        public void RemoveCollectable()
        {
            collectablesLeft--;
            score.AddCollectable();
            OnCollectableGot();
            if (collectablesLeft == 0) OnPartEnd();
        }

        private void OnPartEnd()
        {
            nextPart.Call();
        }

        private void OnCollectableGot()
        {
            collectableGot.Call();
        }

        private void OnCollectableAdded()
        {
            collectableAdd.Call();
        }

        public void OnPhaseEnd()
        {
            endPhase.Call();
        }
    }
}