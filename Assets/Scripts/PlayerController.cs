using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SkillConnect
{
    public Skill skill;
    [Tooltip("Thường thì đặt là SkillOne, SkillTwo")]
    public string CodeControl;
}
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : Entity
{
    public static PlayerController PlayerCurrent;
    public static TakeBuff PlayerTakeBuff = new TakeBuff();
    public Collider2D ColliderTakeDamaged;
    [SerializeField] private float speed = 1;
    [SerializeField] private int shield = 5;
    [SerializeField] private int healPhy = 100;
    [SerializeField] private SpriteRenderer RenderSelected;
    public LayerMask layerTargetFind;
    [SerializeField] private Transform OffSetWeapon;
    [SerializeField] private Transform OffSetCenter;
    public List<SkillConnect> skills;
    public override TakeBuff take => PlayerTakeBuff;
    public ChooseMinapulation PLayerChoose => ChooseMinapulation.PlayerChoose;
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
        }
    }
    public int MaxShield;
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
                OnHealPhyChanged?.Invoke(o, value, MaxHealphy);
        }
    }
    public override bool IsForFind => base.IsForFind && !Died.Value;
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
                if (old != null && old as UnityEngine.Object != null)
                {
                    old.OnTargetFound(this, targetfire);
                }
                if (targetfire != null && targetfire as UnityEngine.Object != null)
                {
                    targetfire.OnTargetFound(this, targetfire);
                }
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
    public bool LocKDirectFire = false;
    public override TypeTarget typeTarget => TypeTarget.Player;
    Animator anim;
    Rigidbody2D rig;

    public ValueBool Tied = new ValueBool(false);
    private IFindTarget targetfire;
    #region Start & Update
    protected override void SetUpAwake()
    {

        base.SetUpAwake();
        gameObject.tag = "Player";
        gameObject.layer = LayerMask.NameToLayer("Player");
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        OnAttacked += TakeTied;
    }

    protected override void SetUpStart()
    {
        base.SetUpStart();
        if (PlayerCurrent == this)
        {
            OnHeathChanged?.Invoke(0, CurrentHeath, MaxHP);
            OnShieldChanged?.Invoke(0, ShieldCurrent, MaxShield);
            OnHealPhyChanged?.Invoke(0, CurrentHealPhy, MaxHealphy);

        }

    }

    protected override void SetUpStartvalue()
    {
        base.SetUpStartvalue();
        MaxShield = (int)(take.GetValue(BuffRegister.TypeBuff.IncreaseMaxShieldByPercent) * shield + shield + take.GetValue(BuffRegister.TypeBuff.IncreaseMaxShieldByValue));

        ShieldCurrent = MaxShield;
        CurrentHealPhy = 0;
    }
    protected override void SetUpStartEvents()
    {
        base.SetUpStartEvents();
        Tied.OnValueChanged += () => OnValueChanged?.Invoke(TIED);
    }

    protected override void SetUpEndEvent()
    {
        base.SetUpEndEvent();
        Tied.OnValueChanged -= () => OnValueChanged?.Invoke(TIED);
    }

    protected override void Update()
    {
        if (PlayerCurrent != this)
            return;
        base.Update();
        UpdateSorting();
        FindTargetFire();
        CheckDistanceWithTargetFire();
        UpdateDirection();
        UpdateDirecFire();
        UpdateRenderFlip();
        CheckShield();
        CheckHealPhy();
        CheckInPut();
        UpdateCamera();
    }
    void UpdateSorting()
    {
        if (transform.hasChanged)
        {
            if (render != null)
                render.sortingOrder = (int)(-10 * transform.position.y);
            if (RenderSelected != null)
                RenderSelected.sortingOrder = (int)(-10 * transform.position.y) - 1;
        }
    }

    protected override void WhenValueChanged(int code)
    {
        if (code == TIED)
        {
            if (Tied.Value)
            {
                LockUseSkill.Register("Tied");
                LockAttack.Register("Tied");
            } else
            {
                LockUseSkill.CancelRegistration("Tied");
                LockAttack.CancelRegistration("Tied");
            }
        }
        if (code == DIED)
        {

            if (Died.Value)
            {
                LockMove.Register("Tied");
                LockUseSkill.Register("Tied");
                LockAttack.Register("Tied");
            }
            else
            {
                LockMove.CancelRegistration("Tied");
                LockUseSkill.CancelRegistration("Tied");
                LockAttack.CancelRegistration("Tied");
            }
        }
    }
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
    private void UpdateDirection()
    {
        Joystick MyJoy = GameController.MyJoy;
        if (MyJoy == null)
        {
            return;
        }
        Direction = MyJoy.Direction.normalized;
        if (Direction != Vector3.zero)
        {
            DirectionCurrent = Direction;
        }
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
            if (TargetFire == null || (TargetFire as UnityEngine.Object) == null || !TargetFire.IsForFind)
            {
                Find();
            }
        }
    }
    void Find()
    {
        TargetFire = global::Find.FindTargetNearest(transform.position, DistanceFindTarget, layerTargetFind);
        timeToFind = 0;
    }
    void CheckDistanceWithTargetFire()
    {
        if (!HasEnemyAliveNear)
            return;
        if (Vector2.Distance(TargetFire.center, transform.position) >= DistanceFindTarget)
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
                Tied.Value = true;
            }
        } else
        {
            Tied.Value = false;
        }
    }
    void CheckInPut()
    {
        if ((Control.GetKey("Attack") || Input.GetKey(KeyCode.X)) && WeaponCurrent != null && PermitAttack)
        {
            Attack();
        }
        else
        {
            if (Control.GetKey("Attack") || Input.GetKey(KeyCode.X) && PermitAttack)
            {
                Debug.Log("Chưa trang vị vũ khí");
            }
            else
            {
                OnNotAttack?.Invoke();
            }
        }
        if (PermitSkill)
        {
            if (skills != null && skills.Count != 0)
            {
                foreach(SkillConnect skill in skills)
                {
                    if (skill.skill != null && skill.skill.isReady)
                    {
                        if (Control.GetKeyDown(skill.CodeControl))
                        {
                            skill.skill.StartSkill();
                        }
                    }
                }
            }
        }
    }
    void UpdateDirecFire()
    {
        if (LocKDirectFire)
            return;
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
    void UpdateCamera()
    {
        CameraMove.Instance.AddPosition(new TaretVector3("Player", center));
        if (TargetFire != null && TargetFire as UnityEngine.Object != null)
        {
            CameraMove.Instance.AddPosition(new TaretVector3("Enemy", TargetFire.center));
        }
        else
        {
            CameraMove.Instance.RemovePosition("Enemy");
        }
    }

    #endregion

    #region Attack & Damaged
    void Attack()
    {
        DamageData damageData = new DamageData();
        SetUpDamageData(damageData);
        if (WeaponCurrent.Attack(damageData.Clone))
        {
            OnAttacked?.Invoke();
        } else
        {
            OnNotAttack?.Invoke();
        }
    }
    

    protected override void SetUpDamageData(DamageData damageData)
    {
        base.SetUpDamageData(damageData);
        damageData.Direction = DirectFire;
        damageData.OnHitEnemy += (e) => OnHitTarget?.Invoke(e);
        damageData.OnHitToDieEnemy += (Enemy enemy) =>
        {
            OnKilledTarget?.Invoke(enemy);
        };
        damageData.AddDamageOrigin(PlayerTakeBuff.GetValue(BuffRegister.TypeBuff.IncreaseDamageByValue));
        damageData.AddDamagePercentOrigin(PlayerTakeBuff.GetValue(BuffRegister.TypeBuff.IncreaseDamageByPercent));
        damageData.AddDamageCritByPercent(PlayerTakeBuff.GetValue(BuffRegister.TypeBuff.IncreaseDamageCritByPercent));
        damageData.AddDecrease(UnityEngine.Random.Range(-1, 2));
        OnSetUpDamageToAttack?.Invoke(damageData);
    }

    protected override void XLDFireNotFireFrom(DamageData damageData)
    {
        if (damageData.FromTNT)
        {
            damageData.AddDecreaseByPercent(0.8f);
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
    public void Equipment(Weapon w)
    {
        WeaponCurrent = w;
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
    public void UseHealPhy(float percent)
    {
        CurrentHealPhy += Mathf.Clamp(MaxHealphy * percent, 0, MaxHealphy);
    }

    protected override void OnDead()
    {
        base.OnDead();
        gameObject.SetActive(false);
    }

    public override void Revive()
    {
        base.Revive();
        Died.Value = false;
        CurrentHeath = MaxHP;
        ShieldCurrent = MaxShield;
        OnShieldChanged?.Invoke(0, MaxShield, MaxShield);
        CurrentHealPhy = 0;
        Debug.Log("Revived");
    }

    #endregion

    #region Info & Action
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
    public static UnityAction<int, int, int> OnShieldChanged;
    public static UnityAction<float, float, float> OnHealPhyChanged;
    public static UnityAction<int, int, int> OnHeathChanged;
    public static UnityAction<DamageData> OnSetUpDamageToAttack;
    // Gọi khi nhân dame nào đó
    public static UnityAction OnReceiveDamage;

    #endregion

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
        Gizmos.DrawWireCube(center, size);
    }
}