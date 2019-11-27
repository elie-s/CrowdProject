using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CrowdProject
{
    public class Frame : MonoBehaviour
    {
        [SerializeField] private RectTransform top = default;
        [SerializeField] private RectTransform bottom = default;
        [SerializeField] private RectTransform left = default;
        [SerializeField] private RectTransform right = default;

        public void Apply(Rect _rect)
        {
            top.anchoredPosition = new Vector2(_rect.x+_rect.width/2, _rect.y + _rect.height);
            top.sizeDelta = new Vector2(_rect.width, 1);

            bottom.anchoredPosition = new Vector2(_rect.x + _rect.width / 2, _rect.y + 0);
            bottom.sizeDelta = new Vector2(_rect.width, 1);

            left.anchoredPosition = new Vector2(_rect.x + 0, _rect.y + _rect.height/2);
            left.sizeDelta = new Vector2(1, _rect.height);

            right.anchoredPosition = new Vector2(_rect.x + _rect.width, _rect.y + _rect.height/2);
            right.sizeDelta = new Vector2(1, _rect.height);
        }
    }
}