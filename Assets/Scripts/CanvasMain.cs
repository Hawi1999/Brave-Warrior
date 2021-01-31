using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMain : MonoBehaviour
{
    void Awake()
    {
        GameController.CanvasMain = this.gameObject;
    }
}
