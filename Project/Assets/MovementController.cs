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
  public LayerMask GroundLayers;
  public float GroundCheckRadius = 0.02f;

  protected Vector2 Input;

  // Dash - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  

  // Start is called before the first frame update
  void Start()
  {
    Rigid = GetComponent<Rigidbody>();
  }

  public void SetInput(Vector2 input)
  {
    Input = input;
  }
  

  void FixedUpdate()
  {
    // Apply horizontal friction
    Rigid.velocity = new Vector3(Rigid.velocity.x * (1.0f - (HFriction * Time.fixedDeltaTime)), Rigid.velocity.y, Rigid.velocity.z);
    
    // Apply horizontal acceleration
    float XAccel = HAcceleration * Input.x * Time.fixedDeltaTime;
    Rigid.velocity += XAccel * Vector3.right;

    // Reset input
    Input = new Vector2();
  }

  private void Update()
  {
    
  }
}
