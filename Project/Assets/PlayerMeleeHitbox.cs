using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHitbox : MonoBehaviour
{
  PlayerController PlyCon;

  // Start is called before the first frame update
  void Start()
  {
    PlyCon = GetComponentInParent<PlayerController>();
  }

  void OnTriggerEnter(Collider other)
  {
    if(PlyCon)
    {
      PlyCon.OnMeleeHit(other);
    }
  }

  void OnTriggerStay(Collider other)
  {
    if (PlyCon)
    {
      PlyCon.OnMeleeHit(other);
    }
  }
}
