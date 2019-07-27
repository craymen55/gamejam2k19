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
  public float GroundSpeed = 10.0f;
  public float GroundFriction = 10.0f;
  public float GroundAcceleration { get { return GroundSpeed * GroundFriction; } }
  public float AirSpeed = 10.0f;
  public float AirFriction = 1.0f;
  public float AirAcceleration { get { return AirSpeed * AirFriction; } }

  public bool IsGrounded { get; set; }
  public LayerMask GroundLayers;
  public float GroundCheckRadius = 0.02f;
  public Transform FeetPos;

  public Vector2 Input { get; set; }

  // Start is called before the first frame update
  void Start()
  {
    Rigid = GetComponent<Rigidbody>();
    IsFacingRight = true;
  }

  void FixedUpdate()
  {
    // Get the appropriate movement values
    float Friction = IsGrounded ? GroundFriction : AirFriction;
    float Acceleration = IsGrounded ? GroundAcceleration : AirAcceleration;

    // Apply horizontal friction
    Rigid.velocity = new Vector3(Rigid.velocity.x * (1.0f - (Friction * Time.fixedDeltaTime)), Rigid.velocity.y, Rigid.velocity.z);

    // Apply horizontal acceleration
    float XAccel = Acceleration * Input.x * Time.fixedDeltaTime;
    Rigid.velocity += XAccel * Vector3.right;

    if (Input.x > 0.0f) IsFacingRight = true;
    else if (Input.x < 0.0f) IsFacingRight = false;

    // Reset input
    Input = new Vector2();
  }

  void Update()
  {
    // Check whether we're grounded
    IsGrounded = Physics.OverlapSphere(FeetPos.position, GroundCheckRadius, GroundLayers).Length > 0;
  }
}
