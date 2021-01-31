using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HienTenVuKhi))]
[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(BoxCollider2D))]
public class RewardWeapon : Reward
{
    Weapon weapon => GetComponent<Weapon>();
    HienTenVuKhi hientenvukhi => GetComponent<HienTenVuKhi>();
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
        host.GetComponent<ChooseReward>().Remove(this);
        hientenvukhi.AnDi();
    }

    public override void Appear()
    {
        PositionControl pct = gameObject.AddComponent<PositionControl>();
        pct.SetUp(transform.position, transform.position + new Vector3(0,0.4f,0), 0.5f);
        pct.StartAnimation();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && WaitingForGet)
        {
            ChooseReward chooseReward = collision.GetComponent<ChooseReward>();
            chooseReward.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && WaitingForGet)
        {
            ChooseReward chooseReward = collision.GetComponent<ChooseReward>();
            chooseReward.Remove(this);
        }
    }
}
