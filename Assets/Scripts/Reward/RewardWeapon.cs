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

    public override bool EqualTypeByChest(TypeReward type)
    {
        if (type == TypeReward.Gold1 || type == TypeReward.Gold2 || type == TypeReward.Gold3)
            return false;
        if (type == TypeReward.WeaponCommon && weapon.TypeOfWeapon == LevelWeapon.Common)
        {
            return true;
        }
        if (type == TypeReward.WeaponEpic && weapon.TypeOfWeapon == LevelWeapon.Epic)
        {
            return true;
        }
        if (type == TypeReward.WeaponRare && weapon.TypeOfWeapon == LevelWeapon.Rare)
        {
            return true;
        }
        if (type == TypeReward.WeaponVeryRare && weapon.TypeOfWeapon == LevelWeapon.VeryRare)
        {
            return true;
        }
        if (type == TypeReward.WeaponLegendary && weapon.TypeOfWeapon == LevelWeapon.Legendary)
        {
            return true;
        }
        return false;
    }

    public override void OnChoose(IManipulation manipulation)
    {
        base.OnChoose(manipulation);
        if (manipulation != null && manipulation as UnityEngine.Object == this)
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

    private void OnValidate()
    {
        weapon = GetComponent<Weapon>();
        showname = GetComponent<ShowName>();
        _Name = "Reward " + weapon.TypeOfWeapon.ToString() + " " + weapon.GetNameOfWeapon();
    }
}
