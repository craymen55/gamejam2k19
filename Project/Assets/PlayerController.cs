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
    return Input.GetAxis("Horizontal Movement");
  }
  float GetVerticalMovement()
  {
    return Input.GetAxis("Vertical Movement");
  }

  // Update is called once per frame
  void Update()
  {
    // Set the horizontal movement for the character based on user input
    MvCon.SetInput(new Vector2(GetHorizontalMovement(), 0.0f));

  }
}
