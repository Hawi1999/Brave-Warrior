using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingText : MonoBehaviour
{
    public Text text;
    public float timeDelay;
    string[] textLoading;
    int id;

    private float timeSum = 0;
    // Start is called before the first frame update
    void Start()
    {
        id = 0;
        textLoading = new string[4];
        textLoading[0] = "Đang tải bản đồ.";
        textLoading[1] = "Đang tải bản đồ..";
        textLoading[2] = "Đang tải bản đồ...";
        textLoading[3] = "Đang tải bản đồ....";
    }

    // Update is called once per frame
    void Update()
    {
        timeSum += Time.deltaTime;
        if (timeSum >= timeDelay)
        {
            id = (id + 1) % textLoading.Length;
            timeSum = 0;
        }
        text.text = textLoading[id];
    }
}
