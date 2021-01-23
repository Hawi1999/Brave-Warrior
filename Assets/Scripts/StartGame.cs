using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartGame : MonoBehaviour
{
    public GameObject Loading;
    public Slider sli;

    private void Start()
    {
        StartCoroutine(LoadAsynchronously(SceneGame.Loading));
    }

    IEnumerator LoadAsynchronously(SceneGame scene)
    {
        Loading.SetActive(true);
        sli.value = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());
        while (!operation.isDone)
        {
            sli.value = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }
}
