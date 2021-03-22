using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class EnemyUnderGround : Enemy
{
    [SerializeField] protected Vector2 time_range_hide;
    [SerializeField] Collider2D colliderMove;
    [SerializeField] Collider2D colliderTakedamage;

    float timeDown = 1f;
    float timeUp = 2f / 3;

    #region Down
    protected override void UpdateDown()
    {
        if (WaitToChooseNextAction)
        {
            SetNewAction(Action.Hide);
            OnBeginHide();
        } else
        {
            CheckTimeToNextAction();
        }
    }
    protected virtual void OnBeginDown()
    {
        if (!PermitDown)
        {
            CurrentAction = Action.Idle;
            OnBeginIdle();
            return;
        }
        SetTimeToNextAction(timeDown);
        SetAnimation(Animate_Down);
    }
    #endregion

    #region Up
    protected virtual void OnBeginUp()
    {
        SetTimeToNextAction(timeUp);
        SetAnimation(Animate_Up);
    }

    protected override void UpdateUp()
    {
        if (WaitToChooseNextAction)
        {
            SetNewAction(Action.Idle);
            OnBeginIdle();
        }
        else
        {
            CheckTimeToNextAction();
        }
    }
    #endregion

    #region Hide
    Vector2 positionEnd;
    protected virtual void OnBeginHide()
    {

        render.enabled = false;
        colliderMove.enabled = false;
        colliderTakedamage.enabled = false;
        OnIntoTheGound?.Invoke();
        float timeHide = Random.Range(time_range_hide.x, time_range_hide.y);
        SetTimeToNextAction(timeHide);
        positionEnd = TileManager.GetPositionInGoundCurrent(transform.position, false);
    }

    protected override void UpdateHide()
    {
        if (WaitToChooseNextAction)
        {
            OnEndHide();
            SetNewAction(Action.Up);
            OnBeginUp();
        }
        else
        {
            CheckTimeToNextAction();
        }
    }
    protected virtual void OnEndHide()
    {
        transform.position = positionEnd;
        render.enabled = true;
        colliderMove.enabled = true;
        colliderTakedamage.enabled = true;
        OnOuttoTheGound?.Invoke();
    }
    #endregion

}
