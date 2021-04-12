using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Line
{
    public float Diem = 0;
    public LineRenderer line
    {
        get;
    }
    public Vector3 position
    {   
        set
        {
            line.transform.position = value;
        }
    }

    private int dem;
    public int Dem
    {
        get
        {
            return dem;
        }
        set
        {
            dem = value;
            Vector3[] a = new Vector3[2];
            a[0] = positionStart;
            a[1] = positionStart + new Vector3(0, ((float)dem) / 10, 0);
            line.SetPositions(a);
        }
    }

    private Vector3 positionStart;

    public Line(Vector3 StartPosition, float Size)
    {
        line = new GameObject("Hihi").AddComponent<LineRenderer>();
        Vector3[] a = new Vector3[2];
        positionStart = StartPosition;
        a[0] = positionStart;
        a[1] = positionStart;
        line.SetPositions(a);
        line.SetPosition(0, Vector3.zero);
        line.SetWidth(Size, Size);
        line.transform.position = StartPosition;
    }

}

public class Nothinh : MonoBehaviour
{
    public float Size = 1;
    public float Distance = 1.5f;
    List<Line> l = new List<Line>();

    private void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            Vector3 pos = new Vector3(Distance * i, 0, 0);
            Line k = new Line(pos, Size);
            k.Diem = i;
            l.Add(k);
        }
        StartCoroutine(Solve());
    }

    IEnumerator Solve()
    {
        int sotran = 0;
        while (true)
        {
            sotran++;
            int dung = 0;
            for (int i = 0; i < 50; i++)
            {
                if (UnityEngine.Random.Range(0, 4) == 0)
                {
                    dung++;
                }
            }
            Array.Find(l.ToArray(), e => e.Diem == dung).Dem++;
            Debug.Log("So Tran da danh: " + sotran);
            yield return null;

        }
    }



    #region BAITAP MAN
    /*
    int max;
    bool[] b;
    public int[] a;
    int[] mu;
    private void Start()
    {
        max = a.Length;
        SetUp();
        for (int i = 0; i < mu[max]; i++)
        {
            MaHoa(i);
            Show();
        }
    }

    private void Show()
    {
        string s = string.Empty;
        for (int i = 0; i <= max; i++)
        {
            if (b[i])
            {
                s += a[i] + " ";
            }
        }
        Debug.Log(s);
    }

    private void SetUp()
    {
        mu = new int[max + 1];
        mu[0] = 1;
        for (int i = 1; i <= max; i++)
        {
            mu[i] = mu[i-1] * 2;
        }
    }

    private void MaHoa(int a)
    {
        b = new bool[max + 1];
        for (int i = max ; i >= 0; i--)
        {
            if (a >= mu[i])
            {
                b[i] = true;
                a -= mu[i];
            }
            else
            {
                b[i] = false;
            }
        }
    }
    */
    #endregion
}
