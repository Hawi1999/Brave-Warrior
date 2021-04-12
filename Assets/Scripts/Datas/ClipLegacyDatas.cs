using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clips", menuName = "Data/Clip Legacy")]
public class ClipLegacyDatas : ScriptableObject
{
    [SerializeField] string Code = "Collect ?";
    [SerializeField] AudioClip clip;

    public bool EqualAndReady(string code)
    {
        return (clip != null && Code == code);
    }

    public AudioClip GetClip()
    {
        return clip;
    }
}
