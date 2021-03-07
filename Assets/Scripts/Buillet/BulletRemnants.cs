using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRemnants : BulletBase
{
    [SerializeField] ParticleSystem VFXRemnants;

    protected override void OnBegin()
    {
        base.OnBegin();
        VFXRemnants.enableEmission = true;
        render.enabled = true;
    }

    protected override void OnAfterDestroyed()
    {
        VFXRemnants.enableEmission = false;
        render.enabled = false;
        StartCoroutine(WaitForRest(3));
    }

    IEnumerator WaitForRest(float a)
    {
        yield return new WaitForSeconds(a);
        Rest();
    }

}
