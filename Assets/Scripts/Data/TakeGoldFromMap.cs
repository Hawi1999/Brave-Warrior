using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TakeGoldFromMap : MonoBehaviour
{
    SpriteRenderer render => GetComponent<SpriteRenderer>();
    BoxCollider2D col => GetComponent<BoxCollider2D>();

    PlayerController player => PlayerController.Instance;

    bool hasPlayer;

    Vector3 positionTarget;

    private Vector3 VelocityCurrent = new Vector3(0,0,0);
    public int giatoc;
    int maxVelocity = 6;
    bool readymovetoposion = false;
    bool readymovetoplayer = false;
    private void Start()
    {
        render.sortingLayerName = "Skin";
        render.sortingOrder = 10;
        col.size = new Vector2(4.1f, 4.1f);
        col.isTrigger = true;
    }

    public void MoveToPosion(Vector3 position)
    {
        readymovetoposion = true;
        positionTarget = position;
    }

    public void MoveToPlayer(PlayerController player)
    {
        readymovetoplayer = true;
        player = player;
    }

    private void Update()
    {
        if (readymovetoposion)
        {
            transform.position = MoveSmooth(transform.position, positionTarget);
            if (isNearPosition(positionTarget, 0.1f))
            {
                VelocityCurrent = Vector3.zero;
                readymovetoposion = false;
            }
        } 
        else
        if (readymovetoplayer)
        {
            transform.position = MoveSmooth(transform.position, player.getPosition());
            if (isNearPlayer())
            {
                // Thêm vàng ở đây
                Destroy(gameObject);
            }
        }
    }

    private bool isNearPlayer()
    {
        if (player == null)
        {
            return false;
        } else
        {
            return Vector2.Distance(transform.position, player.getPosition()) < 0.2f;
        }
    }

    private bool isNearPosition(Vector3 position, float distanceMin)
    {
        return (Vector2.Distance(transform.position, position) < distanceMin);
    }

    private Vector3 MoveSmooth(Vector3 start, Vector3 end)
    {
        Vector3 dir = (end - start).normalized;
        VelocityCurrent = VelocityCurrent + (dir + new Vector3(giatoc, giatoc, 0)) * Time.deltaTime;
        VelocityCurrent.x = Mathf.Min(VelocityCurrent.x, maxVelocity);
        VelocityCurrent.y = Mathf.Min(VelocityCurrent.y, maxVelocity);
        return start + VelocityCurrent * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            if (other.gameObject == player.gameObject && !hasPlayer)
            {
                MoveToPlayer(PlayerController.Instance);
                hasPlayer = true;
            }
        }
    }


}
