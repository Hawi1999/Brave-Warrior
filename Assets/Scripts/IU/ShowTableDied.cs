using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Advertisements;

public class ShowTableDied : MonoBehaviour, IUnityAdsListener
{
    public Image MainImage;
    public Image BackImage;

    bool readyAds = false;
    private void Start()
    {
        MainImage.transform.DOLocalMove(Vector2.zero, 1f);
        if (MainImage != null)
        {
            MainImage.sprite = Languages.getSprite("ContinueAfterDie");
        }
        if (BackImage != null)
        {
            BackImage.sprite = Languages.getSprite("BackToFarm");
        }
    }

    private void Awake()
    {
        Advertisement.AddListener(this);
    }

    private void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

    public void OnBackToFarm()
    {
        GameController.Instance.LoadScene("TrangTrai");
        Destroy(this.gameObject);
    }

    public void OnAds()
    {
        if (AdsManager.TryToAds(AdsManager.AdsReward))
        {

        }
        // Hien quang cao khi co mang
        // Khong co mang thi LoadScene ve luon chu lam gi nua ha
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == AdsManager.AdsReward)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    OnBackToFarm();
                    break;
                case ShowResult.Finished:
                    EntityManager.Instance.RevivePlayer(PlayerController.PlayerCurrent);
                    Destroy(this.gameObject);
                    break;
                case ShowResult.Skipped:
                    OnBackToFarm();
                    break;


            }
        }
    }
}
