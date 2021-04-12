using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
