using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ene_Ohno : EnemyGround
{
    public Transform _PositionSpawnAT1;

    public SpawnBulletCircle at1Prefab;
    public Boom at2Prefab;
    public Vector2Int RangeAmountBoom = new Vector2Int(4, 6);
    float timeReadyAttack = 0.5f;
    float timeAttack1 = 1;
    float timeAttack2;
    Vector3 vitriranda1
    {
        get
        {
            if (_PositionSpawnAT1 == null)
            {
                return transform.position;
            } else
            {
                return _PositionSpawnAT1.position;
            }
        }
    }

    protected override void ChooseNextAction()
    {
        if (CurrentAction == Action.Idle)
        {
            int a = Random.Range(0, 3);
            if (a == 1)
            {
                SetNewAction(Action.ReadyAttack);
                OnBeginReadyAttack1();
            } else if (a == 0)
            {
                SetNewAction(Action.ReadyAttack);
                OnBeginReadyAttack2();
            } else
            {
                SetNewAction(Action.Move);
                OnBeginMove();
            }
        }
    }

    #region  Attack 1

    private void OnBeginReadyAttack1()
    {
        SetAnimation(Animate_ReadyAttack);
        Invoke("OnBeginReadyAttack1Complete", timeReadyAttack);
    }
    // Call by Invoke
    private void OnBeginReadyAttack1Complete()
    {
        SetNewAction(Action.Attack);
        OnBeginAttack1();
    }

    private void OnBeginAttack1()
    {
        SetAnimation(Animate_Attack);
        DamageData damage = new DamageData();
        damage.From = this;
        Instantiate(at1Prefab, vitriranda1, Quaternion.identity).SetUp(damage);
        Invoke("OnAttack1Complete", timeAttack1);
    }

    // Call by invoke
    private void OnAttack1Complete()
    {
        SetNewAction(Action.Idle);
        OnBeginIdle();
    }
    #endregion

    #region

    private void OnBeginReadyAttack2()
    {
        SetAnimation(Animate_ReadyAttack);
        Invoke("OnBeginReadyAttack2Complete", timeReadyAttack);
    }
    // Call by Invoke
    private void OnBeginReadyAttack2Complete()
    {
        SetNewAction(Action.Attack);
        OnBeginAttack2();
    }

    private void OnBeginAttack2()
    {
        SetAnimation(Animate_Attack);
        int Amount = Random.Range(RangeAmountBoom.x, RangeAmountBoom.y);
        timeAttack2 = 1 + 0.4f * Amount;
        for (int i = 0; i<= Amount; i++)
        {
            Invoke("Attacking2", 1 + 0.4f * (i - 1));
        }
        Invoke("OnAttack2Complete", timeAttack2);
    }

    private void Attacking2()
    {
        Vector3 pos = TileManager.GetPositionInGoundCurrent();
        DamageData damage = new DamageData();
        damage.From = this;
        Boom b = Instantiate(at2Prefab, vitriranda1, Quaternion.identity);
        b.SetUp(damage);
        iTween.MoveTo(b.gameObject, iTween.Hash(
            "position", pos,
            "time", b.timeDelay,
            "easetype", iTween.EaseType.easeOutCubic));
    }

    // Call by invoke
    private void OnAttack2Complete()
    {
        SetNewAction(Action.Idle);
        OnBeginIdle();
    }

    #endregion 
}
