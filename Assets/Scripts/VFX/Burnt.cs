using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnt : ElementalBuffBad
{
    private ControlPartice VFXBurnt;

    private float timeDelay = 0;
    private float timeRemaining = 0;
    private Entity target;
    public static float TILE = 0.02f;
    public static int MIN_DAMAGE = 2;
    public static int MAX_DAMAGE = 100000;
    public static float TIME_DELAY = 0.3f;

    private PoolingGameObject pool => PoolingGameObject.PoolingMain;
    private int id_fire => VFXManager.IDPooling_Fire;
    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeDelay += Time.deltaTime;
            if (timeDelay >= TIME_DELAY)
            {
                DamageData damageData = new DamageData();
                SetUpDamageData(damageData);
                target.TakeDamage(damageData);
                timeDelay -= TIME_DELAY;
            }
            VFXBurnt.transform.position = target.center;
        } else
        {
            EndUp();
        }
    }

    private void SetUpDamageData(DamageData damage)
    {
        damage.BackForce = 0;
        damage.Direction = Vector3.up;
        damage.FireFrom = true;
        damage.Type = DamageElement.Fire;
    }

    private void PauseVFX()
    {
        VFXBurnt.gameObject.SetActive(false);
    }

    private void ResumeVFX()
    {
        VFXBurnt.gameObject.SetActive(true);
    }

    public override void StartUp(Entity entity, float time)
    {
        VFXBurnt = pool.Spawn(id_fire,entity.transform.position, Quaternion.identity) as ControlPartice;
        VFXBurnt.Play();
        target = entity;
        VFXBurnt.transform.localScale = new Vector3(entity.size.x, entity.size.y, 1);
        VFXBurnt.transform.position = target.center;
        target.OnBuffsChanged?.Invoke(DamageElement.Fire, true);
        SetLissener(true);
        AddTime(time);
    }

    public void AddTime(float  time)
    {
        timeRemaining += time;
    }

    public override void EndUp()
    {
        SetLissener(false);
        VFXBurnt.Stop();
        target.OnBuffsChanged?.Invoke(DamageElement.Fire, false);
        base.EndUp();
    }

    public static void Chay(Entity entity, float time)
    {
        if (entity.TryGetComponent(out Burnt b))
        {
            b.AddTime(time);
        } else
        {
            entity.gameObject.AddComponent<Burnt>().StartUp(entity, time);
        }
    }

    private void EntityDead()
    {
        EndUp();
    }

    private void SetLissener(bool a)
    {
        if (a)
        {
            target.OnDeath += (Enemy) => EntityDead();
            target.OnIntoTheGound += () => EndUp();
            target.OnHide += PauseVFX;
            target.OnAppear += ResumeVFX;
        } else
        {
            target.OnDeath -= (Enemy) => EntityDead();
            target.OnIntoTheGound -= () => EndUp();
            target.OnHide -= PauseVFX;
            target.OnAppear -= ResumeVFX;
        }
    }
    
}
