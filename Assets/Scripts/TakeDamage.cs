using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour, ITakeHit
{
    public Entity entity;
    public void TakeDamaged(DamageData dama)
    {
        entity.TakeDamage(dama);
    }

    public Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }
}
