using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class HienThiThongTinCayDangTrong : MonoBehaviour
{
    public Image Pic;
    public Text Name;
    public Text SoLuong;
    public Text TimeLeft;
    public Slider sliderTime;
    public void HienThi(DatTrong dat)
    {
        // Hinh Anh
        if (dat.transform.childCount != 0)
        Pic.sprite = dat.Tree.Pic;
        // Ten
        Name.text = dat.Tree.Name;
        // So Luong
        SoLuong.text = Languages.getString("SoLuong") + ": " + dat.Tree.SoLuong.ToString();
        // Timeleft va slider
        long timedatrong = (long)(DateTime.Now - dat.getThoiGianTrong()).TotalMilliseconds;
        long tongthoigian = (long)dat.Tree.ThoiGianLon * 60 * 1000;
        long miniGiay = tongthoigian - timedatrong;
        long Giay = miniGiay / 1000;
        if (miniGiay > 0)
        {
            TimeLeft.text = GameController.getStringTime(Giay);
            sliderTime.value = (float)timedatrong / tongthoigian;
        } else
        {
            TimeLeft.text = Languages.getString("CoTheThuHoach");
            sliderTime.value = 1;
        }

    }

}


    

