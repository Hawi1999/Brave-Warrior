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
    float a = 10f;
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
            if (comingPlayer)
            {
                a += Time.deltaTime * 5;
                iTween.MoveUpdate(gameObject, iTween.Hash("position", player.GetPosition(), "time", 0.5f));
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
