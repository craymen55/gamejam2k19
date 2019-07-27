using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraCameraController : MonoBehaviour
{
    // Target to follow
    public GameObject Target;

    public Vector3 CameraOffset;
    public float SmoothSpeed = 0.005f;
    public float HorizontalExtent = 5.0f;
    public float HorizontalTargetExtent = 4.0f;

    protected bool ShouldMoveCamera = false;
    protected bool IsFacingRightCache = false;

    protected Transform TargetTransform;
    protected Rigidbody TargetRigidbody;
    protected MovementController TargetMovementController;

    protected Vector3 DesiredPosition;

    void Start()
    {
        if (Target)
        {
            TargetTransform = Target.GetComponent<Transform>();
            TargetMovementController = Target.GetComponent<MovementController>();
            DesiredPosition = TargetTransform.position + CameraOffset;
        }
    }

    void FixedUpdate()
    {
        bool PreviousShouldMoveCamera = ShouldMoveCamera;
        // If we aren't in the camera movement state, we should check if we should move it.
        if (!ShouldMoveCamera)
        {
            // 1. Get Camera x position
            float CameraX = transform.position.x;

            // 1. Get outer horizontal bounds of the camera.
            float RightBound = CameraX + HorizontalExtent;
            float LeftBound = CameraX - HorizontalExtent;

            // 2. Get Target X.
            float TargetX = Target.transform.position.x;

            // 3. Check if we are out of bounds. If we are, lets get in bounds.
            if (TargetX <= LeftBound || TargetX >= RightBound)
            {
                ShouldMoveCamera = true;

                if (TargetMovementController)
                {
                    IsFacingRightCache = TargetMovementController.IsFacingRight;
                }
            }
        }
        else // If we are moving the camera, update desired position before we lerp and smooth it.
        {
            if (TargetMovementController)
            {
                if (IsFacingRightCache != TargetMovementController.IsFacingRight)
                {
                    Debug.Log("TURNING OFF CAMERA MOVEMENT");
                    ShouldMoveCamera = false;
                }
            }

            if (ShouldMoveCamera)
            {
                // Create default desired position
                DesiredPosition = TargetTransform.position + CameraOffset;

                // If a MovementController exists on the target, add facing offset to desired position
                if (TargetMovementController)
                {
                    // Get our direction.
                    float Direction = TargetMovementController.IsFacingRight ? 1 : -1;
                    DesiredPosition.x += HorizontalTargetExtent * Direction;
                }

                if (Mathf.Abs(transform.position.x - DesiredPosition.x) < 0.1)
                {
                    ShouldMoveCamera = false;
                }
            }
        }

        // Create a smooth vector.
        Vector3 SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed);

        // Update Camera Position to the desired position
        transform.position = SmoothPosition;
    }
}
