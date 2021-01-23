using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesTroyThis : MonoBehaviour
{
    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void SetActiveFalseThis()
    {
        gameObject.SetActive(false);
    }
}
