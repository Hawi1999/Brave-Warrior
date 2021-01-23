using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformInstanceOnLoad : MonoBehaviour
{
    public static TransformInstanceOnLoad Instance
    {
        get;set;
    }

    public static Transform getTransform()
    {
        if (Instance == null)
        {
            Instance = Instantiate(new GameObject("TransformInstanceOnLoad")).AddComponent<TransformInstanceOnLoad>();
        }
        return Instance.transform;
    }
}
