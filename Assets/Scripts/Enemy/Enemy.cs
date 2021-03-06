﻿using UnityEngine;
using UnityEngine.Events;

public enum DamageElement
{
    Normal = 0,
    Fire = 1,
    Poison = 2,
    Ice = 3,
    Electric = 4
}


[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : Entity
{
    #region EditorVisible
    [SerializeField] string EnemyCodeName = "New Enemy";
    [SerializeField] public Transform PR_HP;
    [SerializeField] public Transform PR_HPsub;
    [SerializeField] Transform OffsetCenter;
    [SerializeField] protected Vector2 time_range_idle = new Vector2(2f, 4f);
    #endregion

    #region EditorInvisible
    public static TakeBuff EnemyTakeBuffer = new TakeBuff();
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
    public override IFindTarget TargetFire
    {
        get
        {
            return PlayerController.PlayerCurrent;
        }
        set 
        { 

        }
    }
    public override Vector3 DirectFire { 
        get {
            if (TargetFire == null || TargetFire as UnityEngine.Object == null)
                return Vector2.zero;
            return (TargetFire.center - center).normalized; 
        } 
        set { } }
    protected virtual bool HasTargetNear
    {
        get
        {
            if (TargetFire == null || TargetFire as UnityEngine.Object == null || !TargetFire.IsForFind)
            {
                return false;
            }
            return Vector2.Distance(TargetFire.center, center) <= DistanceFindTarget;
        }
    }
    private Animator anim;
    private Rigidbody2D rig => GetComponent<Rigidbody2D>();
    protected Selecting selecting;
    protected Vector2 dirMove = new Vector2(0,0);
    public override Vector2 center => (OffsetCenter == null) ? transform.position : OffsetCenter.position;
    protected PoolingGameObject pool => PoolingGameObject.PoolingMain;
    public override TakeBuff take => EnemyTakeBuffer;
    protected float time_start_action;
    protected float time_action;
    #endregion
    protected override void SetUpAwake()
    {
        base.SetUpAwake();
        gameObject.tag = "Enemy"; 
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        anim = GetComponent<Animator>();
        selecting = Instantiate(VFXManager.SelectingEnemyPrefab, transform);
        selecting.StartUp(this);
        selecting.gameObject.SetActive(false);
        render.sortingLayerName = "Current";
    }
    protected virtual void ShowHP()
    {
        EntityManager.Instance.ShowHP(this);
    }
    protected override void SetUpStart()
    {
        base.SetUpStart();
        ShowHP();
        SetNewAction(Action.Idle);
        OnBeginIdle();
    }
    protected override void SetUpStartEvents()
    {
        base.SetUpStartEvents();
        OnTookDamage += EntityManager.Instance.ShowHPSub;
    }
    protected override void SetUpEndEvent()
    {
        base.SetUpEndEvent();
        OnTookDamage -= EntityManager.Instance.ShowHPSub;
    }

    #region Xử lý sát thương

    public override void TakeDamage(DamageData dama)
    {
        CurrentDamageData = dama;
        dama.OnHitEnemy?.Invoke(this);
        base.TakeDamage(dama);
    }
    protected override void CheckHP(DamageData damage)
    {
        if (CurrentHeath <= 0)
        {
            damage.OnHitToDieEnemy?.Invoke(this);
            Death();
        }
    }
    protected override void XLDNormal(DamageData damadata)
    {
        base.XLDNormal(damadata);
    }

    protected override void OnDead()
    {
        gameObject.SetActive(false);
    }

    protected override void SetUpDamageData(DamageData damage)
    {
        base.SetUpDamageData(damage);
        damage.AddDamageOrigin(take.GetValue(BuffRegister.TypeBuff.IncreaseDamageByValue));
        damage.AddDamagePercentOrigin(take.GetValue(BuffRegister.TypeBuff.IncreaseDamageByPercent));
    }
    #endregion

    #region Cật Nhật
    protected override void Update()
    {
        base.Update();
        CheckAttackDefault();
        UpdateStatus();
        setSorting();
        UpdateScaleRender();
        fixTransform();
    }

    protected virtual void FixedUpdate()
    {
        rig.velocity = Vector2.zero;
    }
    private void UpdateStatus()
    {
        switch (CurrentAction)
        {
            case Action.Idle:
                UpdateIdle();
                break;
            case Action.Attack:
                UpdateAttack();
                break;
            case Action.Down:
                UpdateDown();
                break;
            case Action.EndAttack:
                UpdateEndAttack();
                break;
            case Action.StartHide:
                UpdateStartHide();
                break;
            case Action.EndHide:
                UpdateEndHide();
                break;
            case Action.Hide:
                UpdateHide();
                break;
            case Action.Up:
                UpdateUp();
                break;
            case Action.ReadyAttack:
                UpdateReadyAttack();
                break;
            case Action.Move:
                UpdateMove();
                break;
        }
    }

    #endregion 

    #region More Update Action
    protected virtual void UpdateMove()
    {
    }
    protected virtual void UpdateAttack()
    {
    }
    protected virtual void UpdateReadyAttack()
    {
    }
    protected virtual void UpdateEndAttack()
    {
    }
    protected virtual void UpdateDown()
    {
    }
    protected virtual void UpdateHide()
    {
    }
    protected virtual void UpdateUp()
    {
    }
    protected virtual void UpdateStartHide()
    {
    }
    protected virtual void UpdateEndHide()
    {
    }

    #endregion

    #region Chung


    protected bool WaitToChooseNextAction => NextAction == Action.WaitToChoose;

    protected virtual void UpdateScaleRender()
    {
        if (HasTargetNear && CurrentAction == Action.Idle)
        {
            Vector3 dir = (TargetFire.center - center).normalized;
            if (dir.x > 0)
            {
                transform.localScale = ScaleCurrent.Value * new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = ScaleCurrent.Value * Vector3.one;
            }
        }
    }
    protected void setSorting()
    {
        if(transform.hasChanged && render != null)
        {
            render.sortingOrder = (int)(transform.position.y * -10);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        if (collision.gameObject == PlayerController.PlayerCurrent.gameObject)
        {
            rig.velocity = Vector2.zero;
        }
    }

    #endregion

    #region Idle 
    protected virtual void UpdateIdle()
    {
        if (WaitToChooseNextAction)
        {
            OnEndIdle();
            ChooseNextAction();
        } else
        {
            Idling();
            CheckTimeToNextAction();
        }
    }

    protected virtual void OnBeginIdle()
    {
        SetTimeToNextAction(time_range_idle);
        SetAnimation(Animate_Idle);
    }

    protected virtual void Idling()
    {

    }

    protected virtual void OnEndIdle()
    {

    }

    #endregion

    #region Chung
    protected bool EnoughTimeToNextAction => Time.time - time_start_action >= time_action;

    protected Vector2 getRotationToPlayer(Vector2 dir, int offset)
    {
        if (offset < 0)
            offset = -offset;
        offset = offset % 90;
        Vector3 Do = MathQ.DirectionToRotation(dir);
        Do += new Vector3(0, 0, Random.Range(-offset, offset));
        return MathQ.RotationToDirection(Do.z).normalized;
    }

    protected void CheckTimeToNextAction()
    {
        if (Time.time - time_start_action >= time_action)
        {
            NextAction = Action.WaitToChoose;
        }
    }

    protected void SetTimeToNextAction(Vector2 range)
    {
        time_start_action = Time.time;
        time_action = Random.Range(range.x, range.y);
    }

    protected void SetTimeToNextAction(float value)
    {
        time_start_action = Time.time;
        time_action = value;
    }

    Action oldAction = Action.Idle;
    int amount = 0;
    protected void SetNewAction(Action newAction)
    {
        CurrentAction = newAction;
        NextAction = Action.Busy;
    }

    protected void SetAnimation(int a)
    {
        anim.SetInteger("Value", a);
    }

    protected void SetPosition(Vector2 position)
    {
        rig.MovePosition(position);
    }

    protected override void VFXTookDamage(DamageData damadata)
    {
        iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0), 0.1f);
    }

    protected abstract void ChooseNextAction();
    #endregion

    #region Trang Bi
    public override void OnEquipment(Weapon weapon)
    {
        
    }
    public override void OnUnEquipment(Weapon weapon)
    {
        
    }

    #endregion

    #region Show in Editor
    protected virtual void OnValidate()
    {

    }

    protected virtual void CheckAttackDefault()
    {
        if (!HasTargetNear)
        {
            LockAttack.Register("DontHasTargetNear");
        } else
        {
            LockAttack.CancelRegistration("DontHasTargetNear");
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (render == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, Mathf.Max(size.x, size.y) * 1.2f);
    }
    #endregion

    #region Info

    public string GetName()
    {
        string s = Languages.getString(EnemyCodeName);
        if (s.Equals(string.Empty))
        {
            return "Enemy Clone";
        } else
        {
            return s;
        }
    }
    public override Vector3 GetPosition()
    {
        return center;
    }

    public override void OnTargetFound(Entity host, IFindTarget target)
    {
        if (selecting == null)
            return;
        if (host == PlayerController.PlayerCurrent)
        {
            if (target != null && target as UnityEngine.Object != null)
            {
                selecting.gameObject.SetActive(target as UnityEngine.Object == this);
            } else
            {
                selecting.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region Action

    public UnityAction<Enemy> OnSpawnEnemyMore;

    #endregion

}
