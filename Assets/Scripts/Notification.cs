using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public Button OK;
    public Text ThongTin;
    public Button Huy;

    public static void AreYouSure(string content, UnityAction OK)
    {
        GameObject cv = GameObject.FindGameObjectWithTag("CanvasMain");
        if (cv == null)
        {
            Debug.LogWarning("Khong tin thay CanvasMain");
            return;
        }
        Notification tb = Instantiate(GameController.Instance.TB.Prefabs_ChacChanKhong, cv.transform) as Notification;
        tb.OK.onClick.AddListener(OK);
        tb.ThongTin.text = content;
        tb.OK.onClick.AddListener(() => Destroy(tb.gameObject));
        tb.Huy.onClick.AddListener(() => Destroy(tb.gameObject));
    }
    public static void ReMind(string content)
    {
        GameObject cv = GameObject.FindGameObjectWithTag("CanvasMain");
        if (cv == null)
        {
            Debug.LogWarning("Khong tin thay CanvasMain");
            return;
        }
        Notification tb = Instantiate(GameController.Instance.TB.Prefabs_NhacNho, cv.transform);
        tb.ThongTin.text = content;
        tb.OK.onClick.AddListener(() => Destroy(tb.gameObject));
    }
    public static void NoticeBelow(string content)
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
            ThongBaoNen.Instance.setString(content);
        } else
        {
            ThongBaoNen.Instance.addString(content);
        }
    }
}
