using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "R1", menuName = "Data/RoundData")]
public class RoundData : ScriptableObject
{
    public TypeRound typeRound;
    [HideInInspector]
    public Vector2Int position;
    [Header("Round Enemy")]
    [Space]
    public GroupEnemy groupEnemy = GroupEnemy.One;
    public Waves Waves;
    [Header("Round Chets")]
    [Space]
    public TypeChest typeChest;
    public ColorChest colorChest;
    [Header("Round Begin")]
    [Space]
    public CodeMap codeMap;
    private List<Direct> directs;
    private Texture2D _textMap;
    public Texture2D textMap
    {
        get
        {
            return _textMap;
        }
        set
        {
            _textMap = value;
            if (_textMap != null)
                hSize = new Vector2Int(textMap.width / 2, textMap.height / 2);
            else
                hSize = Vector2Int.zero;
        }
    }
    private Vector2Int hSize;

    public void ResetValue()
    {
        textMap = null;
        directs = null;
    }

    public void AddDirect(Direct d)
    {
        if (directs == null)
        {
            directs = new List<Direct>();
        }
        directs.Add(d);
    }

    public List<Direct> GetDirects()
    {
        return directs;
    }
    public void RemoveDirect(Direct d)
    {
        if (directs == null)
        {
            return;
        }
        directs.Remove(d);
    }


    private void OnEnable()
    {
        ResetValue();
    }
    public Vector2Int Size => hSize * 2;

    public Vector2Int GetPosition(Direct t)
    {
        switch (t)
        {
            case Direct.Center:
                return position;
            case Direct.Left:
                return position - new Vector2Int(hSize.x + 1, 0);
            case Direct.Right:
                return position + new Vector2Int(hSize.x + 1, 0);
            case Direct.Up:
                return position + new Vector2Int(0, hSize.y + 1);
            case Direct.Down:
                return position - new Vector2Int(0, hSize.y + 1);
        }
        return position;
    }

    public void GetCellLimit(out Vector2Int MIN, out Vector2Int MAX)
    {
        MIN = position - new Vector2Int(hSize.x, hSize.y);
        MAX = position + new Vector2Int(hSize.x - 1, hSize.y - 1);
    }

    public Vector2[] GetPositionLimit()
    {
        Vector2[] v = new Vector2[2];
        v[0] = position - new Vector2Int(hSize.x, hSize.y);
        v[1] = position + new Vector2Int(hSize.x, hSize.y);
        return v;
    }



}


