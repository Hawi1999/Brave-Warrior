using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowName : MonoBehaviour
{
    public Sprite Name;
    public Sprite MT;
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
        if (renderMT == null || MT == null)
        {
            Gizmos.DrawCube(thongtinMT + new Vector3(0, SizeMT.y/2), SizeMT);
        }
        if (renderName == null || Name == null)
        {
            Gizmos.DrawLine(thongtinName - Vector3.left, thongtinName + Vector3.left);
        }
    }

    private void SetPositionInfo()
    {
        thongtinMT = transform.position + LocalPositionMT - new Vector3(0, 0.07f , 0);
        thongtinName = thongtinMT + LocalPositionName;
    }
    private void OnValidate()
    {

    }
    private void SetUp()
    {
        if (Info == null)
        {
            Info = Instantiate(new GameObject("infomation"), transform);
        }
        if (renderMT == null)
        {
            renderMT = Instantiate(new GameObject("renderMT"), Info.transform).AddComponent<SpriteRenderer>();
        }
        if (renderName == null)
        {
            renderName = Instantiate(new GameObject("renderName"), Info.transform).AddComponent<SpriteRenderer>();
        }
        renderName.sprite = Name;
        renderMT.sprite = MT;
        SetPositionInfo();
        renderName.transform.position = thongtinName;
        renderMT.transform.position = thongtinMT;
        renderName.sortingLayerName = "Skin";
        renderName.sortingOrder = 15;
        renderName.color = ishowname.GetColorName();

        renderMT.sortingLayerName = "Skin";
        renderMT.sortingOrder = 15;
    }


}

