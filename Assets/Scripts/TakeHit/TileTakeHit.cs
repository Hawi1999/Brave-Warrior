using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileTakeHit : MonoBehaviour, ITakeHit
{
    Tilemap tilemap => TileManager.TileCurrent;
    bool removed = false;

    protected void RemoveTile()
    {
        StartCoroutine(WaitToRemoveTile());
    }

    IEnumerator WaitToRemoveTile()
    {
        yield return new WaitForEndOfFrame();
        if (tilemap != null && !removed)
        {
            removed = true;
            tilemap.SetTile(tilemap.WorldToCell(transform.position), null);
        }
    }
    public virtual Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }

    public abstract void TakeDamaged(DamageData data);
}
