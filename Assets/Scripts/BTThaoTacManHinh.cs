using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]

public class BTThaoTacManHinh : MonoBehaviour
{
    public List<GameObject> TuDuoiLenTren;

    private GameObject[] child = { null, null, null, null };
    private bool CheckButton(int id)
    {
        return child[id] != null;
    }

    public Button getButton(int id)
    {
        return child[id].GetComponent<Button>();
    }

    public void DeleteButton(int id)
    {
        if (CheckButton(id))
        {
            Destroy(child[id]);
            child[id] = null;
        }
    }
    public void AddButton(int id)
    {
        DeleteButton(id);
        GameObject a = Instantiate(TuDuoiLenTren[id], gameObject.transform);
        child[id] = a;
    }
    public void AddButton(int id, string Text)
    {
        AddButton(id);
        ChangeText(id, Text);
    }

    public void ChangeText(int id, string name)
    {
        if (child[id] == null)
            return;
        child[id].GetComponentInChildren<Text>().text = name;
    }
    void DeleteAll()
    {
        while (gameObject.transform.childCount != 0)
        {
            Destroy(gameObject.transform.GetChild(0));
        }
    }
    public void SetListener(int id, UnityAction a)
    {
        if (child[id] != null)
        {
            child[id].GetComponent<Button>().onClick.RemoveAllListeners();
            child[id].GetComponent<Button>().onClick.AddListener(() => a());
        }
        
    }

    public void AddListener(int id, UnityAction a)
    {
        if (child[id] != null)
            child[id].GetComponent<Button>().onClick.AddListener(() => a());
    }

    public void DeleteListener(int id, UnityAction a)
    {
        if (child[id] != null)
            child[id].GetComponent<Button>().onClick.RemoveListener(() => a());
    }

    public void DeleteAllListener(int id)
    {
        if (child[id] != null)
            child[id].GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void AddButton(int id, string Text, UnityAction action)
    {
        AddButton(id);
        ChangeText(id, Text);
        AddListener(id, action);
    }
}