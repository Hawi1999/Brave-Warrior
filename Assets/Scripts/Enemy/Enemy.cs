using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public enum DamageElement
{
    Normal,
    Fire,
    Poison,
    Ice,
}


[System.Serializable]
public class EnemyData
{
    public int MaxHP = 50;
    public float SizeHP = 1;
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

    private int lastDamage;
    public int LastDamage
    {
        get
        {
            if (Time.time - lastTimeTakeDamage < 0.5f)
                return lastDamage;
            else
                return 0;
        }
        set
        {
            lastDamage = value;
            lastTimeTakeDamage = Time.time;
        }
    }
    private float lastTimeTakeDamage = 0;
    #endregion

    #region EditorInvisible
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
            return PlayerController.Instance;
        }
    }
    private bool HasPlayerNear
    {
        get
        {
            return targetAttack != null;
        }
    }
    private SpriteRenderer render;
    private AnimationQ anim;
    private Rigidbody2D rig;
    private RoundBase round;
    private BuiControl buicontrol;
    private PlayerController player
    {
        get
        {
            return PlayerController.Instance;
        }
    }
    private bool PermitMove = true;
    private Vector3 lastDirTook = new Vector3(0,1,0);
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

    public UnityAction<Weapon, Weapon> OnWeaponChanged { get ; set ; }

    #endregion
    private void Awake()
    {
        gameObject.tag = "Enemy";
    }
    private void Start()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.ShowHP(this);
            OnTookDamage += EnemyManager.Instance.ShowHPSub;
        } else
        { 
            Debug.Log("Instance cho EnemtManager không tồn tại");
        }
        OnTakeDamage += OnTakedamage;

        OnTookDamage += VFXDamageTook;

        Heath = MaxHP;
        render = GetComponent<SpriteRenderer>();
        anim = GetComponent<AnimationQ>();
        rig = GetComponent<Rigidbody2D>();
        buicontrol = GetComponent<BuiControl>();
        round = RoundBase.RoundCurrent;
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
            buicontrol.SpawnBui(20, (int)MathQ.DirectionToRotation(lastDirTook).z, 45);
        OnDeath?.Invoke(this);
        Destroy(this.gameObject);
    }

    #region Xử lý sát thương

    private void VFXDamageTook(DamageData damadata)
    {
        lastDirTook = damadata.Direction;
        if (buicontrol != null)
            buicontrol.SpawnBui(damadata.getDamage() / 2, (int)MathQ.DirectionToRotation(lastDirTook).z, 45);
    }
    public override void TakeDamage(DamageData dama)
    {
        base.TakeDamage(dama);
        LastDamage = dama.getDamage() + LastDamage;
        lastTimeTakeDamage = Time.time;

    }

    private void OnTakedamage(DamageData damadata)
    {
        damadata.To = this;
        damadata.Decrease(Random.Range(-1, 2));
        if (damadata.Type == DamageElement.Normal)
        {
            damadata.Decrease(ED.Shield);
        }
    }
    #endregion

    private void Update()
    {
        setSorting();
        CheckAttack();
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
        Move();
    }

    #region Di chuyển và tấn công

    protected virtual void CheckAttack()
    {
        if (Time.time - time_start_attack > time_delay_attack && targetAttack != null && Vector2.Distance(targetAttack.ColliderTakeDamaged.bounds.center, vitriradan) <= 6f)
        {
            Attack();
            time_start_attack = Time.time;
            time_delay_attack = Random.Range(ED.time_range_attack.x, ED.time_range_attack.y);
        }
    }
    protected virtual void Attack()
    {
        Vector2 dirAttack = getDirToPlayer(((Vector2)targetAttack.ColliderTakeDamaged.bounds.center - vitriradan).normalized);
        BulletEnemy bullet = Instantiate(ED.BulletPrefabs, vitriradan, MathQ.DirectionToQuaternion(dirAttack));
        DamageData dam = new DamageData(Damage, dirAttack, default, this, new RaycastHit2D());
        bullet.StartUp(dam);
    }

    protected Vector2 getDirToPlayer(Vector2 dir)
    {
        Vector3 Do = MathQ.DirectionToRotation(dir);
        Do += new Vector3(0, 0, Random.Range(-30, 30));
        return MathQ.RotationToDirection(Do.z).normalized;
    }

    private void Move()
    {
        if (round == null)
        {
            return;
        }
        if (!isMoving)
        {
            if (Time.time - time_start_idle > time_idle)
            {
                RoundBase roundCurrent = RoundBase.RoundCurrent;
                if (roundCurrent == null)
                {
                    dirMove = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    Debug.Log("Round is null");
                }
                else
                {
                    BoxCollider2D col = roundCurrent.col;
                    if (col == null)
                    {
                        dirMove = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    } else
                    {
                        Vector3 center = col.bounds.center;
                        float sizeX = col.bounds.size.x / 2;
                        float sizeY = col.bounds.size.y / 2;
                        Vector3 pos;
                        int test = 0;
                        do
                        {
                            pos = center + new Vector3(Random.Range(-sizeX, sizeX), Random.Range(-sizeY, sizeY), 0);
                            test++;
                        } while (!CanSeePosition(transform.position, pos) && test < 10);
                        dirMove = (pos - transform.position).normalized;
                    }
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
    #endregion

}
