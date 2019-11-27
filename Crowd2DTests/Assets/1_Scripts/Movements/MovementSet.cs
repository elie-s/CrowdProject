using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{

    public abstract class MovementSet : ScriptableObject
    {
        public abstract IEnumerator Move(Transform _transform, Vector3 _origin);
    }
}