using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class DatTrongData
{
    public Sprite DaCao;
    public Sprite ChuaCao;
}
public class Data : MonoBehaviour
{
    [SerializeField] private DatTrongData _Dat;
    public static DatTrongData Dat;
    [SerializeField] private List<CayTrong> _Trees;
    public static List<CayTrong> Trees;
    private void Awake()
    {
        Trees = _Trees;
        Dat = _Dat;
    }
    public static Item getCayTrongByCode(string code)
    {
        foreach (CayTrong cay in Trees)
        {
            if (cay.CODE == code)
                return cay;
        }
        return null;
    }
    public static int getIndexByTrees(CayTrong cay)
    {
        for (int i = 0; i < Trees.Count; i++)
        {
            if (Trees[i].CODE == cay.CODE)
                return i;
        }
        return -1;
    }

    // Only for Sort
    public static int getIndexByItem(Item item)
    {
        int id = -1;
        for (int i = 0; i < Trees.Count; i++)
        {
            id++;
            if (Trees[i].CODE == item.CODE)
                return id;
        }
        return -1;
    }

    
}
