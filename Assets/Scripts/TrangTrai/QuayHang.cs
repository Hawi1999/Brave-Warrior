using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[Serializable] 
public enum QuayHang_TGD
{
    Mua, Ban
}

[Serializable]
public class A3 {
    public GameObject InfoMain;
    public GameObject MainSL;
    public Image Pic_Selected;
    public Text name_Selected;
    public Button Min;
    public Button Max;
    public Button L;
    public Button R;
    public Text SL;
    public Text ST;
    public Button MainTT;

    public Text BanKho;
    public Text MuaNongSan;
    public Text Empty;
    public Text Name;
}

public class QuayHang : Bag, IUpdateLanguage
{
    [SerializeField]
    private KhoNongSan Kho;
    [SerializeField]
    private Button BT_HUY;
    [SerializeField]
    private GoTo EventExpected;
    bool SanSangMo;
    [SerializeField]
    private A3 Info;
    private int min = 1;
    private int max = 1;
    public Button BT_Ban;
    public Button BT_Mua;
    public Color cl_Sel;
    public Color cl_Des;
    private bool donewSlot = true;
    private bool donewAmount = true;
    QuayHang_TGD _TGD;
    QuayHang_TGD TGD
    {
        get
        {
            return _TGD;
        }
        set
        {
            QuayHang_TGD oldTGD = _TGD;
            _TGD = value;
            if (oldTGD != _TGD)
            {
                onTGDChanged();
            }
            
        }
    }
    private SlotItem _slotCurrent;
    SlotItem slotCurrent
    {
        get{
            return _slotCurrent;
        }
        set
        {
            SlotItem old = _slotCurrent;
            _slotCurrent = value;
            if (old != _slotCurrent || donewSlot)
            {
                onSlotCurrentChanged();
            }
        }
    }
    int _aMountCurrent;
    int aMountCurrent
    {
        get
        {
            return _aMountCurrent;
        }
        set
        {
            int old = _aMountCurrent;
            _aMountCurrent = value;
            if (_aMountCurrent != old || donewAmount)
            {
                onAMountCurrentChanged(); 
            }
            
        }
    }

    #region Value_Changed
    private void onSlotCurrentChanged()
    {
        donewSlot = false;
        donewAmount = true;
        aMountCurrent = 1;
        setAllSelectedActive(false);
        slotCurrent?.getSelected().SetActive(true);
        setMaxAndMin();
    }
    private void onTGDChanged()
    {
        donewAmount = true;
        donewSlot = true;
        setColorButtonMuaBan();
        UpdateList();
        setNameMainTT();
    }
    private void onAMountCurrentChanged()
    {
        donewAmount = false;
        UpdateInfo();
    }
    #endregion
    protected override void StartUp()
    {
        base.StartUp();
        if (!Kho)
        {
            Kho = FindObjectOfType<KhoNongSan>();
        }
        AnBT();
        BT.AddButton(0, Languages.getString("MoQuay"));
        BT.AddListener(0, Open);
        BT_HUY.onClick.AddListener(onClickHuy);
        BT_Ban.onClick.AddListener(() => onClickBan_Mua(QuayHang_TGD.Ban));
        BT_Mua.onClick.AddListener(() => onClickBan_Mua(QuayHang_TGD.Mua));
        Info.L.onClick.AddListener(onClickL);
        Info.R.onClick.AddListener(onClickR);
        Info.Min.onClick.AddListener(onClickMin);
        Info.Max.onClick.AddListener(onClickMax);
        Info.MainTT.onClick.AddListener(onClickMainTT);
        EventExpected.OnGoIn += onGoIn;
        EventExpected.OnGoOut += onGoOut;
    }

