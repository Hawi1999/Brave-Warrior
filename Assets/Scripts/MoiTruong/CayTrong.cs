using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seed", menuName = "Item/New Seed")]
public class CayTrong : Item
{
    public Sprite[] QuaTrinhLon;
    public int ThoiGianLon;
    public int SoLuong;
    public int GiaBan;
    public int GiaMua;
    public int Offset_Shorting;

    public override int getGiaBan()
    {
        return GiaBan;
    }

    public override int getGiaMua()
    {
        return GiaMua;
    }

    public override int getThoiGianTrong()
    {
        return ThoiGianLon;
    }
}
