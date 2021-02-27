using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_Nhim : Enemy
{

    private Animator animator => GetComponent<Animator>();


    [SerializeField] Vector2 timeHide = new Vector2(0, 5);
    [SerializeField] Vector2 timeIdle = new Vector2(0, 5);
    [SerializeField] Vector2 timeAttack = new Vector2(3, 5);
    [SerializeField] float radius = 0.3f;
    [SerializeField] float distanceTakeDamage = 0.5f;
    [SerializeField] float Speed = 2f;
    [SerializeField] LayerMask layerAttack;
    [SerializeField] Collider2D colliderMove;
    [SerializeField] ParticleSystem VFXAttack;

    private float lastTimeAction;
    private float timeDelayAction;
    private Status STTCurrent = Status.Idle;
    public override bool IsForFind => isActiveAndEnabled && STTCurrent != Status.Hiding; 

    private int consecutiveAttack;
    private List<TimeToTakeHit> takehits = new List<TimeToTakeHit>();
    public enum Status
    {
        Idle,
        Down,
        Hiding,
        Up,
        ReadyAttack,
        Attacking,
        EndAttack,
    }
    public class TimeToTakeHit 
    {
        public ITakeHit takeHit;
        public float time;
        public TimeToTakeHit(ITakeHit takeHit, float time)
        {
            this.takeHit = takeHit;
            this.time = time;
        }
    }
    


    #region Tấn Công
    private void UpdateDamaging()
    {
        if (STTCurrent == Status.Attacking)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(center, radius, layerAttack);
            if (cols == null || cols.Length == 0)
            {
                return;
            } else
            {
                foreach (Collider2D col in cols)
                {
                    ITakeHit take = col.GetComponent<ITakeHit>();
                    if (take != null && !isExist(take))
                    {
                        DamageData damage = setUpDamageData(DirectFire);
                        take.TakeDamaged(damage);
                        takehits.Add(new TimeToTakeHit(take, Time.time));
                    }
                }
            }
        }
    }
    
    bool isExist(ITakeHit take)
    {
        if (takehits == null || takehits.Count == 0)
        {
            return false;
        }
        return Array.Find(takehits.ToArray(), e => e.takeHit == take) != null;
    }
    private void UpdateListTakeHit()
    {
        if (takehits == null || takehits.Count == 0)
            return;
        for (int i = 0; i <  takehits.Count; i++)
        {
            if (Time.time - takehits[i].time > distanceTakeDamage)
            {
                takehits.Remove(takehits[i]);
            }
        }
    }
    protected override void Attack()
    {
        consecutiveAttack++;
        animator.SetTrigger("Attack");
        ResetStatus(timeAttack, Status.ReadyAttack);
        VFXAttack?.Play();
        takehits = new List<TimeToTakeHit>();
        DirectFire = (PlayerController.PlayerCurrent.getPosition() - transform.position).normalized;
        iTween.MoveAdd(gameObject, iTween.Hash(
            "amount", DirectFire * Speed * timeDelayAction,
            "time", timeDelayAction, 
            "easeType", iTween.EaseType.easeInSine,
            "oncomplete", "onCompleteAttack",
            "onupdate", "UpdateAttack"));
    }
    // Call by iTween
    private void UpdateAttack()
    {
        UpdateDamaging();
    }

    protected override void BackForce(DamageData damadata)
    {
        if (STTCurrent == Status.Idle)
        {
            base.BackForce(damadata);
        }
    }

    private void StopByBackForce(DamageData damage)
    {
        if (STTCurrent == Status.Attacking)
        {
            if (damage.BackForce > 2)
            {
                iTween.Stop(gameObject, "MoveAdd");
                onCompleteAttack();
                base.BackForce(damage);
            }
        }
    }

    protected override void checkAttack(BoolAction permit)
    {
        // Không cần chỉnh sửa thêm gì cả ok ?
    }

    #endregion 

    protected override void Start()
    {
        base.Start();
        timeDelayAction = 0;
        OnTookDamage += StopByBackForce;
        VFXAttack?.Stop();
    }

    protected override void Update()
    {
        setSorting();
        UpdateStatus();
        UpdateListTakeHit();
    }
    // Bỏ trống để enemy ko di chuyển
    protected override void FixedUpdate()
    {
        rig.velocity = Vector3.zero;
        fixTransform();
    }
    

    private void UpdateStatus()
    {
        switch (STTCurrent)
        {
            case Status.Idle:
                if (Time.time - lastTimeAction >= timeDelayAction)
                {
                    if (targetAttack == null || consecutiveAttack >= 2)
                    {
                        consecutiveAttack = 0;
                        animator.SetTrigger("Down");
                        ResetStatus(new Vector2(), Status.Down);
                    } else
                    {
                        CheckForAttack();
                    }
                } else
                {
                    UpdateTargetFire();
                    UpdateDirection();
                }
                break;
            case Status.Hiding:
                if (Time.time - lastTimeAction >= timeDelayAction)
                {
                    Vector3 position;
                    if (RoundBase.RoundCurrent != null)
                    {
                        position = RoundBase.RoundCurrent.getRandomPositonInRound();
                    } else
                    {
                        position = transform.position;
                    }
                    transform.position = position;
                    Hide(false);
                    animator.SetTrigger("Up");
                    ResetStatus(new Vector2(), Status.Up);
                }
                break;
        }
    }
    void UpdateDirection()
    {
        if (HasTargetNear)
        {
            Vector2 DirectFire = ((Vector2)targetAttack.getPosition() - center).normalized;
            if (DirectFire.x >= 0)
            {
                gameObject.transform.localScale = new Vector3(-scaleDefault.x, scaleDefault.y, scaleDefault.z);
            } else
            {
                gameObject.transform.localScale = scaleDefault; 
            }
        }
    }


    #region onAction
    // Call by Animator
    private void onCompleteReadyAttack()
    {
        ResetStatus(timeAttack, Status.Attacking);
    }

    // Call By Animator
    private void onCompleteEndAttack()
    {
        ResetStatus(timeIdle, Status.Idle);
    }

    // Call By iTween
    private void onCompleteAttack()
    {
        animator.SetTrigger("EndAttack");
        VFXAttack?.Stop();
        ResetStatus(new Vector2(), Status.EndAttack);
    }

    // Call by Animator
    private void onDownComplete()
    {
        ResetStatus(timeHide, Status.Hiding);
        Hide(true);
    }

    private void Hide(bool a)
    {
        render.enabled = !a;
        colliderTakeDamage.enabled = !a;
        colliderMove.enabled = !a;
        OnHide?.Invoke(a);
    }

    // Call by Animator
    private void onUpComplete()
    {
        ResetStatus(timeIdle, Status.Idle);
    }

    #endregion

    private void ResetStatus(Vector2 a, Status stt)
    {
        STTCurrent = stt;
        timeDelayAction = UnityEngine.Random.Range(a.x, a.y);
        lastTimeAction = Time.time;
    }

    public override Vector2 center => transform.position + new Vector3(0, 0.12f * transform.localScale.y, 0);




}
