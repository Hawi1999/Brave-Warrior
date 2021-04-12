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
    int type;
    private List<ValueBuff> froms = new List<ValueBuff>();
    public int Type => type;
    public List<ValueBuff> Froms => froms;
    public class ValueBuff
    {
        public float value = 0;
        // Tên của trang bị đang buff cho đối tợng
        public IGiveBuff Giver;

        public ValueBuff(IGiveBuff name, float value)
        {
            this.Giver = name;
            this.value = value;
        }
    }

    public struct TypeBuff
    {
        public const int None = 0;
        public const int IncreaseDamageByPercent = 1;
        public const int IncreaseDamageByValue = 2;
        public const int IncreaseMaxHealthByPercent = 3;
        public const int IncreaseMaxHealthByValue = 4;
        public const int IncreaseMaxShieldByPercent = 5;
        public const int IncreaseMaxShieldByValue= 6;
        public const int IncreaseSizeByPercent = 7;
        public const int IncreaseDamageCritByPercent = 8;
        public const int IncreaseDamageLegacyBuff1ByValue = 9;
        public const int IncreaseCoolDownSkillByFix100 = 10;
        public const int DecreaseCoolDownSkillByFix100 = 11;
        public const int IncreaseRatioTakeHealthFromEnemyDied = 12;

        public const int Max = 12;

        public static string GetStringCode(int a)
        {
            switch (a)
            {
                case 1 : return "IncreaseDamageByPercent";
                case 2 : return "IncreaseDamageByValue";
                case 3 : return "IncreaseMaxHealthByPercent";
                case 4 : return "IncreaseMaxHealthByValue";
                case 5 : return "IncreaseMaxShieldByPercent";
                case 6 : return "IncreaseMaxShieldByValue";
                case 7 : return "IncreaseSizeByPercent";
                case 8 : return "IncreaseDamageCritByPercent";
                case 9 : return "IncreaseDamageLegacyBuff1ByValue";
                case 10 : return "IncreaseCoolDownSkillByFix100";
                case 11 : return "DecreaseCoolDownSkillByFix100";
                case 12 : return "IncreaseRatioTakeHealthFromEnemyDied";
                default: return "None";
            }
        }

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
    private ValueBuff Find(IGiveBuff host)
    {
        for (int i = froms.Count - 1; i > -1; i--)
        {
            if (froms[i].Giver == null || froms[i].Giver as UnityEngine.Object == null)
                continue;
            if (froms[i].Giver.Equals(host))
            {
                return froms[i];
            }
        }
        return null;
    }
    public BuffRegister(int type)
    {
        this.type = type;
    }
    public void Register(IGiveBuff give, float value)
    {
        ValueBuff vlb = null;
        if (give != null && give as UnityEngine.Object != null)
        {
            vlb = Find(give);
        }
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
    public void Remove(IGiveBuff f, out bool clear)
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

    public UnityAction<int, ChangesValue> OnValueChanged;
}
