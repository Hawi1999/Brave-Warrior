using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Skill/Glide")]
public class Glide : Skill
{
    public float time;
    public float speed;
    public float density = 5;

    bool skilling = false;
    public override bool isReady => base.isReady && !skilling;
    public override void StartSkill()
    {
        base.StartSkill();
        skilling = true;
        StartGlide();
    }

    void StartGlide()
    {
        Force.BackForce(this.gameObject, host.DirectionCurrent, speed, time);
        Illusion.Create(this.GetComponent<SpriteRenderer>(), time, (int)(speed * density));
        host.LockMove.Register(codeSkill);
        host.LockAttack.Register(codeSkill);
        host.LockColliderTakeDamage.Register(codeSkill);
        Invoke("SkillComplete", time);
    }

    void SkillComplete()
    {
        host.LockAttack.CancelRegistration(codeSkill);
        host.LockMove.CancelRegistration(codeSkill);
        host.LockColliderTakeDamage.CancelRegistration(codeSkill);
        skilling = false;
        lastTimeSkill = Time.time;
    }


}
