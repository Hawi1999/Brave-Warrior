using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class EditorMain 
{
    [MenuItem("Tool/Update All ScriptableObject")]
    public static void UpdateItem(){
        ScriptableObject[] a = Resources.LoadAll<ScriptableObject>("");
        foreach (ScriptableObject b in a)
        {
            if (b is IUpdateItemEditor)
            {
                (b as IUpdateItemEditor).OnUpdate();
            }
        }
        foreach (ScriptableObject b in a)
        {
            if (b is IUpdateItemEditor)
            {
                (b as IUpdateItemEditor).OnUpdate();
            }
        }
    }
}
