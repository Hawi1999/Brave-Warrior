using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Ene_Qua : Enemy
{
    [SerializeField] BoomDrop Boom;
    [SerializeField] float RadiusDropNear = 3;

    private float l => RadiusDropNear;
    private bool attacking = false;
    private bool readyattack = false;
    protected override void Awake()
    {
        base.Awake();
        render.sortingLayerName = "Fly";
    }

    protected override void Update()
    {
        base.Update();
        rig.velocity = Vector3.zero;
        UpdateRender();
    }

    void UpdateRender()
    {
        if (!attacking)
        {
            Vector3 dir = (targetAttack.getPosition() - (Vector3)center).normalized;
            if (dir.x > 0)
            {
                transform.localScale = scaleDefault * Vector3.one;
            } else
            {
                transform.localScale = scaleDefault * new Vector3(-1, 1, 1);
            }
        }
    }

    void ReadyToAttack()
    {
        Vector3 center = targetAttack.transform.position;
        Vector3 positionDrop = GetPositionToDrop(center + new Vector3(Random.Range(-l, l), Random.Range(-l, l), 0));
        Vector3 targetMove = Boom.GetPositionToDrop(positionDrop);
        float timefly = Vector3.Distance(transform.position, targetMove) / speedMove;
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", targetMove,
            "time", timefly,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnCompleteReadyAttack"));
    }
    protected override void Attack()
    {
        DamageData damage = new DamageData();
        damage.From = this;
        Instantiate(Boom, transform.position + Boom.GetLocalPositionDrop(), Quaternion.identity).StartDrop(damage);
        OnAttacked?.Invoke();
    }

    protected override void checkAttack(BoolAction permit)
    {
        permit.IsOK = Time.time - time_start_attack > time_delay_attack && HasTargetNear;
    }

    protected override void CheckForAttack()
    {
        if (!attacking)
        {
            if (PermitAttack)
            {
                ReadyToAttack();
                attacking = true;
            }
            OnNotAttack?.Invoke();
        } 
    }

    Vector3 GetPositionToDrop(Vector3 positionToDrop)
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


    protected override void Move()
    {
        if (limitMove == null || limitMove.Length == 1)
        {
            Debug.Log("Chưa có giới hạn di chuyển");
        }
        if (!attacking)
        {
            if (!isMoving)
            {
                if (Time.time - time_start_idle > time_idle)
                {
                    MoveToNewPosition();
                }
            }
        }
    }

    private void MoveToNewPosition()
    {
        Vector3 pos;
        if (limitMove == null || limitMove.Length == 1)
        {
            pos = transform.position +  new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        }
        else
        {
            pos = new Vector3(Random.Range(limitMove[0].x, limitMove[1].x), Random.Range(limitMove[0].y, limitMove[1].y), 0);
        }
        float time = Vector3.Distance(pos, transform.position)/speedMove;
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", pos,
            "time", time,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnMoveDone"));
        isMoving = true;
    }
    #region Call By iTween
    private void OnCompleteReadyAttack()
    {
        readyattack = true;
        iTween.ValueTo(gameObject, iTween.Hash("" +
            "from", 0,
            "to", 0.8f,
            "easetype", iTween.EaseType.linear,
            "onupdate", "CheckToAttack",
            "oncomplete", "OnCompleteAttack"));
    }
    private void CheckToAttack(float a)
    {
        if (a > 0.4f && readyattack)
        {
            Attack();
            readyattack = false;
        }
    }
    private void OnCompleteAttack()
    {
        attacking = false;
        isMoving = false;
        time_delay_attack = Random.Range(ED.time_range_attack.x, ED.time_range_attack.y);
        time_start_attack = Time.time;
    }
    private void OnMoveDone()
    {
        isMoving = false;
        time_start_idle = Time.time;
        time_idle = Random.Range(ED.time_range_idle.x, ED.time_range_idle.y);
    }

    #endregion
}
