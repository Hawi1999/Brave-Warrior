using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public Button a;
    // Start is called before the first frame update
    void Start()
    {
        a.onClick.AddListener(GameController.ClearAllSave );
;    }

}
