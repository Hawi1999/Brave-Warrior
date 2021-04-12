using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class sss : ScriptableObject
{
    static int a;
    public UnityAction action;
    public ValueInt intv = new ValueInt();
    public void HH()
    {
        Debug.Log("sss nvoked " + ++a);
    }

    public sss Clone()
    {
        sss a = Instantiate(this);
        Debug.Log("Action goc: " + (action.GetInvocationList().Length));
        Debug.Log("Action coppy: " + (action.GetInvocationList().Length));
        if (action != null)
            a.action = (UnityAction)action.Clone();
        a.action?.Invoke();
        return a;
    }
}
