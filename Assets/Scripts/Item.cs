﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public Sprite Pic;
    public string Name;
    public string CODE;
    public TypeSlot TypeSlot;

    public virtual int getGiaBan()
    {
        return 0;
    }

    public virtual int getGiaMua()
    {
        return 0;
    }

    public virtual int getThoiGianTrong()
    {
        return 0;
    }
}