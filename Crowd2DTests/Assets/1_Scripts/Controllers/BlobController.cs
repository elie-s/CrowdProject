using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class BlobController : MovementBehaviour
    {
        [SerializeField] private WebCamManager webcam = default;
        [SerializeField] private AnimationCurve sizeRatioSpeedCurve = default;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private Transform debugDirection = default;
        [SerializeField] private PhaseData phaseData = default;
        
        [Header("Clamp Area")]
        [SerializeField] private Rect[] areas = default;
        private int areaIndex = 0;

        private Rect area => areas[areaIndex];

        private void Start()
        {
            phaseData.endPhase.Register(NextRect);
        }

        // Update is called once per frame
        void Update()
        {
            Move();

            
        }

        private void LateUpdate()
        {
            //ClampArea();
        }

        private void NextRect()
        {
            if (areaIndex < areas.Length - 1) areaIndex++;
        }

        private void Move()
        {
            //Debug.Log(sizeRatioSpeedCurve.Evaluate(SpriteGenerator.ratio) + " - " + SpriteGenerator.ratio);
            transform.position += webcam.direction * Time.deltaTime * speed*sizeRatioSpeedCurve.Evaluate(SpriteGenerator.ratio);
            debugDirection.localPosition = webcam.direction/transform.localScale.x;
            debugDirection.eulerAngles = new Vector3(0, 0, Mathf.Atan2(webcam.direction.normalized.y, webcam.direction.normalized.x) * Mathf.Rad2Deg+180);

            
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

        public enum orientation { Rotate0, Rotate90, Rotate180, Rotate270 }
    }
}