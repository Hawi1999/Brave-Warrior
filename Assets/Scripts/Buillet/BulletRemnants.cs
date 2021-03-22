﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRemnants : BulletBase
{
    [SerializeField] float timeRemnants = 3f;

    bool destroyed = false;
    protected override void OnBegin()
    {
        base.OnBegin();
        destroyed = false;
        render.enabled = true;
    }
    protected override void UpdateCollision()
    {
        if (!destroyed)
        {
            base.UpdateCollision();
        }
    }

    protected override void UpdateTransform()
    {
        if (!destroyed)
        {
            base.UpdateTransform();
        }
    }

    protected override void OnAfterDestroyed()
    {
        render.enabled = false;
        destroyed = true;
        StartCoroutine(WaitForRest(3));
    }

    IEnumerator WaitForRest(float a)
    {
        yield return new WaitForSeconds(a);
        Rest();
    }

}
