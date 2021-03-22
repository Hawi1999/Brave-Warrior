using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    static string Path = "Data/AllWeapons";
    static Weapons AllWeapons;
    public static void LoadData()
    {
        AllWeapons = Resources.Load<Weapons>(Path);
    }

    public static Weapon GetWeaponByName(string Name)
    {
        List <Weapon> Weapons = AllWeapons.weapons;
        return Array.Find(Weapons.ToArray(), e => e.GetNameOfWeapon() == Name);
    }

    public static Weapon[] GetWeaponsByName(string[] Name)
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (string name in Name)
        {
            Weapon weapon = GetWeaponByName(name);
            if (weapon != null)
            {
                weapons.Add(weapon);
            }
        }
        return weapons.ToArray();
    }
}
