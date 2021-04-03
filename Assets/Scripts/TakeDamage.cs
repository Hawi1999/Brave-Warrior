using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour, ITakeHit
{
    public Entity entity;
    private void Awake()
    {
        if (entity != null)
        {
            entity.OnValueChanged += OnEntityBuffsChanged;
        }
    }
    public void TakeDamaged(DamageData dama)
    {
        entity.TakeDamage(dama);
    }

    public Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }

    void OnEntityBuffsChanged(int code)
    {
        if (code == Entity.LOCK_COLLIDER_TAKEDAMAGE)
        {
            bool b = entity.LockColliderTakeDamage.isOk;
            GetCollider().enabled = b;
        }
    }
}
