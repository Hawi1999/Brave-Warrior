﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierTakeHit : TakeHit
{
    [SerializeField] private int shield = 5;
    [SerializeField] VFXSpawnDestroyed VFX;

    private int Shield
    {
        get
        {
            return shield;
        } 
        set
        {
            shield = Mathf.Max(0, value);
            if (shield == 0)
            {
                Destroyed();
            }
        }
    }
    public override void TakeDamaged(DamageData data)
    {
        Shield -= data.getDamage();
    }

    private void Destroyed()
    {
        if (VFX != null)
        {
            Instantiate(VFX, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
        }
        Destroy(gameObject);
    }
}