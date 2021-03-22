using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyGround : Enemy
{

    #region StartAndUpdate
    [SerializeField] Vector2 time_range_move = Vector2.zero;
    [SerializeField] float SpeedMove = 5;

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
        SetAnimation(Animate_Move);
        Vector2 pos = TileManager.GetPositionInGoundCurrent(transform.position, true);
        dirMove = (pos - (Vector2)transform.position).normalized;
        isMoving = true;
    }

    protected virtual void Moving()
    {
        SetPosition(transform.position + (Vector3) dirMove * SpeedMove* Time.fixedDeltaTime);
    }

    protected virtual void OnEndMove()
    {
        dirMove = Vector2.zero;
        isMoving = false;
    }

    bool CanMoveTo(Vector3 from, Vector3 to)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(from, (to - from).normalized, Vector2.Distance(from, to), EntityManager.Instance.WallAndBarrier);
        return (hit.collider == null);
    }

    #endregion

    #region Chung
    #endregion
}
