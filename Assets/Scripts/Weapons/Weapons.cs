using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New List Weapons", menuName = "Data/List Weapons")]
public class Weapons : ScriptableObject, IUpdateItemEditor
{
    public List<Weapon> weapons = new List<Weapon>();

    [Header("Stick One Shot To Update List")]
    public bool Update = false;

    public void OnUpdate()
    {
        Debug.Log("Updated List Weapons");
        Weapon[] weapons = Resources.LoadAll<Weapon>("Weapons");
        this.weapons.Clear();
        for (int i = 0; i < weapons.Length; i++)
        {
            this.weapons.Add(weapons[i]);
        }
    }
}
