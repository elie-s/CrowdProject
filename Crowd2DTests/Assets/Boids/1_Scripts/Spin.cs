using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField, Range(-180.0f, 180.0f)] private float anglePerSeconds = 15.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Do();
    }

    private void Do()
    {
        transform.localEulerAngles += Vector3.forward * anglePerSeconds / 60.0f;
    }
}
