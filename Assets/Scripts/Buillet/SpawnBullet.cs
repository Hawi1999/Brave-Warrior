using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public int Damage = 4;
    public bool StartWhenAwake = false;
    protected DamageData damageData;

    protected bool spawning;
    protected virtual void Awake()
    {
        spawning = StartWhenAwake;
    }
    public void SetUp(DamageData damage)
    {
        this.damageData = damage;
    }

    public virtual void StartSpawn()
    {
        spawning = true;
    }

    protected DamageData SetUpDamageData()
    {
        DamageData damage = this.damageData.Clone;
        damage.Damage = this.Damage;
        return damage;
    }

}
