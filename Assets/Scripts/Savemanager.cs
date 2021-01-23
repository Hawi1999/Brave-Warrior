using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KhoNongSanSave
{
    public List<SlotItemSave> SlotItemSaves;
    public KhoNongSanSave (List<SlotItemSave> SlotItemSaves)
    {
        this.SlotItemSaves = SlotItemSaves;
    }
}
[System.Serializable]
public class SlotItemSave 
{
    public ItemSave iTemSave;
    public int Soluong;
    public SlotItemSave (ItemSave itemSave, int SoLuong)
    {
        iTemSave = itemSave;
        Soluong = SoLuong;
    }
}
[System.Serializable]
public class ItemSave
{
    public string CODE;
    public ItemSave(string code)
    {
        CODE = code;
    }
}
[System.Serializable]
public class DatTrongSave
{
    public Vector2 DiaChi = new Vector2(0, 0);
    public TreeSave Tree = new TreeSave();
    public ThoiGian ThoiGianBatDauTrong;
    public TTCT TrangThai;
    public DatTrongSave(DatTrong dat)
    {
        DiaChi = dat.getDiaChi();
        Tree.CODE = dat.GetCODE();
        ThoiGianBatDauTrong = new ThoiGian(dat.getThoiGianTrong());
        TrangThai = dat.getTrangThai();
    }
}
[System.Serializable]
public class TreeSave
{
    public string CODE;
    public TreeSave()
    {
    }
}

[System.Serializable]
public class QuanlyVuonSave
{
    public List<DatTrongSave> DanhSachDat;
    public QuanlyVuonSave(List<DatTrongSave> a)
    {
        DanhSachDat = a;
    }
}
