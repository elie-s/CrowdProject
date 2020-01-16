using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu()]
    public class WebcamManagerData : ScriptableObject
    {
        [Range(0.0f, 0.5f)] public float ceil;
        public int size;
        public Rect rect;
        [Range(0, 60)] public int perSecondUpdate;
    }
}