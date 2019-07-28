using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{
  public Sprite[] Frames;

  public SpriteRenderer SpriteRenderer;

  void SetFrame(uint index)
  {
    if(SpriteRenderer != null)
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
    else
    {
      Debug.LogError("Tried to use a CharacterSprite without an assigned SpriteRenderer!");
      enabled = false;
    }

  }
}
