using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EANS.Flocks
{
    public class ParallaxHandler : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform = default;
        [SerializeField, Range(0.0f, 1.0f)] private float lerpForce = 0.0f;

        private Vector2 startingPosition;

        // Start is called before the first frame update
        void Start()
        {
            startingPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Parallax();
        }

        private void Parallax()
        {
            transform.position = Vector2.Lerp(startingPosition, cameraTransform.position, lerpForce);
        }

        public void Initialize(Transform _cam, float _size, float _lerp)
        {
            transform.localScale *= _size;
            lerpForce = _lerp;
            cameraTransform = _cam;
        }

        public void Initialize(Transform _cam, float _size, float _lerp, Color _color)
        {
            transform.localScale *= _size;
            lerpForce = _lerp;
            cameraTransform = _cam;
            SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
            sRenderer.color = _color;
            sRenderer.sortingOrder = _size > 4 ? 10 : -10;
        }
    }
}