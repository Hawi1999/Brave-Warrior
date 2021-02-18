using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeChest
{
    
    Copper = 0,
    Silver = 1,
    Gold = 2,
    Start = 3,
}


public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }
    public ChestData[] ChestDatas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public Chest ReWardChest(TypeChest type, Vector3 Position)
    {
        ChestData cdt = getDataChest(type);
        if (cdt == null)
        {
            Debug.Log("Không tìm thấy ChestData type = " + type);
            return null;
        }
        if (cdt.Prefabs == null)
        {
            Debug.Log("Không tìm thấy Prefabs ChestData type = " + type);
            return null;
        }
        Chest chest = Instantiate(cdt.Prefabs, Position, Quaternion.identity);
        chest.setUp(cdt);
        return chest;
    }


    public ChestData getDataChest(TypeChest type)
    {
        return Array.Find(ChestDatas, e => e.Prefabs.type == type);
    }
}
