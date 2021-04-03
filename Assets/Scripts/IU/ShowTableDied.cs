using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowTableDied : MonoBehaviour
{
    public Image MainImage;
    public Image BackImage;

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

    public void OnBackToFarm()
    {
        GameController.Instance.LoadScene("TrangTrai");
        Destroy(this.gameObject);
    }

    public void OnAds()
    {
        // Hien quang cao khi co mang
        // Khong co mang thi LoadScene ve luon chu lam gi nua ha
        EntityManager.Instance.RevivePlayer(PlayerController.PlayerCurrent);
        Debug.Log(this.gameObject.name);
        Destroy(this.gameObject);
    }
}
