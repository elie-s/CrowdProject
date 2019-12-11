using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/Movement/Round")]
    public class CircleMovement : MovementSet
    {
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private bool clockwise = false;

        public override IEnumerator Move(Transform _transform, Vector3 _origin, float _multiplier)
        {
            float angle = 0.0f;

            while (true)
            {
                _transform.position = new Vector2((float)System.Math.Round(_multiplier * radius * Mathf.Cos(angle * Mathf.Deg2Rad), 6) + _origin.x, (float)System.Math.Round(_multiplier * radius * Mathf.Sin(angle * Mathf.Deg2Rad), 6) + _origin.y);
                IncrementAngle(ref angle);
                yield return null;
            }
        }


        private void IncrementAngle(ref float _angle)
        {

            if (!clockwise)
            {
                _angle += Time.deltaTime * speed;

                if (_angle > 180) _angle = (_angle - 180) - 180;
            }
            else
            {
                _angle -= Time.deltaTime * speed;

                if (_angle < -180) _angle = _angle+360;
            }
        }
    }
}