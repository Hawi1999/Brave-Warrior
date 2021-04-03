using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public Sprite sprite;
    public string codeSkill;
    public float cooldown;
    [Range(0, 1f)]
    public float UseHealphy = 0;
    [SerializeField] protected Entity host;


    protected float lastTimeSkill = 0;
    public virtual bool isReady => Time.time - lastTimeSkill >= cooldown;
    public virtual void StartSkill()
    {
        if (host is PlayerController)
        {
            PlayerController player = host as PlayerController;
            player.UseHealPhy(UseHealphy);
        }
    }

    public virtual float GetCountDownPercent => Mathf.Clamp01(1 - (Time.time - lastTimeSkill) / cooldown);
    public virtual float GetCountDownSeconds
    {
        get
        {
            float a = Time.time - lastTimeSkill - cooldown;
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