    public void OnUpdateLanguage()
    {
        Info.BanKho.text = Languages.getString("BanVatPham");
        Info.MuaNongSan.text = Languages.getString("MuaVatPham");
        Info.Name.text = Languages.getString("QuayHang");
        Info.Empty.text = Languages.getString("BanKhongCoVatPham");
    }
    protected override void Open()
    {
        base.Open();
        TGD = QuayHang_TGD.Ban;
        UpdateList();
        UpdateInfo();
        AnBT();
    }
    protected override void Close()
    {
        base.Close();
        HienBT();
    }
    protected override void HienBT()
    {
        if (SanSangMo)
            base.HienBT();
    }
    protected override void AddItem(Item item, int SoLuong)
    {
        if (TGD == QuayHang_TGD.Ban)
        {
            SlotItem slot = Array.Find(slotItems.ToArray(), e => e.item != null && e.item.CODE == item.CODE && !e.isFull);
            if (slot == null)
            {
                slot = Instantiate(PrejabsSlotItem, Content.transform) as SlotItem;
                slotItems.Add(slot);
                slot.item = item;
                slot.setButtonAction(() => onClickItem(slot), ButtonAction.Set);
                AddItem(item, SoLuong);
            }
            else
            {
                int Du;
                slot.Add(SoLuong, out Du);
                if (Du > 0)
                {
                    slot = Instantiate(PrejabsSlotItem, Content.transform) as SlotItem;
                    slotItems.Add(slot);
                    slot.item = item;
                    slot.setButtonAction(() => onClickItem(slot), ButtonAction.Set);
                    AddItem(item, SoLuong);
                }
            }
        }
        if (TGD == QuayHang_TGD.Mua)
        {
            SlotItem slot = Array.Find(slotItems.ToArray(), e => e.item != null && e.item.CODE == item.CODE);
            if (slot == null)
            {
                slot = Instantiate(PrejabsSlotItem, Content.transform) as SlotItem;
                slotItems.Add(slot);
                slot.item = item;
                slot.setButtonAction(() => onClickItem(slot), ButtonAction.Set);
                slot.GetComponentInChildren<Text>().text = "$" + item.getGiaMua().ToString();
            }
        }
    }
    protected override void UpdateList()
    {
        DelAllSlot();
        slotItems = new List<SlotItem>();
        if (TGD == QuayHang_TGD.Ban)
        {   
            List<SlotItemSave> kho_list = Kho.getDataItem();
            foreach (SlotItemSave slotsave in kho_list)
            {
                AddItem(Data.getCayTrongByCode(slotsave.iTemSave.CODE), slotsave.Soluong);
            }
        }
        if (TGD == QuayHang_TGD.Mua)
        {
            List<CayTrong> Trees = Data.Trees;
            foreach (CayTrong cay in Trees)
            {
                AddItem(cay, 1);
            }
        }
        if (slotItems.Count != 0)
        {
            slotCurrent = slotItems[0];
        } else
        {
            slotCurrent = null;
        }
        Empty_Bag.SetActive(slotItems.Count == 0);
        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(600, Mathf.Clamp(((slotItems.Count - 1)/4 + 1) * 150 , 550, Mathf.Infinity));
    }
    private void onGoIn(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            SanSangMo = true;
            HienBT();
        }
    }
    private void onGoOut(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            SanSangMo = false;
            AnBT();
        }
    }
    private void onClickHuy()
    {
        Close();
    }
    private void onClickItem(SlotItem slotItem)
    {
        if (slotCurrent == slotItem)
            return;
        slotCurrent = slotItem;
    }
    private void UpdateInfo()
    {
        if (TGD == QuayHang_TGD.Ban)
        {
            if (slotCurrent == null)
            {
                Info.Pic_Selected.gameObject.SetActive(false);
                Info.name_Selected.text = Languages.getString("ChonVatPham");
                Info.ST.text = 0.ToString();
                Info.SL.text = 0.ToString();
            }
            else
            {
                Item item = slotCurrent.item;
                Info.Pic_Selected.gameObject.SetActive(true);
                Info.Pic_Selected.sprite = item.Pic;
                Info.name_Selected.text = item.Name;
                Info.ST.text = (aMountCurrent * item.getGiaBan()).ToString();
                Info.SL.text = aMountCurrent.ToString();
            }
            
        }
        if (TGD == QuayHang_TGD.Mua)
        {
            Item item = slotCurrent.item;
            Info.Pic_Selected.gameObject.SetActive(true);
            Info.Pic_Selected.sprite = item.Pic;
            Info.name_Selected.text = Languages.getString("Mam") + " " + item.Name;
            Info.SL.text = aMountCurrent.ToString();
            Info.ST.text = (aMountCurrent * item.getGiaMua()).ToString();
        }
    } 
    public void onClickMin()
    {
        aMountCurrent = min;
    }
    public void onClickMax()
    {
        aMountCurrent = max;
    }
    public void onClickL()
    {
        aMountCurrent = Mathf.Clamp(aMountCurrent - 1, min, max);
    }
    public void onClickR()
    {
        aMountCurrent = Mathf.Clamp(aMountCurrent + 1, min, max);
    }
    public void onClickMainTT()
    {
        if (!slotCurrent) return;
        if (TGD == QuayHang_TGD.Ban)
        {
            Notification.AreYouSure(Languages.getString("BanVatPhamVoiGia") + " <color=yellow>$" + (slotCurrent.item.getGiaBan() * aMountCurrent).ToString() + "</color>?", Sell);
        }
        if (TGD == QuayHang_TGD.Mua)
        {
            Notification.ReMind(Languages.getString("MamSeDuocMuaTuDongKhiBanTrong"));
        }
    }
    public void onClickBan_Mua(QuayHang_TGD a)
    {
        if (a != TGD)
        {
            TGD = a;
        }
    }
    private void setColorButtonMuaBan()
    {
        if (TGD == QuayHang_TGD.Ban)
        {
            BT_Ban.GetComponent<Image>().color = cl_Sel;
            BT_Mua.GetComponent<Image>().color = cl_Des;
        }
        if (TGD == QuayHang_TGD.Mua)
        {
            BT_Ban.GetComponent<Image>().color = cl_Des;
            BT_Mua.GetComponent<Image>().color = cl_Sel;
        }
    }
    private void setAllSelectedActive(bool a)
    {
        foreach(SlotItem slot in slotItems)
        {
            slot.getSelected().SetActive(a);
        }
    }
    private void Sell()
    {
        Kho.SubItemSave(slotCurrent.item, aMountCurrent);
        Personal.AddDOLA(slotCurrent.item.getGiaBan() * aMountCurrent);
        UpdateList();
    }
    private void setMaxAndMin()
    {
        if (slotCurrent == null) return;
        min = 1;
        if (TGD == QuayHang_TGD.Ban)
            max = slotCurrent.SoLuong;
        if (TGD == QuayHang_TGD.Mua)
            max = 1;
    }
    private void setNameMainTT()
    {
        if (TGD == QuayHang_TGD.Ban)
        {
            Info.MainTT.transform.GetComponentInChildren<Text>().text = Languages.getString("Ban");
        }
        if (TGD == QuayHang_TGD.Mua)
        {
            Info.MainTT.transform.GetComponentInChildren<Text>().text = Languages.getString("Mua");
        }
    }
}
