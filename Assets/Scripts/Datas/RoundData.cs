using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "R1", menuName = "Data/RoundData")]
public class RoundData : ScriptableObject
{
    public TypeRound typeRound;
    public Vector2Int position;
    public GroupEnemy groupEnemy = GroupEnemy.One;
    public Waves Waves;
    public TypeChest typeChest;
    public ColorChest colorChest;
    public int AmountBoss = 1;
    public WaveHail[] WavesHail;
    public int Level;

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
    public Vector2Int Size => hSize * 2;
    private int[] level;
    private Vector2Int hSize;

    #region Fuction
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
    // Lấy vị trí gần nhất ngoài round (bên ngoài Wall)
    public Vector2Int GetPositionOutSide(Direct t)
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
            case Direct.LeftUp:
                return position + new Vector2Int(-hSize.x - 1, hSize.y + 1);
            case Direct.RightUp:
                return position + new Vector2Int(hSize.x + 1, hSize.y + 1);
            case Direct.LeftDown:
                return position + new Vector2Int(-hSize.x - 1, -hSize.y - 1);
            case Direct.RightDown:
                return position + new Vector2Int(hSize.x + 1, -hSize.y - 1);
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
    /// <summary>
    /// Lấy vị trí xa nhất trong ruond theo hướng chỉ định
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public Vector2 GetPositionInSide(Direct a)
    {
        switch (a)
        {
            case Direct.LeftUp:
                return position + new Vector2(-hSize.x, hSize.y);
            case Direct.RightUp:
                return position + new Vector2(hSize.x, hSize.y);
            case Direct.LeftDown:
                return position + new Vector2(-hSize.x, -hSize.y);
            case Direct.RightDown:
                return position + new Vector2(hSize.x, -hSize.y);
            case Direct.Left:
                return position - new Vector2Int(hSize.x, 0);
            case Direct.Right:
                return position + new Vector2Int(hSize.x, 0);
            case Direct.Up:
                return position + new Vector2Int(0, hSize.y);
            case Direct.Down:
                return position - new Vector2Int(0, hSize.y);
            default:
                return position;
        }
    }

    /// <summary>
    /// Lấy vị trí xa nhất theo hướng chỉ định với trung tâm của Cell xa nhất
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public Vector2 GetMidPositionInCellLimit(Direct a)
    {
        switch (a)
        {
            case Direct.LeftUp:
                return position + new Vector2(-hSize.x, hSize.y) + new Vector2(0.5f, -0.5f);
            case Direct.RightUp:
                return position + new Vector2(hSize.x, hSize.y) + new Vector2(-0.5f, -0.5f);
            case Direct.LeftDown:
                return position + new Vector2(-hSize.x, -hSize.y) + new Vector2(0.5f, 0.5f);
            case Direct.RightDown:
                return position + new Vector2(hSize.x, -hSize.y) + new Vector2(-0.5f, 0.5f);
            case Direct.Left:
                return position - new Vector2Int(hSize.x, 0) + Vector2.right * 0.5f;
            case Direct.Right:
                return position + new Vector2Int(hSize.x, 0) + Vector2.left * 0.5f;
            case Direct.Up:
                return position + new Vector2Int(0, hSize.y) + Vector2.down * 0.5f;
            case Direct.Down:
                return position - new Vector2Int(0, hSize.y) + Vector2.up * 0.5f;
            default:
                return position;
        }
    }
    #endregion 

    [CustomEditor(typeof(RoundData))]
    [CanEditMultipleObjects]
    public class RoundDataEditor : Editor
    {
        SerializedProperty typeRound;
        SerializedProperty groupEnemy;
        SerializedProperty waves;
        SerializedProperty typeChest;
        SerializedProperty colorChest;
        SerializedProperty amountBoss;
        SerializedProperty wavesHaill;
        SerializedProperty level;
        private void OnEnable()
        {
            typeRound = serializedObject.FindProperty("typeRound");
            groupEnemy = serializedObject.FindProperty("groupEnemy");
            waves = serializedObject.FindProperty("Waves");
            typeChest = serializedObject.FindProperty("typeChest");
            colorChest = serializedObject.FindProperty("colorChest");
            amountBoss = serializedObject.FindProperty("AmountBoss");
            wavesHaill = serializedObject.FindProperty("WavesHail");
            level = serializedObject.FindProperty("Level");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            RoundData r = (RoundData)target;
            EditorGUILayout.PropertyField(typeRound, new GUIContent("Loại Round"));
            switch (r.typeRound)
            {
                case TypeRound.Boss:
                    EditorGUILayout.IntSlider(amountBoss, 1, 4, new GUIContent("Số lượng Boss"));
                    break;
                case TypeRound.Enemy:
                    EditorGUILayout.PropertyField(groupEnemy, new GUIContent("Nhóm Enemy"));
                    EditorGUILayout.PropertyField(waves, new GUIContent("Các màn chơi"));
                    EditorGUILayout.PropertyField(typeChest, new GUIContent("Loại rương"));
                    EditorGUILayout.PropertyField(colorChest, new GUIContent("Màu rương"));
                    break;
                case TypeRound.Hail:
                    EditorGUILayout.PropertyField(wavesHaill, new GUIContent("Waves Haill"));
                    EditorGUILayout.PropertyField(level, new GUIContent("Level Buff"));
                    break;
                case TypeRound.Chest:
                    EditorGUILayout.PropertyField(typeChest, new GUIContent("Loại rương"));
                    EditorGUILayout.PropertyField(colorChest, new GUIContent("Màu rương"));
                    break;

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}


