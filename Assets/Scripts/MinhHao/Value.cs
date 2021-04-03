using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValueInt
{
    int value;
    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            int a = this.value;
            this.value = value;
            if (a != value)
            {
                OnValueChanged?.Invoke();
            }
        }
    }
    public UnityAction OnValueChanged;
}
public class ValueFloat
{
    float value;
    public float Value
    {
        get
        {
            return value;
        }
        set
        {
            float a = this.value;
            this.value = value;
            if (a != value)
            {
                OnValueChanged?.Invoke();
            }
        }
    }
    public UnityAction OnValueChanged;

    public ValueFloat(float a)
    {
        Value = a;
    }
}
public class ValueString
{
    string value;
    public string Value
    {
        get
        {
            return value;
        }
        set
        {
            string a = this.value;
            this.value = value;
            if (a != value)
            {
                OnValueChanged?.Invoke();
            }
        }
    }
    public UnityAction OnValueChanged;

}
public class ValueBool
{
    bool value;
    public bool Value
    {
        get
        {
            return value;
        }
        set
        {
            bool a = this.value;
            this.value = value;
            if (a != value)
            {
                OnValueChanged?.Invoke();
            }
        }
    }
    public UnityAction OnValueChanged;
    public ValueBool(bool a)
    {
        Value = a;
    }



}
