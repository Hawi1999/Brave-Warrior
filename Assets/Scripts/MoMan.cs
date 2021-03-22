using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoMan : MonoBehaviour
{
    public Slider Down;
    public Slider Up;
    public float TimeIntoOpen;
    private float time = 0;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Down.value = Mathf.Clamp01(1 - time / TimeIntoOpen);
        Up.value = Mathf.Clamp01(1 - time / TimeIntoOpen);
        if (time >= TimeIntoOpen)
        {
            MAP_GamePlay.StartGame();
            gameObject.SetActive(false);

        }
    }
    private void OnDisable()
    {
        time = 0;
        Up.value = 1;
        Down.value = 1;
    }
}
