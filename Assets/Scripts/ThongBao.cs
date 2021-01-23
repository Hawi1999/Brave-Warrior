using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ThongBao : MonoBehaviour
{
    public Button OK;
    public Text ThongTin;
    public Button Huy;

    public static void ChacChanKhong(string noidung, UnityAction OK)
    {
        GameObject cv = GameObject.FindGameObjectWithTag("CanvasMain");
        if (cv == null)
        {
            Debug.LogWarning("Khong tin thay CanvasMain");
            return;
        }
        ThongBao tb = Instantiate(GameController.Instance.TB.Prefabs_ChacChanKhong, cv.transform) as ThongBao;
        tb.OK.onClick.AddListener(OK);
        tb.ThongTin.text = noidung;
        tb.OK.onClick.AddListener(() => Destroy(tb.gameObject));
        tb.Huy.onClick.AddListener(() => Destroy(tb.gameObject));
    }

    public static void NhacNho(string noidung)
    {
        GameObject cv = GameObject.FindGameObjectWithTag("CanvasMain");
        if (cv == null)
        {
            Debug.LogWarning("Khong tin thay CanvasMain");
            return;
        }
        ThongBao tb = Instantiate(GameController.Instance.TB.Prefabs_NhacNho, cv.transform);
        tb.ThongTin.text = noidung;
        tb.OK.onClick.AddListener(() => Destroy(tb.gameObject));
    }

    public static void Nen(string noidung)
    {
        GameObject cv = GameObject.FindGameObjectWithTag("CanvasMain");
        if (cv == null)
        {
            Debug.LogWarning("Khong tin thay CanvasMain");
            return;
        }
        if (ThongBaoNen.Instance == null)
        {
            ThongBaoNen.Instance = Instantiate(GameController.Instance.TB.Prefabs_Nen, cv.transform);
            ThongBaoNen.Instance.setString(noidung);
        } else
        {
            ThongBaoNen.Instance.addString(noidung);
        }
    }
}
