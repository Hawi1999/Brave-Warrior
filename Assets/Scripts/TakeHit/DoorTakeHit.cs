﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTakeHit : MonoBehaviour, ITakeHit
{
    public void TakeDamaged(DamageData data)
    {

    }

    public Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }
}
