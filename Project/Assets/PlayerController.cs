using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require a MovementController
[RequireComponent(typeof(MovementController))]
public class PlayerController : MonoBehaviour
{
  MovementController MvCon;

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
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      input -= 1.0f;
    }

    // Movement Right
    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      input += 1.0f;
    }
    return input;
  }

  // Update is called once per frame
  void Update()
  {
    // Set the horizontal movement for the character
    // based on user input.

    // TODO: we may want to scale this over time?
    MvCon.SetInput(new Vector2(GetHorizontalMovement(), 0.0f));

  }
}
