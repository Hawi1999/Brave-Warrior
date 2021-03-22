using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletH70 : BulletRemnants
{
    [SerializeField]
    TrailRenderer trail;
    protected override void OnBegin()
    {
        base.OnBegin();
        trail.emitting = true;
        trail.enabled = true;
    }

    protected override void OnRest()
    {
        trail.emitting = false;
        trail.enabled = false;
        trail.Clear();
        base.OnRest();
    }
}
