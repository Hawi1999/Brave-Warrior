using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public Sprite sprite;
    public string codeSkill;
    [SerializeField] float cooldown;
    [Range(0, 1f)]
    public float UseHealphy = 0;
    [SerializeField] protected Entity host;

    public float CoolDown
    {
        get
        {
            if (host != null)
            {
                float ftang = host.take.GetValue(BuffRegister.TypeBuff.IncreaseCoolDownSkillByFix100);
                float fgiam = host.take.GetValue(BuffRegister.TypeBuff.DecreaseCoolDownSkillByFix100);
                float ratiotang = (1 - 100f / (100f + ftang));
                float ratiogiam = (1 - 100f / (100f + fgiam));
                return cooldown * (1 +  ratiotang - ratiogiam);
            }
            return cooldown;
        }
    }


    protected float lastTimeSkill = 0;
    public virtual bool isReady => Time.time - lastTimeSkill >= CoolDown;
    private void Awake()
    {
        lastTimeSkill = Time.time - CoolDown;
    }
    public virtual void StartSkill()
    {
        if (host is PlayerController)
        {
            PlayerController player = host as PlayerController;
            player.UseHealPhy(UseHealphy);
        }
    }

    public virtual float GetCurrentCountDownPercent => Mathf.Clamp01(1 - (Time.time - lastTimeSkill) / CoolDown);
    public virtual float GetCurrentCountDownSeconds
    {
        get
        {
            float a = Time.time - lastTimeSkill - CoolDown;
            if (a >= 1)
                return (int)a;
            else
            {
                a *= 10;
                int b = Mathf.CeilToInt(a);
                a = b / 10f;
                return a;
            }
        }
    }
}
