using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockAction 
{
    List<string> list = new List<string>();
    public UnityAction OnValueChanged;
    /// <summary>
    /// Chỉ được gọi mỗi Frame trong Update
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public bool isOk
    {
        get
        {
            for (int i = list.Count - 1; i > -1; i--)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                    continue;
                }
            }
            return (list.Count == 0);
        }
    }

    public void Register(string m)
    {
        bool a = isOk;
        if (list.Count == 0)
        {
            list.Add(m);
        } else
        {
            for (int i = list.Count - 1; i > -1; i--)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                    continue;
                }
                if (list[i] == m)
                {
                    return;
                }
            }
            list.Add(m);
        }
        if (a != isOk)
        {
            OnValueChanged?.Invoke();
        }
    }


    public void CancelRegistration(string a)
    {
        bool x = isOk;
        for (int i = list.Count - 1; i > -1; i--)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
                continue;
            }
            if (list[i] == a)
            {
                list.RemoveAt(i);
            }
        }
        if (x != isOk)
        {
            OnValueChanged?.Invoke();
        }
    }
}