using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class VFXSpawn : MonoBehaviour
{
    public UnityAction OnCompleteVFX;
    [SerializeField] float TimeLife;

    private float tim = 0;

    protected virtual void Start()
    {
        tim = 0;
    }

    protected virtual void Update()
    {
        tim += Time.deltaTime;
        if (tim > TimeLife)
        {
            OnCompleteVFX?.Invoke();
            Destroy(gameObject);
        }
    }

    
}
