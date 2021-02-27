using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeHit 
{
    Collider2D GetCollider();
    void TakeDamaged(DamageData data);
}
