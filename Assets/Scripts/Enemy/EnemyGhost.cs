using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyGhost : EnemySky
{
    [SerializeField] protected Vector2 time_range_hide = new Vector2(2f, 4f);
    [SerializeField] protected Vector2Int distance_hide = new Vector2Int(4, 4);
    [SerializeField] protected Collider2D colliderTakeDamage;

    protected float timeStartHide = 1;
    protected float timeEndHide = 1;

    protected bool hiding = false;

    public override bool IsForFind => base.IsForFind && !(CurrentAction == Action.Hide);

    #region Start And Update

    #endregion

    #region StartHide 

    protected override void UpdateStartHide()
    {
        if (WaitToChooseNextAction)
        {
            EndStartHide();
            SetNewAction(Action.Hide);
            OnBeginHide();
        } else
        {
            StartHiding();
            CheckTimeToNextAction();
        }
    }

    protected virtual void OnBeginStartHide()
    {
        SetTimeToNextAction(timeStartHide);
    }

    protected virtual void StartHiding()
    {
        float a = (Time.time - time_start_action) / time_action;
        UpdateAlphaRender(render, 1 - a);
        if (!hiding && a >= 0.5f)
        {
            colliderTakeDamage.enabled = false;
            OnHide?.Invoke();
            hiding = true;
        }
    }

    protected virtual void EndStartHide()
    {
        UpdateAlphaRender(render, 0);
    }
    #endregion

    #region Hide
    Vector3 newposition;
    protected virtual void OnBeginHide()
    {
        render.enabled = false;
        newposition = TileManager.GetPositionInGoundCurrent(distance_hide, transform.position, false);
        SetTimeToNextAction(time_range_hide);
    }

    protected override void UpdateHide()
    {
        if (WaitToChooseNextAction)
        {
            OnEndHide();
            SetNewAction(Action.EndHide);
            OnBeginEndHide();
        } else
        {
            CheckTimeToNextAction();
        }
    }

    protected virtual void OnEndHide()
    {
        render.enabled = true;
        transform.position = newposition;
    }
    #endregion

    #region EndHide

    protected virtual void OnBeginEndHide()
    {
        SetTimeToNextAction(timeEndHide);
    }

    protected override void UpdateEndHide()
    {
        if (WaitToChooseNextAction)
        {
            OnEndEndHide();
            SetNewAction(Action.Idle);
            OnBeginIdle();
        } else
        {
            EndHiding();
            CheckTimeToNextAction();
        }
    }

    protected virtual void OnEndEndHide()
    {
        UpdateAlphaRender(render, 1);
    }

    protected virtual void EndHiding()
    {
        float a = (Time.time - time_start_action) / time_action;
        UpdateAlphaRender(render, a);
        if (hiding && a >= 0.5f)
        {
            colliderTakeDamage.enabled = true;
            OnAppear?.Invoke();
            hiding = false;
        }
    }

    #endregion

    private void UpdateAlphaRender(SpriteRenderer sprite, float a)
    {
        Color c = sprite.color;
        c.a = a;
        sprite.color = c;
    }
}
