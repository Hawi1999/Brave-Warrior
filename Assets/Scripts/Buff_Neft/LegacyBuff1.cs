using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LegacyBuff1", menuName = "Buff/LegacyBuff1 (Gây sát thương lan)")]
public class LegacyBuff1 : LegacyBuff
{
    [SerializeField] int Damage = 3;

    [SerializeField] ElectricConnect prefabElectricConnect;
    

    static List<Entity> Buffed = new List<Entity>();
    public override void OnHostTake(Entity entity)
    {
        base.OnHostTake(entity);
        host.take.Register(this, BuffRegister.TypeBuff.IncreaseDamageLegacyBuff1ByValue, Damage);
        if (!Buffed.Contains(entity))
        {
            Buffed.Add(entity);
            host.OnHitTarget += (e) => HitEnemy(host,e, prefabElectricConnect);
        }
        host.take.OnValueChanged += OnTakeBuffValueChanged;
    }

    public static List<TimeToAction> tta = new List<TimeToAction>();
    public static void HitEnemy(PlayerController player, Entity entity, ElectricConnect prefab)
    {
        if (entity is Enemy)
        {
            if (!Check(player))
            {
                return;
            }
            bool has = false;
            Enemy enemy = entity as Enemy;
            if (enemy.CurrentDamageData.FromLegacyBuff1)
            {
                return;
            }
            Collider2D[] col = Physics2D.OverlapCircleAll(enemy.center, 5f, player.layerTargetFind);
            foreach (Collider2D c in col)
            {
                Enemy en = c.gameObject.GetComponent<Enemy>();
                if (en == null || en == enemy)
                {
                    continue;
                }
                has = true;
                DamageData damage = new DamageData();
                SetUpDamageData(player, damage);
                ElectricConnect electricConnect = GameObject.Instantiate(prefab);
                electricConnect.SetUp(enemy, en);
                en.TakeDamage(damage);
            }
            if (has)
            {
                ResetTime(player);
            }
        }
    }

    public static  void SetUpDamageData(PlayerController player, DamageData damage)
    {
        player.SetUpDamageDataOutSide(damage);
        damage.Damage = (int)player.take.GetValue(BuffRegister.TypeBuff.IncreaseDamageLegacyBuff1ByValue);
        damage.FromLegacyBuff1 = true;
    }

    public static bool Check(PlayerController player)
    {
        if (tta.Count == 0)
        {
            tta.Add(new TimeToAction(player, Time.time));
            return true;
        }
        for (int i = tta.Count - 1; i > -1; i--)
        {
            TimeToAction tt = tta[i];
            if (tt.Object == player)
            {
                if (tt.time + 0.5f < Time.time)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } else
            {
                if (tt.time + 0.5f < Time.time)
                {
                    tta.RemoveAt(i);
                }
            }
        }
        tta.Add(new TimeToAction(player, Time.time));
        return true;
    }

    public static void ResetTime(PlayerController player)
    {
        for (int i = tta.Count - 1; i > -1; i--)
        {
            TimeToAction tt = tta[i];
            if (tt.Object == player)
            {
                tt.time = Time.time;
            }
        }
    }
    protected override void OnTakeBuffValueChanged(int a)
    {
        base.OnTakeBuffValueChanged(a);
        if (a == TakeBuff.AMOUNT_GIVEBUFF_REGISTED)
        {
            if (!host.take.ExitBuff(BuffRegister.TypeBuff.IncreaseDamageLegacyBuff1ByValue))
            {
                Buffed.Remove(host);
                host.OnHitTarget -= (e) => HitEnemy(host, e, prefabElectricConnect);
            }
        }
    }

    private void OnDestroy()
    {
        
    }
}
