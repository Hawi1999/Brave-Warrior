using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToTakeHit
{
    public ITakeHit takeHit;
    public float time;
    public TimeToTakeHit(ITakeHit takeHit, float time)
    {
        this.takeHit = takeHit;
        this.time = time;
    }
}
public interface ITakeHit 
{
    Collider2D GetCollider();
    void TakeDamaged(DamageData data);
}
