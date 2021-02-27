using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseMinapulation : MonoBehaviour
{
    public static ChooseMinapulation Instance;
    [HideInInspector] public List<IManipulation> manipulations;
    private IManipulation manipulation_current;
    private PlayerController player => PlayerController.PlayerCurrent;

    private float lastChoose;
    void Awake()
    {
        Instance = this;
    }
    public IManipulation Choosing
    {
        get
        {
            return manipulation_current;
        }
        set
        {
            manipulation_current = value;
            OnChooseMinapulation?.Invoke(value);
        }
    }
    private void Update()
    {
        if (Control.GetKeyDown("X") && Time.time - lastChoose > 0.5f)
        {
            if (Choosing != null)
            {
                Choosing.TakeManipulation(player);
                lastChoose = Time.time;
            }
        }
    }

    public void Add(IManipulation manipulation)
    {
        if (manipulations == null)
        {
            manipulations = new List<IManipulation>();
        }
        if (!isExist(manipulation))
        {
            manipulations.Add(manipulation);
            OnChooseMinapulation += manipulation.OnChoose;
            Control.OnWaitToClick?.Invoke("X");
        }
        Choosing = manipulation;
    }

    public void Remove(IManipulation manipulation)
    {
        if (manipulations != null && isExist(manipulation))
        {
            manipulations.Remove(manipulation);
            if (manipulations.Count == 0)
            {
                Choosing = null;
            } else
            {
                Choosing = manipulations[manipulations.Count - 1];
            }
            OnChooseMinapulation -= manipulation.OnChoose;
        if (manipulations.Count == 0)
            Control.OnEndWaitToClick?.Invoke("X");
        }
    }

    private bool isExist(IManipulation manipulation)
    {
        foreach (IManipulation manipulation1 in manipulations)
        {
            if (manipulation == manipulation1)
            {
                return true;
            }
        }
        return false;
    }

    UnityAction<IManipulation> OnChooseMinapulation;
}
