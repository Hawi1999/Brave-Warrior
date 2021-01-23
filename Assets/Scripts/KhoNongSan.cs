using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KhoNongSan : Bag
{
    public Button BT_HUY;
    [SerializeField]
    GoTo EventE;
    bool SanSangMo;
    
    public static KhoNongSanSave KNSS;
    protected override void Start()
    {
        base.Start();
        LoadData();
    }
    protected override void StartUp()
    {
        base.StartUp();
        BT.AddButton(0, "Mở Kho");
        BT.SetListener(0, Open);
        AnBT();
        BT_HUY.onClick.AddListener(() => Close()); ;
        EventE.OnGoIn += onGoIn;
        EventE.OnGoOut += onGoOut;
    }
    protected override void Close()
    {
        base.Close();
        BT.ChangeText(0, "Mở Kho");
        BT.SetListener(0, Open);
        HienBT();
        PlayerController.Instance.PermitMove = true;
    }
    protected override void Open()
    {
        base.Open();
        BT.ChangeText(0, "Sắp Xếp");
        BT.SetListener(0, SapXep);
        UpdateList();
        PlayerController.Instance.PermitMove = false;
    }

    public void AddItemSave(Item item, int SoLuong)
    {
        SlotItemSave slotSave = Array.Find(KNSS.SlotItemSaves.ToArray(), e => e.iTemSave.CODE == item.CODE);
        if (slotSave == null)
        {
            slotSave = new SlotItemSave(new ItemSave(item.CODE), SoLuong);
            KNSS.SlotItemSaves.Add(slotSave);
        } else
        {
            slotSave.Soluong += SoLuong;
        }
        SaveGame();
        UpdateList();
    }
    public void SubItemSave(Item item, int SoLuong)
    {
        SlotItemSave slotSave = Array.Find(KNSS.SlotItemSaves.ToArray(), e => e.iTemSave.CODE == item.CODE);
        if (slotSave != null)
        {
            slotSave.Soluong = Mathf.Clamp(slotSave.Soluong - SoLuong, 0, 99999);
            if(slotSave.Soluong == 0)
            {
                KNSS.SlotItemSaves.Remove(slotSave);
            }
        }
        SaveGame();
        UpdateList();
    }
    public override int getSoLuong(Item item)
    {
        SlotItemSave slotSave = Array.Find(KNSS.SlotItemSaves.ToArray(), e => e.iTemSave.CODE == item.CODE);
        if (slotSave == null)
        {
            return 0;
        }
        else
        {
            return slotSave.Soluong;
        }
    }
    private static void LoadData()
    {
        if (PlayerPrefs.HasKey("Save_KhoNongSan"))
        {
            KNSS = JsonUtility.FromJson<KhoNongSanSave>(PlayerPrefs.GetString("Save_KhoNongSan"));
        } else
        {
            KNSS = new KhoNongSanSave(new List<SlotItemSave>());
            SaveGame();
        }
    }
    public static void SaveGame()
    {
        PlayerPrefs.SetString("Save_KhoNongSan", JsonUtility.ToJson(KNSS));
    }
    protected override void UpdateList()
    {
        DelAllSlot();
        LoadData();
        slotItems = new List<SlotItem>();
        if (KNSS == null || KNSS.SlotItemSaves.Count <= 0)
        {

        } else
        {
            foreach (SlotItemSave slotSave in KNSS.SlotItemSaves)
            {
                AddItem(Data.getCayTrongByCode(slotSave.iTemSave.CODE), slotSave.Soluong);
            }
        }
        Empty_Bag.SetActive(slotItems.Count == 0);
        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, Mathf.Clamp(200*(slotItems.Count-1)/5 + 1,600, Mathf.Infinity));
    }
    
    protected override void HienBT()
    {
        if (SanSangMo)
        {
            BT.gameObject.SetActive(true);
        }
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
    public List<SlotItemSave> getDataItem()
    {
        LoadData();
        return KNSS.SlotItemSaves;
    }
}
