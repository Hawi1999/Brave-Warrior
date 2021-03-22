using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    [SerializeField] Vector3 RandomRange;
    [SerializeField] Text T;
    [SerializeField] float TimeToDestroy;
    [SerializeField] float TocDoBay;
    [SerializeField] float sizeDefault = 40;

    private Vector3 localTarget = new Vector3(0,1,0);

    private float t = 0;
    private float timeStart;
    private float maxSizeText;

    private void Start()
    {
        maxSizeText = sizeDefault;
        Vector3 crposition = transform.localPosition;
        Vector3 newposition = new Vector3(crposition.x + Random.Range(-RandomRange.x, RandomRange.x), crposition.y + Random.Range(-RandomRange.y, RandomRange.y), crposition.z);
        transform.localPosition = newposition;
        localTarget = newposition + new Vector3(0, 1, 0);
        timeStart = Time.time;
    }
    void UpdatePosition()
    {       
        Vector3 pos = transform.localPosition;
        Vector3 newPos = Vector3.Lerp(pos, localTarget, TocDoBay);
        transform.localPosition = newPos;
    }

    void UpdateTextSize()
    {
        int size = (int)Mathf.Clamp((Time.time - timeStart) * maxSizeText / 0.2f, 0, maxSizeText);
        T.fontSize = size;
    }


    private void Update()
    {
        UpdatePosition();
        UpdateTextSize();
        t += Time.deltaTime;
        if (t > TimeToDestroy) Destroy(this.gameObject);
    }

    public void SetText(string text)
    {
        T.text = text;
    }

    public static string StringColor(string text, string color)
    {
        return StartColor(color) + text + EndColor();
    }
    public static string StartColor(string colorName)
    {
        return "<color=" + colorName + ">";
    }

    public static string EndColor()
    {
        return "</color>";
    }

}

