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
        public int baseScoring = 10;
        public int multiplier = 1;

        public void AddCollectable()
        {
            collectables++;
            value += baseScoring + multiplier;
            multiplier++;
        }

        public void ResetMultiplier()
        {
            multiplier = 1;
        }

        public void Init()
        {
            value = 0;
            collectables = 0;
            baseScoring = 10;
            multiplier = 1;
        }
    }
}