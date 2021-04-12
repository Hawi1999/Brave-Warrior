using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public enum TypeChest
{
    RCopper = 0,
    RSilver = 1,
    RGold = 2,
    Start = 3,

    WeaponCopper = 4,
    WeaponSilver = 5,
    WeaponGold = 6,
}


public class ChestManager 
{
    static string Path = "DataMap";
    public static ChestData[] ChestDatas;
    public static Chest[] ChestPrefabs;

    public static void LoadData()
    {
        ChestDatas = Resources.LoadAll<ChestData>(Path);
        ChestPrefabs = Resources.LoadAll<Chest>(Path);
    }

    public static Chest SpawnStartChest(ColorChest color, TypeChest type, CodeMap codeMap, Vector3 Position)
    {
        ChestData cdt = getDataChest(type, codeMap);
        if (cdt == null)
        {
            Debug.Log("Không tìm thấy ChestData type = " + TypeChest.Start);
            return null;
        }
        Chest cpf = getChestPrefab(color);
        if (cpf == null)
        {
            Debug.Log("Không tìm thấy Prefabs Chest Colortype = " + color);
            return null;
        }
        Chest chest = GameObject.Instantiate(cpf, Position, Quaternion.identity);
        chest.SetUpData(cdt);
        return chest;
    }


    public static Chest SpawnReWardChest(ColorChest color, TypeChest type, Vector3 Position)
    {
        ChestData cdt = getDataChest(type, MAP_GamePlay.CodeMapcurent);
        if (cdt == null)
        {
            Debug.Log("Không tìm thấy ChestData type = " + type);
            return null;
        }
        Chest cpf = getChestPrefab(color);
        if (cpf == null)
        {
            Debug.Log("Không tìm thấy Prefabs Chest Colortype = " + color);
            return null;
        }
        Chest chest = GameObject.Instantiate(cpf, Position, Quaternion.identity);
        chest.SetUpData(cdt);
        return chest;
    }


    public static ChestData getDataChest(TypeChest type, CodeMap codemap)
    {
        return Array.Find(ChestDatas, e => e.Type == type && e.codeMap == codemap);
    }

    public static Chest getChestPrefab(ColorChest color)
    {
        return Array.Find(ChestPrefabs, e => e.colorChest == color);
    }
}
