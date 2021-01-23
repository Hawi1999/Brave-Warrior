using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThongBaoNen : MonoBehaviour
{
    public int TimeRun = 4;
    public int TimeDelay = 2;
    public Slider sli;
    public Text ghi;

    private float timing = 0;
    public List<string> listTBN;
    public static ThongBaoNen Instance = null;
    private bool start = false;
    bool havetext = true;
    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            sli.value = 1;
            return;
        }
        timing += Time.deltaTime;
        sli.value = Mathf.Clamp(1 - timing / TimeRun, 0.02f, 1);
        if (timing > 0.1f && !havetext)
        {
            string str = listTBN[0];
            ghi.text = str;
            listTBN.RemoveAt(0);
            havetext = true;
        }
        if (timing > TimeRun + TimeDelay)
        {
            if (listTBN == null || listTBN.Count == 0)
            {
                Instance = null;
                Destroy(gameObject);
            } else
            {
                ghi.text = "";
                timing = 0;
                havetext = false;
            }
        }
    }

    public void addString(string a)
    {
        if (listTBN == null)
        {
            listTBN = new List<string>();
        }
        listTBN.Add(a);
    }

    public void setString(string a)
    {
        ghi.text = a;
        start = true;
    }
}
