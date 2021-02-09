using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DamageData : Object
{
    public Entity To = null;
    public Entity From = null;
    public int Damage
    {
        get
        {
            return Mathf.Max(0, _damageOriginal - DamageDecrease);
        }
        set
        {
            _damageOriginal = value;
        }
    }
    private int DamageDecrease = 0;
    public bool IsCritical = false;
    public RaycastHit2D hit = new RaycastHit2D();
    public Vector3 Direction = Vector3.up;

    private int _damageOriginal;
    // Đạn bình thường

    public DamageElement Type = DamageElement.Normal;

    // Đạn Điện
    public float timeGiatDien = 4;

    // Đạn Lửa
    public float FireTime = 4;
    public bool FireFrom = false;
    public float FireRatio = 0.2f;
    public float FireDamagePerSecond = 4;

    // Đạn Độc
    public float PoisonTime = 4;
    public bool PoisonFrom = false;
    public float PoisonRatio = 0.2f;
    public float PoisonDamagePerSecond = 5;

    // Đạn Băng
    public float IceTime = 1;
    public float IceRatio = 0.2f;

    public bool Mediated = false;
    public string TextMediated = "<color=red> Trung </color><color=blue>Hòa </color>";

    public virtual void Decrease(int a)
    {
        DamageDecrease += a;
    }

    public virtual void DecreaseByPercent(float _0to1_)
    {
        DamageDecrease += (int)(Damage * _0to1_);
    }

    public DamageData Clone => (DamageData)this.MemberwiseClone();

}
