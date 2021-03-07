using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Languages : MonoBehaviour
{
    private static Nation _LanguageCurrent;
    
    public static Nation LanguageCurrent
    {
        get
        {
            return _LanguageCurrent;
        }
        set
        {
            _LanguageCurrent = value;
            UpdateLanguageInScene();
        }
    }

    public static void UpdateLanguageInScene()
    {
        var updateLanguages = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IUpdateLanguage>();
        foreach (IUpdateLanguage i in updateLanguages)
        {
            i.OnUpdateLanguage();
        }
    }

    private static Lang[] MutiLang;
    public static void LoadData()
    {
         MutiLang = Resources.LoadAll<Lang>("Language");
    }
    public enum Nation
    {
        VietNam = 0,
        EngLish = 1
    }

    public static string getString(string CODE)
    {
        Lang lang = null;
        if (MutiLang != null)
        {
            lang = Array.Find(MutiLang, e => e.CODE == CODE);
        }
        return getString(lang, LanguageCurrent);
    }

    public static string getString(string CODE, bool isMany)
    {
        string a = getString(CODE);
        if (isMany && LanguageCurrent == Nation.EngLish)
        {
            if (a.Length >= 2)
            {
                if (a[a.Length - 1] == 's' || 
                    a[a.Length - 1] == 'x' || 
                    a[a.Length - 1] == 'z' || 
                    a[a.Length - 2] == 'c' && a[a.Length - 1] == 'h' ||
                    a[a.Length - 2] == 's' && a[a.Length - 1] == 'h')
                {
                    return (a + "es");
                }
                else
                {
                    return (a + "s");
                }
            } else
            {
                return (a + "s");
            }
        }
        return a;
    }

    private static string getString(Lang lang, Nation nation)
    {
        if (lang == null)
        {
            return string.Empty;
        }
        switch (nation)
        {
            case Nation.VietNam:
                return lang.VietNam;
            case Nation.EngLish:
                return lang.English;
            default:
                return string.Empty;
        }
    }


}
