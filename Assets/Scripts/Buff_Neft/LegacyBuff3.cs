using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LegacyBuff1", menuName = "Buff/LegacyBuff3 (Hồi máu khi giết Enemy)")]
public class LegacyBuff3 : LegacyBuff
{
    int type = BuffRegister.TypeBuff.IncreaseRatioTakeHealthFromEnemyDied;
    [Range(0, 100)]
    [SerializeField]
    int Ratio = 10;
    [SerializeField]
    int ValueHealth = 3;

    private static List<Entity> Buffed = new List<Entity>();

    public override void OnHostTake(Entity entity)
    {
        if (entity is PlayerController)
        {
            base.OnHostTake(entity);
            if (!Buffed.Contains(entity))
            {
                Buffed.Add(entity);
                host.OnKilledTarget += (e) => OnKilled(host, e, Ratio);
            }
            host.take.Register(this, type, ValueHealth);
            host.take.OnValueChanged += OnTakeBuffValueChanged;
        }
    }

    private static void OnKilled(PlayerController host, Entity enemy, int Ratio)
    {
        if (Random.Range(0, 100) < Ratio)
        {
            int a = (int)host.take.GetValue(BuffRegister.TypeBuff.IncreaseRatioTakeHealthFromEnemyDied);
            host.AddHealth(a);
        }
    }

    protected override void OnTakeBuffValueChanged(int a)
    {
        base.OnTakeBuffValueChanged(a);
        if (a == TakeBuff.AMOUNT_GIVEBUFF_REGISTED)
        {
            if (!host.take.ExitBuff(type))
            {
                Buffed.Remove(host);
                host.OnKilledTarget -= (e) => OnKilled(host, e, Ratio);
            }
        }
    }


}
