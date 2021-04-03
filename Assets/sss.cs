using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class sss 
{
    static int a;
    public UnityAction action;
    public void HH()
    {
        Debug.Log("sss nvoked " + ++a);
    }

    public sss Clone()
    {
        sss a = (sss)MemberwiseClone();
        Debug.Log(a == this);
        return a;
    }
}
