using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMain : MonoBehaviour
{
    void Start()
    {
        GameController.CanvasMain = this.gameObject;
    }
}
