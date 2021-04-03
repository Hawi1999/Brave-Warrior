using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public struct ChangesValue{
    float oldValue;
    float newValue;

    public ChangesValue(float a, float b)
    {
        oldValue = a;
        newValue = b;
    }
}

public class BuffRegister
{
    TypeBuff type;
    private List<ValueBuff> froms = new List<ValueBuff>();
    public TypeBuff Type => type;
    public List<ValueBuff> Froms => froms;
    public class ValueBuff
    {
        public float value = 0;
        // Tên của trang bị đang buff cho đối tợng
        public GiveBuff Giver;

        public ValueBuff(GiveBuff name, float value)
        {
            this.Giver = name;
            this.value = value;
        }
    }
    public enum TypeBuff
    {
        None,
        IncreaseDamageByPercent,
        IncreaseDamageByValue,
        IncreaseMaxHealthByPercent,
        IncreaseMaxHealthByValue,
        IncreaseMaxShieldByPercent,
        IncreaseMaxShieldByValue,
        IncreaseSizeByPercent,
        IncreaseDamageCritByPercent,
    }
    public float GetValue
    {
        get
        {
            float sum = 0;
            for (int i = froms.Count - 1; i > -1; i--)
            {
                sum += froms[i].value;
            }
            return sum;
        }
    }
    private ValueBuff Find(GiveBuff host)
    {
        for (int i = froms.Count - 1; i > -1; i--)
        {
            if (froms[i].Giver.Equals(host))
            {
                return froms[i];
            }
        }
        return null;
    }
    public BuffRegister(TypeBuff type)
    {
        this.type = type;
    }
    public void Register(GiveBuff give, float value)
    {
        ValueBuff vlb = Find(give);
        float oldValue = GetValue;
        if (vlb == null)
        {
            froms.Add(new ValueBuff(give, value));
        } else
        {
            Debug.Log("Da ton tai give");
            vlb.value = value;
        }
        float newValue = GetValue;
        OnValueChanged?.Invoke(Type, new ChangesValue(oldValue, newValue));
    }
    public void Remove(GiveBuff f, out bool clear)
    {
        ValueBuff ee = Find(f);
        float oldValue = GetValue;
        if (ee != null)
        {
            froms.Remove(ee);
        }
        clear = froms.Count == 0;
        float newValue;
        if (clear)
        {
            newValue = 0;
        } else
        {
            newValue = GetValue;
        }
        OnValueChanged?.Invoke(Type, new ChangesValue(oldValue, newValue));
    }

    public UnityAction<TypeBuff, ChangesValue> OnValueChanged;
}
