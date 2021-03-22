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
            return Mathf.Max(0, _damageOriginal - (DamageDecrease + (int)((_damageOriginal) * DamagePercentDecrease)));
        }
        set
        {
            _damageOriginal = value;
        }
    }
    private int DamageDecrease = 0;
    private float DamagePercentDecrease = 0;
    public bool IsCritical = false;
    public Vector3 PointHit;
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

    [Tooltip("Trung Hòa Buffbaf, Mặc định false")]
    public bool Mediated = false;
    public string TextMediated = "<color=red>" + Languages.getString("Loaij") + "</color><color=blue>" + Languages.getString("Bor") + "</color>";
    [Tooltip("Lực bật lùi, Mặc định 0.2f")]
    public float BackForce = 4;

    public bool Dodged = false;
    public string TextDodged = Languages.getString("Ne");
    public bool CanDodge => FromGunWeapon;
    // From
    public bool FromMeleeWeapon = false;
    public bool FromGunWeapon = false;
    public bool FromTNT = false;

    public bool CanDestroyBullet => FromMeleeWeapon || FromMeleeWeapon;
    public virtual void Decrease(int a)
    {
        DamageDecrease += a;
    }

    public virtual void DecreaseByPercent(float _0to1_)
    {
        DamagePercentDecrease += _0to1_;
    }

    public DamageData Clone => (DamageData)this.MemberwiseClone();

    private List<string> listString;
    public void AddText(string a)
    {
        if (listString == null)
        {
            listString = new List<string>();
        }
        listString.Add(a);
    }

    public List<string> GetStringShow()
    {
        List <string> ss = new List<string>();
        if (!Dodged)
        {
            string s = ShowText.StartColor(EntityManager.GetNameOfColorByElement(Type)) + Damage.ToString() + ShowText.EndColor();
            ss.Add(s);
        } else
        {
            ss.Add(TextDodged);
        }
        if (Mediated)
        {
            ss.Add(TextMediated);
        }
        if (listString != null)
            ss.AddRange(listString);
        return ss;
    }
}
