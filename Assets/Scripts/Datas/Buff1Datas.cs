using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Data/Buff (Hiển thị dưới thanh trang thái của entity)")]
public class Buff1Datas : ScriptableObject
{
    public Sprite sprite;
    public Color colorName;
    public string Code
    {
        get
        {
            return name;
        }
    }
}
