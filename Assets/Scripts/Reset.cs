using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public Button a;
    public Text b;
    // Start is called before the first frame update
    void Start()
    {
        a.onClick.AddListener(GameController.ClearAllSave );
        b = GetComponentInChildren<Text>();
;    }

    private void Update()
    {
        b.text = (Mathf.CeilToInt(1 / Time.deltaTime)).ToString();
    }

}
