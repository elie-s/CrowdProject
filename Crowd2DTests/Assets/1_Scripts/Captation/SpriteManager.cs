using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    
    public class SpriteManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sRenderer = default;
        [SerializeField] private WebCamManager webcam = default;

        void Update()
        {
            sRenderer.sprite = webcam.sprite;
        }
    }
}