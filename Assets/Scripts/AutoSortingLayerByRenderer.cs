using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoSortingLayerByRenderer : MonoBehaviour
{
    public int Offset;
    Renderer myrenderer;
    void Start()
    {
        myrenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myrenderer.sortingOrder = (int)((- transform.position.y)*10 - Offset);
    }
}
