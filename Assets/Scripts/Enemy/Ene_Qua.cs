using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Ene_Qua : EnemySky
{
    [SerializeField] BoomDrop Boom;
    [SerializeField] float RadiusDropNear = 3;
    [SerializeField] float RatioDodge = 0.5f;
    private float timeAttack = 1f;
    private float l => RadiusDropNear;
    protected override void Awake()
    {
        base.Awake();
        render.sortingLayerName = "Fly";
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (CurrentAction.Equals(Action.ReadyAttack))
        {
            if (PermitMove)
            {
                ReadyAttacking();
            }
        }
    }

    protected override void UpdateScaleRender()
    {
        if (CurrentAction != Action.Idle)
            return;
        if (HasTargetNear)
        {
            Vector2 dir = (TargetFire.center - center).normalized;
            if (dir.x > 0)
            {
                transform.localScale = ScaleCurrent.Value * Vector3.one;
            }
            else
            {
                transform.localScale = ScaleCurrent.Value * new Vector3(-1, 1, 1);
            }
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

    private void ReadyAttacking()
    {
        SetPosition((Vector2)transform.position + dirMove * SpeedMove * Time.fixedDeltaTime);
    }

    private void OnBeginReadyAttack()
    {
        Vector3 center = TargetFire.center;
        Vector3 positionDrop = FixPositionToDrop(center + new Vector3(Random.Range(-l, l), Random.Range(-l, l), 0));
        Vector3 targetMove = Boom.GetPositionToDrop(positionDrop);
        time_action = Vector3.Distance(transform.position, targetMove) / SpeedMove;
        dirMove = ((Vector2)targetMove - (Vector2)transform.position).normalized;
        SetTimeToNextAction(time_action);
    }

    #endregion

    #region Attack

    bool attacked;
    protected override void UpdateAttack()
    {
        if (WaitToChooseNextAction)
        {
            SetNewAction(Action.Move);
            OnBeginMove();
        } else
        {
            Attacking();
            CheckTimeToNextAction();
        }
    }

    private void Attacking()
    {
        if (Time.time - time_start_action >= timeAttack /2 && !attacked)
        {
            DamageData damage = new DamageData();
            SetUpDamageData(damage);
            Instantiate(Boom, transform.position + Boom.GetLocalPositionDrop(), Quaternion.identity).StartDrop(damage);
            attacked = true;
        }
    }

    private void OnBeginAttack()
    {
        attacked = false;
        SetTimeToNextAction(timeAttack);
    }

    #endregion
    Vector3 FixPositionToDrop(Vector3 positionToDrop)
    {
        Vector3 position = positionToDrop;
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
        return position;
    }

    int attacklt = 0;
    protected override void ChooseNextAction()
    {
        if (CurrentAction == Action.Idle)
        {
            if (attacklt < 2 && PermitMove && PermitAttack)
            {
                OnEndIdle();
                SetNewAction(Action.ReadyAttack);
                attacklt++;
                OnBeginReadyAttack();
            } else
            {
                OnEndIdle();
                SetNewAction(Action.Move);
                attacklt = 0;
                OnBeginMove();
            }
        }
    }

    public override void TakeDamage(DamageData dama)
    {
        float a = Random.Range(0, 1f);
        if (a < RatioDodge && dama.CanDodge)
        {
            dama.Dodged = true;
            dama.Damage = 0;
        }
        base.TakeDamage(dama);
    }
}
