using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class BlobController : MonoBehaviour
    {
        [SerializeField] private WebCamManager webcam = default;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private Transform debugDirection = default;
        [SerializeField] private PhaseData phaseData;
        [Header("Clamp Area")]
        [SerializeField] private Rect[] areas = default;
        private int areaIndex = 0;

        private Rect area => areas[areaIndex];

        private void Awake()
        {
            phaseData.EndPhaseRegisterCallback(NextRect);
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void NextRect()
        {
            if (areaIndex < areas.Length - 1) areaIndex++;
        }

        private void Move()
        {
            transform.position += webcam.direction * Time.deltaTime * speed;
            debugDirection.localPosition = webcam.direction/transform.localScale.x;

            ClampArea();
        }

        private void ClampArea()
        {
            float x = transform.position.x;//Mathf.Clamp(transform.position.x, area.xMax, area.xMin);
            float y = transform.position.y; // Mathf.Clamp(transform.position.y, area.yMax, area.yMin);

            if (x < area.x) x = area.x;
            else if (x > area.x + area.width) x = area.x + area.width;

            if (y < area.y) y = area.y;
            else if (y > area.y + area.height) y = area.y + area.height;

            transform.position = new Vector2(x, y);
        }
    }
}