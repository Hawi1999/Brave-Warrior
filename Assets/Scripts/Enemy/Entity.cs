using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour, ICameraTarget
{
    public virtual int MaxHP
    {
        get; set;
    }
    private int heath = 50;
    public virtual int Heath
    {
        get
        {
            return heath;
        }
        set
        {
            int old = heath;
            heath = Mathf.Clamp(value, 0, MaxHP);
            if (heath != old)
            {
                OnHPChanged?.Invoke(old, heath);
            }
            if (heath == 0)
            {
                Death();
            }
        }
    }

    public UnityAction<int, int> OnHPChanged;
    public UnityAction<Entity> OnDeath;
    private Weapon weapon;
    public Weapon WeaponCurrent
    {
        get
        {
            return weapon;
        }
        set
        {
            if (value != weapon)
            {
                if (weapon != null )
                {
                    weapon.ChangEQuip(this, TrangThaiTrangBiVuKhi.Tudo);
                }
                weapon = value;
                if (value != null)
                {
                    weapon.ChangEQuip(this, TrangThaiTrangBiVuKhi.DangTrangBi);
                }
            }
        }
    }

    protected virtual void Death()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public virtual void TakeDamage(DamageData dama)
    {
        Heath -= dama.Damage;
    }
    public virtual Vector3 getPosition()
    {
        return transform.position;
    }
    public abstract Entity TargetFire
    {
        get; set;
    }

    protected bool Died;
    public virtual bool IsALive
    {
        get
        {
            return !Died;
        }
    }
    public virtual Vector3 DirectFire { get; set; }
    public virtual Vector3 PositionSpawnWeapon { get { return transform.position; } }
    public virtual Vector3 PositionColliderTakeDamage
    {
        get
        {
            return transform.position;
        }
    }

    public bool HasWeapon
    {
        get
        {
            return (WeaponCurrent != null);
        }
    }

}
