using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ChonCayTrong
{
    public GameObject Main;
    public Button BT_HUY;
    public GameObject Content;
    public GameObject Viewport;
    public Text ThongTin;
}
[System.Serializable]
public class A1
{
    public GameObject main;
    public HienThiThongTinCayDangTrong htttcdt;
    public GameObject content;
}
[System.Serializable]
public class A2
{
    public GameObject Main;
    public KhoNongSan QuanLy;
    public GameObject Inventory;
    public GameObject Viewport;
    public GameObject Content;
}
public enum CachChon
{
    MuaDat,
    Cuoc,
    DangCuoc,
    Trong,
    ChoChin,
    Thu
}
public enum CachChonTrong
{
    Empty,
    XacNhan
}

public class QuanLyVuon : MonoBehaviour
{ 
    private List<DatTrong> dats;
    [SerializeField]
    private VFXThuHoach vfx;
    [SerializeField]
    private KhoNongSan Kho;
    private Vector2 Selected;
    public SortingLayer sortingLayer;
    public Transform HinhVuong;
    public GameObject CuocDat;
    public A1 InfoTreeNow;
    public ChonCayTrong ChonCay;
    public BTThaoTacManHinh BT_ThaoTac;
    private VungChonCayDeTrong VC;
    private bool DangTrongVuon = false;
    private Transform GO_TheoDoi;
    private DatTrong dat_current;
    private PlayerController player => PlayerController.PlayerCurrent;
    private void Awake()
    {
        HinhVuong.gameObject.SetActive(false);
    }
    void Start() 
    {
        StartUp();
        LoadGame();
    }
    void StartUp()
    {
        dats = new List<DatTrong>(); 
        VC = GetComponent<VungChonCayDeTrong>();
        Kho = FindObjectOfType<KhoNongSan>();
        ChonCay.Main.SetActive(false);
        ChonCay.Main.GetComponent<Button>().onClick.AddListener(() => onClickHuy());
    }
    private void Update()
    {
        if (DangTrongVuon && GO_TheoDoi != null)
        {
            setSelected(GO_TheoDoi.position);
            setDat_Current();
            if (dat_current != null)
            {
                dat_current.setCachChon();
            }
            setTextButton();
            if (dat_current != null && dat_current.Tree != null)
            {
                InfoTreeNow.content.SetActive(true);
                InfoTreeNow.htttcdt.HienThi(dat_current);
            } else
            {
                InfoTreeNow.content.SetActive(false);
            }
        } else
        {
            InfoTreeNow.content.SetActive(false);
        }
    }
    public void StartSystem()
    {
        dats = new List<DatTrong>() ;
        for (int i = 1; i <= 6; i++)
        {
            CreateNewDatTrong(new Vector2(i, -2));
        }
        
    }
    public void setSelected(Vector3 DiaChi)
    {
        Selected.x = (int)DiaChi.x;
        Selected.y = (int)DiaChi.y - 1;
        HinhVuong.position = Selected;
    }
    void setTextButton()
    {
        if (dat_current == null)
        {
            BT_ThaoTac.ChangeText(0, Languages.getString("MuaDat"));
        }
        else
        {
            switch (dat_current.cachChon)
            {
                case (CachChon.Cuoc):
                    BT_ThaoTac.ChangeText(0, Languages.getString("CuocDat"));
                    break;
                case (CachChon.DangCuoc):
                    BT_ThaoTac.ChangeText(0, Languages.getString("Cho"));
                    break;
                case (CachChon.Trong):
                    switch (dat_current.cachChonTrong)
                    {
                        case (CachChonTrong.Empty):
                            BT_ThaoTac.ChangeText(0, Languages.getString("Trong"));
                            break;
                        case (CachChonTrong.XacNhan):
                            BT_ThaoTac.ChangeText(0, Languages.getString("XacNhan"));
                            break;
                    }
                    break;
                case (CachChon.ChoChin):
                    BT_ThaoTac.ChangeText(0, Languages.getString("Cho"));
                    break;
                case (CachChon.Thu):
                    BT_ThaoTac.ChangeText(0, Languages.getString("ThuHoach"));
                    break;
            }
        }
    }
    void setDat_Current()
    {
        dat_current = getDatTrongSelecting();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "TuongTac")
        {
            DangTrongVuon = true;
            HinhVuong.gameObject.SetActive(true);
            GO_TheoDoi = collision.gameObject.transform;
            BT_ThaoTac.AddButton(0);
            BT_ThaoTac.AddListener(0, onClickChon);
            InfoTreeNow.main.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TuongTac")
        {
            DangTrongVuon = false;
            HinhVuong.gameObject.SetActive(false);
            GO_TheoDoi = null;
            BT_ThaoTac.DeleteButton(0);
            InfoTreeNow.main.SetActive(false);
        }
    }
    void onClickHuy()
    {
        ChonCay.Main.SetActive(false);
        GameController.MyJoy?.gameObject.SetActive(true);
        if (dat_current != null)
        dat_current.cachChonTrong = CachChonTrong.Empty;
        player.LockMove.CancelRegistration("ChonCay");
    }
    void onClickChon()
    {
        if (dat_current == null)
        {
            int gia = (int)((5000 + (dats.Count - 6) * 5000));
            Notification.AreYouSure(Languages.getString("MuaManhDatVoiGia") + " <color=yellow>$" + gia.ToString() + "</color>?", () => MuaDat(gia));
        }
        else
        {
            switch (dat_current.cachChon)
            {
                case (CachChon.Cuoc):
                    CaoDat();
                    break;
                case (CachChon.Trong):
                    switch (dat_current.cachChonTrong)
                    {
                        case (CachChonTrong.Empty):
                            SelectTree();
                            break;
                        case (CachChonTrong.XacNhan):
                            TrongCay();
                            onClickHuy();
                            break;
                    }
                    break;
                case (CachChon.Thu):
                    ThuHoach();
                    break;

            }
        }
    }
    void CaoDat()
    {
        DatTrong dat = dat_current;
        Instantiate(CuocDat, dat.transform).transform.position = dat.getDiaChi();
        StartCoroutine(Wait(dat, 1.5f));
        SaveGame();
    }
    void SelectTree()
    {
        dat_current.cachChonTrong = CachChonTrong.XacNhan;
        ChonCay.Main.SetActive(true);
        VC.CapNhatDanhSachChonMoi();
        GameController.MyJoy?.gameObject.SetActive(false);
        player.LockMove.Register("ChonCay");
    }
    void ThuHoach()
    {
        CayTrong cay = dat_current.Tree;
        Vector3 pos = dat_current.getDiaChi();
        Kho.AddItemSave(cay, cay.SoLuong);
        dat_current.ThuHoach();
        VFXManager.ThuHoach(vfx, cay, cay.SoLuong, pos);
        SaveGame();
    }
    IEnumerator Wait(DatTrong dat, float seconds)
    {
        dat.DangCao = true;
        yield return new WaitForSeconds(seconds);
        dat.CaoDat();
    }
    void MuaDat(int gia)
    {
        if (Personal.DOLA < gia)
        {
            Notification.ReMind(Languages.getString("MuaThatBai"));
            return;
        } else
        {
            Personal.AddDOLA(-gia);
            CreateNewDatTrong(Selected);
        }
        SaveGame();
        
    }
    void TrongCay()
    {
        CayTrong cay = VC.getCayTrongDaChon();
        if (cay == null)
        {
            return;
        }
        int gia = cay.getGiaMua();
        if (Personal.DOLA >= gia)
        {   
            DatTrong dat = getDatTrongSelecting();
            dat.TrongCay(cay);
            Personal.AddDOLA(-cay.getGiaMua());
            SaveGame();
        } else
        {
            Notification.ReMind(Languages.getString("MuaThatBai"));
        }


    }
    DatTrong getDatTrongSelecting()
    {
        foreach(DatTrong dat in dats)
        {
            Vector2 a = dat.getDiaChi();
            if (a.x == Selected.x && a.y == Selected.y)
                return dat;

        }
        return null;
    }
    void CreateNewDatTrong(Vector2 diachi)
    {
        DatTrong dattrong = new GameObject("DatTrong(" + diachi.x + "," + diachi.y + ")").AddComponent<DatTrong>() as DatTrong;
        dattrong.transform.parent = transform;
        dattrong.setDiaChi(diachi);
        dattrong.setStart();
        dats.Add(dattrong);
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Save_QuanLyVuon"))
        {
            Debug.Log("Loading Game ...");
            QuanlyVuonSave qlvdata = JsonUtility.FromJson<QuanlyVuonSave>(PlayerPrefs.GetString("Save_QuanLyVuon"));
            dats = new List<DatTrong>();
            int n = 0;
            foreach (DatTrongSave datdt in qlvdata.DanhSachDat)
            {
                DatTrong dat = new GameObject("DatTrong" + n.ToString()).AddComponent<DatTrong>() as DatTrong;
                dat.transform.parent = transform;
                dat.setStart(datdt);
                dats.Add(dat);
                n++;
            }
        } else
        {
            StartSystem();
        }
    }
    public void SaveGame()
    {
        Debug.Log("Saving Game ...");
        List<DatTrongSave> dattrong = new List<DatTrongSave>();
        foreach (DatTrong dat in dats)
        {
            DatTrongSave crdat = new DatTrongSave(dat);
            dattrong.Add(crdat);
        }
        QuanlyVuonSave qlvdata = new QuanlyVuonSave(dattrong);
        PlayerPrefs.SetString("Save_QuanLyVuon", JsonUtility.ToJson(qlvdata));
    }
}
