using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Chest/new Chest")]
public class ChestData : ScriptableObject
{
    public TypeChest Type = TypeChest.Start;
    public int[] UuTien;
    public string[] NameOfRewards;

    public string getRandomReward()
    {
        int tong = 0;
        for (int i = 0; i < UuTien.Length; i++)
        {
            tong += UuTien[i];
        }
        int random = Random.Range(1, tong + 1);
        int max = 0;
        for (int i = 0; i < NameOfRewards.Length; i++)
        {
            max += UuTien[i];
            if (random <= max)
            {
                return NameOfRewards[i];
            }
        }
        return "";
    }
}
