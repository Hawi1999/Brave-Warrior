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

public abstract class Entity : MonoBehaviour, IFindTarget
{
    [SerializeField] protected int Heath = 50;
    [SerializeField] protected int Shield = 0;
    public virtual int MaxHP => Heath;
    protected int _CurrentHeath;
    public virtual int CurrentHeath
    {
        get
        {
            return _CurrentHeath;
        }
        set
        {
            int old = _CurrentHeath;
            _CurrentHeath = Mathf.Clamp(value, 0, MaxHP);
            OnHPChanged?.Invoke(old, _CurrentHeath, MaxHP);
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

    protected virtual bool PermitDown
    {
        get
        {
            BoolAction check = new BoolAction(true);
            OnCheckForDown?.Invoke(check);
            return check.IsOK;
        }
    }
    public virtual bool IsForFind => (isActiveAndEnabled && !Died);
    [SerializeField]
    protected SpriteRenderer render;

    public Vector3 Direction
    {get;set;
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
    // Kiểm tra được phép Down hay không
    public UnityAction<BoolAction> OnCheckForDown;
    [Tooltip("được gọi khi chui xuống đất")]
    public UnityAction OnIntoTheGound;
    [Tooltip("được gọi khi chui xuống đất")]
    public UnityAction OnOuttoTheGound;

    [Tooltip("được gọi mỗi Frame khi tấn công")]
    public UnityAction OnAttacked;
    [Tooltip("được gọi mỗi Frame khi không tấn công")]
    public UnityAction OnNotAttack;
    [Tooltip("được gọi khi biến mất")]
    public UnityAction OnHide;
    [Tooltip("được gọi khi biến mất")]
    public UnityAction OnAppear;
    protected Weapon LastWeapon;
    private Weapon weapon;
    protected Weapon WeaponCurrent
    {
        get
        {
            return weapon;
        }
        set
        {
            if (value != weapon)
            {
                LastWeapon = weapon;
                if (weapon != null )
                {
                    weapon.ChangEQuip(this, WeaponStatus.Free);
                    OnUnEquipment(weapon);
                }
                weapon = value;
                if (value != null)
                {
                    weapon.ChangEQuip(this, WeaponStatus.Equiping);
                    OnEquipment(weapon);
                }
            }
        }
    }
    protected virtual void Awake()
    {
        AddAllEvents();
        CurrentHeath = Heath;
    }

    #region Invoke UnityAction
    protected void InvokeHide()
    {
        OnHide?.Invoke();
    }

    protected void InvokeAppear()
    {
        OnAppear?.Invoke();
    }
    #endregion

    protected Vector2[] limitMove;
    protected virtual void Start()
    {

    }


    protected void Death()
    {
        Died = true;
        OnDead();
        OnDeath?.Invoke(this);
    }

    protected virtual void OnDead()
    {
    }
    public virtual void TakeDamage(DamageData dama)
    {
        dama.To = this;
        if (!dama.Dodged)
        {
            OnTakeDamage?.Invoke(dama);
        }
        Damaged(dama.Damage);
        OnTookDamage?.Invoke(dama);
        CheckHP();
    }
    protected virtual void XLD(DamageData damadata)
    {

        if (damadata.Type == DamageElement.Normal)
        {
            XLDNormal(damadata);
        } else
        if (damadata.Type == DamageElement.Electric)
        {
            XLDElec(damadata);
        } else
        if (damadata.Type == DamageElement.Fire)
        {
            XLDFire(damadata);
        } else
        if (damadata.Type == DamageElement.Ice)
        {
            XLDIce(damadata);
        } else
        if (damadata.Type == DamageElement.Poison)
        {
            XLDPoison(damadata);
        }
    }
    protected virtual void XLDNormal(DamageData damageData)
    {

    }
    protected virtual void XLDElec(DamageData damageData)
    {
        Electrified.Shockwave(this, damageData.timeGiatDien);
    }
    protected virtual void XLDFire(DamageData damageData)
    {
        if (!damageData.FireFrom)
        {
            XLDFireNotFireFrom(damageData);
        }
        else
        {
            XLDFireFireFrom(damageData);
        }
    }
    protected virtual void XLDFireNotFireFrom(DamageData damageData)
    {
        Freeze fre = GetComponent<Freeze>();
        if (fre != null)
        {
            damageData.Mediated = true;
            fre.EndUp();
        }
        else
        if (Random.Range(0, 1f) < damageData.FireRatio)
        {
            Burnt.Chay(this, damageData.FireTime);
        }
    }
    protected virtual void XLDFireFireFrom(DamageData damageData)
    {
        damageData.Damage = (int)Mathf.Clamp(Burnt.TILE * MaxHP, Burnt.MIN_DAMAGE, Burnt.MAX_DAMAGE);
    }
    protected virtual void XLDPoison(DamageData damageData)
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
            damageData.Damage = (int)Mathf.Clamp(Poisoned.TILE * MaxHP, Poisoned.MIN_DAMAGE, Poisoned.MAX_DAMAGE);
        }
    }
    protected virtual void XLDIce(DamageData damaData)
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
                Freeze.Freezed(this, damaData.IceTime);
            }
        }
    }
    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }
    public abstract IFindTarget TargetFire
    {
        get; set;
    }
    protected virtual void Damaged(int damage)
    {
        CurrentHeath -= damage;
    }
    protected virtual void VFXTookDamage(DamageData damadata)
    {
        Force.BackForce(gameObject, damadata.Direction, damadata.BackForce, 0.2f);
     }
    // CallBack from iTween
    protected void fixTransform()
    {
        if (limitMove == null)
        {
            return;
        }
        Vector3 position = transform.position;
        if (position.x < limitMove[0].x)
        {
            position.x = limitMove[0].x;
        }
        if (position.y < limitMove[0].y)
        {
            position.y = limitMove[0].y;
        }
        if (position.x > limitMove[1].x)
        {
            position.x = limitMove[1].x;
        }
        if (position.y > limitMove[1].y)
        {
            position.y = limitMove[1].y;
        }
        transform.position = position;
    }
    protected void CheckHP()
    {
        if (CurrentHeath <= 0)
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
    public virtual Vector3 PositionSpawnWeapon => center;
    public virtual Vector3 PositionColliderTakeDamage => center;
    public bool HasWeapon
    {
        get
        {
            return (WeaponCurrent != null);
        }
    }
    protected virtual float scaleCurrent => 1;
    public bool IsWeapon(Weapon weapon)
    {
        if (WeaponCurrent == null)
            return false;
        return WeaponCurrent == weapon;
    }

    public virtual void OnEquipment(Weapon weapon)
    {
    }
    public virtual void OnUnEquipment(Weapon weapon)
    {
    }
    public virtual void OnPocket(Weapon weapon)
    {

    }
    protected virtual void RemoveAllEvents()
    {
        OnTakeDamage -= XLD;
        OnTookDamage -= VFXTookDamage;
        OnIntoTheGound -= InvokeHide;
        OnOuttoTheGound -= InvokeAppear;
        OnDeath -= (a) => RemoveAllEvents();
    }

    protected virtual void AddAllEvents()
    {
        OnTakeDamage += XLD;
        OnTookDamage += VFXTookDamage;
        OnIntoTheGound += InvokeHide;
        OnOuttoTheGound += InvokeAppear;
        OnDeath += (a) => RemoveAllEvents();
    }

    protected virtual void OnDestroy()
    {

    }
    public virtual void setLimitMove(Vector2[] vector2s)
    {
        limitMove = vector2s;
    }
    #region IFindTarget
    public virtual TypeTarget typeTarget => TypeTarget.Enemy;
    public virtual Vector2 center => transform.position;
    public virtual Vector2 size => transform.localScale;

    public virtual bool isNull => (this == null);

    protected virtual Action NextAction
    {
        get; set;
    }
    protected virtual Action CurrentAction
    {
        get; set;
    }

    public enum Action
    {
        Attack,
        Down,
        EndAttack,
        StartHide,
        Hide,
        EndHide,
        Idle,
        Move,
        ReadyAttack,
        Up,
        WaitToChoose,
        Busy
    }

    protected virtual void SetUpDamageData(DamageData damage)
    {
        damage.From = this;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    #endregion

    #region  Value Animate

    protected static int Animate_Idle = 5;
    protected static int Animate_Move = 15;
    protected static int Animate_ReadyAttack = 24;
    protected static int Animate_Attack = 25;
    protected static int Animate_EndAttack = 26;
    protected static int Animate_Down = 34;
    protected static int Animate_Up = 36;
    protected static int Animate_Hide = 45;
    protected static int Animate_StartHide = 44;
    protected static int Animate_EndHide = 46;

    #endregion
    public void Spawning()
    {
        gameObject.SetActive(false);
        OnHide?.Invoke();
    }

    public void BeginInRound()
    {
        gameObject.SetActive(true);
        OnAppear?.Invoke();
    }


}