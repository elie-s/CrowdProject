using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class DirectionDisplayer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sRenderer = default;
        [SerializeField] private Gradient color = default;

        private void Update()
        {
            SetColor(transform.localPosition.magnitude*10);
        }

        public void SetColor(float _value)
        {
            sRenderer.color = color.Evaluate(_value);
        }
    }
}