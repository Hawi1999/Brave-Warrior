using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShowName))]
public class RewardWeapon : Reward
{
    public Weapon weapon;
    public ShowName showname;
    public string _Name;

    protected bool showed = false;
    public override bool WaitingForChoose
    {
        get
        {
            return weapon.TrangThai == WeaponStatus.Free && !player.IsWeapon(weapon) && base.WaitingForChoose;
        }
    }
    public override string Name
    {
        get
        {
            return _Name;
        }
    }
    public override void OnChoose(IManipulation manipulation)
    {
        if (manipulation != null && manipulation as Object == this)
        {
            showname.Show();
        }
        else
        {
            showname.Hide();
        }
    }
    public override void TakeManipulation(PlayerController host)
    {
        base.TakeManipulation(host);
        host.Equipment(weapon);
        showname.Hide();
    }

    public override void OnPlayerInto()
    {
        base.OnPlayerInto();
        if (!showed)
        {
            PositionControl pct = gameObject.AddComponent<PositionControl>();
            pct.SetUp(transform.position, transform.position + new Vector3(0,0.4f,0), 0.5f);
            pct.StartAnimation();
            showed = true;
        }
    }

    private void OnValidate()
    {
        weapon = GetComponent<Weapon>();
        showname = GetComponent<ShowName>();
        _Name = "Reward " + weapon.GetNameOfWeapon();
    }
}
