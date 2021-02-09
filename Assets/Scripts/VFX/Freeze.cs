using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : ElementalBuffBad, ILockMove
{
    float Time_remaining = 0f;

    Entity target;

    Sprite[] sprites => VFXManager.Instance.SpritesIce;
    SpriteRenderer sprite;
    SpriteRenderer spritePrefab = VFXManager.Instance.IcePrefab;
    public override void StartUp(Entity entity, float time)
    {
        Time_remaining = time;
        target = entity;
        target.OnBuffsChanged?.Invoke(DamageElement.Ice, true);
        target.OnCheckForAttack += LockAttack;
        target.OnCheckForMove += LockMove;
        SpawnIce();
    }

    private void Update()
    {
        if (Time_remaining <= 0)
        {
            EndUp();
        } 
        sprite.transform.localScale = new Vector3(target.getSize().x, target.getSize().y);
        Time_remaining -= Time.deltaTime;
    }

    private void LockAttack(BoolAction a)
    {
        a.IsOK = false;
    }

    private void LockMove(BoolAction a)
    {
        a.IsOK = false;
    }

    private void SpawnIce()
    {
        if (sprites == null || sprites.Length == 0 || spritePrefab == null)
        {
            Debug.Log("Không có Data VFX Ice");
            return;
        }
        if (sprite != null)
        {
            Destroy(sprite.gameObject);
        }
        Vector3 pos = target.getPosition();
        Vector3 scale = target.getSize();
        sprite = Instantiate(spritePrefab, target.transform);
        sprite.sprite = sprites[Random.Range(0, sprites.Length)];
        sprite.transform.position = pos;
        sprite.transform.localScale = scale;
    }

    public void AddTime(float time)
    {
        if (time > this.Time_remaining)
        {
            Time_remaining = time;
        }
    }

    public static void Freezed(Entity entity, float time)
    {
        Freeze fre = entity.GetComponent<Freeze>();
        if (!fre)
        {
            fre = entity.gameObject.AddComponent<Freeze>();
            fre.StartUp(entity, time);
        } else
        {
            fre.AddTime(time);
        }
    }

    private void OnDestroy()
    {
        target.OnBuffsChanged?.Invoke(DamageElement.Ice, false);
        if (sprite)
        {
            Destroy(sprite.gameObject);
        }
        target.OnCheckForAttack -= LockAttack;
        target.OnCheckForMove -= LockMove;
    }

    public void EndLockMove()
    {
        EndUp();
    }
}
