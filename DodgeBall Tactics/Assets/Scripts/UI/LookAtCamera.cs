using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool Invert;

    private void LateUpdate()
    {
        if (Invert)
        {
            Vector3 dirToCam = (Camera.main.transform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCam * -1);
        }
        else
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
