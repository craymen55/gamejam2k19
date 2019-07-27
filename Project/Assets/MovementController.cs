using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require a MovementController
[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
  protected Rigidbody Rigid;
  
  public bool IsFacingRight { get; set; }

  // Input manipulated by controllers such
  // as the player or AI brain bois.
  // We use this to determine movement based
  // on other tunables.
  public float Speed = 10.0f;
  public float Friction = 10.0f;
  public float Acceleration { get { return Speed * Friction; } }
  public LayerMask GroundLayers;
  public float GroundCheckRadius = 0.02f;

  public Vector2 Input { get; set; }

  // Dash - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


    // Start is called before the first frame update
    void Start()
  {
    Rigid = GetComponent<Rigidbody>();
  }

  public void SetInput(Vector2 input)
  {
    if (Input.x > 0f)
      IsFacingRight = true;
    else if (Input.x < 0f)
      IsFacingRight = false;

    Input = input;
  }

  void FixedUpdate()
  {
    // Apply horizontal friction
    Rigid.velocity = new Vector3(Rigid.velocity.x * (1.0f - (Friction * Time.fixedDeltaTime)), Rigid.velocity.y, Rigid.velocity.z);

    // Apply horizontal acceleration
    float XAccel = Acceleration * Input.x * Time.fixedDeltaTime;
    Rigid.velocity += XAccel * Vector3.right;
  }

  private void Update()
  {

  }
}
