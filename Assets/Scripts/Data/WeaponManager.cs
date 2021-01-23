using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    public List<Weapon> Weapons;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Weapon GetWeaponByName(string Name)
    {
        if (Instance == null)
            return null;
        List <Weapon> Weapons = Instance.Weapons;
        return Array.Find(Weapons.ToArray(), e => e.NameOfWeapon == Name);
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
