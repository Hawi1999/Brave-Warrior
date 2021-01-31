using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ChooseReward))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimationQ))]
public class PlayerController : Entity
{
    public static PlayerController Instance;
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
            OnHeathChanged?.Invoke(old, heath, MaxHP);
            if (heath == 0)
            {
                Death();
            }
        }
    }
    public int MaxShield => shield;
    int _currentShield;
    public int CurentShield
    {
        get
        {
            return _currentShield;
        }
        private set
        {
            int o = _currentShield;
            _currentShield = value;
            OnShieldChanged?.Invoke(o, value, MaxShield);
        }
    }
    private float lastTimeTakeDamage;
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
            OnHealPhyChanged?.Invoke(o, value, MaxHealphy);
        }
    }
    SpriteRenderer Render => GetComponent<SpriteRenderer>();
    AnimationQ anim => GetComponent<AnimationQ>();
    Rigidbody2D rig => GetComponent<Rigidbody2D>();
    public bool PermitMove;
    private bool Tied;
    private bool PermitAttack => !Tied;
    [HideInInspector] public Vector3 Direction = new Vector3(1, 0, 0);
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
    public bool HasEnemyNear
    {
        get
        {
            if (TargetFire == null) return false;
            return TargetFire.IsALive;
        }
    }
    

    public Transform tranSform => transform;
    private void Awake()
    {
        gameObject.tag = "Player";
    }

    // Start is called before the first frame update
    void Start()
    {
        PermitMove = true;
        Heath = MaxHP;
        CurentShield = MaxShield;
        CurrentHealPhy = 0;
    }

    public void UseHealPhy(float percent)
    {
        CurrentHealPhy += Mathf.Clamp(MaxHealphy * percent, 0, MaxHealphy);
    }

    public void TrangBi(Weapon w)
    {
        WeaponCurrent = w;
    }

    private void Update()
    {
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
        if (Control.KeyOne || Input.GetKey(KeyCode.X) && WeaponCurrent != null)
        {
            Attack();
        } else
        {
            if (Control.KeyOne || Input.GetKey(KeyCode.X))
                {
                Debug.Log("Chưa trang vị vũ khí");
            }
        }

    }
    void Attack()
    {
        if (PermitAttack)
        {
            WeaponCurrent.Attack();
        }
    }
    void CheckDistanceWithTargetFire()
    {
        if (!HasEnemyNear)
            return;
        if (Vector2.Distance(TargetFire.transform.position, transform.position) >= 8.1f)
        {
            TargetFire = null;
        }
    }
    void CheckShield()
    {
        if (Time.time - lastTimeTakeDamage > 5f && _currentShield < MaxShield)
        {
            CurentShield += 1;
            lastTimeTakeDamage += 1;
        }
    }
    void CheckHealPhy()
    {
        if (CurrentHealPhy > 0)
        {
            CurrentHealPhy = Mathf.Clamp(_currentHealphy - Time.deltaTime * MaxHealphy / 3, 0, MaxHealphy);
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
        if (HasEnemyNear && WeaponCurrent != null)
        {
            DirectFire = (TargetFire.PositionColliderTakeDamage - WeaponCurrent.viTriRaDan).normalized;
        } else
        {
            if (Direction != Vector3.zero)
            {
                DirectFire = Direction;
            }
        }
    }
    public void Move(Vector2 a)
    {
        if (PermitMove && (a != Vector2.zero))
        {
            if (HasEnemyNear)
            {
                if (DirectFire.x < 0) Render.flipX = true;
                else if (DirectFire.x > 0) Render.flipX = false;
            }
            else
                {
                    if (a.x < 0) Render.flipX = true;
                    else if (a.x > 0) Render.flipX = false;
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
    }
    public void SetAni(string code)
    {
        if (anim != null)
            anim.setAnimation(code);
    }
    protected override void Damaged(int dam)
    {
        if (dam > CurentShield)
        {
            dam = dam - CurentShield;
            CurentShield = 0;
        }
        else
        {
            CurentShield -= dam;
            dam = 0;
        }
        Heath -= dam;
        lastTimeTakeDamage = Time.time;
    }
    protected override void Death()
    {
        OnDeath?.Invoke(this);
    }
    public override Vector3 getPosition()
    {
        return base.getPosition() + new Vector3(0,1,0);
    
    }

    public UnityAction<Enemy> OnTargetFireChanged;
    public static UnityAction<int, int, int> OnShieldChanged;
    public static UnityAction<float, float, float> OnHealPhyChanged;
    public static UnityAction<int, int, int> OnHeathChanged;
}
