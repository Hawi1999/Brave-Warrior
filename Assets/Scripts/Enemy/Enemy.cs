using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

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
public class Enemy : Entity
{

    #region EditorVisible
    public string EnemyName = "New Enemy";
    public int Damage;
    public EnemyData ED;
    public Transform PR_HP;
    public Transform PR_HPsub;
    public Collider2D colliderTakeDamage;
    [SerializeField] LayerMask layerTargetFire;
    [SerializeField] SpriteRenderer Selecting;
    [SerializeField] float speedMove = 5;

    private Vector2[] limitMove;

    #endregion

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
    public override Entity TargetFire { get ; set; }
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
            return PlayerController.PlayerCurrent;
        }
    }
    private bool HasPlayerNear
    {
        get
        {
            return targetAttack != null;
        }
    }
    private SpriteRenderer render => GetComponent<SpriteRenderer>();
    private AnimationQ anim => GetComponent<AnimationQ>();
    private Rigidbody2D rig => GetComponent<Rigidbody2D>();
    private RoundBase round => RoundBase.RoundCurrent;
    private BuiControl buicontrol => GetComponent<BuiControl>();
    private PlayerController player
    {
        get
        {
            return PlayerController.PlayerCurrent;
        }
    }
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

    public override Vector3 PositionSpawnWeapon => vitriradan;
    public Transform tranSform => transform;
    protected override bool PermitAttack
    {
        get
        {
            BoolAction check = new BoolAction(true);
            OnCheckForAttack?.Invoke(check);
            return check.IsOK 
                && Time.time - time_start_attack > time_delay_attack 
                && targetAttack != null 
                && Vector2.Distance(targetAttack.ColliderTakeDamaged.bounds.center, vitriradan) <= 6f; ;
        }
    }

    #endregion
    private void Awake()
    {
        gameObject.tag = "Enemy";
    }
    protected override void Start()
    {
        base.Start();
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

        Selecting?.gameObject.SetActive(false);
        Heath = MaxHP;
        isMoving = false;
        time_start_idle = Random.Range(Time.time - Random.Range(ED.time_range_idle.x, ED.time_range_idle.y), Time.time);
        time_start_attack = Time.time;
        time_idle = Random.Range(ED.time_range_idle.x, ED.time_range_idle.y);
        time_delay_attack = Random.Range(ED.time_range_attack.x, ED.time_range_attack.y);
        player.OnTargetFireChanged += OnSelected;
    }
    protected override void Death()
    {
        Died = true;
        if (buicontrol)
            buicontrol.SpawnBui(20, (int)MathQ.DirectionToRotation(CurrentDamageData.Direction).z, 45);
        OnDeath?.Invoke(this);
        Destroy(this.gameObject);
    }

    private void onDeath(Entity enitty)
    {
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
        if (buicontrol != null)
            buicontrol.SpawnBui(damadata.Damage / 2, (int)MathQ.DirectionToRotation(CurrentDamageData.Direction).z, 45);
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

    private void Update()
    {
        setSorting();
        CheckForAttack();
    }

    private void setSorting()
    {
        if(transform.hasChanged && render != null)
        {
            render.sortingOrder = (int)(transform.position.y * -10);
        }
        if(Selecting != null)
        {
            Selecting.sortingOrder = (int)(transform.position.y * -10) - 1;
        }
    }

    private void OnSelected(Enemy enemy)
    {
        if (Selecting != null)
        {
            Selecting.gameObject.SetActive(enemy == this);
        }
    }

    private void FixedUpdate()
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

    protected virtual void CheckForAttack()
    {
        if (PermitAttack)
        {
            Attack();
            time_start_attack = Time.time;
            time_delay_attack = Random.Range(ED.time_range_attack.x, ED.time_range_attack.y);
        }
    }

    private void checkAttack(BoolAction permit)
    {
        permit.IsOK = Time.time - time_start_attack > time_delay_attack && targetAttack != null && Vector2.Distance(targetAttack.ColliderTakeDamaged.bounds.center, vitriradan) <= 6f;
    }

    protected virtual void Attack()
    {
        Vector2 dirAttack = getDirToPlayer(((Vector2)targetAttack.ColliderTakeDamaged.bounds.center - vitriradan).normalized);
        BulletEnemy bullet = Instantiate(ED.BulletPrefabs, vitriradan, MathQ.DirectionToQuaternion(dirAttack));
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

    protected Vector2 getDirToPlayer(Vector2 dir)
    {
        Vector3 Do = MathQ.DirectionToRotation(dir);
        Do += new Vector3(0, 0, Random.Range(-30, 30));
        return MathQ.RotationToDirection(Do.z).normalized;
    }

    public override Vector2 getSize()
    {
        return transform.localScale * ED.Size;
    }
    public override Vector2 getCenter()
    {
        return transform.position + new Vector3(0, 0.1f) * transform.localScale.y;
    }

    public void setLimitMove(Vector2[] vector2s)
    {
        limitMove = vector2s;
    }

    private void fixTransform()
    {
        if (limitMove == null)
        {
            return;
        }
        Vector3 position = tranSform.position;
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
                if (HasPlayerNear)
                {
                    float dirFireX = targetAttack.transform.position.x - transform.position.x;
                    if (dirFireX < 0) render.flipX = false ;
                    else if (dirFireX > 0) render.flipX = true;
                }
                else
                {
                    if (dirMove.x < 0) render.flipX = false;
                    else if (dirMove.x > 0) render.flipX = true;
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
                if (HasPlayerNear)
                {
                    float dirFireX = targetAttack.transform.position.x - transform.position.x;
                    if (dirFireX < 0) render.flipX = false;
                    else if (dirFireX > 0) render.flipX = true;
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

    public override void OnEquipment(Weapon weapon)
    {
        
    }

    public override void OnUnEquipment(Weapon weapon)
    {
        
    }

    #endregion

}
