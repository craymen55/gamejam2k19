using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraCameraController : MonoBehaviour
{
    // Target to follow
    public Transform Target;

    public Vector3 CameraOffset;
    public float SmoothSpeed = 0.005f;

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector3 DesiredPosition = Target.position + CameraOffset;
        Vector3 SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed);
        transform.position = SmoothPosition;
    }
}
