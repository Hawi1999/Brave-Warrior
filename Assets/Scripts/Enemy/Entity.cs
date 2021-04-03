using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour, IFindTarget
{
    [SerializeField] protected int HeathBasic = 50;
    [SerializeField] protected Vector2 sizeDefault = new Vector2(1, 1);
    public virtual int MaxHP { get; set; }
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
            if (old != _CurrentHeath)
            {
                OnValueChanged?.Invoke(HP);
            }
        }
    }
    protected virtual bool PermitMove => LockMove.isOk;
    protected virtual bool PermitAttack => LockAttack.isOk;
    protected virtual bool PermitSkill => LockUseSkill.isOk;
    public virtual bool IsForFind => (isActiveAndEnabled && !Died.Value);
    public SpriteRenderer render;

    public Vector3 Direction
    {get;set;
    }
    public Vector3 DirectionCurrent;
    public abstract TakeBuff take { get; }
    // Được goi khi Entity Die
    public UnityAction<Entity> OnDeath;
    public UnityAction<Entity> OnRivive;
    // Được gọi khi Buff nào đó xuất hiện hay biến mất
    public UnityAction<int> OnValueChanged;
    // Chỉnh sửa sát thương trước khi nhân
    public UnityAction<DamageData> OnTakeDamage;
    // Xem Damage sau khi nhận
    public UnityAction<DamageData> OnTookDamage;
    public LockAction LockMove = new LockAction();
    public LockAction LockAttack = new LockAction();
    public LockAction LockColliderTakeDamage = new LockAction();
    public LockAction LockUseSkill = new LockAction();
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
        SetUpStartEvents();
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
        SetUpStartvalue();
    }

    protected virtual void Update()
    {
        if (transform.hasChanged)
        {
            OnValueChanged?.Invoke(TRANSFORM);
        }
    }
    protected void Death()
    {
        Died.Value = true;
        OnDead();
        OnDeath?.Invoke(this);
    }
    protected virtual void OnDead()
    {
    }

    public void AddHealth(int a)
    {
        CurrentHeath += a;
    }
    #region TakeDamage
    public virtual void TakeDamage(DamageData dama)
    {
        dama.To = this;
        if (!dama.Dodged)
        {
            OnTakeDamage?.Invoke(dama);
        }
        Damaged(dama.Damage);
        OnTookDamage?.Invoke(dama);
        CheckHP(dama);
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
    public abstract IFindTarget TargetFire
    {
        get; set;
    }
    protected virtual void Damaged(int damage)
    {
        CurrentHeath -= damage;
    }
    protected virtual void WhenValueChanged(int code)
    {

    }
    protected virtual void VFXTookDamage(DamageData damadata)
    {
        Force.BackForce(gameObject, damadata.Direction, damadata.BackForce, 0.2f);
     }
    // CallBack from iTween

    #endregion 
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
    protected virtual void CheckHP(DamageData damage)
    {
        if (CurrentHeath <= 0)
        {
            OnValueChanged?.Invoke(DIED);
            Death();
        }
    }
    protected ValueBool Died = new ValueBool(false);
    public virtual bool IsALive
    {
        get
        {
            return !Died.Value;
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

    public ValueFloat ScaleCurrent = new ValueFloat(1);
    public bool IsWeapon(Weapon weapon)
    {
        if (WeaponCurrent == null)
            return false;
        return WeaponCurrent == weapon;
    }
    public virtual Vector3 GetPosition()
    {
        return transform.position;
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
    protected  virtual void SetUpStartEvents()
    {
        LockAttack.OnValueChanged += () => OnValueChanged?.Invoke(LOCK_ATTACK);
        LockMove.OnValueChanged += () => OnValueChanged?.Invoke(LOCK_MOVE);
        LockColliderTakeDamage.OnValueChanged += () => OnValueChanged?.Invoke(LOCK_COLLIDER_TAKEDAMAGE);
        Died.OnValueChanged += () => OnValueChanged?.Invoke(DIED);
        ScaleCurrent.OnValueChanged += () => OnValueChanged?.Invoke(SCALESIZE);

        take.OnBuffRegisterValueChanged += OnBuffRegistersValueChanged;
        take.OnValueChanged += OnTakeBuffValueChanged;

        OnValueChanged += WhenValueChanged;

        OnTakeDamage += XLD;
        OnTookDamage += VFXTookDamage;
        OnIntoTheGound += InvokeHide;
        OnOuttoTheGound += InvokeAppear;
    }

    protected virtual void SetUpEndEvent()
    {
        LockAttack.OnValueChanged -= () => OnValueChanged?.Invoke(LOCK_ATTACK);
        LockMove.OnValueChanged -= () => OnValueChanged?.Invoke(LOCK_MOVE);
        LockColliderTakeDamage.OnValueChanged -= () => OnValueChanged?.Invoke(LOCK_COLLIDER_TAKEDAMAGE);
        Died.OnValueChanged -= () => OnValueChanged?.Invoke(DIED);
        ScaleCurrent.OnValueChanged -= () => OnValueChanged?.Invoke(SCALESIZE);

        take.OnBuffRegisterValueChanged -= OnBuffRegistersValueChanged;
        take.OnValueChanged -= OnTakeBuffValueChanged;

        OnValueChanged += WhenValueChanged;

        OnTakeDamage -= XLD;
        OnTookDamage -= VFXTookDamage;
        OnIntoTheGound -= InvokeHide;
        OnOuttoTheGound -= InvokeAppear;
    }

    protected virtual void SetUpStartvalue()
    {
        MaxHP = (int)(take.GetValue(BuffRegister.TypeBuff.IncreaseMaxHealthByPercent) * HeathBasic + HeathBasic + take.GetValue(BuffRegister.TypeBuff.IncreaseMaxHealthByValue));

        CurrentHeath = MaxHP;
    }

    protected virtual void OnBuffRegistersValueChanged(BuffRegister.TypeBuff Type, ChangesValue changes)
    {
        if (Type == BuffRegister.TypeBuff.IncreaseSizeByPercent)
        {
            ScaleCurrent.Value = 1 + take.GetValue(BuffRegister.TypeBuff.IncreaseSizeByPercent);
            transform.localScale = new Vector3(ScaleCurrent.Value, ScaleCurrent.Value, 1);
        }
    }

    protected virtual void OnTakeBuffValueChanged(int a)
    {

    }

    protected virtual void OnDestroy()
    {
        SetUpEndEvent();
    }
    public virtual void setLimitMove(Vector2[] vector2s)
    {
        limitMove = vector2s;
    }
    #region IFindTarget
    public virtual TypeTarget typeTarget => TypeTarget.Enemy;
    public virtual Vector2 center => transform.position;
    public virtual Vector2 size => sizeDefault * ScaleCurrent.Value;

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
    public virtual void OnTargetFound(Entity entity, IFindTarget t)
    {

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    #endregion

    #region ValueChanged 
    public static int LOCK_ATTACK = 1;
    public static int LOCK_MOVE = 2;
    public static int LOCK_COLLIDER_TAKEDAMAGE = 3;
    public static int DIED = 4;
    public static int TIED = 5;
    public static int SCALESIZE = 6;
    public static int TRANSFORM = 7;
    public static int HP;
    public static int MAPHP;
    public static int SHIELD;
    public static int MAXSHIELD;
    public static int HEALPHY;

    public static int HARMFUL_ELECTIC = 100;
    public static int HARMFUL_POISON = 101;
    public static int HARMFUL_FIRE = 102;
    public static int HARMFUL_ICE = 103;
    [HideInInspector]
    public bool Harmful_Electric = false;
    [HideInInspector]
    public bool Harmful_Poison = false;
    [HideInInspector]
    public bool Harmful_Fire = false;
    [HideInInspector]
    public bool Harmful_Ice = false;
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

    public virtual void Revive()
    {
        OnRivive?.Invoke(this);
    }


}