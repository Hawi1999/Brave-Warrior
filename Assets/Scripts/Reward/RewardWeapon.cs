using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HienTenVuKhi))]
[RequireComponent(typeof(Weapon))]
public class RewardWeapon : Reward
{
    Weapon weapon => GetComponent<Weapon>();
    HienTenVuKhi hientenvukhi => GetComponent<HienTenVuKhi>();
    public override bool WaitingForGet
    {
        get
        {
            return weapon.TrangThai == WeaponStatus.Free;
        }
    }
    public override string Name
    {
        get
        {
            return "Reward " + weapon.GetNameOfWeapon();
        }
    }

    private void Update()
    {
        if (player != null)
            {
            if (isNearPlayer(1f) && player.WeaponCurrent != weapon)
            {
                ChooseReward.Instance.Add(this);
            } else if (player != null)
            {
                ChooseReward.Instance.Remove(this);
            }
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
        host.Equipment(weapon);
        ChooseReward.Instance.Remove(this);
        hientenvukhi.AnDi();
    }

    public override void Appear()
    {
        PositionControl pct = gameObject.AddComponent<PositionControl>();
        pct.SetUp(transform.position, transform.position + new Vector3(0,0.4f,0), 0.5f);
        pct.StartAnimation();
    }

    private bool isNearPlayer(float Distance)
    {
        if (player == null)
            return false;
        return Vector2.Distance(transform.position, player.getPosition()) <= Distance;
    }
}
