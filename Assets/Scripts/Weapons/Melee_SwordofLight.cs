using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_SwordofLight : MeleeBase
{
    [SerializeField] ParticleSystem parCut;
    protected override void Start()
    {
        base.Start();
        parCut.Stop();
    }


    protected override void OnAttackBegin()
    {
        if (parCut != null)
        {
            parCut.Play();
        }
    }

    protected override void OnAttackEnd()
    {
        if (parCut != null)
        {
            parCut.Stop();
        }
    }
}
