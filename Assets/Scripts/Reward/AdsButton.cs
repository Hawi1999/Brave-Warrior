using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsButton : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] int _AddDOLA = 50;
    [SerializeField] Text text_dola;
    private bool waiting = false;

    
    private void Awake()
    {
        Advertisement.AddListener(this);
        if (Advertisement.IsReady(AdsManager.AdsReward))
        {
            OnUnityAdsReady(AdsManager.AdsReward);
        } else
        {
            gameObject.SetActive(false);
        }
        if (text_dola != null)
        {
            text_dola.text = "+" + _AddDOLA;
        }
    }

    public void ClickAds()
    {
        if (AdsManager.TryToAds(AdsManager.AdsReward))
        {
            waiting = true;
        } else
        {
            Notification.ReMind(Languages.getString("CoLoiXayRaVoiHanhDongNay"));
            gameObject.SetActive(false);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (waiting)
        {
            if (placementId == AdsManager.AdsReward)
            {
                Personal.AddDOLA(_AddDOLA);
            }
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        if (placementId == AdsManager.AdsReward)
            gameObject.SetActive(false);
    }
    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == AdsManager.AdsReward)
            gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
