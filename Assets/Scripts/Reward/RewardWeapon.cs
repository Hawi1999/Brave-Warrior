using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShowName))]
public class RewardWeapon : Reward
{
    public Weapon weapon;
    public ShowName showname;
    public string _Name;
    public override bool WaitingForChoose
    {
        get
        {
            return weapon.TrangThai == WeaponStatus.Free && player.WeaponCurrent != weapon;
        }
    }
    public override string Name
    {
        get
        {
            return _Name;
        }
    }

    private void Update()
    {
        if (player != null)
            {
            if (isNearPlayer(1f) && WaitingForChoose)
            {
                ChooseMinapulation.Instance.Add(this);
            } else if (player != null)
            {
                ChooseMinapulation.Instance.Remove(this);
            }
        }
    }
    public override void OnChoose(IManipulation manipulation)
    {
        if (manipulation == this)
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
        host.Equipment(weapon);
        ChooseMinapulation.Instance.Remove(this);
        showname.Hide();
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

    private void OnValidate()
    {
        weapon = GetComponent<Weapon>();
        showname = GetComponent<ShowName>();
        _Name = "Reward " + weapon.GetNameOfWeapon();
    }
}
