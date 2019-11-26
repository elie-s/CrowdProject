using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class CameraScaler : MonoBehaviour
    {
        [SerializeField] private CameraScalerSet[] sets = default;
        [SerializeField] private int index = 0;
        [Header("Components")]
        [SerializeField] private Camera cam = default;
        [SerializeField] private Transform background = default;

        [ContextMenu("Scale")]
        public void Scale()
        {
            StartCoroutine(sets[index].Scale(background, cam));
            index++;
        }
    }
}