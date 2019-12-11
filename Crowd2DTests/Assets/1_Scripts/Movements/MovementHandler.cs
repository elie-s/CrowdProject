using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class MovementHandler : MovementBehaviour
    {
        [SerializeField] private MovementSet movement = default;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Move(transform.position));
        }

        private IEnumerator Move(Vector3 _origin)
        {
            yield return StartCoroutine(movement.Move(transform, _origin));
            StartCoroutine(Move(_origin));
        }

    }
}