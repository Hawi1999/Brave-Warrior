using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentGamePlay : MonoBehaviour
{
    public static Transform Instance;

    private void Awake()
    {
        Instance = transform;
        DontDestroyOnLoad(this.gameObject);
    }
}
