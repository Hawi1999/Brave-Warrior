using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Control : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static Control Instance;
    public string IDCODE;
    public Sprite SpriteDown;
    public Sprite SpriteUp;
    private static List<string> listKey = new List<string>();
    private static List<string> listKeyDown = new List<string>();

    private static List<string> listKeyDownNew = new List<string>();
    private static List<string> listKeyDownOld = new List<string>();

    private static List<string> listKeyUp = new List<string>();

    private static List<string> listKeyUpNew = new List<string>();
    private static List<string> listKeyUpOld = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    Image image => GetComponent<Image>();
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Add(IDCODE);
        if (SpriteDown != null)
            image.sprite = SpriteDown;
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Remove(IDCODE);
        if (SpriteUp != null)
            image.sprite = SpriteUp;
    }

    protected virtual void Start()
    {
        OnWaitToClick += WaitToClick;
        OnEndWaitToClick += EndWaitToClick;
    }
    protected virtual void Update()
    {
        if (Instance == this)
        {
            UpdateKeyDown();
            UpdateKeyUp();
        }
    }
    protected virtual void FixedUpdate()
    {
        
    }
    
    private static void UpdateKeyDown()
    {
        listKeyDown = new List<string>();
        for (int i = 0; i < listKeyDownNew.Count; i++)
        {
            if (!listKeyDownOld.Contains(listKeyDownNew[i]))
            {
                listKeyDown.Add(listKeyDownNew[i]);
            }
        }
        listKeyDownOld.Clear();
        listKeyDownOld.AddRange(listKeyDownNew);
        listKeyDownNew.Clear();
    }

    private static void UpdateKeyUp()
    {
        listKeyUp = new List<string>();
        for (int i = 0; i < listKeyUpNew.Count; i++)
        {
            if (!listKeyUpOld.Contains(listKeyUpNew[i]))
            {
                listKeyUp.Add(listKeyUpNew[i]);
            }
        }
        listKeyUpOld.Clear();
        listKeyUpOld.AddRange(listKeyUpNew);
        listKeyUpNew.Clear();
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

    public static bool GetKeyDown(string a)
    {
        return listKeyDown.Contains(a);
    }

    public static bool GetKetUp(string a)
    {
        return listKeyUp.Contains(a);
    }

    private static void Remove(string a)
    {
        if (listKey.Contains(a))
        {
            listKey.Remove(a);
        }
        if (!listKeyUpNew.Contains(a))
        {
            listKeyUpNew.Add(a);
        }
    }

    private static void Add(string a)
    {
        if (!listKey.Contains(a))
        {
            listKey.Add(a);
        }
        if (!listKeyDownNew.Contains(a))
        {
            listKeyDownNew.Add(a);
        }
    }

    public static UnityAction<string> OnWaitToClick;
    public static UnityAction<string> OnEndWaitToClick;

    public virtual void WaitToClick(string x)
    {
        if (x == IDCODE)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }

    public virtual void EndWaitToClick(string x)
    {
        if (x == IDCODE)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

}
