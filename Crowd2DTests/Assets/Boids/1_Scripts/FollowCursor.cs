using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    [SerializeField] private Camera mainCamera = default;

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
