using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHit : MonoBehaviour
{
    public virtual void TakeDamaged (DamageData data)
    {
        Debug.Log("Bạn phải Overide hàn TakeDamaged");
    }
}
