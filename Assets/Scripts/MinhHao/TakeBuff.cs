using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakeBuff
{
    public static int AMOUNT_GIVEBUFF_REGISTED = 1;

    #region From GiveBuff
    public List<BuffRegister> buffs = new List<BuffRegister>();
    private List<IGiveBuff> giveBuffs = new List<IGiveBuff>();
    public void Register(IGiveBuff give, int typeBuff, float a)
    {
        if(TryFind(typeBuff, out BuffRegister b))
        {
            b.Register(give, a);
        } else
        {
            b = new BuffRegister(typeBuff);
            b.OnValueChanged += InvokeOnBuffRegisterValueChanged;
            buffs.Add(b);
            b.Register(give, a);
        }
        if (!giveBuffs.Contains(give))
        {
            giveBuffs.Add(give);
        }
        OnValueChanged?.Invoke(AMOUNT_GIVEBUFF_REGISTED);
    }

    public void RemoveRegister(IGiveBuff give)
    {
        for (int i = buffs.Count; i > -1; i--)
        {
            BuffRegister b = buffs[i];
            b.Remove(give,out bool c);
            if (c)
            {
                b.OnValueChanged -= InvokeOnBuffRegisterValueChanged;
                buffs.RemoveAt(i);
            }
        }
        if (giveBuffs.Contains(give))
        {
            giveBuffs.Remove(give);
        }
        OnValueChanged?.Invoke(AMOUNT_GIVEBUFF_REGISTED);
    }
    public void Reset()
    {
        for (int i = buffs.Count - 1; i > -1; i--)
        {
            buffs.RemoveAt(i);
        }
    }
    public float GetValue(int type)
    {
        if (TryFind(type, out BuffRegister b))
        {
            return b.GetValue;
        }
        else
            return 0;
    }
    private bool TryFind(int Type, out BuffRegister b)
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].Type == Type)
            {
                b = buffs[i];
                return true;
            }
        }
        b = null;
        return false;
    }

    public bool ExitBuff(int Type)
    {

        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].Type == Type)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    public List<IShowBuffA> GetALlBuffA() 
    {
        List<IShowBuffA> t = new List<IShowBuffA>();
        foreach (IGiveBuff g in giveBuffs)
        {
            if (g is IShowBuffA)
            {
                t.Add(g as IShowBuffA);
            }
        }
        return t;
    }
    public void InvokeOnBuffRegisterValueChanged(int Type, ChangesValue changes)
    {
        OnBuffRegisterValueChanged?.Invoke(Type, changes);
    }
    public UnityAction<int> OnValueChanged;
    public UnityAction<int, ChangesValue> OnBuffRegisterValueChanged;


}
