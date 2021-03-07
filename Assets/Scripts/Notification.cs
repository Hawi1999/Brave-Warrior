using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Notification : MonoBehaviour, IUpdateLanguage
{
    public Button OK;
    public Text ThongTin;
    public Button Huy;

    private void Start()
    {
        OnUpdateLanguage();
    }

    public static void AreYouSure(string content, UnityAction OK)
    {
        GameObject cv = GameObject.FindGameObjectWithTag("CanvasMain");
        if (cv == null)
        {
            Debug.LogWarning("Khong tin thay CanvasMain");
            return;
        }
        Notification tb = Instantiate(Resources.Load<Notification>("Canvas/ThongBaoChacChanKhong"), cv.transform);
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
        Notification tb = Instantiate(Resources.Load<Notification>("Canvas/ThongBaoNhacNho"), cv.transform);
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
        if (NoticeBackground.Instance == null)
        {
            NoticeBackground.Instance = Instantiate(Resources.Load<NoticeBackground>("Canvas/ThongBaoNen"), cv.transform);
            NoticeBackground.Instance.setString(content);
        } else
        {
            NoticeBackground.Instance.addString(content);
        }
    }

    public void OnUpdateLanguage()
    {
        if (Huy != null)
            Huy.GetComponentInChildren<Text>().text = Languages.getString("Huy");
    }
}
