using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTakeHit : MonoBehaviour, ITakeHit
{
    BulletBase bullet;

    private void Awake()
    {
        bullet = GetComponent<BulletBase>();
    }

    public Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }

    public virtual void TakeDamaged(DamageData damage)
    {
        if (bullet != null && bullet.isEnable)
        {
            bullet.Destroyed();
        }
    }
}
