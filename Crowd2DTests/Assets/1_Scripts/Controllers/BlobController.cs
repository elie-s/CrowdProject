using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class BlobController : MonoBehaviour
    {
        [SerializeField] private WebCamManager webcam = default;
        [SerializeField] private float speed = 5.0f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += webcam.direction * Time.deltaTime * speed;
        }
    }
}