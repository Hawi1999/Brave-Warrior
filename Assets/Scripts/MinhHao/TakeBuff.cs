using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakeBuff
{
    #region Value CallBack Action
    public static int AMOUNT_BUFF2DATA_REGISTED = 1;
    public static int AMOUNT_GIVEBUFF_REGISTED = 2;
    #endregion
    public List<BuffRegister> buffs = new List<BuffRegister>();
    private List<GiveBuff> giveBuffs = new List<GiveBuff>();

    public void Register(GiveBuff give, BuffRegister.TypeBuff typeBuff, float a)
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
            OnValueChanged?.Invoke(AMOUNT_BUFF2DATA_REGISTED);
        }
        Debug.Log("Haha");
        OnValueChanged?.Invoke(AMOUNT_GIVEBUFF_REGISTED);
    }
    public void RemoveRegister(GiveBuff give)
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
            OnValueChanged?.Invoke(AMOUNT_BUFF2DATA_REGISTED);
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
    public float GetValue(BuffRegister.TypeBuff type)
    {
        if (TryFind(type, out BuffRegister b))
        {
            return b.GetValue;
        }
        else
            return 0;
    }
    private bool TryFind(BuffRegister.TypeBuff Type, out BuffRegister b)
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
    public List<T> GetALlGiveBuff<T>() where T : GiveBuff
    {
        List<T> t = new List<T>();
        foreach (GiveBuff g in giveBuffs)
        {
            if (g is T)
            {
                t.Add(g as T);
            }
        }
        return t;
    }
    public void InvokeOnBuffRegisterValueChanged(BuffRegister.TypeBuff Type, ChangesValue changes)
    {
        OnBuffRegisterValueChanged?.Invoke(Type, changes);
    }
    public UnityAction<int> OnValueChanged;
    public UnityAction<BuffRegister.TypeBuff, ChangesValue> OnBuffRegisterValueChanged;

}
