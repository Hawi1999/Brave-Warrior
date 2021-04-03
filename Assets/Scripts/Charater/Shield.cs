using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Skill
{
    public float timeShield = 4;
    public ShowShield prefab;

    private ShowShield ss = null;

    private bool skilling = false;

    public override bool isReady => base.isReady && !skilling;
    public override void StartSkill()
    {
        base.StartSkill();
        skilling = true;
        ss = Instantiate(prefab, host.center, Quaternion.identity);
        ss.SetTarget(host, timeShield);
        host.LockColliderTakeDamage.Register(codeSkill);
        ss.OnOutTime += EndSkill;
    }

    private void EndSkill()
    {
        skilling = false;
        host.LockColliderTakeDamage.CancelRegistration(codeSkill);
        ss = null;
        lastTimeSkill = Time.time;
    }

}
