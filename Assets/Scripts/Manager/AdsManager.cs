using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public static string Adsing;
    public static string ID = "4083973";
    public const string AdsReward = "rewardedVideo";
    public const string AdsSkip = "video";

    void Awake()
    {
        Advertisement.AddListener(this);
    }


    private void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
    public static void Initialize()
    {
        if (!Advertisement.isInitialized)
            Advertisement.Initialize(ID, true);
    }

    public static bool TryToAds(string type)
    {
        if (!Advertisement.IsReady(type))
        {
            return false;
        }
        Adsing = type;
        Advertisement.Show(type);
        return true;
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("UniyAds Sẵn sàng: " + placementId);
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("UniyAds lỗi");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("UniyAds Bắt đầu: " + placementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {

    }
}
