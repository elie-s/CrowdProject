using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/Score/Data")]
    public class ScoreData : ScriptableObject
    {
        public int value = 0;
        public int collectables = 0;
        public int maxCollectables = 0;
        public int baseScoring = 10;
        public int multiplier = 1;

        private List<System.Action> updatingValueCallbacks = default;

        public void AddCollectable()
        {
            collectables++;
            value += baseScoring + multiplier;
            multiplier++;

            OnUpdatedValues();
        }

        public void ResetMultiplier()
        {
            multiplier = 1;
        }

        public void Init()
        {
            value = 0;
            collectables = 0;
            maxCollectables = 0;
            baseScoring = 10;
            multiplier = 1;

            updatingValueCallbacks = new List<System.Action>();
        }

        private void OnUpdatedValues()
        {
            Debug.Log("OnUpdatedValues()");
            for (int i = 0; i < updatingValueCallbacks.Count; i++)
            {
                updatingValueCallbacks[i]();
            }
        }

        public void UpdateValueCallbackRegister(System.Action _callback)
        {
            updatingValueCallbacks.Add(_callback);
        }
    }
}