using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require a MovementController
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
  protected Rigidbody Rigid;

  MovementController MvCon;

  public float InputGraceDuration = 0.1f;
  struct ActionState
  {
    public bool WasInputDown; // Whether input was already down last we checked
    public bool IsActive; // Whether action is happening right now
    public float QueueTime; // How long is left for a queued input
  };

  // DASHING - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  public float DashDuration = 0.5f;
  public float DashSpeed = 10.0f;
  ActionState DashAction;
  float DashTimeSpent = 0.0f;
  Vector2 DashDirection;

  // Start is called before the first frame update
  void Start()
  {
    MvCon = GetComponent<MovementController>();
    Rigid = GetComponent<Rigidbody>();
  }

  #region Input
  float GetHorizontalMovement()
  {
    return Input.GetAxis("Horizontal Movement");
  }
  float GetVerticalMovement()
  {
    return Input.GetAxis("Vertical Movement");
  }
  bool GetDashInput()
  {
    return Input.GetAxis("Dash") > 0.0f || Input.GetButton("Dash");
  }
  bool UpdateAction(ref ActionState action, bool IsButtonDown)
  {
    if(IsButtonDown)
    {
      if (!action.WasInputDown)
      {
        action.WasInputDown = true;
        if (action.IsActive)
        {
          action.QueueTime = InputGraceDuration;
          return false;
        }
        else
        {
          action.QueueTime = 0.0f;
          return true;
        }
      }
      else if (action.QueueTime > 0f)
      {
        if (!action.IsActive)
        {
          action.QueueTime = 0.0f;
          return true;
        }
        action.QueueTime -= Time.deltaTime;
      }
    }
    else
    {
      action.QueueTime = 0.0f;
      action.WasInputDown = false;
    }
    return false;
  }
  #endregion

  // Update is called once per frame
  void Update()
  {
    // Set the horizontal movement for the character based on user input
    Vector2 moveInput = new Vector2(GetHorizontalMovement(), GetVerticalMovement());
    MvCon.SetInput(moveInput);
    
    // Update dash input
    bool dashInput = UpdateAction(ref DashAction, GetDashInput());
    if(dashInput)
    {
      DashAction.IsActive = true;
      DashTimeSpent = 0.0f;
      DashDirection = moveInput;
    }
  }

  void FixedUpdate()
  {
    if(DashAction.IsActive)
    {
      DashTimeSpent += Time.fixedDeltaTime;
      float talpha = DashTimeSpent / DashDuration;
      float scalar = 1.0f - 0.7f * talpha * talpha;
      float dashAcceleration = scalar * DashSpeed * MvCon.AirFriction;
      Rigid.velocity += dashAcceleration * new Vector3(DashDirection.x, DashDirection.y, 0.0f) * Time.fixedDeltaTime;

      // Also counteract gravity while dashing
      Rigid.velocity -= Physics.gravity * scalar * Time.fixedDeltaTime;

      // Check if dashing is done
      if (DashTimeSpent > DashDuration)
      {
        DashAction.IsActive = false;
      }
    }
  }
}
