using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Control : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string IDCODE;
    public Sprite SpriteDown;
    public Sprite SpriteUp;
    private static List<string> listKey = new List<string>();

    Image image => GetComponent<Image>();
    public void OnPointerDown(PointerEventData eventData)
    {
        Add(IDCODE);
        image.sprite = SpriteDown;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Remove(IDCODE);
        image.sprite = SpriteUp;
    }

    private void Start()
    {
        OnWaitToClick += WaitToClick;
        OnEndWaitToClick += EndWaitToClick;
    }

    private void OnDestroy()
    {
        Remove(IDCODE);
        OnWaitToClick -= WaitToClick;
        OnEndWaitToClick -= EndWaitToClick;
    }



    public static bool GetKey(string a)
    {
        return listKey.Contains(a);
    }

    private static void Remove(string a)
    {
        if (listKey.Contains(a))
        {
            listKey.Remove(a);
        } 
    }

    private static void Add(string a)
    {
        if (!listKey.Contains(a))
        {
            listKey.Add(a);
        }else
        {
            Debug.Log("Đã tồn tại Code " + a);
        }
    }

    public static UnityAction<string> OnWaitToClick;
    public static UnityAction<string> OnEndWaitToClick;

    public void WaitToClick(string x)
    {
        if (x == IDCODE)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }

    public void EndWaitToClick(string x)
    {
        if (x == IDCODE)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

}
