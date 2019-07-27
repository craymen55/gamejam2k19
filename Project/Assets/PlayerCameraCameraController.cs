using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraCameraController : MonoBehaviour
{
    // Target to follow
    public GameObject Target;

    public Vector3 CameraOffset;
    public float SmoothSpeed = 0.005f;
    public float FacingOffset = 5.0f;

    protected Transform TargetTransform;
    protected MovementController TargetMovementController;

    void Start()
    {
        if (Target)
        {
            TargetTransform = Target.GetComponent<Transform>();
            TargetMovementController = Target.GetComponent<MovementController>();
        }
    }

    void FixedUpdate()
    {
        

        // Create default desired position
        Vector3 DesiredPosition = TargetTransform.position + CameraOffset;

        if (TargetMovementController)
        {
            float Direction = TargetMovementController.IsFacingRight ? 1 : -1;
            DesiredPosition.x += FacingOffset * Direction;
        }

        // Smooth to desired position using a lerp.
        Vector3 SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed);

        // Update Camera Position
        transform.position = SmoothPosition;
    }
}
