using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10);
    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 rotatedOffset = target.TransformDirection(offset);
            Vector3 desiredPosition = target.position + rotatedOffset;

            transform.position = desiredPosition;
            transform.LookAt(target); 
        }
    }
}
