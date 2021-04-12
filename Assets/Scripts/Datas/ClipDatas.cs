using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Clips", menuName = "Data/Clips")]
public class ClipDatas : ScriptableObject
{
    public Type type = Type.MapNormal;
    public AudioClip[] clips;


    public enum Type
    {
        MapNormal,
        RoundNormal,
    }
    public AudioClip GetClip()
    {
        if (clips == null || clips.Length == 0)
        {
            return null;
        }
        return clips[Random.Range(0, clips.Length)];
    }

    public ClipDatas Clone()
    {
        ClipDatas a = Instantiate(this);
        return a;
    }
}
