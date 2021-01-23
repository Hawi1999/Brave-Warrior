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
    public static PlayerController Instance
    {
        get; set;
    }
    public Collider2D ColliderTakeDamaged;
    [SerializeField] private float speed = 1;
    [SerializeField] private int Shield = 5;
    [SerializeField] private SpriteRenderer RenderSelected;
    [SerializeField] private LayerMask layerTargetFind;
    [SerializeField] private Vector3 OffSetWeapon;

    SpriteRenderer Render;
    AnimationQ anim;
    Rigidbody2D rig;
    public bool PermitMove;
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
        anim = GetComponent<AnimationQ>();
        rig = GetComponent<Rigidbody2D>();
        Render = GetComponent<SpriteRenderer>();
        PermitMove = true;
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
        WeaponCurrent.Attack();
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

    public override void TakeDamage(DamageData damadata)
    {
        int dam = damadata.Damage;
        DamageElement ele = DamageElement.Normal;
        if (damadata.Type == DamageElement.Normal)
        {
            int d = damadata.Damage - Shield;
            dam = (int)Mathf.Clamp(d, 0, Mathf.Infinity);
        }
        Damaged(dam);
        Debug.Log("Máu còn lại: " + Heath);
        OnDamageTook?.Invoke(this, dam, ele);
    }

    private void Damaged(int dam)
    {
        Heath -= dam;
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
    public UnityAction<Entity, int, DamageElement> OnDamageTook;
}
