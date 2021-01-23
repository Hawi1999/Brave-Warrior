using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Dynamic;
public class Bag : MonoBehaviour
{
    [SerializeField]
    protected SlotItem PrejabsSlotItem;
    [SerializeField]
    protected GameObject Main;
    [SerializeField]
    protected GameObject Content;
    [SerializeField]
    protected BTThaoTacManHinh BT;
    [SerializeField]
    protected GameObject Empty_Bag;
    protected List<SlotItem> slotItems;

    protected virtual void Start()
    {
        StartUp();
    }
    protected virtual void AddItem(Item item, int SoLuong)
    {
        SlotItem slot = Array.Find(slotItems.ToArray(), e => e.item != null && e.item.CODE == item.CODE && !e.isFull);
        if (slot == null)
        {
            slot = Instantiate(PrejabsSlotItem, Content.transform) as SlotItem;
            slotItems.Add(slot);
            slot.item = item;
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
                AddItem(item, SoLuong);
            }
        }
    }
    protected virtual void UpdateList()
    {

    }
    protected void DelAllSlot()
    {
        for (int i = 0; i < Content.transform.childCount; i++)
        {
            Destroy(Content.transform.GetChild(i)?.gameObject);
        }
        slotItems = null;
    }
    protected virtual void DelSlot(SlotItem slot)
    {
        Destroy(slot.gameObject);
        slotItems.Remove(slot);
    }
    public virtual int getSoLuong(Item item)
    {
        int count = 0;
        foreach (SlotItem slot in slotItems)
        {
            if (slot.item.CODE == item.CODE)
            {
                count += slot.SoLuong;
            }
        }
        return count;
    }
    protected virtual void StartUp()
    {
        slotItems = new List<SlotItem>();
        Main.SetActive(false);
    }
    protected virtual void Open()
    {
        Main.SetActive(true);
    }
    protected virtual void Close()
    {
        Main.SetActive(false);
    }
    protected virtual void AnBT()
    {
        BT.gameObject.SetActive(false);
    }
    protected virtual void HienBT()
    {
        BT.gameObject.SetActive(true);
    }
    public List<SlotItem> getListSlotsItem()
    {
        return slotItems;
    }
    protected void SapXep()
    {
        for (int i = 0; i < slotItems.Count - 1; i++)
        {
            for (int j = i + 1; j < slotItems.Count; j++)
            {
                int a = Data.getIndexByItem(slotItems[i].item);
                int b = Data.getIndexByItem(slotItems[j].item);
                if (a > b)
                {
                    SwapSlotItem(slotItems[i], slotItems[j]);
                }
            }
        }
    }
    protected void SwapSlotItem(SlotItem a, SlotItem b)
    {
        Item item = a.item;
        a.item = b.item;
        b.item = item;

        int SoLuong = a.SoLuong;
        a.SoLuong = b.SoLuong;
        b.SoLuong = SoLuong;
    }

}
