using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterSprite : MonoBehaviour
{
  public Sprite[] Frames;

  public SpriteRenderer SpriteRenderer;

  void Start()
  {
    SpriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void SetFrame(uint index)
  {
    if(index < Frames.Length)
    {
      SpriteRenderer.sprite = Frames[index];
    }
    else
    {
      Debug.LogError("Frame index " + name + " is out of bounds!");
    }
  }

  public void SetFacing(bool faceRight)
  {
    SpriteRenderer.flipX = !faceRight;
  }
}
