using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimationQ))]
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
    [SerializeField] private Vector3 OffSetWeapon;
    public override int MaxHP => maxHP;
    public override int Heath
    {
        get
        {
            return heath;
        }
        set
        {
            int old = heath;
            heath = Mathf.Clamp(value, 0, MaxHP);
            if (PlayerCurrent == this)
                OnHeathChanged?.Invoke(old, heath, MaxHP);
            if (heath <= 0)
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
    SpriteRenderer Render => GetComponent<SpriteRenderer>();
    AnimationQ anim => GetComponent<AnimationQ>();
    Rigidbody2D rig => GetComponent<Rigidbody2D>();
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
    private Entity targetfire;
    public override Entity TargetFire
    {
        get
        {
            return targetfire;
        }
        set
        {
            Entity old = targetfire;
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
            return transform.position + OffSetWeapon;
        }
    }
    public bool HasEnemyAliveNear
    {
        get
        {
            if (TargetFire == null) return false;
            return TargetFire.IsALive;
        }
    }
    private void Awake()
    {
        gameObject.tag = "Player";
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Heath = MaxHP;
        ShieldCurrent = MaxShield;
        CurrentHealPhy = 0;
    }

    public void UseHealPhy(float percent)
    {
        CurrentHealPhy += Mathf.Clamp(MaxHealphy * percent, 0, MaxHealphy);
    }

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
            if (Render != null)
                Render.sortingOrder = (int)(-10 * transform.position.y);
            if (RenderSelected != null)
                RenderSelected.sortingOrder = (int)(-10 * transform.position.y) - 1;
        }
        TargetFire = FindEnemy.FindEnemyNearest(transform.position, 8f, layerTargetFind);
        CheckDistanceWithTargetFire();
        setDirecFire();
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
        }

    }
    void CheckDistanceWithTargetFire()
    {
        if (!HasEnemyAliveNear)
            return;
        if (Vector2.Distance(TargetFire.transform.position, transform.position) >= 8.1f)
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
    
    void setDirecFire()
    {
        if (HasEnemyAliveNear && WeaponCurrent != null)
        {
            DirectFire = (TargetFire.PositionColliderTakeDamage - WeaponCurrent.PositionStartAttack).normalized;
        } else
        {
            if (Direction != Vector3.zero)
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
            damageData.Decrease(UnityEngine.Random.Range(-1, 2));
            OnSetUpDamageToAttack?.Invoke(damageData);
            if (WeaponCurrent.Attack(damageData.Clone))
            {
                OnAttacked?.Invoke();
            }
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
    public void Move(Vector2 a)
    {
        if (PermitMove && (a != Vector2.zero))
        {
            if (HasEnemyAliveNear)
            {
                if (DirectFire.x < 0) Render.flipX = true;
                else Render.flipX = false;
            }
            else
                {
                    if (a.x < 0) Render.flipX = true;
                    else Render.flipX = false;
                }
            if (rig != null)
                rig.velocity = a.normalized * speed;
            SetAni("Run");
        } else
        {
            if (rig != null)
                rig.velocity = Vector2.zero;
            SetAni("Idle"); 
        }
        fixTransform();
    }
    public void SetAni(string code)
    {
        if (anim != null)
            anim.setAnimation(code);
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
        Heath -= dam;
        if (PlayerCurrent == this && d != 0) 
            OnReceiveDamage?.Invoke();
        lastTimeReceivedamage = Time.time;
    }
    protected override void Death()
    {
        OnDeath?.Invoke(this);
    }
    public override Vector3 getPosition()
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

    public override Vector2 size => new Vector2(0.5f, 0.5f);

    public override Vector2 center => (Vector2)transform.position + new Vector2(0, 0.2f);
    

    public static UnityAction<Enemy> OnTargetFireChanged;
    public static UnityAction<int, int, int> OnShieldChanged;
    public static UnityAction<float, float, float, bool> OnHealPhyChanged;
    public static UnityAction<int, int, int> OnHeathChanged;
    public static UnityAction<DamageData> OnSetUpDamageToAttack;
    // Gọi khi tấn công
    public static UnityAction OnAttacked;
    //
    public static UnityAction OnHit;
    // Gọi khi nhân dame nào đó
    public static UnityAction OnReceiveDamage;

    void Reset()
    {

    }
}
