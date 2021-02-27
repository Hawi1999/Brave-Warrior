using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VungChonCayDeTrong : MonoBehaviour
{
    [SerializeField]
    private GameObject Legacy;
    private QuanLyVuon quanLyVuon;
    private List<CayTrong> list_tree;
    private List<GameObject> selecter;
    private List<GameObject> Cay;
    private string CODE_current = "";

    private RectTransform Content;
    private RectTransform Viewport;
    private Text Info;
    void Start()
    {
        quanLyVuon = GetComponent<QuanLyVuon>();
        Content = quanLyVuon.ChonCay.Content.GetComponent<RectTransform>();
        Viewport = quanLyVuon.ChonCay.Viewport.GetComponent<RectTransform>();
        Info = quanLyVuon.ChonCay.ThongTin;
        list_tree = Data.Trees;

        CODE_current = PlayerPrefs.GetString("ChonCayTrong_CODE");
        CapNhatDanhSachChonMoi();
        ShowSelected(CODE_current);
    }
    public void TreeSelected(string CODE)
    {
        ShowSelected(CODE);
        CODE_current = CODE;
        PlayerPrefs.SetString("ChonCayTrong_CODE", CODE);
    }
    public CayTrong getCayTrongDaChon()
    {
        if (CODE_current == "")
            return null;
        CayTrong cay = Array.Find(list_tree.ToArray(), e => e.CODE == CODE_current);
        if (cay == null)
            Debug.LogWarning("Loi tim cay: khong the tim thay cay boi CODE = " + CODE_current.ToString() + " de trong");
        return cay;
    }
    public void CapNhatDanhSachChonMoi()
    {
        // Xóa tất cả danh sách chọn nếu có
        for (int i = 0; i < Content.childCount; i++)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
        selecter = new List<GameObject>();
        Cay = new List<GameObject>();
        // Lấy Danh Sách cây trồng từ Data;
        list_tree = Data.Trees;

        // Thiết lập Size
        Content.sizeDelta = new Vector2(100 * list_tree.Count, 100);
        Viewport.sizeDelta = new Vector2(list_tree.Count * 100, 100);
        
        // Tạo ra dánh sách để chọn cây từ danh sách đã lấy từ Data
        for (int i = 0; i < list_tree.Count; i++)
        {
            CayTrong cay = list_tree[i];
            GameObject a = Instantiate(Legacy, quanLyVuon.ChonCay.Content.transform);
            Cay.Add(a);
            // Thiết lập tên
            a.name = cay.Name;
            // Thiết lập hình ảnh
            a.transform.GetChild(0).GetComponent<Image>().sprite = cay.Pic;
            // Thêm selected vào danh sách selecter
            selecter.Add(a.transform.GetChild(1).gameObject);
            // Thêm sự kiện khi ấn nút
            a.GetComponent<Button>().onClick.AddListener(() => TreeSelected(cay.CODE));
        }
        if (CODE_current != string.Empty)
        ShowSelected(CODE_current);
    }
    void ShowSelected(string CODE)
    {
        if (Cay == null) return;
        int index = getIndexByCODE(CODE);
        for (int i = 0; i < Cay.Count; i++)
        {
            if (i != index)
            {
                selecter[i].SetActive(false);
            } else
            {
                selecter[i].SetActive(true);
            }
        }
        if (CODE == "") return;
        Item item = Data.getCayTrongByCode(CODE);
        string a = MinuteToHour(item.getThoiGianTrong());
        Info.text = getNamebyCode(CODE) + " - <color=yellow>" + item.getGiaMua().ToString() + "$ -</color> <color=white>" + a + "</color>";
    }
    int getIndexByCODE(string CODE)
    {
        for (int i = 0; i < selecter.Count; i++)
        {
            if (list_tree[i].CODE == CODE)
                return i;
        }
        return -1;
    }
    string getNamebyCode(string CODE)
    {
        CayTrong cay = Array.Find(list_tree.ToArray(), e => CODE == e.CODE);
        if (cay == null)
            return "Chọn cây";
        else
            return cay.Name;
    }
    private string MinuteToHour(int Minutes)
    {
        if (Minutes <= 0) return "";
        if (Minutes < 60) return Minutes.ToString() + " Phút";
        int hour = Minutes / 60;
        Minutes = Minutes % 60;
        string a = hour.ToString() + "Giờ";
        if (Minutes != 0)
        {
            a += " ";
            a += Minutes + "Phút";
        }
        return a;
    }
}
