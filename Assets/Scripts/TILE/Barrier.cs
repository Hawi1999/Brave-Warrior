using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] int DoCung = 2;
    [SerializeField] GameObject VFXDestroy;

    public void TakeDamage(int a)
    {
        DoCung -= a;
        if (DoCung <= 0)
        {
            PhaHuy();
        }
    }

    private void PhaHuy()
    {
        if (VFXDestroy != null)
        {
        Instantiate(VFXDestroy, gameObject.transform.position, Quaternion.identity);
        } else
        {
            Debug.Log("Chưa có VFXDestroy cho Barrier");
        }
    }
}
