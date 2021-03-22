using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySky : Enemy
{

    #region StartAndUpdate
    [SerializeField] Vector2 time_range_move = Vector2.zero;
    [SerializeField] protected float SpeedMove = 5;

    bool isMoving = false;
    protected override void FixedUpdate()
    {
        if (isMoving)
        {
            Moving();
        }
    }
    #endregion
    #region Move
    protected override void UpdateMove()
    {
        if (WaitToChooseNextAction || !PermitMove)
        {
            OnEndMove();
            SetNewAction(Action.Idle);
            OnBeginIdle();
        }
        else
        {
            CheckTimeToNextAction();
        }
    }
    protected virtual void OnBeginMove()
    {
        if (!PermitMove)
        {
            SetNewAction(Action.Idle);
            OnBeginIdle();
            return;
        }
        SetTimeToNextAction(time_range_move);
        Vector2 pos = TileManager.GetPositionInGoundCurrent(transform.position, false);
        dirMove = (pos - (Vector2)transform.position).normalized;
        isMoving = true;
    }

    protected virtual void Moving()
    {
        SetPosition(transform.position + (Vector3)dirMove * SpeedMove * Time.fixedDeltaTime);
    }

    protected virtual void OnEndMove()
    {
        dirMove = Vector2.zero;
        isMoving = false;
    }
    #endregion
}
