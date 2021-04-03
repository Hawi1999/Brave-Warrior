using UnityEngine;
using UnityEngine.Events;

public class test : MonoBehaviour
{
    private void Start()
    {
        sss a = new sss();
        a.action += HH;
        sss b = a.Clone();
        a.action?.Invoke();
        b.action?.Invoke();

    }

    static int a;
    public UnityAction action;

    public void HH()
    {
        Debug.Log("Test Inoved " + ++a);
    }
}