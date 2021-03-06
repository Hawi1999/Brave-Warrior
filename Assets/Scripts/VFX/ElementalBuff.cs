﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementalBuff : MonoBehaviour
{
    public abstract void StartUp(Entity entity, float time);

    public virtual void EndUp()
    {
        gameObject.SetActive(false);
    }

    public void EndLock()
    {
        EndUp();
    }
}
