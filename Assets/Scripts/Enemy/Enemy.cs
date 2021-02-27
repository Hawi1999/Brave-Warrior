using UnityEngine;

public enum DamageElement
{
    Normal = 0,
    Fire = 1,
    Poison = 2,
    Ice = 4,
    Electric = 5
}


[System.Serializable]
public class EnemyData
{
    public int MaxHP = 50;
    public Vector2 Size = new Vector2(0.7f, 0.3f);
    public int Shield = 0;

    public Vector2 time_range_move = new Vector2(1f, 2f);
    public Vector2 time_range_idle = new Vector2(2f, 4f);
    public Vector2 time_range_attack = new Vector2(1f, 3f);

    public Transform PositionRaDan;
    public BulletEnemy BulletPrefabs;
}

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : Entity
{

    #region EditorVisible
    public string EnemyName = "New Enemy";
    public int Damage;
    public EnemyData ED;
    public Transform PR_HP;
    public Transform PR_HPsub;
    public Collider2D colliderTakeDamage;
    [SerializeField] protected LayerMask layerTargetFire;
    [SerializeField] float speedMove = 5;

    #endregion
    [SerializeField]
    private Vector3 a;
    #region EditorInvisible
    [HideInInspector] public DamageData LastDamageData;
    private DamageData _CurrentDamageData;
    [HideInInspector] public DamageData CurrentDamageData
    {
        set
        {
            LastDamageData = _CurrentDamageData;
            _CurrentDamageData = value;
        }
        get
        {
            return _CurrentDamageData;
        }
    }
    public override int Heath { get => base.Heath; set => base.Heath = value; }
    public virtual bool IsForFind
    {
        get
        {
            return(isActiveAndEnabled && !Died);
        }
    }
    public override Vector3 PositionColliderTakeDamage
    {
        get
        {
            if (colliderTakeDamage == null)
            {
                return gameObject.transform.position;
            } else
            {
                return colliderTakeDamage.bounds.center;
            }
        }
    }
    public override Entity TargetFire
    {get;set;
    }
    public override int MaxHP
    {
        get
        {
            return ED.MaxHP;
        }
    }
    protected PlayerController targetAttack
    {
        get
        {
            return TargetFire as PlayerController;
        }
    }
    protected bool HasTargetNear
    {
        get
        {
            if (targetAttack == null)
            {
                return false;
            }
            return Vector2.Distance(targetAttack.getPosition(), center) <= 6f;
        }
    }
    private AnimationQ anim => GetComponent<AnimationQ>();
    protected Rigidbody2D rig => GetComponent<Rigidbody2D>();
    // Di chuyển
    private bool isMoving = false;
    private Vector2 dirMove = new Vector2(0,0);
    private float time_start_move;
    private float time_start_idle;
    private float time_move;
    private float time_idle;
    // Tấn công
    private float time_start_attack;
    private float time_delay_attack;
    protected Vector2 vitriradan
    {
        get
        {
            if (ED == null || ED.PositionRaDan == null)
                return transform.position;
            else
            {
                return ED.PositionRaDan.position;
            }
            
        }
    }
    public override Vector2 size => new Vector2(scaleDefault.x * ED.Size.x, scaleDefault.y * ED.Size.y);
    public override Vector2 center => transform.position + new Vector3(0, 0.1f) * transform.localScale.y;

    public override Vector3 PositionSpawnWeapon => vitriradan;
    protected override bool PermitAttack
    {
        get
        {
            BoolAction check = new BoolAction(true);
            OnCheckForAttack?.Invoke(check);
            return check.IsOK;
        }
    }

    PoolingGameObject<BulletEnemy> pool_bullet;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        gameObject.tag = "Enemy"; 
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        if (ED != null && ED.BulletPrefabs != null)
        {
            pool_bullet = new PoolingGameObject<BulletEnemy>(ED.BulletPrefabs);
            OnDeath += (Enemy) => pool_bullet.DestroyAll();
        }
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.ShowHP(this);
            OnTookDamage += EnemyManager.Instance.ShowHPSub;
        } else
        { 
            Debug.Log("Instance cho EnemyManager không tồn tại");
        }
        OnTookDamage += VFXDamageTook;
        OnCheckForAttack += checkAttack;
        OnDeath += onDeath;
        PlayerController.OnTargetFireChanged += OnSelected;
    }
    protected override void Start()
    {
        base.Start();

        ShowOutLine(false);
        render.sortingLayerName = "Current";
        Heath = MaxHP;
        isMoving = false;
        time_start_idle = Random.Range(Time.time - Random.Range(ED.time_range_idle.x, ED.time_range_idle.y), Time.time);
        time_start_attack = Time.time;
        time_idle = Random.Range(ED.time_range_idle.x, ED.time_range_idle.y);
        time_delay_attack = Random.Range(ED.time_range_attack.x, ED.time_range_attack.y);
    }
    protected override void Death()
    {
        Died = true;
        OnDeath?.Invoke(this);
        Destroy(this.gameObject);
    }

    protected void onDeath(Entity enitty)
    {
        PlayerController.OnTargetFireChanged -= OnSelected;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnTookDamage -= EnemyManager.Instance.ShowHPSub;
        OnTookDamage -= VFXDamageTook;
        OnCheckForAttack -= checkAttack;
        OnDeath -= onDeath;
    }

    #region Xử lý sát thương

    private void VFXDamageTook(DamageData damadata)
    {

    }
    public override void TakeDamage(DamageData dama)
    {
        CurrentDamageData = dama;
        base.TakeDamage(dama);
    }
    protected override void XLDNormal(DamageData damadata)
    {
        base.XLDNormal(damadata);
        damadata.Decrease(ED.Shield);
    }
    #endregion
    protected virtual void Update()
    {
        setSorting();
        UpdateTargetFire();
        CheckForAttack();
    }
    protected void UpdateTargetFire()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(center, 8f, layerTargetFire);
        if (collider2Ds == null || collider2Ds.Length == 0)
        {
            TargetFire = null;
        } else
        {
            int d = 0;
            float minDistance = Vector2.Distance(collider2Ds[0].bounds.center, center);
            for (int i = 1; i < collider2Ds.Length; i++)
            {
                float distance = Vector2.Distance(collider2Ds[i].bounds.center, center);
                if (distance < minDistance && collider2Ds[i].gameObject.GetComponent<PlayerController>() != null)
                {
                    d = i;
                }
            }
            TargetFire = collider2Ds[d].GetComponent<PlayerController>();
        }
    }
    protected void setSorting()
    {
        if(transform.hasChanged && render != null)
        {
            render.sortingOrder = (int)(transform.position.y * -10);
        }
    }
    protected virtual void FixedUpdate()
    {
        if (PermitMove)
        {
            Move();
        } else
        {
            rig.velocity = Vector2.zero;
        }
    }
    public override Vector3 getPosition()
    {
        return transform.position + new Vector3(0, 0.1f, 0);
    }

    #region Di chuyển và tấn công

    protected void CheckForAttack()
    {
        if (PermitAttack)
        {
            Attack();
            time_start_attack = Time.time;
            time_delay_attack = Random.Range(ED.time_range_attack.x, ED.time_range_attack.y);
        }
    }
    protected virtual void checkAttack(BoolAction permit)
    {
        permit.IsOK = Time.time - time_start_attack > time_delay_attack && targetAttack != null && HasTargetNear && ED.BulletPrefabs != null;
    }
    protected virtual void Attack()
    {
        Vector2 dirAttack = getRotationToPlayer(((Vector2)targetAttack.ColliderTakeDamaged.bounds.center - vitriradan).normalized);
        BulletEnemy bullet = pool_bullet.Spawn(vitriradan, MathQ.DirectionToQuaternion(dirAttack));
        DamageData dam = setUpDamageData(dirAttack);
        bullet.StartUp(dam);
    }
    protected DamageData setUpDamageData(Vector3 NewDirection)
    {
        int SatThuong = Damage;
        DamageData damageData = new DamageData();
        damageData.Damage = SatThuong;
        damageData.Direction = NewDirection;
        damageData.From = this;
        return damageData;
    }
    protected Vector2 getRotationToPlayer(Vector2 dir)
    {
        Vector3 Do = MathQ.DirectionToRotation(dir);
        Do += new Vector3(0, 0, Random.Range(-30, 30));
        return MathQ.RotationToDirection(Do.z).normalized;
    }
    private void Move()
    {
        if (limitMove == null || limitMove.Length == 1)
        {
            Debug.Log("Chưa có giới hạn di chuyển");
        }
        if (!isMoving)
        {
            if (Time.time - time_start_idle > time_idle)
            {
                if (limitMove == null || limitMove.Length == 1)
                {
                    dirMove = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                }
                else
                {
                    Vector3 pos;
                    int test = 0;
                    do
                    {
                        pos = new Vector3(Random.Range(limitMove[0].x, limitMove[1].x), Random.Range(limitMove[0].y, limitMove[1].y), 0);
                        test++;
                    } while (!CanSeePosition(transform.position, pos) && test < 10);
                    dirMove = (pos - transform.position).normalized;
                }
                isMoving = true;
                time_start_move = Time.time;
                time_move = Random.Range(ED.time_range_move.x, ED.time_range_move.y);
            } else
            {
                if (HasTargetNear)
                {
                    float dirFireX = targetAttack.transform.position.x - transform.position.x;
                    if (dirFireX < 0) transform.localScale = scaleDefault;
                    else if (dirFireX > 0) transform.localScale = new Vector3(-scaleDefault.x, scaleDefault.y, scaleDefault.z);
                }
                else
                {
                    if (dirMove.x < 0) transform.localScale = scaleDefault;
                    else if (dirMove.x > 0) transform.localScale = new Vector3(-scaleDefault.x, scaleDefault.y, scaleDefault.z);
                }
                rig.velocity = Vector2.zero;
                SetAni("Idle");
            }
        } else
        {
            if (Time.time - time_start_move > time_move)
            {    
                isMoving = false;
                time_start_idle = Time.time;
                time_idle = Random.Range(ED.time_range_idle.x, ED.time_range_idle.y);
            } else
            {
                if (PermitMove)
                {
                    rig.velocity = dirMove.normalized * speedMove;
                    SetAni("Run");
                } else
                {
                    SetAni("Idle");
                }
                if (HasTargetNear)
                {
                    float dirFireX = targetAttack.transform.position.x - transform.position.x;
                    if (dirFireX < 0) transform.localScale = scaleDefault;
                    else if (dirFireX > 0) transform.localScale = new Vector3(-scaleDefault.x, scaleDefault.y, scaleDefault.z);
                }
            }
        }
    }
    bool CanSeePosition(Vector3 from, Vector3 to)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(from, (to - from).normalized, Vector2.Distance(from, to), EnemyManager.Instance.WallAndBarrier);
        return (hit.collider == null);
    }
    void SetAni(string code)
    {
        anim.setAnimation(code);
    }
    #endregion
    public override void OnEquipment(Weapon weapon)
    {
        
    }
    private void OnSelected(Enemy enemy)
    {
        if (enemy != null)
        {
            ShowOutLine(enemy == this);
        } else
        {
            ShowOutLine(false);
        }
    }

    private void ShowOutLine(bool a)
    {
        if (render != null)
        {
            if (a)
            {
                render.material.SetInt("show_outline", 1);
            } else
            {
                render.material.SetInt("show_outline", 0);
            }
        }
    }
    public override void OnUnEquipment(Weapon weapon)
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = render.bounds.ClosestPoint(a);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, pos);
        Gizmos.DrawLine(pos, a);
    }
}
