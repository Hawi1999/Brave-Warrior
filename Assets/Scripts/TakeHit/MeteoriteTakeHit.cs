using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteTakeHit : MonoBehaviour, ITakeHit
{
    public Meteorite meteo;

    public Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }

    public void TakeDamaged(DamageData data)
    {
        if (meteo != null)
        {
            meteo.TakeDamage(data);
        }
    }
}
