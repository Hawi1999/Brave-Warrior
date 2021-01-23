using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum TypeSlot
{
    OnlyOne,
    Max100
}

public enum ButtonAction { 
    Add,
    Del,
    Set
}

public class SlotItem: MonoBehaviour
{
    private Item m_item;
    private int MaxSlot;
    private int m_soLuong;
    public bool isFull
    {
        get
        {
            return SoLuong >= MaxSlot;
        }
    }
    public Item item
    {
        get
        {
            return m_item;
        }
        set
        {
            m_item = value;
            setMaxSlot();
            UpdateHienThi();
        }
    }
    public int SoLuong
    {
        get
        {
            return m_soLuong;
        }
        set
        {
            m_soLuong = Mathf.Clamp(value, 0, MaxSlot); ;
            UpdateHienThi();
        }
    }
    
    public Text text_SoLuong;
    public Image Image;
    public void UpdateHienThi()
    {
        if (item == null)
            return;
        Image.sprite = item.Pic;
        switch (item.TypeSlot)
        {
            case (TypeSlot.OnlyOne):
                text_SoLuong.text = "";
                break;
            case (TypeSlot.Max100):
                text_SoLuong.text = SoLuong.ToString();
                break;
            }
    }
    public void Add(int SoLuong, out int Du)
    {
        Du = this.SoLuong + SoLuong - MaxSlot;
        this.SoLuong = Mathf.Clamp(this.SoLuong + SoLuong, 0, MaxSlot);
    }
    public void Sub(int SoLuong, out int ConLai)
    {
        ConLai = this.SoLuong - SoLuong;
        this.SoLuong = ConLai;
    }
    private void setMaxSlot()
    {
        if (item.TypeSlot == TypeSlot.OnlyOne)
            MaxSlot = 1;
        if (item.TypeSlot == TypeSlot.Max100)
            MaxSlot = 10000;
    }
    public virtual void setSelectedActive(bool tf)
    {
        Debug.Log("Ban chua Overide ham nay");
    }
    public virtual void setButtonAction(UnityAction action, ButtonAction bt)
    {
        Debug.Log("Ban chua Overide ham nay");
    }

    public virtual GameObject getSelected()
    {
        Debug.Log("Ban chua Overide ham nay");
        return null;
    }
}
