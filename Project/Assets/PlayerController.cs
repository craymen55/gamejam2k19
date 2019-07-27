﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require a MovementController
[RequireComponent(typeof(MovementController))]
public class PlayerController : MonoBehaviour
{
  MovementController MvCon;

  public bool IsFacingRight = true;

  // Start is called before the first frame update
  void Start()
  {
    // Set the cache for our movement controller
    MvCon = GetComponent<MovementController>();
  }

  float GetHorizontalMovement()
  {
    float input = 0.0f;

    // Movement left
    if (Input.GetKey(KeyCode.LeftArrow))
    {
      input -= 1.0f;
    }

    // Movement Right
    if (Input.GetKey(KeyCode.RightArrow))
    {
      input += 1.0f;
    }

    if(input > 0.0f)
      IsFacingRight = true;
    else if (input < 0.0f)
      IsFacingRight = false;

    return input;
  }

  // Update is called once per frame
  void Update()
  {
    // Set the horizontal movement for the character
    // based on user input.

    Debug.Log("Horizontal:" + Input.GetAxisRaw("Horizontal"));
    Debug.Log("Vertical:" + Input.GetAxisRaw("Vertical"));
    Debug.Log("Fire1:" + Input.GetAxisRaw("Fire1"));

    // TODO: we may want to scale this over time?
    MvCon.SetInput(new Vector2(GetHorizontalMovement(), 0.0f));

  }
}
