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
        ready = true;
        if (DestroyWhenDone)
        {
            Destroy(gameObject);
        }
        OnRest();
    }

    protected virtual void OnRest()
    {
        gameObject.SetActive(false);
    }
}
