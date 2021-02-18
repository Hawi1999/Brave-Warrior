using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TakeHit 
{
    Collider2D GetCollider();
    void TakeDamaged(DamageData data);
}
