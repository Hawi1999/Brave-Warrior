using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    static AudioSource AudioBGR;
    public static void PLayOneShot(AudioClip clip, float volume = 1)
    {
        if (clip == null)
        {
            Debug.Log("Clip = null thì hát kiểu gì?");
            return;
        }
        AudioSource audio = new GameObject("Audio").AddComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
        float time = clip.length;
        GameObject.Destroy(audio.gameObject, time);
    }

    public static void PLayOneShot(string code, float volume = 1)
    {
        AudioClip clip = DataMap.GetClip(code);
        if (clip == null)
        {
            Debug.Log("Clip = null thì hát kiểu gì?");
            return;
        }
        AudioSource audio = new GameObject("Audio").AddComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
        float time = clip.length;
        GameObject.Destroy(audio.gameObject, time);
    }

    public static void PlayBackGround(AudioClip clip, float volume = 1)
    {
        if (clip == null)
        {
            Debug.Log("Clip = null thì hát kiểu gì?");
            return;
        }
        if (AudioBGR == null)
        {
            AudioBGR = new GameObject("AudioBackGround").AddComponent<AudioSource>();
            AudioBGR.playOnAwake = false;
            AudioBGR.loop = true;
        }
        AudioBGR.clip = clip;
        AudioBGR.volume = 0;
        AudioBGR.Play();
    }

    public static void RemoveBackGround()
    {
        if (AudioBGR == null)
        {
            return;
        }
        AudioBGR.clip = null;
        GameObject.Destroy(AudioBGR.gameObject);
    }

    public static IEnumerator ChangeValueBackGround(float to, float time)
    {
        if (AudioBGR == null && AudioBGR.clip == null)
        {
            yield break;
        }
        float cr = AudioBGR.volume;
        float t = 0;

        yield return null;
        while(t < time)
        {
            t += Time.deltaTime;
            AudioBGR.volume = cr + (to - cr) * t / time;
            yield return null;
        }
        AudioBGR.volume = to;
    }

    public static bool isBackGroundPLaying => (AudioBGR != null && AudioBGR.isPlaying);
}
