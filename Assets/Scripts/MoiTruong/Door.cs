using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public virtual void Open()
    {
        gameObject.SetActive(false);
    }

    public virtual void Close()
    {
        gameObject.SetActive(true);
    }
}
