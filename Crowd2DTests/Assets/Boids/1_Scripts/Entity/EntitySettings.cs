using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{
    [CreateAssetMenu()]
    public class EntitySettings : ScriptableObject
    {
        [Range(0.0f, 1.0f)] public float lerpValue;
        [Range(0.0f, 5.0f)] public float separationForce;
        [Range(0.0f, 5.0f)] public float alignementForce;
        [Range(0.0f, 1.0f)] public float cohesionForce;
        [Range(0.0f, 0.25f)] public float anchoredForce;
        public bool ownCursorForce;
        public bool anchoredToCenter;
        [Range(0.0f, 1.0f)] public float behaviouralAreaSpan;
        public Rect area = new Rect(-9.0f, -5.0f, 18.0f, 10.0f);
        public float inRange = 2.5f;
        public int minNeighbours = 2;
        public int maxNeighbours = 7;
        [Range(0.0f, 20.0f)] public float speed;
        [Range(0.0f, 2 * Mathf.PI)] public float deadSpot;
        public ColorationMode colorationMode = default;
        [Header("Predator")]
        public float predatorMaxRange = 10.0f;
        public AnimationCurve steerForce = default;
        public AnimationCurve fleeingSpeedCurve = default;

    }
}