using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThoiGian 
{
    public int Nam;
    public int Thang;
    public int Ngay;
    public int Gio;
    public int Phut;
    public int Giay;

    public ThoiGian(DateTime a)
    {
        Nam = a.Year;
        Thang = a.Month;
        Ngay = a.Day;
        Gio = a.Hour;
        Phut = a.Minute;
        Giay = a.Second;
    }

}
