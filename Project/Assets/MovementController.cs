using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require a MovementController
[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
  protected Rigidbody Rigid;

  // Input manipulated by controllers such
  // as the player or AI brain bois.
  // We use this to determine movement based
  // on other tunables.
  public float HAcceleration = 100.0f;
  public float HFriction = 10.0f;
  public float Speed { get { return HAcceleration / HFriction; } }

  protected Vector2 Input;
  protected bool IsGrounded;

  // 


  // Start is called before the first frame update
  void Start()
  {
    Rigid = GetComponent<Rigidbody>();
  }

  public void TryJump()
  {
    if (IsGrounded)
    {
      Jump();
    }
  }

  public void Jump()
  {
    // Jumpshitman
  }

  public void SetInput(Vector2 input)
  {
    Input = input;
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    Rigid.velocity = new Vector3(Rigid.velocity.x * (1.0f - (HFriction * Time.fixedDeltaTime)), Rigid.velocity.y, Rigid.velocity.z);
    
    float XAccel = HAcceleration * Input.x * Time.fixedDeltaTime;
    float ZAccel = 0.0f;
    Rigid.velocity += new Vector3(XAccel, 0.0f, ZAccel);

  // Fuck you
  Input = new Vector2();
  }
}
