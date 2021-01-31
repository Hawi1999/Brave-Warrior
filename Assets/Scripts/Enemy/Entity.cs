using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour, ICameraTarget
{
    public abstract int MaxHP
    {
        get;
    }
    protected int heath;
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
            OnHPChanged?.Invoke(old, heath, MaxHP);
            if (heath == 0)
            {
                Death();
            }
        }
    }


    public UnityAction<int, int, int> OnHPChanged;
    public UnityAction<Entity> OnDeath;
    public UnityAction<DamageData> OnTakeDamage;
    public UnityAction<DamageData> OnTookDamage;

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
        OnTakeDamage?.Invoke(dama);
        Damaged(dama.getDamage());
        OnTookDamage?.Invoke(dama);
    }
    public virtual Vector3 getPosition()
    {
        return transform.position;
    }
    public abstract Entity TargetFire
    {
        get; set;
    }

    protected virtual void Damaged(int damage)
    {
        Heath -= damage;
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
