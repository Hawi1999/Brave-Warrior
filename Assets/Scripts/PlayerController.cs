using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : Entity
{
    public static PlayerController PlayerCurrent;
    public Collider2D ColliderTakeDamaged;
    [SerializeField] int maxHP;
    [SerializeField] private float speed = 1;
    [SerializeField] private int shield = 5;
    [SerializeField] private int healPhy = 100;
    [SerializeField] private SpriteRenderer RenderSelected;
    [SerializeField] private LayerMask layerTargetFind;
    [SerializeField] private Transform OffSetWeapon;
    [SerializeField] private Transform OffSetCenter;
    [SerializeField] private Vector2 sizeBody = new Vector2(0.7f, 0.7f);
    public override int MaxHP => maxHP;
    public override int CurrentHeath
    {
        get
        {
            return _CurrentHeath;
        }
        set
        {
            int old = _CurrentHeath;
            _CurrentHeath = Mathf.Clamp(value, 0, MaxHP);
            if (PlayerCurrent == this)
                OnHeathChanged?.Invoke(old, _CurrentHeath, MaxHP);
            if (_CurrentHeath <= 0)
            {
                Death();
            }
        }
    }
    public int MaxShield => shield;
    int _currentShield;
    public int ShieldCurrent
    {
        get
        {
            return _currentShield;
        }
        private set
        {
            int o = _currentShield;
            _currentShield = value;
            if (PlayerCurrent == this)
                OnShieldChanged?.Invoke(o, value, MaxShield);
        }
    }
    private float lastTimeReceivedamage;
    public int MaxHealphy => healPhy;
    float _currentHealphy;
    public float CurrentHealPhy
    {
        get
        {
            return _currentHealphy;
        }
        private set
        {
            float o = _currentHealphy;
            _currentHealphy = value;
            if (PlayerCurrent == this)
                OnHealPhyChanged?.Invoke(o, value, MaxHealphy, Tied);
        }
    }
    public override bool IsForFind => base.IsForFind && !Died;
    Animator anim;
    Rigidbody2D rig;
    private bool Tied;
    protected override bool PermitAttack
    {
        get
        {
            BoolAction check = new BoolAction(true);
            OnCheckForAttack?.Invoke(check);
            return check.IsOK && !Tied;
        }
    }
    private IFindTarget targetfire;
    public override IFindTarget TargetFire
    {
        get
        {
            return targetfire;
        }
        set
        {
            IFindTarget old = targetfire;
            targetfire = value;
            if (old != targetfire)    
            {
                OnTargetFireChanged?.Invoke(targetfire as Enemy);
            }
        }
    }
    public override Vector3 PositionSpawnWeapon
    {
        get
        {
            if (OffSetWeapon == null)
            {
                return base.center;
            }
            else
            {
                return OffSetWeapon.position;
            }
        }
    }
    public bool HasEnemyAliveNear
    {
        get
        {
            if (TargetFire == null ||  TargetFire as UnityEngine.Object == null) return false;
            return TargetFire.IsForFind;
        }
    }
    public override TypeTarget typeTarget => TypeTarget.Player;
    protected override void Awake()
    {
        base.Awake();
        gameObject.tag = "Player";
        gameObject.layer = LayerMask.NameToLayer("Player");
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        OnAttacked += TakeTied;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        CurrentHeath = MaxHP;
        ShieldCurrent = MaxShield;
        CurrentHealPhy = 0;
    }

    /// <summary>
    /// Tổn hao sức khỏe Player
    /// </summary>
    /// <param name="percent"> phần trăm sức khỏe sử dụng </param>
    public void UseHealPhy(float percent)
    {
        CurrentHealPhy += Mathf.Clamp(MaxHealphy * percent, 0, MaxHealphy);
    }

    /// <summary>
    /// Trang bị vũ khí
    /// </summary>
    /// <param name="w"> Vũ khí trang bị </param>
    public void Equipment(Weapon w)
    {
        WeaponCurrent = w;
    }

    private void Update()
    {
        if (PlayerCurrent != this)
            return;
        if (transform.hasChanged)
        {
            if (render != null)
                render.sortingOrder = (int)(-10 * transform.position.y);
            if (RenderSelected != null)
                RenderSelected.sortingOrder = (int)(-10 * transform.position.y) - 1;
        }
        FindTargetFire();
        CheckDistanceWithTargetFire();
        UpdateDirection();
        UpdateDirecFire();
        UpdateRenderFlip();
        CheckShield();
        CheckHealPhy();
        if ((Control.GetKey("Attack") || Input.GetKey(KeyCode.X)) && WeaponCurrent != null)
        {
            Attack();
        } else
        {
            if (Control.GetKey("Attack") || Input.GetKey(KeyCode.X))
            {
                Debug.Log("Chưa trang vị vũ khí");
            }
            else
            {
                OnNotAttack?.Invoke();
            }
        }
    }

    /// <summary>
    /// Flip Render cho đúng hướng di chuyển
    /// </summary>
    private void UpdateRenderFlip()
    {
        render.flipX = (DirectFire.x < 0);
    }

    protected virtual void FixedUpdate()
    {
        if (PlayerCurrent == this && (Vector2)Direction != Vector2.zero && PermitMove)
        {
            Move();
            anim.SetInteger("Value", Animate_Move);
        } else
        {
            anim.SetInteger("Value", Animate_Idle);
        }
    }

    /// <summary>
    /// Cập nhật hướng Player
    /// </summary>
    private void UpdateDirection()
    {
        Joystick MyJoy = GameController.MyJoy;
        if (MyJoy == null)
        {
            return;
        }
        Direction = MyJoy.Direction.normalized;
    }
    protected void Move()
    {
        rig.MovePosition(transform.position + Direction * Time.fixedDeltaTime * speed);
        fixTransform();
    }

    float timeToFind = 0;
    void FindTargetFire()
    {
        timeToFind += Time.deltaTime;
        if (timeToFind >= 0.1f)
        {
            Find();
        } else
        {
            if (TargetFire != null && (TargetFire as UnityEngine.Object) != null && !TargetFire.IsForFind)
            {
                Find();
            }
        }
    }

    void Find()
    {
        TargetFire = global::Find.FindTargetNearest(transform.position, 12f, layerTargetFind);
        timeToFind = 0;
    }
    void CheckDistanceWithTargetFire()
    {
        if (!HasEnemyAliveNear)
            return;
        if (Vector2.Distance(TargetFire.center, transform.position) >= 8.1f)
        {
            TargetFire = null;
        }
    }
    void CheckShield()
    {
        if (Time.time - lastTimeReceivedamage > 5f && _currentShield < MaxShield)
        {
            ShieldCurrent += 1;
            lastTimeReceivedamage += 1;
        }
    }
    void CheckHealPhy()
    {
        if (CurrentHealPhy > 0)
        {
            CurrentHealPhy = Mathf.Clamp(_currentHealphy - Time.deltaTime * MaxHealphy / 2.5f, 0, MaxHealphy);
            if (CurrentHealPhy == MaxHealphy)
            {
                Tied = true;
            }
        } else
        {
            Tied = false;
        }
    }
    
    void UpdateDirecFire()
    {
        if (HasEnemyAliveNear && WeaponCurrent != null)
        {
            Vector2 from = WeaponCurrent.transform.position;
            Vector2 to = TargetFire.center;
            DirectFire = (to - from).normalized;
        } else
        {
            if ((Vector2)Direction != Vector2.zero)
            {
                DirectFire = Direction;
            }
        }
    }
    void Attack()
    {
        if (PermitAttack)
        {
            DamageData damageData = new DamageData();
            damageData.From = this;
            damageData.Decrease(UnityEngine.Random.Range(-1, 2));
            OnSetUpDamageToAttack?.Invoke(damageData);
            if (WeaponCurrent.Attack(damageData.Clone))
            {
                OnAttacked?.Invoke();
            } else
            {
                OnNotAttack?.Invoke();
            }
        } else
        {
            OnNotAttack?.Invoke();
        }
    }
    protected override void XLDFireNotFireFrom(DamageData damageData)
    {
        if (damageData.FromTNT)
        {
            damageData.DecreaseByPercent(0.8f);
            damageData.FireTime = 1f;
        }
        base.XLDFireNotFireFrom(damageData);
    }

    protected override void XLDFireFireFrom(DamageData damageData)
    {
        damageData.Damage = 1;
    }

    protected override void XLDPoison(DamageData damageData)
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
            damageData.Damage = 2;
        }
    }
    protected override void Damaged(int dam)
    {
        int d = dam;
        if (dam > ShieldCurrent)
        {
            dam = dam - ShieldCurrent;
            ShieldCurrent = 0;
        }
        else
        {
            ShieldCurrent -= dam;
            dam = 0;
        }
        CurrentHeath -= dam;
        if (PlayerCurrent == this && d != 0) 
            OnReceiveDamage?.Invoke();
        lastTimeReceivedamage = Time.time;
    }
    public override Vector3 GetPosition()
    {
        return transform.position + new Vector3(0,0.25f,0);
    }

    public override void OnEquipment(Weapon weapon)
    {
        OnAttacked += weapon.OnAttacked;
    }

    public override void OnUnEquipment(Weapon weapon)
    {
        OnAttacked -= weapon.OnAttacked;
    }

    public override void OnPocket(Weapon weapon)
    {
        OnAttacked -= weapon.OnAttacked;
    }

    protected virtual void TakeTied()
    {
        if (WeaponCurrent != null)
        {
            UseHealPhy(WeaponCurrent.TakeTied);
        }
    }

    public override Vector2 size => sizeBody;

    public override Vector2 center
    {
        get
        {
            if (OffSetCenter == null)
            {
                return base.center;
            } else
            {
                return OffSetCenter.position;
            }
        }
    }


    public static UnityAction<Enemy> OnTargetFireChanged;
    public static UnityAction<int, int, int> OnShieldChanged;
    public static UnityAction<float, float, float, bool> OnHealPhyChanged;
    public static UnityAction<int, int, int> OnHeathChanged;
    public static UnityAction<DamageData> OnSetUpDamageToAttack;
    // Gọi khi nhân dame nào đó
    public static UnityAction OnReceiveDamage;

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        rig.velocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        if (TargetFire != null &&  TargetFire as UnityEngine.Object != null && WeaponCurrent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(TargetFire.center, WeaponCurrent.transform.position);
            Gizmos.DrawLine(WeaponCurrent.transform.position, WeaponCurrent.transform.position + DirectFire * 10);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawCube(center, size);
    }
}