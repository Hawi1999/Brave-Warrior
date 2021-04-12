using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIChooseWeaponSeller : MonoBehaviour, IUpdateLanguage
{
    [SerializeField] Animator anim;
    [SerializeField] Element[] listElement = new Element[3];
    [SerializeField]
    bool PermitChoose = false;

    [System.Serializable]
    public class Element
    {
        public Text text_name;
        public Text text_price;
        public int price;
        public string codeName;
        public Transform positionSpawn;
        public Chest chest;
    }

    private void Awake()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnAnimateOpenDone()
    {
        PermitChoose = true;
    }

    private void OnAnimateCloseDone()
    {
        gameObject.SetActive(false);
        OnDeShow?.Invoke();
    }


    public void OnBuy(string code)
    {
        if (!PermitChoose)
            return;
        for (int i = 0; i <listElement.Length; i++)
        {
            Element a = listElement[i];
            if (a.codeName == code)
            {
                if (listElement[i].chest != null)
                {
                    Notification.ReMind(Languages.getString("BanDaMuaLoaiRuongNayRoi"));
                    return;
                }
                ColorChest c = ColorChest.Copper;
                TypeChest t = TypeChest.WeaponCopper;
                switch (listElement[i].codeName)
                {
                    case "RuongDong":
                        c = ColorChest.Copper;
                        t = TypeChest.WeaponCopper;
                        break;
                    case "RuongBac":
                        c = ColorChest.Silver;
                        t = TypeChest.WeaponSilver;
                        break;
                    case "RuongVang":
                        c = ColorChest.Gold;
                        t = TypeChest.WeaponGold;
                        break;
                }
                if (!Personal.TryToSubDOLA(a.price))
                {
                    Notification.ReMind(Languages.getString("MuaThatBai"));
                    break;
                }
                a.chest = ChestManager.SpawnStartChest(c, t, CodeMap.Map1, a.positionSpawn.position);
                if (a.chest != null)
                {
                } else
                {
                    Notification.NoticeBelow(Languages.getString("CoLoiXayRaVoiHanhDongNay"));
                }
                break;
            }
        }
        PermitChoose = false;
        DeShow();
    }
    public void DeShow(){
        if (anim != null)
        {
            anim.SetTrigger("Close");
        }
    }

    public UnityAction OnDeShow;

    private void Start()
    {
        UpdateUI();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (anim != null)
        {
            anim.SetTrigger("Open");
        }
    }

    private void UpdateUI()
    {
        if (listElement.Length == 0)
        {
            return;
        }
        for (int i = 0; i < listElement.Length; i++)
        {
            Element e = listElement[i];
            if (e != null)
            {
                if (e.text_name != null)
                {
                    e.text_name.text = Languages.getString(e.codeName);
                }
                if (e.text_price != null)
                {
                    e.text_price.text = e.price.ToString();
                }
            }
        }
    }

    public void OnUpdateLanguage()
    {
        UpdateUI();
    }
}
