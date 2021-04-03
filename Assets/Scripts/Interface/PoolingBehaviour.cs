using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingBehaviour : MonoBehaviour
{
    bool ready = true;
    public bool isReady => ready;
    public bool DestroyWhenDone;

    public virtual void Begin()
    {
        ready = false;
        OnBegin();
    }

    protected virtual void OnBegin()
    {
        gameObject.SetActive(true);
    }

    public virtual void Rest()
    {
        if (DestroyWhenDone)
        {
            Destroy(gameObject);
        }
        OnRest();
        ready = true;
    }

    protected virtual void OnRest()
    {
        gameObject.SetActive(false);
    }
}
