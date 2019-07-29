using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require a MovementController
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterSprite))]
[RequireComponent(typeof(CharacterAudioManager))]
public class PlayerController : MonoBehaviour
{
  Rigidbody Rigid;
  MovementController MvCon;
  CharacterSprite ChSpr;
  CharacterAudioManager ChAu;

  public float InputGraceDuration = 0.1f;
  struct ActionState
  {
    public bool WasInputDown; // Whether input was already down last we checked
    public bool IsActive; // Whether action is happening right now
    public float QueueTime; // How long is left for a queued input
  };

  // DASH ATTACK - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  public float DashDuration = 0.5f;
  public float DashSpeed = 10f;
  public float GroundDashSpeed = 20.0f;
  public float AttackWindupTime = 0.02f; // Time until attack starts
  public float AttackStopTime = 0.2f; // Time until attack stops (delay after this)
  public float MeleeDamage = 10.0f;
  ActionState DashAttackAction;
  float DashTimeSpent;
  Vector2 DashDirection;

  public Transform MeleeHitbox;
  float MeleeOffset;

  // ATTACK RECOIL - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // If an attack makes contact, it gets short-circuited and becomes a recoil instead
  public float RecoilDuration = 0.1f;
  public float RecoilSpeed = 5f;
  bool IsRecoilActive;

  // Start is called before the first frame update
  void Start()
  {
    MvCon = GetComponent<MovementController>();
    Rigid = GetComponent<Rigidbody>();
    ChSpr = GetComponent<CharacterSprite>();
    ChAu = GetComponent<CharacterAudioManager>();

    MeleeOffset = MeleeHitbox.localPosition.x;
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
    if (!DashAttackAction.IsActive) MvCon.Input = moveInput;
    else MvCon.Input = new Vector2();

    // Update dash input
    bool dashInput = UpdateAction(ref DashAttackAction, GetDashInput());
    if(dashInput && moveInput.magnitude > 0.5f)
    {
      DashAttackAction.IsActive = true;
      DashTimeSpent = 0.0f;
      DashDirection = moveInput.normalized;
    }

    // Update audio state
    if(moveInput.magnitude > 0.5f && !ChAu.GetMoving())
    {
      ChAu.SetMoving(true);
    }
    else if(moveInput.magnitude < 0.5f && ChAu.GetMoving())
    {
      ChAu.SetMoving(false);
    }

    // Update animation state
    if(DashAttackAction.IsActive || !MvCon.IsGrounded)
    {
      ChSpr.SetFrame(2);
      ChSpr.SetFacing(DashDirection.x >= 0.0f);
    }
    else if(!Mathf.Approximately(moveInput.x, 0.0f))
    {

      ChSpr.SetFrame(1);
      ChSpr.SetFacing(moveInput.x >= 0.0f);
    }
    else
    {
      ChSpr.SetFrame(0);
    }
  }

  void FixedUpdate()
  {
    if (DashAttackAction.IsActive)
    {
      if (IsRecoilActive)
      {
        FixedUpdateRecoil();
      }
      // Normal dash attack action
      else
      {
        FixedUpdateDashAttack();
      }
    }
  }

  #region Melee
  void FixedUpdateDashAttack()
  {
    DashTimeSpent += Time.fixedDeltaTime;
    float talpha = DashTimeSpent / DashDuration;
    float scalar = 1.0f - 0.5f * talpha * talpha;
    if (DashDirection.y < 0.3f && MvCon.IsGrounded)
    {
      Vector2 direction = Mathf.Sign(DashDirection.x) * Vector2.right;
      Rigid.velocity = GroundDashSpeed * scalar * direction;
      MeleeHitbox.transform.localPosition = MeleeOffset * direction;
    }
    else
    {
      Rigid.velocity = DashSpeed * scalar * (DashDirection + (1.0f - scalar) * Vector2.down);
      MeleeHitbox.transform.localPosition = MeleeOffset * DashDirection;
    }

    // Check if dashing is done
    if (DashTimeSpent > DashDuration)
    {
      DashAttackAction.IsActive = false;
    }
  }

  void FixedUpdateRecoil()
  {
    DashTimeSpent += Time.fixedDeltaTime;
    float talpha = DashTimeSpent / RecoilDuration;
    float scalar = 1.0f - 0.5f * talpha * talpha;
    Vector2 direction = -DashDirection + 0.3f * Vector2.up;
    Rigid.velocity = RecoilSpeed * scalar * direction;
    if(DashTimeSpent > RecoilDuration)
    {
      IsRecoilActive = false;
      DashAttackAction.IsActive = false;
    }
  }
  
  public void OnMeleeHit(Collider other)
  {
    if(DashAttackAction.IsActive && DashTimeSpent > AttackWindupTime && DashTimeSpent < AttackStopTime && !IsRecoilActive)
    {
      Health otherHealth = other.GetComponent<Health>();
      if (otherHealth)
      {
        IsRecoilActive = true;
        DashTimeSpent = 0.0f;
        otherHealth.DealDamage(MeleeDamage);
        if (otherHealth.BloodSplatPrefab)
        {
          Instantiate(otherHealth.BloodSplatPrefab, MeleeHitbox.transform.position, Quaternion.FromToRotation(Vector3.right, -DashDirection));
        }
      }
    }
  }
  #endregion
}
