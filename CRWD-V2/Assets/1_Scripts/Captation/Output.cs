using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRWD_V2.Captation
{
    public static class Output
    {
        public static Captation[] captations { get; private set; }

        public static void Reset(int _length)
        {
            captations = new Captation[_length];
        }

        public static void UpdateOutput(this Captation _captation, int _index)
        {
            if (_index < 0) Debug.LogWarning("Index must be superior or equal to 0.");

            else if (_index < captations.Length) captations[_index] = _captation;

            else Debug.LogWarning("Index must be in range.");
        }
    }
}