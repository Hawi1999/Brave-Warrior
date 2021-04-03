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
public class Personal : MonoBehaviour
{
    public static int DKN
    {
        set
        {
            PSNS.DKN = value;
            SaveData();
        }
        get
        {
            return PSNS.DKN;
        }
    }
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
    public static int TheLuc
    {
        get
        {
            return PSNS.TheLuc;
        }
        set
        {
            PSNS.TheLuc = Mathf.Clamp(value, 0, THELUCMAX);
            SaveData();
        }
    }
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
    public static int THELUCMAX;
    private void Start()
    {
        LoadData();
        THELUCMAX = 100;
        OnDOLAChanged += ThongBaoDOLAChanged;
        OnDiaChanged += ThongBaoDiaChanged;
        OnTheLucChanged += ThongBaoTLChanged;
        OnDKNChanged += ThongBaoDKNChanged;
    }

    public static UnityAction<int, int> OnDKNChanged;
    public static UnityAction<int, int> OnDOLAChanged;
    public static UnityAction<int, int> OnDiaChanged;
    public static UnityAction<int, int> OnTheLucChanged;

    public static void AddDKN(int a)
    {
        int o = DKN;
        int n = DKN + a;
        DKN = n;
        OnDKNChanged?.Invoke(o, n);
    }
    public static void AddDOLA(int a)
    {
        int old = DOLA;
        DOLA += a;
        OnDOLAChanged?.Invoke(old, DOLA);
    }
    public static void AddDia(int a)
    {
        int old = Dia;
        Dia += a;
        OnDiaChanged?.Invoke(old, Dia);
    }
    public static void AddTheLuc(int a)
    {
        int old = TheLuc;
        TheLuc += a;
        OnTheLucChanged?.Invoke(old, TheLuc);
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
    private void ThongBaoTLChanged(int o, int n)
    {
        int a = n - o;
        if (a >= 0)
        {
            Notification.NoticeBelow(Languages.getString("BanNhanDuoc") + " <color=orange>" + a.ToString() + " " + Languages.getString("TheLuc", a > 1) + "</color>. ");
        }
        else
        {
            Notification.NoticeBelow(Languages.getString("BanBiTru") + " <color=orange>" + (-a).ToString() + " " + Languages.getString("TheLuc", a > 1) + "</color>. ");
        }
    }
    private void ThongBaoDKNChanged(int o, int n)
    {
        int a = n - o;
        if (a >= 0)
        {
            Notification.NoticeBelow(Languages.getString("BanNhanDuoc") + " <color=cyan>" + a.ToString() + " Exp</color>. ");
        }
        else
        {
            Notification.NoticeBelow(Languages.getString("BanBiTru") + " <color=cyan>" + a.ToString() + " Exp</color>.");
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
}
