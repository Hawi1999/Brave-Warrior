using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New List Weapons", menuName = "Data/List Weapons")]
public class Weapons : ScriptableObject
{
    public List<Weapon> weapons = new List<Weapon>();
}
