using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public enum TTCT
{
    ChuaCao, 
    DaCao
}
[RequireComponent(typeof(SpriteRenderer))]
public class DatTrong : MonoBehaviour
{
    public bool DaTruongThanh;
    public CayTrong Tree = null;
    private TTCT TrangThai = TTCT.ChuaCao;
    private Vector2 DiaChi = new Vector2(0, 0);
    public DateTime ThoiGianTrong;
    public SpriteRenderer CayTrongRender;
    public SpriteRenderer DatTrongRender;
    int last_id_sprite = -1;
    public bool DangCao = false;

    public CachChon cachChon = CachChon.Cuoc;
    public CachChonTrong cachChonTrong = CachChonTrong.Empty;
    public int getTheSecondsLeft()
    {
        if (Tree == null) return -1;
        int a = (int)(Tree.ThoiGianLon * 60 - (DateTime.Now - ThoiGianTrong).TotalSeconds);
        return a < 0 ? 0 : a;
    }

    void reSet()
    {
        Tree = null;
        TrangThai = TTCT.ChuaCao;
        last_id_sprite = -1;
        DaTruongThanh = false;

    }

    public int getTheSecondsDaTrong()
    {
        return (int)(DateTime.Now - ThoiGianTrong).TotalSeconds;
    }
    public void TrongCay(CayTrong cay)
    {
        Tree = cay;
        ThoiGianTrong = DateTime.Now;
        GameObject goj = new GameObject(Tree.Name + gameObject.transform.position);
        goj.transform.parent = transform;
        goj.transform.position = DiaChi;
        CayTrongRender = goj.AddComponent<SpriteRenderer>();
        CayTrongRender.sortingOrder = -(int)DiaChi.y * 10 - Tree.Offset_Shorting;
        CayTrongRender.sortingLayerName = "Current";
    }
    public void setStart(DatTrongSave dat)
    {
        if (dat.Tree.CODE != "")
        {
            Tree = GetCayTrongbyCODE(Data.Trees, dat.Tree.CODE);
        }
        TrangThai = dat.TrangThai;
        DiaChi = dat.DiaChi;
        ThoiGian a = dat.ThoiGianBatDauTrong;
        ThoiGianTrong = new DateTime(a.Nam, a.Thang, a.Ngay, a.Gio, a.Phut, a.Giay);
        StartGame();
    }
    public void setStart()
    {
        Tree = null;
        TrangThai = TTCT.ChuaCao;
        CayTrongRender = null;
        StartGame();
    }

    private void Update()
    {
        if (Tree != null)
        {
            int id = (getTheSecondsDaTrong() * (Tree.QuaTrinhLon.Length - 1))/ (Tree.ThoiGianLon * 60);
            id = id >= Tree.QuaTrinhLon.Length ? Tree.QuaTrinhLon.Length - 1 : id;
            if (id != last_id_sprite)
            {
                last_id_sprite = id;
                CayTrongRender.sprite = Tree.QuaTrinhLon[id];
                if (id == Tree.QuaTrinhLon.Length - 1)
                {
                    DaTruongThanh = true;
                }
            }
        }
        if (TrangThai == TTCT.ChuaCao)
        {
            DatTrongRender.sprite = Data.Dat.ChuaCao;
        }
        else
        {
            DatTrongRender.sprite = Data.Dat.DaCao;
        }
    }
    public void setCachChon()
    {
        if (TrangThai == TTCT.ChuaCao && !DangCao)
        {
            cachChon = CachChon.Cuoc;
        } else if (DangCao) {
            cachChon = CachChon.DangCuoc;
        } else if (Tree == null)
        {
            cachChon = CachChon.Trong;
        } else if (!DaTruongThanh)
        {
            cachChon = CachChon.ChoChin;
        } else
        {
            cachChon = CachChon.Thu;
        }
    }
    public void setSelected()
    {

    }

    public void ThuHoach()
    {
        Destroy(CayTrongRender.gameObject);
        DaTruongThanh = false;
        reSet();
    }

    public void setTrangThai(TTCT a)
    {
        TrangThai = a;
    }

    public Vector3 getDiaChi()
    {
        return DiaChi;
    }

    public void setDiaChi(Vector2 a)
    {
        DiaChi = a;
    }

    public string GetCODE()
    {
        if (Tree == null)
            return "";
        return Tree.CODE;
    }

    public bool DaCao()
    {
        return (TrangThai == TTCT.DaCao);
    }

    public void CaoDat()
    {
        TrangThai = TTCT.DaCao;
        DangCao = false;
        GetComponent<SpriteRenderer>().sprite = Data.Dat.DaCao;
    }

    public DateTime getThoiGianTrong()
    {
        return ThoiGianTrong;
    }

    public TTCT getTrangThai()
    {
        return TrangThai;
    }

    public void StartGame()
    {
        gameObject.transform.position = DiaChi;
        // Xu ly Cay dang trong
        if (Tree != null)
        {
            GameObject goj = new GameObject(Tree.Name + gameObject.transform.position);
            goj.transform.parent = transform;
            goj.transform.position = DiaChi;
            CayTrongRender = goj.AddComponent<SpriteRenderer>();
            CayTrongRender.sortingOrder = -(int)DiaChi.y * 10 - Tree.Offset_Shorting;
            CayTrongRender.sortingLayerName = "Current";
        }
        DatTrongRender = GetComponent<SpriteRenderer>();
        DatTrongRender.sortingOrder = -800;
        DatTrongRender.sortingLayerName = "LaneMAP";
    }

    public static CayTrong GetCayTrongbyCODE(List<CayTrong> cay, string CODE)
    {
        for (int i = 0; i < cay.Count; i++)
        {
            if (cay[i].CODE == CODE)
            {
                return cay[i];
            }
        }
        Debug.Log("Khong Tim Thay Cay Boi ID Khi Load Game");
        return null;
    }
}