using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class test : MonoBehaviour
{
    int n = 0;
    int sum = 0;
    private void Start()
    {
        // Cau 10: (6C3 + 2x4C3)/(14C3)
        // Cau 11: 1 - (8C3/14C3)
        // Cau 12: 

        StartCoroutine(ukss());
    }
    IEnumerator ukss()
    {
        int k = 0;
        while (true)
        {
            List<int> l1 = new List<int>();
            List<int> l2 = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                l1.Add(1);
            }
            for (int i = 0; i < 6; i++)
            {
                l1.Add(2);
            }
            for (int i = 0; i < 5; i++)
            {
                l2.Add(1);
            }
            for (int i = 0; i < 5; i++)
            {
                l2.Add(2);
            }
            int a = l1[Random.Range(0, l1.Count)];
            int b = l2[Random.Range(0, l2.Count)];
            if (a == 1 && b == 2)
            {
                n++;
            }
            if (a != b && (a == 1 || b == 1))
                sum++;
            if (++k >= 10000)
            {
                Debug.Log("Thử " + sum + " lần: " + n * 1f / sum);
                k = 0;
                yield return null;
            }
        }
    }

    IEnumerator uks()
    {
        int k = 0;
        while (true)
        {
            int so = 0;
            for (int i = 0; i < 20; i++)
            {
                float a = Random.Range(0, 1f);
                if (a < 0.15f)
                {
                    so++;
                }
            }
            if (so == 3)
            {
                n++;
            }
            sum++;
            if (++k >= 10000)
            {
                Debug.Log(n * 1f / sum);
                k = 0;
                yield return null;
            }
        }
    }

    IEnumerator uk()
    {
        int k = 0;
        while (true)
        {
            List<int> i = new List<int>();
            for (int s = 0; s < 6; s++)
            {
                i.Add(1);
            }
            for (int s = 0; s < 4; s++)
            {
                i.Add(2);
            }
            for (int s = 0; s < 4; s++)
            {
                i.Add(3);
            }
            int a = i[Random.Range(0, i.Count)];
            i.Remove(a);
            int b = i[Random.Range(0, i.Count)];
            i.Remove(b);
            int c = i[Random.Range(0, i.Count)];
            if (a == 1 && b == 1 && c == 1)
            {
                n++;
            }
            sum++;
            if (++k >= 10000)
            {
                Debug.Log(n * 1f / sum);
                k = 0;
                yield return null;
            }
        }
    }
    
}