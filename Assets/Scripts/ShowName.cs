using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowName : MonoBehaviour
{
    public Sprite Name;
    public Material nameSkin;
    public Vector3 LocalPositionMT;
    public Vector3 LocalPositionName;

    private GameObject Info;
    private SpriteRenderer renderName;
    private SpriteRenderer renderMT;
    private IShowName ishowname => GetComponent<IShowName>();


    private Vector3 thongtinName;
    private Vector3 thongtinMT;
    private Vector3 SizeMT = new Vector3(0.25f, 0.13f, 0);
    void Awake()
    {
        SetUp();
    }

    private void Start()
    {
        renderName.color = ishowname.GetColorName();
    }

    public void Show()
    {
        if (Info != null)
            Info.SetActive(true);
    }

    public void Hide()
    {
        if (Info != null)
            Info.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        SetPositionInfo();
        Gizmos.color = Color.red;

        if (renderMT == null)
        {
            Gizmos.DrawCube(thongtinMT + new Vector3(0, SizeMT.y/2), SizeMT);
        }
        if (renderName == null)
        {
            Gizmos.DrawLine(thongtinName - Vector3.left, thongtinName + Vector3.left);
        }
    }

    private void SetPositionInfo()
    {
        thongtinMT = transform.position + LocalPositionMT - new Vector3(0, 0.07f , 0);
        thongtinName = thongtinMT + LocalPositionName;
    }
    private void SetUp()
    {
        if (Info == null)
        {
            Info = new GameObject("infomation");
            Info.transform.parent = transform;
        }
        if (renderMT == null)
        {
            renderMT = new GameObject("renderMT").AddComponent<SpriteRenderer>();
            renderMT.transform.parent = Info.transform;
        }
        if (renderName == null)
        {
            renderName = new GameObject("renderName").AddComponent<SpriteRenderer>();
            renderName.transform.parent = Info.transform;
        }
        if (nameSkin != null)
        {
            renderName.material = nameSkin;
        }
        renderName.sprite = Name;
        renderMT.sprite = Resources.Load<Sprite>("Image/MT");
        SetPositionInfo();
        renderName.transform.position = thongtinName;
        renderMT.transform.position = thongtinMT;
        renderName.sortingLayerName = "Effect";
        renderName.sortingOrder = 15;

        renderMT.sortingLayerName = "Effect";
        renderMT.sortingOrder = 15;
    }


}

