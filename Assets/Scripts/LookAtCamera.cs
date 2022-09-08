using System;
using UnityEngine;

namespace Idle
{
    public class LookAtCamera : MonoBehaviour
    {
        private void Start()
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 lookDirection = (transform.position - cameraPosition).normalized;
            transform.rotation = Quaternion.LookRotation(lookDirection, transform.up);
        }
    }
}