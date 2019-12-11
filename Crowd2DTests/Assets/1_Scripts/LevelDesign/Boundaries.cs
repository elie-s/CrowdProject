using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class Boundaries : MonoBehaviour
    {

        [SerializeField] private BoundType type = default;
        [SerializeField] private float force = 2.0f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                
                MovementBehaviour col = collision.GetComponent<MovementBehaviour>();
                //Debug.Log("Repulse: " + col.name);
                //Debug.Log("Repulse: " + col.direction);
                col.Repulse(GetBounceDirection(col.direction), force * col.inertia);
            }
        }

        private Vector2 GetBounceDirection(Vector2 _direction)
        {
            switch (type)
            {
                case BoundType.Vertical:
                    return new Vector2(-_direction.x, _direction.y).normalized;
                case BoundType.Horizontal:
                    return new Vector2(_direction.x, -_direction.y).normalized;
            }

            return Vector2.zero;
        }

        public enum BoundType { Vertical, Horizontal}
    }
}