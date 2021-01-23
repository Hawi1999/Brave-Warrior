using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DamageData
{
    public Entity From;
    public DamageElement Type = DamageElement.Normal;
    public int Damage;
    public RaycastHit2D hit;
    public Vector3 Direction;

    public DamageData(int da, Vector3 Direction, [DefaultValue(DamageElement.Normal)] DamageElement ele, Entity From, RaycastHit2D hit)
    {
        this.Type = ele;
        this.From = From;
        this.Damage = da;
        this.hit = hit;
        this.Direction = Direction;

    }


}
