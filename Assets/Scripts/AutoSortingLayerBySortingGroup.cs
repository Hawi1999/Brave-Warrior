using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(SortingGroup))]
public class AutoSortingLayerBySortingGroup : MonoBehaviour
{
    public int Offset;
    public bool isStatic;
    SortingGroup sortingGroup;
    void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        sortingGroup.sortingOrder = (int)((- transform.position.y) * 10 - Offset);
    }

    
}
