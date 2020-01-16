using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{
    public class BackgroundElementManager : MonoBehaviour
    {
        [SerializeField] private Transform camTransform = default;
        [SerializeField] private GameObject elementPrefab = default;
        [SerializeField] private float maxRange = default;
        [SerializeField] private int amount = 100;
        [SerializeField] private AnimationCurve sizeDistribution = default;
        [SerializeField] private AnimationCurve lerpDistribution = default;
        [SerializeField] private Gradient depthGradient = default;
        [SerializeField] private float maxSize = 10.0f;

        void Start()
        {
            LoadBackground();
        }

        private void LoadBackground()
        {
            for (int i = 0; i < amount; i++)
            {
                SetElement((float)i / (float)amount);
            }
        }

        private void SetElement(float _value)
        {
            ParallaxHandler element = Instantiate(elementPrefab, Random.insideUnitCircle * maxRange, Quaternion.identity, transform).GetComponent<ParallaxHandler>();
            float size = 1 + Mathf.Round(sizeDistribution.Evaluate(_value) * maxSize);
            element.Initialize(camTransform, size/6.0f, lerpDistribution.Evaluate((size - 1) / maxSize), depthGradient.Evaluate((size - 1) / maxSize));
        }
    }
}