using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Chest/new Chest")]
public class ChestData : ScriptableObject
{
    public Chest Prefabs;
    public string[] NameOfRewards;
}
