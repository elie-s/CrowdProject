using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{
    [CreateAssetMenu()]
    public class EntitiesData : ScriptableObject
    {
        public List<EntityData> entities;
        public Vector3 predatorPosition;
    }
}