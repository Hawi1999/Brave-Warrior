using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_Nhim : EnemyUnderGround
{
    [SerializeField] int Damage = 4; 
    [SerializeField] float radius = 0.3f;
    [SerializeField] float distanceTakeDamage = 0.5f;
    [SerializeField] float Speed = 2f;
    [SerializeField] LayerMask layerAttack;
    [SerializeField] ParticleSystem VFXAttack;
    [SerializeField] Vector2 time_range_attack = new Vector2(4, 6);

    float timereadyattack = 0.33f;
    float timeendattack = 0.33f;
    public override bool IsForFind => base.IsForFind && CurrentAction != Action.Hide; 

    private List<TimeToTakeHit> takehits = new List<TimeToTakeHit>();

    private Vector2 Direct_Attack;

    protected override void ChooseNextAction()
    {
        if (CurrentAction == Action.Idle)
        {
            int a = UnityEngine.Random.Range(0, 4);
            if (a > 1 && PermitMove && PermitAttack)
            {
                SetNewAction(Action.ReadyAttack);
                OnBeginReadyAttack();
            }
            else
            {
                SetNewAction(Action.Down);
                OnBeginDown();
            }
            return;
        }
    }

    #region ReadyAttack 
    protected override void UpdateReadyAttack()
    {
        if (WaitToChooseNextAction)
        {
            SetNewAction(Action.Attack);
            OnBeginAttack();
        } else
        {
            CheckTimeToNextAction();
        }

    }

    protected virtual void OnBeginReadyAttack()
    {
        SetTimeToNextAction(timereadyattack);
        SetAnimation(Animate_ReadyAttack);
    }
    #endregion

    #region Attacking
    protected override void UpdateAttack()
    {
        if (WaitToChooseNextAction)
        {
            OnEndAttack();
            SetNewAction(Action.EndAttack);
            OnBeginEndAttack();
        } else
        {
            Attacking();
            CheckTimeToNextAction();
        }
    }
    protected virtual bool ReadyToDamaged(ITakeHit take)
    {

        if (take == null)
        {
            return false;
        }
        TimeToTakeHit tt = Array.Find(takehits.ToArray(), e => e.takeHit == take);
        if (tt == null)
        {
            takehits.Add(new TimeToTakeHit(take, Time.time));
            return true;
        }
        if (Time.time - tt.time > distanceTakeDamage)
        {
            tt.time = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }
    protected virtual void Attacking()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(center, radius, layerAttack);
        if (cols == null || cols.Length == 0)
        {
            return;
        }
        else
        {
            foreach (Collider2D col in cols)
            {
                ITakeHit take = col.GetComponent<ITakeHit>();
                if (ReadyToDamaged(take))
                {
                    DamageData damage = new DamageData();
                    SetUpDamageData(damage);
                    damage.Direction = Direct_Attack;
                    take.TakeDamaged(damage);
                    takehits.Add(new TimeToTakeHit(take, Time.time));
                }
            }
        }
    }

    protected virtual void OnBeginAttack()
    {
        SetAnimation(Animate_Attack);
        VFXAttack.Play();
        takehits = new List<TimeToTakeHit>();
        Direct_Attack = DirectFire;
        float timeAttack = UnityEngine.Random.Range(time_range_attack.x, time_range_attack.y);
        SetTimeToNextAction(timeAttack);
        iTween.MoveAdd(gameObject, iTween.Hash(
            "amount", (Vector3)Direct_Attack * Speed * timeAttack,
            "time", timeAttack,
            "easeType", iTween.EaseType.easeInSine));
    }
    protected virtual void OnEndAttack()
    {
        VFXAttack.Stop();
        iTween.Stop(gameObject, "MoveAdd");
    }

    protected override void SetUpDamageData(DamageData damage)
    {
        base.SetUpDamageData(damage);
        damage.Damage = Damage;
    }

    #endregion

    #region EndAttack

    private void OnBeginEndAttack()
    {
        SetTimeToNextAction(timeendattack);
        SetAnimation(Animate_EndAttack);
    }

    protected override void UpdateEndAttack()
    {
        if (WaitToChooseNextAction)
        {
            SetNewAction(Action.Idle);
            OnBeginIdle();
        } else
        {
            CheckTimeToNextAction();
        }
    }

    #endregion

    #region Lặt Vặt
    protected override void VFXTookDamage(DamageData damadata)
    {
        if (CurrentAction == Action.Idle)
        {
            base.VFXTookDamage(damadata);
        }
    }

    private void StopByBackForce(DamageData damage)
    {
        if (CurrentAction == Action.Attack)
        {
            if (damage.BackForce > 4)
            {
                iTween.Stop(gameObject, "MoveAdd");
                NextAction = Action.WaitToChoose;
                base.VFXTookDamage(damage);
            }
        }
    }


    #endregion

    #region StartAndUpdate
    protected override void Start()
    {
        base.Start();
        OnTookDamage += StopByBackForce;
        VFXAttack?.Stop();
    }

    protected override void UpdateScaleRender()
    {
        if (CurrentAction == Action.Idle)
        {
            base.UpdateScaleRender();
        }
    }

    #endregion

    #region Inspestor

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, radius);
    }

    #endregion
}
