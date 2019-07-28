using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected float Current = 50;
    public float Max = 50;
    
    void Start()
    {
        Current = Max;
    }

    void DealDamage(float Damage)
    {
        Current = Mathf.Clamp(Current - Damage, 0, Max);
    }

    float Get()
    {
        return Current;
    }
}
