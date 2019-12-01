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
        private List<System.Action> nextPartCallbacks = default;
        private List<System.Action> endPhaseCallbacks = default;
        private List<System.Action> collectableGetCallbacks = default;
        private List<System.Action> collectableAddCallbacks = default;

        private bool disableCallbacks = false;


        public void Init()
        {
            score.Init();
            collectablesLeft = 0;
            disableCallbacks = false;
            nextPartCallbacks = new List<System.Action>();
            endPhaseCallbacks = new List<System.Action>();
            collectableGetCallbacks = new List<System.Action>();
            collectableAddCallbacks = new List<System.Action>();
        }

        public void Reset()
        {
            collectablesLeft = 0;
            nextPartCallbacks = new List<System.Action>();
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
            if (disableCallbacks) return;

            Debug.Log("OnPartEnd()");
            for (int i = 0; i < nextPartCallbacks.Count; i++)
            {
                nextPartCallbacks[i]();
            }
        }

        private void OnCollectableGot()
        {
            if (disableCallbacks) return;

            Debug.Log("OnCollectableGot(): "+collectableGetCallbacks.Count);
            for (int i = 0; i < collectableGetCallbacks.Count; i++)
            {
                collectableGetCallbacks[i]();
            }
        }

        private void OnCollectableAdded()
        {
            if (disableCallbacks) return;

            Debug.Log("OnCollectableAdded()");
            for (int i = 0; i < collectableAddCallbacks.Count; i++)
            {
                collectableAddCallbacks[i]();
            }
        }

        public void OnPhaseEnd()
        {
            if (disableCallbacks) return;

            Debug.Log("OnPhaseEnd()");
            for (int i = 0; i < endPhaseCallbacks.Count; i++)
            {
                endPhaseCallbacks[i]();
            }
        }

        public void NextPartRegisterCallback(System.Action _callback)
        {
            nextPartCallbacks.Add(_callback);
        }

        public void EndPhaseRegisterCallback(System.Action _callback)
        {
            endPhaseCallbacks.Add(_callback);
        }

        public void GetCollectableRegisterCallback(System.Action _callback)
        {
            collectableGetCallbacks.Add(_callback);
        }

        public void AddCollectableRegisterCallback(System.Action _callback)
        {
            collectableAddCallbacks.Add(_callback);
        }
    }
}