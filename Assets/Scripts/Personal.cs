using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PersonalSave
{
    public int DKN = 0;
    public int DOLA = 1000;
    public int Dia = 0;
    public int TheLuc = 100;
    public ThoiGian lastUpdateTL = new ThoiGian(DateTime.Now);
}
public class Personal : MonoBehaviour, IBattle
{
    public static Personal Instance
    {
        get;

        private set;
    }
    public static PersonalSave PSNS;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
    }
    public static int DOLA
    {
        get 
        {
            return PSNS.DOLA;
        } 
        set
        {
            PSNS.DOLA = value;
            SaveData();
        }

    }
    public static int Dia
    {
        get
        {
            return PSNS.Dia;
        }
        set
        {
            PSNS.Dia = value;
            SaveData();
        }
    }
    public static int Coin;
    public static ThoiGian lastUpdateTL 
    {
        get
        {
            return PSNS.lastUpdateTL;
        }
        set
        {
            PSNS.lastUpdateTL = value;
            SaveData();
        }
    }
    private void Start()
    {
        LoadData();
        OnDOLAChanged += ThongBaoDOLAChanged;
        OnDiaChanged += ThongBaoDiaChanged;
    }
    public static UnityAction<int, int> OnDOLAChanged;
    public static UnityAction<int, int> OnDiaChanged;
    public static UnityAction<int, int> OnCoinChanged;

    public static void AddDOLA(int a)
    {
        int old = DOLA;
        DOLA += a;
        OnDOLAChanged?.Invoke(old, DOLA);
    }
    public static bool TryToSubDOLA(int a)
    {
        if (DOLA - a < 0)
        {
            return false;
        } else
        {
            AddDOLA(-a);
            return true;
        }
    }
    public static void AddDia(int a)
    {
        int old = Dia;
        Dia += a;
        OnDiaChanged?.Invoke(old, Dia);
    }

    public static bool TryToSubDia(int a)
    {
        if (Dia - a < 0)
        {
            return false;
        }
        else
        {
            AddDia(-a);
            return true;
        }
    }
    public static void AddCoin(int a)
    {
        int old = Coin;
        Coin += a;
        if (Coin != old)
        {
            OnCoinChanged?.Invoke(old, Coin);
        }
    }
    private void ThongBaoDOLAChanged(int o, int n)
    {
        int a = n - o;
        if (a >= 0)
        {
            Notification.NoticeBelow(Languages.getString("BanNhanDuoc") + " <color=yellow>$" + a.ToString() + "</color>. ");
        }
        else
        {
            Notification.NoticeBelow(Languages.getString("BanBiTru") + " <color=yellow>$" + (-a).ToString() + "</color>. ");
        }
    }
    private void ThongBaoDiaChanged(int o, int n)
    {
        Debug.Log("Da thong bao Dia");
        int a = n - o;
        if (a >= 0)
        {
            Notification.NoticeBelow(Languages.getString("BanNhanDuoc") + " < color =blue>" + a.ToString() + " " + Languages.getString("KimCuongXanh", a > 1) + "</color>. ");
        }
        else
        {
            Notification.NoticeBelow(Languages.getString("BanBiTru") + " <color=blue>" + (-a).ToString() + " " + Languages.getString("KimCuongXanh", a > 1) + "</color>. ");
        }
    }

    static void LoadData()
    {
        if (PlayerPrefs.HasKey("Save_Personal"))
        {
            PSNS = JsonUtility.FromJson<PersonalSave>(PlayerPrefs.GetString("Save_Personal"));
        } else
        {
            PSNS = new PersonalSave();
        }
    }
    static void SaveData()
    {
        PlayerPrefs.SetString("Save_Personal", JsonUtility.ToJson(PSNS));
    }

    public void OnSceneStarted()
    {
        
    }

    public void OnSceneEnded()
    {
        
    }

    public void OnSceneOpen()
    {
        Coin = 0;
    }
}
