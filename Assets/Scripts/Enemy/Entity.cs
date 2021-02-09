using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoolAction
{
    private bool _IsOK;
    private int ptrue = 0;
    private int pfalse = 0;
    public bool IsOK
    {
        set
        {
            _IsOK = value;
        }
        get
        {
            if (Priority_level_true > Priority_level_false)
            {
                return true;
            }
            if (Priority_level_true < Priority_level_false)
            {
                return false;
            }
            return _IsOK;
        }
    }
    public int Priority_level_true
    {
        get
        {
            return ptrue;
        }
        set
        {
            if (value > ptrue)
            {
                ptrue = value;
            }
        }
    }
    public int Priority_level_false
    {
        get
        {
            return pfalse;
        }
        set
        {
            if (value > pfalse)
            {
                pfalse = value;
            }
        }
    }
    public BoolAction(bool a)
    {
        IsOK = a;
    }
}

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
        }
    }

    protected virtual bool PermitMove
    {
        get
        {
            BoolAction pm = new BoolAction(true);
            OnCheckForMove?.Invoke(pm);
            return pm.IsOK;
        }
    }

    protected virtual bool PermitAttack
    {
        get
        {
            BoolAction check = new BoolAction(true);
            OnCheckForAttack?.Invoke(check);
            return check.IsOK;
        }
    }
    // Được gọi khi HO thay đổi
    public UnityAction<int, int, int> OnHPChanged;
    // Được goi khi Entity Die
    public UnityAction<Entity> OnDeath;
    // Được gọi khi Buff nào đó xuất hiện hay biến mất
    public UnityAction<DamageElement, bool> OnBuffsChanged;
    // Chỉnh sửa sát thương trước khi nhân
    public UnityAction<DamageData> OnTakeDamage;
    // Xem Damage sau khi nhận
    public UnityAction<DamageData> OnTookDamage;
    // Kiểm tra được phép di chuyển hay không
    public UnityAction<BoolAction> OnCheckForMove;
    // Kiểm tra được phép tấn công hay không
    public UnityAction<BoolAction> OnCheckForAttack;
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

        dama.To = this;
        OnTakeDamage?.Invoke(dama);
        Damaged(dama.Damage);
        OnTookDamage?.Invoke(dama);
        CheckHP();
    }

    protected virtual void XuLyDan(DamageData damadata)
    {

        if (damadata.Type == DamageElement.Normal)
        {
            XuLyDanNormal(damadata);
        } else
        if (damadata.Type == DamageElement.Electric)
        {
            XuLyDanElec(damadata);
        } else
        if (damadata.Type == DamageElement.Fire)
        {
            XuLyDanFire(damadata);
        } else
        if (damadata.Type == DamageElement.Ice)
        {
            XuLyDanIce(damadata);
        } else
        if (damadata.Type == DamageElement.Poison)
        {
            XuLyDanPoison(damadata);
        }
    }

    protected virtual void XuLyDanNormal(DamageData damageData)
    {

    }
    protected virtual void XuLyDanElec(DamageData damageData)
    {
        Electrified.Shockwave(this, damageData.timeGiatDien);
    }
    protected virtual void XuLyDanFire(DamageData damageData)
    {
        if (!damageData.FireFrom)
        {
            Freeze fre = GetComponent<Freeze>();
            if (fre != null)
            {
                fre.EndUp();
                damageData.Mediated = true;
            }
            else
            if (Random.Range(0, 1f) < damageData.FireRatio)
            {
                Burnt.Chay(this, damageData.FireTime);
            }
        }
        else
        {
            damageData.Damage = (int)Mathf.Clamp(Burnt.Tile * MaxHP, Burnt.MinDamage, Burnt.MaxDamage);
        }
    }
    protected virtual void XuLyDanPoison(DamageData damageData)
    {
        if (!damageData.PoisonFrom)
        {
            if (Random.Range(0, 1f) < damageData.PoisonRatio)
            {
                Poisoned.NhiemDoc(this, damageData.PoisonTime);
            }
        }
        else
        {
            damageData.Damage = (int)Mathf.Clamp(Poisoned.Tile * MaxHP, Poisoned.MinDamage, Poisoned.MaxDamage);
        }
    }
    protected virtual void XuLyDanIce(DamageData damaData)
    {
        Burnt b = GetComponent<Burnt>();
        if (b)
        {
            damaData.Mediated = true;
            b.EndUp();
        }
        else
        {
            if (Random.Range(0, 1f) < damaData.IceRatio)
            {
                Debug.Log("Da Dong bang");
                Freeze.Freezed(this, damaData.IceTime);
            }
        }
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
    protected void CheckHP()
    {
        if (Heath <= 0)
        {
            Death();
        }
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

    private void BounceBack()
    {
        
    }

    public virtual Vector2 getSize()
    {
        return transform.localScale;
    }

    public virtual Vector2 getCenter()
    {
        return transform.position;
    }


}
