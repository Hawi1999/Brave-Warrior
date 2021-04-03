using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisoned : ElementalBuffBad
{
    private ControlPartice VFXPoison;

    private float timeDelay = 0;
    private float timeRemaining = 0;
    private Entity target;
    public static float TILE = 0.005f;
    public static int MIN_DAMAGE = 4;
    public static int MAX_DAMAGE = 100000;
    public static float TIME_DELAY = 0.7f;


    private PoolingGameObject pool => PoolingGameObject.PoolingMain;
    private int id_poison => VFXManager.IDPooling_Poison;
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
        }
        else
        {
            EndUp();
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            VFXPoison.transform.position = target.center;
        }
    }

    private void SetUpDamageData(DamageData damage)
    {
        damage.BackForce = 0;
        damage.Direction = Vector3.up;
        damage.PoisonFrom = true;
        damage.Type = DamageElement.Poison;
    }

    private void PauseVFX()
    {
        VFXPoison.gameObject.SetActive(false);
    }

    private void ResumeVFX()
    {
        VFXPoison.gameObject.SetActive(true);
    }

    public override void StartUp(Entity entity, float time)
    {
        VFXPoison = pool.Spawn(id_poison,entity.center, Quaternion.identity) as ControlPartice;
        VFXPoison.transform.localScale = new Vector3(entity.size.x, entity.size.y, 1);
        VFXPoison.Play();
        if (entity != null)
        {
            target = entity;
            target.Harmful_Poison = true;
            target.OnValueChanged?.Invoke(Entity.HARMFUL_POISON);
            SetLissener(true);
        }
        AddTime(time);
    }

    private void SetLissener(bool a)
    {
        if (a)
        {
            target.OnDeath += (Enemy) => EntityDead();
            target.OnIntoTheGound += () => EndUp();
            target.OnOuttoTheGound += ResumeVFX;
            target.OnHide += PauseVFX;
            target.OnAppear += ResumeVFX;
        }
        else
        {
            target.OnOuttoTheGound -= ResumeVFX;
            target.OnDeath -= (Enemy) => EntityDead();
            target.OnIntoTheGound -= () => EndUp();
            target.OnHide -= PauseVFX;
            target.OnAppear -= ResumeVFX;
        }
    }
    public void AddTime(float time)
    {
        timeRemaining += time;
    }

    public override void EndUp()
    {
        SetLissener(false);
        VFXPoison.Stop();
        if (target != null)
        {
            target.Harmful_Poison = false;
            target.OnValueChanged?.Invoke(Entity.HARMFUL_POISON);
        }
        base.EndUp();
    }

    public static void NhiemDoc(Entity entity, float time)
    {
        if (entity.TryGetComponent(out Poisoned b))
        {
            b.AddTime(time);
        }
        else
        {
            entity.gameObject.AddComponent<Poisoned>().StartUp(entity, time);
        }
    }

    private void EntityDead()
    {
        EndUp();
    }
}
