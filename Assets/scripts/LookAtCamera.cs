using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Start()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 lookDirection = (transform.position - cameraPosition).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection, transform.up);
    }
}