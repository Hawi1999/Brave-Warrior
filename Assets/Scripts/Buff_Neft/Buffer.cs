using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    public delegate void Buff(ref float percent);

    public static float Buff_DOLA
    {
        get
        {
            float value = 1;
            DOLA_Increase?.Invoke(ref value);
            return value;
        }
    }
    public static float Buff_Dia
    {
        get
        {
            float value = 1;
            Dia_Increase?.Invoke(ref value);
            return value;
        }
    }
    public static float Buff_TL
    {
        get
        {
            float value = 1;
            TL_Increase?.Invoke(ref value);
            return value;
        }
    }
    public static float Neft_DOLA
    {
        get
        {
            float value = 1;
            DOLA_Decrease?.Invoke(ref value);
            return value;
        }
    }
    public static float Neft_Dia { 
        get
        {
            float value = 1;
            Dia_Decrease?.Invoke(ref value);
            return value;
        }
    }
    public static float Neft_TL
    {
        get
        {
            float value = 1;
            TL_Decrease?.Invoke(ref value);
            return value;
        }
    }




    public static Buff DOLA_Increase;
    public static Buff Dia_Increase;
    public static Buff TL_Increase;
    public static Buff DOLA_Decrease;
    public static Buff Dia_Decrease;
    public static Buff TL_Decrease;
}
