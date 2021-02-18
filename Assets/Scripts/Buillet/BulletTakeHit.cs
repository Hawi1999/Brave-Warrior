using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTakeHit : MonoBehaviour, TakeHit
{
    public Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }

    public virtual void TakeDamaged(DamageData damage)
    {

    }
}
