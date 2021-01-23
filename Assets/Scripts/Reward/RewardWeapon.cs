using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HienTenVuKhi))]
[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(BoxCollider2D))]
public class RewardWeapon : Reward
{
    Weapon weapon
    {
        get { return GetComponent<Weapon>(); }
    }
    HienTenVuKhi hientenvukhi
    {
        get
        {
            return GetComponent<HienTenVuKhi>();
        }
    }
    public override bool WaitingForGet
    {
        get
        {
            return weapon.TrangThai == TrangThaiTrangBiVuKhi.Tudo;
        }
    }
    public override string Name
    {
        get
        {
            return "Reward " + weapon.NameOfWeapon;
        }
    }
    public override void Choose(Reward reward)
    {
        if (this == reward)
        {
            hientenvukhi.HienLen();
        }
        else
        {
            hientenvukhi.AnDi();
        }
    }
    public override void TakeReward(PlayerController host)
    {
        host.TrangBi(weapon);
        hientenvukhi.AnDi();
    }
}
