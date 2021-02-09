using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TakeGoldFromMap : MonoBehaviour
{
    private enum goldStatus
    {
        MovingToPosition,
        MovingToPlayer,
        Delay,
        Waiting
    }
    SpriteRenderer render => GetComponent<SpriteRenderer>();
    BoxCollider2D col => GetComponent<BoxCollider2D>();

    PlayerController player => PlayerController.PlayerCurrent;

    goldStatus Status;
    //General
    int maxVelocity = 8;

    // MovingToPostion
    Vector3 positionTarget;

    // MovingToPlayer
    bool hadPlayer;

    // Waiting

    // Delay
    bool hasStatusNext;
    goldStatus statucNext;
    float beginDelay;
    float timeDelay= 0.3f;
    private void Start()
    {
        render.sortingLayerName = "Skin";
        render.sortingOrder = 10;
        col.isTrigger = true;
    }

    public void MoveToPosion(Vector3 position)
    {
        if (Status == goldStatus.Delay)
        {
            statucNext = goldStatus.Delay;
            positionTarget = position;
            hasStatusNext = true;
        } else
        {
            Status = goldStatus.MovingToPosition;
            positionTarget = position;
        }
    }

    private void Update()
    {
        switch (Status)
        {
            case goldStatus.MovingToPosition:
                transform.position = Move(transform.position, positionTarget);
                if (isNearPosition(positionTarget, 0.2f))
                {
                    Status = goldStatus.Delay;
                    beginDelay = Time.time;
                }
                break;
            case goldStatus.MovingToPlayer:
                if (player != null)
                {
                    transform.position = Move(transform.position, player.getPosition());
                    if (isNearPlayer(0.2f))
                    {
                        Debug.Log("ận nhan duoc 1 cmn vang");
                        Destroy(gameObject);
                    }
                }
                break;
            case goldStatus.Waiting:
                CheckPlayerNear();
                break;
            case goldStatus.Delay:
                if (Time.time - beginDelay > timeDelay)
                {
                    if (hasStatusNext)
                    {
                        Status = statucNext;
                        hasStatusNext = false;
                    } else
                    {
                        Status = goldStatus.Waiting;
                    }
                }
                break;

        }
    }

    private void CheckPlayerNear()
    {
        
        if (player != null && !hadPlayer)
        {
            if (isNearPlayer(3f))
            {
                Status = goldStatus.MovingToPlayer;
                hadPlayer = true;
            }
        }
    }

    private bool isNearPlayer(float Distance)
    {
        if (player == null)
        {
            return false;
        } else
        {
            return Vector2.Distance(transform.position, player.getPosition()) < Distance;
        }
    }

    private bool isNearPosition(Vector3 position, float distanceMin)
    {
        return (Vector2.Distance(transform.position, position) < distanceMin);
    }

    private Vector3 Move(Vector3 start, Vector3 end)
    {
        return start + (end - start).normalized * Time.deltaTime * maxVelocity;
    }

}
