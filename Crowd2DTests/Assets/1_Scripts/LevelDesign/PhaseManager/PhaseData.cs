using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/Phase/Data")]
    public class PhaseData : ScriptableObject
    {
        private int collectablesLeft = 0;
        private List<System.Action> nextPartCallbacks = default;
        private List<System.Action> endPhaseCallbacks = default;
        private List<System.Action> collectableGetCallbacks = default;


        public void Init()
        {
            collectablesLeft = 0;
            nextPartCallbacks = new List<System.Action>();
            endPhaseCallbacks = new List<System.Action>();
            collectableGetCallbacks = new List<System.Action>();
        }

        public void Reset()
        {
            collectablesLeft = 0;
            nextPartCallbacks = new List<System.Action>();
        }

        public void AddCollectable()
        {
            collectablesLeft++;
        }

        public void RemoveCollectable()
        {
            collectablesLeft--;
            OnCollectableGot();
            if (collectablesLeft == 0) OnPartEnd();
        }

        private void OnPartEnd()
        {
            Debug.Log("OnPartEnd()");
            for (int i = 0; i < nextPartCallbacks.Count; i++)
            {
                nextPartCallbacks[i]();
            }
        }

        private void OnCollectableGot()
        {
            Debug.Log("OnCollectableGotEnd()");
            for (int i = 0; i < collectableGetCallbacks.Count; i++)
            {
                collectableGetCallbacks[i]();
            }
        }

        public void OnPhaseEnd()
        {
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
    }
}