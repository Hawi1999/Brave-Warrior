using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, IUpdateLanguage
{
    [SerializeField] GameObject ShowInfo;
    [SerializeField] Text text_continue;
    [SerializeField] Text text_giveup;
    private void Start()
    {
        if (ShowInfo != null)
        {
            ShowInfo.SetActive(false);
        }
        OnUpdateLanguage();
    }
    public void ShowPause()
    {
        if (ShowInfo != null)
        {
            Time.timeScale = 0;
            ShowInfo.SetActive(true);
        }
    }

    public void BackToHome()
    {
        Notification.AreYouSure(Languages.getString("BanCoChacMuonTuBo"), () => 
        {   
            MAPController.Instance.LoadScene("TrangTrai");
            Time.timeScale = 1;
        });
    }

    public void HidePause()
    {
        if (ShowInfo != null)
        {
            Time.timeScale = 1;
            ShowInfo.SetActive(false);
        }
    }

    public void OnUpdateLanguage()
    {
        if (text_continue != null)
            text_continue.text = Languages.getString("TiepTuc");
        if (text_giveup != null)
            text_giveup.text = Languages.getString("TuBo");
    }
}
