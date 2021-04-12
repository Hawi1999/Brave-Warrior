using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TakeGoldFromMap : MonoBehaviour
{
    SpriteRenderer render => GetComponent<SpriteRenderer>();
    BoxCollider2D col => GetComponent<BoxCollider2D>();

    PlayerController player => PlayerController.PlayerCurrent;

    // MovingToPlayer
    bool waitPlayer;
    bool comingPlayer;
    float speed = 10f;
    private void Start()
    {
        render.sortingLayerName = "Effect";
        render.sortingOrder = 10;
        col.isTrigger = true;
    }

    private void Update()
    {
        if (waitPlayer)
        {
            if (comingPlayer == false && isNearPlayer(3f))
            {
                comingPlayer = true;
            }
            if (comingPlayer && player.IsALive)
            {
                transform.position = transform.position + ((Vector3)player.center - transform.position).normalized * speed * Time.deltaTime;
                onComingPlayer();
            }
        }
    }

    public void MoveToPosion(Vector3 position)
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", position, "time", 0.5f, "oncomplete", "WaitPlayer"));
    }

    private void WaitPlayer()
    {
        StartCoroutine(WaitToWaitPlayer(0.2f));
    }

    IEnumerator WaitToWaitPlayer(float t)
    {
        yield return new WaitForSeconds(t);
        waitPlayer = true;
    }
    private void onComingPlayer()
    {
        if (isNearPlayer(0.3f))
        {
            Personal.AddCoin(1);
            SoundManager.PLayOneShot("Collect Coin", 0.1f);
            Destroy(gameObject);
        }
    }

    private bool isNearPlayer(float Distance)
    {
        if (player == null)
        {
            return false;
        } else
        {
            return Vector2.Distance(transform.position, player.GetPosition()) < Distance;
        }
    }

}
