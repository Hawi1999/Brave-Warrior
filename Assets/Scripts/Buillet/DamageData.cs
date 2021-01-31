using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DamageData
{
    public Entity To;
    public Entity From;
    public DamageElement Type = DamageElement.Normal;
    private int DamageOriginal;
    private int DamageDecrease = 0;
    public RaycastHit2D hit;
    public Vector3 Direction;

    public DamageData(int da, Vector3 Direction, [DefaultValue(DamageElement.Normal)] DamageElement ele, Entity From, RaycastHit2D hit)
    {
        this.Type = ele;
        this.From = From;
        this.DamageOriginal = da;
        this.hit = hit;
        this.Direction = Direction;
        DamageDecrease = 0;
    }

    public DamageData(int da, Vector3 Direction, [DefaultValue(DamageElement.Normal)] DamageElement ele, Entity From)
    {
        this.Type = ele;
        this.From = From;
        this.DamageOriginal = da;
        this.hit = new RaycastHit2D();
        this.Direction = Direction;
        DamageDecrease = 0;
    }

    public int getDamage()
    {
        return Mathf.Max(0, DamageOriginal - DamageDecrease);
    }

    public void Decrease(int a)
    {
        DamageDecrease += a;
    }

    public void DecreaseByPercent(float _0to1_)
    {
        DamageDecrease += (int)(DamageOriginal * _0to1_);
    }

}
