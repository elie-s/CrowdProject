using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CrowdProject
{
    public class RandomScore : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI score = default;

        private void OnEnable()
        {
            int scoreValue = Random.Range(0, 10) + Random.Range(10, 100) + Random.Range(10, 1000) + Random.Range(100, 10000) + Random.Range(1000, 100000) + Random.Range(10000, 1000000) + Random.Range(10000, 1000000);

            score.text = scoreValue.ToString();
        }
    }
}