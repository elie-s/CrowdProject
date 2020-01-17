using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{
    public class BackgroundElementManager : MonoBehaviour
    {
        [SerializeField] private Transform camTransform = default;
        [SerializeField] private GameObject elementPrefab = default;
        [SerializeField] private float sizeFactor = 0.25f;
        [SerializeField] private int amount = 100;
        [SerializeField] private AnimationCurve sizeDistribution = default;
        [SerializeField] private AnimationCurve lerpDistribution = default;
        [SerializeField] private Gradient depthGradient = default;
        [SerializeField] private float maxSize = 10.0f;
        [SerializeField] private Rect centerRect = new Rect(-4.0f, -2.0f, 8.0f, 4.0f);
        [SerializeField] private Rect largeRect = new Rect(-6.5f, -3.5f, 13.0f, 7.0f);

        void Start()
        {
            LoadBackground();
        }

        private void LoadBackground()
        {
            int halfAmount = amount / 2;

            for (int i = 0; i < halfAmount; i++)
            {
                SetElement((float)i / (float)halfAmount, RandomPosition(centerRect));
            }

            for (int i = 0; i < halfAmount; i++)
            {
                SetElement((float)i / (float)halfAmount, RandomPosition(largeRect));
            }
        }

        private void SetElement(float _value, Vector2 _pos)
        {
            ParallaxHandler element = Instantiate(elementPrefab, _pos, Quaternion.identity, transform).GetComponent<ParallaxHandler>();
            float size = 1 + Mathf.Round(sizeDistribution.Evaluate(_value) * maxSize);
            element.Initialize(camTransform, size * sizeFactor, lerpDistribution.Evaluate((size - 1) / maxSize), depthGradient.Evaluate((size - 1) / maxSize));
        }

        private Vector2 RandomPosition(Rect _area)
        {
            return new Vector2(Random.Range(_area.x, _area.x + _area.width), Random.Range(_area.y, _area.y + _area.height));
        }
    }
}